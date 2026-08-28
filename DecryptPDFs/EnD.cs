using DecryptPDFs.Properties;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.Security;

namespace DecryptPDFs
{
    public partial class EnD : Form
    {
        // DATAFILE LOCATION - %LocalAppData%\DecryptPDFs\DecryptPDFs.db  see PasswordManagerContext.cs OnConfiguring()
        static readonly string LOGFILEPATH = "DecryptPDFs.log"; // Specify the log file path
        static readonly string STATE_ERROR = "ERROR";
        static readonly string STATE_NOSECURITY = "No Security";
        static readonly string STATE_HASSECURITYSETTINGS = "Has Security Settings";
        static readonly string STATE_PASSWORDPROTECTED = "Password Protected";
        static readonly string STATE_DECRYPTION_SUCCESS = "Unrestricted ";

        // A password to try, plus its DB row id when known (so a hit can be recorded via the fast,
        // id-based path instead of a string lookup - see PasswordManagerContext.RecordPasswordSuccess)
        private readonly record struct PasswordCandidate(string Password, int? Id);

        static string[]? commandlineArgs;
        private readonly PasswordManagerContext dbContext;
        private bool hasAutoDecryptedOnRecurseToggle = false;
        private CancellationTokenSource? operationCts;
        public EnD(string[] args)
        {
            InitializeComponent();

            dbContext = new PasswordManagerContext();
            dbContext.Database.Migrate();

            // Overwrite the log file with an empty file
            File.WriteAllText(LOGFILEPATH, string.Empty);

            // Restore form size and position
            this.StartPosition = FormStartPosition.Manual;
            this.Width = Settings.Default.FormWidth;
            this.Height = Settings.Default.FormHeight;
            this.Left = Settings.Default.FormLeft;
            this.Top = Settings.Default.FormTop;
            // Restore window state
            if (Enum.TryParse(Properties.Settings.Default.FormWindowState, out FormWindowState windowState))
            {
                this.WindowState = windowState;
            }

            // Ensure the form is within the screen bounds
            /*var screen = Screen.FromControl(this);
            if (this.Left < screen.Bounds.Left || this.Right > screen.Bounds.Right ||
                this.Top < screen.Bounds.Top || this.Bottom > screen.Bounds.Bottom)
            {
                this.Left = screen.Bounds.Left + 100;
                this.Top = screen.Bounds.Top + 100;
            }*/


            //Initialise form controls
            checkBoxOverwrite.Checked = Settings.Default.Overwrite;
            groupBoxOverwrite.Visible = Settings.Default.Overwrite ? false : true;
            textBoxOverwriteString.Text = Settings.Default.OverwritePrefixSuffixName;
            radioButtonPrefix.Checked = Settings.Default.OverwritePrefixSuffix == 'P' ? true : false;
            radioButtonSuffix.Checked = Settings.Default.OverwritePrefixSuffix == 'S' ? true : false;
            textBoxOverwriteString.PlaceholderText = "Enter " + (Settings.Default.OverwritePrefixSuffix == 'P' ? "Prefix" : "Suffix");
            //checkBoxRecurseDir.Checked = Settings.Default.RecurseDirectory;
            textBoxPassword.Text = "Enter Password";
            textBoxPassword.Focus();
            textBoxPassword.SelectAll();


            // Set the linkLabelFolderName text to the directory path of the first item, if applicable
            if (args.Length > 0 && args[0].Contains('\\'))
            {
                linkLabelFolderName.Text = args[0].Substring(0, args[0].LastIndexOf('\\'));
                commandlineArgs = args;
            }
        }
        private void EnD_Shown(object sender, EventArgs e)
        {
            RunScanAndAutoDecrypt(AutoDecryptWithStoredPasswords);
        }

        // Shared by every entry point that kicks off a cancellable, 'S'-to-stop run: creates the
        // token, disables the controls that would otherwise re-enter it, runs action, and always
        // cleans up afterward. Runs on the UI thread - the scan/decrypt loops touch WinForms
        // controls throughout, not just at isolated points, so this stays synchronous and relies
        // on Application.DoEvents() (already in those loops) to keep the form responsive and the
        // 'S' key working. Errors are caught here rather than left to bubble up as an unhandled
        // exception.
        private void RunCancellableOperation(Action<CancellationToken> action)
        {
            var token = BeginOperation();
            try
            {
                action(token);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                EndOperation();
            }
        }

        // Shared by the initial load and the recurse-toggle rescan: scans, then runs the given
        // follow-up (auto-decrypt) with the same cancellation token.
        private void RunScanAndAutoDecrypt(Action<CancellationToken> afterScan)
        {
            if (commandlineArgs == null)
            {
                return;
            }

            RunCancellableOperation(token =>
            {
                PopulateFilesFoldersList(commandlineArgs, token);
                afterScan(token);
            });
        }

        // Marks the start of a cancellable, 'S'-to-stop operation: creates the token the KeyDown
        // handler will cancel, and disables controls whose events would otherwise re-enter the
        // running loop via the Application.DoEvents() pumping inside it (e.g. toggling "Recurse
        // Directory" mid-scan would fire PopulateFilesFoldersList again on top of itself).
        private CancellationToken BeginOperation()
        {
            operationCts = new CancellationTokenSource();
            buttonDecrypt.Enabled = false;
            checkBoxRecurseDir.Enabled = false;
            return operationCts.Token;
        }

        private void EndOperation()
        {
            buttonDecrypt.Enabled = true;
            checkBoxRecurseDir.Enabled = true;
            operationCts?.Dispose();
            operationCts = null;
        }

        private void SetStatus(string text)
        {
            statusLabel.Text = text;
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void EnD_KeyDown(object sender, KeyEventArgs e)
        {
            // 'S' to stop, not Escape - Escape already doubles as this form's CancelButton elsewhere
            if (e.KeyCode == Keys.S && operationCts is { IsCancellationRequested: false })
            {
                operationCts.Cancel();
                e.Handled = true;
                SetStatus("Stopping...");
            }
        }

        private void EnD_FormClosing(object sender, FormClosingEventArgs e)
        {
            operationCts?.Cancel();

            // Save Form window state
            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.FormWidth = this.Width;
                Settings.Default.FormHeight = this.Height;
                Settings.Default.FormLeft = this.Left;
                Settings.Default.FormTop = this.Top;
            }
            else
            {
                Settings.Default.FormWidth = this.RestoreBounds.Width;
                Settings.Default.FormHeight = this.RestoreBounds.Height;
                Settings.Default.FormLeft = this.RestoreBounds.Left;
                Settings.Default.FormTop = this.RestoreBounds.Top;
            }

            Settings.Default.FormWindowState = this.WindowState.ToString();
            // Save the current settings
            Settings.Default.Overwrite = checkBoxOverwrite.Checked;
            Settings.Default.OverwritePrefixSuffix = radioButtonPrefix.Checked ? 'P' : 'S';
            Settings.Default.OverwritePrefixSuffixName = textBoxOverwriteString.Text;
            //Settings.Default.RecurseDirectory = checkBoxRecurseDir.Checked;
            Settings.Default.Save();

            dbContext.Dispose();
        }

        private static void LogMessage(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}"; // Create a formatted log entry

            File.AppendAllText(LOGFILEPATH, logEntry + Environment.NewLine); // Append the log entry to the file

            // Append the log entry to the file
            using (StreamWriter writer = new(LOGFILEPATH, true))
            {
                writer.WriteLine(logEntry);
            }
        }

        private static string GetSecuritySettings(PdfSecuritySettings securitySettings)
        {
            return $"Encryption Level: {securitySettings.DocumentSecurityLevel} | " +
                   $"Print Document: {(securitySettings.PermitPrint ? "Allowed" : "Not Allowed")} | " +
                   $"Quality Print: {(securitySettings.PermitFullQualityPrint ? "Allowed" : "Not Allowed")} | " +
                   $"Assemble Doc: {(securitySettings.PermitAssembleDocument ? "Allowed" : "Not Allowed")} | " +
                   $"Copy Content: {(securitySettings.PermitExtractContent ? "Allowed" : "Not Allowed")} | " +
                   $"Accessibility Copy: {(securitySettings.PermitAccessibilityExtractContent ? "Allowed" : "Not Allowed")} | " +
                   $"Modify Document: {(securitySettings.PermitModifyDocument ? "Allowed" : "Not Allowed")} | " +
                   $"Add Annotations: {(securitySettings.PermitAnnotations ? "Allowed" : "Not Allowed")} | " +
                   $"Fill Forms: {(securitySettings.PermitFormsFill ? "Allowed" : "Not Allowed")}";
        }

        private static (int code, string State, string Comment) GetFileState(string directoryName, string fileName)
        {
            try
            {
                var isEncrypted = true;

                // Opening the document with invalid password to check if it is encrypted at all; if it opens it is not encrypted

                var pdfDocTest1 = PdfReader.Open($"{directoryName}\\{fileName}", "") as PdfDocument;
                isEncrypted = false;

                if (isEncrypted)  // Encrypted
                {
                    return (2, STATE_PASSWORDPROTECTED, "The document is password protected.");
                }
                else // else check for security settings for pdf without password
                {
                    // Attempt to open the PDF document
                    var pdfDocTest = PdfReader.Open($"{directoryName}\\{fileName}", PdfDocumentOpenMode.ReadOnly);

                    // Check if the document is encrypted
                    if (pdfDocTest.SecuritySettings.DocumentSecurityLevel == PdfDocumentSecurityLevel.None)
                    {
                        // PDF is not password protected
                        return (0, STATE_NOSECURITY, "The document is not encrypted.");
                    }
                    else
                    {
                        // PDF is encrypted, extract security settings
                        var securitySettings = pdfDocTest.SecuritySettings;
                        string settings = GetSecuritySettings(securitySettings);

                        return (1, STATE_HASSECURITYSETTINGS, settings);
                    }
                }
            }
            catch (PdfReaderException ex)
            {
                if (ex.Message.Contains("password is invalid"))
                {
                    // Incorrect password, or the document is password-protected and cannot be opened without it
                    return (2, STATE_PASSWORDPROTECTED, "The document is password protected and cannot be opened.");
                }
                else if (ex.Message.Contains("owner password"))
                {
                    // Attempt to open the PDF document
                    var pdfDocTest = PdfReader.Open($"{directoryName}\\{fileName}", PdfDocumentOpenMode.ReadOnly);

                    // Check if the document is encrypted
                    {
                        // PDF is encrypted, extract security settings
                        var securitySettings = pdfDocTest.SecuritySettings;
                        string settings = GetSecuritySettings(securitySettings);

                        return (1, STATE_HASSECURITYSETTINGS, settings);
                    }
                }
                else
                {
                    // Other PDF-related errors
                    var cleanedMessage = ex.Message.Replace("If you think this is a bug in PDFsharp, please send us your PDF file.", "").TrimEnd();
                    return (-2, STATE_ERROR, "PDF: " + cleanedMessage);
                }
            }
            catch (Exception ex)
            {
                // General error handling
                var cleanedMessage = ex.Message.Replace("If you think this is a bug in PDFsharp, please send us your PDF file.", "").TrimEnd();
                return (-3, STATE_ERROR, cleanedMessage);
            }
        }
        private void AddItemtoFileList(string index, string fileName)
        {
            var colour = Color.DimGray; // No security

            var (code, state, comment) = GetFileState(linkLabelFolderName.Text, fileName);

            if (code < 0)
            {
                colour = Color.RosyBrown; // Error
                labelCountError.Text = (int.Parse(labelCountError.Text) + 1).ToString();
            }
            else if (code == 2)
            {
                colour = Color.LightGreen; // Password Protected
                labelCountPasswordProtected.Text = (int.Parse(labelCountPasswordProtected.Text) + 1).ToString();
            }
            else if (code == 1)
            {
                colour = Color.LightSteelBlue; // Security Setting
                labelCountHasSecuritySettings.Text = (int.Parse(labelCountHasSecuritySettings.Text) + 1).ToString();
            }
            else
            {
                labelCountNoSecurity.Text = (int.Parse(labelCountNoSecurity.Text) + 1).ToString();
            }

            // Create a new ListViewItem for each file
            var item = new ListViewItem(index)
            {
                SubItems = { fileName, state, comment },
                ForeColor = colour
            };

            // Add the item to the ListView
            listFilesFolders.Items.Add(item);
        }


        private void PopulateFilesFoldersList(string[] paths, CancellationToken token)
        {
            listFilesFolders.Items.Clear(); // Clear existing items before populating
            ResetRunColumns(); // Rows are being rebuilt from scratch, so any prior "RunN" columns are stale
            var i = 1;
            var rootPath = linkLabelFolderName.Text;
            labelCountError.Text = "0";
            labelCountNoSecurity.Text = "0";
            labelCountHasSecuritySettings.Text = "0";
            labelCountPasswordProtected.Text = "0";


            foreach (var path in paths)
            {
                if (token.IsCancellationRequested)
                {
                    SetStatus($"Scan stopped after {i - 1} file(s).");
                    return;
                }

                // Check if the path is a directory
                if (Directory.Exists(path))
                {
                    // Get all files in the directory
                    var searchOption = checkBoxRecurseDir.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                    //string[] files = Directory.GetFiles(path);
                    string[] files = Directory.GetFiles(path, "*.pdf", searchOption);
                    foreach (var file in files)
                    {
                        if (token.IsCancellationRequested)
                        {
                            SetStatus($"Scan stopped after {i - 1} file(s).");
                            return;
                        }

                        // Extract the file name and directory name
                        string fileName = Path.GetFileName(file);
                        // Remove the root path from the file path
                        string relativePath = file.Replace(rootPath, "").TrimStart('\\');
                        // Get the directory name from the relative path
                        string directoryName = Path.GetDirectoryName(relativePath) ?? string.Empty;

                        SetStatus($"Scanning file {i} of {files.Length}: {fileName}... (Press 'S' to STOP)");

                        // Create a new ListViewItem for each file
                        AddItemtoFileList(i.ToString(), $"{directoryName}\\{fileName}");
                        i++;

                        // Pump the message loop so status text and the 'S' key are processed per
                        // file (not just per top-level arg)
                        Application.DoEvents();
                    }
                }
                else if (File.Exists(path) && Path.GetExtension(path).Equals(".pdf", StringComparison.OrdinalIgnoreCase)) // Check if the path is a file and has a .pdf extension
                {
                    // Extract just the file name
                    string fileName = Path.GetFileName(path);

                    SetStatus($"Scanning {fileName}... (Press 'S' to STOP)");

                    // Create a new ListViewItem for the file
                    AddItemtoFileList(i.ToString(), fileName);
                    i++;
                }
                if (i % 20 == 0) // for lon list auto resize every 20 files
                {
                    // Adjust the column width to fit the content
                    listFilesFolders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listFilesFolders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
                Application.DoEvents();
            }

            // Adjust the column width to fit the content
            listFilesFolders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listFilesFolders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            columnHeaderComment.Width = 300;

            SetStatus(i == 1 ? "No PDF files found." : $"Scan complete - {i - 1} file(s) found.");
        }

        private void linkLabelFolderName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Directory.Exists(linkLabelFolderName.Text))
            {
                System.Diagnostics.Process.Start("explorer.exe", linkLabelFolderName.Text);
            }
        }
        private void radioButtonPrefix_CheckedChanged(object sender, EventArgs e)
        {
            textBoxOverwriteString.PlaceholderText = "Enter Prefix";
            textBoxOverwriteString.Text = "decrypted_";
        }

        private void radioButtonSuffix_CheckedChanged(object sender, EventArgs e)
        {
            textBoxOverwriteString.PlaceholderText = "Enter Suffix";
            textBoxOverwriteString.Text = "_decrypted";
        }

        private void checkBoxOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxOverwrite.Visible = !checkBoxOverwrite.Checked;
        }

        private void listFilesFolders_ItemActivate(object sender, EventArgs e)
        {
            if (listFilesFolders.SelectedItems.Count > 0)
            {
                var selectedItem = listFilesFolders.SelectedItems[0];
                var state = selectedItem.SubItems[2].Text;  // state
                var fileName = selectedItem.SubItems[1].Text;  // filename
                var comment = selectedItem.SubItems[3].Text; // comment
                var msg = "File: " + fileName + "\n\n" + comment;

                if (state == STATE_HASSECURITYSETTINGS)
                {
                    // Replace all occurrences of ":" with ":\t" and "," with "\n"
                    var securitySettings = comment.Replace(":", ":\t").Replace(" | ", "\n");

                    msg = "File: " + fileName + "\n\n" + securitySettings;

                }
                columnHeaderComment.Width = 300;
                // Iterate through run subitems (columns) and show their content
                for (int i = 4; i < selectedItem.SubItems.Count; i++)
                {
                    msg += $"\n\n Run{i - 3}: {selectedItem.SubItems[i].Text}";
                }


                MessageBox.Show(msg, "PDF Security Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string ConstructDecryptedFilePath(string filePath)
        {
            string newFilePath = filePath;

            if (!checkBoxOverwrite.Checked)
            {

                string directoryName = Path.GetDirectoryName(filePath) ?? string.Empty;

                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                string overwriteName = textBoxOverwriteString.Text;

                if (radioButtonPrefix.Checked)
                {
                    newFilePath = Path.Combine(directoryName, $"{overwriteName}{fileNameWithoutExtension}{extension}");
                }
                else
                {
                    newFilePath = Path.Combine(directoryName, $"{fileNameWithoutExtension}{overwriteName}{extension}");
                }
            }

            return newFilePath;
        }

        private (bool returnVal, string errormsg) DecryptPDFFile(string directoryName, string fileName, string password)
        {
            try
            {
                var filePath = Path.Combine(directoryName, fileName);

                // Open PDF using PdfSharpCore
                var pdfDocImport = PdfReader.Open(filePath, password, PdfDocumentOpenMode.Import);
                using (pdfDocImport)
                {
                    // Create a new document
                    using (var document = new PdfDocument())
                    {
                        // Import pages into the new document
                        foreach (var page in pdfDocImport.Pages)
                        {
                            document.AddPage(page);
                        }

                        // Remove security settings
                        document.SecuritySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.None;
                        // Additional permissions can be set as needed
                        /*document.SecuritySettings.PermitPrint = true;
                        document.SecuritySettings.PermitFullQualityPrint = true;
                        document.SecuritySettings.PermitAssembleDocument = true;
                        document.SecuritySettings.PermitExtractContent = true;
                        document.SecuritySettings.PermitAccessibilityExtractContent = true;
                        document.SecuritySettings.PermitModifyDocument = true;
                        document.SecuritySettings.PermitAnnotations = true;
                        document.SecuritySettings.PermitFormsFill = true;
                        document.SecuritySettings.PermitContentCopy = true;
                        document.SecuritySettings.PermitAll(); 
                        */
                        // Save Decrypted File
                        // Construct the new file path based on the controls' values
                        string newFilePath = ConstructDecryptedFilePath(filePath);

                        document.Save(newFilePath);
                    }
                }
                return (true, "Success");
            }
            catch (Exception ex)
            {
                LogMessage($"Error removing security controls: {fileName} - {ex.Message}");
                return (false, $"Error removing security controls: {fileName} - {ex.Message}");
            }
        }

        private List<ListViewItem> GetPasswordProtectedFileList()
        {
            return listFilesFolders.Items
                .Cast<ListViewItem>()
                .Where(item => item.SubItems[2].Text == STATE_PASSWORDPROTECTED)
                .ToList();
        }
        private List<ListViewItem> GetSecuritySettingFileList()
        {
            return listFilesFolders.Items
                .Cast<ListViewItem>()
                .Where(item => item.SubItems[2].Text == STATE_HASSECURITYSETTINGS)
                .ToList();
        }

        private void MarkItemDecrypted(ListViewItem listItem, char state)
        {
            listItem.SubItems[2].Text = STATE_DECRYPTION_SUCCESS; // Update "State" column
            listItem.SubItems[3].Text = "All Passwords and Security Settings removed!"; // Update "Comment" column
            listItem.SubItems.Add("Success"); // Update "Run" column
            listItem.ForeColor = Color.WhiteSmoke;

            if (state == 'P')
            {
                labelCountPasswordProtected.Text = (int.Parse(labelCountPasswordProtected.Text) - 1).ToString();
                labelCountNoSecurity.Text = (int.Parse(labelCountNoSecurity.Text) + 1).ToString();
            }
            else if (state == 'S')
            {
                labelCountHasSecuritySettings.Text = (int.Parse(labelCountHasSecuritySettings.Text) - 1).ToString();
                labelCountNoSecurity.Text = (int.Parse(labelCountNoSecurity.Text) + 1).ToString();
            }
        }

        // Password-protected files always need a password guess; "has security settings" files open
        // fine with a blank password, so they're combined into one work list only when the user has
        // opted in to stripping those settings too.
        private List<ListViewItem> GetDecryptWorkList()
        {
            var workListItems = GetPasswordProtectedFileList();

            if (checkBoxRemoveSecurity.Checked)
            {
                workListItems.AddRange(GetSecuritySettingFileList());
            }

            return workListItems;
        }

        static int RunIndex = 1;
        // Shared core used by both the manual "Decrypt" button and the stored-password auto-try:
        // tries passwordCandidates (in order) against every 'Password Protected' item, stopping at
        // the first hit; 'Has Security Settings' items don't need a guess, so they always try a
        // single blank password. buildNoMatchMessage formats the "Run" column text when nothing works.
        private void DecryptWorkList(List<ListViewItem> workListItems, List<PasswordCandidate> passwordCandidates, Func<string, string> buildNoMatchMessage, CancellationToken token)
        {
            if (workListItems.Count == 0)
            {
                return;
            }

            columnHeaderComment.Width = 300;

            var columnHeaderRun = new ColumnHeader
            {
                Text = $"Run{RunIndex}",
                Width = 100
            };
            listFilesFolders.Columns.Add(columnHeaderRun);

            var directoryName = linkLabelFolderName.Text;
            var blankCandidate = new List<PasswordCandidate> { new(string.Empty, null) };
            var total = workListItems.Count;
            var decryptedCount = 0;
            var unmatchedCount = 0;

            for (var fileIndex = 0; fileIndex < total; fileIndex++)
            {
                if (token.IsCancellationRequested)
                {
                    SetStatus($"Stopped after {fileIndex} of {total}.");
                    RunIndex++;
                    return;
                }

                var item = workListItems[fileIndex];
                var listItem = listFilesFolders.Items[int.Parse(item.SubItems[0].Text) - 1];
                var fileName = item.SubItems[1].Text;
                var state = item.SubItems[2].Text == STATE_PASSWORDPROTECTED ? 'P' : 'S';
                var candidates = state == 'P' ? passwordCandidates : blankCandidate;

                var matched = false;
                var errmsg = string.Empty;
                for (var candidateIndex = 0; candidateIndex < candidates.Count; candidateIndex++)
                {
                    if (token.IsCancellationRequested)
                    {
                        SetStatus($"Stopped after {fileIndex} of {total}.");
                        RunIndex++;
                        return;
                    }

                    var candidate = candidates[candidateIndex];

                    SetStatus(candidates.Count > 1
                        ? $"Decrypting file {fileIndex + 1} of {total}: {fileName} - trying password {candidateIndex + 1} of {candidates.Count}... (Press 'S' to STOP)"
                        : $"Decrypting file {fileIndex + 1} of {total}: {fileName}... (Press 'S' to STOP)");

                    bool success;
                    (success, errmsg) = DecryptPDFFile(directoryName, fileName, candidate.Password);
                    if (success)
                    {
                        MarkItemDecrypted(listItem, state);

                        if (state == 'P')
                        {
                            if (candidate.Id.HasValue)
                            {
                                dbContext.RecordPasswordSuccess(candidate.Id.Value);
                            }
                            else
                            {
                                dbContext.RecordPasswordSuccess(candidate.Password);
                            }
                        }

                        matched = true;
                        decryptedCount++;
                        break;
                    }

                    Application.DoEvents();
                }

                if (!matched)
                {
                    listItem.SubItems.Add(buildNoMatchMessage(errmsg)); // Update "Run" column
                    unmatchedCount++;
                }

                LogMessage($"Index: {item.SubItems[0].Text}, FileName: {fileName}, State: {listItem.SubItems[2].Text}, Comment: {listItem.SubItems[3].Text}");
            }

            RunIndex++;
            SetStatus($"Done - {decryptedCount} decrypted, {unmatchedCount} unmatched.");
        }

        private void AutoDecryptWithStoredPasswords(CancellationToken token)
        {
            // Try stored passwords, most-successful-first, against the current work list
            var storedCandidates = dbContext.GetPasswordsByLikelihood()
                .Select(p => new PasswordCandidate(p.PDFPassword, p.ID))
                .ToList();

            if (storedCandidates.Count == 0)
            {
                return;
            }

            DecryptWorkList(GetDecryptWorkList(), storedCandidates, _ => "No stored password matched", token);
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            var workListItems = GetDecryptWorkList();

            if (workListItems.Count == 0)
            {
                MessageBox.Show("No files with Security Settings or Password Protection found!", "No Files", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Check if ActiveForm is not null before accessing its properties
                if (EnD.ActiveForm != null)
                {
                    EnD.ActiveForm.AcceptButton = buttonClose;
                    EnD.ActiveForm.CancelButton = buttonClose;
                    buttonClose.Focus();
                }
                return;
            }

            var manualCandidate = new List<PasswordCandidate> { new(textBoxPassword.Text, null) };
            RunCancellableOperation(token => DecryptWorkList(workListItems, manualCandidate, errmsg => $"Error: {errmsg}", token));

            textBoxPassword.Focus();
            textBoxPassword.SelectAll();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            EnD.ActiveForm?.Close();
        }

        private void textBoxPassword_MouseDown(object sender, MouseEventArgs e)
        {
            textBoxPassword.SelectAll();
        }

        private void checkBoxRecurseDir_CheckedChanged(object sender, EventArgs e)
        {
            RunScanAndAutoDecrypt(RunAutoDecryptOnFirstRecurseToggle);
        }

        private void ResetRunColumns()
        {
            for (var i = listFilesFolders.Columns.Count - 1; i >= 0; i--)
            {
                var column = listFilesFolders.Columns[i];
                if (column != columnHeaderNo && column != columnHeaderFilename && column != columnHeaderState && column != columnHeaderComment)
                {
                    listFilesFolders.Columns.RemoveAt(i);
                }
            }

            RunIndex = 1;
        }

        private void RunAutoDecryptOnFirstRecurseToggle(CancellationToken token)
        {
            // Recursing pulls in a new, larger set of files - worth one more auto-try. Only once per
            // session though, so repeatedly toggling the checkbox doesn't re-run (and re-count) the same passwords.
            if (hasAutoDecryptedOnRecurseToggle)
            {
                return;
            }

            hasAutoDecryptedOnRecurseToggle = true;
            AutoDecryptWithStoredPasswords(token);
        }

        private void btnOpenPWDMgr_Click(object sender, EventArgs e)
        {
            SetStatus("Confirm your Windows credentials to open the Password Manager...");

            if (!CredentialPrompt.VerifyCurrentUser(Handle, "PDF Tools - Password Manager", "Confirm it's you before viewing stored PDF passwords."))
            {
                SetStatus("Password Manager access cancelled.");
                return;
            }

            SetStatus("Verified - opening Password Manager...");

            PasswordManagerForm passwordManagerForm = new();
            passwordManagerForm.ShowDialog();

            if (passwordManagerForm.PasswordsChanged)
            {
                SetStatus("Passwords updated - retrying stored passwords against this list...");
                RunCancellableOperation(AutoDecryptWithStoredPasswords);
            }
            else
            {
                SetStatus("Password Manager closed.");
            }
        }
    }
}

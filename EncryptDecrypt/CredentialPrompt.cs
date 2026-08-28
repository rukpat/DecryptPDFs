using System.Runtime.InteropServices;
using System.Text;

namespace EncryptDecrypt
{
    // Gates access to something with the native "Windows Security" credential dialog: shows the
    // current user's sign-in tile (password, PIN, or Windows Hello - whatever the account has
    // configured), then actually validates whatever was entered via LogonUser. Showing the dialog
    // alone proves nothing - CredUIPromptForWindowsCredentials just collects credentials, it
    // doesn't check them, so the LogonUser call is the part that makes this a real gate rather
    // than a dialog anyone could click through.
    internal static class CredentialPrompt
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CREDUI_INFO
        {
            public int cbSize;
            public IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }

        [Flags]
        private enum CREDUIWIN_FLAGS
        {
            CREDUIWIN_ENUMERATE_CURRENT_USER = 0x200,
        }

        private const int ERROR_CANCELLED = 1223;
        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_PROVIDER_DEFAULT = 0;
        // Required when unpacking a buffer that came from CredUIPromptForWindowsCredentials (the
        // interactive dialog) rather than one packed by hand via CredPackAuthenticationBuffer -
        // without it, CredUnPackAuthenticationBuffer fails regardless of what was entered.
        private const int CRED_PACK_PROTECTED_CREDENTIALS = 0x1;

        [DllImport("credui.dll", CharSet = CharSet.Unicode)]
        private static extern int CredUIPromptForWindowsCredentials(
            ref CREDUI_INFO notificationDataStruct,
            int authError,
            ref uint authPackage,
            IntPtr inAuthBuffer,
            uint inAuthBufferSize,
            out IntPtr outAuthBuffer,
            out uint outAuthBufferSize,
            ref bool saveCredentials,
            int flags);

        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredUnPackAuthenticationBuffer(
            int flags,
            IntPtr authBuffer,
            uint authBufferSize,
            StringBuilder? userName,
            ref int maxUserName,
            StringBuilder? domainName,
            ref int maxDomainName,
            StringBuilder? password,
            ref int maxPassword);

        [DllImport("ole32.dll")]
        private static extern void CoTaskMemFree(IntPtr ptr);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(
            string username,
            string? domain,
            string password,
            int logonType,
            int logonProvider,
            out IntPtr token);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        // Returns true only if the user completed the prompt (didn't cancel) AND the credentials
        // they entered are actually valid for this Windows account.
        public static bool VerifyCurrentUser(IntPtr ownerHandle, string caption, string message)
        {
            var credUiInfo = new CREDUI_INFO
            {
                cbSize = Marshal.SizeOf(typeof(CREDUI_INFO)),
                hwndParent = ownerHandle,
                pszCaptionText = caption,
                pszMessageText = message
            };

            uint authPackage = 0;
            var save = false;

            var result = CredUIPromptForWindowsCredentials(
                ref credUiInfo,
                0,
                ref authPackage,
                IntPtr.Zero,
                0,
                out var outAuthBuffer,
                out var outAuthBufferSize,
                ref save,
                (int)CREDUIWIN_FLAGS.CREDUIWIN_ENUMERATE_CURRENT_USER);

            if (result == ERROR_CANCELLED)
            {
                return false;
            }

            if (result != 0 || outAuthBuffer == IntPtr.Zero)
            {
                // Unexpected failure from the dialog itself (not a user cancel) - fail closed
                return false;
            }

            try
            {
                // First pass with empty buffers: this call is expected to fail with
                // ERROR_INSUFFICIENT_BUFFER, but it fills in the *required* sizes so we don't have
                // to guess a fixed buffer length (which is what was failing before - a 256-char
                // guess wasn't enough for one of the three fields).
                var maxUserName = 0;
                var maxDomainName = 0;
                var maxPassword = 0;
                CredUnPackAuthenticationBuffer(CRED_PACK_PROTECTED_CREDENTIALS, outAuthBuffer, outAuthBufferSize, null, ref maxUserName, null, ref maxDomainName, null, ref maxPassword);

                var userName = new StringBuilder(maxUserName);
                var domainName = new StringBuilder(maxDomainName);
                var password = new StringBuilder(maxPassword);

                if (!CredUnPackAuthenticationBuffer(CRED_PACK_PROTECTED_CREDENTIALS, outAuthBuffer, outAuthBufferSize, userName, ref maxUserName, domainName, ref maxDomainName, password, ref maxPassword))
                {
                    return false;
                }

                var domain = domainName.Length > 0 ? domainName.ToString() : Environment.UserDomainName;

                var verified = LogonUser(userName.ToString(), domain, password.ToString(), LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out var token);
                if (verified)
                {
                    CloseHandle(token);
                }

                password.Clear();
                return verified;
            }
            finally
            {
                // Zero the buffer before freeing it - it held the plaintext password
                var zeros = new byte[outAuthBufferSize];
                Marshal.Copy(zeros, 0, outAuthBuffer, (int)outAuthBufferSize);
                CoTaskMemFree(outAuthBuffer);
            }
        }
    }
}

# Decrypt PDFs

A Windows utility for finding and removing password protection / security restrictions from PDF files in bulk, with a built-in password manager that learns your passwords and tries them automatically.

Right-click a folder or a batch of PDFs in Explorer (via a context-menu or "Send to" entry), and it scans them, tells you which ones are password-protected or have restricted permissions, and can strip that protection - either with a password you type in, or automatically using passwords it already knows.

## Why I built this

Banks, utilities, and financial services all password-protect the PDFs they send - statements, invoices, bills - and almost every sender reuses the same handful of password patterns (a date of birth, an account number, a postcode). Every time one landed, I was doing the same thing: guess which pattern this particular company used, type it in, get it wrong, try the next.

This tool exists so I stopped doing that guessing by hand. Point it at a folder - your Downloads folder, wherever these things pile up - and it works out which files are locked, tries every password it's already learned against them, and decrypts whatever matches. The first time a new sender's password shows up, you type it in once; every file from that sender from then on just opens.

## Features

- **Bulk scan** a folder (optionally recursive) or a set of selected files, classifying each PDF as: no security, has security settings (owner-password-only restrictions), password protected, or error.
- **Auto-decrypt on open**: on load, automatically tries stored passwords against every password-protected file it finds, most-likely-to-work first - see [how it learns](#how-it-learns-and-auto-decrypts-your-passwords) below.
- **Password Manager**: a CRUD screen for the stored passwords (nickname, description, usage stats), gated behind your Windows login and hidden by default - see [Security](#security) below.
- **Remove security settings** too (optional), not just password-protected files.
- **Overwrite in place, or write out with a prefix/suffix**, your choice.
- **Live progress and cancel**: a status bar shows what it's doing (file X of Y, password X of Y); press **S** at any time to stop.

## How it learns and auto-decrypts your passwords

The first time you hit a file with a password you don't have stored, type it into the password box and click **Decrypt**. If it works, that password is saved automatically - there's no separate "add to password manager" step.

From then on, every time it scans a folder, the tool:

1. Pulls every password it's ever stored, ranked by how often - and how recently - each one has actually worked.
2. Tries them against each password-protected file it finds, most-likely-first, until one succeeds or the list runs out.
3. Decrypts on the first match and moves straight to the next file - no prompts, no waiting on you.

So the more you use it, the less you have to think about it: the password that works for every statement from one sender gets tried first against every future file from that same sender, and a password you first used somewhere else entirely can end up matching a file you've never seen before, automatically.

## Security

Since this tool exists specifically to store and reuse your passwords, here's exactly what it does - and doesn't do - with them:

- **Encrypted at rest with Windows DPAPI** (`ProtectedData.Protect`, `CurrentUser` scope) - every password is encrypted before it ever touches disk, using a key tied to your Windows login. Nobody without your Windows account - not another user on the same PC, not someone who copies the database file elsewhere - can decrypt it.
- **Stored outside the app**, at `%LocalAppData%\DecryptPDFs\DecryptPDFs.db` - a per-user folder that other Windows accounts on the same machine can't read, independent of wherever the app itself is installed.
- **Masked by default in the UI**: the Password Manager shows `********` for every stored password until you explicitly check "Show Passwords."
- **Gated behind your Windows login**: opening the Password Manager at all requires re-confirming your Windows credentials (password, PIN, or Windows Hello), via the same native "Windows Security" prompt Windows uses elsewhere - not a login this app invented itself.
- **Never leaves your machine**: no cloud sync, no telemetry, no network calls of any kind. Whatever it learns stays local.
- **Installer runs without admin rights**: it writes its Explorer integration to your per-user registry hive (`HKEY_CURRENT_USER`), not system-wide - no UAC prompt, and nothing it does affects other accounts on the machine.

## Requirements

- Windows
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (if running a framework-dependent build)

## Installation

There are two ways to install this. The installer is the quick path; the manual steps exist for anyone who'd rather see exactly what's being placed on their machine and in their registry before running it - both end up in the same place.

### Option 1: Installer

1. Download `DecryptPDFs-Setup-<version>.exe` from the [Releases page](../../releases) and run it.
2. It installs to `%LocalAppData%\Programs\DecryptPDFs` under your own user account only (no admin rights, no UAC prompt), and offers two checkboxes: adding the right-click context menu, and adding a "Send to" entry.
3. Since the installer isn't code-signed, Windows SmartScreen will likely show an "unrecognized publisher" warning the first time you run it - click **More info → Run anyway**. This is expected for any small, unsigned tool; it's not specific to this installer.
4. Uninstall any time from **Settings → Apps**, or Control Panel's Add/Remove Programs - this removes the files, both shortcuts, and both registry entries cleanly.

The installer script itself is at [`installer/DecryptPDFs.iss`](installer/DecryptPDFs.iss), if you'd like to read exactly what it does before running it, or build it yourself with [Inno Setup](https://jrsoftware.org/isinfo.php).

### Option 2: Manual

1. Download the latest release from the [Releases page](../../releases), or build from source:
   ```bash
   git clone https://github.com/rukpat/DecryptPDFs.git
   cd DecryptPDFs
   dotnet build
   ```
2. To add it to Explorer's right-click menu, edit the paths in [`Resources/ContextMenuFiles.reg`](Resources/ContextMenuFiles.reg) and [`Resources/ContextMenuDirectories.reg`](Resources/ContextMenuDirectories.reg) to point at wherever you placed `DecryptPDFs.exe`, then double-click each `.reg` file to import it. Each one only adds a single registry key (`HKEY_CLASSES_ROOT\*\shell\DecryptPDFs` or the `Directory` equivalent) - open them in a text editor first if you want to see exactly what they'll write before importing.
3. To add it to the right-click **Send to** menu instead, edit [`Resources/Decrypt PDFs.lnk`](Resources/Decrypt%20PDFs.lnk)'s target to match your install location (right-click → Properties), then copy it into `shell:sendto` (paste that into the Explorer address bar to open the folder).

## Usage

1. Right-click a folder or a selection of PDFs → **Decrypt PDFs** (or launch it from the Start Menu and click **Open PDF Files...** to browse for files directly).
2. The tool scans and lists every PDF found, color-coded by status, and automatically tries any stored passwords.
3. For anything still locked, type a password and hit **Decrypt** - if it works, that password is remembered for next time.
4. Open **Password Manager** to review, add, or edit stored passwords directly (you'll be asked to confirm your Windows login first).

## License

[MIT](LICENSE)

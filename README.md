# Decrypt PDFs

A Windows utility for finding and removing password protection / security restrictions from PDF files in bulk, with a built-in password manager that learns your passwords and tries them automatically.

Right-click a folder or a batch of PDFs in Explorer (via a context-menu or "Send to" entry), and it scans them, tells you which ones are password-protected or have restricted permissions, and can strip that protection - either with a password you type in, or automatically using passwords it already knows.

## Features

- **Bulk scan** a folder (optionally recursive) or a set of selected files, classifying each PDF as: no security, has security settings (owner-password-only restrictions), password protected, or error.
- **Auto-decrypt on open**: on load, automatically tries stored passwords against every password-protected file it finds, most-likely-to-work first (ranked by how often and how recently each password has actually succeeded).
- **Remembers passwords automatically**: any password you type in manually that successfully decrypts a file is saved for next time - no separate step required.
- **Password Manager**: a CRUD screen for the stored passwords (nickname, description, usage stats), gated behind your Windows login (password, PIN, or Windows Hello) and hidden by default (toggle to reveal).
- **Encrypted at rest**: stored passwords are encrypted with Windows DPAPI, tied to your Windows account - the database file itself is unreadable outside your own login on this machine.
- **Remove security settings** too (optional), not just password-protected files.
- **Overwrite in place, or write out with a prefix/suffix**, your choice.
- **Live progress and cancel**: a status bar shows what it's doing (file X of Y, password X of Y); press **S** at any time to stop.

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

1. Right-click a folder or a selection of PDFs → **Decrypt PDFs**.
2. The tool scans and lists every PDF found, color-coded by status, and automatically tries any stored passwords.
3. For anything still locked, type a password and hit **Decrypt** - if it works, that password is remembered for next time.
4. Open **Password Manager** to review, add, or edit stored passwords directly (you'll be asked to confirm your Windows login first).

## Security notes

- Stored PDF passwords are encrypted with `ProtectedData.Protect` (Windows DPAPI, `CurrentUser` scope) - only your Windows account, on this machine, can decrypt them.
- The password database lives at `%LocalAppData%\DecryptPDFs\DecryptPDFs.db`, outside the application folder and inaccessible to other Windows accounts on the same machine.
- Viewing stored passwords in the Password Manager requires re-confirming your Windows credentials, and passwords are masked by default.

## License

[MIT](LICENSE)

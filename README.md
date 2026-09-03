# ![icon](DecryptPDFs/Resources/nS48.png) Decrypt PDFs

Bulk-remove password protection from PDFs, right from Windows Explorer - and it learns your passwords so it gets faster every time.

![Main window](Resources/Readme%20Images/MainWindow.png)

## 🤔 Why I built this

Banks, utilities, and financial services send a lot of password-protected PDFs, and every time one landed I was guessing the password, getting it wrong, trying again. Now I just point this at my Downloads folder and everything decrypts itself.

## 🔒 How is it secured

![Windows credential prompt](Resources/Readme%20Images/WindowsCredentialPrompt.png)

- **Encrypted at rest:** Windows DPAPI, tied to your Windows login only.
- **Masked by default:** passwords show as `********` until you reveal them.
- **Gated access:** re-confirm your Windows login (password, PIN, or Hello) to open the Password Manager.
- **Local only:** no cloud sync, no telemetry, no network calls.
- **No admin rights needed:** installer writes to your user registry hive only.

> [!CAUTION]
> Only use this on PDFs you have the legal right to access - your own statements, bills, and documents.

## ⚡ Features

- **Bulk scan:** a folder (recursive optional) or a batch of selected files.
- **Auto-decrypt:** tries every learned password, most-likely-first, no prompts.
- **Password Manager:** encrypted, masked, gated - see above.
- **Flexible output:** overwrite in place, or write out with a prefix/suffix.
- **Live progress:** status bar shows what's happening; press **S** to stop anytime.

![Password Manager](Resources/Readme%20Images/PasswordManager.png)

## 🔁 How it learns and auto-decrypts

1. Type a password once - if it works, it's saved automatically.
2. Next scan, every stored password gets tried, most-successful-first.
3. First match wins - decrypts and moves to the next file.

## Requirements

- Windows
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (framework-dependent build)

## 📦 Installation

### Option 1: Installer (recommended)

1. Download `DecryptPDFs-Setup-<version>.exe` from [Releases](../../releases) and run it.
2. Installs to your own user folder - no admin rights, no UAC prompt.
3. Choose whether to add the right-click menu and/or "Send to" entry.

> [!TIP]
> Windows SmartScreen may warn about an "unrecognized publisher" since the installer isn't code-signed - click **More info → Run anyway**. Normal for any small, unsigned tool.

> [!NOTE]
> **What gets installed:**
> - Files to `%LocalAppData%\Programs\DecryptPDFs`
> - Two `HKEY_CURRENT_USER\...\shell\DecryptPDFs` registry keys (context menu)
> - A shortcut in `shell:sendto` (if selected)
>
> Uninstall anytime from Add/Remove Programs - cleans up all of the above.

### Option 2: Manual

Prefer to see exactly what's added before running anything? Do it by hand instead:

```bash
git clone https://github.com/rukpat/DecryptPDFs.git
cd DecryptPDFs
dotnet build
```

- Edit and import [`ContextMenuFiles.reg`](Resources/ContextMenuFiles.reg) / [`ContextMenuDirectories.reg`](Resources/ContextMenuDirectories.reg) for the right-click menu (points at wherever you place `DecryptPDFs.exe`).
- Edit and copy [`Decrypt PDFs.lnk`](Resources/Decrypt%20PDFs.lnk) into `shell:sendto` for the Send to menu.

## ▶️ Usage

![Right-click menu](Resources/Readme%20Images/RightClick.png)

1. Right-click a folder or a selection of PDFs → **Decrypt PDFs** (or launch from the Start Menu and click **Open PDF Files...**).
2. Anything already known gets decrypted automatically; for the rest, type a password and hit **Decrypt**.
3. Open **Password Manager** anytime to review or edit stored passwords.

> [!IMPORTANT]
> **License:** [MIT](LICENSE)
> **Contributions:** bug reports, feature requests, and PRs welcome.

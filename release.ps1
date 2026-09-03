<#
.SYNOPSIS
    Builds and packages a new Decrypt PDFs release (portable zip + installer).

.DESCRIPTION
    Automates the safe, local, fully-reversible part of cutting a release:
      1. Bumps the version number in installer\DecryptPDFs.iss
      2. Does a clean Release build (dotnet publish, framework-dependent, win-x64)
      3. Zips the portable build
      4. Compiles the Inno Setup installer against that same build

    It deliberately stops there. Tagging, pushing, creating the GitHub release,
    and submitting a winget manifest update all touch public/shared state and
    are hard to undo - the script prints the exact commands for those as a
    final step instead of running them, so you (or Claude, on your say-so) can
    review the artifacts first and run them deliberately.

.PARAMETER Version
    The version to release, e.g. 2.0.4. If omitted, you'll be prompted for it.

.EXAMPLE
    .\release.ps1 -Version 2.0.4

.EXAMPLE
    .\release.ps1
    (prompts for the version number interactively)

.NOTES
    Requires: dotnet SDK, Inno Setup 6 or 7 (ISCC.exe), and no running
    instance of DecryptPDFs.exe (it'll lock the build output).
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [string]$Version,

    [switch]$Help
)

$ErrorActionPreference = "Stop"

if ($Help) {
    Get-Help $PSCommandPath -Full
    exit 0
}

# ---------------------------------------------------------------------------
# Setup
# ---------------------------------------------------------------------------

$RepoRoot = $PSScriptRoot
$ProjectDir = Join-Path $RepoRoot "DecryptPDFs"
$IssPath = Join-Path $RepoRoot "installer\DecryptPDFs.iss"
$PublishDir = Join-Path $ProjectDir "publish\DecryptPDFs"

Write-Host ""
Write-Host "=== Decrypt PDFs - Release Builder ===" -ForegroundColor Cyan
Write-Host ""

if (-not $Version) {
    $Version = Read-Host "Enter the version to release (e.g. 2.0.4)"
}

if ($Version -notmatch '^\d+\.\d+\.\d+$') {
    Write-Error "Version '$Version' doesn't look like X.Y.Z (e.g. 2.0.4). Aborting."
    exit 1
}

Push-Location $RepoRoot
try {
    $existingTag = git tag -l "v$Version"
    if ($existingTag) {
        Write-Error "Tag v$Version already exists. Pick a new version, or delete the old tag first if this was a mistake. Aborting."
        exit 1
    }
}
finally {
    Pop-Location
}

$running = Get-Process -Name "DecryptPDFs" -ErrorAction SilentlyContinue
if ($running) {
    Write-Error "DecryptPDFs.exe is currently running (PID $($running.Id -join ', ')). Close it first - it'll lock the build output. Aborting."
    exit 1
}

# Find Inno Setup's compiler (7 preferred, fall back to 6)
$IsccCandidates = @(
    "C:\Program Files\Inno Setup 7\ISCC.exe",
    "C:\Program Files (x86)\Inno Setup 6\ISCC.exe",
    "C:\Program Files\Inno Setup 6\ISCC.exe"
)
$Iscc = $IsccCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $Iscc) {
    Write-Error "Couldn't find ISCC.exe (Inno Setup's compiler) in any known location. Is Inno Setup installed? Aborting."
    exit 1
}

Write-Host "Version:      $Version"
Write-Host "Repo root:    $RepoRoot"
Write-Host "Inno Setup:   $Iscc"
Write-Host ""

# ---------------------------------------------------------------------------
# 1. Bump the version in the installer script
# ---------------------------------------------------------------------------

Write-Host "[1/4] Bumping version in installer\DecryptPDFs.iss..." -ForegroundColor Yellow

$issContent = Get-Content $IssPath -Raw
$issContentNew = $issContent -replace '#define MyAppVersion "[^"]+"', "#define MyAppVersion `"$Version`""
if ($issContentNew -eq $issContent) {
    Write-Warning "Could not find a MyAppVersion line to update in $IssPath - check it manually."
}
Set-Content -Path $IssPath -Value $issContentNew -NoNewline

Write-Host "      Done." -ForegroundColor Green
Write-Host ""

# ---------------------------------------------------------------------------
# 2. Clean Release build
# ---------------------------------------------------------------------------

Write-Host "[2/4] Clean Release build (dotnet publish, win-x64, framework-dependent)..." -ForegroundColor Yellow

Push-Location $ProjectDir
try {
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue "publish", "bin\Release", "obj\Release"

    dotnet publish -c Release -r win-x64 --self-contained false -o "publish\DecryptPDFs"
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet publish failed with exit code $LASTEXITCODE"
    }
}
finally {
    Pop-Location
}

Write-Host "      Done." -ForegroundColor Green
Write-Host ""

# ---------------------------------------------------------------------------
# 3. Zip the portable build
# ---------------------------------------------------------------------------

Write-Host "[3/4] Zipping the portable build..." -ForegroundColor Yellow

$ZipPath = Join-Path $ProjectDir "publish\DecryptPDFs-v$Version-win-x64.zip"
Compress-Archive -Path (Join-Path $PublishDir "*") -DestinationPath $ZipPath -Force

Write-Host "      Done: $ZipPath" -ForegroundColor Green
Write-Host ""

# ---------------------------------------------------------------------------
# 4. Compile the installer
# ---------------------------------------------------------------------------

Write-Host "[4/4] Compiling the Inno Setup installer..." -ForegroundColor Yellow

& $Iscc $IssPath
if ($LASTEXITCODE -ne 0) {
    throw "ISCC.exe failed with exit code $LASTEXITCODE"
}

$SetupPath = Join-Path $ProjectDir "publish\DecryptPDFs-Setup-$Version.exe"

Write-Host "      Done: $SetupPath" -ForegroundColor Green
Write-Host ""

# ---------------------------------------------------------------------------
# Summary + next steps
# ---------------------------------------------------------------------------

$zipSize = "{0:N1} MB" -f ((Get-Item $ZipPath).Length / 1MB)
$setupSize = "{0:N1} MB" -f ((Get-Item $SetupPath).Length / 1MB)

Write-Host "=== Build complete ===" -ForegroundColor Cyan
Write-Host "  $ZipPath  ($zipSize)"
Write-Host "  $SetupPath  ($setupSize)"
Write-Host ""
Write-Host "Nothing has been committed, tagged, pushed, or published." -ForegroundColor Cyan
Write-Host "Review the artifacts above, then run (or ask Claude to run) these when ready:" -ForegroundColor Cyan
Write-Host ""
Write-Host "  git add installer/DecryptPDFs.iss"
Write-Host "  git commit -m `"Bump installer version to $Version`""
Write-Host "  git push origin master"
Write-Host ""
Write-Host "  git tag -a v$Version -m `"v$Version`""
Write-Host "  git push origin v$Version"
Write-Host ""
Write-Host "  gh release create v$Version `"$ZipPath`" `"$SetupPath`" \"
Write-Host "    --repo rukpat/DecryptPDFs --title `"v$Version`" --notes `"...`""
Write-Host ""
Write-Host "  # winget update (once wingetcreate is installed):"
Write-Host "  wingetcreate update rukpat.DecryptPDFs --version $Version \"
Write-Host "    --urls `"https://github.com/rukpat/DecryptPDFs/releases/download/v$Version/DecryptPDFs-Setup-$Version.exe`" \"
Write-Host "    --submit"
Write-Host ""

# ![ClipboardEdit Logo](src/ClipboardEdit/Bitmaps/logo.png) ClipboardEdit
A utility that allows you to view and edit the Windows clipboard.

## Building
1. Clone the repository.
```pwsh
git clone https://github.com/FireBlade211/ClipboardEdit.git
```

2. Open the directory where the repository was cloned.
```cmd
start ClipboardEdit
```

3. Open the <code>src/ClipboardEdit.slnx</code> file in <B>Visual Studio 2026</B>.
4. Click <B>Build</B> -> <B>Build Solution</B>.

## Dependencies and Requirements
- **Windows 7** or later
- **.NET Framework** 4.7.2 ([Download](https://dotnet.microsoft.com/download/dotnet-framework/net472))

## Installation
Go to the [Releases](https://github.com/FireBlade211/ClipboardEdit/releases) page, download the latest ZIP file, extract it, and run `ClipboardEdit.exe`.

## Help
For more info, see the help file by clicking **Help** -> **Help Contents** in ClipboardEdit.

## Snapshot Format
**ClipboardEdit** uses a custom file format, **.CSNAP**, to store clipboard snapshots. These clipboard snapshots store information about the formats in the clipboard in binary format. You can view more info in the [help file](#help), but for your convenience, if you want to inspect these files in a hex editor, an **ImHex** pattern is provided [here](misc/pattern.hexpat).

However note that this pattern won't help you for **.CSNAPZ** files. **.CSNAPZ** files are identical to their regular **.CSNAP** counterparts, but their full content is **GZIP**-compressed.

# Tool to download EVERYTHING on Sanctuary!

This tool will download everything on [Sanctuary](https://sanctuary.moe) onto your computer.
Due to ongoing legal pressure, this tool can be used to archive everything on Sanctuary in the event we are taken down or we lose our hosting to the servers. 

## Download [here.](https://github.com/sanctuarymoe/sanctuary-archiver/releases/download/release/sanc-download.exe)

If you're not interested in using our tool, a full JSON dump of our data is available at [https://sanctuary.moe/assets/@all/json](https://sanctuary.moe/assets/@all/json)

# Usage:

Use the tool by simply double clicking the executable file, and it'll use 28 pools to download all 3k+ files on your computer. It'll create a folder called `sanctuary-archive`, and make sure you have about ~350 GB of space free for it all.

**If you want to utilize more or less threads, use the tool in a command prompt/terminal like so: `sanc-download.exe <number of threads>`.**
For example, for 40 downloads at once: `sanc-download.exe 40`.

### Note
If you restart, existing files will be overridden (or a new folder may be created), so you should probably clean the dump or copy it somewhere else before restarting again.

If the executable does not work, you may need to download .NET 6 from https://dot.net (the runtime), however it should work without any depedencies.

### Source Code

The source code has been made public (built with .NET 6 in like four hours, lol), if you do not trust the executable, you can load this in Visual Studio, or compile yourself using `dotnet run` or `dotnet build -c Release -r win-x64` and use the executable built from the source code. You can read the source code, to prove that there is nothing malicious in the file.

Linux & Mac users can also leverage .NET 6's cross-platform capabilities to use this.
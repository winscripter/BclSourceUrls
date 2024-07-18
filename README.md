# BclSourceUrls
Huge json file that contains type names included in the .NET BCL and their corresponding source code URLs hosted on GitHub (Auto-generated)

First, build `BclSourceFetcher`, then copy bcl.json to your bin folder in Visual Studio (found in the `./generated` folder) and then use the following sample C# code:
```cs
using BclSourceFetcher;

var sources = BclSources.LoadFrom("bcl.json");
string url = sources.GetUrlOfTypeName("System.Console");
Console.WriteLine(url);
// Output:
// https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Console/src/System/Console.cs
//
// If you do a GET request to this URL you'll get the raw source code since .NET is open source
```

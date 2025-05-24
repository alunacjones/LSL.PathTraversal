[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-pathtraversal.svg)](https://ci.appveyor.com/project/alunacjones/lsl-pathtraversal)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.PathTraversal)](https://coveralls.io/github/alunacjones/LSL.PathTraversal)
[![NuGet](https://img.shields.io/nuget/v/LSL.PathTraversal.svg)](https://www.nuget.org/packages/LSL.PathTraversal/)

# LSL.PathTraversal

This library provides a static method for enumerating up a directory hierarchy from a given initial path
or the current directory if no initial path is provided.

## Locating a file in the hierarchy

The following example attempts to locate a file in the hierarchy, starting from 
the value of `Directory.GetCurrentDirectory()`:

```csharp
// Necessary usings:
using LSL.PathTraversal;

...

var fileInfo = PathTraversal.Enumerate()
    // GetFileInfo is an extension method to the DirectoryInfo class
    .Select(directoryInfo => directoryInfo.GetFileInfo(filename))
    .FirstOrDefault(fileInfo => fileInfo.Exists)

// If the file was found then fileInfo will provide all information
// otherwise fileInfo will be null
```

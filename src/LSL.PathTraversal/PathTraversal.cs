using System;
using System.Collections.Generic;
using System.IO;

namespace LSL.PathTraversal;

/// <summary>
/// Holds the static methods that initiate traversal of paths
/// </summary>
public static class PathTraversal
{
    /// <summary>
    /// Starts enumerating all paths from initialPath back up to the root of
    /// the file system. This then allows for the normal LINQ operators
    /// to be used to do things such as find the first matching file
    /// found in the hierarchy.
    /// </summary>
    /// <remarks>
    /// If <paramref name="initialPath"/> is null or omitted then the current directory is used
    /// as the initial path
    /// </remarks>
    /// <param name="initialPath"></param>
    /// <returns></returns>
    public static IEnumerable<DirectoryInfo> Enumerate(string initialPath = null)
    {
        initialPath ??= Directory.GetCurrentDirectory();
        var currentFolder = new DirectoryInfo(initialPath);
        if (!currentFolder.Exists)
        {
            throw new ArgumentException($"Path '{currentFolder.FullName}' does not exist", nameof(initialPath));
        }
        
        while (currentFolder != null)
        {
            yield return currentFolder;
            currentFolder = currentFolder.Parent;
        }
    }
}

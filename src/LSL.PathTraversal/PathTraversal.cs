using System.IO;

namespace LSL.PathTraversal;

/// <summary>
/// Holds the static methods that initiate traversal
/// of paths to match a file
/// </summary>
public static class PathTraversal
{
    /// <summary>
    /// Traverses up the directory structure to find a matching file until
    /// it reaches the root,
    /// satisfies the <see cref="PathTraversalOptions.FileMatcher"/>
    /// or satisfies <see cref="PathTraversalOptions.ShouldStopTraversal"/> delegate. 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static DirectoryInfo Traverse(PathTraversalOptions options = null)
    {
        options ??= new PathTraversalOptions();
        options.Validate();
        
        var currentDirectory = new DirectoryInfo(options.StartPath);

        while (currentDirectory != null && !options.ShouldStopTraversal(currentDirectory) && !options.FileMatcher(currentDirectory))
        {
            currentDirectory = currentDirectory.Parent;
        }

        return !options.FileMatcher(currentDirectory)
            ? null
            : currentDirectory;
    }

    /// <summary>
    /// Traverses up the directory structure to find a matching file until
    /// it reaches the root,
    /// satisfies the <see cref="PathTraversalOptions.FileMatcher"/> (which 
    /// is set to match the given filename)
    /// or satisfies <see cref="PathTraversalOptions.ShouldStopTraversal"/> delegate. 
    /// </summary>
    /// <param name="fileNameToMatch"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static FileInfo Traverse(string fileNameToMatch, PathTraversalOptions options = null)
    {
        options ??= new PathTraversalOptions();
        options.FileMatcher = directoryInfo => File.Exists(Path.Combine(directoryInfo.FullName, fileNameToMatch));

        var result = Traverse(options);

        return result == null
            ? null
            : new FileInfo(Path.Combine(Traverse(options).FullName, fileNameToMatch));
    }
}

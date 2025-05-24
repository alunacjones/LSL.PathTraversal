using System;
using System.IO;

namespace LSL.PathTraversal;

/// <summary>
/// The options for the path traversal
/// </summary>
public class PathTraversalOptions
{
    /// <summary>
    /// An optional delegate to test a <see cref="DirectoryInfo"></see> 
    /// and return a boolean to indicate if the traversal should stop
    /// </summary>
    /// <remarks>
    /// If omitted then traversal stops at the root.
    /// </remarks>
    public Func<DirectoryInfo, bool> ShouldStopTraversal { get; set; }

    /// <summary>
    /// The start path for traversal to commence from
    /// </summary>
    /// <remarks>
    /// If omitted then the current directory is used.
    /// </remarks>
    public string StartPath { get; set; }

    /// <summary>
    /// A delegate to match a file (or many files) that reside in the given
    /// <see cref="DirectoryInfo"/>
    /// </summary>
    /// <remarks>
    /// This defaults to matching a <c>.env</c> file
    /// </remarks>
    public Func<DirectoryInfo, bool> FileMatcher { get; set; }

    internal void Validate()
    {
        FileMatcher ??= directoryInfo => directoryInfo.FileExists(".env");
        StartPath ??= Directory.GetCurrentDirectory();
        ShouldStopTraversal ??= directoryInfo => directoryInfo.Parent is null;
    }
}

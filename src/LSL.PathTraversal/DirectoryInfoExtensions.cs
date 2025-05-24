using System.IO;

namespace LSL.PathTraversal;

/// <summary>
/// DirectoryInfo Extensions
/// </summary>
public static class DirectoryInfoExtensions
{
    /// <summary>
    /// A helper method to get a <see cref="FileInfo"/> for the given filename.
    /// This can then be checked for file existence in your LINQ query.
    /// </summary>
    /// <param name="directoryInfo"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static FileInfo GetFileInfo(this DirectoryInfo directoryInfo, string fileName) =>
        new(Path.Combine(
            directoryInfo.GuardAgainstNull(nameof(directoryInfo)).FullName,
            fileName.GuardAgainstNull(nameof(fileName)))
        );
}
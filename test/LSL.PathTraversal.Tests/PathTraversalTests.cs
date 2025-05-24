using System;
using System.IO;
using Diamond.Core.System.TemporaryFolder;
using FluentAssertions;
using NUnit.Framework;

namespace LSL.PathTraversal.Tests;

public class PathTraversalTests
{
    [Test]
    public void Traverse_WithNoOptions_ShouldNotThrowAnArgumentException()
    {
        PathTraversal.Traverse().Should().BeNull();
    }

    [Test]
    public void Traverse_WithDefaultOptionsAndNoMatch_ShouldReturnNull()
    {
        var result = PathTraversal.Traverse(new()
        {
            FileMatcher = directoryInfo => directoryInfo.FileExists(".env")
        });

        result.Should().BeNull();
    }

    [Test]
    public void Traverse_WithDefaultOptions_AndAFileMatch_ShouldReturnTheDirectoryInfoThatContainsTheFile()
    {
        using var tempFolder = new TemporaryFolderFactory().Create();
        var startFolder = Directory.CreateDirectory("test/deeper");

        File.WriteAllText(Path.Combine(startFolder.FullName, ".env"), "content");

        var result = PathTraversal.Traverse(new()
        {
            StartPath = startFolder.FullName,
            FileMatcher = directoryInfo => directoryInfo.FileExists(".env")
        });

        result.Should().NotBeNull();
    }

    [Test]
    public void TraverseForFile_WithDefaultOptionsAndNoMatch_ShouldReturnNull()
    {
        var result = PathTraversal.Traverse(".env");

        result.Should().BeNull();
    }

    [Test]
    public void TraverseForFile_WithDefaultOptions_AndAFileMatch_ShouldReturnTheDirectoryInfoThatContainsTheFile()
    {
        using var tempFolder = new TemporaryFolderFactory().Create();
        var startFolder = Directory.CreateDirectory("test/deeper");

        File.WriteAllText(Path.Combine(startFolder.FullName, ".env"), "content");

        var result = PathTraversal.Traverse(
            ".env",
            new PathTraversalOptions
            {
                StartPath = startFolder.FullName,
                FileMatcher = directoryInfo => directoryInfo.FileExists(".env")
            });

        result.Should().NotBeNull();
    }

    [Test]
    public void TraverseForFile_WithCustomPathChecker_AndNoFileMatch_ShouldReturnNull()
    {
        using var tempFolder = new TemporaryFolderFactory().Create();
        var startFolder = Directory.CreateDirectory(Path.Combine(tempFolder.FullPath, "test/deeper"));

        File.WriteAllText(Path.Combine(startFolder.Parent.Parent.FullName, ".env"), "content");

        var result = PathTraversal.Traverse(
            ".env",
            new PathTraversalOptions
            {
                StartPath = startFolder.FullName,
                ShouldStopTraversal = directoryInfo => directoryInfo.FullName == startFolder.Parent.FullName
            });

        result.Should().BeNull();
    }             
}
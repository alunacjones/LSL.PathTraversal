using System;
using System.IO;
using System.Linq;
using Diamond.Core.System.TemporaryFolder;
using FluentAssertions;
using NUnit.Framework;

namespace LSL.PathTraversal.Tests;

public class PathTraversalTests
{
    [Test]
    public void Enumerate_WithAValidPath_ShouldEnumerateAsExpected()
    {
        PathTraversal.Enumerate().Should().HaveCountGreaterThan(0);
    }

    [Test]
    public void Enumerate_WithAnInvalidPath_ShouldThrowTheExpectedException()
    {
        new Action(() => _ = PathTraversal.Enumerate("not-a-folder").ToList())
            .Should()
            .Throw<ArgumentException>()
            .And
            .ParamName
            .Should()
            .Be("initialPath");
    }

    [Test]
    public void Enumerate_WithAValidPath_AndMatchingASingleFileThat_DoesNotExist_ShouldReturnNoMatches()
    {
        PathTraversal.Enumerate()
            .Select(di => di.GetFileInfo(".env"))
            .FirstOrDefault(fi => fi.Exists)
            .Should()
            .BeNull();
    }

    [Test]
    public void Enumerate_WithAValidPath_AndMatchingASingleFileThat_DoesExist_ShouldReturnTheMatchedFile()
    {
        using var tempFolder = new TemporaryFolderFactory().Create();
        var startFolder = Directory.CreateDirectory("test/deeper");
        var filename = ".env";

        File.WriteAllText(Path.Combine(startFolder.FullName, filename), "content");

        PathTraversal.Enumerate(startFolder.FullName)
            .Select(directoryInfo => directoryInfo.GetFileInfo(filename))
            .FirstOrDefault(fileInfo => fileInfo.Exists)
            .Should()
            .NotBeNull();
    }
    
    [Test]
    public void Enumerate_WithAValidPath_AndMatchingANullSingleFile_ShouldThrowTheExpectedException()
    {
        using var tempFolder = new TemporaryFolderFactory().Create();
        var startFolder = Directory.CreateDirectory("test/deeper");
        var filename = (string)null;

        new Action(() => _ = PathTraversal.Enumerate(startFolder.FullName)
            .Select(di => di.GetFileInfo(filename))
            .FirstOrDefault(fi => fi.Exists))
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("fileName");
    }    
}
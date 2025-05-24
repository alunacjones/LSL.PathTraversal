using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.System.TemporaryFolder;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using DC = LSL.DisposableCurrentDirectory;

namespace LSL.PathTraversal.Tests;

public class PathTraversalTests
{
    [Test]
    public void Enumerate_WithAValidPath_ShouldEnumerateAsExpected()
    {
        TraversePath.Enumerate().Should().HaveCountGreaterThan(0);
    }

    [Test]
    public void Enumerate_WithAnInvalidPath_ShouldThrowTheExpectedException()
    {
        new Action(() => _ = TraversePath.Enumerate("not-a-folder").ToList())
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
        TraversePath.Enumerate()
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

        TraversePath.Enumerate(startFolder.FullName)
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

        new Action(() => _ = TraversePath.Enumerate(startFolder.FullName)
            .Select(di => di.GetFileInfo(filename))
            .FirstOrDefault(fi => fi.Exists))
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("fileName");
    }

    [Test]
    public async Task Enumerate_Merging()
    {
        using var currentFolder = new DC.DisposableCurrentDirectory("TestData/Merging/inner1/inner2");

        var files = TraversePath.Enumerate()
            .Select(di => di.GetFileInfo("test.json"))
            .TakeWhile(fi => fi.Directory.Name != "TestData")
            .Reverse();

        var startValue = JObject.Parse("{}");

        foreach (var file in files)
        {
            var content = await File.ReadAllTextAsync(file.FullName);
            startValue.Merge(JObject.Parse(content), new() { MergeArrayHandling = MergeArrayHandling.Replace });
        }

        startValue.ToObject<MergeData>()
            .Should()
            .BeEquivalentTo(new MergeData
            {
                Integer = 99,
                String = "inner1-string",
                StringArray = [
                    "inner2-item1",
                    "inner2-item2",
                    "inner2-item3"
                ]
            });

        files.Should().HaveCount(3);
    }

    private class MergeData
    {
        public int Integer { get; set; }
        public string String { get; set; }
        public IEnumerable<string> StringArray { get; set; } = [];
    }
}
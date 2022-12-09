using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AOC2022.Solutions
{
    public class Day7
    {
        private readonly ITestOutputHelper _output;

        public Day7(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Run()
        {
            var file = Helpers.ReadTextFile("*-7.txt");
            var parentFolder = new Folder
            {
                Name = ""
            };
            var currentFolder = parentFolder;
            foreach (var line in file.Split(Environment.NewLine))
            {
                if (line.Contains("$")) _output.WriteLine(line);
                if (line.Contains(".."))
                {
                    currentFolder = currentFolder.ParentFolder;
                    continue;
                }

                if (line.Contains("$ cd")) // Navigating to new sub folder
                {
                    var nextFolderName = line.Split("$ cd ").Skip(1).FirstOrDefault();
                    _output.WriteLine($"Now in folder {currentFolder.Name} and navigating to {nextFolderName}");

                    var newSubFolder = new Folder { Name = nextFolderName, ParentFolder = currentFolder };
                    currentFolder.SubFolders.Add(newSubFolder);

                    currentFolder = newSubFolder;
                }

                var regex = @"\d+";
                if (Regex.Match(line, regex).Value != "")
                {
                    var number = int.Parse(Regex.Match(line, regex).Value);

                    currentFolder.ContainedFiles.Add(new File
                    {
                        Name = line.Split(" ").Skip(1).FirstOrDefault(),
                        Size = number,
                    });
                }

                _output.WriteLine(currentFolder.Name);
            }

            var allSubFolders = parentFolder.AllSubFolders.ToList();
            var foldersBelowThreshold = allSubFolders.Where(f => f.FolderSize <= 100000).ToList();
            var totalSystemSpace = 70_000_000;
            var neededFreeSpace = 30_000_000;
            var totalSpaceUsed = parentFolder.FolderSize;
            var freeSpace = totalSystemSpace - totalSpaceUsed;
            var spaceToFreeUp = neededFreeSpace - freeSpace;
            var smallestFolderToFreeSpace = allSubFolders.OrderBy(f => f.FolderSize)
                .FirstOrDefault(f => f.FolderSize >= spaceToFreeUp);
            // var totalUsedSpace = 

            _output.WriteLine(foldersBelowThreshold.Sum(f => f.FolderSize).ToString());
        }

        public class Folder
        {
            public string Name { get; set; }
            public List<Folder> SubFolders { get; set; } = new();
            public List<File> ContainedFiles { get; set; } = new();
            public int FolderSize => SubFolders.Sum(f => f.FolderSize) + ContainedFiles.Sum(f => f.Size);

            public List<Folder> AllSubFolders =>
                SubFolders.Concat(SubFolders.SelectMany(f => f.AllSubFolders)).ToList();

            public Folder ParentFolder { get; set; }
        }

        public class File
        {
            public string Name { get; set; }
            public int Size { get; set; }
        }
    }
}
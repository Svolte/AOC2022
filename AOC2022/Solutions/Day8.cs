using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AOC2022.Solutions
{
    public class Day8
    {
        private readonly ITestOutputHelper _output;

        public Day8(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// It's not pretty but it sure works
        /// </summary>
        [Fact]
        public async Task Run()
        {
            var file = Helpers.ReadTextFile("*-8.txt");
            var lines = new List<List<int>>();
            foreach (var line in file.Split(Environment.NewLine))
            {
                var thisLine = new List<int>();
                foreach (var character in line)
                {
                    thisLine.Add(int.Parse(character.ToString()));
                }

                lines.Add(thisLine);
            }

            var allTrees = new List<Tree>();
            for (int r = 0; r < lines.Count; r++)
            {
                var row = lines[r];
                for (int c = 0; c < row.Count; c++)
                {
                    var currentValue = row[c];
                    var currentColumn = lines.Select(l => l.Skip(c).FirstOrDefault()).ToList();
                    var currentRow = lines[r];

                    var thisTree = new Tree
                    {
                        Column = c,
                        Row = r,
                        ScenicScore = 0
                    };
                    allTrees.Add(thisTree);

                    // Visible from top?
                    var aboveThis = currentColumn.Take(r).ToList();
                    if (aboveThis.All(cu => cu < currentValue) || aboveThis.Count == 0)
                    {
                        thisTree.IsVisible = true;
                    }

                    aboveThis.Reverse();
                    var firstHigherTree = aboveThis.FindIndex(t => t >= currentValue);
                    var topScore = firstHigherTree == -1 ? aboveThis.Count : firstHigherTree + 1;
                    _output.WriteLine($"{r},{c} top score: {topScore}");

                    // Visible from bottom?
                    var belowThis = currentColumn.Skip(r + 1).ToList();
                    if (belowThis.All(cu => cu < currentValue) || belowThis.Count == 0)
                    {
                        thisTree.IsVisible = true;
                    }

                    firstHigherTree = belowThis.FindIndex(t => t >= currentValue);
                    var belowScore = firstHigherTree == -1 ? belowThis.Count : firstHigherTree + 1;
                    _output.WriteLine($"{r},{c} below score: {belowScore}");

                    // Visible from the left?
                    var leftOfThis = currentRow.Take(c).ToList();
                    if (leftOfThis.All(cu => cu < currentValue) || leftOfThis.Count == 0)
                    {
                        thisTree.IsVisible = true;
                    }

                    leftOfThis.Reverse();
                    firstHigherTree = leftOfThis.FindIndex(t => t >= currentValue);
                    var leftScore = firstHigherTree == -1 ? leftOfThis.Count : firstHigherTree + 1;
                    _output.WriteLine($"{r},{c} left score: {leftScore}");

                    // Visible from the right?
                    var rightOfThis = currentRow.Skip(c + 1).ToList();
                    if (rightOfThis.All(cu => cu < currentValue) || rightOfThis.Count == 0)
                    {
                        thisTree.IsVisible = true;
                    }

                    firstHigherTree = rightOfThis.FindIndex(t => t >= currentValue);
                    var rightScore = firstHigherTree == -1 ? rightOfThis.Count : firstHigherTree + 1;
                    _output.WriteLine($"{r},{c} right score: {rightScore}");

                    thisTree.ScenicScore = topScore * belowScore * leftScore * rightScore;
                    _output.WriteLine(currentValue.ToString());
                    _output.WriteLine($"This tree's scenic score: {thisTree.ScenicScore.ToString()}");
                }
            }

            _output.WriteLine(allTrees.Count(t => t.IsVisible).ToString());
            _output.WriteLine(allTrees.OrderByDescending(t => t.ScenicScore).FirstOrDefault().ScenicScore.ToString());
        }

        public class Tree
        {
            public int Row { get; set; }
            public int Column { get; set; }
            bool _isVisible;

            public bool IsVisible { get; set; }

            public int ScenicScore { get; set; }
        }
    }
}
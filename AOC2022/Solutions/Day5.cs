using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AOC2022;
using Xunit;
using Xunit.Abstractions;

namespace AOC2022.Solutions
{
    public class Day5
    {
        private readonly ITestOutputHelper _output;

        public Day5(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Run()
        {
            var lists = GetArrangedLists();
            var instructions = Helpers.ReadTextFile("*-5.txt");
            var instructionsList = instructions.Split("\r\n").ToList();
            foreach (var instruction in instructionsList)
            {
                var regex = @"move | from | to ";
                var splits = Regex.Split(instruction, regex).Skip(1).Select(s => int.Parse(s)).ToList();
                var (count, from, to) = (splits[0], splits[1], splits[2]);
                for (int i = 0; i < count; i++)
                {
                    MoveTopCharacter(lists, from, to);
                }
            }

            _output.WriteLine("testg");
        }

        [Fact]
        public async Task Run_2()
        {
            var lists = GetArrangedLists();
            var instructions = Helpers.ReadTextFile("*-5.txt");
            var instructionsList = instructions.Split("\r\n").ToList();
            foreach (var instruction in instructionsList)
            {
                var regex = @"move | from | to ";
                var splits = Regex.Split(instruction, regex).Skip(1).Select(s => int.Parse(s)).ToList();
                var (count, from, to) = (splits[0], splits[1], splits[2]);
                MoveMultipleCharacters(lists, count, from, to);
            }

            _output.WriteLine("testg");
        }

        private List<List<string>> GetArrangedLists()
        {
            string[,] matrix = new string[8, 9]
            {
                { "B", "", "", "", "", "", "N", "", "H" },
                { "V", "", "", "P", "T", "", "V", "", "P" },
                { "W", "", "C", "T", "S", "", "H", "", "N" },
                { "T", "", "J", "Z", "M", "N", "F", "", "L" },
                { "Q", "", "W", "N", "J", "T", "Q", "R", "B" },
                { "N", "B", "Q", "R", "V", "F", "D", "F", "M" },
                { "H", "W", "S", "J", "P", "W", "L", "P", "S" },
                { "D", "D", "T", "F", "G", "B", "B", "H", "Z" },
            };

            var transposedMatrix = new string[9, 8];
            for (int j = 0; j < 9; j++)
            for (int r = 0; r < 8; r++)
                transposedMatrix[j, r] = matrix[r, j];

            var matrixList = new List<List<string>>();

            for (int k = 0; k < transposedMatrix.GetLength(0); k++)
            {
                var newList = new List<string>();
                for (int l = 0; l < transposedMatrix.GetLength(1); l++)
                {
                    var value = transposedMatrix[k, l];
                    if (!string.IsNullOrEmpty(value)) newList.Add(transposedMatrix[k, l]);
                    _output.WriteLine($"{transposedMatrix[k, l]}");
                }

                matrixList.Add(newList);
            }

            return matrixList;
        }

        private void MoveTopCharacter(List<List<string>> lists, int from, int to)
        {
            var fromList = lists[from - 1];
            var toList = lists[to - 1];
            var firstCharacter = fromList.FirstOrDefault();
            if (firstCharacter != null)
            {
                fromList.Remove(fromList.FirstOrDefault());
                toList.Insert(0, firstCharacter);
            }
        }

        private void MoveMultipleCharacters(List<List<string>> lists, int count, int from, int to)
        {
            var fromList = lists[from - 1];
            var toList = lists[to - 1];

            var realCount = Math.Min(count, fromList.Count);
            var characters = fromList.Take(realCount);

            if (characters.Any())
            {
                toList.InsertRange(0, characters.ToList());
                fromList.RemoveRange(0, characters.Count());
                //fromList.RemoveRange(0, realCount);
            }
        }
    }
}
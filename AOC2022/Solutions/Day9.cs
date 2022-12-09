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
    public class Day9
    {
        private readonly ITestOutputHelper _output;

        public Day9(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task Run()
        {
            var file = Helpers.ReadTextFile("*-9.txt");
            var startingPosition = new Position()
            {
                X = 0,
                Y = 0
            };

            List<Knot> knots = new()
            {
                new Knot
                {
                    Name = "H",
                    CurrentPosition = startingPosition
                },
                new Knot
                {
                    Name = "T",
                    CurrentPosition = startingPosition
                }
            };

            var headKnot = knots.FirstOrDefault();
            var tailKnot = knots.Skip(1).FirstOrDefault();

            foreach (var line in file.Split(Environment.NewLine))
            {
                var splitLine = line.Split(" ");
                var direction = splitLine.FirstOrDefault();
                var steps = splitLine.Skip(1).FirstOrDefault();

                var xMovement = direction switch
                {
                    "R" => 1,
                    "L" => -1,
                    _ => 0
                };
                var yMovement = direction switch
                {
                    "U" => 1,
                    "D" => -1,
                    _ => 0
                };

                var currentPosition = headKnot.CurrentPosition;
                headKnot.HistoricalPositions.Add(currentPosition);
                var newPosition = new Position { X = currentPosition.X + xMovement, Y = currentPosition.Y + yMovement };
                headKnot.CurrentPosition = newPosition;

                var headTailDelta = (headKnot.CurrentPosition.X - tailKnot.CurrentPosition.X,
                    headKnot.CurrentPosition.Y - tailKnot.CurrentPosition.Y);
                var headTailDistance = Math.Abs(headKnot.CurrentPosition.X - tailKnot.CurrentPosition.X) +
                                       Math.Abs(headKnot.CurrentPosition.Y - tailKnot.CurrentPosition.Y);
                
                Console.WriteLine($"Head is at {headKnot.CurrentPosition.Name}. Tail is at {tailKnot.CurrentPosition.Name}. Distance is {headTailDistance} with delta {headTailDelta.Item1}, {headTailDelta.Item2}");
                if (headTailDistance is 1) continue;

                if (!HeadAndKnotAreInTouch(headKnot, tailKnot))
                {
                    var tailCurrentPosition = tailKnot.CurrentPosition;
                    tailKnot.HistoricalPositions.Add(tailCurrentPosition);
                    tailKnot.CurrentPosition = new Position
                    {
                        X = tailCurrentPosition.X + headTailDelta.Item1, Y = tailCurrentPosition.Y + headTailDelta.Item2
                    };
                }
            }

            var uniqueTailPositions = tailKnot.HistoricalPositions.GroupBy(p => p.Name).Select(g => g.FirstOrDefault()).ToList();
            _output.WriteLine($"Done! Tail knot went to {uniqueTailPositions.Count} positions.");
        }
        
        public bool HeadAndKnotAreInTouch(Knot headKnot, Knot tailKnot)
        {
            var xDistance = Math.Abs(headKnot.CurrentPosition.X - tailKnot.CurrentPosition.X);
            var yDistance = Math.Abs(headKnot.CurrentPosition.Y - tailKnot.CurrentPosition.Y);
            if (xDistance > 1 || yDistance > 1) return false;

            return true;
        }

        public class Knot
        {
            public string Name { get; set; }
            public Position CurrentPosition { get; set; }
            public List<Position> HistoricalPositions { get; set; } = new List<Position>();
        }

        public class Position
        {
            public string Name => $"{X},{Y}";
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
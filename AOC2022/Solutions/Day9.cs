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

                for (int i = 0; i < int.Parse(steps); i++)
                {
                    var currentPosition = headKnot.CurrentPosition;
                    headKnot.HistoricalPositions.Add(currentPosition);
                    var newPosition = new Position
                        { X = currentPosition.X + xMovement, Y = currentPosition.Y + yMovement };
                    headKnot.CurrentPosition = newPosition;

                    var headTailDelta = (headKnot.CurrentPosition.X - tailKnot.CurrentPosition.X,
                        headKnot.CurrentPosition.Y - tailKnot.CurrentPosition.Y);
                    var headTailDistance = Math.Abs(headKnot.CurrentPosition.X - tailKnot.CurrentPosition.X) +
                                           Math.Abs(headKnot.CurrentPosition.Y - tailKnot.CurrentPosition.Y);

                    Console.WriteLine(
                        $"Head is at {headKnot.CurrentPosition.Name}. Tail is at {tailKnot.CurrentPosition.Name}. Distance is {headTailDistance} with delta {headTailDelta.Item1}, {headTailDelta.Item2}");
                    if (headTailDistance is 1) continue;

                    if (!HeadAndKnotAreInTouch(headKnot, tailKnot))
                    {
                        var tailCurrentPosition = tailKnot.CurrentPosition;
                        tailKnot.HistoricalPositions.Add(tailCurrentPosition);
                        var xCorrection = Math.Abs(headTailDelta.Item1) switch
                        {
                            > 1 => 1,
                            _ => 0
                        };
                        if (direction is "R") xCorrection = -xCorrection;
                        var yCorrection = Math.Abs(headTailDelta.Item2) switch
                        {
                            > 1 => 1,
                            _ => 0
                        };
                        if (direction is "U") yCorrection = -yCorrection;
                        tailKnot.CurrentPosition = new Position
                        {
                            X = tailCurrentPosition.X + headTailDelta.Item1 + xCorrection,
                            Y = tailCurrentPosition.Y + headTailDelta.Item2 + yCorrection
                        };
                    }
                }
            }

            var uniqueTailPositions = tailKnot.HistoricalPositions.GroupBy(p => p.Name).Select(g => g.FirstOrDefault())
                .ToList();
            _output.WriteLine($"Done! Tail knot went to {uniqueTailPositions.Count + 1} positions.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task Run_2()
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
                    Name = "1",
                    CurrentPosition = startingPosition
                },
                new Knot
                {
                    Name = "2",
                    CurrentPosition = startingPosition
                },
                new Knot
                {
                    Name = "3",
                    CurrentPosition = startingPosition
                },
                new Knot
                {
                    Name = "4",
                    CurrentPosition = startingPosition
                },
                new Knot
                {
                    Name = "5",
                    CurrentPosition = startingPosition
                },
                new Knot
                {
                    Name = "6",
                    CurrentPosition = startingPosition
                },
                new Knot
                {
                    Name = "7",
                    CurrentPosition = startingPosition
                },
                new Knot
                {
                    Name = "8",
                    CurrentPosition = startingPosition
                },
                new Knot
                {
                    Name = "9",
                    CurrentPosition = startingPosition
                },
            };


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
                var headKnot = knots.FirstOrDefault();

                for (int i = 0; i < int.Parse(steps); i++)
                {
                    var currentPosition = headKnot.CurrentPosition;
                    headKnot.HistoricalPositions.Add(currentPosition);
                    var newPosition = new Position
                        { X = currentPosition.X + xMovement, Y = currentPosition.Y + yMovement };
                    headKnot.CurrentPosition = newPosition;

                    for (int j = 0; j < knots.Count; j++)
                    {
                        if (j == 0) continue;
                        var tailKnot = knots[j];
                        var myHeadKnot = knots[j - 1];
                        var headTailDelta = (myHeadKnot.CurrentPosition.X - tailKnot.CurrentPosition.X,
                            myHeadKnot.CurrentPosition.Y - tailKnot.CurrentPosition.Y);
                        var headTailDistance = Math.Abs(myHeadKnot.CurrentPosition.X - tailKnot.CurrentPosition.X) +
                                               Math.Abs(myHeadKnot.CurrentPosition.Y - tailKnot.CurrentPosition.Y);

                        if (headTailDistance is 1) continue;
                        if (!HeadAndKnotAreInTouch(myHeadKnot, tailKnot))
                        {
                            var tailCurrentPosition = tailKnot.CurrentPosition;
                            tailKnot.HistoricalPositions.Add(tailCurrentPosition);
                            var xCorrection = headTailDelta.Item1 switch
                            {
                                >= 1 => 1,
                                <= -1 => -1,
                                _ => 0
                            };
                            var yCorrection = headTailDelta.Item2 switch
                            {
                                >= 1 => 1,
                                <= -1 => -1,
                                _ => 0
                            };
                            tailKnot.CurrentPosition = new Position
                            {
                                X = tailCurrentPosition.X + xCorrection,
                                Y = tailCurrentPosition.Y + yCorrection
                            };
                        }
                        Console.WriteLine($"Head knot is at {headKnot.CurrentPosition.Name}. My head is at {myHeadKnot.CurrentPosition.Name}. Tail {tailKnot.Name} is at {tailKnot.CurrentPosition.Name}. Distance is {headTailDistance} with delta {headTailDelta.Item1}, {headTailDelta.Item2}");

                    }
                }
            }

            var uniqueTailPositions = knots.LastOrDefault().HistoricalPositions.GroupBy(p => p.Name)
                .Select(g => g.FirstOrDefault()).ToList();
            _output.WriteLine($"Done! Tail knot went to {uniqueTailPositions.Count + 1} positions.");
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
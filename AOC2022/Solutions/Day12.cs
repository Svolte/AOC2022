using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AOC2022.Solutions
{
    public class Day12
    {
        private readonly ITestOutputHelper _output;

        public Day12(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Part1()
        {
            var history = GetPathHistory();
            var distanceToStart = history.FirstOrDefault(h => h.Value.Name == 'S').Value.Distance;
            _output.WriteLine(distanceToStart.ToString());
        }

        [Fact]
        public void Part2()
        {
            var history = GetPathHistory();
            var shortestToA = history.Where(p => p.Value.Name == 'a').OrderBy(p => p.Value.Distance).First().Value
                .Distance;
            _output.WriteLine(shortestToA.ToString());
        }

        private Dictionary<Coordinate, VisitedPoint> GetPathHistory()
        {
            var input = Helpers.ReadTextFile("*-12.txt");
            var endElevation = 'E';
            var inputMap = LocationMap(input);
            var start = inputMap.FirstOrDefault(l => l.Value == endElevation);
            var pathHistory = new Dictionary<Coordinate, VisitedPoint>
            {
                {
                    start.Key,
                    new VisitedPoint(endElevation, 0)
                }
            };

            var queue = new Queue<Coordinate>();
            queue.Enqueue(start.Key);

            while (queue.Any())
            {
                var current = queue.Dequeue();
                var currentPoint = pathHistory[current];
                var reachableNeighbors = ReachableNeighbors(current);

                foreach (var neighbor in reachableNeighbors)
                {
                    if (pathHistory.ContainsKey(neighbor)) continue;
                    if (!inputMap.ContainsKey(neighbor)) continue;

                    var thisElevation = GetElevation(inputMap[current]);
                    var nextElevation = GetElevation(inputMap[neighbor]);
                    if (thisElevation - nextElevation > 1) continue;

                    pathHistory[neighbor] = new VisitedPoint(inputMap[neighbor], currentPoint.Distance + 1);
                    queue.Enqueue(neighbor);
                }
            }

            return pathHistory;
        }

        char GetElevation(char s)
        {
            return s switch
            {
                'S' => 'a',
                'E' => 'z',
                _ => s
            };
        }

        List<Coordinate> ReachableNeighbors(Coordinate coordinate)
        {
            return new List<Coordinate>
            {
                coordinate with { X = coordinate.X + 1 },
                coordinate with { X = coordinate.X - 1 },
                coordinate with { Y = coordinate.Y + 1 },
                coordinate with { Y = coordinate.Y - 1 }
            };
        }

        ImmutableDictionary<Coordinate, char> LocationMap(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var kvp =
                from y in Enumerable.Range(0, lines.Length)
                from x in Enumerable.Range(0, lines[0].Length)
                select new KeyValuePair<Coordinate, char>(
                    new Coordinate(x, y), lines[y][x]
                );
            return kvp.ToImmutableDictionary();
        }
    }

    public record Coordinate(int X, int Y);

    public record VisitedPoint(char Name, int Distance);
}
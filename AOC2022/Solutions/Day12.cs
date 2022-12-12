using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task Run()
        {
            var input = Helpers.ReadTextFile("*-12.txt");
            var allLocations = CreateLocationMap(input);
            var possiblePaths = new List<List<Location>>();
            var startElevation = 'S';
            var endElevation = 'E';
            var locationElevations = LocationElevationDictionary(input);
            var start = locationElevations.FirstOrDefault(l => l.Value == endElevation);
            var currentLocation = start.Key;
            var paths = new List<List<Location>>();

            var locationsNextToStart = LocationsNextToThis(locationElevations, start.Key);

            foreach (var locationNextToStart in locationsNextToStart)
            {
                var locationsNextToThis = LocationsNextToThis(locationElevations, locationNextToStart);
                foreach (var location in locationsNextToThis)
                {
                    if (currentLocation.Name - 1 >= location.Name)
                    {
                        Console.WriteLine($"Found lower point: {location.Name}");
                    }
                }
            }

            // while (currentLocation.Name != startElevation)
            // {
            //     var neighbors = LocationsNextToThis(currentLocation);
            //     foreach (var neighbor in neighbors)
            //     {
            //         var path = new List<Location>();
            //         path.Add(currentLocation);
            //         path.Add(neighbor);
            //         paths.Add(path);
            //     }
            // }
        }

        public List<Location> LocationsNextToThis(ImmutableDictionary<Location, char> locations, Location location)
        {
            var locationsNextToThis = locations.Where(v =>
                v.Key.X >= location.X - 1 && v.Key.X <= location.X + 1 &&
                v.Key.Y >= location.Y - 1 &&
                v.Key.Y <= location.Y + 1 && v.Key != location).Select(c => c.Key).ToList();
            return locationsNextToThis;
        }

        public ImmutableDictionary<Location, char> LocationElevationDictionary(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var asd =
                from y in Enumerable.Range(0, lines.Length)
                from x in Enumerable.Range(0, lines[0].Length - 1)
                select new KeyValuePair<Location, char>(
                    new Location(x, y), lines[y][x]
                );
            return asd.ToImmutableDictionary();
        }

        public string[,] CreateLocationMap(string input)
        {
            var lines = input.Split("\r\n").Select(c => c.ToCharArray()).ToList();
            var allLocations = new string[lines.Count, lines[0].Length];

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    var value = lines[i][j].ToString();
                    allLocations[i, j] = value;
                }
            }

            return allLocations;
        }

        public List<Location> PossibleStepsFromHere(List<Location> locations, Location location)
        {
            return new List<Location>();
        }

        // public List<Location> LocationsNextToThis(Location location)
        // {
        //     return new List<Location>
        //     {
        //         new Location(location.X + 1, location.Y),
        //         new Location(location.X - 1, location.Y),
        //         new Location(location.X, location.Y + 1),
        //         new Location(location.X, location.Y - 1),
        //     };
        // }
    }

    public class Location
    {
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public char Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace CalendarConsole
{
    public class Tests
    {
        private readonly ITestOutputHelper _output;

        public Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>Outputs the sum of each Elves Calories on a new line in the output. Excel was used to
        /// sort the resulting values by descending to get the sum of the top three Calory carrying Elves.
        /// (Ugly, but quicker than rewriting the code to account for this ...)</summary>
        [Fact]
        public void Day1()
        {
            var file = Directory.GetFiles(@"C:\Calendar", "*.txt").FirstOrDefault();
            using (StreamReader sr = File.OpenText(file))
            {
                string s;
                int sum = 0;
                while ((s = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        _output.WriteLine(sum.ToString());
                        sum = 0;
                        continue;
                    }

                    sum += int.Parse(s);
                }
            }
        }

        /// <summary>
        /// Note to self: please read the instructions VERY carefully (do not assume an order of, let's say, XYZ,
        /// for the given parameters ...) next time.
        /// </summary>
        [Fact]
        public void Day2()
        {
            var file = Directory.GetFiles(@"C:\Calendar", "*-2.txt").FirstOrDefault();
            using (StreamReader sr = File.OpenText(file))
            {
                string s;
                int sum = 0;
                var sums = new List<(int, int)>();
                while ((s = sr.ReadLine()) != null)
                {
                    var splits = s.Split(' ');
                    var firstPlayerBet = splits.FirstOrDefault();
                    var secondPlayerBet = splits.Skip(1).FirstOrDefault();

                    var firstBasePoints = firstPlayerBet switch
                    {
                        "A" => 1,
                        "B" => 2,
                        "C" => 3
                    };

                    var secondPlayerRealBet = (firstPlayerBet, secondPlayerBet) switch
                    {
                        ("A", "X") => "Z",
                        ("A", "Y") => "X",
                        ("A", "Z") => "Y",
                        ("B", "X") => "X",
                        ("B", "Y") => "Y",
                        ("B", "Z") => "Z",
                        ("C", "X") => "Y",
                        ("C", "Y") => "Z",
                        ("C", "Z") => "X",
                    };
                    
                    // X - Need to lose
                    // Y - Need to draw
                    // Z - need to win
                    var secondRealBasePoints = secondPlayerRealBet switch
                    {
                        "X" => 1,
                        "Y" => 2,
                        "Z" => 3
                    };

                    var winnerPoints = (firstPlayerBet, secondPlayerRealBet) switch
                    {
                        ("A", "Y") => (0, 6),
                        ("A", "X") => (3, 3),
                        ("A", "Z") => (6, 0),
                        ("B", "Y") => (3, 3),
                        ("B", "X") => (6, 0),
                        ("B", "Z") => (0, 6),
                        ("C", "Y") => (6, 0),
                        ("C", "X") => (0, 6),
                        ("C", "Z") => (3, 3)
                    };
                    
                    sums.Add((firstBasePoints + winnerPoints.Item1, secondRealBasePoints + winnerPoints.Item2));
                }

                _output.WriteLine(sums.Select(s => s.Item1).Sum().ToString());
                _output.WriteLine(sums.Select(s => s.Item2).Sum().ToString());
                
            }
        }
        
        /// <summary>
        /// Note to self: please read the instructions VERY carefully (do not assume an order of, let's say, XYZ,
        /// for the given parameters ...) next time.
        /// </summary>
        [Fact]
        public void Day2_Refactored()
        {
            var file = Directory.GetFiles(@"C:\Calendar", "*-2.txt").FirstOrDefault();
            using (StreamReader sr = File.OpenText(file))
            {
                string s;
                int sum = 0;
                var sums = new List<(int, int)>();
                while ((s = sr.ReadLine()) != null)
                {
                    var splits = s.Split(' ');
                    var firstPlayerBet = splits.FirstOrDefault();
                    var secondPlayerBet = splits.Skip(1).FirstOrDefault();

                    var firstBasePoints = firstPlayerBet switch
                    {
                        "A" => 1,
                        "B" => 2,
                        "C" => 3
                    };

                    var secondPlayerRealBet = (firstPlayerBet, secondPlayerBet) switch
                    {
                        ("A", "X") => "Z",
                        ("A", "Y") => "X",
                        ("A", "Z") => "Y",
                        ("B", "X") => "X",
                        ("B", "Y") => "Y",
                        ("B", "Z") => "Z",
                        ("C", "X") => "Y",
                        ("C", "Y") => "Z",
                        ("C", "Z") => "X",
                    };
                    
                    // X - Need to lose
                    // Y - Need to draw
                    // Z - need to win
                    var secondRealBasePoints = secondPlayerRealBet switch
                    {
                        "X" => 1,
                        "Y" => 2,
                        "Z" => 3
                    };

                    var winnerPoints = (firstPlayerBet, secondPlayerRealBet) switch
                    {
                        ("A", "Y") => (0, 6),
                        ("A", "X") => (3, 3),
                        ("A", "Z") => (6, 0),
                        ("B", "Y") => (3, 3),
                        ("B", "X") => (6, 0),
                        ("B", "Z") => (0, 6),
                        ("C", "Y") => (6, 0),
                        ("C", "X") => (0, 6),
                        ("C", "Z") => (3, 3)
                    };
                    
                    sums.Add((firstBasePoints + winnerPoints.Item1, secondRealBasePoints + winnerPoints.Item2));
                }

                _output.WriteLine(sums.Select(s => s.Item1).Sum().ToString());
                _output.WriteLine(sums.Select(s => s.Item2).Sum().ToString());
                
            }
        }
    }
}
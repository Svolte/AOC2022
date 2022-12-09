using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AOC2022.Solutions
{
    public class Day4
    {
        private readonly ITestOutputHelper _output;

        public Day4(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// In hindsight, why did I even bother making ranges out of everything, instead of comparing the extremes?
        /// </summary>
        [Fact]
        public void Run()
        {
            var file = Directory.GetFiles(@"C:\Calendar", "*-4.txt").FirstOrDefault();
            int sumContained = 0;
            using (StreamReader sr = File.OpenText(file))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    var pairs = s.Split(",");
                    var firstPairFrom = int.Parse(pairs[0].Split("-")[0]);
                    var firstPairTo = int.Parse(pairs[0].Split("-")[1]);
                    var secondPairFrom = int.Parse(pairs[1].Split("-")[0]);
                    var secondPairTo = int.Parse(pairs[1].Split("-")[1]);

                    var firstPair = Enumerable.Range(firstPairFrom, firstPairTo - firstPairFrom + 1).ToList();
                    var secondPair = Enumerable.Range(secondPairFrom, secondPairTo - secondPairFrom + 1).ToList();

                    var firstExcludedSecond = firstPair.Except(secondPair).ToList();
                    var secondExcludedFirst = secondPair.Except(firstPair).ToList();
                    
                    if (!firstExcludedSecond.Any() || !secondExcludedFirst.Any())
                    {
                        sumContained++;
                    }
                }

                _output.WriteLine(sumContained.ToString());
            }
        }
        
        /// <summary>
        /// Copy-paste from part 1, with the only difference being an intersect rather than an except.
        /// </summary>
        [Fact]
        public void Run_2()
        {
            var file = Directory.GetFiles(@"C:\Calendar", "*-4.txt").FirstOrDefault();
            int sumContained = 0;
            using (StreamReader sr = File.OpenText(file))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    var pairs = s.Split(",");
                    var firstPairFrom = int.Parse(pairs[0].Split("-")[0]);
                    var firstPairTo = int.Parse(pairs[0].Split("-")[1]);
                    var secondPairFrom = int.Parse(pairs[1].Split("-")[0]);
                    var secondPairTo = int.Parse(pairs[1].Split("-")[1]);

                    var firstPair = Enumerable.Range(firstPairFrom, firstPairTo - firstPairFrom + 1).ToList();
                    var secondPair = Enumerable.Range(secondPairFrom, secondPairTo - secondPairFrom + 1).ToList();

                    var firstHasOverlap = firstPair.Intersect(secondPair).Count() != 0;
                    var secondHasOverlap = secondPair.Intersect(firstPair).Count() != 0;
                    
                    if (firstHasOverlap || secondHasOverlap)
                    {
                        sumContained++;
                    }
                }

                _output.WriteLine(sumContained.ToString());
            }
        }
    }
}
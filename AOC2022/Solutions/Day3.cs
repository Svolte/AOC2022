using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AOC2022.Solutions
{
    public class Day3
    {
        private readonly ITestOutputHelper _output;

        public Day3(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// Compare two strings representing the contents of two compartments
        /// </summary>
        [Fact]
        public void Run()
        {
            var file = Directory.GetFiles(@"C:\Calendar", "*-3.txt").FirstOrDefault();
            using (StreamReader sr = File.OpenText(file))
            {
                int sum = 0;
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    var length = s.Length;
                    var itemsInFirstCompartment = s.Substring(0, length / 2);
                    var itemsInSecondCompartment = s.Substring(length / 2, length / 2);

                    var commonChars = itemsInFirstCompartment.Intersect(itemsInSecondCompartment);

                    foreach (var character in commonChars)
                    {
                        if (character >= 97)
                        {
                            sum += character - 96;
                        }
                        else
                        {
                            sum += character - 38;
                        }
                    }
                }

                _output.WriteLine(sum.ToString());
            }
        }

        [Fact]
        public void Run_2()
        {
            var completeString = Helpers.ReadTextFile("*-3.txt");

            int sum = 0;
            var splits = completeString.Split(Environment.NewLine);
            foreach (var group in splits.Batch(3))
            {
                if (group.FirstOrDefault() == "") continue;
                // Lol, forgive me for this
                var groupList = group.ToList();
                var commonCharacter = groupList[0].Intersect(groupList[1]).Intersect(groupList[2]).FirstOrDefault();

                if (commonCharacter >= 97)
                {
                    sum += commonCharacter - 96;
                }
                else
                {
                    sum += commonCharacter - 38;
                }
            }

            _output.WriteLine(sum.ToString());
        }
    }
}
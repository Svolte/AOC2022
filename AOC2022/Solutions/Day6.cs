using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AOC2022.Solutions
{
    public class Day6
    {
        private readonly ITestOutputHelper _output;

        public Day6(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Run()
        {
            var file = Helpers.ReadTextFile("*-6.txt");
            var seen = new List<char>();
            var i = 0;
            var endIndex = 0;
            while (seen.Count < 4)
            {
                seen.AddRange(file.Skip(i).Take(4));
                i++;
                if (seen.GroupBy(s => s).Count() < 4)
                {
                    seen = new List<char>();
                }

                endIndex = i + 4 - 1;
            }


            _output.WriteLine(endIndex.ToString());
        }
        
        [Fact]
        public async Task Run_2()
        {
            var file = Helpers.ReadTextFile("*-6.txt");
            var seen = new List<char>();
            var i = 0;
            var endIndex = 0;
            while (seen.Count < 14)
            {
                seen.AddRange(file.Skip(i).Take(14));
                i++;
                if (seen.GroupBy(s => s).Count() < 14)
                {
                    seen = new List<char>();
                }
                endIndex = i + 14 - 1;
            }


            _output.WriteLine(endIndex.ToString());
        }
    }
}
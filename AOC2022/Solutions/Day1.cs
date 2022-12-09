using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AOC2022.Solutions
{
    public class Day1
    {
        private readonly ITestOutputHelper _output;

        public Day1(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>Outputs the sum of each Elves Calories on a new line in the output. Excel was used to
        /// sort the resulting values by descending to get the sum of the top three Calory carrying Elves (part 2).
        /// (Ugly, but quicker than rewriting the code to account for this ...)</summary>
        [Fact]
        public void Run()
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
    }
}
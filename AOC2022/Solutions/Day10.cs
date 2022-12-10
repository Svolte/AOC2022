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
    public class Day10
    {
        private readonly ITestOutputHelper _output;
        private List<List<string>> crtPrint = new List<List<string>>();
        private List<string> currentCrtLine = new List<string>();
        private int cycle = 1;

        public Day10(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Run()
        {
            var cycleHistory = GetCycles();

            var interestingSignals = cycleHistory.Where(c =>
                c.CycleNumber == 20 || c.CycleNumber == 60 || c.CycleNumber == 100 || c.CycleNumber == 140 ||
                c.CycleNumber == 180 || c.CycleNumber == 220).ToList();
            _output.WriteLine(
                $"Sum of interesting signals is {interestingSignals.Sum(s => s.SignalStrength).ToString()}");
        }

        private List<Cycle> GetCycles()
        {
            var file = Helpers.ReadTextFile("*-10.txt");
            var x = 1;
            var cycleHistory = new List<Cycle>();
            var spritePosition = 0;
            foreach (var line in file.Split(Environment.NewLine))
            {
                var noop = line.Split("noop");
                var addx = line.Split("addx");

                if (noop.Length > 1)
                {
                    cycleHistory.Add(new Cycle { CycleNumber = cycle, XValue = x });
                    currentCrtLine.Add(GetPixel(cycle, x, crtPrint.Count));
                    AddCycle();
                }

                if (addx.Length > 1)
                {
                    cycleHistory.Add(new Cycle { CycleNumber = cycle, XValue = x });
                    currentCrtLine.Add(GetPixel(cycle, x, crtPrint.Count));
                    AddCycle();

                    cycleHistory.Add(new Cycle { CycleNumber = cycle, XValue = x });
                    currentCrtLine.Add(GetPixel(cycle, x, crtPrint.Count));
                    x += int.Parse(addx.Skip(1).FirstOrDefault());
                    spritePosition = x;
                    AddCycle();
                }
            }

            // _output.WriteLine(crtPrint);
            foreach (var list in crtPrint)
            {
                Console.WriteLine(string.Join("", list.Select(l => l)));
            }
            return cycleHistory;
        }

        private string GetPixel(int cycle, int spritePosition, int row)
        {
            var localCycle = cycle - 1;
            if (localCycle >= 40) localCycle = localCycle - (40 * row);
            
            if (spritePosition - 1 == localCycle) return "#";
            if (spritePosition == localCycle) return "#";
            if (spritePosition + 1 == localCycle) return "#";

            return ".";
        }

        private void AddCycle()
        {
            if (cycle % 40 == 0)
            {
                crtPrint.Add(currentCrtLine);
                currentCrtLine = new List<string>();
            }

            cycle++;
        }
        
        [Fact]
        public void Test()
        {
            _output.WriteLine((40 % 40).ToString());
            _output.WriteLine((41 % 40).ToString());
            _output.WriteLine((42 % 40).ToString());
            _output.WriteLine((80 % 40).ToString());
            _output.WriteLine((1 % 40).ToString());
            _output.WriteLine((39 % 40).ToString());
        }
    }

    public class Cycle
    {
        public int CycleNumber { get; set; }
        public int XValue { get; set; }
        public int SignalStrength => CycleNumber * XValue;
    }

}
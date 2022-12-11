using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AOC2022.Solutions
{
    public class Day11
    {
        private readonly ITestOutputHelper _output;
        private static int _modulus = 1;

        public Day11(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Run()
        {
            var monkeys = new List<Monkey>();
            var rounds = 10000;
            var input = Helpers.ReadTextFile("*-11.txt");
            foreach (var monkeyLines in input.Split(Environment.NewLine).Batch(7))
            {
                var monkey = CreateMonkey(monkeyLines.ToList());
                monkeys.Add(monkey);
            }

            foreach (var monkey in monkeys)
            {
                _modulus *= monkey.Divisor;
            }

            for (int i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    if (monkey.Items.Count == 0) continue;
                    while (monkey.Items.Any())
                    {
                        var (worryLevel, nextMonkey) = monkey.NextOperation();
                        var monkeyToCatch = monkeys.FirstOrDefault(m => m.Name == nextMonkey);
                        monkeyToCatch.Items.Add(worryLevel);
                        monkey.Items.RemoveAt(0);
                        monkey.NumberOfInspections++;
                    }
                }
            }

            var monkeyBusiness = monkeys.OrderByDescending(m => m.NumberOfInspections)
                .Select(m => m.NumberOfInspections).Take(2);
            var firstMonkey = monkeyBusiness.FirstOrDefault();
            var secondMonkey = monkeyBusiness.Skip(1).FirstOrDefault();
            _output.WriteLine(
                $"Monkey business: {(firstMonkey * secondMonkey).ToString()}");
        }

        public Monkey CreateMonkey(List<string> monkeyLines)
        {
            var monkey = new Monkey();
            var name = int.Parse(monkeyLines[0][7].ToString());
            var startingItems = monkeyLines.Skip(1).FirstOrDefault().Split("Starting items: ").Skip(1).FirstOrDefault()
                .Split(", ").Select(l => long.Parse(l)).ToList();
            monkey.Name = name;
            monkey.Items = startingItems;

            var operationLine = monkeyLines.Skip(2).FirstOrDefault();
            if (operationLine.Contains("* old"))
            {
                monkey.IsSelfMultiplier = true;
            }
            else
            {
                var multiplierSplit = operationLine.Split("* ").Skip(1).FirstOrDefault();
                if (multiplierSplit != null)
                {
                    monkey.Multiplier = int.Parse(multiplierSplit);
                }

                var additionSplit = operationLine.Split("+ ").Skip(1).FirstOrDefault();
                if (additionSplit != null)
                {
                    monkey.Addition = int.Parse(additionSplit);
                }
            }

            var divisorLine = monkeyLines.Skip(3).FirstOrDefault();
            monkey.Divisor = int.Parse(divisorLine.Split("divisible by ").Skip(1).FirstOrDefault());

            monkey.FirstMonkey = int.Parse(monkeyLines[4][29].ToString());
            monkey.SecondMonkey = int.Parse(monkeyLines[5][30].ToString());

            return monkey;
        }

        public class Monkey
        {
            public int Name { get; set; }
            public List<long> Items { get; set; }
            public long NumberOfInspections { get; set; }
            public int Divisor { get; set; }
            public int? Addition { get; set; }
            public int? Multiplier { get; set; }
            public int FirstMonkey { get; set; }
            public int SecondMonkey { get; set; }
            public bool ShouldUseMultiplication => Multiplier != null;
            public bool ShouldUseAddition => Addition != null;
            public bool IsSelfMultiplier { get; set; } = false;

            public (long WorryLevel, int NextMonkey) NextOperation()
            {
                var item = Items.FirstOrDefault();
                var worryLevel = ((ShouldUseMultiplication
                                     ? item * Multiplier!.Value
                                     : 0) +
                                 (ShouldUseAddition
                                     ? item + Addition!.Value
                                     : 0) +
                                 (IsSelfMultiplier
                                     ? item * item
                                     : 0)) % _modulus;
                if (worryLevel < 0)
                {
                    Console.WriteLine("wtf");
                    var asd = worryLevel % _modulus;
                    var asd2 = "";
                }
                var nextMonkey = worryLevel % _modulus % Divisor == 0 ? FirstMonkey : SecondMonkey;
                return (worryLevel, nextMonkey);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace AOC2022.Solutions
{
    public class Day11
    {
        private readonly ITestOutputHelper _output;

        public Day11(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Run()
        {
            var monkeys = new List<Monkey>();
            var rounds = 1000;
            var input = Helpers.ReadTextFile("*-11.txt");
            foreach (var monkeyLines in input.Split(Environment.NewLine).Batch(7))
            {
                var monkey = CreateMonkey(monkeyLines);
                monkeys.Add(monkey);
            }

            for (int i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    if (monkey.Items.Count == 0) continue;
                    while (monkey.Items.Any())
                    {
                        monkey.NumberOfInspections++;
                        var itemToThrow = monkey.NextOperation;
                        var monkeyToCatch = monkeys.FirstOrDefault(m => m.Name == itemToThrow.NextMonkey);
                        monkeyToCatch.Items.Add(itemToThrow.WorryLevel!.Value);
                        monkey.Items.RemoveAt(0);
                    }
                }
                if (i == 9999) Console.WriteLine("9999");
            }

            var monkeyBusiness = monkeys.OrderByDescending(m => m.NumberOfInspections)
                .Select(m => m.NumberOfInspections).Take(2);
            var firstMonkey = monkeyBusiness.FirstOrDefault();
            var secondMonkey = monkeyBusiness.Skip(1).FirstOrDefault();
            _output.WriteLine(
                $"First monkey count: {firstMonkey.ToString()}");
            _output.WriteLine(
                $"Second monkey count: {secondMonkey.ToString()}");
            _output.WriteLine(
                $"Monkey business: {(firstMonkey * secondMonkey).ToString()}");
        }

        public Monkey CreateMonkey(IEnumerable<string> monkeyLines)
        {
            var monkey = new Monkey();
            foreach (var line in monkeyLines)
            {
                if (line.StartsWith("Monkey")) monkey.Name = int.Parse(line.Substring(7, 1));
                if (line.Trim().StartsWith("Starting"))
                {
                    monkey.Items = line.Split("Starting items: ").Skip(1).FirstOrDefault().Split(", ")
                        .Select(s => BigInteger.Parse(s)).ToList();
                    // monkey.Items = longList.Select(l => (long?) l).ToList();
                }
            }

            return monkey;
        }

        public class Monkey
        {
            public int Name { get; set; }
            public List<BigInteger> Items { get; set; }
            //public long? CurrentItem => Items.FirstOrDefault();
            public BigInteger NumberOfInspections { get; set; }

            public (BigInteger? WorryLevel, int NextMonkey) NextOperation
            {
                get
                {
                    if (Name == 0)
                    {
                        var worryLevel = Items.FirstOrDefault() * 19;
                        var nextMonkey = worryLevel % 23 == 0 ? 2 : 3;
                        return (worryLevel, nextMonkey);
                    }
                    if (Name == 1)
                    {
                        var worryLevel = Items.FirstOrDefault() + 6;
                        var nextMonkey = worryLevel % 19 == 0 ? 2 : 0;
                        return (worryLevel, nextMonkey);
                    }
                    if (Name == 2)
                    {
                        var worryLevel = Items.FirstOrDefault() * Items.FirstOrDefault();
                        var nextMonkey = worryLevel % 13 == 0 ? 1 : 3;
                        return (worryLevel, nextMonkey);
                    }
                    if (Name == 3)
                    {
                        var worryLevel = Items.FirstOrDefault() + 3;
                        var nextMonkey = worryLevel % 17 == 0 ? 0 : 1;
                        return (worryLevel, nextMonkey);
                    }
                    // if (Name == 0)
                    // {
                    //     var worryLevel = Items.FirstOrDefault() * 11;
                    //     var nextMonkey = worryLevel % 13 == 0 ? 1 : 7;
                    //     return (worryLevel, nextMonkey);
                    // }
                    //
                    // if (Name == 1)
                    // {
                    //     var worryLevel = (Items.FirstOrDefault() + 1);
                    //     var nextMonkey = worryLevel % 7 == 0 ? 3 : 6;
                    //     return (worryLevel, nextMonkey);
                    // }
                    //
                    // if (Name == 2)
                    // {
                    //     var worryLevel = (Items.FirstOrDefault() * Items.FirstOrDefault());
                    //     var nextMonkey = worryLevel % 3 == 0 ? 5 : 4;
                    //     return (worryLevel, nextMonkey);
                    // }
                    //
                    // if (Name == 3)
                    // {
                    //     var worryLevel = (Items.FirstOrDefault() + 2);
                    //     var nextMonkey = worryLevel % 19 == 0 ? 2 : 6;
                    //     return (worryLevel, nextMonkey);
                    // }
                    //
                    // if (Name == 4)
                    // {
                    //     var worryLevel = (Items.FirstOrDefault() + 6);
                    //     var nextMonkey = worryLevel % 5 == 0 ? 0 : 5;
                    //     return (worryLevel, nextMonkey);
                    // }
                    //
                    // if (Name == 5)
                    // {
                    //     var worryLevel = (Items.FirstOrDefault() + 7);
                    //     var nextMonkey = worryLevel % 2 == 0 ? 7 : 0;
                    //     return (worryLevel, nextMonkey);
                    // }
                    //
                    // if (Name == 6)
                    // {
                    //     var worryLevel = (Items.FirstOrDefault() * 7);
                    //     var nextMonkey = worryLevel % 11 == 0 ? 2 : 4;
                    //     return (worryLevel, nextMonkey);
                    // }
                    //
                    // if (Name == 7)
                    // {
                    //     var worryLevel = (Items.FirstOrDefault() + 8);
                    //     var nextMonkey = worryLevel % 17 == 0 ? 1 : 3;
                    //     return (worryLevel, nextMonkey);
                    // }

                    throw new NotImplementedException();
                }
            }
        }
    }
}
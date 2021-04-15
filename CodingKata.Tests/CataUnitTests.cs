using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingKata.Tests
{
    public class Tests
    {
        readonly BerlinClockTimeFormatter formatter = new BerlinClockTimeFormatter();

        [DatapointSource]
        public KeyValuePair<string, int>[] hours = new KeyValuePair<string, int>[]
       {
            new KeyValuePair<string, int>("OOOO\r\nOOOO\r\n", 0), new KeyValuePair<string, int>("OOOO\r\nROOO\r\n", 1), new KeyValuePair<string, int>("OOOO\r\nRROO\r\n", 2), new KeyValuePair<string, int>("OOOO\r\nRRRO\r\n", 3),new KeyValuePair<string, int>( "OOOO\r\nRRRR\r\n", 4),
            new KeyValuePair<string, int>("ROOO\r\nOOOO\r\n", 5), new KeyValuePair<string, int>("ROOO\r\nROOO\r\n", 6), new KeyValuePair<string, int>("ROOO\r\nRROO\r\n", 7), new KeyValuePair<string, int>("ROOO\r\nRRRO\r\n", 8),new KeyValuePair<string, int>( "ROOO\r\nRRRR\r\n", 9),
            new KeyValuePair<string, int>("RROO\r\nOOOO\r\n", 10), new KeyValuePair<string, int>("RROO\r\nROOO\r\n", 11), new KeyValuePair<string, int>("RROO\r\nRROO\r\n", 12), new KeyValuePair<string, int>("RROO\r\nRRRO\r\n", 13),new KeyValuePair<string, int>( "RROO\r\nRRRR\r\n", 14),
            new KeyValuePair<string, int>("RRRO\r\nOOOO\r\n", 15), new KeyValuePair<string, int>("RRRO\r\nROOO\r\n", 16), new KeyValuePair<string, int>("RRRO\r\nRROO\r\n", 17),new KeyValuePair<string, int>( "RRRO\r\nRRRO\r\n", 18),new KeyValuePair<string, int>( "RRRO\r\nRRRR\r\n", 19),
            new KeyValuePair<string, int>("RRRR\r\nOOOO\r\n", 20), new KeyValuePair<string, int>("RRRR\r\nROOO\r\n", 21), new KeyValuePair<string, int>("RRRR\r\nRROO\r\n", 22),new KeyValuePair<string, int>( "RRRR\r\nRRRO\r\n", 23)
       };

        [Theory]
        public void TestHours(KeyValuePair<string, int> hour)
        {
            var dateTime = new DateTimeOffset(2021, 1, 1, hour.Value, 0, 0, 0, TimeSpan.Zero);
            var berlinTime = new BerlinClockTime(dateTime);
            var formatted = berlinTime.ToString("H", formatter);
            Assert.AreEqual(hour.Key, formatted);
        }

        [Test]
        public void TestSeconds()
        {
            var dateTime = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var berlinTime = new BerlinClockTime(dateTime);
            var formatted = berlinTime.ToString("S", formatter);
            Assert.AreEqual("Y\r\n", formatted);


            var dateTimeNonBlinking = new DateTimeOffset(2021, 1, 1, 0, 0, 1, TimeSpan.Zero);
            var berlinTimeNonBlinking = new BerlinClockTime(dateTimeNonBlinking);
            var formattedNonBlinking = berlinTimeNonBlinking.ToString("S", formatter);
            Assert.AreEqual("O\r\n", formattedNonBlinking);
        }

        [Test]
        public void TestSingleMinutes()
        {
            for (int i = 0; i < 60; i++)
            {
                var dateTime = new DateTimeOffset(2021, 1, 1, 0, i, 0, TimeSpan.Zero);
                var berlinTime = new BerlinClockTime(dateTime);
                var formatted = berlinTime.ToString("M", formatter);
                var lastLine = formatted.Split("\r\n").Skip(1).First();
                Assert.AreEqual(lastLine.Length, 4);
                var activeCount = lastLine.Count(x => x == 'Y');
                Assert.AreEqual(activeCount, i % 5);
                Assert.AreEqual(lastLine.Count(x => x == 'O'), 4-activeCount);
            }
        }

        [Test]
        public void TestQuartersAndFiveMinutes()
        {
            for (int i = 0; i < 60; i++)
            {
                var dateTime = new DateTimeOffset(2021, 1, 1, 0, i, 0, TimeSpan.Zero);
                var berlinTime = new BerlinClockTime(dateTime);
                var formatted = berlinTime.ToString("M", formatter);
                var lastLine = formatted.Split("\r\n").First();
                Assert.AreEqual(lastLine.Length, 11);
                var redCount = lastLine.Count(x => x == 'R');
                Assert.AreEqual(redCount, i / 15);
                var yellowCount = lastLine.Count(x => x == 'Y');
                Assert.AreEqual(yellowCount, i / 5 - i / 15);
                var activeCount = lastLine.Count(x => x != 'O');
                Assert.AreEqual(lastLine.Count(x => x == 'O'), 11 - activeCount);
            }
        }
    }
}
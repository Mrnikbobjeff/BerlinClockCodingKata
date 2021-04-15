using System;
using static CodingKata.BerlinClockTimeFormatter;

namespace CodingKata
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTimeOffset time = DateTimeOffset.MinValue;
            while(time == DateTimeOffset.MinValue)
            {
                try
                {
                    Console.WriteLine("Enter a time to see in Berlin Clock Format. Input format: HH:MM:SS");
                    time = DateTimeOffset.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Error parsing the input string. Format should be HH:MM:SS");
                }
            }
            var berlinTime = new BerlinClockTime(time);
            Console.WriteLine(berlinTime.ToString("F", new BerlinClockTimeFormatter()));
        }
    }
}

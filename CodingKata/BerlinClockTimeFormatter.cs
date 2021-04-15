using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodingKata
{
    static class LinqExtensions
    {
        public static string TryAggregate(this IEnumerable<string> values)
        {
            if (!values.Any())
            {
                return string.Empty;
            }
            return values.Aggregate((x, y) => x + y);
        }
    }

    public class BerlinClockTimeFormatter : IFormatProvider, ICustomFormatter
    {
        StringBuilder AppendQuarters(StringBuilder sb, BerlinClockTime clock)
        {
            sb.AppendLine(Enumerable.Repeat("YYR", clock.Quarters).Concat(Enumerable.Repeat("Y", clock.FiveMinutes)).TryAggregate().PadRight(11, 'O'));
            return sb;
        }

        StringBuilder AppendSingleMinutes(StringBuilder sb, BerlinClockTime clock)
        {
            sb.AppendLine(Enumerable.Repeat("Y", clock.SingleMinutes).TryAggregate().PadRight(4, 'O'));
            return sb;
        }

        StringBuilder AppendSeconds(StringBuilder sb, BerlinClockTime clock)
        {
            sb.AppendLine(clock.IsTwoSecondClockBlinking ? "Y" : "O");
            return sb;
        }

        StringBuilder AppendHours(StringBuilder sb, BerlinClockTime clock)
        {
            sb.AppendLine(Enumerable.Repeat("R", clock.NumberOfFiveHourSignals).TryAggregate().PadRight(4, 'O'));
            sb.AppendLine(Enumerable.Repeat("R", clock.NumberOfOneHourSignals).TryAggregate().PadRight(4, 'O'));
            return sb;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!(arg is BerlinClockTime clock))
                throw new InvalidOperationException();

            var sb = new StringBuilder();

            switch (format)
            {
                case "F":
                    AppendSeconds(sb, clock);
                    AppendHours(sb, clock);
                    AppendQuarters(sb, clock);
                    AppendSingleMinutes(sb, clock);
                    break;
                case "H":
                    AppendHours(sb, clock);
                    break;
                case "M":
                    AppendQuarters(sb, clock);
                    AppendSingleMinutes(sb, clock);
                    break;
                case "S":
                    AppendSeconds(sb, clock);
                    break;
                default:
                    throw new FormatException(format);
            }

            return sb.ToString();
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        public class BerlinClockTime : IFormattable
        {
            public BerlinClockTime(DateTimeOffset offset)
            {
                // when the second is odd the light is off
                IsTwoSecondClockBlinking = offset.Second % 2 == 0;

                // How many five hour blocks exist
                NumberOfFiveHourSignals = offset.Hour / 5;
                //How many remaining single hour blocks exist
                NumberOfOneHourSignals = offset.Hour - NumberOfFiveHourSignals * 5;

                //How Many 15 minute blocks have passed
                Quarters = offset.Minute / 15;
                //How Many remaining five minute blocks are there
                FiveMinutes = (offset.Minute - 15 * Quarters) / 5;
                //How many single minute blocks are remaining
                SingleMinutes = offset.Minute % 5;
            }

            public bool IsTwoSecondClockBlinking { get; }
            public int NumberOfFiveHourSignals { get; }
            public int NumberOfOneHourSignals { get; }
            public int Quarters { get; }
            public int FiveMinutes { get; }
            public int SingleMinutes { get; }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                var formatter = (formatProvider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter);
                return formatter.Format(format, this, formatProvider);
            }
        }
    }
}

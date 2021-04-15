using System;
using System.Linq;
using System.Text;

namespace CodingKata
{
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
    }
}

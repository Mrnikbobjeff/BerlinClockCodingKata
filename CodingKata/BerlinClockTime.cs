using System;

namespace CodingKata
{
    public readonly struct BerlinClockTime : IFormattable, IEquatable<BerlinClockTime>
    {
        public DateTimeOffset Offset { get; }

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
            Offset = offset;
        }

        public bool IsTwoSecondClockBlinking { get; }
        public int NumberOfFiveHourSignals { get; }
        public int NumberOfOneHourSignals { get; }
        public int Quarters { get; }
        public int FiveMinutes { get; }
        public int SingleMinutes { get; }

        public bool Equals(BerlinClockTime other)
        {
            return Offset.Equals(other);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var formatter = (formatProvider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter);
            return formatter.Format(format, this, formatProvider);
        }
    }
}

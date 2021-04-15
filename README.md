# BerlinClockCodingKata
[![.NET](https://github.com/Mrnikbobjeff/BerlinClockCodingKata/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Mrnikbobjeff/BerlinClockCodingKata/actions/workflows/dotnet.yml)

##Extra thoughts:
Code duplication can be reduced even more in formatter by creating an appendRepeatAndPad method which ould be used by hour and single minute formatter methods.
String size is known beforehad from format specifier and platform. for maximum efficiency remove link parts and add a manual counter to set characters based on offset. Requires only one allocation in total for the string.

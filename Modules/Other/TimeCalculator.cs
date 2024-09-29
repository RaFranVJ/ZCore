using System;

namespace ZCore.Modules.Other
{
/// <summary> Initializes calculating Functions for Time Values (such as DateTimes and TimeStamps). </summary>

public static class TimeCalculator
{
/// <summary> The epoch Time used as a Reference when Parsing DateTimes. </summary>

private static readonly DateTime epochTime = DateTime.Parse("1970/1/1");

/** <summary> Calculates a DateTime from a given TimeStamp Value. </summary>
<param name = "timeStamp"> The TimeStamp where the DateTime will be Calculated from. </param>

<exception cref = "ArgumentOutOfRangeException"></exception>
<exception cref = "OverflowException"></exception>

<returns> The DateTime Calculated. </returns> */

public static DateTime CalculateDateTime(double timeStamp) => epochTime.AddSeconds(timeStamp);

// Method variation Exclusive for JS

public static object CalculateDateTimeJS(string arg) => CalculateDateTime( InputHelper.FilterNumber<double>(arg) );

/** <summary> Calculates a TimeStamp from a given DateTime Value. </summary>
<param name = "dateTime"> The DateTime where the TimeStamp will be Calculated from. </param>

<exception cref = "ArgumentOutOfRangeException"></exception>
<returns> The TimeStamp Calculated. </returns> */

public static double CalculateTimeStamp(DateTime dateTime) => Math.Truncate(dateTime.Subtract(epochTime).TotalSeconds);

// Method variation Exclusive for JS

public static object CalculateTimeStampJS(string arg) => CalculateTimeStamp( InputHelper.FilterDateTime(arg) );
}

}
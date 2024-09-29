using System;
using System.IO;

namespace ZCore.Modules.Other
{
/// <summary> Initializes calculating Functions for VarInt and Integer Values. </summary>

public static class VarIntCalculator
{
/** <summary> Calculates an Integer from a given VarInt Value. </summary>

<param name = "targetValue"> The VarInt where the Integer will be Calculated from. </param>
<param name = "treatValueAsSigned"> Determines if the Integer calculated should be treated as Signed or not. </param>

<exception cref = "ArgumentOutOfRangeException"></exception>
<exception cref = "EndOfStreamException"></exception>
<exception cref = "InvalidOperationException"></exception>
<exception cref = "OverflowException"></exception>

<returns> The Integer Calculated. </returns> */

public static int CalculateInt(int targetValue, bool treatValueAsSigned)
{
using MemoryStream memoryBuffers = new();
byte[] varIntBytes = BitConverter.GetBytes(targetValue);

memoryBuffers.Write(varIntBytes, 0, varIntBytes.Length);
memoryBuffers.Position = 0;

using BinaryReader bufferReader = new(memoryBuffers);
int integerValue = bufferReader.Read7BitEncodedInt();

if(treatValueAsSigned)
{
int logicValue = integerValue & 1;
int squareSuplement = integerValue / 2;

integerValue = (logicValue != 0) ? (squareSuplement + 1) * -1 : squareSuplement;
}

return integerValue;
}

// Method for JS

public static object CalculateIntJS(string arg, string arg2) 
{
int vInt = InputHelper.FilterNumber<int>(arg);

if(!bool.TryParse(arg2, out bool isSigned) )
isSigned = default;

return CalculateInt(vInt, isSigned);
}

/** <summary> Calculates a VarInt from a given Integer Value. </summary>

<param name = "targetValue"> The Integer where the VarInt will be Calculated from. </param>
<param name = "treatValueAsSigned"> Determines if the Integer entered by the User should be treated as Signed or not. </param>

<exception cref = "OverflowException"></exception>
<returns> The Varint Value Calculated. </returns> */

public static int CalculateVarInt(int targetValue, bool treatValueAsSigned)
{
using MemoryStream memoryBuffers = new();
using BinaryWriter bufferWriter = new(memoryBuffers);

if(treatValueAsSigned)
{
int logicValue = targetValue & 1;
int squareComplement = targetValue * 2;

targetValue = (logicValue != 0) ? (squareComplement - 1) : squareComplement;
}

bufferWriter.Write7BitEncodedInt(targetValue);
byte[] integerBytes = memoryBuffers.ToArray();

int expectedLength = 4;

if(integerBytes.Length < expectedLength)
InputHelper.FillArray(ref integerBytes, expectedLength);

return BitConverter.ToInt32(integerBytes, 0);
}

// Method for JS

public static object CalculateVarIntJS(string arg, string arg2)
{
int dInt = InputHelper.FilterNumber<int>(arg);

if(!bool.TryParse(arg2, out bool isSigned) )
isSigned = default;

return CalculateVarInt(dInt, isSigned);
}

}

}
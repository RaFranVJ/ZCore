using System;

namespace ZCore.Modules.Other
{
/// <summary> Initializes Base64 Parse Tasks for Strings. </summary>

public static class Base64StringParser
{
/** <summary> Encodes an Array of Bytes as a Base64 String. </summary>

<param name = "inputBytes"> The Bytes to be Encoded. </param>
<param name = "isWebSafe"> A boolean that Determines if the Base64 string will be Generated as a Web Safe string or not. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FormatException"></exception>
<exception cref = "OutOfMemoryException"></exception>

<returns> The Base64 String. </returns> */

public static string EncodeBytes(byte[] inputBytes, bool isWebSafe)
{
string encodedString = Convert.ToBase64String(inputBytes);

if(isWebSafe)
encodedString = encodedString.TrimEnd('=').Replace('+', '-').Replace('/', '_'); 

return encodedString;
}

// Method for JS

public static object EncodeBytesJS(string arg, string arg2)
{
byte[] data = Console.InputEncoding.GetBytes(arg);

if(!bool.TryParse(arg2, out bool isWebSafe) )
isWebSafe = default;

return EncodeBytes(data, isWebSafe);
}

/** <summary> Decodes a Base64 String as an Array of Bytes. </summary>

<param name = "inputString"> The String to be Decoded. </param>
<param name = "isWebSafe"> A boolean that Determines if the Base64 string was Generated as a Web Safe string or not. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FormatException"></exception>
<exception cref = "OutOfMemoryException"></exception>

<returns> The Bytes Decoded. </returns> */

public static byte[] DecodeString(string inputString, bool isWebSafe)
{
	
if(isWebSafe)
{
inputString = inputString.Replace('_', '/').Replace('-', '+');
int trailingSum = inputString.Length % 4;

switch(trailingSum)
{
case 2:
inputString += '=' + '=';
break;

case 3:
inputString += '=';
break;
}

}

return Convert.FromBase64String(inputString);
}

// Method for JS

public static object DecodeStringJS(string arg, string arg2)
{

if(!bool.TryParse(arg2, out bool isWebSafe) )
isWebSafe = default;

return DecodeString(arg, isWebSafe);
}

}

}
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using ZCore.Modules.Other;
using ZCore.Serializables.ArgumentsInfo.FileSecurity.Integrity;

namespace ZCore.Modules.SexyCompressors.StandarMediaFile
{
/// <summary> Stores Info related that Specify how a SMF File was Compressed. </summary>

public class StandarMediaInfo : MetaModel<StandarMediaInfo>
{
/** <summary> Gets or Sets the Size of a <c>StandarMediaFile</c> before Compression. </summary>
<returns> The Size before Compression. </returns> */

public byte[] HexSizeBeforeCompression{ get; set; }

/** <summary> Gets or Sets Info about the <c>CompressionLevel</c> used for Compressing the Data. </summary>
<returns> The Compression Flags. </returns> */

public ushort CompressionFlags{ get; set; }

/** <summary> Gets or Sets the Adler32 Bytes of a <c>StandarMediaFile</c>. </summary>
<returns> The Adler32 Bytes. </returns> */

public byte[] Adler32Bytes{ get; set; }

/// <summary> Creates a new Instance of the <c>StandarMediaInfo</c>. </summary>

public StandarMediaInfo()
{
using MemoryStream defaultStream = new();
HexSizeBeforeCompression = GetSizeInfo(defaultStream);

CompressionFlags = GetCompressionFlags();
Adler32Bytes = Adler32BytesChecksum.GetAdler32Bytes(defaultStream);
}

/** <summary> Creates a new Instance of the <c>StandarMediaInfo</c> with the given Parameters. </summary>

<param name = "targetStream"> The Stream to be Read. </param>
<param name = "compressionLvl"> The <c>CompressionLevel</c> to be Used. </param> */

public StandarMediaInfo(Stream targetStream, int hexSize, CompressionLevel compressionLvl, Adler32BytesInfo adler32Cfg)
{
HexSizeBeforeCompression = GetSizeInfo(targetStream, hexSize);
CompressionFlags = GetCompressionFlags(compressionLvl);

Adler32Bytes = Adler32BytesChecksum.GetAdler32Bytes(targetStream, adler32Cfg);
}

/** <summary> Gets a Bytes Array from an Hexadecimal String and Flips its Bytes after Splitting. </summary>

<param name = "plainSize"> The Size of the File expressed in a Decimal Base. </param>

<returns> The Hexadecimal Bytes. </returns> */

private static byte[] GetHexBytes(long plainSize)
{
string hexString = plainSize.ToString("x2");

if(hexString.Length % 2 != 0)
hexString = "0" + hexString;

byte[] hexBytes = new byte[hexString.Length / 2];

for(int i = 0; i < hexBytes.Length; i++)
{
string hexPair = hexString.Substring(i * 2, 2);
hexBytes[i] = byte.Parse(hexPair, NumberStyles.HexNumber);
}

Array.Reverse(hexBytes);

return hexBytes;
}

/** <summary> Gets the Size of a Stream as an Hexadecimal Bytes Array. </summary>

<param name = "targetStream"> The Stream to be Analized. </param>
<param name = "bytesCount"> The Number of Bytes to be Analized (Default is 4). </param>

<returns> The Size Info. </returns> */

public static byte[] GetSizeInfo(Stream targetStream, int bytesCount = 4) 
{
byte[] hexBytes = GetHexBytes(targetStream.Length);
byte[] sizeInfo = new byte[bytesCount];

if(hexBytes.Length > sizeInfo.Length)
throw new DataMisalignedException("The Number of Hexadecimal Bytes is Higher than the Size of your File.");

Array.Copy(hexBytes, sizeInfo, hexBytes.Length);

return sizeInfo;
}

/** <summary> Gets the Original Size of a Stream by using an Hexadecimal Bytes Array. </summary>

<param name = "hexBytes"> The Hex Bytes to Analize. </param>

<returns> The Plain Size. </returns> */

public static long GetPlainSize(byte[] hexBytes)
{
Array.Reverse(hexBytes);

string hexString = BitConverter.ToString(hexBytes).Replace("-", "");

return long.Parse(hexString, NumberStyles.HexNumber);
}

/** <summary> Gets the Compression Info of a SMF File based on the CompressionLevel used. </summary>

<param name = "compressionLvl"> The CompressionLevel used. </param>

<returns> The Compression Flags. </returns> */

public static ushort GetCompressionFlags(CompressionLevel compressionLvl = default)
{

return compressionLvl switch
{
CompressionLevel.Fastest => 40056, // 0x9C 0x38
CompressionLevel.Optimal => 55928, // 0x98 0xDA
_ => 376 // 0x78 0x01
};

}

}

}
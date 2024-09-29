using System;
using System.IO;
using System.Text;

namespace ZCore.Modules.SexyCompressors.ArcVPackage
{
/// <summary> A useful Class used for Handling the Info inside a ARCV Stream. </summary>

public static class ArcvHelper
{
// Convert Res Name into Bytes

private static byte[] GetBinaryName(string resPath)
{
string resName = Path.GetFileNameWithoutExtension(resPath);
byte[] binaryName = Encoding.ASCII.GetBytes(resName);

InputHelper.FillArray(ref binaryName, 8);

if(BitConverter.IsLittleEndian)
Array.Reverse(binaryName);

return binaryName;
}

// Convert Binary Name into String

private static string GetPlainName(long crc32)
{
byte[] binaryName = BitConverter.GetBytes(crc32);

if(BitConverter.IsLittleEndian)
Array.Reverse(binaryName);
    
return Encoding.ASCII.GetString(binaryName);
}

// Get CRC32 Checksum

public static long GetCRC32(string path) => BitConverter.ToInt64( GetBinaryName(path) );

// Convert CRC32 into String and Apply Padding if Necessary

public static string GetCRC32(long crc32) => crc32.ToString().PadLeft(10, '0');

/** <summary> Checks the Extension of an ARCV Resource and Changes it to its External Format. </summary>

<param name = "targetExt" > The Extension of the ARCV Resource. </param>

<returns> The External Extension. </returns> */

private static string CheckExtension(string targetExt)
{

return targetExt switch
{
"NARC" => ".narc",
"RAMN" => ".nmar",
"RCMN" => ".nmcr",
"RCSN" => ".nscr",
"RECN" => ".ncer",
"RGCN" => ".ncgr",
"RLCN" => ".nclr",
"RNAN" => ".nanr",
"RTFN" => ".nftr",
"SDAT" => ".sdat",
_ => ".dat"
};

}

public static string BuildResName(long crc32, string fileExt) => GetPlainName(crc32) + CheckExtension(fileExt);

// Build Full Res Name from CRC32 and Inner Extension as a BinaryName

public static string BuildResNameAsBin(long crc32, string fileExt) => GetCRC32(crc32) + CheckExtension(fileExt);
}

}
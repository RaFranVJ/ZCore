using System;
using System.IO;
using ZCore.Modules;
using ZCore.Modules.Other;

namespace ZCore.Modules.FileParsers
{
/// <summary> Initializes Parsing Task for Files by using the Base64 Algorithm. </summary>

public static class Base64Parser
{
/** <summary> Encodes a File by using Base64 Encoding. </summary>

<param name = "inputPath"> The Path where the File to be Encoded is Located. </param>
<param name = "outputPath"> The Path where the Encoded File will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "OutOfMemoryException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncodeFile(string inputPath, string outputPath, bool isWebSafe, int bufferSize)
{
using FileStream inputFile = File.OpenRead(inputPath);
using FileStream outputFile = File.OpenWrite(outputPath);

byte[] encodeFunc(byte[] bufferData, int bytesRead)
{
string encodedString = Base64StringParser.EncodeBytes(bufferData, isWebSafe);
return Console.InputEncoding.GetBytes(encodedString);
};

FileManager.ProcessBuffer(inputFile, outputFile, encodeFunc, bufferSize);
}

/** <summary> Decodes a File by using Base64 Encoding. </summary>

<param name = "inputPath"> The Path where the File to be Decoded is Located. </param>
<param name = "outputPath"> The Path where the Decoded File will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "OutOfMemoryException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecodeFile(string inputPath, string outputPath, bool isWebSafe, int bufferSize)
{
using FileStream inputFile = File.OpenRead(inputPath);
using FileStream outputFile = File.OpenWrite(outputPath);

byte[] decodeFunc(byte[] bufferData, int bytesRead) // Error on Block Decoding
{
string inputString = Console.InputEncoding.GetString(bufferData);
return Base64StringParser.DecodeString(inputString, isWebSafe);
};

FileManager.ProcessBuffer(inputFile, outputFile, decodeFunc, bufferSize);
}

}

}
using System;
using System.IO;
using ZCore.Modules;
using ZCore.Modules.Other;

namespace ZCore.Modules.FileSecurity
{
/// <summary> Initializes Exclusive-OR (XOR) Ciphering Functions for Files. </summary>

public static class XorCryptor
{
/** <summary> Ciphers a File by using XOR Ciphering. </summary>

<param name = "inputPath"> The Path where the File to Cipher is Located. </param>
<param name = "outputPath"> The Location where the Ciphered File will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void CipherFile(string inputPath, string outputPath, byte[] cipherKey, int bufferSize,
bool deriveKey = false, byte[] saltValue = null, string hashType = null, uint? iterations = null)
{
cipherKey = CryptoParams.CipherKeySchedule(cipherKey, new(1, Array.MaxLength), deriveKey, saltValue, hashType, iterations);

using FileStream inputFile = File.OpenRead(inputPath);
using FileStream outputFile = File.OpenWrite(outputPath);

byte[] processFunc(byte[] bufferData, int bytesRead) => XorBytesCryptor.CipherData(bufferData, cipherKey);
FileManager.ProcessBuffer(inputFile, outputFile, processFunc, bufferSize);
}

}

}
using System;
using System.IO;
using ZCore.Modules.Other;
using ZCore.Modules.SexyCryptors.PopCapCipheredData.Exceptions;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;

namespace ZCore.Modules.SexyCryptors.PopCapCipheredData
{
/// <summary> Initializes Functions for the Ciphered Data (CDAT) from PopCap Files. </summary>

public static class PopDataCryptor
{
/** <summary> The Header of a CDAT File. </summary>

<remarks> It Occupies 11 Bytes in the Stream, which are:
<c>0x43 0x52 0x59 0x50 0x54 0x5F 0x52 0x45 0x53 0x0A 0x00</c> </remarks> */

private const string CdatHeader = "CRYPT_RES\x0A\x00";

/** <summary> Compares the Size of a CDAT File. </summary>

<param name = "sizeToCompare"> The CDAT File to Compare. </param>
<param name = "originalSize"> The Size Contained inside the CDAT Metadata. </param>

<returns> <b>true</b> if is a Size Match; otherwise, <b>false</b>. </returns> */

private static bool CompareCdatSize(CipheredDataInfo sourceInfo, Stream targetStream)
{
bool sizeMatch = sourceInfo.SizeBeforeEncryption.Equals(targetStream.Length);

if(!sizeMatch)
throw new CdatSizeMismatchException(targetStream.Length, sourceInfo.SizeBeforeEncryption);

return sizeMatch;
}

// Get CDAT Stream

public static BinaryStream EncryptStream(Stream input, byte[] cipherKey, int bufferSize, Endian endian = default,
int bytesToEncrypt = 0x100, string outputPath = null, string pathToCdatInfo = null)
{

CipheredDataInfo cryptoInfo = string.IsNullOrEmpty(pathToCdatInfo) ? new(input.Length) :
new CipheredDataInfo().ReadObject(pathToCdatInfo);

PathHelper.ChangeExtension(ref outputPath, ".cdat");
BinaryStream cdatStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

if(cryptoInfo.SizeBeforeEncryption >= bytesToEncrypt)
{
cdatStream.WriteString(CdatHeader, default, endian);
cryptoInfo.WriteBin(cdatStream, endian);

byte[] encryptFunc(byte[] bufferData) => XorBytesCryptor.CipherData(bufferData, cipherKey);

FileManager.ProcessBuffer(input, cdatStream, encryptFunc, bufferSize, bytesToEncrypt);
}

FileManager.ProcessBuffer(input, cdatStream, bufferSize);

return cdatStream;
}

/** <summary> Encrypts the specified File by using XOR Ciphering. </summary>

<param name = "inputPath"> The Path where the File to be Encrypted is Located. </param>
<param name = "outputPath"> The Location where the Encrypted File will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncryptFile(string inputPath, string outputPath, byte[] cipherKey, int bufferSize,
Endian endian = default, int bytesToEncrypt = 0x100, bool deriveKey = false, byte[] saltValue = null,
string hashType = null, uint? iterations = null, MetadataImportParams importCfg = null)
{
cipherKey = CryptoParams.CipherKeySchedule(cipherKey, new(1, Array.MaxLength), deriveKey, saltValue, hashType, iterations);
importCfg ??= new();

string pathToCdatInfo = importCfg.ImportMetadataToFiles ? CipheredDataInfo.ResolvePath(inputPath, importCfg) : null;
using FileStream inputFile = File.OpenRead(inputPath);

using var outputFile = EncryptStream(inputFile, cipherKey, bufferSize, endian, bytesToEncrypt, outputPath, pathToCdatInfo); 
}

// Get Plain Stream

public static Stream DecryptStream(BinaryStream input, byte[] cipherKey, int bufferSize, Endian endian = default,
bool compareSize = false, bool useSizeInfo = true, int bytesToDecrypt = 0x100, string outputPath = null,
string pathToCdatInfo = null)
{
input.CompareString<InvalidCdatException>(CdatHeader, default, endian);

CipheredDataInfo cryptoInfo = CipheredDataInfo.ReadBin(input, endian);

if(compareSize)
CompareCdatSize(cryptoInfo, input);

PathHelper.ChangeExtension(ref outputPath, ".png");
Stream output = string.IsNullOrEmpty(outputPath) ? new MemoryStream() : File.OpenWrite(outputPath);

if(useSizeInfo)
output.SetLength(cryptoInfo.SizeBeforeEncryption);

if(input.Length >= bytesToDecrypt + CdatHeader.Length + sizeof(long) )
{
byte[] decryptFunc(byte[] bufferData) => XorBytesCryptor.CipherData(bufferData, cipherKey);

FileManager.ProcessBuffer(input, output, decryptFunc, bufferSize, bytesToDecrypt);
}

FileManager.ProcessBuffer(input, output, bufferSize);

if(!string.IsNullOrEmpty(pathToCdatInfo) )
cryptoInfo.WriteObject(pathToCdatInfo);

return output;
}

/** <summary> Decrypts a CDAT File that was Encrypted with XOR Ciphering. </summary>

<param name = "inputPath"> The Path where the File to be Decrypted is Located. </param>
<param name = "outputPath"> The Location where the Decrypted File will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecryptFile(string inputPath, string outputPath, byte[] cipherKey, int bufferSize,
Endian endian = default, bool compareSize = false, bool useSizeInfo = true, int bytesToDecrypt = 0x100,
bool deriveKey = false, byte[] saltValue = null, string hashType = null,  uint? iterations = null,
MetadataExportParams exportCfg = null)
{
cipherKey = CryptoParams.CipherKeySchedule(cipherKey, new(1, Array.MaxLength), deriveKey, saltValue, hashType, iterations);
exportCfg ??= new();

using BinaryStream inputFile = BinaryStream.Open(inputPath);
string pathToCdatInfo = exportCfg.ExportMetadataFromFiles ? CipheredDataInfo.ResolvePath(inputPath, exportCfg) : null;

using Stream outputFile = DecryptStream(inputFile, cipherKey, bufferSize, endian, compareSize,
useSizeInfo, bytesToDecrypt, outputPath, pathToCdatInfo);

}

/** <summary> Checks the if the Size of a CDAT matches with its SizeInfo. </summary>

<param name = "targetPath"> The Path to the File to Analize. </param> */

public static void CheckFileSize(string targetPath, MetadataImportParams importCfg = null)
{
importCfg ??= new();

using BinaryStream inputFile = BinaryStream.Open(targetPath);

string pathToCdatInfo = CipheredDataInfo.ResolvePath(targetPath, importCfg);
var fileInfo = new CipheredDataInfo().ReadObject(pathToCdatInfo);

CompareCdatSize(fileInfo, inputFile);
}

}

}
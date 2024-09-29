using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.IO;

namespace ZCore.Modules.SexyCryptors
{
/// <summary> Initializes Rijndael (Rijmen, Vincent and Joan Daemen) Ciphering Functions for Files. </summary>

public class RijndaelCryptor
{
/** <summary> Gets the Size a Rijndael Key must Have. </summary>
<returns> The Rijndael Key Size. </returns> */

protected static Limit<int> GetKeySizeRange() => new(16, 32);

/** <summary> Ciphers the Data specified with the provided Key. </summary>

<param name = "inputBytes"> The Bytes to be Ciphered. </param>
<param name = "cipherKey"> The Cipher Key to be Used. </param>
<param name = "IV"> The Cipher Key to be Used. </param>
<param name = "isForEncryption"> A Boolean that determines if the Data should be Encrypted or not. </param>
<param name = "blockCipherName"> The expected BlockCipher Name (Default is CBC). </param>
<param name = "cipherPaddingIndex"> The Index of the BlockCipherPadding (Default is ZeroPadding). </param>

<returns> The Data Ciphered. </returns> */

public static byte[] CipherData(byte[] inputBytes, byte[] cipherKey, byte[] IV, 
bool isForEncryption, string blockCipherName = "CBC", int cipherPaddingIndex = 0)
{
RijndaelEngine cryptoEngine = new(IV.Length * 8);
var blockCipher = GetBlockCipher(blockCipherName, cryptoEngine);

var blockCipherPadding = GetBlockCipherPadding(cipherPaddingIndex);
PaddedBufferedBlockCipher cipherAlgorithm = new(blockCipher, blockCipherPadding);

ParametersWithIV cryptoParams = new( new KeyParameter(cipherKey), IV);
cipherAlgorithm.Init(isForEncryption, cryptoParams);

int minBlockSize = cipherAlgorithm.GetOutputSize(inputBytes.Length);
byte[] outputBuffers = new byte[minBlockSize];

int processedBytesCount = cipherAlgorithm.ProcessBytes(inputBytes, 0, inputBytes.Length, outputBuffers, 0);
int finalBuffersLength = cipherAlgorithm.DoFinal(outputBuffers, processedBytesCount);

byte[] cipheredData = new byte[processedBytesCount + finalBuffersLength];
Array.Copy(outputBuffers, cipheredData, cipheredData.Length);

return cipheredData;
}

/** <summary> Gets a Instance from the <c>IBlockCipher</c> Interface basing on the given Cipher Name. </summary>

<param name = "blockCipherName"> The expected BlockCipher Name. </param>
<param name = "cryptoEngine"> An Instance from the <c>RijndalEngine</c> where the Block Cipher will be Obtained from. </param>

<returns> The Block Cipher Obtained. </returns> */

private static IBlockCipher GetBlockCipher(string blockCipherName, RijndaelEngine cryptoEngine)
{
Limit<int> cipherKeySize = GetKeySizeRange();

IBlockCipher blockCipher = blockCipherName switch
{
"CFB" => new CfbBlockCipher(cryptoEngine, cipherKeySize.MinValue),
"OFB" => new OfbBlockCipher(cryptoEngine, cipherKeySize.MinValue),
"SIC" => new SicBlockCipher(cryptoEngine),
_ => new CbcBlockCipher(cryptoEngine)
};

return blockCipher;
}

/** <summary> Gets a Instance from the <c>IBlockCipherPadding</c> Interface basing on the given Padding Index. </summary>

<param name = "cipherPaddingIndex"> The Index of the BlockCipherPadding. </param>

<returns> The BlockCipherPadding Obtained. </returns> */

private static IBlockCipherPadding GetBlockCipherPadding(int cipherPaddingIndex)
{

IBlockCipherPadding blockCipherPadding = cipherPaddingIndex switch
{
1 => new ISO7816d4Padding(),
2 => new Pkcs7Padding(),
3 => new TbcPadding(),
4 => new X923Padding(),
_ => new ZeroBytePadding()
};

return blockCipherPadding;
}

/** <summary> Encrypts a File by using Rijndael Ciphering. </summary>

<param name = "inputPath"> The Path where the File to be Encrypted is Located. </param>
<param name = "outputPath"> The Location where the Encrypted File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "CryptographicException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncryptFile(string inputPath, string outputPath, byte[] cipherKey,
bool deriveKey = false, byte[] saltValue = null, string hashType = null, 
uint? iterations = null, string blockCipherName = "CBC", int cipherPaddingIndex = 0)
{
Limit<int> keySizeRange = GetKeySizeRange();

cipherKey = CryptoParams.CipherKeySchedule(cipherKey, keySizeRange, deriveKey, saltValue, hashType, iterations);

byte[] IV = CryptoParams.InitVector(cipherKey, keySizeRange);
byte[] encryptedData = CipherData(File.ReadAllBytes(inputPath), cipherKey, IV, true, blockCipherName, cipherPaddingIndex);

File.WriteAllBytes(outputPath, encryptedData);
}

/** <summary> Decrypts a File by using Rijndael Ciphering. </summary>

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

public static void DecryptFile(string inputPath, string outputPath, byte[] cipherKey,
bool deriveKey = false, byte[] saltValue = null, string hashType = null, 
uint? iterations = null, string blockCipherName = "CBC", int cipherPaddingIndex = 0)
{
Limit<int> keySizeRange = GetKeySizeRange();

cipherKey = CryptoParams.CipherKeySchedule(cipherKey, keySizeRange, deriveKey, saltValue, hashType, iterations);

byte[] IV = CryptoParams.InitVector(cipherKey, keySizeRange);
byte[] decryptedData = CipherData(File.ReadAllBytes(inputPath), cipherKey, IV, false, blockCipherName, cipherPaddingIndex);

File.WriteAllBytes(outputPath, decryptedData);
}

}

}
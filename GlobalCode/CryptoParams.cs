using System;
using System.Security.Cryptography;

/// <summary> Initializes Handling Functions for Parameters that are used on Data Encryption or Decryption. </summary>

public static class CryptoParams
{
/** <summary> Checks if the Size of the given Data Block meets the expected Range. </summary>

<param name = "blockData"> The Data Block to be Validated. </param>
<param name = "expectedSize"> The Size Excepted. </param> 

<returns> The Block Size. </returns> */

private static int CheckBlockSize(byte[] blockData, Limit<int> expectedSize)
{
int blockSize;

if(blockData.Length < expectedSize.MinValue)
{
blockSize = expectedSize.MinValue;

InputHelper.FillArray(ref blockData, blockSize);
}

else if(blockData.Length > expectedSize.MaxValue)
blockSize = expectedSize.MaxValue;

else
blockSize = blockData.Length;

return blockSize;
}

/** <summary> Checks if the providen Cipher Key meets the expected Size. </summary>
<remarks> In case the Cipher Key doesn't meet the expected Size, a similar will be Generated instead. </remarks>

<param name = "sourceKey"> The Key to be Validated. </param>
<param name = "expectedSize"> The Key Size Excepted. </param> */

public static void CheckKeySize(byte[] sourceKey, Limit<int> expectedSize)
{
sourceKey ??= Console.InputEncoding.GetBytes("<Enter a Cipher Key>");

if(sourceKey.Length < expectedSize.MinValue)
InputHelper.FillArray(ref sourceKey, expectedSize.MinValue);

else if(sourceKey.Length > expectedSize.MaxValue)
Array.Resize(ref sourceKey, expectedSize.MaxValue);

return;
}

/** <summary> Checks if the providen IV meets the expected Size. </summary>
<remarks> In case the IV doesn't meet the expected Size, a similar will be Generated instead. </remarks>

<param name = "sourceIV"> The IV to Validate. </param>
<param name = "expectedLength"> The Length Excepted. </param> */

public static void CheckIVLength(byte[] sourceIV, Limit<int> expectedLength)
{
sourceIV ??= Console.InputEncoding.GetBytes("<Enter a IV>");

if(sourceIV.Length < expectedLength.MinValue)
InputHelper.FillArray(ref sourceIV, expectedLength.MinValue);

else if(sourceIV.Length > expectedLength.MaxValue)
Array.Resize(ref sourceIV, expectedLength.MaxValue);

return;
}

/** <summary> Generates a derived Key from an Existing one, by Performing some Iterations. </summary>

<param name = "cipherKey"> The Cipher Key to Derive. </param>
<param name = "saltValue"> The Salt Value used for Reinforcing the Cipher Key. </param>
<param name = "hashType"> The Hash to be used. </param>
<param name = "iterationsCount"> The number of Iterations to be Perfomed. </param>
<param name = "expectedKeySize"> The Key Size Excepted. </param>
<param name = "expectedIterations"> The expected Number of Iterations. </param>

<returns> The derived Cipher Key. </returns> */

public static byte[] CipherKeySchedule(byte[] cipherKey, Limit<int> expectedKeySize, bool deriveKeys = false,
byte[] saltValue = null, string hashType = null, uint? iterations = null)
{
CheckKeySize(cipherKey, expectedKeySize);

if(deriveKeys)
{
PasswordDeriveBytes derivedKey = new(cipherKey, saltValue, hashType, (int)(iterations ?? 1) );

return derivedKey.GetBytes( CheckBlockSize(cipherKey, expectedKeySize) );
}

return cipherKey;
}

/** <summary> Initializates a Vector from a Cipher Key. </summary>

<param name = "cipherKey"> The Cipher Key to used. </param>
<param name = "expectedVectorSize"> The Vector Size Excepted. </param>
<param name = "vectorIndex"> Specifies where the Vector should Start Copying the Bytes from the Cipher Key (Default Index is 0). </param>

<returns> The IV that was Generated. </returns> */

public static byte[] InitVector(byte[] cipherKey, Limit<int> expectedVectorSize, int vectorIndex = 0)
{
int vectorSize = CheckBlockSize(cipherKey, expectedVectorSize);
byte[] IV = new byte[vectorSize];

if(cipherKey.Length == vectorSize)
Array.Reverse(IV);

else
Array.Copy(cipherKey, vectorIndex, IV, 0, vectorSize);

return IV;
}

}
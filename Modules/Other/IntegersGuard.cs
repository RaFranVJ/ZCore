using ZCore.Modules;

namespace ZCore.Modules.Other
{
/// <summary> Initializes ciphering Functions for Integer values. </summary>

public static class IntegersGuard
{
/// <summary> The Cipher Key used for Encrypting/Decrypting Values. </summary>

private const int CIPHER_KEY = 13;

/// <summary> The Logic Factor used for Encrypting/Decrypting Values. </summary>

private const int LOGIC_FACTOR = 0x1F;

/// <summary> The expected Base of an Integer value. </summary>

private const int EXPECTED_BASE = 32;

/// <summary> The BitMask used for Encrypting/Decrypting Values. </summary>

private const int BITMASK = 0xFF;

/** <summary> Encrypts an Integer value by Performing some bitwise Operations on it. </summary>

<param name = "targetValue"> The Value to be Encrypted. </param>

<exception cref = "ArithmeticException"></exception>
<exception cref = "OverflowException"></exception>

<returns> The Encrypted Value. </returns> */

public static int EncryptValue(uint targetValue)
{
uint xorValue = targetValue ^ CIPHER_KEY;
int highBitsRate = CIPHER_KEY & LOGIC_FACTOR;

uint bigInteger = xorValue << highBitsRate;
int baseDifference = EXPECTED_BASE - highBitsRate;

int lowBitsRate = baseDifference & BITMASK;
uint smallInteger = xorValue >> lowBitsRate;

return (int)(bigInteger | smallInteger);
}

// JS Func

public static object EncryptValueJS(string arg) => EncryptValue( InputHelper.FilterNumber<uint>(arg) );

/** <summary> Decrypts an Integer value by Performing some bitwise Operations on it. </summary>

<param name = "targetValue"> The Value to be Decrypted. </param>

<exception cref = "ArithmeticException"></exception>
<exception cref = "OverflowException"></exception>

<returns> The Decrypted Value. </returns> */

public static int DecryptValue(uint targetValue)
{
int lowBitsRate = CIPHER_KEY & LOGIC_FACTOR;
uint smallInteger = targetValue >> lowBitsRate;

int baseDifference = EXPECTED_BASE - lowBitsRate;
int highBitsRate = baseDifference & BITMASK;

uint bigInteger = targetValue << highBitsRate;
int xnorValue = (int)(smallInteger | bigInteger);

return CIPHER_KEY ^ xnorValue;
}

// JS Func

public static object DecryptValueJS(string arg) => DecryptValue( InputHelper.FilterNumber<uint>(arg) );
}

}
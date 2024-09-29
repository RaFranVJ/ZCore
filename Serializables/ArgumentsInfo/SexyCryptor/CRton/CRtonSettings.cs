using ZCore.Serializables.ArgumentsInfo.SexyCryptor.Rijndael;
using System.Text;

namespace ZCore.Serializables.ArgumentsInfo.SexyCryptor.CRton
{
/// <summary> Groups Info related to specific Params used for Ciphering RTON Files with Rijndael. </summary>

public class CRtonSettings : RijndaelSettings
{
/// <summary> Creates a new Instance of the <c>RijndaelInfo</c>. </summary>

public CRtonSettings()
{
CipherKey = Encoding.UTF8.GetBytes("com_popcap_pvz2_magento_product_2013_05_05");
SaltValue = null;

DeriveKeys = false;
}

/// <summary> Checks each nullable Field of the <c>RijndaelInfo</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
CRtonSettings defaultInfo = new();

#region ======== Set default Values to Null Fields ========

CipherKey ??= defaultInfo.CipherKey;
BlockCipherName ??= defaultInfo.BlockCipherName;
HashType ??= defaultInfo.HashType;

if(DeriveKeys)
{
RijndaelSettings baseSettings = new();

SaltValue ??= baseSettings.SaltValue;
}

#endregion
}

}

}
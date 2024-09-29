using ZCore.Serializables.ArgumentsInfo.SexyCryptor.Cdat;
using ZCore.Serializables.ArgumentsInfo.SexyCryptor.CRton;
using ZCore.Serializables.ArgumentsInfo.SexyCryptor.Rijndael;
using ZCore.Serializables.ArgumentsInfo.SexyCryptor.RSA;

namespace ZCore.Serializables.Arguments.ParamGroups
{
/// <summary> The Arguments used in the <c>SexyCryptors</c> Tasks. </summary>

public class SexyCryptorsArgs : ArgumentsSet
{
/** <summary> Gets or Sets the Crypto Info related to the Rijndael Algorithm. </summary>
<returns> The Rijndael Crypto Info. </returns> */

public RijndaelSettings RijndaelConfig{ get; set; }

/** <summary> Gets or Sets the Crypto Info related to the RSA Algorithm. </summary>
<returns> The RSA Crypto Info. </returns> */

#if WINDOWS || WINDOWSCONSOLE

public RSASettings_Windows RsaConfig{ get; set; }

#else

public RSASettings RsaConfig{ get; set; }

#endif

/** <summary> Gets or Sets the Crypto Info related to the CDAT Algorithm. </summary>
<returns> The CDAT Crypto Info. </returns> */

public CdatSettings CdatConfig{ get; set; }

/** <summary> Gets or Sets the Crypto Info related to RTON Ciphering. </summary>
<returns> The RTON Crypto Info. </returns> */

public CRtonSettings RtonCryptoInfo{ get; set; }

/// <summary> Creates a new Instance of the <c>SexyCryptorsArgs</c>. </summary>

public SexyCryptorsArgs()
{
RijndaelConfig = new();
RsaConfig = new();

CdatConfig = new();
RtonCryptoInfo = new();
}

/// <summary> Checks each nullable Field of the <c>SexyCryptorsArgs</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
SexyCryptorsArgs defaultArgs = new();

#region ======== Set default Values to Null Fields ========

InputPath ??= defaultArgs.InputPath;
OutputPath ??= defaultArgs.OutputPath;
CdatConfig ??= defaultArgs.CdatConfig;
RijndaelConfig ??= defaultArgs.RijndaelConfig;
RsaConfig ??= defaultArgs.RsaConfig;

#endregion

CdatConfig.CheckForNullFields();
RijndaelConfig.CheckForNullFields();
RsaConfig.CheckForNullFields();

PathHelper.CheckExistingPath(InputPath, true);
PathHelper.CheckExistingPath(OutputPath, false);
}

}

}
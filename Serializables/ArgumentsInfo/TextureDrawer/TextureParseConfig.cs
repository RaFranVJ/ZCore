using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.SexyTex;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Txz;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.UTex;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer
{
/// <summary> The Info used in the <c>TextureParsers/c>. </summary>

public class TextureParseConfig : ParamGroupInfo
{
/** <summary> Gets or Sets the Settings related to the TXZ Parser. </summary>
<returns> The TxzConfig. </returns> */

public TxzSettings TxzConfig{ get; set; }

/** <summary> Gets or Sets the Settings related to the UTex Parser. </summary>
<returns> The UTexConfig. </returns> */

public UTexSettings UTexConfig{ get; set; }

/** <summary> Gets or Sets the Settings related to the XNB Parser. </summary>
<returns> The UTexConfig. </returns> */

public XnbSettings XnbConfig{ get; set; }

/** <summary> Gets or Sets the Settings related to the SexyTex Parser. </summary>
<returns> The SexyTexConfig. </returns> */

public SexyTexSettings SexyTexConfig{ get; set; }

/** <summary> Gets or Sets the Settings related to the PTX Parser. </summary>
<returns> The PtxConfig. </returns> */

public PtxSettings PtxConfig{ get; set; }

/// <summary> Creates a new Instance of the <c>TextureParseConfig/c>. </summary>

public TextureParseConfig()
{
TxzConfig = new();
UTexConfig = new();

XnbConfig = new();
SexyTexConfig = new();

PtxConfig = new();
}

/// <summary> Checks each nullable Field of this instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
TextureParseConfig defaultConfig = new();

#region ======== Set default Values to Null Fields ========

TxzConfig ??= defaultConfig.TxzConfig;
UTexConfig ??= defaultConfig.UTexConfig;
XnbConfig ??= defaultConfig.XnbConfig;
SexyTexConfig ??= defaultConfig.SexyTexConfig;
PtxConfig ??= defaultConfig.PtxConfig;

#endregion
}

}

}
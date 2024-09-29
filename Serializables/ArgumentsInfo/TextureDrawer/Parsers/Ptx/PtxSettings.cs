using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Config;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.Mobile;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.PlayStation;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx
{
/** <summary> Groups multiple Params related to the PTX Parser on Different Platforms. </summary>

<remarks> PTX stands for "PopCap Texture" but also means "Packed Texture" </remarks> */

public class PtxSettings : ParamGroupInfo
{
/** <summary> Gets or Sets some Params used when Parsing Files on Android. </summary>
<returns> The Android Params. </returns> */

public GenericImgSettings<PtxFormat_Android> ParamsForAndroid{ get; set; }

/** <summary> Gets or Sets some Params used when Parsing Files on Android. (Chinese Version) </summary>
<returns> The Android Params (China). </returns> */

public GenericImgSettings<PtxFormat_AndroidCN> ParamsForAndroidCN{ get; set; }

/** <summary> Gets or Sets some Params used when Parsing Files on iOS. </summary>
<returns> The iOS Params. </returns> */

public GenericImgSettings<PtxFormat_iOS> ParamsForiOS{ get; set; }

/** <summary> Gets or Sets some Params used when Parsing Files on PSP. </summary>
<returns> The PSP Params. </returns> */

public GenericImgSettings_Metadata<PtxFormat_PSP> ParamsForPSP{ get; set; }

/** <summary> Gets or Sets some Params used when Parsing Files on PSV. </summary>
<returns> The PSV Params. </returns> */

public PtxConfigForPSV ParamsForPSV{ get; set; }

/** <summary> Gets or Sets some Params used when Parsing Files on PS3. </summary>
<returns> The PS3 Params. </returns> */

public PtxConfigForPS3 ParamsForPS3{ get; set; }

/** <summary> Gets or Sets some Params used when Parsing Files on PS4. </summary>
<returns> The PS4 Params. </returns> */

public PtxConfigForPS4 ParamsForPS4{ get; set; }

/** <summary> Gets or Sets some Params used when Parsing Files on Xbox360. </summary>
<returns> The Xbox Params. </returns> */

public PtxConfigForXbox360 ParamsForXbox360{ get; set; }

/// <summary> Creates a new Instance of the <c>TxzSettings</c>. </summary>

public PtxSettings()
{
ParamsForAndroid = new();
ParamsForAndroidCN = new(PtxFormat_AndroidCN.ETC1_RGB_A_Palette);
ParamsForiOS = new(PtxFormat_iOS.PVRTC_4BPP_RGB_A8);

ParamsForPSP = new();
ParamsForPSV = new();
ParamsForPS3 = new();
ParamsForPS4 = new();

ParamsForXbox360 = new();
}

/// <summary> Checks each nullable Field of the <c>PtxSettings</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
PtxSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

ParamsForAndroid ??= defaultOptions.ParamsForAndroid;
ParamsForAndroidCN ??= defaultOptions.ParamsForAndroidCN;
ParamsForiOS ??= defaultOptions.ParamsForiOS;

ParamsForPSP ??= defaultOptions.ParamsForPSP;
ParamsForPSV ??= defaultOptions.ParamsForPSV;
ParamsForPS3 ??= defaultOptions.ParamsForPS3;
ParamsForPS4 ??= defaultOptions.ParamsForPS4;

ParamsForXbox360 ??= defaultOptions.ParamsForXbox360;

#endregion

ParamsForAndroid.CheckForNullFields();
ParamsForAndroidCN.CheckForNullFields();
ParamsForiOS.CheckForNullFields();

ParamsForPSP.CheckForNullFields();
ParamsForPSV.CheckForNullFields();
ParamsForPS3.CheckForNullFields();
ParamsForPS4.CheckForNullFields();

ParamsForXbox360.CheckForNullFields();
}

}

}
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.ArcV;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Dz;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsb;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsg;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Smf;

namespace ZCore.Serializables.Arguments.ParamGroups
{
/// <summary> The Arguments used in the <c>SexyCompressors</c> Tasks. </summary>

public class SexyCompressorsArgs : ArgumentsSet
{
/** <summary> Gets or Sets the Compress Info related to the ARCV Algorithm. </summary>
<returns> The ARCV Compress Info. </returns> */

public ArcvSettings ArcvConfig{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the DTRZ Algorithm. </summary>
<returns> The DZ Compress Info. </returns> */

public DzSettings DzConfig{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the RSB Algorithm. </summary>
<returns> The RSB Compress Info. </returns> */

public RsbSettings RsbConfig{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the RSG Algorithm. </summary>
<returns> The RSG Compress Info. </returns> */

public RsgSettings RsgConfig{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the SMF Algorithm. </summary>
<returns> The SMF Compress Info. </returns> */

public SmfSettings SmfConfig{ get; set; }

/// <summary> Creates a new Instance of the <c>FileCompressorsArgs</c>. </summary>

public SexyCompressorsArgs()
{
ArcvConfig = new();
DzConfig = new();

RsbConfig = new();
RsgConfig = new();

SmfConfig = new();
}

/// <summary> Checks each nullable Field of the <c>FileSecurityArgs</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
SexyCompressorsArgs defaultArgs = new();

#region ======== Set default Values to Null Fields ========

InputPath ??= defaultArgs.InputPath;
OutputPath ??= defaultArgs.OutputPath;
RsbConfig ??= defaultArgs.RsbConfig;
RsgConfig ??= defaultArgs.RsgConfig;
SmfConfig ??= defaultArgs.SmfConfig;

#endregion

RsbConfig.CheckForNullFields();
SmfConfig.CheckForNullFields();

PathHelper.CheckExistingPath(InputPath, true);
PathHelper.CheckExistingPath(OutputPath, false);
}

}

}
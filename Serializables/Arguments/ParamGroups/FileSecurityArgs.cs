using ZCore.Serializables.ArgumentsInfo.FileSecurity;
using ZCore.Serializables.ArgumentsInfo.FileSecurity.Integrity;

namespace ZCore.Serializables.Arguments.ParamGroups
{
/// <summary> The Arguments used in the <c>FileSecurity</c> Tasks. </summary>

public class FileSecurityArgs : ArgumentsSet
{
/** <summary> Gets or Sets some Config for the Archive Hashers. </summary>
<returns> The File Hashers Config. </returns> */

public ArchiveHashersConfig FileHashersConfig{ get; set; }

/** <summary> Gets or Sets some Config for Adler32. </summary>
<returns> The Adler32 Config. </returns> */

public Adler32BytesInfo Adler32Config{ get; set; }

/** <summary> Gets or Sets some Config for the Archive Cryptors. </summary>
<returns> The File Cryptors Config. </returns> */

public ArchiveCryptorsConfig FileCryptorsConfig{ get; set; }

/// <summary> Creates a new Instance of the <c>FileSecurityArgs</c>. </summary>

public FileSecurityArgs()
{
FileHashersConfig = new();
Adler32Config = new();

FileCryptorsConfig = new();
}

/// <summary> Checks each nullable Field of the <c>FileSecurityArgs</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
FileSecurityArgs defaultArgs = new();

#region ======== Set default Values to Null Fields ========

InputPath ??= defaultArgs.InputPath;
OutputPath ??= defaultArgs.OutputPath;
FileHashersConfig ??= defaultArgs.FileHashersConfig;
Adler32Config ??= defaultArgs.Adler32Config;
FileCryptorsConfig ??= defaultArgs.FileCryptorsConfig;

#endregion

FileHashersConfig.CheckForNullFields();
FileCryptorsConfig.CheckForNullFields();

PathHelper.CheckExistingPath(InputPath, true);
PathHelper.CheckExistingPath(OutputPath, false);
}

}

}
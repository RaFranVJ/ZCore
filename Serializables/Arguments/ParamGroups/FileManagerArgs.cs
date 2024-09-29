using System.IO;
using ZCore.Serializables.Arguments;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.FileManager.Directory;
using ZCore.Modules;

namespace ZCore.Serializables.Arguments.ParamGroups
{
/// <summary> The Arguments used in the <c>FileManager</c> Tasks. </summary>

public class FileManagerArgs : ArgumentsSet
{
/** <summary> Gets or Sets some Options related to Files. </summary>
<returns> The File Options. </returns> */

public ArchiveManagerOptions FileOptions{ get; set; }

/** <summary> Gets or Sets some Options related to Folder. </summary>
<returns> The Directory Options. </returns> */

public DirectoryManagerOptions FolderOptions{ get; set; }

/// <summary> Creates a new Instance of the <c>FileManagerArgs</c>. </summary>

public FileManagerArgs()
{
InputPath = GetDefaultInputPath();
OutputPath = GetDefaultOutputPath();

FileOptions = new();
FolderOptions = new();
}

/// <summary> Checks each nullable Field of the <c>FileManagerArgs</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
FileManagerArgs defaultArgs = new();

#region ======== Set default Values to Null Fields ========

InputPath ??= defaultArgs.InputPath;
OutputPath ??= defaultArgs.OutputPath;
FileOptions ??= defaultArgs.FileOptions;
FolderOptions ??= defaultArgs.FolderOptions;

#endregion

FileOptions.CheckForNullFields();
FolderOptions.CheckForNullFields();

PathHelper.CheckExistingPath(InputPath, true);
PathHelper.CheckExistingPath(OutputPath, false);
}

/** <summary> Gets the default Output Path basing on the CurrentAppDirectory. </summary>
<returns> The default Output Path. </returns> */

protected override string GetDefaultOutputPath() => LibInfo.CurrentDllDirectory + Path.DirectorySeparatorChar + "Output.fs";
}

}
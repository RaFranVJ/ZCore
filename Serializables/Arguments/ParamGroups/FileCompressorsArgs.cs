using System.IO;
using ZCore.Serializables.ArgumentsInfo.Compressor;
using ZCore.Serializables.ArgumentsInfo.Compressor.BZip2;
using ZCore.Serializables.ArgumentsInfo.Compressor.LZ4;
using ZCore.Serializables.ArgumentsInfo.Compressor.Lzma;
using ZCore.Serializables.ArgumentsInfo.Compressor.Tar;
using ZCore.Serializables.ArgumentsInfo.Compressor.Zip;

namespace ZCore.Serializables.Arguments.ParamGroups
{
/// <summary> The Arguments used in the <c>FileCompressors</c> Tasks. </summary>

public class FileCompressorsArgs : ArgumentsSet
{
/** <summary> Gets or Sets the Compress Info related to the Brotli Algorithm. </summary>
<returns> The Brotli Compress Info. </returns> */

public GenericCompressorInfo BrotliConfig{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the BZip2 Algorithm. </summary>
<returns> The BZip2 Compress Info. </returns> */

public BZip2Settings BZip2Config{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the Deflate Algorithm. </summary>
<returns> The Deflate Compress Info. </returns> */

public GenericCompressorInfo DeflateConfig{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the GZip Algorithm. </summary>
<returns> The GZip Compress Info. </returns> */

public GenericCompressorInfo GZipConfig{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the LZ4 Algorithm. </summary>
<returns> The LZ4 Compress Info. </returns> */

public LZ4Settings LZ4Config{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the LZMA Algorithm. </summary>
<returns> The LZMA Compress Info. </returns> */

public LzmaSettings LzmaConfig{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the TAR Algorithm. </summary>
<returns> The TAR Compress Info. </returns> */

public TarSettings TarConfig{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the ZIP Algorithm. </summary>
<returns> The ZIP Compress Info. </returns> */

public ZipSettings ZipConfig{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the ZLib Algorithm. </summary>
<returns> The ZLib Compress Info. </returns> */

public GenericCompressorInfo ZLibConfig{ get; set; }

/// <summary> Creates a new Instance of the <c>FileCompressorsArgs</c>. </summary>

public FileCompressorsArgs()
{
BrotliConfig = new();
BZip2Config = new();

DeflateConfig = new();
GZipConfig = new();

LZ4Config = new();
LzmaConfig = new();

TarConfig = new();
ZipConfig = new();

ZLibConfig = new();
}

/// <summary> Checks each nullable Field of the <c>FileCompressorsArgs</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
FileCompressorsArgs defaultArgs = new();

#region ======== Set default Values to Null Fields ========

InputPath ??= defaultArgs.InputPath;
OutputPath ??= defaultArgs.OutputPath;
BrotliConfig ??= defaultArgs.BrotliConfig;
BZip2Config ??= defaultArgs.BZip2Config;
DeflateConfig ??= defaultArgs.DeflateConfig;
GZipConfig ??= defaultArgs.GZipConfig;
LZ4Config ??= defaultArgs.LZ4Config;
LzmaConfig ??= defaultArgs.LzmaConfig;
TarConfig ??= defaultArgs.TarConfig;
ZipConfig ??= defaultArgs.ZipConfig;

#endregion

BrotliConfig.CheckForNullFields();
BZip2Config.CheckForNullFields();
DeflateConfig.CheckForNullFields();
GZipConfig.CheckForNullFields();
LZ4Config.CheckForNullFields();
LzmaConfig.CheckForNullFields();
TarConfig.CheckForNullFields();
ZipConfig.CheckForNullFields();

PathHelper.CheckExistingPath(InputPath, true);
PathHelper.CheckExistingPath(OutputPath, false);
}

/** <summary> Gets the default Output Path basing on the CurrentAppDirectory. </summary>
<returns> The default Output Path. </returns> */

protected override string GetDefaultOutputPath() => LibInfo.CurrentDllDirectory + Path.DirectorySeparatorChar + "Output.compressed";
}

}
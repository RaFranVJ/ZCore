using System.IO;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer;

namespace ZCore.Serializables.Arguments.ParamGroups
{
/// <summary> The Arguments used in the <c>TextureDrawer</c>. </summary>

public class TextureDrawerArgs : ArgumentsSet
{
/** <summary> Gets or Sets some Params for the Texture Parsers. </summary>
<returns> The RTON Parse Info. </returns> */

public TextureParseConfig ParamsForTextureParsers{ get; set; }

// ParamsForAtlas

/// <summary> Creates a new Instance of the <c>TextureDrawerArgs</c>. </summary>

public TextureDrawerArgs()
{
ParamsForTextureParsers	= new();
}

/// <summary> Checks each nullable Field of the <c>TextureDrawerArgs</c> and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
TextureDrawerArgs defaultArgs = new();

#region ======== Set default Values to Null Fields ========

InputPath ??= defaultArgs.InputPath;
OutputPath ??= defaultArgs.OutputPath;
ParamsForTextureParsers ??= defaultArgs.ParamsForTextureParsers;

#endregion

ParamsForTextureParsers.CheckForNullFields();

PathHelper.CheckExistingPath(InputPath, true);
PathHelper.CheckExistingPath(OutputPath, false);
}

/** <summary> Gets the default Input Path basing on the CurrentAppDirectory. </summary>
<returns> The default Input Path. </returns> */

protected override string GetDefaultInputPath() => LibInfo.CurrentDllDirectory + Path.DirectorySeparatorChar + "Input.png";
}

}
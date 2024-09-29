using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings;

namespace ZCore.Serializables.Arguments.ParamGroups
{
/// <summary> The Arguments used in the <c>TextProcessor</c>. </summary>

public class TextProcessorArgs : ArgumentsSet
{
/** <summary> Gets or Sets some Params for the Texture Parsers. </summary>
<returns> The RTON Parse Info. </returns> */

public LawnStringsSettings LawnStringsConfig{ get; set; }

// Add fields

/// <summary> Creates a new Instance of the <c>TextProcessorArgs</c>. </summary>

public TextProcessorArgs()
{
LawnStringsConfig = new();
}

/// <summary> Checks each nullable Field of the <c>TextureDrawerArgs</c> and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
TextProcessorArgs defaultArgs = new();

#region ======== Set default Values to Null Fields ========

InputPath ??= defaultArgs.InputPath;
OutputPath ??= defaultArgs.OutputPath;
LawnStringsConfig ??= defaultArgs.LawnStringsConfig;

#endregion

LawnStringsConfig.CheckForNullFields();

PathHelper.CheckExistingPath(InputPath, true);
PathHelper.CheckExistingPath(OutputPath, false);
}

}

}
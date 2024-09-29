namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture
{
/// <summary> Determines how to Handle Alpha (Transparency) when Parsing PTX Files. </summary>

public enum PtxAlphaChannel : uint
{
/// <summary> Default Alpha Channel </summary>
Default,

/// <summary> Alpha Channel will use a Palette </summary>
A_Palette = 0x64
}

}
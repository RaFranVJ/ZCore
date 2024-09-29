using System;
using System.IO;

namespace ZCore.Modules.TextureDrawer.Parsers.PopCapTexture
{
// Exception thrown when no PTX Info is Providen

public class MissingPtxInfoException(string filePath) : 
Exception($"Missing Ptx Info for: \"{Path.GetFileName(filePath)}\"")
{
}

}
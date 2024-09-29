using System;

namespace ZCore.Modules.SexyParsers.CharacterFontWidget2.Exceptions
{
// Exception thrown when Invalid ver is Read

public class InvalidCfw2VersionException(Int128 ver, Int128 expected) : 
InvalidFileVersionException<Int128>(ver, $"Unknown Cfw2 Version ({ver})\nAllowed Version is v{expected}", expected)
{
}

}
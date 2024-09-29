using System;

namespace ZCore.Modules.SexyCompressors.ArcVPackage.Exceptions
{
// Exception thrown when no Entries are defined

public class EmptyArcvDirException() : Exception("Empty ARCV Folder. Must define at Least one File Entry")
{
}

}
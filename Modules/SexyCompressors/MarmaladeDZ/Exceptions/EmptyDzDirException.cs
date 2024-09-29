using System;

namespace ZCore.Modules.SexyCompressors.MarmaladeDZ.Exceptions
{
// Exception thrown when no Entries are defined

public class EmptyDzDirException() : Exception("Empty Dz Folder. Must define at Least one File for Compression")
{
}

}
using System;
using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ArcVPackage.Definitions
{
/// <summary> Allows Comparing the Info of two Different Files. </summary>

public class ArcvResComparer : IComparer<ArcvResEntry>
{
/** <summary> Compares two ARCV Resources by Analizing its CRC32. </summary>

<param name = "x" > The First File. </param>
<param name = "y" > The Second File. </param>

<returns> The Difference Obtained. </returns> */

public int Compare(ArcvResEntry x, ArcvResEntry y) => Math.Sign(x.CRC32 - y.CRC32);
}

}
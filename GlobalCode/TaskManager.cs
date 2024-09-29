using System;
using System.Runtime.InteropServices;

/** <summary> Initializes Handling Functions for any Task that is being Executed in 
the Runtime Enviroment of the Device. </summary> */

public static class TaskManager
{
/** <summary> Sets a new Working Size to a Process. </summary>

<param name = "processHandle"> The Handle of the Process to be Resized. </param>
<param name = "minSize"> The Minimum Size of the Process. </param>
<param name = "maxSize"> The Maximum Size of the Process. </param>

<returns> The new Working Size of the Process. </returns> */

[DllImport("kernel32.dll") ]

private static extern int SetProcessWorkingSetSize(IntPtr processHandle, int minSize = -1, int maxSize = 0);

/** <summary> Releases all the Resources that a Task Occupies. </summary>
<param name = "targetHandle"> The Handle of the Process to be Released. </param> */

public static void ReleaseTaskResources(IntPtr targetHandle) => SetProcessWorkingSetSize(targetHandle);
}
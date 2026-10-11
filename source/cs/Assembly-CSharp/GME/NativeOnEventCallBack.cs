using System;
using System.Runtime.InteropServices;

namespace GME;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void NativeOnEventCallBack(IntPtr nativeInstance, int event_type, string data);

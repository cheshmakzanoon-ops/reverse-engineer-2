using System.Runtime.InteropServices;

namespace GME;

public delegate void QAVEndpointsUpdateInfo(int eventID, int count, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] string[] userIDList);

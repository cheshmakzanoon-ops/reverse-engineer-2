using System.Runtime.InteropServices;

namespace GME;

public delegate void QAVPlayMusicFinishCallback(int result, long sound_id, [MarshalAs(UnmanagedType.U1)] bool is_finish, string file_path);

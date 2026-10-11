namespace GME;

public class QAVError
{
	public static int OK = 0;

	public static int AV_ERR_UNKNOWN = 1;

	public static int AV_ERR_FREQUENT_OPERATION = 1000;

	public static int AV_ERR_REPETITIVE_OPERATION = 1001;

	public static int AV_ERR_EXCLUSIVE_OPERATION = 1002;

	public static int AV_ERR_HAS_IN_THE_STATE = 1003;

	public static int AV_ERR_INVALID_ARGUMENT = 1004;

	public static int AV_ERR_TIMEOUT = 1005;

	public static int AV_ERR_NOT_IMPLEMENTED = 1006;

	public static int AV_ERR_REFUSED = 1007;

	public static int AV_ERR_INVALID_LICENSE = 1008;

	public static int AV_ERR_DISCONNECTED = 1009;

	public static int AV_ERR_CONTEXT_NOT_START = 1101;

	public static int AV_ERR_ROOM_NOT_EXIST = 1201;

	public static int AV_ERR_ROOM_NOT_EXITED = 1202;

	public static int AV_ERR_DEVICE_NOT_EXIST = 1301;

	public static int AV_ERR_SERVER_FAILED = 10001;

	public static int AV_ERR_VOICE_RECORDER_PARAM_INVALID = 4097;

	public static int AV_ERR_VOICE_RECORDER_NO_MIC_PERMISSION = 4098;

	public static int AV_ERR_VOICE_RECORDER_ALREADY_STARTED = 4099;

	public static int AV_ERR_VOICE_RECORD_AUDIO_TOO_SHORT = 4100;

	public static int AV_ERR_VOICE_RECORDER_OPENFILE_ERROR = 4101;

	public static int AV_ERR_VOICE_RECORD_FORMAT_NOT_SUPPORTED = 4105;

	public static int AV_ERR_VOICE_RECORDER_INNER_ERROR = 4106;

	public static int AV_ERR_VOICE_PLAYER_OPENFILE_ERROR = 20484;

	public static int AV_ERR_VOICE_PLAYER_FORMAT_NOT_SUPPORTED = 20486;

	public static int AV_ERR_REQUEST_IP_TIMEOUT = 7001;

	public static int AV_ERR_CONNECT_SERVER_TIMEOUT = 7002;

	public static int AV_ERR_ENTER_ROOM_FAILED = 7004;

	public static int AV_ERR_AUTH_FIALD = 7006;

	public static int AV_ERR_IN_OTHER_ROOM = 7007;

	public static int AV_ERR_DISSOLVED_OVERUSER = 7008;

	public static int AV_ERR_VOICE_MESSAGE_SERVICE_INVALID = 7009;

	public static int AV_ERR_SPEECH_TO_TEXT_SERVICE_INVALID = 7010;

	public static int AV_ERR_TRANSLATE_TEXT_SERVICE_INVALID = 7011;

	public static int AV_ERR_TEXT_TO_SPEECH_SERVICE_INVALID = 7012;

	public static int AV_ERR_EFFECT_OPENFILE_FAILED = 4051;

	public static int AV_ERR_EFFECT_DECODER_FAILED = 4053;

	public static int AV_ERR_EFFECT_STATE_ILLIGAL = 4057;

	public static int AV_ERR_SYSTEM_LOOPBACK_START_FAILED = 4058;

	public static int AV_ERR_SYSTEM_LOOPBACK_THIRD_PARTY_APP_EXIT = 4059;

	public static int AV_ERR_RECORD_CREATEFILE_FAILED = 5001;

	public static int AV_ERR_RECORD_FILE_FORAMT_NOTSUPPORT = 5002;

	public static int AV_ERR_RECORD_START_FAILED = 5003;

	public static int AV_ERR_OPERATOR_DEVICE_REFUSED = 6999;

	public static int AV_ERR_VOICE_UPLOAD_FILE_ACCESSERROR = 8193;

	public static int AV_ERR_VOICE_UPLOAD_SIGN_CHECK_FAIL = 8194;

	public static int AV_ERR_VOICE_UPLOAD_COS_INTERNAL_FAIL = 8195;

	public static int AV_ERR_VOICE_UPLOAD_GET_TOKEN_NETWORK_FAIL = 8196;

	public static int AV_ERR_VOICE_UPLOAD_SYSTEM_INNER_ERROR = 8197;

	public static int AV_ERR_VOICE_UPLOAD_COS_NETWORK_FAIL = 8199;

	public static int AV_ERR_VOICE_DOWNLOAD_FILE_URL_INVALID = 12288;

	public static int AV_ERR_VOICE_DOWNLOAD_FILE_ACCESS_ERROR = 12289;

	public static int AV_ERR_VOICE_DOWNLOAD_SIGN_CHECK_FAIL = 12290;

	public static int AV_ERR_VOICE_DOWNLOAD_COS_INTERNAL_FAIL = 12291;

	public static int AV_ERR_VOICE_DOWNLOAD_GET_SIGN_NETWORK_FAIL = 12293;

	public static int AV_ERR_VOICE_DOWNLOAD_SYSTEM_INNER_ERROR = 12294;

	public static int AV_ERR_VOICE_DOWNLOAD_COS_NETWORK_FAIL = 12298;

	public static int AV_ERR_VOICE_DOWNLOAD_OVER_LIMIT = 12299;

	public static int AV_ERR_VOICE_S2T_SYSTEM_INTERNAL_ERROR = 32769;

	public static int AV_ERR_VOICE_S2T_NETWORK_FAIL = 32770;

	public static int AV_ERR_VOICE_S2T_RSP_DATA_DECODE_FAIL = 32772;

	public static int AV_ERR_VOICE_S2T_SIGN_CHECK_FAIL = 32776;

	public static int AV_ERR_VOICE_S2T_PARAM_NULL = 32784;

	public static int AV_ERR_VOICE_S2T_AUTO_SPEECH_REC_ERROR = 32785;

	public static int AV_ERR_VOICE_STREAMIN_RUNING_ERROR = 32786;

	public static int AV_ERR_VOICE_FILE_COS_NOT_FOUND = 32789;

	public static int AV_ERR_SHARE_ROOM_INVALID_VALUE = 700004;

	public static int AV_ERR_SHARE_ROOM_TIMEOUT = 700008;

	public static int AV_ERR_SHARE_ROOM_TYPE_ERR = 700016;

	public static string GMESDK_VERSION_local = "local";
}

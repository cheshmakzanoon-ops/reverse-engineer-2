using System;

namespace KWSVerification;

[Serializable]
public class AgeVerifyResponse : BaseResponse
{
	public string url;

	public int age_verification_cooldown;
}

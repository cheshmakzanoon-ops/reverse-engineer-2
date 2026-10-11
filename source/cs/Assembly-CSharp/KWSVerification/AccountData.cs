using System;

namespace KWSVerification;

[Serializable]
public class AccountData
{
	public AccountInfo accountInfo;

	public string age_verification_cooldown;

	public string age_verified;

	public string selfMail;

	public bool need_verify;

	public bool can_enter;

	public bool need_account;
}

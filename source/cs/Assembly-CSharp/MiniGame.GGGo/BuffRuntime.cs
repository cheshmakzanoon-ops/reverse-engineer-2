using System.Collections.Generic;
using Box2DSharp.Common;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class BuffRuntime
{
	public FP ElapsedTime;

	public FP ElapsedIntervalTime;

	public FP Duration
	{
		get
		{
			if (!(DurationOverride >= FP.Zero))
			{
				return BuffData.Duration;
			}
			return DurationOverride;
		}
	}

	public FP DurationOverride { private get; set; }

	public List<IAction> OnApplyActions { get; private set; }

	public List<IAction> OnExpireActions { get; private set; }

	public List<IAction> OnIntervalActions { get; private set; }

	public List<ICondition> Conditions { get; private set; }

	public BuffData BuffData { get; private set; }

	public BuffRuntime(BuffData buffData)
	{
		BuffData = buffData;
	}

	public void Init(List<IAction> onApplyActions, List<IAction> onExpireActions, List<IAction> onIntervalActions = null, List<ICondition> conditions = null)
	{
		OnApplyActions = onApplyActions;
		OnExpireActions = onExpireActions;
		OnIntervalActions = onIntervalActions;
		Conditions = conditions;
	}

	public BuffRuntime Clone()
	{
		return new BuffRuntime(BuffData)
		{
			ElapsedTime = ElapsedTime,
			ElapsedIntervalTime = ElapsedIntervalTime,
			DurationOverride = DurationOverride,
			OnApplyActions = OnApplyActions,
			OnExpireActions = OnExpireActions,
			OnIntervalActions = OnIntervalActions,
			Conditions = Conditions
		};
	}

	public bool Equals(BuffRuntime other)
	{
		if (ElapsedTime != other.ElapsedTime || ElapsedIntervalTime != other.ElapsedIntervalTime || DurationOverride != other.DurationOverride || OnApplyActions != other.OnApplyActions || OnExpireActions != other.OnExpireActions || OnIntervalActions != other.OnIntervalActions || Conditions != other.Conditions)
		{
			return false;
		}
		if (!BuffData.Equals(other.BuffData))
		{
			return false;
		}
		return true;
	}
}

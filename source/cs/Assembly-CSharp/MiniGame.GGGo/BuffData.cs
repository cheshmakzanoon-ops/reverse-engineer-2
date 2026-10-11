using System.Collections.Generic;
using Box2DSharp.Common;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class BuffData
{
	public int BuffID { get; private set; }

	public FP Duration { get; private set; }

	public FP Interval { get; private set; }

	public List<IAction> OnApplyActions { get; private set; }

	public List<IAction> OnExpireActions { get; private set; }

	public List<IAction> OnIntervalActions { get; private set; }

	public List<ICondition> Conditions { get; private set; }

	public BuffData(int buffId)
	{
		BuffID = buffId;
	}

	public void Init(FP duration, FP interval, List<IAction> onApplyActions = null, List<IAction> onExpireActions = null, List<IAction> onIntervalActions = null, List<ICondition> conditions = null)
	{
		Duration = duration;
		Interval = interval;
		OnApplyActions = onApplyActions;
		OnExpireActions = onExpireActions;
		OnIntervalActions = onIntervalActions;
		Conditions = conditions;
	}
}

using System;
using System.Collections.Generic;

namespace MiniGame.Core;

public class Trigger
{
	public List<Type> Events;

	public List<ICondition> Conditions;

	public List<IAction> Actions;

	public int TriggerLimit;

	public int TriggerCount;

	public bool IsExhausted
	{
		get
		{
			if (TriggerLimit > 0)
			{
				return TriggerCount >= TriggerLimit;
			}
			return false;
		}
	}

	public Trigger Clone()
	{
		Trigger trigger = new Trigger
		{
			Events = new List<Type>(),
			Conditions = new List<ICondition>(),
			Actions = new List<IAction>(),
			TriggerLimit = TriggerLimit,
			TriggerCount = TriggerCount
		};
		trigger.Events.AddRange(Events);
		if (Conditions != null)
		{
			for (int i = 0; i < Conditions.Count; i++)
			{
				trigger.Conditions.Add(Conditions[i].Clone());
			}
		}
		for (int j = 0; j < Actions.Count; j++)
		{
			trigger.Actions.Add(Actions[j].Clone());
		}
		return trigger;
	}

	public bool Equals(Trigger other)
	{
		if ((Events == null && other.Events != null) || (Events != null && other.Events == null))
		{
			return false;
		}
		if (Events != null && other.Events != null)
		{
			if (Events.Count != other.Events.Count)
			{
				return false;
			}
			for (int i = 0; i < Events.Count; i++)
			{
				if (Events[i] != other.Events[i])
				{
					return false;
				}
			}
		}
		if ((Conditions == null && other.Conditions != null) || (Conditions != null && other.Conditions == null))
		{
			return false;
		}
		if (Conditions != null && other.Conditions != null)
		{
			if (Conditions.Count != other.Conditions.Count)
			{
				return false;
			}
			for (int j = 0; j < Conditions.Count; j++)
			{
				if (!Conditions[j].Equals(other.Conditions[j]))
				{
					return false;
				}
			}
		}
		if ((Actions == null && other.Actions != null) || (Actions != null && other.Actions == null))
		{
			return false;
		}
		if (Actions != null && other.Actions != null)
		{
			if (Actions.Count != other.Actions.Count)
			{
				return false;
			}
			for (int k = 0; k < Actions.Count; k++)
			{
				if (!Actions[k].Equals(other.Actions[k]))
				{
					return false;
				}
			}
		}
		if (TriggerLimit != other.TriggerLimit || TriggerCount != other.TriggerCount)
		{
			return false;
		}
		return true;
	}
}

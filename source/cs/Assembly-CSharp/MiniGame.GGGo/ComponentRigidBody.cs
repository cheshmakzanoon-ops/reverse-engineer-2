using System;
using Leopotam.EcsLite;
using MiniGame.OdinInspector;

namespace MiniGame.GGGo;

public struct ComponentRigidBody : TEcsPoolDelegate<ComponentRigidBody>, IEcsPoolDelegate, IEcsAutoReset<ComponentRigidBody>, IEcsAutoCopy<ComponentRigidBody>, IEcsAutoSnapshot<ComponentRigidBody>
{
	[LabelText("连续检测模式")]
	public bool ContinuousDetection;

	[LabelText("是否在地面上")]
	public bool IsGrounded;

	public int PrevTriggerCount;

	public int[] PrevTriggerIds;

	private const int MaxPrevTriggers = 32;

	public Type DelegateType => typeof(ComponentRigidBody);

	public void AutoReset(ref ComponentRigidBody c, EcsWorld world, int entity)
	{
		c.PrevTriggerIds = null;
		c.PrevTriggerCount = 0;
		c.IsGrounded = false;
		c.ContinuousDetection = false;
	}

	public void AutoCopy(ref ComponentRigidBody src, ref ComponentRigidBody dst)
	{
		throw new NotSupportedException("ComponentRigidBody不支持拷贝");
	}

	public object TakeSnapshot(ref ComponentRigidBody c, EcsWorld world, int entity, object env)
	{
		ComponentRigidBodySnapshotData componentRigidBodySnapshotData = new ComponentRigidBodySnapshotData();
		componentRigidBodySnapshotData.IsGrounded = c.IsGrounded;
		componentRigidBodySnapshotData.ContinuousDetection = c.ContinuousDetection;
		componentRigidBodySnapshotData.PrevTriggerCount = c.PrevTriggerCount;
		if (c.PrevTriggerIds == null || c.PrevTriggerCount <= 0)
		{
			componentRigidBodySnapshotData.PrevTriggerIds = Array.Empty<int>();
		}
		else
		{
			int num = Math.Min(c.PrevTriggerCount, c.PrevTriggerIds.Length);
			componentRigidBodySnapshotData.PrevTriggerIds = new int[num];
			Array.Copy(c.PrevTriggerIds, 0, componentRigidBodySnapshotData.PrevTriggerIds, 0, num);
		}
		return componentRigidBodySnapshotData;
	}

	public void RestoreSnapshot(ref ComponentRigidBody c, EcsWorld world, int entity, object data, object env)
	{
		if (!(data is ComponentRigidBodySnapshotData componentRigidBodySnapshotData))
		{
			c.IsGrounded = false;
			c.ContinuousDetection = false;
			c.PrevTriggerCount = 0;
			c.PrevTriggerIds = null;
			return;
		}
		c.IsGrounded = componentRigidBodySnapshotData.IsGrounded;
		c.ContinuousDetection = componentRigidBodySnapshotData.ContinuousDetection;
		c.PrevTriggerCount = Math.Max(0, Math.Min(componentRigidBodySnapshotData.PrevTriggerCount, 32));
		c.PrevTriggerIds = new int[32];
		if (componentRigidBodySnapshotData.PrevTriggerIds != null && componentRigidBodySnapshotData.PrevTriggerIds.Length != 0)
		{
			int length = Math.Min(componentRigidBodySnapshotData.PrevTriggerIds.Length, c.PrevTriggerIds.Length);
			Array.Copy(componentRigidBodySnapshotData.PrevTriggerIds, 0, c.PrevTriggerIds, 0, length);
		}
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		ComponentRigidBodySnapshotData componentRigidBodySnapshotData = a as ComponentRigidBodySnapshotData;
		ComponentRigidBodySnapshotData componentRigidBodySnapshotData2 = b as ComponentRigidBodySnapshotData;
		if (componentRigidBodySnapshotData == null && componentRigidBodySnapshotData2 == null)
		{
			return true;
		}
		if (componentRigidBodySnapshotData == null || componentRigidBodySnapshotData2 == null)
		{
			return false;
		}
		if (componentRigidBodySnapshotData.IsGrounded != componentRigidBodySnapshotData2.IsGrounded)
		{
			return false;
		}
		if (componentRigidBodySnapshotData.ContinuousDetection != componentRigidBodySnapshotData2.ContinuousDetection)
		{
			return false;
		}
		if (componentRigidBodySnapshotData.PrevTriggerCount != componentRigidBodySnapshotData2.PrevTriggerCount)
		{
			return false;
		}
		int prevTriggerCount = componentRigidBodySnapshotData.PrevTriggerCount;
		for (int i = 0; i < prevTriggerCount; i++)
		{
			int num = ((componentRigidBodySnapshotData.PrevTriggerIds != null && i < componentRigidBodySnapshotData.PrevTriggerIds.Length) ? componentRigidBodySnapshotData.PrevTriggerIds[i] : 0);
			int num2 = ((componentRigidBodySnapshotData2.PrevTriggerIds != null && i < componentRigidBodySnapshotData2.PrevTriggerIds.Length) ? componentRigidBodySnapshotData2.PrevTriggerIds[i] : 0);
			if (num != num2)
			{
				return false;
			}
		}
		return true;
	}
}

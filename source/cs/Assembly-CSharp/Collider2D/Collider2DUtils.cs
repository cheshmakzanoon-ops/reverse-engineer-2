using System;
using System.Collections.Generic;
using UnityEngine;

namespace Collider2D;

public static class Collider2DUtils
{
	private static ISpaceQuery _spaceQuery;

	private static Agent[] _agents;

	private static int _agentCount;

	private static Dictionary<long, int> _agentIdMap;

	private static int[] _queriedAgents;

	private static Transform[] _agentTransforms;

	private static bool _isDebug;

	private static List<int> _sortedList;

	private static void Init()
	{
		if (_agents == null)
		{
			_agents = new Agent[512];
		}
		if (_agentIdMap == null)
		{
			_agentIdMap = new Dictionary<long, int>(512);
		}
		if (_spaceQuery == null)
		{
			_spaceQuery = new GridSpace();
		}
		if (_queriedAgents == null)
		{
			_queriedAgents = new int[100];
		}
		if (_agentTransforms == null)
		{
			_agentTransforms = new Transform[512];
		}
		if (_sortedList == null)
		{
			_sortedList = new List<int>();
		}
	}

	public static bool TryGetAgent(int uid, out Agent agent)
	{
		Init();
		agent = default(Agent);
		if (!_agentIdMap.TryGetValue(uid, out var value))
		{
			return false;
		}
		agent = _agents[value];
		return true;
	}

	public static void SetDebug(bool value)
	{
		_isDebug = value;
	}

	public static bool IsDebug()
	{
		return _isDebug;
	}

	public static void AddAgent(int uid, int colliderId, Collider unityCollider)
	{
		if (unityCollider == null)
		{
			Debug.LogError($"unityCollider is null! agentId:{uid}");
			return;
		}
		Init();
		bool flag = false;
		Transform transform = unityCollider.transform;
		if (_agentIdMap.TryGetValue(uid, out var value))
		{
			_agents[value].ColliderId = colliderId;
			if (_agentTransforms[value] != transform)
			{
				flag = true;
				_agentTransforms[value] = transform;
			}
		}
		else
		{
			value = _agentCount++;
			_agentIdMap[uid] = value;
			_agents[value] = new Agent
			{
				Id = uid,
				ColliderId = colliderId
			};
			_agentTransforms[value] = transform;
			if (_agentCount >= _agents.Length)
			{
				Agent[] array = new Agent[_agents.Length << 1];
				Array.Copy(_agents, array, _agents.Length);
				_agents = array;
			}
			if (_agentCount >= _agentTransforms.Length)
			{
				Transform[] array2 = new Transform[_agentTransforms.Length << 1];
				Array.Copy(_agentTransforms, array2, _agentTransforms.Length);
				_agentTransforms = array2;
			}
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		float colliderCenterX = 0f;
		float colliderCenterY = 0f;
		float colliderExtendX = 0.5f;
		float colliderExtendY = 0.5f;
		Collider2DType collider2DType = Collider2DType.Circle;
		if (unityCollider is BoxCollider boxCollider)
		{
			colliderCenterX = boxCollider.center.x;
			colliderCenterY = boxCollider.center.z;
			colliderExtendX = boxCollider.size.x * 0.5f;
			colliderExtendY = boxCollider.size.z * 0.5f;
			collider2DType = Collider2DType.Cube;
		}
		else if (unityCollider is CapsuleCollider capsuleCollider)
		{
			colliderCenterX = capsuleCollider.center.x;
			colliderCenterY = capsuleCollider.center.z;
			colliderExtendX = (colliderExtendY = capsuleCollider.radius);
			collider2DType = Collider2DType.Circle;
		}
		else if (unityCollider is SphereCollider sphereCollider)
		{
			colliderCenterX = sphereCollider.center.x;
			colliderCenterY = sphereCollider.center.z;
			colliderExtendX = (colliderExtendY = sphereCollider.radius);
			collider2DType = Collider2DType.Circle;
		}
		_agents[value].SetColliderData(collider2DType, colliderCenterX, colliderCenterY, colliderExtendX, colliderExtendY, unityCollider.gameObject.layer);
		if (_isDebug)
		{
			transform.TryGetComponent<AgentDebug>(out var component);
			if (component == null)
			{
				component = transform.gameObject.AddComponent<AgentDebug>();
			}
			component.enabled = true;
			component.Uid = uid;
		}
	}

	public static void RemoveAgent(long uid)
	{
		if (_agentIdMap == null || !_agentIdMap.TryGetValue(uid, out var value))
		{
			return;
		}
		if (_isDebug)
		{
			_agentTransforms[value].TryGetComponent<AgentDebug>(out var component);
			if (component != null)
			{
				component.enabled = false;
				component.Uid = -1;
			}
		}
		int num = _agentCount - 1;
		if (value < num)
		{
			Agent agent = _agents[num];
			_agents[value] = agent;
			Transform transform = _agentTransforms[num];
			_agentTransforms[value] = transform;
			_agentIdMap[agent.Id] = value;
		}
		_agentCount--;
		_agentIdMap.Remove(uid);
	}

	public static void UpdateAgents()
	{
		if (_agentTransforms != null)
		{
			int num = _agentTransforms.Length;
			for (int i = 0; i < num; i++)
			{
				Transform transform = _agentTransforms[i];
				_agents[i].ResetByTransform(transform);
			}
		}
	}

	public static void Build(int frame)
	{
		if (_spaceQuery != null)
		{
			_spaceQuery.Build(_agents, _agentCount, frame);
			if (_isDebug)
			{
				_spaceQuery.DebugLine();
			}
		}
	}

	public static int OverlapFastCapsule2DCollider(Vector2 startPoint, Vector2 endPoint, float capsuleRadius, int layerMask, ref int[] detectedColliderIds)
	{
		if (_spaceQuery == null)
		{
			return 0;
		}
		_sortedList.Clear();
		float num = float.PositiveInfinity;
		int num2 = -1;
		float num3 = 0f;
		int num4 = -1;
		int num5 = 0;
		float x = startPoint.x;
		float y = startPoint.y;
		float x2 = endPoint.x;
		float y2 = endPoint.y;
		float num6 = x2 - x;
		float num7 = y2 - y;
		float num8 = num6 * num6 + num7 * num7;
		float posX = x + num6 * 0.5f;
		float posY = y + num7 * 0.5f;
		float radis = Mathf.Sqrt(num8) * 0.5f + capsuleRadius;
		_spaceQuery.QueryNearAgents(posX, posY, radis, ref _queriedAgents, layerMask, out var queriedCount);
		for (int i = 0; i < queriedCount; i++)
		{
			int num9 = _queriedAgents[i];
			Agent agent = _agents[num9];
			bool flag = false;
			float num10 = 0f;
			if (agent.ColliderType == Collider2DType.Circle)
			{
				float transformedColliderCenterX = agent.TransformedColliderCenterX;
				float transformedColliderCenterY = agent.TransformedColliderCenterY;
				float transformedColliderExtendX = agent.TransformedColliderExtendX;
				float num11 = transformedColliderCenterX - x2;
				float num12 = transformedColliderCenterY - y2;
				float num13 = transformedColliderExtendX + capsuleRadius;
				float num14 = num13 * num13;
				if (num8 <= float.Epsilon)
				{
					num10 = num11 * num11 + num12 * num12;
					flag = num10 < num14;
					num10 = Mathf.Abs(num11) + Mathf.Abs(num12);
				}
				else
				{
					float num15 = 0f;
					float num16 = num11 * num6 + num12 * num7;
					if (num16 > 0f)
					{
						num15 = num11 * num11 + num12 * num12;
					}
					else if (num16 + num8 < 0f)
					{
						float num17 = transformedColliderCenterX - x;
						float num18 = transformedColliderCenterY - y;
						num15 = num17 * num17 + num18 * num18;
						num10 = Mathf.Abs(num17) + Mathf.Abs(num18);
					}
					else
					{
						float num19 = num16 / num8;
						float num20 = x2 + num6 * num19;
						float num21 = y2 + num7 * num19;
						float num22 = transformedColliderCenterX - num20;
						float num23 = transformedColliderCenterY - num21;
						num15 = num22 * num22 + num23 * num23;
						num10 = Mathf.Abs(num22) + Mathf.Abs(num23);
					}
					flag = num15 < num14;
				}
			}
			else if (agent.ColliderType == Collider2DType.Cube)
			{
				float num24 = agent.TransformedAABBMinX - capsuleRadius;
				float num25 = agent.TransformedAABBMinY - capsuleRadius;
				float num26 = agent.TransformedAABBMaxX + capsuleRadius;
				float num27 = agent.TransformedAABBMaxY + capsuleRadius;
				float num28 = x2 - x;
				float num29 = y2 - y;
				float num30 = Math.Abs(num28);
				float num31 = Math.Abs(num29);
				if ((!(num30 < float.Epsilon) || (!(x < num24) && !(x > num26))) && (!(num31 < float.Epsilon) || (!(y < num25) && !(y > num27))))
				{
					float num32 = 1f / num28;
					float num33 = (num24 - x) * num32;
					float num34 = (num26 - x) * num32;
					if (num33 > num34)
					{
						float num35 = num33;
						num33 = num34;
						num34 = num35;
					}
					float num36 = ((num33 < 0f) ? 0f : num33);
					float num37 = ((num34 > 1f) ? 1f : num34);
					if (num36 <= num37)
					{
						float num38 = 1f / num29;
						num33 = (num25 - y) * num38;
						num34 = (num27 - y) * num38;
						if (num33 > num34)
						{
							float num39 = num33;
							num33 = num34;
							num34 = num39;
						}
						float num40 = ((num33 < 0f) ? 0f : num33);
						num37 = ((num34 > 1f) ? 1f : num34);
						if (num40 <= num37)
						{
							float transformedColliderCenterX2 = agent.TransformedColliderCenterX;
							float transformedColliderCenterY2 = agent.TransformedColliderCenterY;
							float f = transformedColliderCenterX2 - x2;
							float f2 = transformedColliderCenterY2 - y2;
							num10 = Mathf.Abs(f) + Mathf.Abs(f2);
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				_sortedList.Add(agent.ColliderId);
				if (num10 < num)
				{
					num = num10;
					num2 = num5;
				}
				else if (num10 > num3)
				{
					num3 = num10;
					num4 = num5;
				}
				num5++;
			}
		}
		if (num5 >= detectedColliderIds.Length)
		{
			int num41 = detectedColliderIds.Length * 2;
			int[] array = new int[(num5 >= num41) ? num5 : num41];
			detectedColliderIds = array;
		}
		if (num2 > 0)
		{
			int value = _sortedList[num2];
			_sortedList[num2] = _sortedList[0];
			_sortedList[0] = value;
		}
		if (num4 >= 0 && num4 != num5 - 1)
		{
			int value2 = _sortedList[num4];
			_sortedList[num4] = _sortedList[num5 - 1];
			_sortedList[num5 - 1] = value2;
		}
		for (int j = 0; j < num5; j++)
		{
			detectedColliderIds[j] = _sortedList[j];
		}
		return num5;
	}

	public static int OverlapCircle2DCollider(Vector2 circleCenter, Vector2 targetPos, float circleRadius, int layerMask, ref int[] detectedColliderIds)
	{
		if (_spaceQuery == null)
		{
			return 0;
		}
		_sortedList.Clear();
		float num = float.PositiveInfinity;
		int num2 = -1;
		float num3 = 0f;
		int num4 = -1;
		int num5 = 0;
		float x = circleCenter.x;
		float y = circleCenter.y;
		float x2 = targetPos.x;
		float y2 = targetPos.y;
		_spaceQuery.QueryNearAgents(x, y, circleRadius, ref _queriedAgents, layerMask, out var queriedCount);
		for (int i = 0; i < queriedCount; i++)
		{
			int num6 = _queriedAgents[i];
			Agent agent = _agents[num6];
			bool flag = false;
			float num7 = 0f;
			if (agent.ColliderType == Collider2DType.Circle)
			{
				float num8 = circleRadius + agent.TransformedColliderExtendX;
				float num9 = x - agent.TransformedColliderCenterX;
				float num10 = y - agent.TransformedColliderCenterY;
				float f = x2 - agent.TransformedColliderCenterX;
				float f2 = y2 - agent.TransformedColliderCenterY;
				num7 = num9 * num9 + num10 * num10;
				flag = num7 <= num8 * num8;
				Debug.Log($"{num7} {num8 * num8}");
				num7 = Mathf.Abs(f) + Mathf.Abs(f2);
			}
			else if (agent.ColliderType == Collider2DType.Cube)
			{
				float transformedAABBMinX = agent.TransformedAABBMinX;
				float transformedAABBMinY = agent.TransformedAABBMinY;
				float transformedAABBMaxX = agent.TransformedAABBMaxX;
				float transformedAABBMaxY = agent.TransformedAABBMaxY;
				float num11 = x;
				if (transformedAABBMaxX < num11)
				{
					num11 = transformedAABBMaxX;
				}
				if (transformedAABBMinX > num11)
				{
					num11 = transformedAABBMinX;
				}
				float num12 = y;
				if (transformedAABBMaxY < num12)
				{
					num12 = transformedAABBMaxY;
				}
				if (transformedAABBMinY > num12)
				{
					num12 = transformedAABBMinY;
				}
				float num13 = x - num11;
				float num14 = y - num12;
				num7 = num13 * num13 + num14 * num14;
				flag = num7 < circleRadius * circleRadius;
				Debug.Log($"{num7} {circleRadius * circleRadius}");
				float f3 = x2 - agent.TransformedColliderCenterX;
				float f4 = y2 - agent.TransformedColliderCenterY;
				num7 = Mathf.Abs(f3) + Mathf.Abs(f4);
			}
			if (flag)
			{
				_sortedList.Add(agent.ColliderId);
				if (num7 < num)
				{
					num = num7;
					num2 = num5;
				}
				else if (num7 > num3)
				{
					num3 = num7;
					num4 = num5;
				}
				num5++;
			}
		}
		if (num5 >= detectedColliderIds.Length)
		{
			int num15 = detectedColliderIds.Length * 2;
			int[] array = new int[(num5 >= num15) ? num5 : num15];
			detectedColliderIds = array;
		}
		if (num2 > 0)
		{
			int value = _sortedList[num2];
			_sortedList[num2] = _sortedList[0];
			_sortedList[0] = value;
		}
		if (num4 >= 0 && num4 != num5 - 1)
		{
			int value2 = _sortedList[num4];
			_sortedList[num4] = _sortedList[num5 - 1];
			_sortedList[num5 - 1] = value2;
		}
		for (int j = 0; j < num5; j++)
		{
			detectedColliderIds[j] = _sortedList[j];
		}
		return num5;
	}

	public static int OverlapAABB2DCollider(Vector2 targetPos, float aAABBMinX, float aAABBMinY, float aAABBMaxX, float aAABBMaxY, int layerMask, ref int[] detectedColliderIds)
	{
		if (_spaceQuery == null)
		{
			return 0;
		}
		_sortedList.Clear();
		float num = float.PositiveInfinity;
		int num2 = -1;
		float num3 = 0f;
		int num4 = -1;
		int num5 = 0;
		float num6 = (aAABBMaxX - aAABBMinX) * 0.5f;
		float num7 = (aAABBMaxY - aAABBMinY) * 0.5f;
		float radis = Mathf.Sqrt(num6 * num6 + num7 * num7);
		float x = targetPos.x;
		float y = targetPos.y;
		_spaceQuery.QueryNearAgents(aAABBMinX + num6, aAABBMinY + num7, radis, ref _queriedAgents, layerMask, out var queriedCount);
		if (queriedCount == 0)
		{
			return 0;
		}
		for (int i = 0; i < queriedCount; i++)
		{
			int num8 = _queriedAgents[i];
			Agent agent = _agents[num8];
			bool flag = false;
			float num9 = 0f;
			if (agent.ColliderType == Collider2DType.Circle)
			{
				flag = IntersectDetect2D.AABBIntersectCircle2D(aAABBMinX, aAABBMinY, aAABBMaxX, aAABBMaxY, agent.TransformedColliderCenterX, agent.TransformedColliderCenterY, agent.TransformedColliderExtendX);
				float f = x - agent.TransformedColliderCenterX;
				float f2 = y - agent.TransformedColliderCenterY;
				num9 = Mathf.Abs(f) + Mathf.Abs(f2);
			}
			else if (agent.ColliderType == Collider2DType.Cube)
			{
				flag = IntersectDetect2D.AABBIntersectAABB2D(agent.TransformedAABBMinX, agent.TransformedAABBMinY, agent.TransformedAABBMaxX, agent.TransformedAABBMaxY, aAABBMinX, aAABBMinY, aAABBMaxX, aAABBMaxY);
				float f3 = x - agent.TransformedColliderCenterX;
				float f4 = y - agent.TransformedColliderCenterY;
				num9 = Mathf.Abs(f3) + Mathf.Abs(f4);
			}
			if (flag)
			{
				_sortedList.Add(agent.ColliderId);
				if (num9 < num)
				{
					num = num9;
					num2 = num5;
				}
				else if (num9 > num3)
				{
					num3 = num9;
					num4 = num5;
				}
				num5++;
			}
		}
		if (num5 >= detectedColliderIds.Length)
		{
			int num10 = detectedColliderIds.Length * 2;
			int[] array = new int[(num5 >= num10) ? num5 : num10];
			detectedColliderIds = array;
		}
		if (num2 > 0)
		{
			int value = _sortedList[num2];
			_sortedList[num2] = _sortedList[0];
			_sortedList[0] = value;
		}
		if (num4 >= 0 && num4 != num5 - 1)
		{
			int value2 = _sortedList[num4];
			_sortedList[num4] = _sortedList[num5 - 1];
			_sortedList[num5 - 1] = value2;
		}
		for (int j = 0; j < num5; j++)
		{
			detectedColliderIds[j] = _sortedList[j];
		}
		return num5;
	}

	public static void Clear()
	{
		if (_agents != null)
		{
			Array.Clear(_agents, 0, _agents.Length);
		}
		_agentCount = 0;
		if (_agentTransforms != null)
		{
			Array.Clear(_agentTransforms, 0, _agentTransforms.Length);
		}
		if (_queriedAgents != null)
		{
			Array.Clear(_queriedAgents, 0, _queriedAgents.Length);
		}
		_agentIdMap?.Clear();
		_spaceQuery?.Clear();
	}
}

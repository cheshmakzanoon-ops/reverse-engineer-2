using System;
using System.Collections.Generic;
using System.IO;
using RVO;
using UnityEngine;
using VEngine;

public class LWBattleRVOManager
{
	private class RectAgentGroup
	{
		public int groupId;

		public readonly List<int> agentIds = new List<int>(8);

		public readonly HashSet<int> agentIdSet = new HashSet<int>();

		public float width;

		public int circleCount;

		public Vector3 lastPos;
	}

	private int _configCount;

	private int _finishConfigCount;

	private RVO.Vector2 _targetPosition;

	private readonly Dictionary<int, LWBattleRVOAgent> _rvoAgents = new Dictionary<int, LWBattleRVOAgent>(32);

	private Dictionary<int, RectAgentGroup> groups = new Dictionary<int, RectAgentGroup>();

	private int nextGroupId = 1;

	public RVO.Vector2 targetPosition => _targetPosition;

	public void InitLW(float timeStep, float neighborDist, int maxNeighbors, float timeHorizon, float timeHorizonObst, float radius, float maxSpeed, int step = 1, bool agentOpt = false)
	{
		Simulator.Instance.Clear();
		Simulator.Instance.SetNumWorkers(step);
		Simulator.Instance.optAgentUpdate = agentOpt;
		Simulator.Instance.setAgentDefaults(neighborDist, maxNeighbors, timeHorizon, timeHorizonObst, radius, maxSpeed, new RVO.Vector2(0f, 0f));
		_configCount = 0;
		_finishConfigCount = 0;
	}

	public void Append(string configPath, float offset)
	{
		_configCount++;
		Asset req = GameEntry.Resource.LoadAssetAsync(configPath, typeof(TextAsset));
		Asset asset = req;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			using (BinaryReader binaryReader = new BinaryReader(new MemoryStream((req.asset as TextAsset).bytes)))
			{
				int num = binaryReader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					IList<RVO.Vector2> vertices = new List<RVO.Vector2>
					{
						new RVO.Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle() + offset),
						new RVO.Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle() + offset),
						new RVO.Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle() + offset),
						new RVO.Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle() + offset)
					};
					Simulator.Instance.addObstacle(vertices);
				}
			}
			_finishConfigCount++;
			if (_finishConfigCount == _configCount)
			{
				Simulator.Instance.processObstacles();
			}
		});
	}

	public void Destory()
	{
		Simulator.Instance.Clear();
	}

	public void SyncTargetPosition(float x, float z)
	{
		_targetPosition.x_ = x;
		_targetPosition.y_ = z;
	}

	public void Update(float x, float z)
	{
		Simulator.Instance.setTimeStep(Time.deltaTime);
		Simulator.Instance.doStep();
		SyncTargetPosition(x, z);
	}

	public int AddAgent(Vector3 position, GameObject gameObject, float speed, float radius, bool externalControl = false)
	{
		RVO.Vector2 position2 = default(RVO.Vector2);
		position2.x_ = position.x;
		position2.y_ = position.z;
		int num = Simulator.Instance.addAgent(position2);
		LWBattleRVOAgent lWBattleRVOAgent = gameObject.GetComponent<LWBattleRVOAgent>();
		if (lWBattleRVOAgent == null)
		{
			lWBattleRVOAgent = gameObject.AddComponent<LWBattleRVOAgent>();
		}
		lWBattleRVOAgent.sid = num;
		lWBattleRVOAgent.mgr = this;
		lWBattleRVOAgent.speed = speed;
		lWBattleRVOAgent.externalControl = externalControl;
		Simulator.Instance.setAgentRadius(num, radius);
		Simulator.Instance.setAgentMaxSpeed(num, speed * 2f);
		_rvoAgents[num] = lWBattleRVOAgent;
		return num;
	}

	public void DeleteAgent(int sid)
	{
		Simulator.Instance.delAgent(sid);
		_rvoAgents.Remove(sid);
	}

	public int CreateRectAgents(GameObject go)
	{
		BoxCollider component = go.GetComponent<BoxCollider>();
		if (component == null)
		{
			Debug.LogError("[LWBattleRVOManager.CreateRectAgents] " + go.name + " 没有 BoxCollider，无法构建矩形代理。");
			return -1;
		}
		Vector3 lossyScale = go.transform.lossyScale;
		float num = component.size.x * lossyScale.x;
		float num2 = component.size.z * lossyScale.z;
		bool flag = num >= num2;
		float num3 = (flag ? num : num2);
		float num4 = (flag ? num2 : num);
		float radius = num4 * 0.5f;
		int num5 = Mathf.Max(2, Mathf.CeilToInt(num3 / num4) + 1);
		float num6 = ((num5 == 1) ? 0f : (num3 / (float)(num5 - 1)));
		float num7 = (float)(num5 - 1) * num6;
		RectAgentGroup rectAgentGroup = new RectAgentGroup
		{
			groupId = nextGroupId++,
			width = num,
			circleCount = num5,
			lastPos = go.transform.position
		};
		for (int i = 0; i < num5; i++)
		{
			float num8 = -0.5f * num7 + (float)i * num6;
			Vector3 position = (flag ? new Vector3(num8, 0f, 0f) : new Vector3(0f, 0f, num8));
			Vector3 vector = go.transform.TransformPoint(position);
			int num9 = Simulator.Instance.addAgent(new RVO.Vector2(vector.x, vector.z));
			rectAgentGroup.agentIds.Add(num9);
			rectAgentGroup.agentIdSet.Add(num9);
			Simulator.Instance.setAgentRadius(num9, radius);
			Simulator.Instance.setAgentMaxSpeed(num9, 0f);
		}
		groups[rectAgentGroup.groupId] = rectAgentGroup;
		return rectAgentGroup.groupId;
	}

	public void UpdateRectAgents(GameObject go, int groupId)
	{
		if (groups.TryGetValue(groupId, out var value))
		{
			float num = value.width * 0.5f;
			float num2 = ((value.circleCount == 1) ? 0f : (value.width / (float)(value.circleCount - 1)));
			float f = go.transform.eulerAngles.y * (MathF.PI / 180f);
			UnityEngine.Vector2 vector = new UnityEngine.Vector2(Mathf.Cos(f), Mathf.Sin(f));
			Vector3 position = go.transform.position;
			for (int i = 0; i < value.circleCount; i++)
			{
				float num3 = 0f - num + (float)i * num2;
				UnityEngine.Vector2 vector2 = vector * num3;
				Vector3 vector3 = position + new Vector3(vector2.x, 0f, vector2.y);
				int agentNo = value.agentIds[i];
				Simulator.Instance.setAgentPosition(agentNo, new RVO.Vector2(vector3.x, vector3.z));
			}
			Vector3 forward = go.transform.forward;
			Vector3 rhs = position - value.lastPos;
			value.lastPos = position;
			if (Vector3.Dot(forward, rhs) >= 0f)
			{
				ApplyPushForces(value, new UnityEngine.Vector2(rhs.normalized.x, rhs.normalized.z));
			}
		}
	}

	private void ApplyPushForces(RectAgentGroup group, UnityEngine.Vector2 defaultDir)
	{
		float num = 1f;
		float num2 = 8f;
		Simulator instance = Simulator.Instance;
		Dictionary<int, RVO.Vector2> dictionary = new Dictionary<int, RVO.Vector2>();
		for (int i = 0; i < group.circleCount; i++)
		{
			int agentNo = group.agentIds[i];
			RVO.Vector2 agentPosition = instance.getAgentPosition(agentNo);
			float num3 = instance.getAgentRadius(agentNo) * num;
			int agentNumAgentNeighbors = instance.getAgentNumAgentNeighbors(agentNo);
			for (int j = 0; j < agentNumAgentNeighbors; j++)
			{
				int agentAgentNeighbor = instance.getAgentAgentNeighbor(agentNo, j);
				if (IsRectAgent(agentAgentNeighbor))
				{
					continue;
				}
				RVO.Vector2 agentPosition2 = instance.getAgentPosition(agentAgentNeighbor);
				float agentRadius = instance.getAgentRadius(agentAgentNeighbor);
				RVO.Vector2 vector = agentPosition2 - agentPosition;
				float num4 = Mathf.Sqrt(RVOMath.absSq(vector));
				float num5 = num3 + agentRadius - num4;
				if (!(num5 <= 0f))
				{
					RVO.Vector2 vector2 = ((num4 > 1E-06f) ? RVOMath.normalize(vector) : new RVO.Vector2(defaultDir.x, defaultDir.y));
					float num6 = num5;
					if (!dictionary.ContainsKey(agentAgentNeighbor))
					{
						dictionary[agentAgentNeighbor] = vector2 * num6;
					}
					else
					{
						dictionary[agentAgentNeighbor] += vector2 * num6;
					}
				}
			}
		}
		foreach (KeyValuePair<int, RVO.Vector2> item in dictionary)
		{
			int key = item.Key;
			if (_rvoAgents.TryGetValue(key, out var value) && value.IsExternalControl())
			{
				RVO.Vector2 vector3 = RVOMath.normalize(item.Value);
				RVO.Vector2 agentPosition3 = instance.getAgentPosition(key);
				agentPosition3 += vector3 * num2 * Time.deltaTime;
				instance.setAgentPosition(key, agentPosition3);
				RVO.Vector2 agentVelocity = instance.getAgentVelocity(key);
				agentVelocity += vector3;
				instance.setAgentVelocity(key, agentVelocity);
				value.TryUpdateTransform();
			}
		}
	}

	private bool IsRectAgent(int agentId)
	{
		foreach (RectAgentGroup value in groups.Values)
		{
			if (value.agentIdSet.Contains(agentId))
			{
				return true;
			}
		}
		return false;
	}

	public void RemoveRectAgents(int groupId)
	{
		if (!groups.TryGetValue(groupId, out var value))
		{
			return;
		}
		foreach (int agentId in value.agentIds)
		{
			Simulator.Instance.delAgent(agentId);
		}
		groups.Remove(groupId);
	}
}

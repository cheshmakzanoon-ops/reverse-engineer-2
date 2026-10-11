using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;
using GameFramework;
using GameKit.Base;
using Google.Protobuf.Collections;
using Protobuf;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using UnityEngine;
using XLua;

public class WorldMarchDataManager : WorldManagerBase
{
	private enum BatleResult
	{
		DEFAULT = -1,
		SELF_WIN,
		OTHER_WIN,
		DRAW
	}

	public enum BattleWordType
	{
		Normal,
		Cure,
		Skill
	}

	private enum EMarchStepUpdateMoveState
	{
		Invalid,
		UpdateVisible,
		UpdatePerformance,
		UpdateTroopLine
	}

	private enum CounterType
	{
		Squad,
		TroopLine,
		Count
	}

	private struct CountPair
	{
		public int displayCount;

		public int dataCount;

		public CountPair(int displayCount, int dataCount)
		{
			this.displayCount = displayCount;
			this.dataCount = dataCount;
		}
	}

	private sealed class MarchObjectCounter
	{
		private readonly CountPair[] _current;

		private readonly CountPair[] _last;

		public MarchObjectCounter()
		{
			int num = 2;
			_current = new CountPair[num];
			_last = new CountPair[num];
			ResetAll();
		}

		public void ResetAll()
		{
			int num = 2;
			for (int i = 0; i < num; i++)
			{
				_current[i] = new CountPair(0, 0);
				_last[i] = new CountPair(-1, -1);
			}
		}

		public void ResetCurrent()
		{
			int num = 2;
			for (int i = 0; i < num; i++)
			{
				_current[i] = new CountPair(0, 0);
			}
		}

		public void Accumulate(CounterType metric, bool isData, bool isDisplay)
		{
			CountPair countPair = _current[(int)metric];
			if (isData)
			{
				countPair.dataCount++;
			}
			if (isDisplay)
			{
				countPair.displayCount++;
			}
			_current[(int)metric] = countPair;
		}

		public bool Commit()
		{
			bool result = false;
			int num = 2;
			for (int i = 0; i < num; i++)
			{
				CountPair countPair = _current[i];
				CountPair countPair2 = _last[i];
				if (countPair.displayCount != countPair2.displayCount || countPair.dataCount != countPair2.dataCount)
				{
					_last[i] = countPair;
					result = true;
				}
			}
			return result;
		}

		public int GetLastDisplayCount(CounterType metric)
		{
			return _last[(int)metric].displayCount;
		}

		public int GetLastDataCount(CounterType metric)
		{
			return _last[(int)metric].dataCount;
		}
	}

	public struct LittleNormalMarchData
	{
		public long marchUuid;

		public long startTime;

		public long endTime;

		public long blackStartTime;

		public long blackEndTime;

		public long lastUpdateTime;

		public Vector2 startWorldPos;

		public Vector2 pathStartPos;

		public Vector2 targetWorldPos;

		public Vector2 moveDirection;

		public Vector2 mainWorldPos;

		public Vector2 currentWorldPos;

		public float remainDistance;

		public float moveSpeed;

		public float blackSpeed;

		public int ownerServer;

		public int ownerCurServerId;

		public PlayerType playerRelationType;

		public bool multiTroopLine;

		public bool troopLineIsVisible;

		public bool troopIsVisible;
	}

	public class MarchDataUpdater
	{
		private struct TempMarchData
		{
			public long uuid;

			public long startTime;

			public long endTime;

			public long blackStartTime;

			public long blackEndTime;

			public int startServer;

			public int startPointIdx;

			public int homeServer;

			public int homePointIdx;

			public int endServer;

			public int endPointIndex;

			public int pathStartServer;

			public int pathStartPointIndex;

			public int ownerServer;

			public PlayerType playerRelationType;

			public float speed;

			public float blackSpeed;
		}

		private WorldMarchDataManager manager;

		public LittleNormalMarchData[] normalMarchArray = new LittleNormalMarchData[128];

		private Queue<int> unusedIdx = new Queue<int>();

		public Dictionary<long, int> marchId2DataIndex = new Dictionary<long, int>(128);

		private Dictionary<long, string> marchId2AllianceID = new Dictionary<long, string>(128);

		private int marchDataCount;

		private int maxDataIndex;

		private const int COMPACT_THRESHOLD = 100;

		private long serverTimeNow;

		private string myUid;

		private string myAllianceUid;

		private Rect viewRect;

		private TempMarchData tmpMarchData;

		private Stopwatch stopwatch = new Stopwatch();

		private long updateElapsed;

		private long ServerTimeNow
		{
			get
			{
				if (serverTimeNow <= 0)
				{
					serverTimeNow = GameEntry.Timer.GetServerTime();
				}
				return serverTimeNow;
			}
		}

		private string MyUid
		{
			get
			{
				if (string.IsNullOrEmpty(myUid))
				{
					myUid = GameEntry.Data.Player.Uid;
				}
				return myUid;
			}
		}

		private string MyAllianceUid
		{
			get
			{
				if (string.IsNullOrEmpty(myAllianceUid))
				{
					myAllianceUid = GameEntry.Data.Player.GetAllianceId();
				}
				return myAllianceUid;
			}
		}

		public MarchDataUpdater(WorldMarchDataManager manager)
		{
			this.manager = manager;
		}

		public void Clear()
		{
			Array.Clear(normalMarchArray, 0, normalMarchArray.Length);
			unusedIdx.Clear();
			marchId2DataIndex.Clear();
			marchId2AllianceID.Clear();
			marchDataCount = 0;
			maxDataIndex = -1;
			serverTimeNow = 0L;
			myUid = string.Empty;
			myAllianceUid = string.Empty;
		}

		private bool IntersectsSegment(in Rect rect, in Vector2 p1, in Vector2 p2)
		{
			float num = Mathf.Min(p1.x, p2.x);
			float num2 = Mathf.Max(p1.x, p2.x);
			if (num2 > rect.xMax)
			{
				num2 = rect.xMax;
			}
			if (num < rect.xMin)
			{
				num = rect.xMin;
			}
			if (num > num2)
			{
				return false;
			}
			float num3 = Mathf.Min(p1.y, p2.y);
			float num4 = Mathf.Max(p1.y, p2.y);
			float num5 = p2.x - p1.x;
			if ((double)Mathf.Abs(num5) > 1.40129846432482E-45)
			{
				float num6 = (p2.y - p1.y) / num5;
				float num7 = p1.y - num6 * p1.x;
				num3 = num6 * num + num7;
				num4 = num6 * num2 + num7;
			}
			if (num3 > num4)
			{
				float num8 = num4;
				num4 = num3;
				num3 = num8;
			}
			if (num4 > rect.yMax)
			{
				num4 = rect.yMax;
			}
			if (num3 < rect.yMin)
			{
				num3 = rect.yMin;
			}
			return num3 <= num4;
		}

		private void RefreshPlayerRelationType(ref LittleNormalMarchData marchData)
		{
			if (marchId2AllianceID.TryGetValue(marchData.marchUuid, out var value))
			{
				if (value == MyAllianceUid)
				{
					marchData.playerRelationType = PlayerType.PlayerAlliance;
					return;
				}
				marchData.playerRelationType = manager?.world?.IsMyEnemy(marchData.ownerServer, value, marchData.ownerCurServerId) ?? PlayerType.PlayerOther;
				if (marchData.playerRelationType == PlayerType.PlayerNone)
				{
					marchData.playerRelationType = PlayerType.PlayerOther;
				}
			}
			else
			{
				marchData.playerRelationType = PlayerType.PlayerOther;
			}
		}

		public void TryUpdateFromProto(ISFSObject obj, Protobuf.WorldMarch proto)
		{
			NewMarchType type = (NewMarchType)proto.Type;
			if ((type != NewMarchType.NORMAL && type != NewMarchType.CROSS_NORMAL) || proto.Status != 1)
			{
				return;
			}
			tmpMarchData.uuid = obj.TryGetLong("uuid");
			tmpMarchData.ownerServer = proto.OwnerServer;
			tmpMarchData.startTime = proto.StartTime;
			tmpMarchData.endTime = proto.EndTime;
			tmpMarchData.speed = proto.Speed;
			tmpMarchData.startPointIdx = proto.StartPos;
			tmpMarchData.startServer = proto.SrcServer;
			tmpMarchData.homeServer = proto.OwnerCurServerId;
			tmpMarchData.homePointIdx = proto.MainPointId;
			tmpMarchData.endPointIndex = proto.TargetPos;
			tmpMarchData.endServer = proto.TargetServer;
			tmpMarchData.pathStartServer = proto.PathStartServerId;
			if (tmpMarchData.pathStartServer <= 0)
			{
				tmpMarchData.pathStartServer = tmpMarchData.startServer;
			}
			if (tmpMarchData.homeServer <= 0)
			{
				tmpMarchData.homeServer = tmpMarchData.ownerServer;
			}
			tmpMarchData.pathStartPointIndex = tmpMarchData.startPointIdx;
			string path = proto.Path;
			if (!string.IsNullOrEmpty(path))
			{
				ReadOnlySpan<char> span = MemoryExtensions.AsSpan(path);
				int num = span.IndexOf(';');
				if (num > 0 && int.TryParse(span.Slice(0, num).ToString(), out var result))
				{
					tmpMarchData.pathStartPointIndex = result;
				}
			}
			if (obj.TryGetString("ownerUid") == MyUid)
			{
				tmpMarchData.playerRelationType = PlayerType.PlayerSelf;
			}
			else
			{
				tmpMarchData.playerRelationType = PlayerType.PlayerNone;
				string allianceUid = proto.AllianceUid;
				if (!string.IsNullOrEmpty(allianceUid))
				{
					marchId2AllianceID[tmpMarchData.uuid] = allianceUid;
				}
			}
			if (proto.WorldId <= 0)
			{
				tmpMarchData.blackStartTime = proto.BlackStartTime;
				if (tmpMarchData.blackStartTime > 0)
				{
					tmpMarchData.blackEndTime = proto.BlackEndTime;
					tmpMarchData.blackSpeed = proto.BlackSpeed;
				}
				else
				{
					tmpMarchData.blackEndTime = 0L;
					tmpMarchData.blackSpeed = 0f;
				}
			}
			else
			{
				tmpMarchData.blackStartTime = 0L;
				tmpMarchData.blackEndTime = 0L;
				tmpMarchData.blackSpeed = 0f;
			}
			UpdateMarch(tmpMarchData.uuid);
		}

		public void TryUpdateFromProto(ISFSObject obj)
		{
			ByteArray byteArray = obj.GetByteArray("_proto");
			Protobuf.WorldMarch worldMarch = Protobuf.WorldMarch.Parser.ParseFrom(byteArray.Bytes);
			if (worldMarch != null)
			{
				TryUpdateFromProto(obj, worldMarch);
			}
		}

		private void CompactDataArray()
		{
			int num = 0;
			for (int i = 0; i <= maxDataIndex; i++)
			{
				ref LittleNormalMarchData reference = ref normalMarchArray[i];
				if (reference.marchUuid > 0)
				{
					if (i != num)
					{
						normalMarchArray[num] = reference;
						marchId2DataIndex[reference.marchUuid] = num;
						reference.marchUuid = 0L;
						reference.lastUpdateTime = 0L;
					}
					num++;
					if (num >= marchDataCount)
					{
						break;
					}
				}
			}
			maxDataIndex = marchDataCount - 1;
			unusedIdx.Clear();
		}

		private void UpdateMarch(long uuid)
		{
			if (uuid != tmpMarchData.uuid)
			{
				return;
			}
			if (!marchId2DataIndex.TryGetValue(uuid, out var value))
			{
				if (unusedIdx.Count <= 0)
				{
					int num = normalMarchArray.Length;
					if (marchDataCount >= num)
					{
						int newSize = Mathf.FloorToInt((float)num * 1.2f);
						Array.Resize(ref normalMarchArray, newSize);
					}
					value = marchDataCount;
				}
				else
				{
					value = unusedIdx.Dequeue();
				}
				maxDataIndex = ((maxDataIndex < value) ? value : maxDataIndex);
				marchDataCount++;
				marchId2DataIndex[uuid] = value;
			}
			Vector3 vector = TileCoord.TileIndexToWorld(tmpMarchData.startPointIdx, ForceChangeScene.World, tmpMarchData.startServer);
			Vector3 vector2 = TileCoord.TileIndexToWorld(tmpMarchData.endPointIndex, ForceChangeScene.World, tmpMarchData.endServer);
			Vector3 vector3 = TileCoord.TileIndexToWorld(tmpMarchData.pathStartPointIndex, ForceChangeScene.World, tmpMarchData.pathStartServer);
			ref LittleNormalMarchData reference = ref normalMarchArray[value];
			reference.marchUuid = tmpMarchData.uuid;
			reference.startTime = tmpMarchData.startTime;
			reference.endTime = tmpMarchData.endTime;
			reference.blackStartTime = tmpMarchData.blackStartTime;
			reference.blackEndTime = tmpMarchData.blackEndTime;
			reference.ownerServer = tmpMarchData.ownerServer;
			reference.ownerCurServerId = tmpMarchData.homeServer;
			reference.playerRelationType = tmpMarchData.playerRelationType;
			if (tmpMarchData.homeServer == tmpMarchData.startServer && tmpMarchData.homePointIdx == tmpMarchData.startPointIdx)
			{
				reference.multiTroopLine = false;
				reference.mainWorldPos.Set(vector.x, vector.z);
			}
			else if (tmpMarchData.homeServer == tmpMarchData.endServer && tmpMarchData.homePointIdx == tmpMarchData.endPointIndex)
			{
				reference.multiTroopLine = false;
				reference.mainWorldPos.Set(vector2.x, vector2.z);
			}
			else
			{
				reference.multiTroopLine = true;
				Vector3 vector4 = TileCoord.TileIndexToWorld(tmpMarchData.homePointIdx, ForceChangeScene.World, tmpMarchData.homeServer);
				reference.mainWorldPos.Set(vector4.x, vector4.z);
			}
			reference.startWorldPos.Set(vector.x, vector.z);
			reference.targetWorldPos.Set(vector2.x, vector2.z);
			reference.pathStartPos.Set(vector3.x, vector3.z);
			Vector2 vector5 = reference.targetWorldPos - reference.pathStartPos;
			reference.remainDistance = vector5.magnitude;
			reference.moveDirection = vector5.normalized;
			reference.moveSpeed = tmpMarchData.speed * 2f;
			reference.blackSpeed = tmpMarchData.blackSpeed * 2f;
			reference.troopLineIsVisible = IntersectsSegment(in viewRect, in reference.startWorldPos, in reference.targetWorldPos);
			if (!reference.troopLineIsVisible && reference.multiTroopLine)
			{
				reference.troopLineIsVisible = IntersectsSegment(in viewRect, in reference.mainWorldPos, in reference.startWorldPos);
			}
			long num2 = ServerTimeNow;
			if (num2 >= reference.endTime)
			{
				reference.currentWorldPos = reference.targetWorldPos;
				reference.remainDistance = 0f;
			}
			else
			{
				float num3 = 0f;
				if (reference.blackStartTime > 0)
				{
					if (num2 <= reference.blackStartTime)
					{
						num3 = (float)(num2 - reference.startTime) * 0.001f * reference.moveSpeed;
					}
					else if (num2 <= reference.blackEndTime)
					{
						num3 = (float)(reference.blackStartTime - reference.startTime) * 0.001f * reference.moveSpeed;
						num3 += (float)(num2 - reference.blackStartTime) * 0.001f * reference.blackSpeed;
					}
					else
					{
						num3 = (float)(reference.blackStartTime - reference.startTime) * 0.001f * reference.moveSpeed;
						num3 += (float)(reference.blackEndTime - reference.blackStartTime) * 0.001f * reference.blackSpeed;
						num3 += (float)(num2 - reference.blackEndTime) * 0.001f * reference.moveSpeed;
					}
				}
				else
				{
					num3 = (float)(num2 - reference.startTime) * 0.001f * reference.moveSpeed;
				}
				num3 = Mathf.Clamp(num3, 0f, reference.remainDistance);
				reference.currentWorldPos = reference.pathStartPos + reference.moveDirection * num3;
			}
			reference.troopIsVisible = viewRect.Contains(reference.currentWorldPos);
			if (reference.troopIsVisible && reference.playerRelationType == PlayerType.PlayerNone)
			{
				RefreshPlayerRelationType(ref reference);
			}
			reference.lastUpdateTime = ServerTimeNow;
			tmpMarchData.uuid = 0L;
		}

		public void DeltaUpdate(float deltaTime, long serverTimeNow, bool viewDirty, Rect viewRect)
		{
			if (!EnableWorldMarchDataOptHahaha)
			{
				return;
			}
			this.serverTimeNow = serverTimeNow;
			this.viewRect = viewRect;
			if (unusedIdx.Count > 100)
			{
				CompactDataArray();
			}
			stopwatch.Restart();
			for (int i = 0; i <= maxDataIndex; i++)
			{
				ref LittleNormalMarchData reference = ref normalMarchArray[i];
				if (reference.marchUuid <= 0)
				{
					continue;
				}
				if (viewDirty)
				{
					reference.troopLineIsVisible = IntersectsSegment(in viewRect, in reference.startWorldPos, in reference.targetWorldPos);
					if (!reference.troopLineIsVisible && reference.multiTroopLine)
					{
						reference.troopLineIsVisible = IntersectsSegment(in viewRect, in reference.mainWorldPos, in reference.startWorldPos);
					}
				}
				if (serverTimeNow < reference.endTime)
				{
					float value = 0f;
					if (reference.blackStartTime > 0)
					{
						if (serverTimeNow <= reference.blackStartTime)
						{
							value = (float)(serverTimeNow - reference.lastUpdateTime) * 0.001f * reference.moveSpeed;
						}
						else if (serverTimeNow <= reference.blackEndTime)
						{
							if (reference.lastUpdateTime >= reference.blackStartTime)
							{
								value = (float)(serverTimeNow - reference.lastUpdateTime) * 0.001f * reference.blackSpeed;
							}
							else
							{
								value = (float)(reference.blackStartTime - reference.lastUpdateTime) * 0.001f * reference.moveSpeed;
								long num = serverTimeNow - reference.blackStartTime;
								value += (float)num * 0.001f * reference.blackSpeed;
							}
						}
						else if (reference.lastUpdateTime >= reference.blackEndTime)
						{
							value = (float)(serverTimeNow - reference.lastUpdateTime) * 0.001f * reference.moveSpeed;
						}
						else
						{
							value = (float)(reference.blackEndTime - reference.lastUpdateTime) * 0.001f * reference.blackSpeed;
							long num2 = serverTimeNow - reference.blackEndTime;
							value += (float)num2 * 0.001f * reference.moveSpeed;
						}
					}
					else
					{
						long num3 = serverTimeNow - reference.lastUpdateTime;
						if (num3 > 0)
						{
							value = (float)num3 * 0.001f * reference.moveSpeed;
						}
					}
					value = Mathf.Clamp(value, 0f, reference.remainDistance);
					reference.remainDistance -= value;
					reference.currentWorldPos += reference.moveDirection * value;
				}
				else
				{
					reference.currentWorldPos = reference.targetWorldPos;
					reference.remainDistance = 0f;
				}
				reference.lastUpdateTime = serverTimeNow;
				reference.troopIsVisible = viewRect.Contains(reference.currentWorldPos);
				if (reference.troopIsVisible && reference.playerRelationType == PlayerType.PlayerNone)
				{
					RefreshPlayerRelationType(ref reference);
				}
			}
			updateElapsed = stopwatch.ElapsedMilliseconds;
			stopwatch.Stop();
		}

		public void TryRemoveMarch(long marchId)
		{
			if (EnableWorldMarchDataOptHahaha && marchId2DataIndex.TryGetValue(marchId, out var value))
			{
				marchDataCount--;
				ref LittleNormalMarchData reference = ref normalMarchArray[value];
				reference.marchUuid = 0L;
				reference.lastUpdateTime = 0L;
				unusedIdx.Enqueue(value);
				marchId2AllianceID.Remove(marchId);
				marchId2DataIndex.Remove(marchId);
			}
		}

		public bool TryGetMarchData(long marchId, ref LittleNormalMarchData data)
		{
			if (!EnableWorldMarchDataOptHahaha)
			{
				return false;
			}
			if (marchId2DataIndex.TryGetValue(marchId, out var value))
			{
				data = normalMarchArray[value];
				return true;
			}
			return false;
		}

		public string Description()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"March Data:{marchDataCount}");
			stringBuilder.AppendLine($"Buffer Size:{normalMarchArray.Length}");
			stringBuilder.AppendLine($"Unused:{unusedIdx.Count}");
			stringBuilder.AppendLine($"Max Idx:{maxDataIndex}");
			for (int i = 0; i <= maxDataIndex; i++)
			{
				ref LittleNormalMarchData reference = ref normalMarchArray[i];
				if (reference.marchUuid > 0)
				{
					stringBuilder.AppendLine(string.Format("[{0}][{1}][{2}]{3},{4}[line:{5}][troop:{6}]", i, (!reference.multiTroopLine) ? 1 : 2, reference.playerRelationType, reference.startWorldPos, reference.targetWorldPos, reference.troopLineIsVisible ? "√" : "×", reference.troopIsVisible ? "√" : "×"));
				}
			}
			return stringBuilder.ToString();
		}

		public string LitDescription()
		{
			return $"{marchDataCount}/{normalMarchArray.Length}({updateElapsed})";
		}

		public void OnDrawGizmos()
		{
			if (!EnableWorldMarchDataOptHahaha)
			{
				return;
			}
			for (int i = 0; i <= maxDataIndex; i++)
			{
				ref LittleNormalMarchData reference = ref normalMarchArray[i];
				if (reference.marchUuid > 0)
				{
					if (reference.troopLineIsVisible)
					{
						Gizmos.color = Color.yellow;
					}
					else
					{
						Gizmos.color = Color.gray;
					}
					ref Vector2 startWorldPos = ref reference.startWorldPos;
					ref Vector2 targetWorldPos = ref reference.targetWorldPos;
					ref Vector2 mainWorldPos = ref reference.mainWorldPos;
					Vector3 vector = new Vector3(startWorldPos.x, 0f, startWorldPos.y);
					Vector3 vector2 = new Vector3(targetWorldPos.x, 0f, targetWorldPos.y);
					Vector3 vector3 = new Vector3(mainWorldPos.x, 0f, mainWorldPos.y);
					Gizmos.DrawLine(vector, vector2);
					if (reference.multiTroopLine)
					{
						Gizmos.DrawLine(vector3, vector);
					}
					Gizmos.color = Color.green;
					Gizmos.DrawWireSphere(vector, 1f);
					Gizmos.color = Color.red;
					Gizmos.DrawWireSphere(vector2, 1f);
					Gizmos.color = Color.magenta;
					Gizmos.DrawWireSphere(vector3, 1f);
					if (reference.troopIsVisible)
					{
						Gizmos.color = Color.white;
					}
					else
					{
						Gizmos.color = Color.black;
					}
					ref Vector2 currentWorldPos = ref reference.currentWorldPos;
					Gizmos.DrawWireSphere(new Vector3(currentWorldPos.x, 0f, currentWorldPos.y), 1f);
				}
			}
		}
	}

	public const int normalAttackId = 100000;

	private const string sheldPath = "Assets/_Art/Effect/prefab/hero/Shaonian/VFX_shaonian_hudun.prefab";

	private List<long> removeList = new List<long>();

	private const int __REMAIN_COUNT__ = 1000;

	private HashSet<long> allMarchUuids = new HashSet<long>();

	private Dictionary<long, WorldMarch> allMarches = new Dictionary<long, WorldMarch>();

	private HashSet<long> toMeMarchUuids = new HashSet<long>();

	private Dictionary<long, int> toMeMarchTargetUuids = new Dictionary<long, int>();

	private HashSet<long> myMarchUuids = new HashSet<long>();

	private Dictionary<long, WorldMarch> ownerMarches = new Dictionary<long, WorldMarch>();

	private Dictionary<long, long> team2MarchUuid = new Dictionary<long, long>();

	private Dictionary<long, long> member2LeaderUuid = new Dictionary<long, long>();

	private Dictionary<long, int> myAssitanceUuid2Points = new Dictionary<long, int>();

	private Dictionary<int, List<long>> myAssistancePoint2Uuids = new Dictionary<int, List<long>>();

	private Dictionary<string, List<WorldMarch>> allianceMarches = new Dictionary<string, List<WorldMarch>>();

	private Dictionary<long, List<WorldMarch>> fakeRetreatMarches = new Dictionary<long, List<WorldMarch>>();

	private Dictionary<long, WorldMarch> fakeSampleMarches = new Dictionary<long, WorldMarch>();

	private Dictionary<long, WorldMarch> fakeAttackMonsterMarches = new Dictionary<long, WorldMarch>();

	private Dictionary<int, WorldTrainConfig> trainConfigs = new Dictionary<int, WorldTrainConfig>();

	private Dictionary<int, int> flowerCarLength = new Dictionary<int, int>();

	private HashSet<long> _tmpMarchSet = new HashSet<long>();

	private List<WorldMarch> _tmpMarchList = new List<WorldMarch>();

	public static Action<long, int, int, int> OnMonsterAdd;

	public static Action<long, int, int, int> OnMonsterDelete;

	private Bounds viewBounds;

	private Rect viewRect;

	private Vector4 lastCameraViewRange;

	private Rect cameraViewRect;

	private static long _fakeUuid = 0L;

	private Dictionary<long, long> _delayDestroyTroop = new Dictionary<long, long>();

	private bool myMarchDirty;

	private bool toMeMarchDirty;

	private Dictionary<string, long> _cacheClientCreateGuidAndTimeDict = new Dictionary<string, long>();

	private int multiKillPVPMin = -1;

	private int multiKillPVEMin = -1;

	private bool? multiKillSwitch;

	private static bool enableWorldMarchDataOptHahaha = false;

	private List<long> _toRemoveList = new List<long>();

	private Dictionary<long, WorldMarch> targetForMineMarchDic = new Dictionary<long, WorldMarch>(1024);

	private Dictionary<long, WorldMarch> targetForMineMarchDesertDic = new Dictionary<long, WorldMarch>(1024);

	private bool toMeMarchNeedUpdate = true;

	private bool toMeMarchDesertNeedUpdate = true;

	private WorldMarchBattleSound _marchBattleSound = new WorldMarchBattleSound();

	private StepProcessQueue<(WorldMarchMessageType, ISFSObject)> _messages;

	private Dictionary<long, (ISFSObject lastObjState, WorldMarchMessageType lastMsgType)> _messagesMergeDict;

	private Dictionary<long, List<int>> _prevWorldMarchLogMap;

	private Dictionary<long, List<int>> _worldMarchLogMap;

	private Dictionary<long, System.DateTime> _addMarchLogTime = new Dictionary<long, System.DateTime>();

	private System.DateTime _lastGetBlockMsgTime;

	private System.DateTime _lastHandleBlockMsgTime;

	private List<long> _logMarchUuids1 = new List<long>();

	private List<long> _logMarchUuids2 = new List<long>();

	private System.DateTime _clearMarchesLogTime;

	private int _clearMarchesFrom;

	private HashSet<long> _clearMarchesLogSet;

	private System.DateTime _prevClearMarchesLogTime;

	private int _prevClearMarchesFrom;

	private HashSet<long> _prevClearMarchesLogSet;

	private Stopwatch _marchStepUpdateTimer = new Stopwatch();

	private int _marchStepUpdateMoveId = -1;

	private readonly MarchObjectCounter _marchObjectCounter = new MarchObjectCounter();

	private bool littleSmartDirty;

	private EMarchStepUpdateMoveState _marchStepUpdateState;

	private HashSet<long> _invalidIncrementPushWmNewMap;

	private bool _delayToRefresh;

	private ITimer _delayRefreshTimer;

	public int hahaUpdateCount;

	public int legacyUpdateCount;

	private static Dictionary<long, WorldMarch> EmptyDic = new Dictionary<long, WorldMarch>();

	private readonly HashSet<int> allianceMembersHomePos = new HashSet<int>();

	private Dictionary<long, int> myMarchMultiKillPVE = new Dictionary<long, int>();

	private Dictionary<long, int> myMarchMultiKillPVP = new Dictionary<long, int>();

	private MarchDataUpdater marchDataUpdater;

	private Dictionary<int, int> tileBlockIndex = new Dictionary<int, int>();

	private bool isRecordingMarchBlock;

	private int recordingWorldId = int.MinValue;

	public static long SelectMarchUuid = 0L;

	public static long DebugMarchUuid = 0L;

	public static int __INC_UPDATE__ { get; private set; } = 0;

	public int AllMarchesCount => allMarchUuids.Count;

	private int MultiKillPVPMin
	{
		get
		{
			if (multiKillPVPMin == -1)
			{
				multiKillPVPMin = GameEntry.Lua.CallWithReturn<int, string, string, int>("CSharpCallLuaInterface.GetConfigNum", "killstreak_report_UI", "k1", 5);
			}
			return multiKillPVPMin;
		}
	}

	private int MultiKillPVEMin
	{
		get
		{
			if (multiKillPVEMin == -1)
			{
				multiKillPVEMin = GameEntry.Lua.CallWithReturn<int, string, string, int>("CSharpCallLuaInterface.GetConfigNum", "killstreak_report_UI", "k3", 5);
			}
			return multiKillPVEMin;
		}
	}

	private bool? MultiKillSwitch => multiKillSwitch ?? (multiKillSwitch = GameEntry.Data?.Player?.CheckSwitch("killstreak_report_UI_switch", defaultVal: false) ?? false);

	public bool EnableWorldAssistanceOpt => world?.EnableWorldAssistanceOpt ?? false;

	public static bool EnableWorldMarchDataOptHahaha
	{
		get
		{
			if (GMSwitch.IsGM)
			{
				return GMSwitch.GetInt("DebugWorldMarchDataHaha") switch
				{
					1 => true, 
					-1 => false, 
					_ => enableWorldMarchDataOptHahaha, 
				};
			}
			return enableWorldMarchDataOptHahaha;
		}
	}

	public int lastPerformanceCount => _marchObjectCounter.GetLastDisplayCount(CounterType.Squad);

	public int lastPerformanceSquadDisplayCount => _marchObjectCounter.GetLastDisplayCount(CounterType.Squad);

	public int lastPerformanceSquadDataCount => _marchObjectCounter.GetLastDataCount(CounterType.Squad);

	public int lastPerformanceTroopLineCount => _marchObjectCounter.GetLastDataCount(CounterType.TroopLine);

	public int lastPerformanceTroopLineDisplayCount => _marchObjectCounter.GetLastDisplayCount(CounterType.TroopLine);

	public int lastPerformanceTroopLineDataCount => _marchObjectCounter.GetLastDataCount(CounterType.TroopLine);

	public MarchDataUpdater DataUpdater => marchDataUpdater;

	public static WorldMarchDataManager EditorInstance => null;

	public Dictionary<int, int> TileBlockIndex => tileBlockIndex;

	private static bool IsCombine(CombatUnitType type)
	{
		switch (type)
		{
		case CombatUnitType.BUILDING:
		case CombatUnitType.TOWER:
		case CombatUnitType.RALLY_TEAM:
		case CombatUnitType.CITY:
		case CombatUnitType.ALLIANCE_OCCUPIED_CITY:
			return true;
		default:
			return false;
		}
	}

	private bool IsArmyInView(long uuid, CombatUnitType combineType)
	{
		switch (combineType)
		{
		case CombatUnitType.BUILDING:
		case CombatUnitType.TOWER:
		case CombatUnitType.CITY:
		case CombatUnitType.EXPLORE_POINT:
		case CombatUnitType.ALLIANCE_NEUTRAL_CITY:
		case CombatUnitType.ALLIANCE_OCCUPIED_CITY:
		{
			PointInfo pointInfoByUuid = world.GetPointInfoByUuid(uuid);
			if (pointInfoByUuid != null && IsInView(world.TileIndexToWorld(pointInfoByUuid.mainIndex)))
			{
				return true;
			}
			break;
		}
		case CombatUnitType.ARMY:
		case CombatUnitType.MONSTER:
		case CombatUnitType.RALLY_TEAM:
		case CombatUnitType.BOSS:
		case CombatUnitType.ACT_BOSS:
		case CombatUnitType.PUZZLE_BOSS:
		case CombatUnitType.CHALLENGE_BOSS:
		{
			WorldMarch march = GetMarch(uuid);
			if (march != null && IsInView(march.position))
			{
				return true;
			}
			break;
		}
		}
		return false;
	}

	public void UpdateBattleMessage(ISFSObject message)
	{
		byte[] data = Base64.Decode(message.GetUtfString("content"));
		BattleRoundPushInfo battleRoundPushInfo = BattleRoundPushInfo.Parser.ParseFrom(data);
		CombatUnitType type = (CombatUnitType)battleRoundPushInfo.Type;
		if (IsCombine(type))
		{
			if (battleRoundPushInfo.CombineArmyInfo != null)
			{
				UpdateCombineArmyInfo(battleRoundPushInfo.CombineArmyInfo, battleRoundPushInfo.OutRange, battleRoundPushInfo.RoundReports, type);
			}
			if (battleRoundPushInfo.SimpleArmyInfo != null)
			{
				UpdateSimpleArmyInfo(battleRoundPushInfo.SimpleArmyInfo, battleRoundPushInfo.OutRange, battleRoundPushInfo.RoundReports, type);
			}
		}
		else
		{
			UpdateSimpleArmyInfo(battleRoundPushInfo.SimpleArmyInfo, battleRoundPushInfo.OutRange, battleRoundPushInfo.RoundReports, type);
		}
	}

	private void UpdateCombineArmyInfo(CombineSelfArmyInfo combineArmyInfo, bool outRange, RepeatedField<BaseRoundReportPush> roundReports, CombatUnitType combineType)
	{
		RepeatedField<SimpleSelfArmyInfo> members = combineArmyInfo.Members;
		SimpleCombatUnitPushObj targetInfo = combineArmyInfo.TargetInfo;
		long num = 0L;
		if (outRange)
		{
			return;
		}
		Dictionary<long, bool> dictionary = new Dictionary<long, bool>();
		foreach (SimpleSelfArmyInfo item in members)
		{
			if (num == 0L && item.ArmyInfo != null)
			{
				num = item.ArmyInfo.TopUuid;
			}
			if (item.ArmyInfo != null && item.ArmyInfo.ArmyInfo != null)
			{
				dictionary[item.ArmyInfo.ArmyInfo.Uuid] = true;
			}
		}
		if (combineType == CombatUnitType.RALLY_TEAM)
		{
			num = 0L;
			foreach (KeyValuePair<long, bool> item2 in dictionary)
			{
				WorldMarch march = GetMarch(item2.Key);
				if (march != null)
				{
					WorldMarch allianceMarchesInTeam = GetAllianceMarchesInTeam(march.allianceUid, march.teamUuid);
					if (allianceMarchesInTeam != null)
					{
						num = allianceMarchesInTeam.uuid;
					}
				}
			}
		}
		if (num == 0L || !IsArmyInView(num, combineType))
		{
			return;
		}
		int heal = 0;
		int normalHurt = 0;
		int skillHurt = 0;
		int showAttackSkillId = 0;
		int showHurtSkillId = 0;
		bool isActiveAttack = false;
		List<int> effectBuffList = new List<int>();
		string text = "";
		for (int i = 0; i < roundReports.Count; i++)
		{
			BaseRoundReportPush reportInfo = roundReports[i];
			foreach (KeyValuePair<long, bool> item3 in dictionary)
			{
				CheckArmyDoSkill(item3.Key, reportInfo, ref showAttackSkillId, ref showHurtSkillId, ref skillHurt, ref normalHurt, ref isActiveAttack, ref effectBuffList);
			}
		}
		for (int j = 0; j < effectBuffList.Count; j++)
		{
			text += effectBuffList[j];
			if (j < effectBuffList.Count - 1)
			{
				text += ";";
			}
		}
		switch (combineType)
		{
		case CombatUnitType.ALLIANCE_OCCUPIED_CITY:
			foreach (SimpleSelfArmyInfo item4 in members)
			{
				if (item4.ArmyInfo != null && item4.ArmyInfo.ArmyInfo != null)
				{
					SimpleCombatUnit armyInfo2 = item4.ArmyInfo.ArmyInfo;
					if (armyInfo2.Uuid == num)
					{
						AllianceCityUpdateHeadUI(num, armyInfo2.Health, armyInfo2.InitHealth);
						break;
					}
				}
			}
			ShowAllianceCityBloodHurt(num, normalHurt, skillHurt, heal);
			ShowAllianceCityBuff(num, text);
			break;
		case CombatUnitType.RALLY_TEAM:
		{
			long defAtkUuid = targetInfo?.TopUuid ?? 0;
			SetTroopAttack(num, defAtkUuid, isActiveAttack);
			ShowTroopBloodHurt(num, normalHurt, skillHurt, heal);
			int anger = 0;
			int num4 = 0;
			int num5 = 0;
			foreach (SimpleSelfArmyInfo item5 in members)
			{
				if (item5.ArmyInfo != null && item5.ArmyInfo.ArmyInfo != null)
				{
					SimpleCombatUnit armyInfo3 = item5.ArmyInfo.ArmyInfo;
					num4 += armyInfo3.Health;
					num5 += armyInfo3.InitHealth;
				}
			}
			TroopUpdateHeadUI(num, anger, num4, num5);
			ShowTroopBuff(num, text);
			break;
		}
		case CombatUnitType.BUILDING:
		case CombatUnitType.TOWER:
		case CombatUnitType.CITY:
		{
			int num2 = 0;
			int num3 = 0;
			foreach (SimpleSelfArmyInfo item6 in members)
			{
				if (item6.ArmyInfo != null && item6.ArmyInfo.ArmyInfo != null)
				{
					SimpleCombatUnit armyInfo = item6.ArmyInfo.ArmyInfo;
					num2 += armyInfo.Health;
					num3 += armyInfo.InitHealth;
				}
			}
			string userData = num + ";" + num2 + ";" + num3;
			GameEntry.Event.Fire(EventId.ShowBuildAttackHeadUI, userData);
			ShowPlayerBuildBloodHurt(num, normalHurt, skillHurt, heal);
			ShowPlayerBuildBuff(num, text);
			break;
		}
		}
	}

	private void UpdateSimpleArmyInfo(SimpleSelfArmyInfo selfArmyInfo, bool outRange, RepeatedField<BaseRoundReportPush> roundReports, CombatUnitType combineType)
	{
		SimpleCombatUnitPushObj armyInfo = selfArmyInfo.ArmyInfo;
		SimpleCombatUnitPushObj targetInfo = selfArmyInfo.TargetInfo;
		int heal = selfArmyInfo.Heal;
		int shield = selfArmyInfo.Shield;
		if (armyInfo.TopUuid == 0L || outRange || !IsArmyInView(armyInfo.TopUuid, combineType))
		{
			return;
		}
		int normalHurt = 0;
		int skillHurt = 0;
		int showAttackSkillId = 0;
		int showHurtSkillId = 0;
		bool isActiveAttack = false;
		List<int> effectBuffList = new List<int>();
		string text = "";
		for (int i = 0; i < roundReports.Count; i++)
		{
			BaseRoundReportPush reportInfo = roundReports[i];
			CheckArmyDoSkill(armyInfo.TopUuid, reportInfo, ref showAttackSkillId, ref showHurtSkillId, ref skillHurt, ref normalHurt, ref isActiveAttack, ref effectBuffList);
		}
		for (int j = 0; j < effectBuffList.Count; j++)
		{
			text += effectBuffList[j];
			if (j < effectBuffList.Count - 1)
			{
				text += ";";
			}
		}
		switch (combineType)
		{
		case CombatUnitType.ALLIANCE_NEUTRAL_CITY:
			AllianceCityUpdateHeadUI(armyInfo.TopUuid, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.InitHealth);
			ShowAllianceCityBloodHurt(armyInfo.TopUuid, normalHurt, skillHurt, heal);
			ShowAllianceCityBuff(armyInfo.TopUuid, text);
			break;
		case CombatUnitType.ARMY:
		case CombatUnitType.MONSTER:
		case CombatUnitType.BOSS:
		case CombatUnitType.ACT_BOSS:
		case CombatUnitType.PUZZLE_BOSS:
		case CombatUnitType.CHALLENGE_BOSS:
		{
			long defAtkUuid = targetInfo?.TopUuid ?? 0;
			if (targetInfo != null && targetInfo.Type == 5)
			{
				defAtkUuid = 0L;
			}
			SetTroopAttack(armyInfo.TopUuid, defAtkUuid, isActiveAttack);
			ShowShieldEffect(armyInfo.TopUuid, shield);
			ShowTroopSkill(armyInfo.TopUuid, showAttackSkillId, showHurtSkillId);
			ShowTroopBloodHurt(armyInfo.TopUuid, normalHurt, skillHurt, heal);
			if (combineType == CombatUnitType.ACT_BOSS || combineType == CombatUnitType.PUZZLE_BOSS)
			{
				ActBossUpdateHeadUI(armyInfo.TopUuid, selfArmyInfo.Anger, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.InitHealth);
			}
			else
			{
				TroopUpdateHeadUI(armyInfo.TopUuid, selfArmyInfo.Anger, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.InitHealth);
			}
			ShowTroopBuff(armyInfo.TopUuid, text);
			break;
		}
		case CombatUnitType.TOWER:
			if (targetInfo != null && targetInfo.ArmyInfo != null)
			{
				BuildingAttack(armyInfo.ArmyInfo.Uuid, targetInfo.ArmyInfo.Uuid);
				ShowPlayerBuildBuff(armyInfo.ArmyInfo.Uuid, text);
			}
			break;
		case CombatUnitType.EXPLORE_POINT:
		{
			long defenderUid = targetInfo?.TopUuid ?? 0;
			ExploreAttack(armyInfo.TopUuid, defenderUid, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.InitHealth);
			ExploreUpdateHeadUI(armyInfo.TopUuid, selfArmyInfo.Anger, armyInfo.ArmyInfo.Health, armyInfo.ArmyInfo.InitHealth);
			ShowExploreSkill(armyInfo.TopUuid, showAttackSkillId, showHurtSkillId);
			ShowExploreBloodHurt(armyInfo.TopUuid, normalHurt, skillHurt, heal);
			ShowExploreBuff(armyInfo.TopUuid, text);
			break;
		}
		}
	}

	private void ShowShieldEffect(long atkUuid, int shield)
	{
		WorldTroop troop = world.GetTroop(atkUuid);
		if (troop != null)
		{
			if (shield > 0)
			{
				troop.AddShield();
			}
			else
			{
				troop.DelShield();
			}
		}
	}

	private void SetTroopAttack(long atkUuid, long defAtkUuid, bool isActiveAttack)
	{
		WorldTroop troop = world.GetTroop(atkUuid);
		if (troop == null)
		{
			return;
		}
		troop.SetRotationRoot();
		troop.SetIsBattle(value: true);
		WorldMarch marchInfo = troop.GetMarchInfo();
		if (marchInfo != null && (marchInfo.type == NewMarchType.ACT_BOSS || marchInfo.type == NewMarchType.PUZZLE_BOSS))
		{
			return;
		}
		if (marchInfo != null && (marchInfo.IsMonsterOrOrdinaryBoss() || marchInfo.IsWanderBoss() || marchInfo.type == NewMarchType.DARK_KNIGHT_CITY || marchInfo.type == NewMarchType.CHALLENGE_BOSS))
		{
			troop.defAtkUuid = 0L;
			troop.SetRotation(Quaternion.LookRotation(troop.GetDefenderPosition() - troop.GetPosition()));
			troop.ReSetEntityTarget();
			troop.Attack();
		}
		else
		{
			if (!isActiveAttack)
			{
				return;
			}
			troop.defAtkUuid = defAtkUuid;
			WorldMarch march = world.GetMarch(defAtkUuid);
			if (march != null && march.ownerUid == GameEntry.Data.Player.Uid)
			{
				GameEntry.Event.Fire(EventId.ShowBattleRedName, atkUuid);
			}
			troop.SetRotation(Quaternion.LookRotation(troop.GetDefenderPosition() - troop.GetPosition()));
			troop.ReSetEntityTarget();
			troop.Attack();
			if (marchInfo != null)
			{
				switch (marchInfo.type)
				{
				case NewMarchType.DEFAULT:
				case NewMarchType.NORMAL:
				case NewMarchType.EXPLORE:
				case NewMarchType.DIRECT_MOVE_MARCH:
				case NewMarchType.ALL_OUT:
				case NewMarchType.FAKE_ATTACK:
					troop.ShowAttack();
					break;
				case NewMarchType.ASSEMBLY_MARCH:
					troop.ShowRallyMarchAttack();
					break;
				}
			}
		}
	}

	private void ShowTroopSkill(long skillTargetUuid, int selfSkillId, int hurtSkillId)
	{
		WorldTroop troop = world.GetTroop(skillTargetUuid);
		if (troop != null)
		{
			if (hurtSkillId > 0)
			{
				troop.DoSkill(hurtSkillId, DamageType.ATTACK, 0, null, 0L);
			}
			if (selfSkillId > 0)
			{
				GameEntry.Event.Fire(EventId.ShowHeroIconByUseSkill, skillTargetUuid);
			}
			else if (hurtSkillId > 0)
			{
				GameEntry.Event.Fire(EventId.ShowHeroHitedUiEffect, skillTargetUuid);
			}
		}
	}

	private void ShowTroopBloodHurt(long hurtTargetUuid, int normalHurt, int skillHurt, int heal)
	{
		WorldTroop troop = world.GetTroop(hurtTargetUuid);
		if (troop == null)
		{
			WorldMarch march = world.GetMarch(hurtTargetUuid);
			if (march != null && march.status == MarchStatus.COLLECTING)
			{
				int targetPos = march.targetPos;
				ShowCollectPointBloodHurt(targetPos, normalHurt, skillHurt, heal);
			}
			return;
		}
		if (normalHurt > 0)
		{
			troop.ShowBattleHurt(normalHurt, BattleWordType.Normal);
		}
		if (skillHurt > 0)
		{
			troop.ShowBattleHurt(skillHurt, BattleWordType.Skill);
		}
		if (heal > 0)
		{
			troop.ShowBattleHurt(heal * -1, BattleWordType.Cure);
		}
	}

	private void TroopUpdateHeadUI(long marchUuid, int anger, int hp, int hpMax)
	{
		if (world.GetTroop(marchUuid) == null)
		{
			WorldMarch march = world.GetMarch(marchUuid);
			if (march != null && march.status == MarchStatus.COLLECTING)
			{
				int targetPos = march.targetPos;
				ShowCollectUpdateHeadUI(marchUuid, targetPos, anger, hp, hpMax);
			}
		}
		else
		{
			string userData = marchUuid + ";" + anger + ";" + hp + ";" + hpMax;
			GameEntry.Event.Fire(EventId.ShowTroopBattleValue, userData);
		}
	}

	private void ActBossUpdateHeadUI(long marchUuid, int anger, int hp, int hpMax)
	{
		string userData = marchUuid + ";" + anger + ";" + hp + ";" + hpMax;
		GameEntry.Event.Fire(EventId.ShowActBossBattleValue, userData);
	}

	private void ShowTroopBuff(long targetUuid, string effectStr)
	{
		if (effectStr.IsNullOrEmpty())
		{
			return;
		}
		WorldTroop troop = world.GetTroop(targetUuid);
		if (troop == null)
		{
			WorldMarch march = world.GetMarch(targetUuid);
			if (march != null && march.status == MarchStatus.COLLECTING)
			{
				int targetPos = march.targetPos;
				ShowCollectPointBuff(targetUuid, targetPos, effectStr);
			}
		}
		else
		{
			string userData = targetUuid + "|" + world.WorldToTileIndex(troop.GetPosition()) + "|" + effectStr;
			GameEntry.Event.Fire(EventId.ShowBattleBuff, userData);
		}
	}

	private void ShowExploreSkill(long skillTargetUuid, int selfSkillId, int hurtSkillId)
	{
		if (world.GetObjectByUuid(skillTargetUuid) is WorldExploreObject worldExploreObject)
		{
			if (selfSkillId > 0)
			{
				worldExploreObject.DoSkill(selfSkillId, DamageType.USE_SKILL, 0, null, 0L);
			}
			if (hurtSkillId > 0)
			{
				worldExploreObject.DoSkill(hurtSkillId, DamageType.ATTACK, 0, null, 0L);
			}
		}
	}

	private void ShowExploreBloodHurt(long hurtTargetUuid, int normalHurt, int skillHurt, int heal)
	{
		if (world.GetObjectByUuid(hurtTargetUuid) is WorldExploreObject worldExploreObject)
		{
			if (normalHurt > 0)
			{
				worldExploreObject.ShowBattleHurt(normalHurt, BattleWordType.Normal);
			}
			if (skillHurt > 0)
			{
				worldExploreObject.ShowBattleHurt(skillHurt, BattleWordType.Skill);
			}
			if (heal > 0)
			{
				worldExploreObject.ShowBattleHurt(heal * -1, BattleWordType.Cure);
			}
		}
	}

	private void ExploreAttack(long atkUuid, long defenderUid, int soliderNum, int hp, int hpMax)
	{
		if (world.GetObjectByUuid(atkUuid) is WorldExploreObject worldExploreObject)
		{
			worldExploreObject.DoWhenAttackExploreStart(defenderUid, soliderNum, hp, hpMax);
		}
	}

	private void ExploreUpdateHeadUI(long marchUuid, int anger, int hp, int hpMax)
	{
		if (world.GetObjectByUuid(marchUuid) is WorldExploreObject worldExploreObject)
		{
			worldExploreObject.UpdateBattleHeadUI(anger, hp, hpMax);
		}
	}

	private void ShowExploreBuff(long targetUuid, string effectStr)
	{
		if (!effectStr.IsNullOrEmpty() && world.GetObjectByUuid(targetUuid) is WorldExploreObject worldExploreObject)
		{
			Transform transform = worldExploreObject.GetTransform();
			if (!(transform == null))
			{
				string userData = targetUuid + "|" + world.WorldToTileIndex(transform.position) + "|" + effectStr + "|" + 8;
				GameEntry.Event.Fire(EventId.ShowBattleBuff, userData);
			}
		}
	}

	private void AllianceCityUpdateHeadUI(long cityUuid, int hp, int hpMax)
	{
		string userData = cityUuid + ";" + hp + ";" + hpMax;
		GameEntry.Event.Fire(EventId.ShowAllianceCitySoldierBlood, userData);
	}

	private void ShowAllianceCityBloodHurt(long hurtTargetUuid, int normalHurt, int skillHurt, int heal)
	{
		PointInfo pointInfoByUuid = world.GetPointInfoByUuid(hurtTargetUuid);
		if (pointInfoByUuid != null)
		{
			Vector3 startPos = world.TileIndexToWorld(pointInfoByUuid.mainIndex) + new Vector3(-6f, 0f, -14f);
			if (normalHurt > 0)
			{
				string path = "Assets/Main/Prefabs/UI/BattleWord/BattleBuildNormalBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = normalHurt,
					path = path
				}, path);
			}
			if (skillHurt > 0)
			{
				string path2 = "Assets/Main/Prefabs/UI/BattleWord/BattleBuildDecBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = skillHurt,
					path = path2
				}, path2);
			}
		}
	}

	private void ShowAllianceCityBuff(long targetUuid, string effectStr)
	{
		PointInfo pointInfoByUuid = world.GetPointInfoByUuid(targetUuid);
		if (pointInfoByUuid != null)
		{
			Vector3 pos = world.TileIndexToWorld(pointInfoByUuid.mainIndex) + new Vector3(-6f, 0f, -14f);
			if (!effectStr.IsNullOrEmpty())
			{
				string userData = targetUuid + "|" + world.WorldToTileIndex(pos) + "|" + effectStr + "|" + 11;
				GameEntry.Event.Fire(EventId.ShowBattleBuff, userData);
			}
		}
	}

	private void BuildingAttack(long atkUuid, long defUuid)
	{
		CityBuilding buildingByUuid = world.GetBuildingByUuid(atkUuid);
		if (!(buildingByUuid == null))
		{
			buildingByUuid.OnBattleAtkUpdate(defUuid);
		}
	}

	private void ShowPlayerBuildBloodHurt(long hurtTargetUuid, int normalHurt, int skillHurt, int heal)
	{
		if (world.GetPointInfoByUuid(hurtTargetUuid) is BuildPointInfo { tileSize: var num } buildPointInfo)
		{
			if (num <= 1)
			{
				num = 0;
			}
			Vector3 startPos = world.TileIndexToWorld(buildPointInfo.mainIndex) + new Vector3(0f - (float)num / 2f, 0f, -2 * num);
			if (normalHurt > 0)
			{
				string path = "Assets/Main/Prefabs/UI/BattleWord/BattleBuildNormalBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = normalHurt,
					path = path
				}, path);
			}
			if (skillHurt > 0)
			{
				string path2 = "Assets/Main/Prefabs/UI/BattleWord/BattleBuildDecBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = skillHurt,
					path = path2
				}, path2);
			}
		}
	}

	private void ShowPlayerBuildBuff(long targetUuid, string effectStr)
	{
		if (!effectStr.IsNullOrEmpty() && world.GetPointInfoByUuid(targetUuid) is BuildPointInfo { tileSize: var num } buildPointInfo)
		{
			if (num <= 1)
			{
				num = 0;
			}
			Vector3 pos = world.TileIndexToWorld(buildPointInfo.mainIndex) + new Vector3(0f - (float)num / 2f, 0f, -2 * num);
			string userData = targetUuid + "|" + world.WorldToTileIndex(pos) + "|" + effectStr + "|" + 6;
			GameEntry.Event.Fire(EventId.ShowBattleBuff, userData);
		}
	}

	public void BattleFinish(ISFSObject message)
	{
		try
		{
			long num = message.GetLong("leaderUuid");
			BatleResult batleResult = (BatleResult)message.GetInt("result");
			WorldTroop troop = world.GetTroop(num);
			try
			{
				if (troop != null && troop.DelayApply)
				{
					float delayApplyTime = troop.DelayApplyTime;
					world.StartCoroutine(DelayApply(delayApplyTime, num, batleResult));
					if (troop.GetMarchInfo().IsEVP())
					{
						switch (batleResult)
						{
						case BatleResult.SELF_WIN:
							troop.ShowZombieRushDefendSuccess();
							break;
						case BatleResult.OTHER_WIN:
							troop.ShowZombieRushDefendFailed();
							break;
						}
					}
				}
				else
				{
					onBattleFinish(num, batleResult);
				}
			}
			catch (Exception arg)
			{
				Log.Error($"Exception when BattleFinish 2 {arg}");
			}
		}
		catch (Exception arg2)
		{
			Log.Error($"Exception when BattleFinish 1 {arg2}");
		}
	}

	private IEnumerator DelayApply(float delay, long uuid, BatleResult result)
	{
		yield return new WaitForSeconds(delay);
		try
		{
			onBattleFinish(uuid, result);
		}
		catch (Exception arg)
		{
			Log.Error($"Exception when BattleFinish 3 {arg}");
		}
	}

	private void onBattleFinish(long uuid, BatleResult result)
	{
		WorldTroop troop = world.GetTroop(uuid);
		world.HideTroopDestination(uuid);
		GameEntry.Event.Fire(EventId.CollectPointOut, uuid);
		GameEntry.Event.Fire(EventId.HideAllianceCitySoliderBlood, uuid);
		GameEntry.Event.Fire(EventId.HideBuildAttackHeadUI, uuid);
		GameEntry.Event.Fire(EventId.HideBattleBuff, uuid);
		world.RemovePosAndRotationDataByMarchUuid(uuid);
		if (troop == null)
		{
			return;
		}
		troop.SetIsBattle(value: false);
		troop.defAtkUuid = 0L;
		switch (result)
		{
		case BatleResult.SELF_WIN:
		{
			if (world.IsSelfInCurrentMarchTeam(uuid))
			{
				GameEntry.Sound.PlayEffectById(61035);
				troop.ShowBattleSuccess();
				GameEntry.Event.Fire(EventId.MarchEndWithReward, uuid);
			}
			WorldTroop targetTroop2 = troop.GetTargetTroop();
			if (targetTroop2 != null && targetTroop2.IsCityStrongholdMonsterTroop())
			{
				string text = targetTroop2.GetMarchUUID().ToString();
				string key = text + "_reborn";
				string data = GameEntry.Data.Player.GetData(key);
				if (text.Equals(data))
				{
					GameEntry.Data.Player.DeleteData(key);
					targetTroop2.TryPlayBornAnim();
				}
				break;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>(GameEntry.Data.Player.GetAllData());
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				if (item.Key.EndsWith("_reborn"))
				{
					list.Add(item.Key);
					world.GetTroop(long.Parse(item.Value))?.TryPlayBornAnim();
				}
			}
			foreach (string item2 in list)
			{
				GameEntry.Data.Player.DeleteData(item2);
			}
			break;
		}
		case BatleResult.OTHER_WIN:
			if (world.IsSelfInCurrentMarchTeam(uuid))
			{
				GameEntry.Sound.PlayEffectById(61035);
				GameEntry.Event.Fire(EventId.MarchFail, uuid);
				troop.ShowBattleFailed();
			}
			if (troop.DelayApply && troop.IsMarchTargetAttack() && !troop.IsAttackWorldBoss() && !troop.IsAttackAisilla())
			{
				WorldTroop targetTroop = troop.GetTargetTroop();
				if (targetTroop != null && targetTroop.IsMonsterTroop() && targetTroop.GetMarchTargetType() != MarchTargetType.RUNNING_BOSS_ATTACK_CITY && targetTroop.IsCanPlayAni())
				{
					targetTroop.TryPlay("idle", targetTroop.GetPosition() + Vector3.back);
				}
			}
			break;
		}
		troop.BackTroopUnits();
		troop.ClearEffect();
		MarchStatus marchStatus = troop.GetMarchStatus();
		MarchTargetType marchTargetType = troop.GetMarchTargetType();
		if (!troop.IsDelayDestroy)
		{
			if (marchStatus == MarchStatus.CHASING || marchStatus == MarchStatus.MOVING || marchTargetType == MarchTargetType.BACK_HOME)
			{
				troop.PlayAnim("run");
			}
			else
			{
				troop.PlayAnim("idle");
			}
		}
	}

	private void ShowCollectPointBloodHurt(int pointIndex, int normalHurt, int skillHurt, int heal)
	{
		if (world.GetPointInfo(pointIndex) is ResPointInfo { tileSize: var num } resPointInfo)
		{
			if (num <= 1)
			{
				num = 0;
			}
			Vector3 startPos = world.TileIndexToWorld(resPointInfo.mainIndex) + new Vector3(0f - (float)num / 2f, 0f, -2 * num);
			if (normalHurt > 0)
			{
				string path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = normalHurt,
					path = path
				}, path);
			}
			if (skillHurt > 0)
			{
				string path2 = "Assets/Main/Prefabs/UI/BattleWord/BattleDecBloodTip.prefab";
				world.ShowBattleBlood(new BattleDecBloodTip.Param
				{
					startPos = startPos,
					num = skillHurt,
					path = path2
				}, path2);
			}
		}
	}

	private void ShowCollectPointBuff(long targetUuid, int pointIndex, string effectStr)
	{
		if (!effectStr.IsNullOrEmpty() && world.GetPointInfo(pointIndex) is ResPointInfo { tileSize: var num } resPointInfo)
		{
			if (num <= 1)
			{
				num = 0;
			}
			Vector3 pos = world.TileIndexToWorld(resPointInfo.mainIndex) + new Vector3(0f - (float)num / 2f, 0f, -2 * num);
			string userData = targetUuid + "|" + world.WorldToTileIndex(pos) + "|" + effectStr + "|" + 7;
			GameEntry.Event.Fire(EventId.ShowBattleBuff, userData);
		}
	}

	private void ShowCollectUpdateHeadUI(long marchUuid, int pointIndex, int anger, int hp, int hpMax)
	{
		string userData = marchUuid + ";" + pointIndex + ";" + anger + ";" + hp + ";" + hpMax;
		GameEntry.Event.Fire(EventId.ShowCollectBattleValue, userData);
	}

	private void CheckArmyDoSkill(long armyUuid, BaseRoundReportPush reportInfo, ref int showAttackSkillId, ref int showHurtSkillId, ref int skillHurt, ref int normalHurt, ref bool isActiveAttack, ref List<int> effectBuffList)
	{
		long triggerUuid = reportInfo.TriggerUuid;
		long targetUuid = reportInfo.TargetUuid;
		BaseRoundReport roundReport = reportInfo.RoundReport;
		DamageType type = (DamageType)roundReport.Type;
		switch (type)
		{
		case DamageType.USE_SKILL:
			if (triggerUuid != armyUuid)
			{
				break;
			}
			if (showAttackSkillId <= 0)
			{
				int skillId2 = roundReport.SkillId;
				string templateData3 = GameEntry.ConfigCache.GetTemplateData("skill", skillId2, "effect_path");
				string templateData4 = GameEntry.ConfigCache.GetTemplateData("skill", skillId2, "effect_point_type");
				int num3 = GameEntry.ConfigCache.GetTemplateData("skill", skillId2, "type").ToInt();
				if (!templateData4.IsNullOrEmpty())
				{
					templateData4.ToInt();
				}
				if (num3 == 11 && !templateData3.IsNullOrEmpty() && "Assets/_Art/Effect/prefab/hero/Shaonian/VFX_shaonian_hudun.prefab" != templateData3)
				{
					showAttackSkillId = skillId2;
				}
			}
			if (roundReport.SkillId == 100000)
			{
				isActiveAttack = true;
			}
			break;
		case DamageType.ATTACK:
			if (targetUuid == armyUuid)
			{
				if (roundReport.SkillId == 100000)
				{
					normalHurt += roundReport.Value;
				}
				else
				{
					skillHurt += roundReport.Value;
					if (showHurtSkillId <= 0)
					{
						int skillId = roundReport.SkillId;
						string templateData = GameEntry.ConfigCache.GetTemplateData("skill", skillId, "effect_path");
						string templateData2 = GameEntry.ConfigCache.GetTemplateData("skill", skillId, "effect_point_type");
						int num = GameEntry.ConfigCache.GetTemplateData("skill", skillId, "type").ToInt();
						int num2 = 0;
						if (!templateData2.IsNullOrEmpty())
						{
							num2 = templateData2.ToInt();
						}
						if (num == 11 && !templateData.IsNullOrEmpty() && type == (DamageType)num2 && "Assets/_Art/Effect/prefab/hero/Shaonian/VFX_shaonian_hudun.prefab" != templateData)
						{
							showHurtSkillId = skillId;
						}
					}
				}
			}
			if (triggerUuid == armyUuid && roundReport.SkillId == 100000)
			{
				isActiveAttack = true;
			}
			break;
		case DamageType.COUNTER_ATTACK:
			if (targetUuid == armyUuid)
			{
				normalHurt += roundReport.Value;
			}
			break;
		case DamageType.ADD_EFFECT:
			if (targetUuid == armyUuid)
			{
				effectBuffList.Add(roundReport.Value);
			}
			break;
		case DamageType.SHIELD_ATTACK:
		case DamageType.SHIELD:
		case DamageType.RECOVER_DAMAGE:
			break;
		}
	}

	public void UpdateBattle(float deltaTime)
	{
	}

	private void AddMarchOptLog(WorldMarch march, int opt)
	{
		if (march == null)
		{
			return;
		}
		int marchType = march.GetMarchType();
		if (marchType == 0 || marchType == 4)
		{
			if (_worldMarchLogMap == null)
			{
				_worldMarchLogMap = new Dictionary<long, List<int>>();
			}
			if (!_worldMarchLogMap.ContainsKey(march.uuid))
			{
				_worldMarchLogMap.Add(march.uuid, new List<int>());
			}
			List<int> list = _worldMarchLogMap[march.uuid];
			switch (opt)
			{
			case 1:
				_addMarchLogTime[march.uuid] = System.DateTime.UtcNow;
				break;
			case 2:
			case 3:
				_addMarchLogTime.Remove(march.uuid);
				break;
			}
			list.Add(opt);
		}
	}

	private void DelayToRefreshMarch()
	{
		if (_invalidIncrementPushWmNewMap != null && _invalidIncrementPushWmNewMap.Count > 0 && SceneManager.World != null)
		{
			SceneManager.World.SetFirstViewRequestFlag(isFirstTime: true);
			SceneManager.World.UpdateViewRequest(isForce: true);
		}
	}

	private void ClearDelayToRefreshMarch()
	{
		if (_invalidIncrementPushWmNewMap != null)
		{
			_invalidIncrementPushWmNewMap.Clear();
		}
		_delayToRefresh = false;
		if (_delayRefreshTimer != null)
		{
			GameEntry.Timer.CancelTimer(_delayRefreshTimer);
			_delayRefreshTimer = null;
		}
	}

	public WorldMarchDataManager(WorldScene scene)
		: base(scene)
	{
		_messages = new StepProcessQueue<(WorldMarchMessageType, ISFSObject)>(HandleMessage, 2L, 1, 500);
		_messagesMergeDict = new Dictionary<long, (ISFSObject, WorldMarchMessageType)>();
	}

	public void SetWorldScene(WorldScene scene)
	{
		if (scene == null)
		{
			_messages.Clear();
			_messagesMergeDict.Clear();
			allMarchUuids.Clear();
			toMeMarchUuids.Clear();
			toMeMarchTargetUuids.Clear();
			myMarchUuids.Clear();
			if (world != null)
			{
				foreach (long key in allMarches.Keys)
				{
					world.DestroyTroop(key);
					world.DestroyTroopLine(key);
				}
			}
			if (allMarches.Count > 0)
			{
				foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
				{
					WorldMarch value = allMarch.Value;
					if (value != null)
					{
						BIRecordWorldMarch(value);
					}
				}
			}
			AddClearLog(2);
			allMarches.Clear();
			marchDataUpdater?.Clear();
			ownerMarches.Clear();
			myAssistancePoint2Uuids.Clear();
			myAssitanceUuid2Points.Clear();
			team2MarchUuid.Clear();
			member2LeaderUuid.Clear();
			allianceMarches.Clear();
			fakeSampleMarches.Clear();
			fakeAttackMonsterMarches.Clear();
			_delayDestroyTroop.Clear();
			_cacheClientCreateGuidAndTimeDict.Clear();
			_marchBattleSound.UnInit();
			ClearDelayToRefreshMarch();
		}
		else
		{
			_marchBattleSound.Init();
		}
		world = scene;
		ResetStepUpdateMove();
	}

	private void AddClearLog(int clearFrom)
	{
		try
		{
			if (_worldMarchLogMap != null && _worldMarchLogMap.Count > 0)
			{
				if (_clearMarchesLogSet == null)
				{
					_clearMarchesLogSet = new HashSet<long>();
				}
				else
				{
					_prevClearMarchesFrom = _clearMarchesFrom;
					_prevClearMarchesLogTime = _clearMarchesLogTime;
					_prevClearMarchesLogSet = _clearMarchesLogSet;
					_clearMarchesLogSet = new HashSet<long>();
				}
				_clearMarchesFrom = clearFrom;
				_clearMarchesLogTime = System.DateTime.UtcNow;
				foreach (KeyValuePair<long, List<int>> item in _worldMarchLogMap)
				{
					_clearMarchesLogSet.Add(item.Key);
				}
			}
			_prevWorldMarchLogMap = _worldMarchLogMap;
			_worldMarchLogMap = new Dictionary<long, List<int>>();
		}
		catch (Exception)
		{
			Log.Error("[IncreUpdate]日志拼接失败");
		}
		_addMarchLogTime.Clear();
	}

	public override void Init()
	{
		base.Init();
		GameEntry.Event.Subscribe(EventId.APP_APPLICATION_PAUSE, OnApplicationPause);
		_marchObjectCounter.ResetAll();
		enableWorldMarchDataOptHahaha = GameEntry.Data?.Player?.CheckSwitch("world_march_data_opt_hahaha", defaultVal: false) ?? false;
		marchDataUpdater = marchDataUpdater ?? new MarchDataUpdater(this);
		marchDataUpdater.Clear();
		ResetStepUpdateMove();
	}

	private void OnReloadOrReconnect(object obj)
	{
		if (world != null)
		{
			world.OnChangeServerRemove();
			world.SetFirstViewRequestFlag(isFirstTime: true);
			world.UpdateViewRequest(isForce: true);
		}
	}

	private void OnApplicationPause(object userData)
	{
		object obj;
		if ((obj = userData) is bool && !(bool)obj)
		{
			RefreshMAMTroopPosition();
		}
	}

	private void RefreshMAMTroopPosition()
	{
		foreach (WorldMarch value2 in ownerMarches.Values)
		{
			value2.InitMove(CreatePathSegment(value2));
			if (world != null)
			{
				world.TroopRefreshPosition(value2);
			}
		}
		foreach (long toMeMarchUuid in toMeMarchUuids)
		{
			if (allMarches.TryGetValue(toMeMarchUuid, out var value))
			{
				value.InitMove(CreatePathSegment(value));
				if (world != null)
				{
					world.TroopRefreshPosition(value);
				}
			}
		}
	}

	public override void UnInit()
	{
		base.UnInit();
		targetForMineMarchDic.Clear();
		targetForMineMarchDesertDic.Clear();
		_delayDestroyTroop.Clear();
		_cacheClientCreateGuidAndTimeDict.Clear();
		trainConfigs.Clear();
		flowerCarLength.Clear();
		marchDataUpdater?.Clear();
		CleanAllianceMembersHomePos();
		GameEntry.Event.Unsubscribe(EventId.APP_APPLICATION_PAUSE, OnApplicationPause);
		_marchBattleSound.UnInit();
		StopRecordMarchBlock();
	}

	private void __ReInit()
	{
		_messages.Clear();
		_messagesMergeDict.Clear();
		allMarchUuids.Clear();
		toMeMarchUuids.Clear();
		toMeMarchTargetUuids.Clear();
		myMarchUuids.Clear();
		if (world != null)
		{
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				world.DestroyTroop(allMarch.Key);
				world.DestroyTroopLine(allMarch.Key);
			}
		}
		if (allMarches.Count > 0)
		{
			foreach (KeyValuePair<long, WorldMarch> allMarch2 in allMarches)
			{
				WorldMarch value = allMarch2.Value;
				if (value != null)
				{
					BIRecordWorldMarch(value);
				}
			}
		}
		AddClearLog(1);
		allMarches.Clear();
		marchDataUpdater?.Clear();
		ownerMarches.Clear();
		myAssistancePoint2Uuids.Clear();
		myAssitanceUuid2Points.Clear();
		team2MarchUuid.Clear();
		member2LeaderUuid.Clear();
		allianceMarches.Clear();
		fakeSampleMarches.Clear();
		fakeAttackMonsterMarches.Clear();
		_delayDestroyTroop.Clear();
		_cacheClientCreateGuidAndTimeDict.Clear();
		targetForMineMarchDic.Clear();
		targetForMineMarchDesertDic.Clear();
		WorldGetMarchInfos();
		ResetStepUpdateMove();
	}

	public override void OnUpdate(float deltaTime)
	{
		int num = lastPerformanceSquadDisplayCount;
		bool viewDirty = UpdateViewRect();
		UpdateMessage();
		marchDataUpdater?.DeltaUpdate(deltaTime, GameEntry.Timer.GetServerTime(), viewDirty, cameraViewRect);
		UpdateMove(deltaTime);
		UpdateDelayDestroy();
		int num2 = lastPerformanceSquadDisplayCount;
		if (num != num2)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshPerformanceTroopCount", num2);
		}
	}

	private void UpdateDelayDestroy()
	{
		if (_delayDestroyTroop.Count <= 0)
		{
			return;
		}
		_toRemoveList.Clear();
		Dictionary<long, long>.Enumerator enumerator = _delayDestroyTroop.GetEnumerator();
		long serverTime = GameEntry.Timer.GetServerTime();
		while (enumerator.MoveNext())
		{
			long key = enumerator.Current.Key;
			long value = enumerator.Current.Value;
			if (serverTime >= value)
			{
				_toRemoveList.Add(key);
				if (world != null)
				{
					world.DestroyTroopLine(key);
					WorldMarch march = GetMarch(key);
					AddMarchOptLog(march, 13);
					RemoveMarch(key);
				}
				GameEntry.Event.Fire(EventId.HideTroopName, key);
			}
		}
		for (int i = 0; i < _toRemoveList.Count; i++)
		{
			_delayDestroyTroop.Remove(_toRemoveList[i]);
		}
	}

	private bool UpdateViewRect()
	{
		if (world == null)
		{
			return false;
		}
		Camera main = Camera.main;
		Vector3 curTarget = world.CurTarget;
		viewBounds.center = curTarget;
		viewBounds.extents = Vector3.one;
		viewBounds.Encapsulate(world.GetRaycastGroundPoint(new Vector3(0f, 0f, 0f)));
		viewBounds.Encapsulate(world.GetRaycastGroundPoint(new Vector3(0f, main.pixelHeight, 0f)));
		viewBounds.Encapsulate(world.GetRaycastGroundPoint(new Vector3(main.pixelWidth, main.pixelHeight, 0f)));
		viewBounds.Encapsulate(world.GetRaycastGroundPoint(new Vector3(main.pixelWidth, 0f, 0f)));
		cameraViewRect.center = new Vector2(viewBounds.center.x, viewBounds.center.z);
		cameraViewRect.size = new Vector2(viewBounds.size.x, viewBounds.size.z);
		viewBounds.Expand(world.TileSize * 5f);
		viewRect.center = new Vector2(viewBounds.center.x, viewBounds.center.z);
		viewRect.size = new Vector2(viewBounds.size.x, viewBounds.size.z);
		if (!Mathf.Approximately(cameraViewRect.xMin, lastCameraViewRange.x) || !Mathf.Approximately(cameraViewRect.xMax, lastCameraViewRange.y) || !Mathf.Approximately(cameraViewRect.yMin, lastCameraViewRange.z) || !Mathf.Approximately(cameraViewRect.yMax, lastCameraViewRange.w))
		{
			lastCameraViewRange.Set(cameraViewRect.xMin, cameraViewRect.xMax, cameraViewRect.yMin, cameraViewRect.yMax);
			return true;
		}
		return false;
	}

	private bool IsInView(Vector3 position)
	{
		return IsRectInView(position, 0);
	}

	private bool IsLineInView(Vector3 start, Vector3 end)
	{
		return IntersectsSegment(viewRect, new Vector2(start.x, start.z), new Vector2(end.x, end.z));
	}

	private bool IsRectInView(Vector3 point, int size)
	{
		if (world == null)
		{
			return false;
		}
		float distance = Mathf.Max(size, 3);
		return world.Camera.InCamera(point.x, point.z, distance);
	}

	private bool IsMarchInView(WorldMarch march)
	{
		if (march.type == NewMarchType.TRAIN)
		{
			return IsLineInView(march.position, march.GetTrainTailPos());
		}
		if (march.type == NewMarchType.ZONE_TRAIN)
		{
			return HSRMarchManager.GetInstance().IsInView(viewRect, march.uuid);
		}
		if (march.IsFlowerCar())
		{
			return IsLineInView(march.position, march.GetFlowerCarTailPos());
		}
		if (march.IsFlowerTrain())
		{
			return IsLineInView(march.position, march.GetFlowerTrainTailPos());
		}
		if (march.type == NewMarchType.DETECT_ZOMBIE_BUS_TRAIN)
		{
			return IsLineInView(march.position, march.GetZombieBusTrainTailPos());
		}
		if (march.IsBloodyQueenQueenGunner())
		{
			return IsLineInView(march.position, march.bloodyQueenMonster.standWorldPos);
		}
		return IsRectInView(march.position, march.GetMarchBlockSize());
	}

	private static bool IntersectsSegment(Rect rect, Vector2 p1, Vector2 p2)
	{
		float num = Mathf.Min(p1.x, p2.x);
		float num2 = Mathf.Max(p1.x, p2.x);
		if ((double)num2 > (double)rect.xMax)
		{
			num2 = rect.xMax;
		}
		if ((double)num < (double)rect.xMin)
		{
			num = rect.xMin;
		}
		if ((double)num > (double)num2)
		{
			return false;
		}
		float num3 = Mathf.Min(p1.y, p2.y);
		float num4 = Mathf.Max(p1.y, p2.y);
		float num5 = p2.x - p1.x;
		if ((double)Mathf.Abs(num5) > 1.40129846432482E-45)
		{
			float num6 = (p2.y - p1.y) / num5;
			float num7 = p1.y - num6 * p1.x;
			num3 = num6 * num + num7;
			num4 = num6 * num2 + num7;
		}
		if ((double)num3 > (double)num4)
		{
			float num8 = num4;
			num4 = num3;
			num3 = num8;
		}
		if ((double)num4 > (double)rect.yMax)
		{
			num4 = rect.yMax;
		}
		if ((double)num3 < (double)rect.yMin)
		{
			num3 = rect.yMin;
		}
		return (double)num3 <= (double)num4;
	}

	public static bool AxisAlignRectIntersectAxisAlignSegment(Rect rect, Vector2 p1, Vector2 p2)
	{
		if (Math.Abs(p2.x - p1.x) < 0.01f)
		{
			if (p1.x < rect.xMin || p1.x > rect.xMax)
			{
				return false;
			}
			double num = Math.Min(p1.y, p2.y);
			if ((double)Math.Max(p1.y, p2.y) >= (double)rect.yMin)
			{
				return num <= (double)rect.yMax;
			}
			return false;
		}
		if (Math.Abs(p1.y - p2.y) < 0.01f)
		{
			if (p1.y < rect.yMin || p1.y > rect.yMax)
			{
				return false;
			}
			double num2 = Math.Min(p1.x, p2.x);
			if ((double)Math.Max(p1.x, p2.x) >= (double)rect.xMin)
			{
				return num2 <= (double)rect.xMax;
			}
			return false;
		}
		Log.Error("IsAxisAlignedSegmentIntersect 线段没有轴对齐，请使用IntersectsSegment方法 " + p1.ToString() + ", " + p2.ToString());
		return false;
	}

	private void UpdateLastMarchMessage(WorldMarchMessageType type, ISFSObject message)
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		message.PutLong("msgTime", serverTime);
		if (type == WorldMarchMessageType.BlockGet)
		{
			if (message.ContainsKey("marchInfos"))
			{
				ISFSArray sFSArray = message.GetSFSArray("marchInfos");
				int count = sFSArray.Count;
				for (int i = 0; i < count; i++)
				{
					long key = sFSArray.GetSFSObject(i).TryGetLong("uuid");
					_messagesMergeDict[key] = (message, type);
				}
			}
		}
		else if (message.ContainsKey("uuid"))
		{
			ByteArray byteArray = message.GetByteArray("mdpm");
			if (byteArray == null || byteArray.Bytes == null || byteArray.Length <= 0)
			{
				long key2 = message.TryGetLong("uuid");
				_messagesMergeDict[key2] = (message, type);
			}
		}
	}

	private bool TryExcuteMarchMessage(WorldMarchMessageType type, ISFSObject message)
	{
		if (type == WorldMarchMessageType.BlockGet)
		{
			if (!message.ContainsKey("marchInfos"))
			{
				return true;
			}
			ISFSArray sFSArray = message.GetSFSArray("marchInfos");
			int count = sFSArray.Count;
			for (int i = 0; i < count; i++)
			{
				long key = sFSArray.GetSFSObject(i).TryGetLong("uuid");
				if (_messagesMergeDict.TryGetValue(key, out (ISFSObject, WorldMarchMessageType) value) && message == value.Item1)
				{
					_messagesMergeDict.Remove(key);
				}
			}
		}
		else
		{
			long key2 = message.TryGetLong("uuid");
			if (_messagesMergeDict.TryGetValue(key2, out (ISFSObject, WorldMarchMessageType) value2))
			{
				long num = message.TryGetLong("msgTime");
				long num2 = value2.Item1.TryGetLong("msgTime");
				if (num != 0L && num2 != 0L && num < num2)
				{
					return false;
				}
			}
			_messagesMergeDict.Remove(key2);
		}
		return true;
	}

	private void HandleMessage((WorldMarchMessageType type, ISFSObject obj) message)
	{
		if (TryExcuteMarchMessage(message.type, message.obj))
		{
			switch (message.type)
			{
			case WorldMarchMessageType.BlockGet:
				HandleWorldMarchGetImpl(message.obj);
				break;
			case WorldMarchMessageType.PushAdd:
				HandlePushWorldMarchAddImpl(message.obj);
				break;
			case WorldMarchMessageType.PushDel:
				HandlePushWorldMarchDelImpl(message.obj);
				break;
			case WorldMarchMessageType.Formation:
				HandleFormationMarchImpl(message.obj);
				break;
			case WorldMarchMessageType.FormationUpdate:
				HandleFormationMarchChangeImpl(message.obj);
				break;
			default:
				throw new ArgumentException();
			}
		}
	}

	public void HandleWorldMarchGet(ISFSObject message)
	{
		_lastGetBlockMsgTime = System.DateTime.UtcNow;
		ISFSArray sFSArray = message.GetSFSArray("serverMarchArr");
		if (sFSArray != null)
		{
			int count = sFSArray.Count;
			SFSObject sFSObject = new SFSObject();
			SFSArray sFSArray2 = new SFSArray();
			bool flag = GameEntry.Data?.Player?.IsInBattleField() ?? false;
			List<long> list = new List<long>();
			List<int> list2 = new List<int>();
			_logMarchUuids1.Clear();
			for (int i = 0; i < count; i++)
			{
				ISFSObject sFSObject2 = sFSArray.GetSFSObject(i);
				int item = sFSObject2.GetInt("serverId");
				list2.Add(item);
				long[] longArray = sFSObject2.GetLongArray("uuidSet");
				if (longArray != null)
				{
					long[] array = longArray;
					foreach (long item2 in array)
					{
						list.Add(item2);
					}
				}
				ISFSArray sFSArray3 = sFSObject2.GetSFSArray("marchInfos");
				if (sFSArray3 != null)
				{
					int count2 = sFSArray3.Count;
					for (int k = 0; k < count2; k++)
					{
						ISFSObject sFSObject3 = sFSArray3.GetSFSObject(k);
						sFSArray2.AddSFSObject(sFSObject3);
						long item3 = sFSObject3.TryGetLong("uuid");
						_logMarchUuids1.Add(item3);
					}
				}
				if (flag)
				{
					sFSObject.PutBool("marchOptimize", sFSObject2.GetBool("marchOptimize"));
				}
				else
				{
					sFSObject.PutBool("marchOptimize", val: true);
				}
			}
			sFSObject.PutBool("serverMarchArrMode", val: true);
			sFSObject.PutIntArray("serverSet", list2.ToArray());
			sFSObject.PutLongArray("uuidSet", list.ToArray());
			sFSObject.PutSFSArray("marchInfos", sFSArray2);
			if (list2.Count != 0)
			{
				_messages.Push((WorldMarchMessageType.BlockGet, sFSObject));
				UpdateLastMarchMessage(WorldMarchMessageType.BlockGet, sFSObject);
			}
		}
		else
		{
			_messages.Push((WorldMarchMessageType.BlockGet, message));
			UpdateLastMarchMessage(WorldMarchMessageType.BlockGet, message);
		}
	}

	public void HandleWorldMarchGetImpl(ISFSObject message)
	{
		_lastHandleBlockMsgTime = System.DateTime.UtcNow;
		ClearDelayToRefreshMarch();
		bool num = message.ContainsKey("marchOptimize") && message.GetBool("marchOptimize");
		bool flag = false;
		if (num)
		{
			if (__INC_UPDATE__ < 0)
			{
				__ReInit();
				flag = true;
			}
			__INC_UPDATE__ = 1;
		}
		else
		{
			if (__INC_UPDATE__ > 0)
			{
				__ReInit();
				flag = true;
			}
			__INC_UPDATE__ = -1;
		}
		if (flag && SceneManager.World != null)
		{
			SceneManager.World.SetFirstViewRequestFlag(isFirstTime: true);
			SceneManager.World.UpdateViewRequest(isForce: true);
		}
		if (__INC_UPDATE__ > 0)
		{
			HashSet<long> hashSet = new HashSet<long>(message.GetLongArray("uuidSet"));
			hashSet.UnionWith(myMarchUuids);
			foreach (long item in allMarchUuids.Except(hashSet))
			{
				WorldMarch march = GetMarch(item);
				AddMarchOptLog(march, 103);
				DestroyMarch(item, isBattleFail: false);
			}
			if (CommonUtils.IsDebug())
			{
				ISFSArray sFSArray = message.GetSFSArray("marchInfos");
				if (sFSArray != null)
				{
					for (int i = 0; i < sFSArray.Count; i++)
					{
						long num2 = sFSArray.GetSFSObject(i).TryGetLong("uuid");
						if (!hashSet.Contains(num2))
						{
							Log.Error($"AOI::Get消息中 marchinfo 不在 uuidset 里：{num2}");
						}
					}
				}
			}
			allMarchUuids = hashSet;
			GameEntry.Event.Fire(EventId.UpdateMarchItem);
		}
		if (message.ContainsKey("marchInfos"))
		{
			ParseDataWorldMarchGet(message);
		}
	}

	private void ParseDataWorldMarchGet(ISFSObject message)
	{
		_tmpMarchSet.Clear();
		_logMarchUuids2.Clear();
		ISFSArray sFSArray = message.GetSFSArray("marchInfos");
		int count = sFSArray.Count;
		for (int i = 0; i < count; i++)
		{
			ISFSObject sFSObject = sFSArray.GetSFSObject(i);
			long num = sFSObject.TryGetLong("uuid");
			_tmpMarchSet.Add(num);
			if (!HasMarchUuid(num))
			{
				continue;
			}
			string value = sFSObject.TryGetString("eventId");
			string text = sFSObject.TryGetString("belongUid");
			if (!string.IsNullOrEmpty(value) && text != GameEntry.Data.Player.Uid)
			{
				continue;
			}
			if (_delayDestroyTroop.ContainsKey(num))
			{
				_delayDestroyTroop.Remove(num);
			}
			if (!GameEntry.Data.Player.GetIsAlreadyBerserkBossReward(num))
			{
				_logMarchUuids2.Add(num);
				if (!allMarches.TryGetValue(num, out var value2))
				{
					value2 = new WorldMarch();
				}
				else
				{
					CheckToMeMarchDirty(value2);
				}
				value2.UpdateWorldMarch(sFSObject);
				UpdateMarch(value2);
			}
		}
		if (__INC_UPDATE__ > 0)
		{
			bool flag = false;
			_toRemoveList.Clear();
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				long key = allMarch.Key;
				if (!allMarchUuids.Contains(key) && !_delayDestroyTroop.ContainsKey(key) && !myMarchUuids.Contains(key) && !allMarch.Value.isFake)
				{
					flag = true;
					_toRemoveList.Add(key);
					allMarchUuids.Add(key);
				}
			}
			if (flag)
			{
				if (CommonUtils.IsDebug() && _toRemoveList.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder("AOI::客户端有未收管理的数据：");
					for (int j = 0; j < _toRemoveList.Count; j++)
					{
						stringBuilder.Append(_toRemoveList[j] + ",");
					}
					Log.Error(stringBuilder.ToString());
				}
				if (world != null)
				{
					world.SetFirstViewRequestFlag(isFirstTime: true);
				}
			}
		}
		else
		{
			_toRemoveList.Clear();
			foreach (KeyValuePair<long, WorldMarch> allMarch2 in allMarches)
			{
				if (!IsFakeSampleMarchData(allMarch2.Key) && !IsFakeAttackMonsterMarchData(allMarch2.Key) && !NeedReserveFakeRetreatMarchData(allMarch2.Value) && !_tmpMarchSet.Contains(allMarch2.Key) && !toMeMarchUuids.Contains(allMarch2.Key) && !myMarchUuids.Contains(allMarch2.Key))
				{
					_toRemoveList.Add(allMarch2.Key);
				}
			}
			foreach (long toRemove in _toRemoveList)
			{
				WorldMarch march = GetMarch(toRemove);
				AddMarchOptLog(march, 203);
				DestroyMarch(toRemove, isBattleFail: false);
			}
		}
		FireMyOrToMeMarchRefresh();
	}

	public void HandleWorldGetRectMarchInfos(ISFSObject message)
	{
		_messages.Clear();
		_messagesMergeDict.Clear();
		HashSet<long> hashSet = new HashSet<long>();
		hashSet.UnionWith(toMeMarchUuids);
		hashSet.UnionWith(myMarchUuids);
		toMeMarchUuids.Clear();
		toMeMarchTargetUuids.Clear();
		myMarchUuids.Clear();
		ownerMarches.Clear();
		myAssistancePoint2Uuids.Clear();
		myAssitanceUuid2Points.Clear();
		team2MarchUuid.Clear();
		member2LeaderUuid.Clear();
		toMeMarchDirty = true;
		toMeMarchNeedUpdate = true;
		toMeMarchDesertNeedUpdate = true;
		myMarchDirty = true;
		if (message.ContainsKey("marchInfos"))
		{
			ISFSArray sFSArray = message.GetSFSArray("marchInfos");
			int count = sFSArray.Count;
			_tmpMarchList.Clear();
			for (int i = 0; i < count; i++)
			{
				ISFSObject sFSObject = sFSArray.GetSFSObject(i);
				long num = sFSObject.TryGetLong("uuid");
				hashSet.Remove(num);
				string value = sFSObject.TryGetString("eventId");
				string text = sFSObject.TryGetString("belongUid");
				if (string.IsNullOrEmpty(value) || !(text != GameEntry.Data.Player.Uid))
				{
					if (_delayDestroyTroop.ContainsKey(num))
					{
						_delayDestroyTroop.Remove(num);
					}
					if (!allMarches.TryGetValue(num, out var value2))
					{
						value2 = new WorldMarch();
					}
					value2.UpdateWorldMarch(sFSObject);
					if (IsMyMarch(value2) || IsTargetForMine(value2))
					{
						UpdateMarch(value2);
					}
					else
					{
						_tmpMarchList.Add(value2);
					}
				}
			}
			foreach (WorldMarch tmpMarch in _tmpMarchList)
			{
				if (IsMyJoinAssemblyMarch(tmpMarch))
				{
					UpdateMarch(tmpMarch);
				}
			}
		}
		foreach (long item in hashSet)
		{
			WorldMarch march = GetMarch(item);
			AddMarchOptLog(march, 303);
			DestroyMarch(item, isBattleFail: false);
		}
		FireMyOrToMeMarchRefresh();
	}

	private void BeforeFireMyOrToMeMarchRefresh()
	{
		toMeMarchTargetUuids.Clear();
		foreach (long toMeMarchUuid in toMeMarchUuids)
		{
			if (allMarches.TryGetValue(toMeMarchUuid, out var value) && value != null && value.targetUuid > 0)
			{
				toMeMarchTargetUuids[value.targetUuid] = value.targetPos;
			}
		}
	}

	public bool CheckIsOtherTarget(long uuid, int targetPos)
	{
		if (!toMeMarchTargetUuids.TryGetValue(uuid, out var value))
		{
			return false;
		}
		return value == targetPos;
	}

	private void FireMyOrToMeMarchRefresh()
	{
		if (toMeMarchDirty)
		{
			BeforeFireMyOrToMeMarchRefresh();
			GameEntry.Event.Fire(EventId.MarchItemTargetMeUpdate);
			toMeMarchDirty = false;
		}
		if (!myMarchDirty)
		{
			return;
		}
		GameEntry.Event.Fire(EventId.MarchItemUpdateSelf);
		myMarchDirty = false;
		if (MultiKillSwitch != true)
		{
			return;
		}
		foreach (WorldMarch value in ownerMarches.Values)
		{
			if (value.ownerUid == GameEntry.Data.Player.Uid)
			{
				if (MultiKillPVEMin <= value.pveNum && GetMyMarchMultiKillPVE(value.uuid) < value.pveNum)
				{
					myMarchMultiKillPVE[value.uuid] = value.pveNum;
					GameEntry.Event.Fire(EventId.MyMarchMultiKillPVEAdd, value.uuid);
				}
				if (MultiKillPVPMin <= value.pvpNum && GetMyMarchMultiKillPVP(value.uuid) < value.pvpNum)
				{
					myMarchMultiKillPVP[value.uuid] = value.pvpNum;
					GameEntry.Event.Fire(EventId.MyMarchMultiKillPVPAdd, value.uuid);
				}
			}
		}
	}

	public void HandlePushWorldMarchAdd(ISFSObject message)
	{
		_messages.Push((WorldMarchMessageType.PushAdd, message));
		UpdateLastMarchMessage(WorldMarchMessageType.PushAdd, message);
	}

	public void HandlePushWorldMarchAddImpl(ISFSObject message)
	{
		if (!message.ContainsKey("uuid"))
		{
			return;
		}
		if (__INC_UPDATE__ > 0)
		{
			long item = message.GetLong("uuid");
			if (!allMarchUuids.Contains(item))
			{
				allMarchUuids.Add(item);
			}
		}
		ParsePushWorldMarchAdd(message, WorldMarchMessageType.PushAdd);
	}

	public void ParsePushWorldMarchAdd(ISFSObject message, WorldMarchMessageType msgType)
	{
		long num = message.TryGetLong("uuid");
		if (!HasMarchUuid(num))
		{
			return;
		}
		string value = message.TryGetString("eventId");
		string text = message.TryGetString("belongUid");
		if (!string.IsNullOrEmpty(value) && text != GameEntry.Data.Player.Uid)
		{
			return;
		}
		if (_delayDestroyTroop.ContainsKey(num))
		{
			_delayDestroyTroop.Remove(num);
		}
		if (GameEntry.Data.Player.GetIsAlreadyBerserkBossReward(num))
		{
			return;
		}
		bool flag = false;
		ByteArray byteArray = message.GetByteArray("mdpm");
		if (!allMarches.TryGetValue(num, out var value2))
		{
			if (byteArray != null && byteArray.Bytes != null && byteArray.Length > 0)
			{
				if (_invalidIncrementPushWmNewMap == null)
				{
					_invalidIncrementPushWmNewMap = new HashSet<long>();
				}
				if (!_invalidIncrementPushWmNewMap.Contains(num) && world != null)
				{
					SetMarchOptLog(num);
					_invalidIncrementPushWmNewMap.Add(num);
					if (!_delayToRefresh)
					{
						_delayToRefresh = true;
						_delayRefreshTimer = GameEntry.Timer.RegisterTimer(1f, DelayToRefreshMarch);
					}
				}
				return;
			}
			if (_invalidIncrementPushWmNewMap != null && _invalidIncrementPushWmNewMap.Contains(num))
			{
				_invalidIncrementPushWmNewMap.Remove(num);
			}
			value2 = new WorldMarch();
			value2.UpdateWorldMarch(message, isPushAdd: true);
		}
		else
		{
			CheckToMeMarchDirty(value2);
			flag = value2.IsFrozen();
			if (byteArray == null || byteArray.Bytes == null || byteArray.Length == 0)
			{
				value2.UpdateWorldMarch(message);
			}
			else
			{
				WorldMarchDeltaMeta delta = WorldMarchDeltaMeta.Parser.ParseFrom(byteArray.Bytes);
				value2.IncrementalUpdateWorldMarch(message, delta);
			}
		}
		UpdateMarch(value2);
		if (world != null)
		{
			world.CreateTroopLine(value2);
			if (flag && !value2.IsFrozen())
			{
				world.OnMonsterIceBroken(value2.uuid);
			}
		}
		FireMyOrToMeMarchRefresh();
		GameEntry.Event.Fire(EventId.SingleMarchStateUpdate, value2.uuid);
		if (msgType == WorldMarchMessageType.Formation)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.OnLaunchMarchSuccess", value2.GetMarchTargetType(), value2.targetUuid, value2.targetPos);
		}
	}

	private void SetMarchOptLog(long uuid)
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("log:");
			if (_addMarchLogTime.ContainsKey(uuid))
			{
				string value = $"Add Time{_addMarchLogTime[uuid]}";
				stringBuilder.AppendLine(value);
			}
			if (_clearMarchesLogSet != null && _clearMarchesLogSet.Contains(uuid))
			{
				string value2 = $"Clear Type:{_clearMarchesFrom} Time{_clearMarchesLogTime}";
				stringBuilder.AppendLine(value2);
			}
			else if (_prevClearMarchesLogSet != null && _prevClearMarchesLogSet.Contains(uuid))
			{
				string value3 = $"Clear(prev) Type:{_prevClearMarchesFrom} Time{_prevClearMarchesLogTime}";
				stringBuilder.AppendLine(value3);
			}
			if (_worldMarchLogMap != null && _worldMarchLogMap.ContainsKey(uuid))
			{
				stringBuilder.AppendLine("opt:");
				List<int> list = _worldMarchLogMap[uuid];
				for (int i = 0; i < list.Count; i++)
				{
					stringBuilder.AppendLine(list[i].ToString());
				}
				Log.Info($"[IncreUpdate]Wrong:{uuid}{stringBuilder}");
				return;
			}
			if (_prevWorldMarchLogMap != null && _prevWorldMarchLogMap.ContainsKey(uuid))
			{
				stringBuilder.AppendLine("opt(prev):");
				List<int> list2 = _prevWorldMarchLogMap[uuid];
				for (int j = 0; j < list2.Count; j++)
				{
					stringBuilder.AppendLine(list2[j].ToString());
				}
				Log.Info($"[IncreUpdate]Wrong:{uuid}{stringBuilder}");
				return;
			}
			List<long> list3 = allMarches.Keys.ToList();
			stringBuilder.AppendLine("lastGetBlockTime:" + _lastGetBlockMsgTime);
			stringBuilder.AppendLine("lastHandleBlockTime:" + _lastHandleBlockMsgTime);
			stringBuilder.AppendLine("now:" + System.DateTime.UtcNow);
			stringBuilder.AppendLine("log1:" + _logMarchUuids1.Count);
			for (int k = 0; k < _logMarchUuids1.Count; k++)
			{
				long num = _logMarchUuids1[k];
				if (_logMarchUuids2.IndexOf(num) == -1)
				{
					stringBuilder.Append(num + ";");
				}
			}
			stringBuilder.AppendLine("log2:" + _logMarchUuids2.Count);
			for (int l = 0; l < _logMarchUuids2.Count; l++)
			{
				long num2 = _logMarchUuids2[l];
				if (list3.IndexOf(num2) == -1)
				{
					stringBuilder.Append(num2 + ";");
				}
			}
			stringBuilder.AppendLine("allMarches:" + list3.Count);
			for (int m = 0; m < list3.Count; m++)
			{
				long num3 = list3[m];
				stringBuilder.Append(num3 + ";");
			}
			Log.Info($"[IncreUpdate]Wrong:{uuid} not find -> {stringBuilder}");
		}
		catch (Exception)
		{
			Log.Error("[IncreUpdate]日志拼接失败");
		}
	}

	public void HandlePushWorldMarchDel(ISFSObject message)
	{
		_messages.Push((WorldMarchMessageType.PushDel, message));
		UpdateLastMarchMessage(WorldMarchMessageType.PushDel, message);
	}

	public void HandlePushWorldMarchDelImpl(ISFSObject message)
	{
		if (message.ContainsKey("uuid"))
		{
			if (__INC_UPDATE__ > 0)
			{
				long item = message.GetLong("uuid");
				allMarchUuids.Remove(item);
			}
			long num = message.TryGetLong("uuid");
			bool isBattleFail = message.GetBool("isBattleFail");
			WorldMarch march = GetMarch(num);
			AddMarchOptLog(march, 403);
			DestroyMarch(num, isBattleFail);
			FireMyOrToMeMarchRefresh();
		}
	}

	public void HandleFormationMarch(ISFSObject message)
	{
		_messages.Push((WorldMarchMessageType.Formation, message));
		UpdateLastMarchMessage(WorldMarchMessageType.Formation, message);
	}

	public void HandleFormationMarchImpl(ISFSObject message)
	{
		if (GameEntry.Data.Player.GetSourceServerId() == 180)
		{
			Log.Info("HandleFormationMarch.receiveMsg");
		}
		ParseFormationUpdate(message, WorldMarchMessageType.Formation);
	}

	public void HandleFormationMarchChange(ISFSObject message)
	{
		_messages.Push((WorldMarchMessageType.FormationUpdate, message));
		UpdateLastMarchMessage(WorldMarchMessageType.FormationUpdate, message);
	}

	public void HandleFormationMarchChangeImpl(ISFSObject message)
	{
		ParseFormationUpdate(message, WorldMarchMessageType.FormationUpdate);
	}

	private void ParseFormationUpdate(ISFSObject message, WorldMarchMessageType msgType)
	{
		if (!message.ContainsKey("uuid"))
		{
			return;
		}
		if (__INC_UPDATE__ > 0)
		{
			long item = message.GetLong("uuid");
			if (!allMarchUuids.Contains(item))
			{
				allMarchUuids.Add(item);
			}
		}
		ParsePushWorldMarchAdd(message, msgType);
		if (message.ContainsKey("resource"))
		{
			ISFSObject sFSObject = message.GetSFSObject("resource");
			GameEntry.Lua.Call("LuaEntry.Resource:UpdateResource", ((SFSObject)sFSObject).ToLuaTable(GameEntry.Lua.Env));
		}
		FireMyOrToMeMarchRefresh();
	}

	public void DelFakeAttackMonsterMarchDataRandom(int count)
	{
		if (count >= fakeAttackMonsterMarches.Count)
		{
			foreach (KeyValuePair<long, WorldMarch> fakeAttackMonsterMarch in fakeAttackMonsterMarches)
			{
				DestroyMarch(fakeAttackMonsterMarch.Key, isBattleFail: false);
			}
			fakeRetreatMarches.Clear();
			return;
		}
		List<long> list = new List<long>(fakeAttackMonsterMarches.Keys);
		while (count > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			fakeRetreatMarches.Remove(list[index]);
			DestroyMarch(list[index], isBattleFail: false);
			list.RemoveAt(index);
			count--;
		}
	}

	public void AddFakeAttackMonsterMarchData(long startIndex, long endIndex, float marchTimeSec, string ownerUid)
	{
		if (!(world == null))
		{
			DCPlayer player = GameEntry.Data.Player;
			FakeWorldMarch fakeWorldMarch = new FakeWorldMarch();
			fakeWorldMarch.startTime = GameEntry.Timer.GetServerTime();
			fakeWorldMarch.endTime = fakeWorldMarch.startTime + (long)(marchTimeSec * 1000f);
			fakeWorldMarch.fakeMarchTime = fakeWorldMarch.endTime - fakeWorldMarch.startTime;
			fakeWorldMarch.type = NewMarchType.NORMAL;
			fakeWorldMarch.target = MarchTargetType.ATTACK_MONSTER;
			fakeWorldMarch.status = MarchStatus.MOVING;
			fakeWorldMarch.uuid = System.DateTime.Now.Ticks;
			fakeWorldMarch.ownerUid = ownerUid;
			fakeWorldMarch.ownerName = ((ownerUid == player.GetUid()) ? player.GetName() : "Fake Attack March");
			fakeWorldMarch.pic = "";
			fakeWorldMarch.picVer = 0;
			fakeWorldMarch.allianceUid = ((ownerUid == player.GetUid()) ? player.GetAllianceId() : string.Empty);
			fakeWorldMarch.allianceAbbr = "";
			fakeWorldMarch.allianceIcon = "";
			PointInfo pointInfo = world.GetPointInfo((int)endIndex);
			if (pointInfo != null)
			{
				fakeWorldMarch.targetUuid = pointInfo.uuid;
			}
			fakeWorldMarch.targetServer = player.GetCurServerId();
			fakeWorldMarch.srcServer = player.GetCurServerId();
			fakeWorldMarch.startPos = (int)startIndex;
			fakeWorldMarch.targetPos = (int)endIndex;
			fakeWorldMarch.pathStartServerId = fakeWorldMarch.srcServer;
			fakeWorldMarch.targetWorldPos = TileCoord.TileIndexToWorld(fakeWorldMarch.targetPos, ForceChangeScene.World, fakeWorldMarch.targetServer);
			fakeWorldMarch.startWorldPos = TileCoord.TileIndexToWorld(fakeWorldMarch.startPos, ForceChangeScene.World, fakeWorldMarch.srcServer);
			Vector2Int item = world.IndexToTilePos(fakeWorldMarch.startPos);
			Vector2Int item2 = world.IndexToTilePos(fakeWorldMarch.targetPos);
			double num = Math.Sqrt(Math.Pow(item.x - item2.x, 2.0) + Math.Pow(item.y - item2.y, 2.0));
			fakeWorldMarch.speed = (float)num * 1000f / (float)fakeWorldMarch.fakeMarchTime;
			string text = WorldPathfinding.PathToString(new List<Vector2Int> { item, item2 });
			fakeWorldMarch.path = (from a in text.Split(new char[1] { ';' })
				select a.ToInt()).ToArray();
			fakeAttackMonsterMarches[fakeWorldMarch.uuid] = fakeWorldMarch;
			FakeAddMarch(fakeWorldMarch);
			ArmyInfo armyInfo = new ArmyInfo();
			fakeWorldMarch.armyInfos.Add(armyInfo);
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.RandomHeroIdsAndTacWeaponInfo");
			HeroInfo heroInfo = new HeroInfo();
			heroInfo.index = 1;
			heroInfo.heroId = luaTable.Get<int>("heroId_1");
			armyInfo.HeroInfos.Add(heroInfo);
			heroInfo = new HeroInfo();
			heroInfo.index = 2;
			heroInfo.heroId = luaTable.Get<int>("heroId_2");
			armyInfo.HeroInfos.Add(heroInfo);
			heroInfo = new HeroInfo();
			heroInfo.index = 3;
			heroInfo.heroId = luaTable.Get<int>("heroId_3");
			armyInfo.HeroInfos.Add(heroInfo);
			heroInfo = new HeroInfo();
			heroInfo.index = 4;
			heroInfo.heroId = luaTable.Get<int>("heroId_4");
			armyInfo.HeroInfos.Add(heroInfo);
			heroInfo = new HeroInfo();
			heroInfo.index = 5;
			heroInfo.heroId = luaTable.Get<int>("heroId_5");
			armyInfo.HeroInfos.Add(heroInfo);
			TacWeaponInfo tacWeaponInfo = new TacWeaponInfo();
			tacWeaponInfo.weaponId = luaTable.Get<int>("weaponId");
			tacWeaponInfo.weaponLevel = luaTable.Get<int>("weaponLv");
			armyInfo.WeaponInfo = tacWeaponInfo;
			fakeWorldMarch.isFake = true;
			fakeWorldMarch.isFakeAttack = true;
			fakeWorldMarch.IsValid = true;
		}
	}

	public void UpdateFakeAttackMonsterMarch(long uuid)
	{
		if (world == null || !fakeAttackMonsterMarches.TryGetValue(uuid, out var value))
		{
			return;
		}
		if (value.target == MarchTargetType.BACK_HOME)
		{
			AddMarchOptLog(value, 2);
			allMarches.Remove(uuid);
			fakeAttackMonsterMarches.Remove(uuid);
			value.IsValid = false;
			world.DestroyTroopLine(uuid);
			return;
		}
		DCPlayer player = GameEntry.Data.Player;
		int targetPos = value.targetPos;
		value.targetServer = player.GetCurServerId();
		value.srcServer = player.GetCurServerId();
		value.pathStartServerId = value.srcServer;
		value.targetPos = value.startPos;
		value.startPos = targetPos;
		value.status = MarchStatus.MOVING;
		value.target = MarchTargetType.BACK_HOME;
		value.startTime = GameEntry.Timer.GetServerTime();
		value.endTime = value.startTime + value.fakeMarchTime;
		Vector2Int item = world.IndexToTilePos(value.startPos);
		Vector2Int item2 = world.IndexToTilePos(value.targetPos);
		double num = Math.Sqrt(Math.Pow(item.x - item2.x, 2.0) + Math.Pow(item.y - item2.y, 2.0));
		value.speed = (float)num * 1000f / (float)value.fakeMarchTime;
		string text = WorldPathfinding.PathToString(new List<Vector2Int> { item, item2 });
		value.path = (from a in text.Split(new char[1] { ';' })
			select a.ToInt()).ToArray();
		GameEntry.Event.Fire(EventId.MarchItemUpdateSelf);
	}

	private long GenerateFakeUuid()
	{
		return --_fakeUuid;
	}

	public void AddFakeRetreatMarchData(WorldMarch realMarch, PointInfo info, int timeDelta)
	{
		if (!(world == null))
		{
			DCPlayer player = GameEntry.Data.Player;
			FakeWorldMarch fakeWorldMarch = new FakeWorldMarch();
			fakeWorldMarch.parentUuid = realMarch.uuid;
			fakeWorldMarch.allianceAbbr = realMarch.allianceAbbr;
			fakeWorldMarch.allianceName = realMarch.allianceName;
			fakeWorldMarch.allianceIcon = realMarch.allianceIcon;
			int num = UnityEngine.Random.Range(0, timeDelta);
			fakeWorldMarch.startTime = realMarch.startTime + num;
			fakeWorldMarch.endTime = realMarch.endTime + num;
			fakeWorldMarch.cityId = realMarch.cityId;
			fakeWorldMarch.type = NewMarchType.ZOMBIE_RETREAT;
			fakeWorldMarch.target = MarchTargetType.NORMAL_FAKE_MARCH;
			fakeWorldMarch.status = MarchStatus.MOVING;
			fakeWorldMarch.uuid = GenerateFakeUuid();
			fakeWorldMarch.pic = "";
			fakeWorldMarch.picVer = 0;
			if (info != null)
			{
				fakeWorldMarch.targetUuid = info.uuid;
			}
			fakeWorldMarch.targetServer = player.GetCurServerId();
			fakeWorldMarch.srcServer = player.GetCurServerId();
			fakeWorldMarch.pathStartServerId = fakeWorldMarch.srcServer;
			fakeWorldMarch.startPos = realMarch.startPos;
			fakeWorldMarch.targetPos = realMarch.targetPos;
			fakeWorldMarch.targetWorldPos = TileCoord.TileIndexToWorld(fakeWorldMarch.targetPos, ForceChangeScene.World, fakeWorldMarch.targetServer);
			fakeWorldMarch.startWorldPos = TileCoord.TileIndexToWorld(fakeWorldMarch.startPos, ForceChangeScene.World, fakeWorldMarch.srcServer);
			Vector2Int item = world.IndexToTilePos(fakeWorldMarch.startPos);
			Vector2Int item2 = world.IndexToTilePos(fakeWorldMarch.targetPos);
			fakeWorldMarch.speed = realMarch.speed;
			string text = WorldPathfinding.PathToString(new List<Vector2Int> { item, item2 });
			fakeWorldMarch.path = (from a in text.Split(new char[1] { ';' })
				select a.ToInt()).ToArray();
			if (!fakeRetreatMarches.ContainsKey(realMarch.uuid))
			{
				fakeRetreatMarches[realMarch.uuid] = new List<WorldMarch>();
			}
			fakeRetreatMarches[realMarch.uuid].Add(fakeWorldMarch);
			FakeAddMarch(fakeWorldMarch);
			fakeWorldMarch.isFake = true;
		}
	}

	private bool NeedReserveFakeRetreatMarchData(WorldMarch march)
	{
		if (march.type == NewMarchType.ZOMBIE_RETREAT)
		{
			if (march.isFake)
			{
				if (allMarches.ContainsKey(march.parentUuid))
				{
					return true;
				}
				if (fakeRetreatMarches.ContainsKey(march.parentUuid))
				{
					fakeRetreatMarches[march.parentUuid].Clear();
					fakeRetreatMarches.Remove(march.parentUuid);
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void AddFakeSampleMarchData(long startIndex, long endIndex, long startTime, long endTime, int marchTargetType, int srcServer = -1, int targetServer = -1)
	{
		if (!(world == null) && !IsFakeSampleMarchData(endIndex))
		{
			DCPlayer player = GameEntry.Data.Player;
			PointInfo pointInfo = world.GetPointInfo((int)endIndex);
			FakeWorldMarch fakeWorldMarch = new FakeWorldMarch();
			fakeWorldMarch.endTime = endTime;
			fakeWorldMarch.allianceUid = player.GetAllianceId();
			fakeWorldMarch.allianceAbbr = "";
			fakeWorldMarch.allianceIcon = "";
			fakeWorldMarch.startTime = startTime;
			fakeWorldMarch.type = NewMarchType.NORMAL;
			fakeWorldMarch.target = (MarchTargetType)marchTargetType;
			fakeWorldMarch.status = MarchStatus.MOVING;
			fakeWorldMarch.uuid = endIndex;
			fakeWorldMarch.pic = "";
			fakeWorldMarch.picVer = 0;
			if (pointInfo != null)
			{
				fakeWorldMarch.targetUuid = pointInfo.uuid;
			}
			fakeWorldMarch.targetServer = ((targetServer > 0) ? targetServer : player.GetCurServerId());
			fakeWorldMarch.srcServer = ((srcServer > 0) ? srcServer : player.GetCurServerId());
			fakeWorldMarch.serverId = fakeWorldMarch.srcServer;
			fakeWorldMarch.pathStartServerId = fakeWorldMarch.srcServer;
			fakeWorldMarch.ownerName = player.GetName();
			fakeWorldMarch.ownerUid = player.GetUid();
			fakeWorldMarch.startPos = (int)startIndex;
			fakeWorldMarch.targetPos = (int)endIndex;
			fakeWorldMarch.targetWorldPos = TileCoord.TileIndexToWorld(fakeWorldMarch.targetPos, ForceChangeScene.World, fakeWorldMarch.targetServer);
			fakeWorldMarch.startWorldPos = TileCoord.TileIndexToWorld(fakeWorldMarch.startPos, ForceChangeScene.World, fakeWorldMarch.srcServer);
			Vector2Int vector2Int = world.IndexToTilePos(fakeWorldMarch.startPos);
			Vector2Int vector2Int2 = world.IndexToTilePos(fakeWorldMarch.targetPos);
			double num = Math.Sqrt(Math.Pow(vector2Int.x - vector2Int2.x, 2.0) + Math.Pow(vector2Int.y - vector2Int2.y, 2.0));
			fakeWorldMarch.speed = (float)num * 1000f / (float)(endTime - startTime);
			if (fakeWorldMarch.target == MarchTargetType.SAMPLE)
			{
				vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 0f);
			}
			string text = WorldPathfinding.PathToString(new List<Vector2Int> { vector2Int, vector2Int2 });
			fakeWorldMarch.path = (from a in text.Split(new char[1] { ';' })
				select a.ToInt()).ToArray();
			fakeSampleMarches[endIndex] = fakeWorldMarch;
			FakeAddMarch(fakeWorldMarch);
			fakeWorldMarch.isFake = true;
			fakeWorldMarch.IsValid = true;
		}
	}

	public Dictionary<long, WorldMarch> GetAllSampleFakeData()
	{
		return fakeSampleMarches;
	}

	public void UpdateFakeSampleMarchDataWhenStartPick(long index, long endTime)
	{
		if (IsFakeSampleMarchData(index))
		{
			WorldMarch worldMarch = fakeSampleMarches[index];
			worldMarch.startTime = worldMarch.endTime;
			worldMarch.endTime = endTime;
			worldMarch.status = MarchStatus.SAMPLING;
			GameEntry.Event.Fire(EventId.GarbageCollectStart, worldMarch.targetUuid);
			GameEntry.Event.Fire(EventId.MarchItemUpdateSelf);
		}
	}

	public void UpdateFakeSampleMarchDataWhenBack(long index, long startTime, long endTime)
	{
		if (!(world == null) && IsFakeSampleMarchData(index))
		{
			WorldMarch worldMarch = fakeSampleMarches[index];
			MarchTargetType target = worldMarch.target;
			worldMarch.targetPos = worldMarch.startPos;
			worldMarch.startPos = (int)index;
			worldMarch.status = MarchStatus.MOVING;
			worldMarch.target = MarchTargetType.BACK_HOME;
			worldMarch.startTime = startTime;
			worldMarch.endTime = endTime;
			Vector2Int vector2Int = world.IndexToTilePos(worldMarch.startPos);
			Vector2Int vector2Int2 = world.IndexToTilePos(worldMarch.targetPos);
			double num = Math.Sqrt(Math.Pow(vector2Int.x - vector2Int2.x, 2.0) + Math.Pow(vector2Int.y - vector2Int2.y, 2.0));
			worldMarch.speed = (float)num * 1000f / (float)(endTime - startTime);
			if (target == MarchTargetType.SAMPLE)
			{
				vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 0f);
			}
			string text = WorldPathfinding.PathToString(new List<Vector2Int> { vector2Int, vector2Int2 });
			worldMarch.path = (from a in text.Split(new char[1] { ';' })
				select a.ToInt()).ToArray();
			GameEntry.Event.Fire(EventId.MarchItemUpdateSelf);
		}
	}

	public void RemoveFakeSampleMarchData(long index)
	{
		if (!(world == null) && IsFakeSampleMarchData(index))
		{
			world.DestroyTroop(index);
			world.DestroyTroopLine(index);
			fakeSampleMarches.Remove(index);
			RemoveMarch(index);
		}
	}

	private bool IsFakeSampleMarchData(long index)
	{
		return fakeSampleMarches.ContainsKey(index);
	}

	private bool IsFakeAttackMonsterMarchData(long uuid)
	{
		return fakeAttackMonsterMarches.ContainsKey(uuid);
	}

	public bool ExistMarch(long uuid)
	{
		return allMarches.ContainsKey(uuid);
	}

	public bool IsInRallyMarch(long uuid)
	{
		bool result = false;
		if (allMarches.ContainsKey(uuid))
		{
			WorldMarch worldMarch = allMarches[uuid];
			if (worldMarch.status == MarchStatus.IN_TEAM || worldMarch.status == MarchStatus.WAIT_RALLY)
			{
				result = true;
			}
		}
		return result;
	}

	public bool IsInCollectMarch(long uuid)
	{
		bool result = false;
		if (allMarches.ContainsKey(uuid) && allMarches[uuid].status == MarchStatus.COLLECTING)
		{
			result = true;
		}
		return result;
	}

	public bool IsInAssistanceMarch(long uuid)
	{
		bool result = false;
		if (allMarches.ContainsKey(uuid) && allMarches[uuid].status == MarchStatus.ASSISTANCE)
		{
			result = true;
		}
		return result;
	}

	public bool IsSelfInCurrentMarchTeam(long rallyMarchUuid)
	{
		WorldMarch march = GetMarch(rallyMarchUuid);
		if (march == null)
		{
			return false;
		}
		if (march.IsMonsterOrBoss())
		{
			return false;
		}
		if (march.ownerUid.IsNullOrEmpty())
		{
			return false;
		}
		if (march.ownerUid == GameEntry.Data.Player.Uid)
		{
			return true;
		}
		long teamUuid = march.teamUuid;
		if (teamUuid <= 0)
		{
			return false;
		}
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		List<WorldMarch> list = GetOwnerMarches(GameEntry.Data.Player.Uid, allianceId);
		if (list != null)
		{
			foreach (WorldMarch item in list)
			{
				if (teamUuid == item.teamUuid)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasMarchUuid(long uuid)
	{
		if (__INC_UPDATE__ > 0)
		{
			return allMarchUuids.Contains(uuid);
		}
		return true;
	}

	public WorldMarch GetMarch(long uuid)
	{
		allMarches.TryGetValue(uuid, out var value);
		return value;
	}

	public WorldMarch GetMonster(long targetPoint)
	{
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			if (allMarch.Value.IsMonsterOrOrdinaryBoss() && allMarch.Value.IsVisibleMarch() && allMarch.Value.startPos == targetPoint && allMarch.Value.startPos == allMarch.Value.targetPos)
			{
				return allMarch.Value;
			}
		}
		return null;
	}

	public List<WorldMarch> GetOwnerMarches(string ownerUid = "", string allianceUid = "")
	{
		List<WorldMarch> list = new List<WorldMarch>();
		list.AddRange(ownerMarches.Values);
		return list;
	}

	public List<WorldMarch> GetMyDisguiseMarches(string ownerUid = "", string allianceUid = "")
	{
		List<WorldMarch> list = new List<WorldMarch>();
		foreach (WorldMarch value in ownerMarches.Values)
		{
			if (value.type == NewMarchType.FAKE_ATTACK)
			{
				list.Add(value);
			}
		}
		return list;
	}

	public WorldMarch GetAllianceMarchesInTeam(string allianceUid, long teamUuid)
	{
		if (!allianceUid.IsNullOrEmpty() && allianceMarches.ContainsKey(allianceUid))
		{
			foreach (WorldMarch item in allianceMarches[allianceUid])
			{
				if (item.teamUuid == teamUuid)
				{
					return item;
				}
			}
		}
		if (GameEntry.Data.Player.IsInBattleField(3))
		{
			string text = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetEpidemicTeammateByAllianceId", allianceUid);
			if (!text.IsNullOrEmpty() && allianceMarches.ContainsKey(text))
			{
				foreach (WorldMarch item2 in allianceMarches[text])
				{
					if (item2.teamUuid == teamUuid)
					{
						return item2;
					}
				}
			}
		}
		return null;
	}

	public bool IsHaveMarchInWorld(string allianceUid, long teamUuid)
	{
		foreach (WorldMarch ownerMarch in GetOwnerMarches())
		{
			if (ownerMarch.status != MarchStatus.DEFAULT && ownerMarch.type != NewMarchType.SCOUT && ownerMarch.type != NewMarchType.TREAT_VIRUS && ownerMarch.type != NewMarchType.LOTTO_RECEIVE && ownerMarch.type != NewMarchType.TRAIN && ownerMarch.type != NewMarchType.ZONE_MOBILIZATION_DONATE && ownerMarch.type != NewMarchType.MONSTER_CHALLENGE_DONATE)
			{
				return true;
			}
		}
		return false;
	}

	public WorldMarch GetBestMarch(int pointId, string allianceId, int worldId)
	{
		WorldMarch worldMarch = null;
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.worldId == worldId && value.targetPos == pointId && (allianceId.IsNullOrEmpty() || value.allianceUid == allianceId) && (worldMarch == null || worldMarch.power < value.power))
			{
				worldMarch = value;
			}
		}
		return worldMarch;
	}

	public void CleanDragonWar()
	{
		List<long> list = new List<long>();
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.worldId > 0)
			{
				list.Add(value.uuid);
			}
		}
		foreach (long item in list)
		{
			WorldMarch march = GetMarch(item);
			AddMarchOptLog(march, 503);
			DestroyMarch(item, isBattleFail: false);
		}
	}

	public WorldMarch GetOwnerFormationMarch(string ownerUid, long formationUuid, string allianceUid = "")
	{
		return GetOwnerMarches(ownerUid, allianceUid)?.Find((WorldMarch m) => m.ownerFormationUuid == formationUuid);
	}

	public void StartMarch(int targetType, int targetPoint, long targetUuid, int timeIndex, long marchUuid = 0L, long formationUuid = 0L, int backHome = 1, byte[] sfsObjBinary = null, int startPos = 0, int targetServerId = -1)
	{
		if (targetType == 2)
		{
			GameEntry.Event.Fire(EventId.UIMAIN_VISIBLE, true);
			if (world != null)
			{
				world.AutoLookat(SceneManager.World.CurTarget, SceneManager.World.InitZoom, 0.4f);
			}
		}
		if (formationUuid != 0L)
		{
			string allianceId = GameEntry.Data.Player.GetAllianceId();
			WorldMarch ownerFormationMarch = GetOwnerFormationMarch(GameEntry.Data.Player.Uid, formationUuid, allianceId);
			if (ownerFormationMarch != null)
			{
				SendChangeMarchToServer(ownerFormationMarch.uuid, targetType, targetPoint, targetUuid, backHome == 1, targetServerId);
				return;
			}
			SFSObject formationData = null;
			if (sfsObjBinary != null)
			{
				formationData = SFSObject.NewFromBinaryData(new ByteArray(sfsObjBinary));
			}
			SendCreateMarchToServer(formationUuid, targetType, targetPoint, targetUuid, timeIndex, formationData, startPos, backHome == 1, targetServerId);
		}
		else if (marchUuid != 0L)
		{
			SendChangeMarchToServer(marchUuid, targetType, targetPoint, targetUuid, backHome == 1, targetServerId);
		}
	}

	private static Vector2Int GetAttackPos(Vector2Int start, Vector2Int end, float attackOffsetRange)
	{
		float num = Vector2Int.Distance(start, end);
		float num2 = 1f - attackOffsetRange / num;
		return new Vector2Int(Mathf.RoundToInt((float)(end.x - start.x) * num2 + (float)start.x), Mathf.RoundToInt((float)(end.y - start.y) * num2 + (float)start.y));
	}

	private void SendCreateMarchToServer(long formationUuid, int targetType, int targetPoint, long targetUuid, int timeIndex, SFSObject formationData, int startPos, bool backHome = true, int targetServerId = -1)
	{
		int index = startPos;
		if (startPos <= 0)
		{
			LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(10100000);
			if (buildingDataByBuildId != null)
			{
				index = buildingDataByBuildId.pointId;
			}
		}
		Vector2Int vector2Int = TileCoord.IndexToTilePos(index, ForceChangeScene.World);
		Vector2Int vector2Int2 = TileCoord.IndexToTilePos(targetPoint, ForceChangeScene.World);
		if (targetType == 5 || targetType == 1 || targetType == 4 || targetType == 20 || targetType == 21 || targetType == 41 || targetType == 46 || targetType == 119 || targetType == 43 || targetType == 44 || targetType == 183 || targetType == 184 || targetType == 186 || targetType == 23)
		{
			vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 3f);
		}
		else
		{
			switch (targetType)
			{
			case 7:
				vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 3f);
				break;
			case 25:
			case 27:
			case 41:
			case 43:
			case 71:
			case 72:
			case 183:
			case 186:
			{
				Vector2Int end = vector2Int2 + new Vector2Int(-3, -3);
				vector2Int2 = GetAttackPos(vector2Int, end, 5f);
				break;
			}
			}
		}
		int worldId = GameEntry.Data.Player.GetWorldId();
		string path = WorldPathfinding.PathToString(new List<Vector2Int> { vector2Int, vector2Int2 });
		WorldMarchFormationMessage.Instance.Send(new WorldMarchFormationMessage.Request
		{
			formationUuid = formationUuid,
			path = path,
			targetUid = targetUuid,
			worldId = worldId,
			target = targetType,
			waitTimeIndex = timeIndex,
			autoBackHome = backHome,
			formationParam = formationData,
			targetServerId = targetServerId
		});
	}

	private void SendChangeMarchToServer(long marchUuid, int targetType, int targetPoint, long targetUuid, bool backHome = true, int targetServerId = -1)
	{
		WorldMarch march = GetMarch(marchUuid);
		if (march == null)
		{
			return;
		}
		int num = 0;
		if (march.status == MarchStatus.COLLECTING || march.status == MarchStatus.ASSISTANCE)
		{
			num = march.targetPos;
			if (world != null && march.status == MarchStatus.COLLECTING)
			{
				world.DestroyArmyAnimalObject(marchUuid);
			}
		}
		else
		{
			num = TileCoord.WorldToTileIndex(march.position, ForceChangeScene.World);
		}
		Vector2Int vector2Int = TileCoord.IndexToTilePos(num, ForceChangeScene.World);
		Vector2Int vector2Int2 = TileCoord.IndexToTilePos(targetPoint, ForceChangeScene.World);
		if (targetType == 5 || targetType == 1 || targetType == 4 || targetType == 20 || targetType == 21 || targetType == 41 || targetType == 46 || targetType == 119 || targetType == 43 || targetType == 44 || targetType == 183 || targetType == 184 || targetType == 186 || targetType == 23)
		{
			vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 3f);
		}
		else
		{
			switch (targetType)
			{
			case 7:
				vector2Int2 = GetAttackPos(vector2Int, vector2Int2, 3f);
				break;
			case 25:
			case 27:
			case 41:
			case 43:
			case 71:
			case 72:
			case 183:
			case 186:
			{
				Vector2Int end = vector2Int2 + new Vector2Int(-3, -3);
				vector2Int2 = GetAttackPos(vector2Int, end, 5f);
				break;
			}
			}
		}
		int worldId = GameEntry.Data.Player.GetWorldId();
		string path = WorldPathfinding.PathToString(new List<Vector2Int> { vector2Int, vector2Int2 });
		WorldMarchFormationChangeMessage.Instance.Send(new WorldMarchFormationChangeMessage.Request
		{
			uuid = marchUuid,
			path = path,
			targetUid = targetUuid,
			worldId = worldId,
			target = targetType,
			autoBackHome = backHome,
			targetServerId = targetServerId
		});
	}

	public void WorldGetMarchInfos()
	{
		int worldMainPos = GameEntry.Data.Building.GetWorldMainPos();
		if (worldMainPos > 0)
		{
			Vector2Int vector2Int = TileCoord.IndexToTilePos(worldMainPos, ForceChangeScene.World);
			WorldGetRectMarchInfosMessage.Instance.Send(new WorldGetRectMarchInfosMessage.Request
			{
				x = vector2Int.x,
				y = vector2Int.y
			});
		}
	}

	private bool NeedGetRealTargetPos(WorldMarch march)
	{
		if (march.target == MarchTargetType.ATTACK_BUILDING)
		{
			return true;
		}
		if (march.target == MarchTargetType.ATTACK_ARMY || march.target == MarchTargetType.ATTACK_MONSTER || march.target == MarchTargetType.RALLY_FOR_BOSS || march.target == MarchTargetType.EXPLORE || march.target == MarchTargetType.SAMPLE || march.target == MarchTargetType.RALLY_THRONE || march.target == MarchTargetType.RALLY_DRAGON_BUILDING || march.target == MarchTargetType.RALLY_EPIDEMIC_BUILDING || march.target == MarchTargetType.ATTACK_THRONE || march.target == MarchTargetType.ASSISTANCE_THRONE || march.target == MarchTargetType.RAINFOREST_THRONE_ATTACK || march.target == MarchTargetType.RAINFOREST_THRONE_ASSISTANCE || march.target == MarchTargetType.RAINFOREST_THRONE_RALLY || march.target == MarchTargetType.DETECT_TREASURE || march.target == MarchTargetType.PICK_GARBAGE || march.target == MarchTargetType.RALLY_FOR_BUILDING || march.target == MarchTargetType.RALLY_EPIDEMIC_CITY)
		{
			return true;
		}
		return false;
	}

	private int GetRealMarchTargetPos(WorldMarch march)
	{
		if (world == null)
		{
			return 0;
		}
		if (march.target == MarchTargetType.ATTACK_BUILDING || march.target == MarchTargetType.RALLY_FOR_BUILDING)
		{
			PointInfo pointInfoByUuid = world.GetPointInfoByUuid(march.targetUuid);
			if (pointInfoByUuid != null && pointInfoByUuid.pointType == WorldPointType.PlayerBuilding)
			{
				return pointInfoByUuid.mainIndex;
			}
		}
		else if (march.target == MarchTargetType.ATTACK_ARMY || march.target == MarchTargetType.ATTACK_MONSTER || march.target == MarchTargetType.RALLY_THRONE || march.target == MarchTargetType.RALLY_DRAGON_BUILDING || march.target == MarchTargetType.RALLY_EPIDEMIC_BUILDING || march.target == MarchTargetType.ATTACK_THRONE || march.target == MarchTargetType.ASSISTANCE_THRONE || march.target == MarchTargetType.RAINFOREST_THRONE_ATTACK || march.target == MarchTargetType.RAINFOREST_THRONE_ASSISTANCE || march.target == MarchTargetType.RAINFOREST_THRONE_RALLY || march.target == MarchTargetType.RALLY_FOR_BOSS)
		{
			WorldMarch march2 = GetMarch(march.targetUuid);
			if (march2 != null)
			{
				return world.WorldToTileIndex(march2.position);
			}
		}
		else if (march.target == MarchTargetType.EXPLORE || march.target == MarchTargetType.SAMPLE || march.target == MarchTargetType.PICK_GARBAGE)
		{
			PointInfo pointInfoByUuid2 = world.GetPointInfoByUuid(march.targetUuid);
			if (pointInfoByUuid2 != null)
			{
				return pointInfoByUuid2.pointIndex;
			}
		}
		else if (march.target == MarchTargetType.GOLLOES_EXPLORE)
		{
			return (int)march.targetUuid;
		}
		return 0;
	}

	private void UpdateMessage()
	{
		_messages.UpdateStep();
	}

	private void UpdateMove(float deltaTime)
	{
		StepUpdateMove(deltaTime);
	}

	private void TryStartStepUpdateMove()
	{
		if (_marchStepUpdateState == EMarchStepUpdateMoveState.Invalid)
		{
			_marchStepUpdateMoveId = Time.frameCount;
			_marchStepUpdateState = EMarchStepUpdateMoveState.UpdateVisible;
			_marchObjectCounter.ResetCurrent();
		}
		_marchStepUpdateTimer.Restart();
	}

	private void ResetStepUpdateMove()
	{
		_marchStepUpdateState = EMarchStepUpdateMoveState.Invalid;
		_marchStepUpdateTimer.Stop();
		_marchStepUpdateMoveId = -1;
	}

	public bool ConsumeLittleSmartDirty()
	{
		if (!littleSmartDirty)
		{
			return false;
		}
		littleSmartDirty = false;
		return true;
	}

	private void StepUpdateMove(float deltaTime)
	{
		if (!(world == null))
		{
			TryStartStepUpdateMove();
			if (StepUpdateMarchVisible(deltaTime) && StepUpdateMarchPerformance() && StepUpdateMarchTroop())
			{
				ResetStepUpdateMove();
			}
		}
	}

	private bool StepUpdateMarchVisible(float deltaTime)
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (_marchStepUpdateState != EMarchStepUpdateMoveState.UpdateVisible)
		{
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				WorldMarch value = allMarch.Value;
				if (value == null)
				{
					continue;
				}
				bool flag = false;
				if (EnableWorldMarchDataOptHahaha)
				{
					flag = marchDataUpdater.marchId2DataIndex.TryGetValue(value.uuid, out var value2);
					if (flag)
					{
						value.UpdateMove(ref marchDataUpdater.normalMarchArray[value2]);
					}
				}
				if (flag)
				{
					UpdateTroopLine(value);
					continue;
				}
				value.UpdateMove(deltaTime, serverTime);
				UpdateTroopLine(value);
			}
			_marchStepUpdateTimer.Restart();
			return true;
		}
		hahaUpdateCount = 0;
		legacyUpdateCount = 0;
		bool flag2 = true;
		int num = 4;
		foreach (KeyValuePair<long, WorldMarch> allMarch2 in allMarches)
		{
			WorldMarch value3 = allMarch2.Value;
			if (value3 == null)
			{
				continue;
			}
			bool flag3 = false;
			bool flag4 = false;
			if (EnableWorldMarchDataOptHahaha)
			{
				flag3 = marchDataUpdater.marchId2DataIndex.TryGetValue(value3.uuid, out var value4);
				if (flag3)
				{
					ref LittleNormalMarchData reference = ref marchDataUpdater.normalMarchArray[value4];
					value3.UpdateMove(ref reference);
					flag4 = reference.troopLineIsVisible;
				}
			}
			if (flag3)
			{
				hahaUpdateCount++;
				UpdateTroopLine(value3);
			}
			else
			{
				legacyUpdateCount++;
				value3.UpdateMove(deltaTime, serverTime);
				UpdateTroopLine(value3);
			}
			if (value3.updateVisibleID == _marchStepUpdateMoveId)
			{
				continue;
			}
			if (num <= 0 && _marchStepUpdateTimer.ElapsedMilliseconds >= 2)
			{
				flag2 = false;
				continue;
			}
			num--;
			value3.updateVisibleID = _marchStepUpdateMoveId;
			value3.UpdateViewRectState(IsMarchInView(value3));
			bool flag5 = value3.IsPerformanceMarch();
			bool isDisplay = value3.IsVisibleMarch() && value3.IsInViewRect && flag5;
			_marchObjectCounter.Accumulate(CounterType.Squad, flag5, isDisplay);
			if (!value3.NeedCreateTroopLine() || !value3.SupportLittleSmartTroopLineMode())
			{
				continue;
			}
			bool isDisplay2 = false;
			if (value3.IsVisibleMarch())
			{
				bool num2;
				if (!flag3)
				{
					if (IsLineInView(value3.homeWorldPos, value3.startWorldPos))
					{
						goto IL_0267;
					}
					num2 = IsLineInView(value3.startWorldPos, value3.targetWorldPos);
				}
				else
				{
					num2 = flag4;
				}
				if (num2)
				{
					goto IL_0267;
				}
			}
			goto IL_027f;
			IL_0267:
			if (value3.type != NewMarchType.ACT_BERSERK_BOSS || value3.status != MarchStatus.ATTACKING)
			{
				isDisplay2 = true;
			}
			goto IL_027f;
			IL_027f:
			_marchObjectCounter.Accumulate(CounterType.TroopLine, isData: true, isDisplay2);
		}
		if (flag2)
		{
			_marchStepUpdateState = EMarchStepUpdateMoveState.UpdatePerformance;
			return true;
		}
		return false;
	}

	private bool StepUpdateMarchPerformance()
	{
		if (_marchStepUpdateState == EMarchStepUpdateMoveState.UpdateTroopLine)
		{
			return true;
		}
		if (_marchStepUpdateState == EMarchStepUpdateMoveState.UpdatePerformance)
		{
			if (_marchObjectCounter.Commit())
			{
				littleSmartDirty = true;
			}
			_marchStepUpdateState = EMarchStepUpdateMoveState.UpdateTroopLine;
			return true;
		}
		throw new ArgumentException("StepUpdateMarchPerformance can not be UpdateVisible state.");
	}

	private bool StepUpdateMarchTroop()
	{
		bool flag = true;
		int num = 4;
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			WorldMarch value = allMarch.Value;
			if (value == null)
			{
				continue;
			}
			if (!IsMyMarch(value))
			{
				if (value.updateTroopID == _marchStepUpdateMoveId)
				{
					continue;
				}
				if (num <= 0 && _marchStepUpdateTimer.ElapsedMilliseconds >= 2)
				{
					flag = false;
					continue;
				}
			}
			bool flag2 = false;
			bool flag3 = false;
			if (EnableWorldMarchDataOptHahaha)
			{
				flag2 = marchDataUpdater.marchId2DataIndex.TryGetValue(value.uuid, out var value2);
				if (flag2)
				{
					flag3 = marchDataUpdater.normalMarchArray[value2].troopLineIsVisible;
				}
			}
			num--;
			value.updateTroopID = _marchStepUpdateMoveId;
			bool flag4 = value.IsVisibleMarch();
			if (value.IsInViewRect && flag4)
			{
				if ((value.type != NewMarchType.ZOMBIE_RUSH || (value.type == NewMarchType.ZOMBIE_RUSH && value.status == MarchStatus.MOVING)) && !world.IsTroopCreate(value.uuid))
				{
					world.CreateTroop(value);
				}
			}
			else if (world.IsTroopCreate(value.uuid))
			{
				world.DestroyTroop(value.uuid);
			}
			if (flag4)
			{
				bool flag5 = false;
				if ((!flag2) ? (IsLineInView(value.homeWorldPos, value.startWorldPos) || IsLineInView(value.startWorldPos, value.targetWorldPos)) : flag3)
				{
					if (value.type == NewMarchType.ACT_BERSERK_BOSS && value.status == MarchStatus.ATTACKING)
					{
						if (world.IsTroopLineCreate(value.uuid))
						{
							world.DestroyTroopLine(value.uuid);
						}
					}
					else if (!world.IsTroopLineCreate(value.uuid) || world.IsMarchOutOfData(value))
					{
						world.CreateTroopLine(value);
					}
				}
				else if (world.IsTroopLineCreate(value.uuid))
				{
					world.DestroyTroopLine(value.uuid);
				}
			}
			else if (world.IsTroopLineCreate(value.uuid))
			{
				world.DestroyTroopLine(value.uuid);
			}
		}
		if (flag)
		{
			_marchStepUpdateState = EMarchStepUpdateMoveState.Invalid;
			GameEntry.Event.Fire(EventId.WorldGetMarchInfosMsg);
		}
		return flag;
	}

	private void UpdateTroopLine(WorldMarch march)
	{
		Vector3 position = march.position;
		WorldTroop troop = world.GetTroop(march.uuid);
		if (troop != null)
		{
			position = troop.GetPosition();
		}
		world.UpdateTroopLineNew(march, position);
	}

	private void UpdateCollition(WorldMarch march)
	{
		if (world != null && march.status == MarchStatus.COLLECTING)
		{
			int pointIndex = world.WorldToTileIndex(march.position);
			if (world.GetObjectByPoint(pointIndex) is WorldResObject worldResObject)
			{
				worldResObject.UpdateMarch();
			}
		}
	}

	private void UpdateMarch(WorldMarch march)
	{
		AddOrUpdateMarch(march);
		if (world == null)
		{
			return;
		}
		world.RefreshNeedCreateTroop(march);
		if (world.IsTroopCreate(march.uuid))
		{
			if (march.IsMine() || (IsMarchInView(march) && march.IsVisibleMarch()))
			{
				world.UpdateTroop(march);
			}
			else
			{
				world.DestroyTroop(march.uuid);
			}
		}
	}

	private void AddToDelayDestroy(long uuid, float delaySecond)
	{
		if (delaySecond > 3f)
		{
			delaySecond = 3f;
		}
		if (!_delayDestroyTroop.ContainsKey(uuid))
		{
			_delayDestroyTroop.Add(uuid, GameEntry.Timer.GetServerTime() + (long)(delaySecond * 1000f));
		}
	}

	private void DestroyMarch(long marchUuid, bool isBattleFail)
	{
		if (world != null)
		{
			float num = world.DestroyTroop(marchUuid, isBattleFail);
			if (num > 0f)
			{
				AddToDelayDestroy(marchUuid, num);
				WorldMarch march = GetMarch(marchUuid);
				AddMarchOptLog(march, 23);
				RemoveMarch(marchUuid);
			}
			else
			{
				world.DestroyTroopLine(marchUuid);
				WorldMarch march2 = GetMarch(marchUuid);
				AddMarchOptLog(march2, 33);
				RemoveMarch(marchUuid);
				GameEntry.Event.Fire(EventId.HideTroopName, marchUuid);
			}
		}
		else
		{
			WorldMarch march3 = GetMarch(marchUuid);
			AddMarchOptLog(march3, 43);
			RemoveMarch(marchUuid);
		}
	}

	private bool IsVisibleToLocalPlayer(WorldMarch march)
	{
		if (march.IsMonsterOrOrdinaryBoss())
		{
			if (!string.IsNullOrEmpty(march.belongUid) && march.belongUid == GameEntry.Data.Player.Uid)
			{
				return true;
			}
			if (world != null && !world.IsInSelfLandBlock(march.targetPos))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	private void RemoveMarch(long marchUuid)
	{
		WorldMarch march = GetMarch(marchUuid);
		try
		{
			if (march == null)
			{
				return;
			}
			CheckMyMarchDirty(march);
			CheckToMeMarchDirty(march);
			toMeMarchUuids.Remove(march.uuid);
			myMarchUuids.Remove(march.uuid);
		}
		catch (Exception arg)
		{
			Log.Error($"Exception when RemoveMarch 1 {arg}");
		}
		try
		{
			if (march.ownerUid == GameEntry.Data.Player.Uid)
			{
				team2MarchUuid.Remove(march.teamUuid);
				GameEntry.Event.Fire(EventId.WorldMarchDelete, marchUuid);
				myMarchMultiKillPVE.Remove(march.uuid);
				myMarchMultiKillPVP.Remove(march.uuid);
			}
		}
		catch (Exception arg2)
		{
			Log.Error($"Exception when RemoveMarch 2 {arg2}");
		}
		try
		{
			if (march.IsMonsterOrOrdinaryBoss())
			{
				OnMonsterDelete?.Invoke(march.uuid, march.monsterId, march.serverId, march.startPos);
			}
			if (world != null && march.IsMonsterOrOrdinaryBoss())
			{
				world.RemoveOccupyPoints(world.IndexToTilePos(march.targetPos), Vector2Int.one, march.targetServer);
			}
			BIRecordWorldMarch(march);
			AddMarchOptLog(march, 3);
			allMarches.Remove(march.uuid);
			marchDataUpdater?.TryRemoveMarch(marchUuid);
		}
		catch (Exception arg3)
		{
			Log.Error($"Exception when RemoveMarch 3 {arg3}");
		}
		if (isRecordingMarchBlock)
		{
			march.RecordMarchBlock(tileBlockIndex);
		}
		try
		{
			ownerMarches.Remove(march.uuid);
			if (IsMyMarch(march))
			{
				TryUpdateMyAssistanceMarch(march, isRemove: true);
			}
			if (!string.IsNullOrEmpty(march.allianceUid) && allianceMarches.TryGetValue(march.allianceUid, out var value))
			{
				value.RemoveAll((WorldMarch i) => i.uuid == march.uuid);
			}
			_marchBattleSound.RemoveMarch(march.uuid);
		}
		catch (Exception arg4)
		{
			Log.Error($"Exception when RemoveMarch 4 {arg4}");
		}
	}

	private void CheckMyMarchDirty(WorldMarch march)
	{
		if (!myMarchDirty && (IsMyMarch(march) || IsMyJoinAssemblyMarch(march)))
		{
			myMarchDirty = true;
		}
	}

	private void CheckToMeMarchDirty(WorldMarch march)
	{
		if (!toMeMarchDirty && (IsTargetForMine(march) || IsTargetForMine(march, onlyAttack: false, isDesertBattle: true)))
		{
			toMeMarchDirty = true;
			toMeMarchNeedUpdate = true;
			toMeMarchDesertNeedUpdate = true;
		}
	}

	public static bool IsMyMarch(WorldMarch march)
	{
		if (march.ownerUid == GameEntry.Data.Player.Uid && march.type != NewMarchType.DEFAULT)
		{
			return march.type != NewMarchType.TRAIN;
		}
		return false;
	}

	private bool IsMyAllyRallyMarch(WorldMarch march)
	{
		if (march.type != NewMarchType.ASSEMBLY_MARCH)
		{
			return false;
		}
		if (string.IsNullOrEmpty(march.allianceUid))
		{
			return false;
		}
		int num = 3;
		if (march.worldType == num)
		{
			return !GameEntry.Lua.CallWithReturn<bool, string, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", march.allianceUid, num);
		}
		return march.allianceUid == GameEntry.Data.Player.GetAllianceId();
	}

	private bool IsMyJoinAssemblyMarch(WorldMarch march)
	{
		if (!IsMyAllyRallyMarch(march))
		{
			return false;
		}
		if (team2MarchUuid.ContainsKey(march.teamUuid))
		{
			return true;
		}
		return false;
	}

	private void TryUpdateMyAssistanceMarch(WorldMarch march, bool isRemove)
	{
		if (!EnableWorldAssistanceOpt)
		{
			return;
		}
		if ((march.target == MarchTargetType.ASSISTANCE_BUILD || march.target == MarchTargetType.ASSISTANCE_CITY || march.target == MarchTargetType.ASSISTANCE_OUTPOST_BUILDING || march.target == MarchTargetType.ASSISTANCE_ALLIANCE_CITY || march.target == MarchTargetType.ASSISTANCE_CITY_TRADE || march.target == MarchTargetType.ASSISTANCE_CITY_ALTAR || march.target == MarchTargetType.ASSISTANCE_ALLIANCE_BUILDING || march.target == MarchTargetType.ASSISTANCE_CITY_STRONGHOLD || march.target == MarchTargetType.ASSISTANCE_WINTER_STORM_CITY || march.target == MarchTargetType.ASSISTANCE_THRONE || march.target == MarchTargetType.RAINFOREST_THRONE_ASSISTANCE || march.target == MarchTargetType.ASSISTANCE_WINTER_ENTITY || march.target == MarchTargetType.ASSISTANCE_EPIDEMIC_CITY || march.target == MarchTargetType.ASSISTANCE_EPIDEMIC_BUILDING || march.target == MarchTargetType.ASSISTANCE_DRAGON_BUILDING || march.target == MarchTargetType.ASSISTANCE_SERVER_THRONE_BUILDING || march.target == MarchTargetType.ASSISTANCE_ZWL_BUILDING) && !isRemove)
		{
			if (myAssitanceUuid2Points.TryGetValue(march.uuid, out var value))
			{
				if (march.targetPos != value)
				{
					myAssitanceUuid2Points[march.uuid] = march.targetPos;
					if (myAssistancePoint2Uuids.TryGetValue(value, out var value2))
					{
						value2.Remove(march.uuid);
						if (value2.Count <= 0)
						{
							myAssistancePoint2Uuids.Remove(value);
						}
					}
					if (!myAssistancePoint2Uuids.TryGetValue(march.targetPos, out value2))
					{
						value2 = new List<long>();
						value2.Add(march.uuid);
						myAssistancePoint2Uuids.Add(march.targetPos, value2);
					}
					else if (!value2.Contains(march.uuid))
					{
						value2.Add(march.uuid);
					}
					PointInfo pointInfo = world?.GetPointInfo(march.targetPos);
					if (pointInfo != null)
					{
						world?.MarkPointIsDirty(pointInfo.uuid);
					}
				}
				else
				{
					PointInfo pointInfo2 = world?.GetPointInfo(march.targetPos);
					if (pointInfo2 != null)
					{
						world?.MarkPointIsDirty(pointInfo2.uuid);
					}
				}
			}
			else
			{
				myAssitanceUuid2Points[march.uuid] = march.targetPos;
				if (!myAssistancePoint2Uuids.TryGetValue(march.targetPos, out var value3))
				{
					value3 = new List<long>();
					value3.Add(march.uuid);
					myAssistancePoint2Uuids.Add(march.targetPos, value3);
				}
				else if (!value3.Contains(march.uuid))
				{
					value3.Add(march.uuid);
				}
				PointInfo pointInfo3 = world?.GetPointInfo(march.targetPos);
				if (pointInfo3 != null)
				{
					world?.MarkPointIsDirty(pointInfo3.uuid);
				}
			}
		}
		else
		{
			if (!myAssitanceUuid2Points.TryGetValue(march.uuid, out var value4))
			{
				return;
			}
			myAssitanceUuid2Points.Remove(march.uuid);
			if (myAssistancePoint2Uuids.TryGetValue(value4, out var value5))
			{
				value5.Remove(march.uuid);
				if (value5.Count <= 0)
				{
					myAssistancePoint2Uuids.Remove(value4);
				}
			}
			PointInfo pointInfo4 = world?.GetPointInfo(value4);
			if (pointInfo4 != null)
			{
				world?.MarkPointIsDirty(pointInfo4.uuid);
			}
		}
	}

	public int GetMyAssistanceCountByPointIndex(int pointIndex)
	{
		if (myAssistancePoint2Uuids.TryGetValue(pointIndex, out var value))
		{
			return value.Count;
		}
		return 0;
	}

	public int GetMyAssistanceFirstHero(int pointIndex)
	{
		long firstMyAssistanceMarchUuid = GetFirstMyAssistanceMarchUuid(pointIndex);
		if (firstMyAssistanceMarchUuid == 0L)
		{
			return 0;
		}
		WorldMarch march = GetMarch(firstMyAssistanceMarchUuid);
		if (march == null)
		{
			return 0;
		}
		if (GameEntry.Timer.GetServerTime() >= march.endTime)
		{
			return march.GetLeaderHero()?.heroId ?? 0;
		}
		return 0;
	}

	public long GetFirstMyAssistanceMarchUuid(int pointIndex)
	{
		if (myAssistancePoint2Uuids.TryGetValue(pointIndex, out var value) && value.Count > 0)
		{
			return value[0];
		}
		return 0L;
	}

	private void AddOrUpdateMarch(WorldMarch march)
	{
		AddMarch(march);
		if (world != null && march.IsMonsterOrOrdinaryBoss())
		{
			OnMonsterAdd?.Invoke(march.uuid, march.monsterId, march.serverId, march.startPos);
			world.AddOccupyPoints(world.IndexToTilePos(march.targetPos), Vector2Int.one, march.targetServer);
		}
		if (IsMyMarch(march))
		{
			myMarchDirty = true;
			myMarchUuids.Add(march.uuid);
			ownerMarches[march.uuid] = march;
			if (march.teamUuid > 0)
			{
				team2MarchUuid[march.teamUuid] = march.uuid;
			}
			if (march.target == MarchTargetType.BACK_HOME && member2LeaderUuid.TryGetValue(march.uuid, out var value))
			{
				myMarchUuids.Remove(value);
				ownerMarches.Remove(value);
				member2LeaderUuid.Remove(march.uuid);
			}
			TryUpdateMyAssistanceMarch(march, isRemove: false);
		}
		else if (IsMyJoinAssemblyMarch(march))
		{
			myMarchDirty = true;
			myMarchUuids.Add(march.uuid);
			ownerMarches[march.uuid] = march;
			if (team2MarchUuid.TryGetValue(march.teamUuid, out var value2))
			{
				member2LeaderUuid[value2] = march.uuid;
			}
		}
		else
		{
			ownerMarches.Remove(march.uuid);
			if (IsTargetForMine(march) || IsTargetForMine(march, onlyAttack: false, isDesertBattle: true))
			{
				toMeMarchDirty = true;
				toMeMarchNeedUpdate = true;
				toMeMarchDesertNeedUpdate = true;
				toMeMarchUuids.Add(march.uuid);
			}
		}
		List<WorldMarch> value4;
		if (march.type == NewMarchType.ASSEMBLY_MARCH)
		{
			if (!string.IsNullOrEmpty(march.allianceUid))
			{
				if (!allianceMarches.TryGetValue(march.allianceUid, out var value3))
				{
					value3 = new List<WorldMarch>();
					allianceMarches.Add(march.allianceUid, value3);
				}
				int num = value3.FindIndex((WorldMarch i) => i.uuid == march.uuid);
				if (num != -1)
				{
					value3[num] = march;
				}
				else
				{
					value3.Add(march);
				}
			}
		}
		else if (!string.IsNullOrEmpty(march.allianceUid) && allianceMarches.TryGetValue(march.allianceUid, out value4))
		{
			value4.RemoveAll((WorldMarch i) => i.uuid == march.uuid);
		}
		if (march.status == MarchStatus.COLLECTING)
		{
			UpdateCollition(march);
		}
		if (march.type == NewMarchType.TRAIN)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.AddOrUpdateTrain", ((SFSObject)march.train.trainData).ToLuaTable(GameEntry.Lua.Env));
		}
		if (!(world != null))
		{
			return;
		}
		if (march.type == NewMarchType.ZOMBIE_RETREAT)
		{
			if (!fakeRetreatMarches.ContainsKey(march.uuid))
			{
				fakeRetreatMarches[march.uuid] = new List<WorldMarch>();
			}
			int num2 = Mathf.Min(march.npcNum - 1, 5);
			if (fakeRetreatMarches[march.uuid].Count < num2)
			{
				int timeDelta = Mathf.Max((int)(GameEntry.Timer.GetServerTime() - march.startTime), 0);
				PointInfo pointInfo = world.GetPointInfo(march.targetPos);
				for (int num3 = fakeRetreatMarches[march.uuid].Count; num3 < num2; num3++)
				{
					AddFakeRetreatMarchData(march, pointInfo, timeDelta);
				}
			}
		}
		else if (march.type != NewMarchType.ZONE_TRAIN)
		{
			if (march.type == NewMarchType.FLOWER_TRAIN)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateFlowerTrainData", march);
			}
			else if (march.type == NewMarchType.BOSS && march.allianceBoss != null)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshAllyDrillBase", march);
			}
			else if (march.type == NewMarchType.BOSS && march.invasionBossInfo != null)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshAisilaCtrl", march);
			}
			else if (march.type == NewMarchType.ZONE_MOBILIZATION_BOSS)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshZMBossActionCtrl", march);
			}
			else if (march.IsAlChallengeKirov())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshKillZombieKirovActionCtrl", march);
			}
			else if (march.type == NewMarchType.BOSS && march.s0AllianceBossInfo != null)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshS0AllianceBossActionCtrl", march);
			}
		}
		MarchTargetType target = march.target;
		if (target == MarchTargetType.ATTACK_ALLIANCE_CITY || target == MarchTargetType.ATTACK_SERVER_THRONE_BUILDING || target == MarchTargetType.ATTACK_THRONE || target == MarchTargetType.RALLY_THRONE || target == MarchTargetType.ASSISTANCE_THRONE || target == MarchTargetType.RAINFOREST_THRONE_ATTACK || target == MarchTargetType.RAINFOREST_THRONE_ASSISTANCE || target == MarchTargetType.RAINFOREST_THRONE_RALLY || target == MarchTargetType.ASSISTANCE_SERVER_THRONE_BUILDING || target == MarchTargetType.RALLY_SERVER_THRONE_BUILDING || target == MarchTargetType.RALLY_FOR_ALLIANCE_CITY)
		{
			_marchBattleSound.AddMarch(march.uuid, march.targetPos);
		}
	}

	private WorldTroopPathSegment[] CreatePathSegment(WorldMarch march)
	{
		if (march.status == MarchStatus.MOVING || march.status == MarchStatus.BACK_HOME || march.status == MarchStatus.CHASING || march.status == MarchStatus.IN_WORM_HOLE || march.status == MarchStatus.TRAIN_PULL_IN || march.status == MarchStatus.ZOMBIE_RUSH_WAITING || march.status == MarchStatus.BERSERK_BOSS_WAITING)
		{
			return march.CreatePathSegment();
		}
		if (march.status == MarchStatus.STATION && march.type == NewMarchType.RUNNING_MUMMY)
		{
			return march.CreatePathSegment();
		}
		return null;
	}

	public IEnumerator UIMainWarningHide(WarningType warningType)
	{
		yield return new WaitForSeconds(5f);
		GameEntry.Event.Fire(EventId.UIMainWarningHide, (int)warningType);
	}

	public Dictionary<long, WorldMarch> GetAllMarchesByCS()
	{
		return allMarches;
	}

	public WorldMarch GetMarchesByStartIndex(int posIndex)
	{
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.startPos == posIndex)
			{
				return value;
			}
		}
		return null;
	}

	public WorldMarch GetMarchByTargetPos(int targetPos)
	{
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.targetPos == targetPos)
			{
				return value;
			}
		}
		return null;
	}

	public WorldMarch GetMarchByType(int marchType)
	{
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.type == (NewMarchType)marchType)
			{
				return value;
			}
		}
		return null;
	}

	public Dictionary<string, int> GetMarchCountToTargetGroupByOwnerUid(int targetPos)
	{
		Dictionary<string, int> dictionary = null;
		foreach (WorldMarch value in allMarches.Values)
		{
			if (value.targetPos != targetPos)
			{
				continue;
			}
			string ownerUid = value.ownerUid;
			if (!string.IsNullOrEmpty(ownerUid))
			{
				dictionary = dictionary ?? new Dictionary<string, int>();
				if (dictionary.ContainsKey(ownerUid))
				{
					dictionary[ownerUid]++;
				}
				else
				{
					dictionary.Add(ownerUid, 1);
				}
			}
		}
		return dictionary;
	}

	public void GetMonsterListInArea(Vector2Int center, int size, Dictionary<int, int> monsterIds, Dictionary<long, Vector2Int> result)
	{
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			if (allMarch.Value.IsMonster() && allMarch.Value.startPos == allMarch.Value.targetPos && monsterIds.ContainsKey(allMarch.Value.monsterId))
			{
				Vector2Int value = TileCoord.IndexToTilePos(allMarch.Value.startPos, ForceChangeScene.World);
				if (value.x >= center.x - size && value.x <= center.x + size && value.y >= center.y - size && value.y <= center.y + size)
				{
					result[allMarch.Value.uuid] = value;
				}
			}
		}
	}

	public Dictionary<long, WorldMarch> GetMarchesBossInfo()
	{
		Dictionary<long, WorldMarch> dictionary = new Dictionary<long, WorldMarch>();
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			if (allMarch.Value.IsOrdinaryBoss())
			{
				dictionary.Add(allMarch.Key, allMarch.Value);
			}
		}
		if (dictionary != null)
		{
			return dictionary;
		}
		return null;
	}

	public Dictionary<long, WorldMarch> GetMarchesTargetForMine(bool onlyDesertBattle)
	{
		return GetMarchesTargetForMineCached(onlyDesertBattle);
	}

	public Dictionary<long, WorldMarch> GetMarchesTargetForMineLite()
	{
		return GetMarchesTargetForMineCached(onlyDesertBattle: false);
	}

	public Dictionary<long, WorldMarch> GetMarchesTargetForMineCached(bool onlyDesertBattle)
	{
		Dictionary<long, WorldMarch> dictionary = (onlyDesertBattle ? targetForMineMarchDesertDic : targetForMineMarchDic);
		if (onlyDesertBattle ? toMeMarchDesertNeedUpdate : toMeMarchNeedUpdate)
		{
			dictionary.Clear();
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				if (IsTargetForMine(allMarch.Value, onlyAttack: false, onlyDesertBattle))
				{
					dictionary.Add(allMarch.Key, allMarch.Value);
				}
			}
			if (onlyDesertBattle)
			{
				toMeMarchDesertNeedUpdate = false;
			}
			else
			{
				toMeMarchNeedUpdate = false;
			}
		}
		return dictionary;
	}

	public Dictionary<long, WorldMarch> GetInimicalMarchesTargetForMine()
	{
		Dictionary<long, WorldMarch> dictionary = new Dictionary<long, WorldMarch>();
		int worldMainPos = GameEntry.Data.Building.GetWorldMainPos();
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			if (IsTargetForMine(allMarch.Value, onlyAttack: true) && (allMarch.Value.targetPos == 0 || allMarch.Value.targetPos == worldMainPos))
			{
				dictionary.Add(allMarch.Key, allMarch.Value);
			}
		}
		return dictionary;
	}

	public bool IsTargetForMine(WorldMarch march, bool onlyAttack = false, bool isDesertBattle = false)
	{
		if (isDesertBattle && march.worldId <= 0)
		{
			return false;
		}
		switch (march.target)
		{
		case MarchTargetType.ATTACK_BUILDING:
		case MarchTargetType.RALLY_FOR_BUILDING:
		case MarchTargetType.ATTACK_CITY:
		case MarchTargetType.RALLY_FOR_CITY:
		case MarchTargetType.SCOUT_CITY:
		case MarchTargetType.SCOUT_BUILDING:
		case MarchTargetType.SCOUT_WINTER_STORM_CITY:
		case MarchTargetType.ATTACK_WINTER_STORM_CITY:
		case MarchTargetType.FAKE_ATTACK:
		case MarchTargetType.ATTACK_EPIDEMIC_CITY:
		case MarchTargetType.SCOUT_EPIDEMIC_CITY:
		case MarchTargetType.RALLY_EPIDEMIC_CITY:
		case MarchTargetType.SCOUT_OUTPOST_BUILDING:
			if (isDesertBattle)
			{
				return march.targetPos == GameEntry.Data.Building.GetDragonWorldPos();
			}
			return GameEntry.Data.Building.CheckIsMyBuilding(march.targetUuid);
		case MarchTargetType.SCOUT_TREAT:
		case MarchTargetType.ZOMBIE_BOSS_ATTACK_CITY:
		case MarchTargetType.DIG_ICE_ENEMY:
		case MarchTargetType.SEASON_FARMER_SEND_RES:
		case MarchTargetType.LOTTO_RECEIVE_BASE_REWARD:
		case MarchTargetType.SCOUT_ZONE_MOBILIZATION_DONATE:
		case MarchTargetType.RUNNING_MUMMY:
		case MarchTargetType.VALENTINE_RECEIVE_BASE_REWARD:
		case MarchTargetType.DARKNESS_MONSTER_ATTACK_CITY:
		case MarchTargetType.ALLIANCE_BOSS_SAND_ATTACK_CITY:
		case MarchTargetType.POWER_WORKER_BACK:
		case MarchTargetType.POWER_WORK_HELPER_CHARGE:
		case MarchTargetType.CHARGE_SUPPLIES:
		case MarchTargetType.ALLIANCE_MONSTER_CHALLENGE_NEW_DONATE:
			return GameEntry.Data.Building.CheckIsMyBuilding(march.targetUuid);
		case MarchTargetType.ATTACK_ARMY:
		case MarchTargetType.ATTACK_ARMY_COLLECT:
		case MarchTargetType.SCOUT_ARMY_COLLECT:
		case MarchTargetType.SCOUT_TROOP:
		case MarchTargetType.ATTACK_METEORITE:
		case MarchTargetType.SCOUT_METEORITE:
			return IsSelfInCurrentMarchTeam(march.targetUuid);
		case MarchTargetType.RUNNING_BOSS_ATTACK_CITY:
			return true;
		case MarchTargetType.ATTACK_ROAD:
			return GameEntry.Lua.CallWithReturn<LuaTable, long>("CSharpCallLuaInterface.GetBoardData", march.targetUuid) != null;
		case MarchTargetType.ASSISTANCE_BUILD:
		case MarchTargetType.ASSISTANCE_CITY:
		case MarchTargetType.DIG_ICE_ALLY:
		case MarchTargetType.ASSISTANCE_WINTER_STORM_CITY:
		case MarchTargetType.ASSISTANCE_EPIDEMIC_CITY:
			if (!onlyAttack)
			{
				return GameEntry.Data.Building.CheckIsMyBuilding(march.targetUuid);
			}
			return false;
		default:
			return false;
		}
	}

	public bool IsTargetForAlly(WorldMarch march)
	{
		switch (march.target)
		{
		case MarchTargetType.BACK_HOME:
			if (march.allianceUid == GameEntry.Data.Player.GetAllianceId() && march.ownerUid != GameEntry.Data.Player.Uid)
			{
				return true;
			}
			break;
		case MarchTargetType.ATTACK_BUILDING:
		case MarchTargetType.RALLY_FOR_BUILDING:
		case MarchTargetType.ATTACK_CITY:
		case MarchTargetType.ASSISTANCE_BUILD:
		case MarchTargetType.ASSISTANCE_CITY:
		case MarchTargetType.SCOUT_CITY:
		case MarchTargetType.SCOUT_BUILDING:
		case MarchTargetType.RALLY_THRONE:
		case MarchTargetType.SCOUT_THRONE:
		case MarchTargetType.ATTACK_THRONE:
		case MarchTargetType.ASSISTANCE_THRONE:
		case MarchTargetType.RALLY_DRAGON_BUILDING:
		case MarchTargetType.SCOUT_TREAT:
		case MarchTargetType.ZOMBIE_BOSS_ATTACK_CITY:
		case MarchTargetType.ASSISTANCE_WINTER_STORM_CITY:
		case MarchTargetType.SCOUT_WINTER_STORM_CITY:
		case MarchTargetType.ATTACK_WINTER_STORM_CITY:
		case MarchTargetType.SEASON_FARMER_SEND_RES:
		case MarchTargetType.LOTTO_RECEIVE_BASE_REWARD:
		case MarchTargetType.SCOUT_ZONE_MOBILIZATION_DONATE:
		case MarchTargetType.VALENTINE_RECEIVE_BASE_REWARD:
		case MarchTargetType.RALLY_EPIDEMIC_BUILDING:
		case MarchTargetType.ATTACK_EPIDEMIC_CITY:
		case MarchTargetType.ASSISTANCE_EPIDEMIC_CITY:
		case MarchTargetType.SCOUT_EPIDEMIC_CITY:
		case MarchTargetType.POWER_WORKER_BACK:
		case MarchTargetType.POWER_WORK_HELPER_CHARGE:
		case MarchTargetType.CHARGE_SUPPLIES:
		case MarchTargetType.ALLIANCE_MONSTER_CHALLENGE_NEW_DONATE:
		case MarchTargetType.SCOUT_OUTPOST_BUILDING:
		case MarchTargetType.RAINFOREST_THRONE_ATTACK:
		case MarchTargetType.RAINFOREST_THRONE_ASSISTANCE:
		case MarchTargetType.RAINFOREST_THRONE_SCOUT:
		case MarchTargetType.RAINFOREST_THRONE_RALLY:
			if (SceneManager.World != null && SceneManager.World.GetPointInfoByUuid(march.targetUuid) is BuildPointInfo buildPointInfo && buildPointInfo.allianceId == GameEntry.Data.Player.GetAllianceId())
			{
				return true;
			}
			break;
		case MarchTargetType.ATTACK_ARMY:
		case MarchTargetType.ATTACK_ARMY_COLLECT:
		case MarchTargetType.SCOUT_ARMY_COLLECT:
		case MarchTargetType.SCOUT_TROOP:
		{
			WorldMarch march2 = GetMarch(march.targetUuid);
			if (march2 != null && march2.allianceUid == GameEntry.Data.Player.GetAllianceId())
			{
				return true;
			}
			break;
		}
		case MarchTargetType.ATTACK_DESERT:
		case MarchTargetType.SCOUT_DESERT:
		{
			if (!(world != null))
			{
				break;
			}
			WorldTileInfo worldTileInfo = world.GetWorldTileInfo(march.targetUuid.ToInt());
			if (worldTileInfo != null)
			{
				WorldDesertInfo desertInfoByUuid = worldTileInfo.GetWorldDesertInfo();
				if (desertInfoByUuid != null && desertInfoByUuid.GetPlayerType() == PlayerType.PlayerSelf)
				{
					return true;
				}
			}
			break;
		}
		case MarchTargetType.ASSISTANCE_DESERT:
			if (world != null)
			{
				WorldDesertInfo desertInfoByUuid = world.GetDesertInfoByUuid(march.targetUuid);
				if (desertInfoByUuid != null && desertInfoByUuid.GetPlayerType() == PlayerType.PlayerSelf)
				{
					return true;
				}
			}
			break;
		}
		return false;
	}

	private void InitTrainConfig()
	{
		float.TryParse(GameEntry.ConfigCache.GetTemplateData("train_para", 10, "val"), out WorldTrain.Carriage_Length);
		WorldTrain.Carriage_Length *= 2f;
		trainConfigs.Clear();
		GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllTrainConfig")?.ForEach(delegate(int id, LuaTable data)
		{
			if (data.ContainsKey("quality") && data.ContainsKey("length"))
			{
				int quality = data.Get<int>("quality");
				int carriageNum = data.Get<int>("length");
				trainConfigs[id] = new WorldTrainConfig(id, quality, carriageNum);
			}
		});
	}

	private void InitFlowerCarConfig()
	{
		flowerCarLength.Clear();
		GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetFlowerCarLength")?.ForEach(delegate(int id, int data)
		{
			flowerCarLength[id] = data;
		});
	}

	public WorldTrainConfig GetTrainConfig(int id)
	{
		if (trainConfigs.Count == 0)
		{
			InitTrainConfig();
		}
		if (trainConfigs.TryGetValue(id, out var value))
		{
			return value;
		}
		Log.Error("火车配置找不到，cfgId = " + id);
		return new WorldTrainConfig(0, 1, 1);
	}

	public int GetFlowerCarLength(int id)
	{
		if (flowerCarLength.Count == 0)
		{
			InitFlowerCarConfig();
		}
		if (flowerCarLength.TryGetValue(id, out var value))
		{
			return value;
		}
		Log.Error("花车长度配置找不到，cfgId = " + id);
		return 77;
	}

	public void CacheAllianceMembersHomePos()
	{
		allianceMembersHomePos.Clear();
		GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllianceMembersHomePos")?.ForEach(delegate(int pointId, string uid)
		{
			allianceMembersHomePos.Add(pointId);
		});
	}

	public void CleanAllianceMembersHomePos()
	{
		allianceMembersHomePos.Clear();
	}

	public bool IsMemberByPointId(int pointId)
	{
		return allianceMembersHomePos.Contains(pointId);
	}

	public void DestroyBerserkBossMarchData(long marchUuid)
	{
		DestroyMarch(marchUuid, isBattleFail: true);
	}

	public string SaveCreateMarchRecordTime()
	{
		string text = Guid.NewGuid().ToString();
		long serverTime = GameEntry.Timer.GetServerTime();
		_cacheClientCreateGuidAndTimeDict[text] = serverTime;
		return text;
	}

	public void RemoveCreateMarchRecordTime(WorldMarch worldMarch)
	{
		if (!string.IsNullOrEmpty(worldMarch.clientCreateGuid) && _cacheClientCreateGuidAndTimeDict.ContainsKey(worldMarch.clientCreateGuid))
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			long num = _cacheClientCreateGuidAndTimeDict[worldMarch.clientCreateGuid];
			long num2 = serverTime - num;
			_cacheClientCreateGuidAndTimeDict.Remove(worldMarch.clientCreateGuid);
			PostEventLog.TrackMap("CreateMarchDeltaTime", new Dictionary<string, object>
			{
				{ "uuid", worldMarch.uuid },
				{ "timeDiff", num2 }
			});
		}
	}

	private int GetMyMarchMultiKillPVE(long marchUuid)
	{
		if (!myMarchMultiKillPVE.TryGetValue(marchUuid, out var value))
		{
			return 0;
		}
		return value;
	}

	private int GetMyMarchMultiKillPVP(long marchUuid)
	{
		if (!myMarchMultiKillPVP.TryGetValue(marchUuid, out var value))
		{
			return 0;
		}
		return value;
	}

	public void UpdateBattleSoundData()
	{
		_marchBattleSound.Init();
	}

	public void OnDrawGizmos()
	{
		marchDataUpdater?.OnDrawGizmos();
		Gizmos.color = Color.magenta;
		Vector3 center = new Vector3(cameraViewRect.center.x, 0f, cameraViewRect.center.y);
		Vector3 size = new Vector3(cameraViewRect.size.x, 0f, cameraViewRect.size.y);
		Gizmos.DrawWireCube(center, size);
	}

	[Conditional("UNITY_EDITOR")]
	public static void EditorLog(string log)
	{
	}

	public string EditorDescription()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (marchDataUpdater != null)
		{
			stringBuilder.AppendLine("Little march data updater:");
			stringBuilder.AppendLine(marchDataUpdater.Description());
		}
		stringBuilder.AppendLine("---World March Data Manager---");
		stringBuilder.AppendLine($"当前所有WorldMarch数量:{allMarches.Count}");
		stringBuilder.AppendLine($"性能相关行军数量:{lastPerformanceCount}");
		int num = 0;
		int num2 = 0;
		if (allMarches.Count > 0)
		{
			Dictionary<NewMarchType, int> dictionary = new Dictionary<NewMarchType, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
			{
				WorldMarch value = allMarch.Value;
				if (value.IsVisibleMarch())
				{
					num++;
					if (value.IsInViewRect)
					{
						num2++;
					}
				}
				NewMarchType type = allMarch.Value.type;
				if (dictionary.TryGetValue(type, out var value2))
				{
					dictionary[type] = value2 + 1;
				}
				else
				{
					dictionary[type] = 1;
				}
				int worldId = allMarch.Value.worldId;
				if (dictionary2.TryGetValue(worldId, out var value3))
				{
					dictionary2[worldId] = value3 + 1;
				}
				else
				{
					dictionary2[worldId] = 1;
				}
			}
			stringBuilder.AppendLine("按照类型分组统计:");
			foreach (KeyValuePair<NewMarchType, int> item in dictionary)
			{
				stringBuilder.AppendLine($"{item.Key.ToString()}:{item.Value}");
			}
			stringBuilder.AppendLine("按照世界id分组统计:");
			foreach (KeyValuePair<int, int> item2 in dictionary2)
			{
				stringBuilder.AppendLine($"{item2.Key.ToString()}:{item2.Value}");
			}
		}
		stringBuilder.AppendLine($"可见行军:{num}");
		stringBuilder.AppendLine($"视野内行军:{num2}");
		stringBuilder.AppendLine($"目标是我的行军:{toMeMarchUuids.Count}");
		stringBuilder.AppendLine($"目标是我的行军的目标...:{toMeMarchTargetUuids.Count}");
		stringBuilder.AppendLine($"是否在记录地格占用:{isRecordingMarchBlock}");
		if (isRecordingMarchBlock)
		{
			stringBuilder.AppendLine($"已记录数量:{tileBlockIndex.Count}");
		}
		stringBuilder.AppendLine($"我驻防的行军数量:{myAssitanceUuid2Points.Count}");
		if (myAssitanceUuid2Points.Count > 0)
		{
			foreach (KeyValuePair<long, int> myAssitanceUuid2Point in myAssitanceUuid2Points)
			{
				stringBuilder.AppendLine($"{myAssitanceUuid2Point.Key}:{world.IndexToTilePos(myAssitanceUuid2Point.Value)}");
			}
		}
		stringBuilder.AppendLine($"我驻防的目标数量:{myAssistancePoint2Uuids.Count}");
		if (myAssistancePoint2Uuids.Count > 0)
		{
			foreach (KeyValuePair<int, List<long>> myAssistancePoint2Uuid in myAssistancePoint2Uuids)
			{
				stringBuilder.AppendLine($"{world.IndexToTilePos(myAssistancePoint2Uuid.Key)}:{myAssistancePoint2Uuid.Value?.Count}");
				if (myAssistancePoint2Uuid.Value.Count <= 0)
				{
					continue;
				}
				foreach (long item3 in myAssistancePoint2Uuid.Value)
				{
					stringBuilder.AppendLine($"\t{item3}");
				}
			}
		}
		return stringBuilder.ToString();
	}

	public void AddMarch(WorldMarch march)
	{
		AddMarchOptLog(march, 1);
		allMarches[march.uuid] = march;
		march.InitMove(CreatePathSegment(march));
		if (isRecordingMarchBlock && recordingWorldId == march.worldId)
		{
			march?.RecordMarchBlock(tileBlockIndex);
		}
	}

	public void FakeAddMarch(WorldMarch march)
	{
		AddMarch(march);
	}

	public void StartRecordMarchBlock(int worldId)
	{
		if (isRecordingMarchBlock)
		{
			return;
		}
		isRecordingMarchBlock = true;
		tileBlockIndex.Clear();
		recordingWorldId = worldId;
		int num = 0;
		int num2 = -1;
		foreach (KeyValuePair<long, WorldMarch> allMarch in allMarches)
		{
			WorldMarch value = allMarch.Value;
			if (value != null)
			{
				if (value.worldId != recordingWorldId)
				{
					num++;
					num2 = value.worldId;
				}
				else
				{
					allMarch.Value?.RecordMarchBlock(tileBlockIndex);
				}
			}
		}
		if (num > 0)
		{
			Log.Info($"WorldMapGridRenderer.StartRecordTileBlock skip march count => {num}! target worldId:{recordingWorldId}, skip worldId:{num2}");
		}
	}

	public void StopRecordMarchBlock()
	{
		tileBlockIndex.Clear();
		isRecordingMarchBlock = false;
		recordingWorldId = int.MinValue;
	}

	public void BIRecordWorldMarch(WorldMarch march)
	{
		if (march != null && march.type == NewMarchType.NORMAL)
		{
			WorldScene.BIRecordWorldMarch(march);
		}
	}
}

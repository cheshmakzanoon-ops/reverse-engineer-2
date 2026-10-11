using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using UnityEngine;
using UnityEngine.Audio;
using VEngine;

public sealed class SoundComponent
{
	internal static class Constant
	{
		internal const float DefaultTime = 0f;

		internal const bool DefaultMute = false;

		internal const bool DefaultLoop = false;

		internal const int DefaultPriority = 0;

		internal const float DefaultVolume = 1f;

		internal const float DefaultFadeInSeconds = 0f;

		internal const float DefaultFadeOutSeconds = 0f;

		internal const float DefaultPitch = 1f;

		internal const float DefaultPanStereo = 0f;

		internal const float DefaultSpatialBlend = 0f;

		internal const float DefaultMaxDistance = 100f;

		internal const float DefaultDopplerLevel = 1f;

		internal const float DefaultSoundVolumeSet = -1f;

		internal const int DefaultLoop_Gap = -1;

		internal const int DefaultPreTime = -1;
	}

	public enum AudioPlayingState
	{
		Default,
		Delay,
		Playing,
		Stop,
		LoopGap
	}

	public sealed class PlayAudioParams
	{
		public int id;

		public string path;

		public int randomType;

		public float delay;

		public float delayMin;

		public float delayMax;

		public float startTime;

		public int limit;

		public int whenEqual;

		public int instanceGroupId;

		public int instanceGroupLimit;

		public int instanceGroupWhenEqual;

		public int loop;

		public float fadeIn;

		public float fadeOut;

		public float loopGap;

		public float randomPitchMin = -1f;

		public float randomPitchMax = -1f;

		public float randomVolumeMin = -1f;

		public float randomVolumeMax = -1f;

		public AudioPlayingState state;

		public int serialId;

		public string usePath;

		public string[] pathArr;

		public int pathIndex;

		public float dspTime;

		public float endTime;

		public float fadeInTime;

		public float fadeOutTime;

		public float crossFadeInTime;

		public float crossFadeOutTime;

		public float reactive;

		public float delayTimeCounter;

		public float loopGapCounter;

		public float loopCrossFade;

		public float defaultVolume;

		public float clipLength;

		public bool broken;

		public bool isPause;

		public float randomKeepTime;

		public float loadStartTime;

		public Action callback;

		public Action onMusicLoadFinish;

		public Func<float> getDspTimeFunc;

		public void SetupPlayingData()
		{
			state = AudioPlayingState.Default;
			endTime = clipLength;
			fadeInTime = fadeIn;
			fadeOutTime = fadeOut;
			delayTimeCounter = delay;
			loopGapCounter = loopGap;
		}

		public PlayAudioParams CopyData()
		{
			return new PlayAudioParams
			{
				id = id,
				path = path,
				randomType = randomType,
				delay = delay,
				delayMin = delayMin,
				delayMax = delayMax,
				startTime = startTime,
				limit = limit,
				whenEqual = whenEqual,
				instanceGroupId = instanceGroupId,
				instanceGroupLimit = instanceGroupLimit,
				instanceGroupWhenEqual = instanceGroupWhenEqual,
				loop = loop,
				fadeIn = fadeIn,
				fadeOut = fadeOut,
				loopGap = loopGap,
				randomPitchMin = randomPitchMin,
				randomPitchMax = randomPitchMax,
				randomVolumeMin = randomVolumeMin,
				randomVolumeMax = randomVolumeMax
			};
		}
	}

	public sealed class PlaySoundParams
	{
		public int serialId;

		private float m_Time;

		private bool m_MuteInSoundGroup;

		private bool m_Loop;

		private int m_Priority;

		private float m_VolumeInSoundGroup;

		private float m_FadeInSeconds;

		private float m_Pitch;

		private float m_PanStereo;

		private float m_SpatialBlend;

		private float m_MaxDistance;

		private float m_DopplerLevel;

		private float m_soundVolumeSet;

		private float m_FadeOutSeconds;

		private float m_ShotValumeScale;

		private int m_Loop_gap;

		private List<string> m_SoundAssetPaths;

		public float Time
		{
			get
			{
				return m_Time;
			}
			set
			{
				m_Time = value;
			}
		}

		public bool MuteInSoundGroup
		{
			get
			{
				return m_MuteInSoundGroup;
			}
			set
			{
				m_MuteInSoundGroup = value;
			}
		}

		public bool Loop
		{
			get
			{
				return m_Loop;
			}
			set
			{
				m_Loop = value;
			}
		}

		public int Priority
		{
			get
			{
				return m_Priority;
			}
			set
			{
				m_Priority = value;
			}
		}

		public float VolumeInSoundGroup
		{
			get
			{
				return m_VolumeInSoundGroup;
			}
			set
			{
				m_VolumeInSoundGroup = value;
			}
		}

		public float ShotVolumeScale
		{
			get
			{
				return m_ShotValumeScale;
			}
			set
			{
				m_ShotValumeScale = value;
			}
		}

		public float FadeInSeconds
		{
			get
			{
				return m_FadeInSeconds;
			}
			set
			{
				m_FadeInSeconds = value;
			}
		}

		public float FadeOutSeconds
		{
			get
			{
				return m_FadeOutSeconds;
			}
			set
			{
				m_FadeOutSeconds = value;
			}
		}

		public float Pitch
		{
			get
			{
				return m_Pitch;
			}
			set
			{
				m_Pitch = value;
			}
		}

		public float PanStereo
		{
			get
			{
				return m_PanStereo;
			}
			set
			{
				m_PanStereo = value;
			}
		}

		public float SpatialBlend
		{
			get
			{
				return m_SpatialBlend;
			}
			set
			{
				m_SpatialBlend = value;
			}
		}

		public float MaxDistance
		{
			get
			{
				return m_MaxDistance;
			}
			set
			{
				m_MaxDistance = value;
			}
		}

		public float DopplerLevel
		{
			get
			{
				return m_DopplerLevel;
			}
			set
			{
				m_DopplerLevel = value;
			}
		}

		public float SoundVolumeSet
		{
			get
			{
				return m_soundVolumeSet;
			}
			set
			{
				m_soundVolumeSet = value;
			}
		}

		public int Loop_Gap
		{
			get
			{
				return m_Loop_gap;
			}
			set
			{
				m_Loop_gap = value;
			}
		}

		public List<string> SoundAssetPaths
		{
			get
			{
				return m_SoundAssetPaths;
			}
			set
			{
				m_SoundAssetPaths = value;
			}
		}

		public PlaySoundParams()
		{
			m_Time = 0f;
			m_MuteInSoundGroup = false;
			m_Loop = false;
			m_Priority = 0;
			m_VolumeInSoundGroup = 1f;
			m_FadeInSeconds = 0f;
			m_Pitch = 1f;
			m_PanStereo = 0f;
			m_SpatialBlend = 0f;
			m_MaxDistance = 100f;
			m_DopplerLevel = 1f;
			m_soundVolumeSet = -1f;
			m_FadeOutSeconds = 0f;
			m_ShotValumeScale = 1f;
			m_Loop_gap = -1;
			m_SoundAssetPaths = null;
		}
	}

	internal sealed class PlaySoundInfo
	{
		private readonly Vector3 m_WorldPosition;

		private readonly object m_UserData;

		private readonly int m_SerialId;

		private readonly SoundGroup m_SoundGroup;

		private readonly PlaySoundParams m_PlaySoundParams;

		public Vector3 WorldPosition => m_WorldPosition;

		public object UserData => m_UserData;

		public int SerialId => m_SerialId;

		public SoundGroup SoundGroup => m_SoundGroup;

		public PlaySoundParams PlaySoundParams => m_PlaySoundParams;

		public PlaySoundInfo(int serialId, SoundGroup soundGroup, PlaySoundParams playSoundParams, object userData)
		{
			m_SerialId = serialId;
			m_SoundGroup = soundGroup;
			m_PlaySoundParams = playSoundParams;
			m_UserData = userData;
		}

		public PlaySoundInfo(Vector3 worldPosition, object userData)
		{
			m_WorldPosition = worldPosition;
			m_UserData = userData;
		}
	}

	[Serializable]
	public sealed class SoundGroup
	{
		private class SoundInstLimitGroup
		{
			private int _id;

			private int _limit;

			private int _whenLimited;

			private LinkedList<AudioSource> _list = new LinkedList<AudioSource>();

			public int Limit => _limit;

			public int Length => _list.Count;

			public SoundInstLimitGroup(int id, int limit, int whenLimited)
			{
				_id = id;
				_limit = limit;
				_whenLimited = whenLimited;
			}

			public void Add(AudioSource audioSource)
			{
				if (_list.Count > 0)
				{
					LinkedListNode<AudioSource> linkedListNode = null;
					LinkedListNode<AudioSource> linkedListNode2 = _list.First;
					while (linkedListNode2 != null && linkedListNode2.Value.priority >= audioSource.priority)
					{
						linkedListNode = linkedListNode2;
						linkedListNode2 = linkedListNode2.Next;
					}
					if (linkedListNode != null)
					{
						_list.AddAfter(linkedListNode, audioSource);
					}
					else
					{
						_list.AddFirst(audioSource);
					}
				}
				else
				{
					_list.AddLast(audioSource);
				}
			}

			public void Remove(AudioSource audioSource)
			{
				if (_list.Contains(audioSource))
				{
					_list.Remove(audioSource);
				}
			}

			public AudioSource RemoveLowestPriorityAudio()
			{
				if (_whenLimited == 0)
				{
					AudioSource value = _list.First.Value;
					_list.RemoveFirst();
					return value;
				}
				LinkedListNode<AudioSource> linkedListNode = null;
				LinkedListNode<AudioSource> linkedListNode2 = _list.First;
				while (linkedListNode2 != null && linkedListNode2.Value.priority >= _list.First.Value.priority)
				{
					linkedListNode = linkedListNode2;
					linkedListNode2 = linkedListNode2.Next;
				}
				_list.Remove(linkedListNode);
				return linkedListNode.Value;
			}

			public int GetLowestPriority()
			{
				if (_list != null && _list.First != null && _list.First.Value != null)
				{
					return _list.First.Value.priority;
				}
				return 0;
			}
		}

		private class OneShotSound
		{
			public Asset soundAsset;

			public float playTime;

			public float clipLength;

			public bool cacheAsset;

			public int serialId;

			public float soundVolumeSet;
		}

		private Dictionary<string, string> _groupVolumeNameMap = new Dictionary<string, string>
		{
			{ "Master", "Master_Volume" },
			{ "UI_Reward", "UI_Reward_Volume" },
			{ "UI_Click", "UI_Click_Volume" },
			{ "SFX_Battle_Player_Attk", "SFX_Battle_Player_Attk_Volume" },
			{ "SFX_Battle_Player_Skill", "SFX_Battle_Player_Skill_Volume" },
			{ "SFX_Battle_Enemy_Attk", "SFX_Battle_Enemy_Attk_Volume" },
			{ "SFX_Battle_Boss_Attk", "SFX_Battle_Boss_Attk_Volume" },
			{ "SFX_Battle_Boss_Skill", "SFX_Battle_Boss_Skill_Volume" },
			{ "SFX_Battle_Object_Basic", "SFX_Battle_Object_Basic_Volume" },
			{ "SFX_Battle_Object_Fast", "SFX_Battle_Object_Fast_Volume" },
			{ "SFX_Env", "SFX_Env_Volume" },
			{ "SFX_Gameplay", "SFX_Gameplay_Volume" },
			{ "VO", "VO_Volume" },
			{ "AMB", "AMB_Volume" },
			{ "TIMELINE_SFX", "TIMELINE_SFX_Volume" },
			{ "TIMELINE_Music", "TIMELINE_Music_Volume" },
			{ "MUSIC", "MUSIC_Volume" }
		};

		private Dictionary<string, string> _groupEQNameMap = new Dictionary<string, string>
		{
			{ "Master", "Master_EQ" },
			{ "UI_Reward", "UI_Reward_EQ" },
			{ "UI_Click", "UI_Click_EQ" },
			{ "SFX_Battle_Player_Attk", "SFX_Battle_Player_Attk_EQ" },
			{ "SFX_Battle_Player_Skill", "SFX_Battle_Player_Skill_EQ" },
			{ "SFX_Battle_Enemy_Attk", "SFX_Battle_Enemy_Attk_EQ" },
			{ "SFX_Battle_Boss_Attk", "SFX_Battle_Boss_Attk_EQ" },
			{ "SFX_Battle_Boss_Skill", "SFX_Battle_Boss_Skill_EQ" },
			{ "SFX_Battle_Object_Basic", "SFX_Battle_Object_Basic_EQ" },
			{ "SFX_Battle_Object_Fast", "SFX_Battle_Object_Fast_EQ" },
			{ "SFX_Env", "SFX_Env_EQ" },
			{ "SFX_Gameplay", "SFX_Gameplay_EQ" },
			{ "VO", "VO_EQ" },
			{ "AMB", "AMB_EQ" },
			{ "TIMELINE_SFX", "TIMELINE_SFX_EQ" },
			{ "TIMELINE_Music", "TIMELINE_Music_EQ" },
			{ "MUSIC", "MUSIC_EQ" }
		};

		private AudioMixerGroup _mixerGroup;

		private List<AudioSource> _audioSources = new List<AudioSource>();

		private List<string> _audioUrls = new List<string>();

		private List<PlayAudioParams> _playAudioParams = new List<PlayAudioParams>();

		private Dictionary<AudioPlayingState, Action<int, float>> _audioStateHandlers;

		private List<AudioSource> _retiring = new List<AudioSource>();

		private List<PlayAudioParams> _retiringParams = new List<PlayAudioParams>();

		private Dictionary<string, List<AudioSource>> _soundLimitMap = new Dictionary<string, List<AudioSource>>();

		private Dictionary<int, SoundInstLimitGroup> _soundInstGroupLimitMap = new Dictionary<int, SoundInstLimitGroup>();

		public bool isEff;

		public bool isAmb;

		public float lodVolumeCityMin;

		public float lodVolumeCityMax;

		public float lodEQCityMin;

		public float lodEQCityMax;

		public float lodVolumeWorldMin;

		public float lodVolumeWorldMax;

		public float lodEQWorldMin;

		public float lodEQWorldMax;

		private bool _newGroup;

		private float _globalSoundVolumeRatio = 1f;

		private float _lodVolumeRatio = 1f;

		private float _globalSettingVolumeRatio = 1f;

		private int m_serialId;

		private string m_Name;

		private AudioSource m_AudioSource;

		private Transform m_AudioSourceTransform;

		private Asset m_SoundAsset;

		private List<string> m_SoundAssetPaths;

		private float m_soundVolumeSet = -1f;

		private float m_soundStopTimer = -1f;

		private bool m_soundStopFadeOut;

		private float _fromVolume;

		private float _toVolume;

		private float _time;

		private float _curTime;

		private bool _changeVolume;

		private bool _inFadeOut;

		private int _waitSerialId;

		private float _startTime;

		private Asset _waitSoundAsset;

		private PlaySoundParams _waitSoundParams;

		private Action _waitOnFinishCallback;

		private Action _onFinishCallback;

		private List<OneShotSound> m_OneShots = new List<OneShotSound>();

		private bool _mute;

		private float _volume = 1f;

		public AudioMixerGroup MixerGroup
		{
			get
			{
				return _mixerGroup;
			}
			set
			{
				_mixerGroup = value;
				if (!_mute)
				{
					SetAudioMixerGroupDbVolume(_volume * _lodVolumeRatio * GlobalSettingVolumeRatio);
				}
			}
		}

		public bool IsNewGroup => _newGroup;

		public float GlobalSoundVolumeRatio
		{
			get
			{
				return _globalSoundVolumeRatio;
			}
			set
			{
				_globalSoundVolumeRatio = value;
				float volume = Volume;
				Volume = volume;
			}
		}

		public float LodVolumeRatio
		{
			get
			{
				return _lodVolumeRatio;
			}
			set
			{
				_lodVolumeRatio = value;
				float volume = Volume;
				Volume = volume;
			}
		}

		public float GlobalSettingVolumeRatio
		{
			get
			{
				return _globalSettingVolumeRatio;
			}
			set
			{
				_globalSettingVolumeRatio = value;
				float volume = Volume;
				Volume = volume;
			}
		}

		public AudioSource SharedAudioSource => m_AudioSource;

		public Asset SoundAsset => m_SoundAsset;

		public int SerialId => m_serialId;

		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		public float CurTime => m_AudioSource.time;

		public Transform AudioSourceTransform => m_AudioSourceTransform;

		public bool Mute
		{
			get
			{
				return _mute;
			}
			set
			{
				_mute = value;
				m_AudioSource.mute = value;
				for (int i = 0; i < _audioSources.Count; i++)
				{
					_audioSources[i].mute = value;
				}
				if (_mute)
				{
					SetAudioMixerGroupDbVolume(0f);
				}
				else
				{
					SetAudioMixerGroupDbVolume(_volume * _lodVolumeRatio * GlobalSettingVolumeRatio);
				}
			}
		}

		public float Volume
		{
			get
			{
				return _volume;
			}
			set
			{
				m_AudioSource.volume = value * GlobalSoundVolumeRatio * GlobalSettingVolumeRatio;
				_volume = value;
				if (!_mute)
				{
					SetAudioMixerGroupDbVolume(value * _lodVolumeRatio * GlobalSettingVolumeRatio);
				}
			}
		}

		public bool Loop
		{
			get
			{
				return m_AudioSource.loop;
			}
			set
			{
				m_AudioSource.loop = value;
			}
		}

		public List<AudioSource> GetAudioSources()
		{
			return _audioSources;
		}

		public List<string> GetAudioURLs()
		{
			return _audioUrls;
		}

		public bool IsSoundPlaying(string path)
		{
			if (!_audioUrls.Contains(path))
			{
				return false;
			}
			int index = _audioUrls.IndexOf(path);
			if (_playAudioParams[index].broken)
			{
				return false;
			}
			return true;
		}

		public bool HasSerialId(int serialId)
		{
			for (int i = 0; i < _playAudioParams.Count; i++)
			{
				if (_playAudioParams[i].serialId == serialId)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsNotExpired(float loadStartTime)
		{
			if (_playAudioParams.Count == 0)
			{
				return true;
			}
			for (int i = 0; i < _playAudioParams.Count; i++)
			{
				if (_playAudioParams[i].loadStartTime < loadStartTime)
				{
					return true;
				}
			}
			return false;
		}

		private void AddAudioSource(AudioSource audioSource, string url, PlayAudioParams info)
		{
			_audioSources.Add(audioSource);
			_audioUrls.Add(url);
			_playAudioParams.Add(info);
		}

		private void RemoveAudioSource(int index)
		{
			AudioSource audio = _audioSources[index];
			_audioSources.RemoveAt(index);
			_audioUrls.RemoveAt(index);
			PlayAudioParams playAudioParams = _playAudioParams[index];
			playAudioParams.callback = null;
			_playAudioParams.RemoveAt(index);
			RemoveFromLimit(audio, playAudioParams);
		}

		private void RemoveFromLimit(AudioSource audio, PlayAudioParams info)
		{
			_soundLimitMap[info.path].Remove(audio);
			_soundInstGroupLimitMap[info.instanceGroupId].Remove(audio);
		}

		public void StopAllAudioSource()
		{
			for (int num = _audioSources.Count - 1; num >= 0; num--)
			{
				AudioSource audioSource = _audioSources[num];
				PlayAudioParams playAudioParams = _playAudioParams[num];
				if (!audioSource.isPlaying)
				{
					RemoveAudioSource(num);
					GameEntry.Sound.UnspawnAudioSource(playAudioParams.usePath, audioSource);
				}
				else
				{
					audioSource.loop = false;
					playAudioParams.state = AudioPlayingState.Stop;
					playAudioParams.endTime = audioSource.time + 0.06f;
					playAudioParams.fadeOutTime = 0.06f;
					playAudioParams.callback = null;
					playAudioParams.broken = true;
					playAudioParams.isPause = false;
				}
			}
		}

		private void StopAudioSource(AudioSource audioSource)
		{
			int index = _audioSources.IndexOf(audioSource);
			AudioSource audioSource2 = _audioSources[index];
			PlayAudioParams playAudioParams = _playAudioParams[index];
			if (!audioSource2.isPlaying)
			{
				RemoveAudioSource(index);
				GameEntry.Sound.UnspawnAudioSource(playAudioParams.usePath, audioSource2);
				return;
			}
			audioSource2.loop = false;
			playAudioParams.state = AudioPlayingState.Stop;
			playAudioParams.isPause = false;
			playAudioParams.endTime = audioSource2.time + 0.06f;
			playAudioParams.fadeOutTime = 0.06f;
			playAudioParams.callback = null;
		}

		public void PlayAudioSource(int serialId, PlayAudioParams playAudioParams, Asset soundAsset)
		{
			AudioSource audioSource = GameEntry.Sound.SpawnAudioSource(soundAsset.pathOrURL);
			if (!_soundInstGroupLimitMap.ContainsKey(playAudioParams.instanceGroupId))
			{
				_soundInstGroupLimitMap.Add(playAudioParams.instanceGroupId, new SoundInstLimitGroup(playAudioParams.instanceGroupId, playAudioParams.instanceGroupLimit, playAudioParams.instanceGroupWhenEqual));
			}
			SoundInstLimitGroup soundInstLimitGroup = _soundInstGroupLimitMap[playAudioParams.instanceGroupId];
			if (!_soundLimitMap.ContainsKey(playAudioParams.path))
			{
				_soundLimitMap.Add(playAudioParams.path, new List<AudioSource>());
			}
			else if (playAudioParams.limit > 0 && _soundLimitMap[playAudioParams.path].Count >= playAudioParams.limit)
			{
				if (playAudioParams.whenEqual == -1)
				{
					playAudioParams.whenEqual = playAudioParams.instanceGroupWhenEqual;
				}
				if (playAudioParams.whenEqual != 0)
				{
					GameEntry.Sound.UnspawnAudioSource(soundAsset.pathOrURL, audioSource);
					return;
				}
				AudioSource audioSource2 = _soundLimitMap[playAudioParams.path][0];
				_soundLimitMap[playAudioParams.path].RemoveAt(0);
				soundInstLimitGroup.Remove(audioSource2);
				StopAudioSource(audioSource2);
			}
			if (soundInstLimitGroup.Length >= soundInstLimitGroup.Limit)
			{
				int lowestPriority = soundInstLimitGroup.GetLowestPriority();
				AudioSource component = (soundAsset.asset as GameObject).GetComponent<AudioSource>();
				if (lowestPriority > component.priority)
				{
					AudioSource audioSource3 = soundInstLimitGroup.RemoveLowestPriorityAudio();
					_soundLimitMap[playAudioParams.path].Remove(audioSource3);
					StopAudioSource(audioSource3);
				}
				else
				{
					if (lowestPriority != component.priority)
					{
						GameEntry.Sound.UnspawnAudioSource(soundAsset.pathOrURL, audioSource);
						return;
					}
					if (playAudioParams.instanceGroupWhenEqual != 0)
					{
						GameEntry.Sound.UnspawnAudioSource(soundAsset.pathOrURL, audioSource);
						return;
					}
					AudioSource audioSource4 = soundInstLimitGroup.RemoveLowestPriorityAudio();
					_soundLimitMap[playAudioParams.path].Remove(audioSource4);
					StopAudioSource(audioSource4);
				}
			}
			soundInstLimitGroup.Add(audioSource);
			_soundLimitMap[playAudioParams.path].Add(audioSource);
			audioSource.gameObject.name = soundAsset.pathOrURL;
			if (playAudioParams.randomPitchMin != playAudioParams.randomPitchMax)
			{
				audioSource.pitch = UnityEngine.Random.Range(playAudioParams.randomPitchMin, playAudioParams.randomPitchMax);
			}
			else if (playAudioParams.randomPitchMin != -1f)
			{
				audioSource.pitch = playAudioParams.randomPitchMin;
			}
			if (playAudioParams.randomVolumeMin != playAudioParams.randomVolumeMax)
			{
				audioSource.volume = UnityEngine.Random.Range(playAudioParams.randomVolumeMin, playAudioParams.randomVolumeMax);
			}
			else if (playAudioParams.randomVolumeMin != -1f)
			{
				audioSource.volume = playAudioParams.randomVolumeMin;
			}
			audioSource.loop = false;
			if (playAudioParams.loop == 1 && playAudioParams.pathArr == null && playAudioParams.loopGap == 0f)
			{
				audioSource.loop = true;
			}
			playAudioParams.serialId = serialId;
			playAudioParams.defaultVolume = audioSource.volume;
			playAudioParams.clipLength = audioSource.clip.length;
			playAudioParams.SetupPlayingData();
			playAudioParams.state = AudioPlayingState.Default;
			if (playAudioParams.delayMin == 0f && playAudioParams.delayMax == 0f)
			{
				if (playAudioParams.crossFadeInTime > 0f)
				{
					if (_audioSources.Count > 0)
					{
						audioSource.volume = 0f;
						for (int num = _audioSources.Count - 1; num >= 0; num--)
						{
							AudioSource audioSource5 = _audioSources[num];
							PlayAudioParams playAudioParams2 = _playAudioParams[num];
							if (!audioSource5.isPlaying)
							{
								RemoveAudioSource(num);
								GameEntry.Sound.UnspawnAudioSource(playAudioParams2.usePath, audioSource5);
							}
							else
							{
								audioSource5.loop = false;
								playAudioParams2.crossFadeOutTime = playAudioParams.crossFadeInTime;
								playAudioParams2.endTime = audioSource5.time + playAudioParams.crossFadeInTime;
								playAudioParams2.isPause = false;
								_retiring.Add(audioSource5);
								_retiringParams.Add(playAudioParams2);
								RemoveAudioSource(num);
							}
						}
					}
					else
					{
						playAudioParams.crossFadeInTime = 0f;
					}
				}
				audioSource.mute = Mute;
				playAudioParams.state = AudioPlayingState.Playing;
				if (playAudioParams.fadeInTime > 0f)
				{
					audioSource.volume = 0f;
				}
				if (playAudioParams.dspTime <= 0f)
				{
					audioSource.time = playAudioParams.startTime;
					audioSource.Play();
				}
				else
				{
					audioSource.time = 0f;
					audioSource.PlayScheduled(playAudioParams.dspTime);
				}
			}
			AddAudioSource(audioSource, soundAsset.pathOrURL, playAudioParams);
		}

		private void InitAudioFSM()
		{
			if (_audioStateHandlers == null)
			{
				_audioStateHandlers = new Dictionary<AudioPlayingState, Action<int, float>>
				{
					{
						AudioPlayingState.Default,
						State_Default
					},
					{
						AudioPlayingState.Delay,
						State_Delay
					},
					{
						AudioPlayingState.Playing,
						State_Playing
					},
					{
						AudioPlayingState.LoopGap,
						State_LoopGap
					},
					{
						AudioPlayingState.Stop,
						State_Stop
					}
				};
			}
		}

		private void State_Default(int i, float dt)
		{
			PlayAudioParams playAudioParams = _playAudioParams[i];
			if (Mathf.Abs(playAudioParams.delayMin - playAudioParams.delayMax) > 0.001f)
			{
				playAudioParams.delay = UnityEngine.Random.Range(playAudioParams.delayMin, playAudioParams.delayMax);
			}
			else if (playAudioParams.delayMin > 0f)
			{
				playAudioParams.delay = playAudioParams.delayMin;
			}
			if (playAudioParams.delay > 0f)
			{
				playAudioParams.delayTimeCounter = playAudioParams.delay;
				playAudioParams.delay = 0f;
				playAudioParams.delayMin = 0f;
				playAudioParams.delayMax = 0f;
				playAudioParams.state = AudioPlayingState.Delay;
			}
			else
			{
				playAudioParams.state = AudioPlayingState.Playing;
			}
		}

		private void State_Delay(int i, float dt)
		{
			PlayAudioParams playAudioParams = _playAudioParams[i];
			AudioSource audioSource = _audioSources[i];
			if (playAudioParams.delayTimeCounter > 0f)
			{
				playAudioParams.delayTimeCounter -= dt;
				return;
			}
			playAudioParams.state = AudioPlayingState.Playing;
			if (playAudioParams.fadeInTime > 0f)
			{
				audioSource.volume = 0f;
			}
			audioSource.time = playAudioParams.startTime;
			playAudioParams.isPause = false;
			if (playAudioParams.dspTime <= 0f)
			{
				audioSource.Play();
			}
			else
			{
				audioSource.PlayScheduled(playAudioParams.dspTime);
			}
			if (!(playAudioParams.crossFadeInTime > 0f) || _audioSources.Count <= 0)
			{
				return;
			}
			audioSource.volume = 0f;
			for (int num = _audioSources.Count - 1; num > -1; num--)
			{
				AudioSource audioSource2 = _audioSources[num];
				if (audioSource2 != audioSource)
				{
					PlayAudioParams playAudioParams2 = _playAudioParams[num];
					if (!audioSource2.isPlaying)
					{
						RemoveAudioSource(num);
						GameEntry.Sound.UnspawnAudioSource(playAudioParams2.usePath, audioSource2);
					}
					else
					{
						audioSource2.loop = false;
						playAudioParams2.crossFadeOutTime = playAudioParams.crossFadeInTime;
						playAudioParams2.endTime = audioSource2.time + playAudioParams.crossFadeInTime;
						playAudioParams2.isPause = false;
						_retiring.Add(audioSource2);
						_retiringParams.Add(playAudioParams2);
						RemoveAudioSource(num);
					}
				}
			}
		}

		private void State_Playing(int i, float dt)
		{
			PlayAudioParams playAudioParams = _playAudioParams[i];
			AudioSource audioSource = _audioSources[i];
			float volume = playAudioParams.defaultVolume;
			float num = ((playAudioParams.crossFadeInTime > 0f) ? playAudioParams.crossFadeInTime : playAudioParams.fadeInTime);
			if (num > 0f && audioSource.time - playAudioParams.startTime < num)
			{
				volume = Mathf.Lerp(0f, playAudioParams.defaultVolume, Mathf.Min((audioSource.time - playAudioParams.startTime) / num, 1f));
			}
			float num2 = ((playAudioParams.crossFadeOutTime > 0f) ? playAudioParams.crossFadeOutTime : playAudioParams.fadeOutTime);
			if (num2 <= 0.06f)
			{
				num2 = 0.06f;
			}
			if (audioSource.time > playAudioParams.endTime - num2)
			{
				float num3 = playAudioParams.endTime - num2;
				volume = Mathf.Lerp(playAudioParams.defaultVolume, 0f, (audioSource.time - num3) / num2);
			}
			audioSource.volume = volume;
			if (!playAudioParams.broken && playAudioParams.pathArr != null && playAudioParams.pathArr.Length > 1 && playAudioParams.loop == 1 && playAudioParams.loopGap == 0f && playAudioParams.loopCrossFade > 0f && audioSource.time > playAudioParams.endTime - playAudioParams.loopCrossFade && audioSource.isActiveAndEnabled)
			{
				PlayAudioParams playAudioParams2 = playAudioParams.CopyData();
				playAudioParams2.loopCrossFade = playAudioParams.loopCrossFade;
				playAudioParams2.crossFadeInTime = playAudioParams.loopCrossFade;
				playAudioParams2.crossFadeOutTime = 0f;
				playAudioParams2.pathIndex = playAudioParams.pathIndex;
				playAudioParams.broken = true;
				GameEntry.Sound.PlayAudio(playAudioParams2);
			}
			if (audioSource.isPlaying || playAudioParams.isPause)
			{
				return;
			}
			if (playAudioParams.loop == 1 && !playAudioParams.broken)
			{
				if (playAudioParams.loopGap > 0f)
				{
					playAudioParams.loopGapCounter = playAudioParams.loopGap;
					playAudioParams.state = AudioPlayingState.LoopGap;
					return;
				}
				string path = _audioUrls[i];
				playAudioParams.callback?.Invoke();
				RemoveAudioSource(i);
				if (!playAudioParams.broken && playAudioParams.loopCrossFade == 0f && audioSource.isActiveAndEnabled)
				{
					GameEntry.Sound.PlayAudio(playAudioParams);
				}
				GameEntry.Sound.UnspawnAudioSource(path, audioSource);
			}
			else
			{
				playAudioParams.state = AudioPlayingState.Stop;
			}
		}

		private void State_LoopGap(int i, float dt)
		{
			PlayAudioParams playAudioParams = _playAudioParams[i];
			AudioSource audioSource = _audioSources[i];
			if (playAudioParams.loopGapCounter > 0f)
			{
				playAudioParams.loopGapCounter -= dt;
				return;
			}
			string path = _audioUrls[i];
			playAudioParams.callback?.Invoke();
			RemoveAudioSource(i);
			if (!playAudioParams.broken && audioSource.isActiveAndEnabled)
			{
				GameEntry.Sound.PlayAudio(playAudioParams);
			}
			GameEntry.Sound.UnspawnAudioSource(path, audioSource);
		}

		private void State_Stop(int i, float dt)
		{
			PlayAudioParams playAudioParams = _playAudioParams[i];
			AudioSource audioSource = _audioSources[i];
			if (audioSource.time >= playAudioParams.endTime)
			{
				audioSource.Stop();
			}
			if (!audioSource.isPlaying && !playAudioParams.isPause)
			{
				string path = _audioUrls[i];
				playAudioParams.callback?.Invoke();
				RemoveAudioSource(i);
				GameEntry.Sound.UnspawnAudioSource(path, audioSource);
				GameEntry.Sound.RandomSoundEndCheck(playAudioParams);
			}
		}

		public SoundGroup(string name, bool useAudioMixer = false)
		{
			m_Name = name;
			_onFinishCallback = null;
			_newGroup = useAudioMixer;
			GameObject gameObject = new GameObject("SoundGroup_" + name);
			m_AudioSourceTransform = gameObject.transform;
			m_AudioSource = gameObject.AddComponent<AudioSource>();
			m_AudioSource.playOnAwake = false;
			m_AudioSource.rolloffMode = AudioRolloffMode.Custom;
			GlobalSoundVolumeRatio = GameEntry.Sound.globalSoundVolumeRatio;
			switch (name)
			{
			case "Music":
			case "AMBSound":
			case "MUSIC":
			case "AMB":
			{
				float globalSettingVolumeRatio2 = GameEntry.Setting.GetFloat("MUSIC_VOLUME", 1f);
				GlobalSettingVolumeRatio = globalSettingVolumeRatio2;
				break;
			}
			default:
			{
				float globalSettingVolumeRatio = GameEntry.Setting.GetFloat("EFFECT_VOLUME", 1f);
				GlobalSettingVolumeRatio = globalSettingVolumeRatio;
				break;
			}
			}
			InitAudioFSM();
		}

		public float GetAudioMixerGroupEQ()
		{
			float value = float.MinValue;
			if (_mixerGroup != null)
			{
				string name = _groupEQNameMap[_mixerGroup.name];
				_mixerGroup.audioMixer.GetFloat(name, out value);
			}
			return value;
		}

		public void SetAudioMixerGroupEQ(float v)
		{
			if (_mixerGroup != null)
			{
				string name = _groupEQNameMap[_mixerGroup.name];
				_mixerGroup.audioMixer.SetFloat(name, v);
			}
		}

		public float GetAudioMixerGroupDbVolume()
		{
			float value = float.MinValue;
			if (_mixerGroup != null)
			{
				string name = _groupVolumeNameMap[_mixerGroup.name];
				_mixerGroup.audioMixer.GetFloat(name, out value);
			}
			return value;
		}

		public void SetAudioMixerGroupDbVolume(float v)
		{
			if (_mixerGroup != null)
			{
				float num = Mathf.Clamp01(v);
				float value = ((num > 0.0001f) ? (Mathf.Log10(num) * 20f) : (-80f));
				string name = _groupVolumeNameMap[_mixerGroup.name];
				_mixerGroup.audioMixer.SetFloat(name, value);
			}
		}

		public void PlaySound(int serialId, Asset soundAsset, PlaySoundParams playSoundParams, float startTime = 0f, float dspTime = -1f, Action onFinishCallback = null)
		{
			if (_inFadeOut)
			{
				_waitSerialId = serialId;
				_waitSoundAsset = soundAsset;
				_waitSoundParams = playSoundParams;
				_startTime = startTime;
				_waitOnFinishCallback = onFinishCallback;
				return;
			}
			_onFinishCallback = onFinishCallback;
			if (m_SoundAsset != null)
			{
				m_SoundAsset.Release();
			}
			if (m_OneShots.Count > 0)
			{
				foreach (OneShotSound oneShot in m_OneShots)
				{
					if (!oneShot.cacheAsset)
					{
						oneShot.soundAsset.Release();
					}
				}
				m_OneShots.Clear();
			}
			m_SoundAsset = soundAsset;
			AudioClip audioClip = soundAsset.asset as AudioClip;
			if (audioClip == null)
			{
				Log.Error("Audio Clip is Null");
				return;
			}
			if (m_AudioSource == null)
			{
				Log.Error("Audio Source is Null");
				return;
			}
			if (m_soundVolumeSet > -1f)
			{
				GameEntry.Sound.TryResumeGlobalSoundControl(m_serialId);
				m_soundVolumeSet = -1f;
			}
			m_serialId = serialId;
			Loop = playSoundParams.Loop;
			m_soundStopTimer = -1f;
			m_soundStopFadeOut = false;
			float soundVolumeSet = playSoundParams.SoundVolumeSet;
			if (soundVolumeSet > -1f)
			{
				GameEntry.Sound.TryControlGlobalSound(serialId, Name, soundVolumeSet);
				GlobalSoundVolumeRatio = 1f;
			}
			if (!Loop && audioClip.length > 0f)
			{
				m_soundStopTimer = audioClip.length;
			}
			if (playSoundParams.FadeOutSeconds > 0f && !Loop)
			{
				if (m_soundStopTimer < 0f)
				{
					m_soundStopTimer = audioClip.length;
				}
				m_soundStopTimer -= playSoundParams.FadeOutSeconds;
				m_soundStopTimer = Mathf.Max(float.Epsilon, m_soundStopTimer);
				m_soundStopFadeOut = true;
			}
			m_soundVolumeSet = soundVolumeSet;
			Volume = playSoundParams.VolumeInSoundGroup;
			if (startTime < 0f || startTime >= audioClip.length)
			{
				Log.Error("[AudioMixer]Set AudioSource to a wrong time : " + soundAsset.pathOrURL);
				startTime = 0f;
			}
			m_AudioSource.time = startTime;
			m_AudioSource.clip = audioClip;
			m_AudioSource.pitch = playSoundParams.Pitch;
			if (dspTime < 0f)
			{
				m_AudioSource.Play();
			}
			else
			{
				m_AudioSource.PlayScheduled(dspTime);
			}
			if (playSoundParams.FadeInSeconds > 0f)
			{
				Volume = 0f;
				ChangeVolume(playSoundParams.VolumeInSoundGroup, playSoundParams.FadeInSeconds);
			}
		}

		public void PlayOneShot(int serialId, Asset soundAsset, PlaySoundParams playSoundParams, bool cacheAsset = false)
		{
			AudioClip audioClip = soundAsset.asset as AudioClip;
			m_OneShots.Add(new OneShotSound
			{
				soundAsset = soundAsset,
				clipLength = ((audioClip != null) ? audioClip.length : 0f),
				playTime = 0f,
				cacheAsset = cacheAsset,
				serialId = serialId,
				soundVolumeSet = playSoundParams.SoundVolumeSet
			});
			m_serialId = serialId;
			Loop = playSoundParams.Loop;
			m_soundVolumeSet = -1f;
			float soundVolumeSet = playSoundParams.SoundVolumeSet;
			if (soundVolumeSet > -1f)
			{
				GameEntry.Sound.TryControlGlobalSound(serialId, Name, soundVolumeSet);
				GlobalSoundVolumeRatio = 1f;
			}
			m_soundVolumeSet = soundVolumeSet;
			Volume = playSoundParams.VolumeInSoundGroup;
			m_AudioSource.PlayOneShot(audioClip, playSoundParams.ShotVolumeScale);
		}

		public void StopSound()
		{
			if (!_newGroup)
			{
				if (m_soundVolumeSet > -1f)
				{
					GameEntry.Sound.TryResumeGlobalSoundControl(m_serialId);
				}
				m_serialId = 0;
				m_soundVolumeSet = -1f;
				m_soundStopTimer = -1f;
				m_soundStopFadeOut = false;
				if (m_AudioSource != null)
				{
					m_AudioSource.Stop();
					m_AudioSource.clip = null;
				}
				if (m_SoundAsset != null)
				{
					m_SoundAsset.Release();
					m_SoundAsset = null;
				}
				if (m_OneShots.Count > 0)
				{
					foreach (OneShotSound oneShot in m_OneShots)
					{
						if (!oneShot.cacheAsset)
						{
							oneShot.soundAsset.Release();
						}
						if (oneShot.soundVolumeSet > -1f)
						{
							GameEntry.Sound.TryResumeGlobalSoundControl(oneShot.serialId);
						}
					}
					m_OneShots.Clear();
				}
				_onFinishCallback = null;
				_inFadeOut = false;
				_waitSerialId = 0;
				_startTime = 0f;
				_waitSoundAsset = null;
				_waitSoundParams = null;
			}
			else
			{
				StopAllAudioSource();
			}
		}

		public bool StopSoundBySerialId(int serialId)
		{
			for (int num = _playAudioParams.Count - 1; num >= 0; num--)
			{
				AudioSource audioSource = _audioSources[num];
				PlayAudioParams playAudioParams = _playAudioParams[num];
				if (playAudioParams.serialId == serialId)
				{
					if (!audioSource.isPlaying)
					{
						RemoveAudioSource(num);
						GameEntry.Sound.UnspawnAudioSource(playAudioParams.usePath, audioSource);
					}
					else
					{
						audioSource.loop = false;
						playAudioParams.isPause = false;
						playAudioParams.endTime = audioSource.time + 0.06f;
						playAudioParams.fadeOutTime = 0.06f;
						playAudioParams.callback = null;
						playAudioParams.broken = true;
					}
					return true;
				}
			}
			return false;
		}

		public void PauseSound()
		{
			if (m_AudioSource != null)
			{
				m_AudioSource.Pause();
			}
			for (int i = 0; i < _audioSources.Count; i++)
			{
				_audioSources[i].Pause();
				_playAudioParams[i].isPause = true;
			}
		}

		public void ResumeSound()
		{
			if (m_AudioSource != null)
			{
				m_AudioSource.UnPause();
			}
			for (int i = 0; i < _audioSources.Count; i++)
			{
				_audioSources[i].UnPause();
				_playAudioParams[i].isPause = false;
			}
		}

		public void OnUpdate(float elapseSeconds)
		{
			if (!_newGroup)
			{
				for (int num = m_OneShots.Count - 1; num >= 0; num--)
				{
					OneShotSound oneShotSound = m_OneShots[num];
					if (oneShotSound.playTime < oneShotSound.clipLength)
					{
						oneShotSound.playTime += Time.unscaledDeltaTime;
					}
					else
					{
						if (!oneShotSound.cacheAsset)
						{
							oneShotSound.soundAsset.Release();
						}
						if (oneShotSound.soundVolumeSet > -1f)
						{
							GameEntry.Sound.TryResumeGlobalSoundControl(oneShotSound.serialId);
						}
						m_OneShots.RemoveAt(num);
					}
				}
				if (_changeVolume)
				{
					_curTime += Time.unscaledDeltaTime;
					if (_curTime >= _time)
					{
						_changeVolume = false;
						Volume = _toVolume;
						if (_inFadeOut)
						{
							_inFadeOut = false;
							if (_waitSerialId > 0)
							{
								if (m_AudioSource != null)
								{
									m_AudioSource.Stop();
								}
								if (!GameEntry.Sound.IsUseAudioMixer())
								{
									PlaySound(_waitSerialId, _waitSoundAsset, _waitSoundParams, _startTime, -1f, _waitOnFinishCallback);
								}
								_waitSerialId = 0;
								_startTime = 0f;
								_waitSoundAsset = null;
								_waitSoundParams = null;
								_waitOnFinishCallback = null;
							}
							else
							{
								StopSound();
							}
						}
					}
					else
					{
						Volume = Mathf.Lerp(_fromVolume, _toVolume, _curTime / _time);
					}
				}
				if (!(m_soundStopTimer > 0f))
				{
					return;
				}
				m_soundStopTimer -= Time.unscaledDeltaTime;
				if (m_soundStopTimer <= 0f)
				{
					if (m_soundVolumeSet > -1f)
					{
						GameEntry.Sound.TryResumeGlobalSoundControl(m_serialId);
						m_soundVolumeSet = -1f;
					}
					if (m_soundStopFadeOut)
					{
						m_soundStopFadeOut = false;
						FadeOutAndPlaySound(0.5f);
					}
					if (_onFinishCallback != null)
					{
						_onFinishCallback();
					}
					_onFinishCallback = null;
				}
				return;
			}
			if (_changeVolume)
			{
				_curTime += Time.unscaledDeltaTime;
				if (_curTime >= _time)
				{
					_changeVolume = false;
					Volume = _toVolume;
				}
				else
				{
					Volume = Mathf.Lerp(_fromVolume, _toVolume, _curTime / _time);
				}
			}
			if (_audioSources.Count > 0)
			{
				for (int num2 = _audioSources.Count - 1; num2 >= 0; num2--)
				{
					PlayAudioParams playAudioParams = _playAudioParams[num2];
					if (_audioStateHandlers.TryGetValue(playAudioParams.state, out var value))
					{
						value(num2, elapseSeconds);
					}
				}
			}
			if (_retiring.Count <= 0)
			{
				return;
			}
			for (int num3 = _retiring.Count - 1; num3 > -1; num3--)
			{
				AudioSource audioSource = _retiring[num3];
				PlayAudioParams playAudioParams2 = _retiringParams[num3];
				float volume = playAudioParams2.defaultVolume;
				float num4 = ((playAudioParams2.crossFadeOutTime > 0f) ? playAudioParams2.crossFadeOutTime : playAudioParams2.fadeOutTime);
				if (num4 > 0f && audioSource.time > playAudioParams2.endTime - num4)
				{
					float num5 = playAudioParams2.endTime - num4;
					volume = Mathf.Lerp(playAudioParams2.defaultVolume, 0f, (audioSource.time - num5) / num4);
				}
				audioSource.volume = volume;
				if (audioSource.time >= playAudioParams2.endTime)
				{
					audioSource.Stop();
				}
				if (!audioSource.isPlaying)
				{
					GameEntry.Sound.UnspawnAudioSource(playAudioParams2.usePath, audioSource);
					_retiring.RemoveAt(num3);
					_retiringParams.RemoveAt(num3);
				}
			}
		}

		public void ChangeVolume(float to, float time)
		{
			if (time > 0f)
			{
				_fromVolume = Volume;
				_toVolume = to;
				_time = time;
				_curTime = 0f;
				_changeVolume = true;
			}
		}

		public void FadeOutAndPlaySound(float time)
		{
			_inFadeOut = true;
			ChangeVolume(0f, time);
		}
	}

	private HashSet<string> _groupNameStrs = new HashSet<string>
	{
		"Master", "UI_Reward", "UI_Click", "SFX_Battle_Player_Attk", "SFX_Battle_Player_Skill", "SFX_Battle_Enemy_Attk", "SFX_Battle_Boss_Attk", "SFX_Battle_Boss_Skill", "SFX_Battle_Object_Basic", "SFX_Battle_Object_Fast",
		"SFX_Env", "SFX_Gameplay", "VO", "AMB", "TIMELINE_SFX", "TIMELINE_Music", "MUSIC"
	};

	private AudioListener m_AudioListener;

	private Transform m_InstanceRoot;

	private readonly Dictionary<string, SoundGroup> m_SoundGroupDic = new Dictionary<string, SoundGroup>();

	private int m_Serial;

	private readonly Dictionary<int, Asset> m_SoundsBeingLoaded = new Dictionary<int, Asset>();

	private readonly HashSet<int> m_SoundsToReleaseOnLoad = new HashSet<int>();

	private int _bgMusicId;

	private int _ambIdSoundId;

	private Dictionary<ELoopSoundLimit, int> m_loopSoundLimitMap = new Dictionary<ELoopSoundLimit, int>();

	private Dictionary<ELoopSoundLimit, List<LoopTimerSound>> m_loopSoundTimerMap = new Dictionary<ELoopSoundLimit, List<LoopTimerSound>>();

	private readonly HashSet<int> _soundBeingLoadedCache = new HashSet<int>();

	private readonly Dictionary<string, Asset> _effectAssetCache = new Dictionary<string, Asset>();

	private readonly Dictionary<int, string> _preloadedPathBySoundId = new Dictionary<int, string>();

	private readonly Dictionary<int, int> _preloadSerialBySoundId = new Dictionary<int, int>();

	private string mCurrentBGMAssetPath;

	private PlaySoundParams mCurrentPlayBGMSoundParams;

	private ITimer mBGMFinishTimer;

	private ITimer mBGMDelayTimer;

	private string mCurrentAMBSoundAssetPath;

	private bool mAMBIsPlaying;

	private float mCurrentAMBSoundVolume = 1f;

	private PlaySoundParams mCurrentPlayAMBSoundParams;

	private ITimer mAMBSoundTimer;

	private ITimer mAMBSoundDelayTimer;

	private bool mPauseAMBSound;

	private ITimer mResetAMBdelayTimer;

	private bool _useAudioMixer;

	private AudioMixer _audioMixer;

	private AudioSourcePool _audioSourcePool;

	private Dictionary<int, PlayAudioParams> _audioTempMap = new Dictionary<int, PlayAudioParams>();

	private Dictionary<string, Asset> _assetMap = new Dictionary<string, Asset>();

	private Dictionary<string, List<AudioSource>> _audioRefMap = new Dictionary<string, List<AudioSource>>();

	private Dictionary<int, PlayAudioParams> _randomMap = new Dictionary<int, PlayAudioParams>();

	private List<PlayAudioParams> _randomKeepList = new List<PlayAudioParams>();

	private const string MIXER_PATH = "Assets/Main/Sound/AudioMixer.mixer";

	private bool _audioMixerMode;

	private bool _audioMixerLoading;

	private Asset _audioMixerAsset;

	private float _lastMusicTime;

	private string _controlSoundGroupName = string.Empty;

	private int _controlSoundSerialId = -1;

	private float _globalSoundVolumeRatio = 1f;

	private float _audioLodRate;

	private float _cityZoomMin;

	private float _cityZoomMax;

	private float _worldZoomMin;

	private float _worldZoomMax;

	private List<GameObject> _objs = new List<GameObject>();

	private List<string> _paths = new List<string>();

	public int SoundGroupCount => m_SoundGroupDic.Count;

	private float globalSoundVolumeRatio
	{
		get
		{
			return _globalSoundVolumeRatio;
		}
		set
		{
			if (!(Math.Abs(_globalSoundVolumeRatio - value) > float.Epsilon))
			{
				return;
			}
			_globalSoundVolumeRatio = value;
			foreach (SoundGroup value2 in m_SoundGroupDic.Values)
			{
				if (!value2.Name.Equals(_controlSoundGroupName))
				{
					value2.GlobalSoundVolumeRatio = value;
				}
			}
		}
	}

	public bool HasMixerGroup_Str(string name)
	{
		if (_groupNameStrs.Contains(name))
		{
			return true;
		}
		return false;
	}

	public bool CheckSoundWrongOutput()
	{
		if (IsUseAudioMixer())
		{
			bool flag = true;
			foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
			{
				if (!item.Value.Mute)
				{
					flag = false;
					break;
				}
			}
			if (flag && IsAnySoundPlaying())
			{
				return true;
			}
		}
		return false;
	}

	private bool IsAnySoundPlaying(float threshold = 0.01f, int sampleSize = 256)
	{
		float[] array = new float[sampleSize];
		AudioListener.GetOutputData(array, 0);
		float num = 0f;
		for (int i = 0; i < array.Length; i++)
		{
			num += Mathf.Abs(array[i]);
		}
		return num / (float)array.Length > threshold;
	}

	public bool IsAudioMixerLoaded()
	{
		return _audioMixer != null;
	}

	private bool IsUseAudioMixer()
	{
		if (_useAudioMixer && _audioMixer != null)
		{
			return true;
		}
		return false;
	}

	public void SyncAudioMixerUsing(bool use)
	{
		if (use)
		{
			_audioMixerMode = true;
			ReleaseAudioMixer();
			LoadAudioMixer(delegate(AudioMixer mixer)
			{
				if (mixer != null && _audioMixerMode)
				{
					_audioMixer = mixer;
					ChangeToAudioMixerMode();
				}
				else
				{
					_audioMixerMode = false;
					ChangeToSingleAudioSourceMode();
				}
			});
		}
		else
		{
			_audioMixerMode = false;
			ChangeToSingleAudioSourceMode();
		}
	}

	private void ChangeToAudioMixerMode()
	{
		Log.Info("[AudioMixer]MixerOn");
		CancelAMBSoundFinishTimer();
		CancelAMBSoundDelayTimer();
		_useAudioMixer = true;
		StopAllSounds();
		GameEntry.Lua.Call("CSharpCallLuaInterface.SyncUseMixer", param1: true);
	}

	private void ChangeToSingleAudioSourceMode()
	{
		Log.Info("[AudioMixer]MixerOff");
		ReleaseAudioMixer();
		_useAudioMixer = false;
		StopAllSounds();
		GameEntry.Lua.Call("CSharpCallLuaInterface.SyncUseMixer", param1: false);
	}

	private void LoadAudioMixer(Action<AudioMixer> complete = null)
	{
		if (_audioMixerLoading)
		{
			Log.Warning("[AudioMixer]AudioMixer is Loading");
			complete?.Invoke(null);
			return;
		}
		if (!GameEntry.Resource.HasAsset("Assets/Main/Sound/AudioMixer.mixer"))
		{
			Log.Warning("[AudioMixer]Not Find AudioMixer Asset!");
			complete?.Invoke(null);
			return;
		}
		_audioMixerAsset = GameEntry.Resource.LoadAssetAsync("Assets/Main/Sound/AudioMixer.mixer", typeof(AudioMixer));
		if (_audioMixerAsset != null)
		{
			_audioMixerLoading = true;
			Asset audioMixerAsset = _audioMixerAsset;
			audioMixerAsset.completed = (Action<Asset>)Delegate.Combine(audioMixerAsset.completed, (Action<Asset>)delegate
			{
				_audioMixerLoading = false;
				if (_audioMixerAsset != null && !_audioMixerAsset.isError && _audioMixerAsset.asset != null)
				{
					AudioMixer audioMixer = _audioMixerAsset.asset as AudioMixer;
					if (audioMixer != null)
					{
						complete?.Invoke(audioMixer);
					}
				}
				else
				{
					Log.Warning("[AudioMixer]Load AudioMixer Failed!");
					complete?.Invoke(null);
				}
			});
		}
		else
		{
			Log.Warning("[AudioMixer]AudioMixer File Not Find!");
			complete?.Invoke(null);
		}
	}

	private void ReleaseAudioMixer()
	{
		if (_audioMixerAsset != null)
		{
			_audioMixerAsset.Release();
			_audioMixerAsset = null;
			_audioMixer = null;
			_audioMixerLoading = false;
		}
	}

	public string GetLastMusicPath()
	{
		if (IsUseAudioMixer())
		{
			SoundGroup soundGroup = GetSoundGroup("MUSIC", useAudioMixer: true);
			if (soundGroup != null)
			{
				List<AudioSource> audioSources = soundGroup.GetAudioSources();
				List<string> audioURLs = soundGroup.GetAudioURLs();
				if (audioSources != null && audioSources.Count > 0)
				{
					int num = 0;
					if (num < audioSources.Count)
					{
						return audioURLs[num];
					}
				}
			}
		}
		else
		{
			SoundGroup soundGroup2 = GetSoundGroup("Music");
			if (soundGroup2 != null)
			{
				Asset soundAsset = soundGroup2.SoundAsset;
				if (soundAsset != null)
				{
					return soundAsset.pathOrURL;
				}
			}
		}
		return null;
	}

	public void PlayLastMusic(string path)
	{
		if (IsUseAudioMixer())
		{
			PlayAudioParams playAudioParams = new PlayAudioParams();
			playAudioParams.id = 0;
			playAudioParams.path = path;
			playAudioParams.startTime = _lastMusicTime;
			playAudioParams.delay = 0f;
			playAudioParams.loop = 1;
			playAudioParams.fadeIn = 0f;
			playAudioParams.fadeOut = 0f;
			playAudioParams.loopGap = 0f;
			playAudioParams.limit = 1;
			playAudioParams.whenEqual = 0;
			playAudioParams.instanceGroupId = 25;
			playAudioParams.instanceGroupLimit = 1;
			playAudioParams.instanceGroupWhenEqual = 0;
			PlayAudio(playAudioParams);
		}
		else
		{
			PlayMusic(path, loop: true, 0.5f, 0f, 1f, -1f, 1f, useSoundPath2: true, -1, -1, 600);
		}
	}

	public void TryAddAudioSourceAsset(Asset soundAsset)
	{
		_assetMap[soundAsset.pathOrURL] = soundAsset;
		if (!_audioRefMap.ContainsKey(soundAsset.pathOrURL))
		{
			_audioRefMap.Add(soundAsset.pathOrURL, new List<AudioSource>());
		}
	}

	public AudioSource SpawnAudioSource(string path)
	{
		if (_assetMap.ContainsKey(path))
		{
			AudioSource audioSource = _audioSourcePool.Spawn();
			AudioSourcePool.CopyFromTo((_assetMap[path].asset as GameObject).GetComponent<AudioSource>(), audioSource);
			_audioRefMap[path].Add(audioSource);
			return audioSource;
		}
		return null;
	}

	public void UnspawnAudioSource(string path, AudioSource audioSource)
	{
		_audioSourcePool.Unspawn(audioSource);
		if (_audioRefMap.ContainsKey(path))
		{
			_audioRefMap[path].Remove(audioSource);
		}
		if (_audioRefMap[path].Count == 0 && !_effectAssetCache.ContainsKey(path))
		{
			_assetMap[path].Release();
			_assetMap.Remove(path);
		}
	}

	public void SetMasterGroupVolume(float v)
	{
		if (_audioMixer != null)
		{
			float num = Mathf.Clamp01(v);
			float value = ((num > 0.0001f) ? (Mathf.Log10(num) * 20f) : (-80f));
			_audioMixer.SetFloat("Master_Volume", value);
		}
	}

	private void AdjustMixerGroupIntensity(float rate, bool isWorld = false)
	{
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			SoundGroup value = item.Value;
			if (!isWorld)
			{
				if (value.lodEQCityMin != value.lodEQCityMax)
				{
					float value2 = value.lodEQCityMin + (value.lodEQCityMax - value.lodEQCityMin) * rate;
					SetMixerGroupEQ(item.Value.Name, value2);
				}
				if (value.lodVolumeCityMin != value.lodVolumeCityMax)
				{
					float num = ReverseCalculateVolume(value.lodVolumeCityMin);
					float num2 = ReverseCalculateVolume(value.lodVolumeCityMax);
					float value3 = num + (num2 - num) * rate;
					SetMixerGroupLODVolume(item.Value.Name, value3);
				}
			}
			else
			{
				if (value.lodEQWorldMin != value.lodEQWorldMax)
				{
					float value4 = value.lodEQWorldMin + (value.lodEQWorldMax - value.lodEQWorldMin) * rate;
					SetMixerGroupEQ(item.Value.Name, value4);
				}
				if (value.lodVolumeWorldMin != value.lodVolumeWorldMax)
				{
					float num3 = ReverseCalculateVolume(value.lodVolumeWorldMin);
					float num4 = ReverseCalculateVolume(value.lodVolumeWorldMax);
					float value5 = num3 + (num4 - num3) * rate;
					SetMixerGroupLODVolume(item.Value.Name, value5);
				}
			}
		}
	}

	private void AdjustMixerGroupIntensityDefault()
	{
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			SetMixerGroupEQ(item.Value.Name, 1f);
			SetMixerGroupLODVolume(item.Value.Name, 1f);
		}
	}

	private float ReverseCalculateVolume(float dbValue)
	{
		if (dbValue <= -80f)
		{
			return 0.0001f;
		}
		return Mathf.Clamp01(Mathf.Pow(10f, dbValue / 20f));
	}

	private void SetMixerGroupEQ(string groupName, float value)
	{
		switch (groupName)
		{
		case "UI_Reward":
			return;
		case "UI_Click":
			return;
		}
		GetSoundGroup(groupName, useAudioMixer: true).SetAudioMixerGroupEQ(value);
	}

	private void SetMixerGroupLODVolume(string groupName, float value)
	{
		GetSoundGroup(groupName, useAudioMixer: true).LodVolumeRatio = value;
	}

	public void SetAMBSoundPause(bool pause)
	{
		if (pause)
		{
			mPauseAMBSound = true;
			return;
		}
		mPauseAMBSound = false;
		if (!mAMBIsPlaying)
		{
			PlayAMBSoundInLoop();
		}
	}

	public void SetBGMVolumeTo0()
	{
		if (IsUseAudioMixer())
		{
			ChangeVolume("MUSIC", 0f, 0.5f, newGroup: true);
		}
		else
		{
			ChangeVolume("Music", 0f, 0.5f, newGroup: false);
		}
	}

	public void ResetMusicVolume()
	{
		if (IsUseAudioMixer())
		{
			ChangeVolume("MUSIC", 1f, 1f, newGroup: true);
		}
		else
		{
			ChangeVolume("Music", 1f, 1f, newGroup: false);
		}
	}

	public void SetAMBSoundVolumeTo0()
	{
		mCurrentAMBSoundVolume = 0f;
		if (IsUseAudioMixer())
		{
			ChangeVolume("AMB", 0f, 0.5f, newGroup: true);
		}
		else
		{
			ChangeVolume("AMBSound", 0f, 0.5f, newGroup: false);
		}
		CancelResetAmbSoundVolumeTimer();
	}

	public void ResetAMBSoundVolumeInternal()
	{
		mCurrentAMBSoundVolume = 1f;
		if (IsUseAudioMixer())
		{
			ChangeVolume("AMB", 1f, 0.5f, newGroup: true);
		}
		else
		{
			ChangeVolume("AMBSound", 1f, 0.5f, newGroup: false);
		}
	}

	public void ResetAMBSoundVolume()
	{
		CancelResetAmbSoundVolumeTimer();
		mResetAMBdelayTimer = GameEntry.Timer.RegisterTimer(1.5f, ResetAMBSoundVolumeInternal);
	}

	public void CancelResetAmbSoundVolumeTimer()
	{
		if (mResetAMBdelayTimer != null)
		{
			GameEntry.Timer.CancelTimer(mResetAMBdelayTimer);
			mResetAMBdelayTimer = null;
		}
	}

	public void TryControlGlobalSound(int serialId, string soundGroupName, float volumeRatio)
	{
		_controlSoundSerialId = serialId;
		_controlSoundGroupName = soundGroupName;
		globalSoundVolumeRatio = volumeRatio;
	}

	public void TryResumeGlobalSoundControl(int serialId)
	{
		if (serialId == _controlSoundSerialId)
		{
			_controlSoundSerialId = -1;
			_controlSoundGroupName = string.Empty;
			globalSoundVolumeRatio = 1f;
		}
	}

	public SoundComponent()
	{
		m_Serial = 1;
		GameObject gameObject = new GameObject("SoundComponent");
		m_InstanceRoot = gameObject.transform;
		m_AudioListener = UnityEngine.Object.FindObjectOfType<AudioListener>();
		if (m_AudioListener == null)
		{
			m_InstanceRoot.gameObject.AddComponent<AudioListener>();
		}
		_audioSourcePool = new AudioSourcePool(gameObject.transform);
	}

	public bool HasSoundGroup(string soundGroupName)
	{
		return m_SoundGroupDic.ContainsKey(soundGroupName);
	}

	public void SetSoundGroupMute(string soundGroupName, bool mute, bool newGroup = false)
	{
		GetSoundGroup(soundGroupName, newGroup).Mute = mute;
		TimelineAudioManager.Inst.ChangeMute(soundGroupName, mute);
	}

	private void CancelBGMFinishTimer()
	{
		if (mBGMFinishTimer != null)
		{
			GameEntry.Timer.CancelTimer(mBGMFinishTimer);
			mBGMFinishTimer = null;
		}
	}

	private void OnBGMPlayFinished()
	{
		if (mCurrentPlayBGMSoundParams != null && mCurrentPlayBGMSoundParams.Loop_Gap >= 0)
		{
			CancelBGMFinishTimer();
			float delaySec = (float)mCurrentPlayBGMSoundParams.Loop_Gap / 1000f;
			mBGMFinishTimer = GameEntry.Timer.RegisterTimer(delaySec, PlayBGMSoundInLoop);
		}
	}

	private void CancelBGMDelayTimer()
	{
		if (mBGMDelayTimer != null)
		{
			GameEntry.Timer.CancelTimer(mBGMDelayTimer);
			mBGMDelayTimer = null;
		}
	}

	private void PlayBGMSoundInDelay()
	{
		string text = mCurrentBGMAssetPath;
		CancelBGMDelayTimer();
		if (!string.IsNullOrEmpty(text))
		{
			_bgMusicId = PlaySound(text, "Music", mCurrentPlayBGMSoundParams, null, 0f, OnBGMPlayFinished);
		}
	}

	public int PlayMusic(string name, bool loop = true, float fadeInSeconds = 0.5f, float startTime = 0f, float volume = 1f, float soundVolumeSet = -1f, float speed = 1f, bool useSoundPath2 = false, int reactive = -1, int loop_gap = -1, int pre_time = -1, List<string> soundPathTable = null)
	{
		string text = "";
		text = ((!useSoundPath2) ? $"Assets/Main/Sound/Music/{name}.ogg" : name);
		Action onFinishCallback = null;
		if (IsUseAudioMixer())
		{
			PlayAudioParams param = new PlayAudioParams
			{
				path = text,
				startTime = startTime,
				loop = 1,
				limit = 1,
				instanceGroupLimit = 1,
				instanceGroupId = 25
			};
			return PlayAudio(param);
		}
		if (loop_gap >= 0)
		{
			onFinishCallback = OnBGMPlayFinished;
		}
		if (reactive == 1 && mCurrentBGMAssetPath != null && mCurrentBGMAssetPath == text)
		{
			return _bgMusicId;
		}
		if (_bgMusicId > 0)
		{
			FadeOutAndPlayMusic(_bgMusicId, fadeInSeconds);
		}
		mCurrentBGMAssetPath = text;
		mCurrentPlayBGMSoundParams = new PlaySoundParams
		{
			Loop = loop,
			FadeInSeconds = fadeInSeconds,
			VolumeInSoundGroup = volume,
			SoundVolumeSet = soundVolumeSet,
			Loop_Gap = loop_gap,
			SoundAssetPaths = soundPathTable,
			Pitch = speed
		};
		if (pre_time > 0)
		{
			CancelBGMDelayTimer();
			float delaySec = (float)pre_time / 1000f;
			mBGMDelayTimer = GameEntry.Timer.RegisterTimer(delaySec, PlayBGMSoundInDelay);
			return -1;
		}
		return PlaySound(text, "Music", mCurrentPlayBGMSoundParams, null, startTime, onFinishCallback);
	}

	private void PlayBGMSoundInLoop()
	{
		string text = null;
		if (mCurrentBGMAssetPath != null)
		{
			text = ((mCurrentPlayBGMSoundParams.SoundAssetPaths == null || mCurrentPlayBGMSoundParams.SoundAssetPaths.Count <= 0) ? mCurrentBGMAssetPath : mCurrentPlayBGMSoundParams.SoundAssetPaths[UnityEngine.Random.Range(0, mCurrentPlayBGMSoundParams.SoundAssetPaths.Count)]);
			CancelBGMFinishTimer();
			if (!string.IsNullOrEmpty(text))
			{
				_bgMusicId = PlaySound(text, "Music", mCurrentPlayBGMSoundParams, null, 0f, OnBGMPlayFinished);
			}
		}
	}

	public int PlayAMBSound(string name, bool useSoundPath2, List<string> soundPathTable, float volume = 1f, int reactive = -1, int loop_gap = -1, int pre_time = -1, float speed = 1f)
	{
		if (string.IsNullOrEmpty(name))
		{
			Log.Error("PlayAMBSound name '{0}' is not exist.", name);
			return -1;
		}
		string text = "";
		text = ((!useSoundPath2) ? $"Assets/Main/Sound/Music/{name}.ogg" : name);
		Action onFinishCallback = null;
		if (loop_gap >= 0)
		{
			onFinishCallback = OnAMBSoundPlayFinished;
		}
		if (reactive == 1 && mCurrentAMBSoundAssetPath != null && mCurrentAMBSoundAssetPath == text)
		{
			return _ambIdSoundId;
		}
		mCurrentPlayAMBSoundParams = new PlaySoundParams
		{
			Loop = false,
			FadeInSeconds = 0f,
			VolumeInSoundGroup = mCurrentAMBSoundVolume,
			SoundVolumeSet = -1f,
			Loop_Gap = loop_gap,
			SoundAssetPaths = soundPathTable,
			Pitch = speed
		};
		mCurrentAMBSoundAssetPath = text;
		if (pre_time > 0)
		{
			CancelAMBSoundDelayTimer();
			float delaySec = (float)pre_time / 1000f;
			mAMBSoundDelayTimer = GameEntry.Timer.RegisterTimer(delaySec, PlayAMBSoundInDelay);
			return -1;
		}
		mAMBIsPlaying = true;
		_ambIdSoundId = PlaySound(text, "AMBSound", mCurrentPlayAMBSoundParams, null, 0f, onFinishCallback);
		return _ambIdSoundId;
	}

	public void OnAMBSoundPlayFinished()
	{
		if (mCurrentPlayAMBSoundParams != null && mCurrentPlayAMBSoundParams.Loop_Gap >= 0)
		{
			CancelAMBSoundFinishTimer();
			float delaySec = (float)mCurrentPlayAMBSoundParams.Loop_Gap / 1000f;
			mAMBIsPlaying = false;
			mAMBSoundTimer = GameEntry.Timer.RegisterTimer(delaySec, PlayAMBSoundInLoop);
		}
	}

	private void CancelAMBSoundFinishTimer()
	{
		if (mAMBSoundTimer != null)
		{
			GameEntry.Timer.CancelTimer(mAMBSoundTimer);
			mAMBSoundTimer = null;
		}
	}

	private void PlayAMBSoundInLoop()
	{
		string text = null;
		if (mCurrentPlayAMBSoundParams == null)
		{
			return;
		}
		text = ((mCurrentPlayAMBSoundParams.SoundAssetPaths == null || mCurrentPlayAMBSoundParams.SoundAssetPaths.Count <= 0) ? mCurrentAMBSoundAssetPath : mCurrentPlayAMBSoundParams.SoundAssetPaths[UnityEngine.Random.Range(0, mCurrentPlayAMBSoundParams.SoundAssetPaths.Count)]);
		CancelAMBSoundFinishTimer();
		if (string.IsNullOrEmpty(mCurrentAMBSoundAssetPath))
		{
			return;
		}
		if (mPauseAMBSound)
		{
			_ = mCurrentPlayAMBSoundParams.Loop_Gap;
			return;
		}
		mCurrentPlayAMBSoundParams.VolumeInSoundGroup = mCurrentAMBSoundVolume;
		mAMBIsPlaying = true;
		if (!string.IsNullOrEmpty(text))
		{
			_ambIdSoundId = PlaySound(text, "AMBSound", mCurrentPlayAMBSoundParams, null, 0f, OnAMBSoundPlayFinished);
		}
	}

	private void CancelAMBSoundDelayTimer()
	{
		if (mAMBSoundDelayTimer != null)
		{
			GameEntry.Timer.CancelTimer(mAMBSoundDelayTimer);
			mAMBSoundDelayTimer = null;
		}
	}

	private void PlayAMBSoundInDelay()
	{
		string text = mCurrentAMBSoundAssetPath;
		CancelAMBSoundFinishTimer();
		mAMBIsPlaying = true;
		mCurrentPlayAMBSoundParams.VolumeInSoundGroup = mCurrentAMBSoundVolume;
		if (!string.IsNullOrEmpty(text))
		{
			_ambIdSoundId = PlaySound(text, "AMBSound", mCurrentPlayAMBSoundParams, null, 0f, OnAMBSoundPlayFinished);
		}
	}

	public int PlayEffectFullPath(string fullPath, float volumeScale = 1f, float soundVolumeSet = -1f)
	{
		if (!IsUseAudioMixer())
		{
			fullPath = SwitchToAudioClipPath(fullPath);
		}
		if (!SoundResourceDownloadManager.Instance.IsCanAsync(fullPath, "Effect"))
		{
			return -1;
		}
		Asset soundAsset = GameEntry.Resource.LoadAssetAsync(fullPath, typeof(UnityEngine.Object));
		if (soundAsset != null)
		{
			int serialId = GetSerial();
			m_SoundsBeingLoaded.Add(serialId, soundAsset);
			Asset asset = soundAsset;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (!soundAsset.isError)
				{
					m_SoundsBeingLoaded.Remove(serialId);
					if (m_SoundsToReleaseOnLoad.Contains(serialId))
					{
						m_SoundsToReleaseOnLoad.Remove(serialId);
						soundAsset.Release();
					}
					else if (IsUseAudioMixer())
					{
						soundAsset.Release();
					}
					else
					{
						GetSoundGroup("Effect").PlayOneShot(serialId, soundAsset, new PlaySoundParams
						{
							VolumeInSoundGroup = 1f,
							ShotVolumeScale = volumeScale,
							SoundVolumeSet = soundVolumeSet
						});
					}
				}
				else
				{
					m_SoundsBeingLoaded.Remove(serialId);
					m_SoundsToReleaseOnLoad.Remove(serialId);
					soundAsset.Release();
				}
			});
			return serialId;
		}
		return -1;
	}

	public int PlayEffectById(int id)
	{
		if (IsUseAudioMixer())
		{
			PlayAudioParams playAudioParamsById = GetPlayAudioParamsById(id);
			return PlayAudio(playAudioParamsById);
		}
		return GameEntry.Lua.CallWithReturn<int, int>("DataCenter.LWSoundManager:PlayEffect", id);
	}

	private string SwitchToAudioSourcePath(string path)
	{
		if (!path.Contains("/prefab/"))
		{
			int num = path.LastIndexOf('/');
			string text = path.Substring(0, num);
			string text2 = path.Substring(num + 1);
			path = text + "/prefab/" + text2;
		}
		return Path.ChangeExtension(path, ".prefab");
	}

	private string SwitchToAudioClipPath(string path)
	{
		if (path.Contains("/prefab/"))
		{
			int num = path.LastIndexOf('/');
			if (num > 0)
			{
				string text = path.Substring(0, num);
				string text2 = path.Substring(num + 1);
				if (text.EndsWith("/prefab"))
				{
					text = text.Substring(0, text.LastIndexOf("/prefab"));
				}
				path = text + "/" + text2;
			}
		}
		return Path.ChangeExtension(path, ".ogg");
	}

	public int PlayEffectCache(string soundAssetName, float volumeScale = 1f, float soundVolumeSet = -1f)
	{
		if (!IsUseAudioMixer())
		{
			soundAssetName = SwitchToAudioClipPath(soundAssetName);
		}
		int serialId = GetSerial();
		if (!_effectAssetCache.TryGetValue(soundAssetName, out var soundAsset))
		{
			if (!SoundResourceDownloadManager.Instance.IsCanAsync(soundAssetName, "Effect"))
			{
				return -1;
			}
			soundAsset = GameEntry.Resource.LoadAssetAsync(soundAssetName, typeof(UnityEngine.Object));
			if (soundAsset != null)
			{
				_effectAssetCache.Add(soundAssetName, soundAsset);
			}
		}
		if (soundAsset != null)
		{
			if (soundAsset.isDone)
			{
				GetSoundGroup("Effect").PlayOneShot(serialId, soundAsset, new PlaySoundParams
				{
					VolumeInSoundGroup = 1f,
					ShotVolumeScale = volumeScale,
					SoundVolumeSet = soundVolumeSet
				}, cacheAsset: true);
				return serialId;
			}
			_soundBeingLoadedCache.Add(serialId);
			Asset asset = soundAsset;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (!soundAsset.isError)
				{
					bool flag = _soundBeingLoadedCache.Remove(serialId);
					if (m_SoundsToReleaseOnLoad.Contains(serialId))
					{
						m_SoundsToReleaseOnLoad.Remove(serialId);
						if (!flag)
						{
							soundAsset.Release();
						}
					}
					else if (IsUseAudioMixer())
					{
						soundAsset.Release();
					}
					else
					{
						GetSoundGroup("Effect").PlayOneShot(serialId, soundAsset, new PlaySoundParams
						{
							VolumeInSoundGroup = 1f,
							ShotVolumeScale = volumeScale,
							SoundVolumeSet = soundVolumeSet
						}, cacheAsset: true);
					}
				}
				else
				{
					bool num = _soundBeingLoadedCache.Remove(serialId);
					m_SoundsToReleaseOnLoad.Remove(serialId);
					if (!num)
					{
						soundAsset.Release();
					}
				}
			});
			return serialId;
		}
		return -1;
	}

	public bool ReleaseEffect(string name)
	{
		if (_effectAssetCache.TryGetValue(name, out var value))
		{
			value?.Release();
			_effectAssetCache.Remove(name);
			return true;
		}
		return false;
	}

	public void PlayBGMWithDspTime(string strMusicPath, bool loop, float fadeIn, Func<float> getDspTimeFunc, Action onMusicLoadFinish = null)
	{
		SoundGroup soundGroup = null;
		if (!IsUseAudioMixer())
		{
			strMusicPath = SwitchToAudioClipPath(strMusicPath);
			soundGroup = GetSoundGroup("Music");
		}
		if (_bgMusicId > 0)
		{
			StopSound(_bgMusicId);
		}
		int serialId = GetSerial();
		_bgMusicId = serialId;
		Asset req = GameEntry.Resource.LoadAssetAsync(strMusicPath, typeof(UnityEngine.Object));
		if (req == null)
		{
			return;
		}
		m_SoundsBeingLoaded.Add(serialId, req);
		Asset asset = req;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			bool flag = IsUseAudioMixer();
			if (flag)
			{
				m_SoundsBeingLoaded.Remove(serialId);
				m_SoundsToReleaseOnLoad.Remove(serialId);
				req.Release();
			}
			else
			{
				PlaySoundParams playSoundParams = new PlaySoundParams
				{
					Loop = loop,
					FadeInSeconds = fadeIn,
					VolumeInSoundGroup = 1f,
					SoundVolumeSet = -1f
				};
				if (req.isError)
				{
					LoadSoundFailure(strMusicPath, req, new PlaySoundInfo(serialId, soundGroup, playSoundParams, null));
					if (onMusicLoadFinish != null)
					{
						onMusicLoadFinish();
					}
				}
				else
				{
					float dspTime = 0f;
					if (getDspTimeFunc != null)
					{
						dspTime = getDspTimeFunc();
					}
					LoadSoundSuccess(strMusicPath, req, 0f, new PlaySoundInfo(serialId, soundGroup, playSoundParams, null), 0f, dspTime, null, flag);
					if (onMusicLoadFinish != null && req.asset as AudioClip != null)
					{
						onMusicLoadFinish();
					}
				}
			}
		});
	}

	public void PlayBGMusicByNameWithStartTime(string nameStr, float startTime, bool isloop, float volume = 1f, float soundVolumeSet = -1f, float fadeTime = 0.5f, float speed = 1f, bool usePath2 = false, int reactive = -1, int loop_gap = -1, int pre_time = -1)
	{
		_bgMusicId = PlayMusic(nameStr, isloop, fadeTime, startTime, volume, soundVolumeSet, speed, usePath2, reactive, loop_gap, pre_time);
	}

	public void PlayLoadingBgMusic()
	{
		bool flag = false;
		long num = PlayerPrefs.GetString("SeasonEndTime", "0").ToLong();
		bool flag2 = PlayerPrefs.GetInt("USE_SEASON_BGM", 1) == 1;
		if (num > 0 && GameEntry.Timer.GetServerTime() < num && flag2)
		{
			SeasonType seasonType = (SeasonType)PlayerPrefs.GetInt("SEASON_MAP_TYPE", 0);
			DownloadMode downloadMode = DownloadMode.Base;
			switch (seasonType)
			{
			case SeasonType.CityStronghold:
				downloadMode = DownloadMode.Season2;
				break;
			case SeasonType.Snow:
				downloadMode = DownloadMode.Season3;
				break;
			case SeasonType.Mummy:
				downloadMode = DownloadMode.Season4;
				break;
			case SeasonType.Darkness:
				downloadMode = DownloadMode.Season5;
				break;
			case SeasonType.NineNation:
				switch (PlayerPrefs.GetInt("SEASON_MAP_TYPE2", 0))
				{
				case 0:
					downloadMode = DownloadMode.Season6;
					break;
				case 1:
					downloadMode = DownloadMode.Season7;
					break;
				case 2:
					downloadMode = DownloadMode.Season8;
					break;
				}
				break;
			case SeasonType.NineNationRainforest:
				downloadMode = DownloadMode.Season7;
				break;
			}
			if (downloadMode != DownloadMode.Base && ResourcePackageManager.IsPackageDownloaded((int)downloadMode))
			{
				flag = TryPlayLoadingSeasonBGMMusic();
			}
		}
		if (!flag)
		{
			if (_bgMusicId > 0)
			{
				FadeOutAndPlayMusic(_bgMusicId, 1f);
			}
			bool num2 = GameEntry.Setting.HasSetting("LOADING_DEFAULT_BGM");
			string text = string.Format("Assets/Main/Sound/Music/{0}.ogg", "bgm_base_day_01");
			if (!num2)
			{
				GameEntry.Setting.SetString("LOADING_DEFAULT_BGM", text);
			}
			string name = GameEntry.Setting.GetString("LOADING_DEFAULT_BGM", text);
			_bgMusicId = PlayMusic(name, loop: true, 0.5f, 0f, 1f, -1f, 1f, useSoundPath2: true);
		}
	}

	private bool TryPlayLoadingSeasonBGMMusic()
	{
		string text = PlayerPrefs.GetString("SeasonBGM", "");
		if (!text.IsNullOrEmpty())
		{
			string[] array = text.Split(new char[1] { '|' });
			if (array.Length > 1)
			{
				if (array[0] == "1")
				{
					if (array.Length == 6)
					{
						string s = array[1];
						string text2 = array[2];
						_ = array[3];
						string s2 = array[4];
						string text3 = array[5];
						int.Parse(s);
						int loop_gap = -1;
						if (!string.IsNullOrEmpty(text3) && int.TryParse(text3, out var result))
						{
							loop_gap = result;
						}
						string text4 = string.Empty;
						string[] array2 = text2.Split(new char[1] { ';' });
						if (array2.Length > 1)
						{
							if (array2.Length != 0)
							{
								text4 = array2[UnityEngine.Random.Range(0, array2.Length - 1)];
							}
						}
						else
						{
							if (array2.Length != 1)
							{
								return false;
							}
							text4 = text2;
						}
						int.TryParse(s2, out var result2);
						if (_bgMusicId > 0)
						{
							FadeOutAndPlayMusic(_bgMusicId, 1f);
						}
						if (GameEntry.Resource.HasAsset(text4))
						{
							_bgMusicId = PlayMusic(text4, loop: true, 0.5f, 0f, 1f, -1f, 1f, useSoundPath2: true, result2, loop_gap, 0);
						}
						else
						{
							_bgMusicId = PlayMusic("bgm_base_day_01");
						}
						return true;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void StopBGMusic()
	{
		if (IsUseAudioMixer())
		{
			GetSoundGroup("MUSIC", useAudioMixer: true).StopAllAudioSource();
		}
		else
		{
			if (_bgMusicId > 0)
			{
				FadeOutAndPlayMusic(_bgMusicId, 1f);
				_bgMusicId = 0;
			}
			mCurrentBGMAssetPath = null;
		}
		StopAMBSound();
	}

	public void StopAMBSound()
	{
		if (IsUseAudioMixer())
		{
			GetSoundGroup("AMB", useAudioMixer: true).StopAllAudioSource();
		}
		else if (_ambIdSoundId > 0)
		{
			FadeOutAndPlayMusic(_ambIdSoundId, 0.5f);
			_ambIdSoundId = 0;
		}
	}

	public int GetBGMusic()
	{
		return _bgMusicId;
	}

	public void OnUpdate(float elapseSeconds)
	{
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			item.Value.OnUpdate(elapseSeconds);
		}
		_audioSourcePool.Update();
		if (_randomKeepList.Count > 0)
		{
			for (int num = _randomKeepList.Count - 1; num > -1; num--)
			{
				PlayAudioParams playAudioParams = _randomKeepList[num];
				if (playAudioParams.randomKeepTime > 0f)
				{
					playAudioParams.randomKeepTime -= elapseSeconds;
				}
				else
				{
					_randomKeepList.RemoveAt(num);
					_randomMap.Remove(playAudioParams.id);
				}
			}
		}
		if (!IsUseAudioMixer())
		{
			return;
		}
		if (SceneManager.IsInCity() || SceneManager.IsInWorld())
		{
			SceneInterface world = SceneManager.World;
			if (world == null)
			{
				return;
			}
			float num2 = (SceneManager.IsInCity() ? _cityZoomMin : _worldZoomMin);
			float num3 = (SceneManager.IsInCity() ? _cityZoomMax : _worldZoomMax);
			float num4 = Mathf.Max(world.Zoom, num2);
			if (num2 != num3)
			{
				float num5 = Mathf.Clamp01(1f - (num4 - num2) / (num3 - num2));
				if (_audioLodRate != num5)
				{
					_audioLodRate = num5;
					AdjustMixerGroupIntensity(num5, SceneManager.IsInWorld());
				}
			}
		}
		else
		{
			AdjustMixerGroupIntensityDefault();
		}
	}

	public void Setup3DAudioZoomRange(float cityZoomMin, float cityZoomMax, float worldZoomMin, float worldZoomMax)
	{
		_cityZoomMin = cityZoomMin;
		_cityZoomMax = cityZoomMax;
		_worldZoomMin = worldZoomMin;
		_worldZoomMax = worldZoomMax;
	}

	public bool StopSound(int serialId)
	{
		if (m_SoundsBeingLoaded.ContainsKey(serialId))
		{
			m_SoundsToReleaseOnLoad.Add(serialId);
			return true;
		}
		if (_soundBeingLoadedCache.Contains(serialId))
		{
			m_SoundsToReleaseOnLoad.Add(serialId);
			return true;
		}
		if (!IsUseAudioMixer())
		{
			foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
			{
				if (item.Value != null && item.Value.SerialId == serialId)
				{
					item.Value.StopSound();
					break;
				}
			}
			if (serialId == _bgMusicId)
			{
				mCurrentBGMAssetPath = null;
			}
			else if (serialId == _ambIdSoundId)
			{
				mAMBIsPlaying = false;
				mCurrentAMBSoundAssetPath = null;
			}
		}
		else
		{
			using Dictionary<string, SoundGroup>.Enumerator enumerator = m_SoundGroupDic.GetEnumerator();
			while (enumerator.MoveNext() && !enumerator.Current.Value.StopSoundBySerialId(serialId))
			{
			}
		}
		return false;
	}

	public bool FadeOutAndPlayMusic(int serialId, float time)
	{
		if (m_SoundsBeingLoaded.ContainsKey(serialId))
		{
			m_SoundsToReleaseOnLoad.Add(serialId);
			return true;
		}
		if (_soundBeingLoadedCache.Contains(serialId))
		{
			m_SoundsToReleaseOnLoad.Add(serialId);
			return true;
		}
		if (IsUseAudioMixer())
		{
			foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
			{
				if (item.Value != null && item.Value.HasSerialId(serialId))
				{
					item.Value.StopAllAudioSource();
					return true;
				}
			}
		}
		else
		{
			if (serialId == _bgMusicId)
			{
				mCurrentBGMAssetPath = null;
			}
			else if (serialId == _ambIdSoundId)
			{
				mAMBIsPlaying = false;
				mCurrentAMBSoundAssetPath = null;
			}
			foreach (KeyValuePair<string, SoundGroup> item2 in m_SoundGroupDic)
			{
				if (item2.Value != null && item2.Value.SerialId == serialId)
				{
					item2.Value.FadeOutAndPlaySound(time);
					return true;
				}
			}
		}
		return false;
	}

	public void StopAllSounds()
	{
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			item.Value.StopSound();
		}
		foreach (KeyValuePair<int, Asset> item2 in m_SoundsBeingLoaded)
		{
			m_SoundsToReleaseOnLoad.Add(item2.Key);
		}
		_soundBeingLoadedCache.Clear();
		foreach (KeyValuePair<int, string> item3 in _preloadedPathBySoundId)
		{
			string value = item3.Value;
			if (_assetMap.ContainsKey(value) && (!_audioRefMap.ContainsKey(value) || _audioRefMap[value].Count == 0))
			{
				_assetMap[value].Release();
				_assetMap.Remove(value);
				_audioRefMap.Remove(value);
			}
		}
		_preloadedPathBySoundId.Clear();
		_preloadSerialBySoundId.Clear();
		foreach (KeyValuePair<string, Asset> item4 in _effectAssetCache)
		{
			Asset value2 = item4.Value;
			if (value2.isDone)
			{
				value2.Release();
			}
		}
		_effectAssetCache.Clear();
		mCurrentBGMAssetPath = null;
		mAMBIsPlaying = false;
		mCurrentAMBSoundAssetPath = null;
	}

	public void PauseSound(int serialId)
	{
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			if (item.Value.SerialId == serialId || item.Value.HasSerialId(serialId))
			{
				item.Value.PauseSound();
			}
		}
	}

	public void ResumeSound(int serialId)
	{
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			if (item.Value.SerialId == serialId || item.Value.HasSerialId(serialId))
			{
				item.Value.ResumeSound();
			}
		}
	}

	private int GetSerial()
	{
		if (++m_Serial == int.MaxValue)
		{
			m_Serial = 1;
		}
		return m_Serial;
	}

	public int PlaySound(string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams, object userData, float startTime = 0f, Action _onFinishCallback = null)
	{
		int serialId = GetSerial();
		if (playSoundParams == null)
		{
			playSoundParams = new PlaySoundParams();
		}
		PlaySoundErrorCode? playSoundErrorCode = null;
		SoundGroup soundGroup = null;
		string text = null;
		if (IsUseAudioMixer())
		{
			soundAssetName = SwitchToAudioSourcePath(soundAssetName);
		}
		else
		{
			soundAssetName = SwitchToAudioClipPath(soundAssetName);
			soundGroup = GetSoundGroup(soundGroupName, IsUseAudioMixer());
			if (soundGroup == null)
			{
				Log.Error("Sound group '{0}' is not exist.", soundGroupName);
				playSoundErrorCode = PlaySoundErrorCode.SoundGroupNotExist;
				text = $"Sound group '{soundGroupName}' is not exist.";
				if (playSoundErrorCode.HasValue)
				{
					throw new GameFrameworkException(text);
				}
			}
		}
		bool useAudioMixer = IsUseAudioMixer();
		if (!SoundResourceDownloadManager.Instance.IsCanAsync(soundAssetName, soundGroupName))
		{
			return -1;
		}
		Asset req = GameEntry.Resource.LoadAssetAsync(soundAssetName, typeof(UnityEngine.Object));
		if (req != null)
		{
			m_SoundsBeingLoaded.Add(serialId, req);
			Asset asset = req;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (useAudioMixer != IsUseAudioMixer())
				{
					m_SoundsBeingLoaded.Remove(serialId);
					m_SoundsToReleaseOnLoad.Remove(serialId);
					req.Release();
				}
				else if (!req.isError)
				{
					LoadSoundSuccess(soundAssetName, req, 0f, new PlaySoundInfo(serialId, soundGroup, playSoundParams, userData), startTime, -1f, _onFinishCallback, useAudioMixer);
				}
				else
				{
					LoadSoundFailure(soundAssetName, req, new PlaySoundInfo(serialId, soundGroup, playSoundParams, userData));
				}
			});
		}
		return serialId;
	}

	public int PlayAudio(PlayAudioParams param)
	{
		int serialId = GetSerial();
		string text;
		if (param.id > 0 && _preloadedPathBySoundId.TryGetValue(param.id, out var value))
		{
			text = value;
			_preloadedPathBySoundId.Remove(param.id);
			if (_preloadSerialBySoundId.TryGetValue(param.id, out var value2))
			{
				m_SoundsToReleaseOnLoad.Add(value2);
				_preloadSerialBySoundId.Remove(param.id);
			}
		}
		else
		{
			GetRandomPath(param);
			text = SwitchToAudioSourcePath(param.usePath);
		}
		param.usePath = text;
		param.loadStartTime = Time.realtimeSinceStartup;
		if (param.instanceGroupId == 25)
		{
			if (param.reactive == 1f && GetSoundGroup("MUSIC", useAudioMixer: true).IsSoundPlaying(param.usePath))
			{
				return _bgMusicId;
			}
			_bgMusicId = serialId;
		}
		if (param.instanceGroupId == 22)
		{
			if (param.reactive == 1f && GetSoundGroup("AMB", useAudioMixer: true).IsSoundPlaying(param.usePath))
			{
				return _ambIdSoundId;
			}
			_ambIdSoundId = serialId;
		}
		if (_assetMap.ContainsKey(text))
		{
			if (param.getDspTimeFunc != null)
			{
				param.dspTime = param.getDspTimeFunc();
				param.getDspTimeFunc = null;
			}
			LoadAudioSuccess(serialId, param, _assetMap[text]);
			param.onMusicLoadFinish?.Invoke();
			param.onMusicLoadFinish = null;
		}
		else
		{
			bool useAudioMixer = IsUseAudioMixer();
			Asset req = GameEntry.Resource.LoadAssetAsync(text, typeof(UnityEngine.Object));
			if (req != null)
			{
				m_SoundsBeingLoaded.Add(serialId, req);
				Asset asset = req;
				asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
				{
					if (useAudioMixer != IsUseAudioMixer())
					{
						m_SoundsBeingLoaded.Remove(serialId);
						m_SoundsToReleaseOnLoad.Remove(serialId);
						req.Release();
					}
					else if (!req.isError)
					{
						if (param.getDspTimeFunc != null)
						{
							param.dspTime = param.getDspTimeFunc();
							param.getDspTimeFunc = null;
						}
						LoadAudioSuccess(serialId, param, req);
						param.onMusicLoadFinish?.Invoke();
						param.onMusicLoadFinish = null;
					}
					else
					{
						LoadAudioFailed(serialId, param, req);
						param.onMusicLoadFinish?.Invoke();
						param.onMusicLoadFinish = null;
					}
				});
			}
		}
		return serialId;
	}

	public void PreloadAudioById(int soundId)
	{
		if (soundId <= 0 || _preloadedPathBySoundId.ContainsKey(soundId))
		{
			return;
		}
		PlayAudioParams playAudioParamsById = GetPlayAudioParamsById(soundId);
		if (playAudioParamsById == null || string.IsNullOrEmpty(playAudioParamsById.path))
		{
			return;
		}
		if (playAudioParamsById.randomType == 1 || playAudioParamsById.randomType == 2)
		{
			Log.Warning("[AudioMixer]randomType 不支持预加载");
			return;
		}
		GetRandomPath(playAudioParamsById);
		string path = SwitchToAudioSourcePath(playAudioParamsById.usePath);
		_preloadedPathBySoundId[soundId] = path;
		if (_assetMap.ContainsKey(path))
		{
			return;
		}
		Asset req = GameEntry.Resource.LoadAssetAsync(path, typeof(UnityEngine.Object));
		if (req == null)
		{
			return;
		}
		int serialId = GetSerial();
		_preloadSerialBySoundId[soundId] = serialId;
		m_SoundsBeingLoaded[serialId] = req;
		Asset asset = req;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			m_SoundsBeingLoaded.Remove(serialId);
			int value;
			bool flag = !_preloadSerialBySoundId.TryGetValue(soundId, out value) || value == serialId;
			if (flag)
			{
				_preloadSerialBySoundId.Remove(soundId);
			}
			if (m_SoundsToReleaseOnLoad.Contains(serialId))
			{
				m_SoundsToReleaseOnLoad.Remove(serialId);
				if (flag)
				{
					_preloadedPathBySoundId.Remove(soundId);
				}
				req.Release();
			}
			else if (!req.isError)
			{
				if (!_assetMap.ContainsKey(path))
				{
					TryAddAudioSourceAsset(req);
				}
				else
				{
					req.Release();
				}
			}
			else if (flag)
			{
				_preloadedPathBySoundId.Remove(soundId);
			}
		});
	}

	public void ReleasePreloadedAudioById(int soundId)
	{
		if (!_preloadedPathBySoundId.TryGetValue(soundId, out var value))
		{
			return;
		}
		if (_preloadSerialBySoundId.TryGetValue(soundId, out var value2))
		{
			m_SoundsToReleaseOnLoad.Add(value2);
			_preloadSerialBySoundId.Remove(soundId);
			_preloadedPathBySoundId.Remove(soundId);
			return;
		}
		_preloadedPathBySoundId.Remove(soundId);
		if (_assetMap.ContainsKey(value) && (!_audioRefMap.ContainsKey(value) || _audioRefMap[value].Count <= 0))
		{
			_assetMap[value].Release();
			_assetMap.Remove(value);
			_audioRefMap.Remove(value);
		}
	}

	private void LoadAudioSuccess(int serialId, PlayAudioParams param, Asset asset)
	{
		m_SoundsBeingLoaded.Remove(serialId);
		if (m_SoundsToReleaseOnLoad.Contains(serialId))
		{
			m_SoundsToReleaseOnLoad.Remove(serialId);
			asset.Release();
			return;
		}
		GameObject gameObject = asset.asset as GameObject;
		if (gameObject != null)
		{
			AudioSource component = gameObject.GetComponent<AudioSource>();
			if (component.clip == null)
			{
				asset.Release();
				Log.Error("AudioSource: {0} clip is null.", param.path);
				return;
			}
			if (component.outputAudioMixerGroup == null)
			{
				asset.Release();
				Log.Error("AudioSource: {0} outputAudioMixerGroup is null.", param.path);
				return;
			}
			SoundGroup soundGroup = GetSoundGroup(component.outputAudioMixerGroup.name, useAudioMixer: true);
			if ((soundGroup.Name == "MUSIC" || soundGroup.Name == "AMB") && !soundGroup.IsNotExpired(param.loadStartTime))
			{
				asset.Release();
				return;
			}
			TryAddAudioSourceAsset(asset);
			if (soundGroup.Name == "MUSIC" || soundGroup.Name == "AMB")
			{
				if (soundGroup.GetAudioSources().Count > 0)
				{
					param.crossFadeInTime = 0.03f;
				}
				if (param.loop == 1 && param.loopGap == 0f)
				{
					param.loopCrossFade = 0.5f;
				}
			}
			else if (soundGroup.Name == "VO" && param.loop == 1 && param.loopGap == 0f)
			{
				param.loopCrossFade = 0.03f;
			}
			soundGroup.PlayAudioSource(serialId, param, asset);
		}
		else
		{
			asset.Release();
			Log.Error("AudioSource: {0} is not a gameObject.", param.path);
		}
	}

	private void LoadAudioFailed(int serialId, PlayAudioParams param, Asset asset)
	{
		m_SoundsBeingLoaded.Remove(serialId);
		m_SoundsToReleaseOnLoad.Remove(serialId);
		Log.Error($"Load sound failure, asset name '{param.path}', error message '{asset.error}'.");
		asset.Release();
	}

	public void RandomSoundEndCheck(PlayAudioParams playAudioParams)
	{
		if (!_randomMap.ContainsKey(playAudioParams.id))
		{
			return;
		}
		PlayAudioParams playAudioParams2 = _randomMap[playAudioParams.id];
		if (playAudioParams2 == playAudioParams)
		{
			playAudioParams2.randomKeepTime = 3f;
			if (!_randomKeepList.Contains(playAudioParams2))
			{
				_randomKeepList.Add(playAudioParams2);
			}
		}
	}

	private void GetRandomPath(PlayAudioParams playAudioParams)
	{
		playAudioParams.usePath = playAudioParams.path;
		if (playAudioParams.randomType == 1 || playAudioParams.randomType == 2)
		{
			if (_randomMap.ContainsKey(playAudioParams.id))
			{
				PlayAudioParams playAudioParams2 = _randomMap[playAudioParams.id];
				if (_randomKeepList.Contains(playAudioParams2))
				{
					_randomKeepList.Remove(playAudioParams2);
				}
				playAudioParams.pathIndex = playAudioParams2.pathIndex;
				playAudioParams.pathIndex++;
				playAudioParams.pathArr = playAudioParams2.pathArr;
				if (playAudioParams.pathIndex == playAudioParams2.pathArr.Length)
				{
					playAudioParams.pathIndex = 0;
					if (playAudioParams.randomType == 2)
					{
						for (int i = 0; i < playAudioParams2.pathArr.Length; i++)
						{
							int num = UnityEngine.Random.Range(i, playAudioParams2.pathArr.Length);
							ref string reference = ref playAudioParams2.pathArr[i];
							ref string reference2 = ref playAudioParams2.pathArr[num];
							string text = playAudioParams2.pathArr[num];
							string text2 = playAudioParams2.pathArr[i];
							reference = text;
							reference2 = text2;
						}
					}
				}
				_randomMap[playAudioParams.id] = playAudioParams;
				playAudioParams.usePath = playAudioParams.pathArr[playAudioParams.pathIndex];
				return;
			}
			if (playAudioParams.path.Contains(";") && playAudioParams.pathArr == null)
			{
				playAudioParams.pathArr = playAudioParams.path.Split(new char[1] { ';' });
			}
			if (playAudioParams.pathArr == null || playAudioParams.pathArr.Length == 0)
			{
				return;
			}
			if (playAudioParams.pathIndex == playAudioParams.pathArr.Length)
			{
				playAudioParams.pathIndex = 0;
			}
			if (playAudioParams.randomType == 1)
			{
				playAudioParams.usePath = playAudioParams.pathArr[playAudioParams.pathIndex];
			}
			else if (playAudioParams.randomType == 2)
			{
				if (playAudioParams.pathIndex == 0)
				{
					for (int j = 0; j < playAudioParams.pathArr.Length; j++)
					{
						int num2 = UnityEngine.Random.Range(j, playAudioParams.pathArr.Length);
						ref string reference = ref playAudioParams.pathArr[j];
						ref string reference3 = ref playAudioParams.pathArr[num2];
						string text2 = playAudioParams.pathArr[num2];
						string text = playAudioParams.pathArr[j];
						reference = text2;
						reference3 = text;
					}
				}
				playAudioParams.usePath = playAudioParams.pathArr[playAudioParams.pathIndex];
			}
			_randomMap.Add(playAudioParams.id, playAudioParams);
		}
		else
		{
			if (playAudioParams.path.Contains(";") && playAudioParams.pathArr == null)
			{
				playAudioParams.pathArr = playAudioParams.path.Split(new char[1] { ';' });
			}
			if (playAudioParams.pathArr != null && playAudioParams.pathArr.Length != 0)
			{
				int num3 = UnityEngine.Random.Range(0, playAudioParams.pathArr.Length);
				playAudioParams.usePath = playAudioParams.pathArr[num3];
			}
		}
	}

	public void SetupSoundGroup(string soundGroupName, int menuOption, float lodVolumeCityMin = 0f, float lodVolumeCityMax = 0f, float lodEQCityMin = 0f, float lodEQCityMax = 0f, float lodVolumeWorldMin = 0f, float lodVolumeWorldMax = 0f, float lodEQWorldMin = 0f, float lodEQWorldMax = 0f)
	{
		SoundGroup soundGroup = GetSoundGroup(soundGroupName, useAudioMixer: true);
		TryToSetupAudioMixer(soundGroup);
		soundGroup.lodVolumeCityMin = lodVolumeCityMin;
		soundGroup.lodVolumeCityMax = lodVolumeCityMax;
		soundGroup.lodEQCityMin = lodEQCityMin;
		soundGroup.lodEQCityMax = lodEQCityMax;
		soundGroup.lodVolumeWorldMin = lodVolumeWorldMin;
		soundGroup.lodVolumeWorldMax = lodVolumeWorldMax;
		soundGroup.lodEQWorldMin = lodEQWorldMin;
		soundGroup.lodEQWorldMax = lodEQWorldMax;
		soundGroup.isEff = menuOption == 1;
		if (soundGroup.isEff)
		{
			bool mute = !GameEntry.Setting.GetBool("isEffectMusicOn");
			soundGroup.Mute = mute;
		}
		else
		{
			bool mute2 = !GameEntry.Setting.GetBool("isBGMusicOn");
			soundGroup.Mute = mute2;
		}
	}

	private SoundGroup GetSoundGroup(string soundGroupName, bool useAudioMixer = false)
	{
		SoundGroup value = null;
		if (m_SoundGroupDic.TryGetValue(soundGroupName, out value))
		{
			if (useAudioMixer)
			{
				TryToSetupAudioMixer(value);
			}
			return value;
		}
		value = new SoundGroup(soundGroupName, useAudioMixer);
		if (useAudioMixer)
		{
			TryToSetupAudioMixer(value);
		}
		value.AudioSourceTransform.SetParent(m_InstanceRoot);
		m_SoundGroupDic.Add(soundGroupName, value);
		if (useAudioMixer)
		{
			if (!HasMixerGroup_Str(soundGroupName))
			{
				Log.Error("[AudioMixer]Group not find  : " + soundGroupName);
			}
		}
		else if (HasMixerGroup_Str(soundGroupName))
		{
			Log.Error("[AudioMixer]Group wrong : " + soundGroupName);
		}
		return value;
	}

	private void TryToSetupAudioMixer(SoundGroup soundGroup)
	{
		if (_audioMixer != null)
		{
			AudioMixerGroup[] array = _audioMixer.FindMatchingGroups(soundGroup.Name);
			if (array != null && array.Length != 0 && soundGroup.MixerGroup != array[0])
			{
				soundGroup.MixerGroup = array[0];
				soundGroup.isEff = GameEntry.Lua.CallWithReturn<bool, string>("CSharpCallLuaInterface.IsSoundEffectGroup", soundGroup.Name);
				soundGroup.isAmb = GameEntry.Lua.CallWithReturn<bool, string>("CSharpCallLuaInterface.IsEvnSoundEffectGroup", soundGroup.Name);
				Log.Info("[AudioMixer]ResetMixerGroup:" + soundGroup.Name);
			}
		}
		if (soundGroup.isEff)
		{
			soundGroup.Mute = !GameEntry.Setting.GetBool("isEffectMusicOn");
		}
		else if (soundGroup.isAmb)
		{
			soundGroup.Mute = !GameEntry.Setting.GetBool("ENV_SOUND_ON");
		}
		else
		{
			soundGroup.Mute = !GameEntry.Setting.GetBool("isBGMusicOn");
		}
	}

	private void LoadSoundSuccess(string soundAssetName, Asset soundAsset, float duration, object userData, float startTime = 0f, float dspTime = -1f, Action _onFinishCallback = null, bool useAudioMixer = false)
	{
		PlaySoundInfo playSoundInfo = (PlaySoundInfo)userData;
		if (playSoundInfo == null)
		{
			Debug.LogError("Play sound info is invalid.");
		}
		m_SoundsBeingLoaded.Remove(playSoundInfo.SerialId);
		if (m_SoundsToReleaseOnLoad.Contains(playSoundInfo.SerialId))
		{
			m_SoundsToReleaseOnLoad.Remove(playSoundInfo.SerialId);
			soundAsset.Release();
		}
		else if (useAudioMixer != IsUseAudioMixer())
		{
			soundAsset.Release();
		}
		else if (playSoundInfo.SoundGroup != null)
		{
			playSoundInfo.SoundGroup.PlaySound(playSoundInfo.SerialId, soundAsset, playSoundInfo.PlaySoundParams, startTime, dspTime, _onFinishCallback);
		}
		else
		{
			soundAsset.Release();
			Log.Error(soundAssetName + " -> Sound Group Can not Find!");
		}
	}

	private void LoadSoundFailure(string soundAssetName, Asset soundAsset, object userData)
	{
		PlaySoundInfo playSoundInfo = (PlaySoundInfo)userData;
		if (playSoundInfo == null)
		{
			Log.Error("Play sound info is invalid.");
		}
		m_SoundsBeingLoaded.Remove(playSoundInfo.SerialId);
		m_SoundsToReleaseOnLoad.Remove(playSoundInfo.SerialId);
		Log.Error($"Load sound failure, asset name '{soundAssetName}', error message '{soundAsset.error}'.");
		soundAsset.Release();
	}

	public void ChangeVolume(string soundGroupName, float toVolume, float time, bool newGroup)
	{
		SoundGroup soundGroup = GetSoundGroup(soundGroupName, newGroup);
		if (soundGroup != null)
		{
			if (time <= 0f)
			{
				soundGroup.Volume = toVolume;
			}
			else
			{
				soundGroup.ChangeVolume(toVolume, time);
			}
		}
	}

	public void ChangeGlobalSettingVolumeRatio(string soundGroupName, float toVolume, bool newGroup)
	{
		SoundGroup soundGroup = GetSoundGroup(soundGroupName, newGroup);
		if (soundGroup != null)
		{
			soundGroup.GlobalSettingVolumeRatio = toVolume;
			TimelineAudioManager.Inst.ChangeVolume(soundGroupName, toVolume);
		}
	}

	public static PlayAudioParams GetPlayAudioParamsById(int soundId)
	{
		PlayAudioParams playAudioParams = new PlayAudioParams();
		playAudioParams.id = soundId;
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "audiosource");
		if (!string.IsNullOrEmpty(templateData))
		{
			playAudioParams.path = templateData;
		}
		string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound_delay_play");
		if (!string.IsNullOrEmpty(templateData2))
		{
			string[] array = templateData2.Split(new char[1] { ',' });
			if (float.TryParse(array[0], out var result))
			{
				playAudioParams.delayMin = result;
				playAudioParams.delayMax = result;
				if (array.Length > 1 && float.TryParse(array[1], out var result2))
				{
					playAudioParams.delayMax = result2;
				}
				if (playAudioParams.delayMin > playAudioParams.delayMax)
				{
					PlayAudioParams playAudioParams2 = playAudioParams;
					float delayMax = playAudioParams.delayMax;
					float delayMin = playAudioParams.delayMin;
					playAudioParams.delayMin = delayMax;
					playAudioParams2.delayMax = delayMin;
				}
			}
		}
		string templateData3 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "loop");
		if (!string.IsNullOrEmpty(templateData3))
		{
			playAudioParams.loop = int.Parse(templateData3);
		}
		string templateData4 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "reactive");
		if (!string.IsNullOrEmpty(templateData4))
		{
			playAudioParams.reactive = float.Parse(templateData4);
		}
		string templateData5 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "fade_in");
		if (!string.IsNullOrEmpty(templateData5))
		{
			playAudioParams.fadeIn = float.Parse(templateData5);
		}
		string templateData6 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "fade_out");
		if (!string.IsNullOrEmpty(templateData6))
		{
			playAudioParams.fadeOut = float.Parse(templateData6);
		}
		string templateData7 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "loop_end_gap");
		if (!string.IsNullOrEmpty(templateData7))
		{
			playAudioParams.loopGap = float.Parse(templateData7);
		}
		string templateData8 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound_instance_limit");
		if (!string.IsNullOrEmpty(templateData8))
		{
			playAudioParams.limit = int.Parse(templateData8);
		}
		playAudioParams.whenEqual = -1;
		string templateData9 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "when_priority_equal");
		if (!string.IsNullOrEmpty(templateData9))
		{
			playAudioParams.whenEqual = int.Parse(templateData9);
		}
		string templateData10 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "random");
		if (!string.IsNullOrEmpty(templateData10))
		{
			playAudioParams.randomType = int.Parse(templateData10);
		}
		string templateData11 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "random_pitch");
		playAudioParams.randomPitchMin = -1f;
		playAudioParams.randomPitchMax = -1f;
		if (!string.IsNullOrEmpty(templateData11))
		{
			string[] array2 = templateData11.Split(new char[1] { ',' });
			if (array2.Length == 1)
			{
				playAudioParams.randomPitchMin = float.Parse(array2[0]);
				playAudioParams.randomPitchMax = float.Parse(array2[0]);
			}
			else
			{
				playAudioParams.randomPitchMin = float.Parse(array2[0]);
				playAudioParams.randomPitchMax = float.Parse(array2[1]);
			}
		}
		string templateData12 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "random_volume");
		playAudioParams.randomVolumeMin = -1f;
		playAudioParams.randomVolumeMax = -1f;
		if (!string.IsNullOrEmpty(templateData12))
		{
			string[] array3 = templateData12.Split(new char[1] { ',' });
			if (array3.Length == 1)
			{
				playAudioParams.randomVolumeMin = float.Parse(array3[0]);
				playAudioParams.randomVolumeMax = float.Parse(array3[0]);
			}
			else
			{
				playAudioParams.randomVolumeMin = float.Parse(array3[0]);
				playAudioParams.randomVolumeMax = float.Parse(array3[1]);
			}
		}
		playAudioParams.instanceGroupId = 1;
		string templateData13 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "max_instances_group_id");
		if (!string.IsNullOrEmpty(templateData13))
		{
			int id = (playAudioParams.instanceGroupId = int.Parse(templateData13));
			string templateData14 = GameEntry.ConfigCache.GetTemplateData("lw_sound_max_instances", id, "sound_instance_limit");
			if (!string.IsNullOrEmpty(templateData14))
			{
				playAudioParams.instanceGroupLimit = int.Parse(templateData14);
			}
			string templateData15 = GameEntry.ConfigCache.GetTemplateData("lw_sound_max_instances", id, "when_priority_equal");
			if (!string.IsNullOrEmpty(templateData15))
			{
				playAudioParams.instanceGroupWhenEqual = int.Parse(templateData15);
			}
		}
		return playAudioParams;
	}

	public int PlaySoundById(int soundId, string soundGroupName)
	{
		if (IsUseAudioMixer())
		{
			if (_audioTempMap.ContainsKey(soundId))
			{
				PlayAudioParams param = _audioTempMap[soundId].CopyData();
				return PlayAudio(param);
			}
			PlayAudioParams playAudioParamsById = GetPlayAudioParamsById(soundId);
			if (playAudioParamsById != null)
			{
				_audioTempMap.Add(soundId, playAudioParamsById);
				PlayAudioParams param2 = playAudioParamsById.CopyData();
				return PlayAudio(param2);
			}
		}
		else
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound2");
			if (!string.IsNullOrEmpty(templateData))
			{
				string[] array = templateData.Split(new char[1] { ';' });
				if (array.Length != 0)
				{
					int num = UnityEngine.Random.Range(0, array.Length);
					return PlaySound(array[num], soundGroupName, new PlaySoundParams
					{
						VolumeInSoundGroup = 1f,
						SoundVolumeSet = -1f
					}, null);
				}
				return PlaySound(templateData, soundGroupName, new PlaySoundParams
				{
					VolumeInSoundGroup = 1f,
					SoundVolumeSet = -1f
				}, null);
			}
		}
		return -1;
	}

	public LoopTimerSound PlaySoundByIdWithLimit(int soundId)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound_num");
		string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound_time");
		if (int.TryParse(templateData, out var result) && int.TryParse(templateData2, out var result2))
		{
			float volumeScale = 1f;
			float soundVolumeSet = -1f;
			string templateData3 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound_volume");
			string templateData4 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound_set");
			if (!string.IsNullOrEmpty(templateData3) && float.TryParse(templateData3, out var result3))
			{
				volumeScale = result3;
			}
			if (!string.IsNullOrEmpty(templateData4) && float.TryParse(templateData4, out var result4))
			{
				soundVolumeSet = result4;
			}
			m_loopSoundLimitMap[(ELoopSoundLimit)soundId] = result;
			string[] array = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound").Split(new char[1] { ';' });
			string assetName = string.Empty;
			if (array.Length != 0)
			{
				assetName = array[UnityEngine.Random.Range(0, array.Length)];
			}
			if (!m_loopSoundTimerMap.TryGetValue((ELoopSoundLimit)soundId, out var value))
			{
				LoopTimerSound loopTimerSound = new LoopTimerSound(soundId, assetName, result2 / 1000, volumeScale, soundVolumeSet);
				value = new List<LoopTimerSound> { loopTimerSound };
				m_loopSoundTimerMap.Add((ELoopSoundLimit)soundId, value);
				return loopTimerSound;
			}
			if (value.Count < result)
			{
				LoopTimerSound loopTimerSound2 = new LoopTimerSound(soundId, assetName, result2 / 1000, volumeScale, soundVolumeSet);
				value.Add(loopTimerSound2);
				return loopTimerSound2;
			}
		}
		return null;
	}

	public void StopPlayLoopSoundWithLimit(LoopTimerSound timer)
	{
		if (timer != null && m_loopSoundTimerMap.TryGetValue((ELoopSoundLimit)timer.soundId, out var value))
		{
			timer.Dispose();
			value.Remove(timer);
		}
	}

	public void GetAudioLength(string soundAssetName, Action<float> onGetAudioLength)
	{
		if (soundAssetName.IsNullOrEmpty())
		{
			Debug.LogError("soundAssetName is null or empty");
			return;
		}
		soundAssetName = (IsUseAudioMixer() ? SwitchToAudioSourcePath(soundAssetName) : SwitchToAudioClipPath(soundAssetName));
		Asset req = GameEntry.Resource.LoadAssetAsync(soundAssetName, typeof(UnityEngine.Object));
		if (req == null)
		{
			return;
		}
		bool useAudioMixer = IsUseAudioMixer();
		Asset asset = req;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (!req.isError && useAudioMixer == IsUseAudioMixer())
			{
				if (!useAudioMixer)
				{
					AudioClip audioClip = req.asset as AudioClip;
					if (audioClip != null)
					{
						onGetAudioLength(audioClip.length);
					}
				}
				else
				{
					GameObject gameObject = req.asset as GameObject;
					if (gameObject != null)
					{
						AudioSource component = gameObject.GetComponent<AudioSource>();
						if (component.clip != null)
						{
							onGetAudioLength(component.clip.length);
						}
					}
				}
			}
		});
	}

	public int PlayTimeline(int id)
	{
		if (IsUseAudioMixer())
		{
			PlayAudioParams playAudioParamsById = GetPlayAudioParamsById(id);
			return PlayAudio(playAudioParamsById);
		}
		return GameEntry.Lua.CallWithReturn<int, int>("DataCenter.LWSoundManager:PlayTimeline", id);
	}

	public double GetDSPTime()
	{
		return AudioSettings.dspTime;
	}

	public static string GetSoundPath(int soundId)
	{
		if (GameEntry.Sound.IsUseAudioMixer())
		{
			return GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "audiosource");
		}
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound");
		if (templateData.IsNullOrEmpty())
		{
			return GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound2");
		}
		return templateData;
	}

	public void SetDspBufferSize(int buffSize)
	{
		AudioConfiguration configuration = AudioSettings.GetConfiguration();
		configuration.dspBufferSize = buffSize;
		AudioSettings.Reset(configuration);
	}

	public int GetDspBufferSize()
	{
		return AudioSettings.GetConfiguration().dspBufferSize;
	}

	public void GetAudioSourceObjs(Action<List<GameObject>> onGetObjs)
	{
		for (int i = 0; i < _audioSourcePool.root.childCount; i++)
		{
			_objs.Add(_audioSourcePool.root.GetChild(i).gameObject);
		}
		onGetObjs?.Invoke(_objs);
		_objs.Clear();
	}

	public void GetAudioSourcePaths(Action<List<string>> onGetPaths)
	{
	}

	public AudioMixerGroup GetTargetAudioMixerGroup(string name)
	{
		return GetSoundGroup(name, useAudioMixer: true)?.MixerGroup;
	}
}

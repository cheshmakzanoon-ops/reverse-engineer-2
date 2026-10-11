using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using VEngine;

public class RawImageSeriesVideoPlayer : MonoBehaviour
{
	private enum LoadState
	{
		None,
		Loading,
		Loaded
	}

	public List<string> videoPaths = new List<string>();

	public int width = 810;

	public int height = 1440;

	public bool waitForFirstFrame = true;

	public bool loopLastVideo;

	public bool skipOnDrop = true;

	public bool restartOnEnable;

	public bool loadAllAtOnce;

	public VideoAudioOutputMode audioOutputMode;

	public VideoAspectRatio aspectRatio = VideoAspectRatio.FitVertically;

	public Action OnAllVideosComplete;

	private VideoPlayer _videoPlayer;

	private RawImage _rawImage;

	private RenderTexture _rt;

	private Vector2Int _rtSize = new Vector2Int(810, 1440);

	private RawAssetFile _rawFileAsync;

	private RawAssetFile _nextRawFileAsync;

	private List<RawAssetFile> _allVideoHandles = new List<RawAssetFile>();

	private string[] _allVideoURLs;

	private string _videoURL = string.Empty;

	private string _nextVideoURL = string.Empty;

	private LoadState _loadState;

	private LoadState _nextLoadState;

	private Texture _texture;

	private int _currentIndex;

	private int _loadFinishedCount;

	private string currentVideoPath
	{
		get
		{
			if (videoPaths != null && _currentIndex >= 0 && _currentIndex < videoPaths.Count)
			{
				return videoPaths[_currentIndex];
			}
			return string.Empty;
		}
	}

	private string nextVideoPath
	{
		get
		{
			int num = _currentIndex + 1;
			if (videoPaths != null && num >= 0 && num < videoPaths.Count)
			{
				return videoPaths[num];
			}
			return string.Empty;
		}
	}

	private void OnEnable()
	{
		if (!CommonUtils.IsDebug() || GMSwitch.GetBool("RawImageVideoPlayer", defaultVal: true))
		{
			if (restartOnEnable)
			{
				_currentIndex = 0;
				_loadState = LoadState.None;
				_nextLoadState = LoadState.None;
				_loadFinishedCount = 0;
				_videoURL = string.Empty;
				_nextVideoURL = string.Empty;
			}
			if (loadAllAtOnce)
			{
				LoadAllVideos();
			}
			else if (!restartOnEnable && _loadState == LoadState.Loaded && !string.IsNullOrEmpty(_videoURL))
			{
				PlayInternal();
			}
			else
			{
				LoadVideo();
			}
		}
	}

	private void OnDisable()
	{
		PauseVideo();
	}

	private void OnDestroy()
	{
		if (_videoPlayer != null)
		{
			_videoPlayer.loopPointReached -= OnVideoFinished;
		}
		StopVideo();
		ClearRT();
	}

	public void SetVideoQueue(List<string> paths)
	{
		StopVideo();
		videoPaths.Clear();
		if (paths != null && paths.Count > 0)
		{
			videoPaths.AddRange(paths);
		}
		_currentIndex = 0;
		if (videoPaths.Count > 0)
		{
			if (loadAllAtOnce)
			{
				LoadAllVideos();
			}
			else
			{
				LoadVideo();
			}
		}
	}

	public void LoadVideo(string newVideoPath = null)
	{
		if (_rawImage == null)
		{
			_rawImage = base.gameObject.GetOrAddComponent<RawImage>();
		}
		if (_videoPlayer == null)
		{
			CreateVideoPlayer();
		}
		_rtSize = new Vector2Int(width, height);
		if (!string.IsNullOrEmpty(newVideoPath))
		{
			if (currentVideoPath != newVideoPath)
			{
				StopVideo();
			}
			videoPaths.Clear();
			videoPaths.Add(newVideoPath);
			_currentIndex = 0;
		}
		if (loadAllAtOnce && _allVideoURLs != null && _currentIndex < _allVideoURLs.Length)
		{
			string text = _allVideoURLs[_currentIndex];
			if (string.IsNullOrEmpty(text))
			{
				Log.Error("Video preload failed, skipping index {0}", _currentIndex);
				OnVideoFinished(null);
			}
			else
			{
				_videoURL = text;
				_loadState = LoadState.Loaded;
				PlayInternal();
			}
			return;
		}
		string text2 = currentVideoPath;
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		switch (_loadState)
		{
		case LoadState.Loaded:
			PlayInternal();
			break;
		case LoadState.None:
			_loadState = LoadState.Loading;
			_videoPlayer.Stop();
			_videoURL = string.Empty;
			ClearRT();
			ReleaseRawFile(ref _rawFileAsync, OnRawCompleted);
			_rawFileAsync = RawAssetFile.LoadAsync(text2);
			if (_rawFileAsync == null)
			{
				Log.Error("Video loadAsync is null: {0}", text2);
				OnVideoFinished(null);
			}
			else
			{
				RawAssetFile rawFileAsync = _rawFileAsync;
				rawFileAsync.completed = (Action<RawAssetFile>)Delegate.Combine(rawFileAsync.completed, new Action<RawAssetFile>(OnRawCompleted));
			}
			break;
		case LoadState.Loading:
			break;
		}
	}

	private void LoadAllVideos()
	{
		StopVideo();
		if (videoPaths == null || videoPaths.Count == 0)
		{
			return;
		}
		_loadState = LoadState.Loading;
		_loadFinishedCount = 0;
		_allVideoURLs = new string[videoPaths.Count];
		_allVideoHandles.Clear();
		for (int i = 0; i < videoPaths.Count; i++)
		{
			string rawAssetPath = videoPaths[i];
			int index = i;
			RawAssetFile rawAssetFile = RawAssetFile.LoadAsync(rawAssetPath);
			_allVideoHandles.Add(rawAssetFile);
			if (rawAssetFile != null)
			{
				rawAssetFile.completed = (Action<RawAssetFile>)Delegate.Combine(rawAssetFile.completed, (Action<RawAssetFile>)delegate(RawAssetFile f)
				{
					OnAllVideosPreloadCompleted(f, index);
				});
			}
			else
			{
				OnAllVideosPreloadCompleted(null, index);
			}
		}
	}

	private void OnAllVideosPreloadCompleted(RawAssetFile rawFile, int index)
	{
		if (rawFile != null && rawFile.isDone && rawFile.status == LoadableStatus.SuccessToLoad)
		{
			_allVideoURLs[index] = rawFile.rawSavePath;
		}
		else
		{
			Log.Error("Video preload failed: {0}", videoPaths[index]);
			_allVideoURLs[index] = string.Empty;
		}
		_loadFinishedCount++;
		if (_loadFinishedCount == videoPaths.Count)
		{
			_loadState = LoadState.None;
			_currentIndex = 0;
			LoadVideo();
		}
	}

	private void ReleaseRawFile(ref RawAssetFile handle, Action<RawAssetFile> callback)
	{
		if (handle != null)
		{
			if (callback != null)
			{
				RawAssetFile obj = handle;
				obj.completed = (Action<RawAssetFile>)Delegate.Remove(obj.completed, callback);
			}
			handle.Release();
			handle = null;
		}
	}

	private void ReleaseAllHandles()
	{
		for (int i = 0; i < _allVideoHandles.Count; i++)
		{
			if (_allVideoHandles[i] != null)
			{
				_allVideoHandles[i].Release();
			}
		}
		_allVideoHandles.Clear();
	}

	public void PauseVideo()
	{
		if (_videoPlayer != null)
		{
			_videoPlayer.Pause();
		}
		ClearRT();
	}

	public void StopVideo()
	{
		if (_videoPlayer != null)
		{
			_videoPlayer.Stop();
		}
		ClearRT();
		ReleaseRawFile(ref _rawFileAsync, OnRawCompleted);
		ReleaseRawFile(ref _nextRawFileAsync, OnNextRawCompleted);
		ReleaseAllHandles();
		_videoURL = string.Empty;
		_nextVideoURL = string.Empty;
		_allVideoURLs = null;
		_loadState = LoadState.None;
		_nextLoadState = LoadState.None;
	}

	private void PlayInternal()
	{
		if (!(_videoPlayer == null) && !string.IsNullOrEmpty(_videoURL))
		{
			TryCreateRT();
			_videoPlayer.targetTexture = _rt;
			bool flag = videoPaths != null && _currentIndex == videoPaths.Count - 1;
			_videoPlayer.isLooping = flag && loopLastVideo;
			if (_videoPlayer.url != _videoURL)
			{
				_videoPlayer.url = _videoURL;
			}
			_videoPlayer.Prepare();
			if (!loadAllAtOnce)
			{
				PreloadNextVideo();
			}
		}
	}

	private void CreateVideoPlayer()
	{
		if (!(_videoPlayer != null))
		{
			_videoPlayer = base.gameObject.GetComponent<VideoPlayer>();
			if (_videoPlayer == null)
			{
				_videoPlayer = base.gameObject.AddComponent<VideoPlayer>();
			}
			_videoPlayer.prepareCompleted += OnVideoPlayerStarted;
			_videoPlayer.playOnAwake = false;
			_videoPlayer.waitForFirstFrame = waitForFirstFrame;
			_videoPlayer.skipOnDrop = skipOnDrop;
			_videoPlayer.isLooping = false;
			_videoPlayer.source = VideoSource.Url;
			_videoPlayer.audioOutputMode = audioOutputMode;
			_videoPlayer.renderMode = VideoRenderMode.RenderTexture;
			_videoPlayer.aspectRatio = aspectRatio;
			_videoPlayer.loopPointReached += OnVideoFinished;
		}
	}

	private void OnVideoFinished(VideoPlayer vp)
	{
		if (videoPaths != null && _currentIndex < videoPaths.Count - 1)
		{
			_currentIndex++;
			if (_videoPlayer != null && _videoPlayer.isPlaying)
			{
				_videoPlayer.Pause();
			}
			if (loadAllAtOnce)
			{
				LoadVideo();
			}
			else
			{
				LoadVideoNextInQueue();
			}
		}
		else if (!loopLastVideo)
		{
			OnAllVideosComplete?.Invoke();
		}
	}

	private void LoadVideoNextInQueue()
	{
		if (_nextLoadState == LoadState.Loaded && !string.IsNullOrEmpty(_nextVideoURL))
		{
			_videoURL = _nextVideoURL;
			_loadState = LoadState.Loaded;
			ReleaseRawFile(ref _rawFileAsync, OnRawCompleted);
			_rawFileAsync = _nextRawFileAsync;
			if (_rawFileAsync != null)
			{
				RawAssetFile rawFileAsync = _rawFileAsync;
				rawFileAsync.completed = (Action<RawAssetFile>)Delegate.Remove(rawFileAsync.completed, new Action<RawAssetFile>(OnNextRawCompleted));
				RawAssetFile rawFileAsync2 = _rawFileAsync;
				rawFileAsync2.completed = (Action<RawAssetFile>)Delegate.Combine(rawFileAsync2.completed, new Action<RawAssetFile>(OnRawCompleted));
			}
			_nextRawFileAsync = null;
			_nextLoadState = LoadState.None;
			_nextVideoURL = string.Empty;
			PlayInternal();
		}
		else
		{
			_loadState = LoadState.Loading;
			_videoURL = string.Empty;
			if (_nextLoadState != LoadState.Loading)
			{
				LoadVideo();
			}
		}
	}

	private void PreloadNextVideo()
	{
		string text = nextVideoPath;
		if (!string.IsNullOrEmpty(text) && _nextLoadState == LoadState.None)
		{
			_nextLoadState = LoadState.Loading;
			_nextVideoURL = string.Empty;
			ReleaseRawFile(ref _nextRawFileAsync, OnNextRawCompleted);
			_nextRawFileAsync = RawAssetFile.LoadAsync(text);
			if (_nextRawFileAsync != null)
			{
				RawAssetFile nextRawFileAsync = _nextRawFileAsync;
				nextRawFileAsync.completed = (Action<RawAssetFile>)Delegate.Combine(nextRawFileAsync.completed, new Action<RawAssetFile>(OnNextRawCompleted));
			}
			else
			{
				_nextLoadState = LoadState.None;
			}
		}
	}

	private void OnVideoPlayerStarted(VideoPlayer vp)
	{
		_videoPlayer.Play();
		if (_texture == null)
		{
			_texture = _rawImage.texture;
		}
		GameEntry.Timer.RegisterTimer(0f, delegate
		{
			if (_rawImage != null && _rt != null)
			{
				_rawImage.texture = _rt;
			}
		});
		GameEntry.Event.Fire(EventId.RawImageVideoPlayerRealStart, currentVideoPath);
	}

	private void TryCreateRT()
	{
		if (_rawImage != null)
		{
			if (_rt == null)
			{
				_rt = RenderTexture.GetTemporary(_rtSize.x, _rtSize.y, 0, RenderTextureFormat.ARGB32);
				_rt.name = $"VideoRT_{_rtSize.x}_{_rtSize.y}";
			}
			else if (_rt.width != _rtSize.x || _rt.height != _rtSize.y)
			{
				RenderTexture.ReleaseTemporary(_rt);
				_rt = RenderTexture.GetTemporary(_rtSize.x, _rtSize.y, 0, RenderTextureFormat.ARGB32);
				_rt.name = $"VideoRT_{_rtSize.x}_{_rtSize.y}";
			}
		}
		if (_videoPlayer != null)
		{
			_videoPlayer.targetTexture = _rt;
		}
	}

	private void ClearRT()
	{
		if (_rt != null)
		{
			RenderTexture active = RenderTexture.active;
			try
			{
				RenderTexture.active = _rt;
				GL.Clear(clearDepth: true, clearColor: true, Color.black);
			}
			finally
			{
				RenderTexture.active = active;
			}
			RenderTexture.ReleaseTemporary(_rt);
			_rt = null;
		}
		if (_videoPlayer != null)
		{
			_videoPlayer.targetTexture = null;
		}
		if (_texture != null)
		{
			_rawImage.texture = _texture;
		}
	}

	private void OnRawCompleted(RawAssetFile rawFile)
	{
		if (_rawFileAsync == null || rawFile != _rawFileAsync)
		{
			return;
		}
		string text = currentVideoPath;
		if (rawFile.isDone && rawFile.status == LoadableStatus.SuccessToLoad)
		{
			if (rawFile.name == text)
			{
				OnVideoLoaded(rawFile.rawSavePath);
			}
			else
			{
				Log.Info("rawFile is already Unloaded: {0}", rawFile.name);
			}
		}
		else
		{
			Log.Error("Video file loading failed or incomplete: {0}", text);
			OnVideoFinished(null);
			ReleaseRawFile(ref _rawFileAsync, OnRawCompleted);
		}
	}

	private void OnNextRawCompleted(RawAssetFile rawFile)
	{
		if (_nextRawFileAsync == null || rawFile != _nextRawFileAsync)
		{
			return;
		}
		if (rawFile.isDone && rawFile.status == LoadableStatus.SuccessToLoad)
		{
			_nextVideoURL = rawFile.rawSavePath;
			_nextLoadState = LoadState.Loaded;
			if (_loadState == LoadState.Loading && currentVideoPath == rawFile.name)
			{
				LoadVideoNextInQueue();
			}
		}
		else
		{
			_nextLoadState = LoadState.None;
			ReleaseRawFile(ref _nextRawFileAsync, OnNextRawCompleted);
			if (_loadState == LoadState.Loading && currentVideoPath == rawFile.name)
			{
				OnVideoFinished(null);
			}
		}
	}

	private void OnVideoLoaded(string path)
	{
		if (_videoPlayer == null)
		{
			Log.Error("VideoPlayer component not found!");
			return;
		}
		_videoURL = path;
		_loadState = LoadState.Loaded;
		PlayInternal();
	}

	private void LoadAndPlay_Editor(string path, bool isCurrent, int index)
	{
		if (_rawImage == null)
		{
			_rawImage = base.gameObject.GetOrAddComponent<RawImage>();
		}
		if (_videoPlayer == null)
		{
			CreateVideoPlayer();
		}
		_rtSize = new Vector2Int(width, height);
		if (File.Exists(path))
		{
			string text = "file://" + Path.GetFullPath(path);
			if (loadAllAtOnce)
			{
				if (_allVideoURLs != null && index < _allVideoURLs.Length)
				{
					_allVideoURLs[index] = text;
					_loadFinishedCount++;
					if (_loadFinishedCount == videoPaths.Count)
					{
						_loadState = LoadState.None;
						_currentIndex = 0;
						LoadVideo();
					}
				}
			}
			else if (isCurrent)
			{
				_videoURL = text;
				_loadState = LoadState.Loaded;
				PlayInternal();
			}
			else
			{
				_nextVideoURL = text;
				_nextLoadState = LoadState.Loaded;
				if (_loadState == LoadState.Loading && currentVideoPath == path)
				{
					LoadVideoNextInQueue();
				}
			}
			return;
		}
		Log.Error("视频文件未找到: {0}", path);
		if (loadAllAtOnce)
		{
			if (_allVideoURLs != null && index < _allVideoURLs.Length)
			{
				_allVideoURLs[index] = string.Empty;
				_loadFinishedCount++;
				if (_loadFinishedCount == videoPaths.Count)
				{
					_loadState = LoadState.None;
					_currentIndex = 0;
					LoadVideo();
				}
			}
		}
		else if (isCurrent)
		{
			OnVideoFinished(null);
		}
		else
		{
			_nextLoadState = LoadState.None;
		}
	}
}

using System;
using System.IO;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using VEngine;

public class RawImageVideoPlayer : MonoBehaviour
{
	private enum LoadState
	{
		None,
		Loading,
		Loaded
	}

	public string videoPath = "";

	public int width = 810;

	public int height = 1440;

	public bool playOnAwake;

	public bool waitForFirstFrame = true;

	public bool loop;

	public bool skipOnDrop = true;

	public VideoAudioOutputMode audioOutputMode;

	public VideoAspectRatio aspectRatio = VideoAspectRatio.FitVertically;

	private VideoPlayer _videoPlayer;

	private RawImage _rawImage;

	private RenderTexture _rt;

	private Vector2Int _rtSize = new Vector2Int(810, 1440);

	private RawAssetFile _rawFileAsync;

	private string _videoURL = string.Empty;

	private LoadState _loadState;

	private Texture _texture;

	private void OnEnable()
	{
		if (!CommonUtils.IsDebug() || GMSwitch.GetBool("RawImageVideoPlayer", defaultVal: true))
		{
			LoadVideo();
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
			_videoPlayer.prepareCompleted -= OnVideoPlayerStarted;
			_videoPlayer.frameReady -= OnVideoFrameReady;
		}
		StopVideo();
	}

	public void LoadVideo(string newVideoPath = null)
	{
		if (!string.IsNullOrEmpty(newVideoPath))
		{
			if (videoPath != newVideoPath)
			{
				StopVideo();
			}
			videoPath = newVideoPath;
		}
		if (string.IsNullOrEmpty(videoPath))
		{
			return;
		}
		if (_rawImage == null)
		{
			_rawImage = base.gameObject.GetOrAddComponent<RawImage>();
		}
		if (_videoPlayer == null)
		{
			CreateVideoPlayer();
		}
		_rtSize = new Vector2Int(width, height);
		switch (_loadState)
		{
		case LoadState.Loaded:
			PlayInternal();
			break;
		case LoadState.None:
		{
			_loadState = LoadState.Loading;
			_videoPlayer.Stop();
			_videoURL = string.Empty;
			ClearRT();
			if (_rawFileAsync != null)
			{
				_rawFileAsync.Release();
				_rawFileAsync.completed = null;
				_rawFileAsync = null;
			}
			_rawFileAsync = RawAssetFile.LoadAsync(videoPath);
			if (_rawFileAsync == null)
			{
				Log.Error("Video loadAsync is null");
				break;
			}
			RawAssetFile rawFileAsync = _rawFileAsync;
			rawFileAsync.completed = (Action<RawAssetFile>)Delegate.Combine(rawFileAsync.completed, new Action<RawAssetFile>(OnRawCompleted));
			break;
		}
		case LoadState.Loading:
			break;
		}
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
		if (_rawFileAsync != null)
		{
			_rawFileAsync.Release();
			_rawFileAsync.completed = null;
			_rawFileAsync = null;
		}
		_videoURL = string.Empty;
		_loadState = LoadState.None;
	}

	private void PlayInternal()
	{
		if (!(_videoPlayer == null) && !string.IsNullOrEmpty(_videoURL))
		{
			_videoPlayer.sendFrameReadyEvents = true;
			TryCreateRT();
			_videoPlayer.targetTexture = _rt;
			if (_videoPlayer.url != _videoURL)
			{
				_videoPlayer.url = _videoURL;
			}
			_videoPlayer.Prepare();
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
			_videoPlayer.frameReady += OnVideoFrameReady;
			_videoPlayer.sendFrameReadyEvents = true;
			_videoPlayer.playOnAwake = playOnAwake;
			_videoPlayer.waitForFirstFrame = waitForFirstFrame;
			_videoPlayer.skipOnDrop = skipOnDrop;
			_videoPlayer.isLooping = loop;
			_videoPlayer.source = VideoSource.Url;
			_videoPlayer.audioOutputMode = audioOutputMode;
			_videoPlayer.renderMode = VideoRenderMode.RenderTexture;
			_videoPlayer.aspectRatio = aspectRatio;
		}
	}

	private void OnVideoPlayerStarted(VideoPlayer vp)
	{
		_videoPlayer.Play();
		if (_texture == null)
		{
			_texture = _rawImage.texture;
		}
	}

	private void OnVideoFrameReady(VideoPlayer source, long frameIdx)
	{
		if (_rawImage != null && _rt != null)
		{
			_rawImage.texture = _rt;
		}
		_videoPlayer.sendFrameReadyEvents = false;
		GameEntry.Event.Fire(EventId.RawImageVideoPlayerRealStart, videoPath);
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
			_videoPlayer.sendFrameReadyEvents = false;
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
		if (rawFile.isDone && rawFile.status == LoadableStatus.SuccessToLoad)
		{
			if (rawFile.name == videoPath)
			{
				OnVideoLoaded(rawFile.rawSavePath);
			}
			else
			{
				Log.Info("rawFile is already Unloaded:" + rawFile.name);
			}
		}
		else
		{
			Log.Error("Video file loading failed or incomplete.");
		}
		_rawFileAsync.Release();
		_rawFileAsync = null;
	}

	private void OnVideoLoaded(string path)
	{
		if (_videoPlayer == null)
		{
			Log.Error("VideoPlayer component not found!");
			return;
		}
		_videoPlayer.sendFrameReadyEvents = true;
		TryCreateRT();
		_videoPlayer.renderMode = VideoRenderMode.RenderTexture;
		_videoPlayer.targetTexture = _rt;
		_videoPlayer.source = VideoSource.Url;
		_videoPlayer.url = path;
		_videoURL = path;
		_loadState = LoadState.Loaded;
		_videoPlayer.Prepare();
	}

	private void LoadAndPlay_Editor()
	{
		if (_videoPlayer == null)
		{
			Log.Error("VideoPlayer component not found!");
			return;
		}
		_videoPlayer.sendFrameReadyEvents = true;
		TryCreateRT();
		_videoPlayer.targetTexture = _rt;
		if (File.Exists(videoPath))
		{
			string text = "file://" + Path.GetFullPath(videoPath);
			_videoPlayer.url = text;
			_videoURL = text;
			_loadState = LoadState.Loaded;
			_videoPlayer.Prepare();
		}
		else
		{
			Log.Error("视频文件未找到: " + videoPath);
		}
	}
}

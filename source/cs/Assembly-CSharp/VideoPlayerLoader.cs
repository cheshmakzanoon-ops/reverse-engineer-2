using System;
using GameFramework;
using UnityEngine;
using UnityEngine.Video;
using VEngine;

public class VideoPlayerLoader : MonoBehaviour
{
	private enum LoadState
	{
		None,
		Loading,
		Loaded
	}

	public string videoPath = "";

	public bool loop;

	public VideoAudioOutputMode audioOutputMode;

	public VideoAspectRatio aspectRatio = VideoAspectRatio.FitVertically;

	public RenderTexture targetRT;

	private VideoPlayer _videoPlayer;

	private RawAssetFile _rawFileAsync;

	private string _videoURL = string.Empty;

	private LoadState _loadState;

	private void OnEnable()
	{
		LoadVideo();
	}

	private void OnDisable()
	{
		PauseVideo();
	}

	private void OnDestroy()
	{
		if (_videoPlayer != null)
		{
			_videoPlayer.prepareCompleted -= OnVideoPlayerPrepared;
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
		if (_videoPlayer == null)
		{
			CreateVideoPlayer();
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
			if (_rawFileAsync != null)
			{
				_rawFileAsync.Release();
				_rawFileAsync.completed = null;
				_rawFileAsync = null;
			}
			_rawFileAsync = RawAssetFile.LoadAsync(videoPath);
			if (_rawFileAsync == null)
			{
				Log.Error("Video loadAsync is null: {0}", videoPath);
				_loadState = LoadState.None;
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

	public void PauseVideo()
	{
		if (_videoPlayer != null)
		{
			_videoPlayer.Pause();
		}
	}

	public void StopVideo()
	{
		if (_videoPlayer != null)
		{
			_videoPlayer.Stop();
		}
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
			if (targetRT == null)
			{
				Log.Warning("VideoPlayerLoader: targetRT is null, video will not be rendered.");
			}
			_videoPlayer.targetTexture = targetRT;
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
			_videoPlayer.prepareCompleted += OnVideoPlayerPrepared;
			_videoPlayer.playOnAwake = false;
			_videoPlayer.waitForFirstFrame = true;
			_videoPlayer.skipOnDrop = true;
			_videoPlayer.isLooping = loop;
			_videoPlayer.source = VideoSource.Url;
			_videoPlayer.audioOutputMode = audioOutputMode;
			_videoPlayer.renderMode = VideoRenderMode.RenderTexture;
			_videoPlayer.aspectRatio = aspectRatio;
		}
	}

	private void OnVideoPlayerPrepared(VideoPlayer vp)
	{
		_videoPlayer.Play();
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
				Log.Info("VideoPlayerLoader: rawFile is already Unloaded: {0}", rawFile.name);
				_loadState = LoadState.None;
			}
		}
		else
		{
			Log.Error("VideoPlayerLoader: Video file loading failed: {0}", videoPath);
			_loadState = LoadState.None;
		}
		if (_rawFileAsync != null)
		{
			_rawFileAsync.Release();
			_rawFileAsync = null;
		}
	}

	private void OnVideoLoaded(string path)
	{
		_videoURL = path;
		_loadState = LoadState.Loaded;
		PlayInternal();
	}
}

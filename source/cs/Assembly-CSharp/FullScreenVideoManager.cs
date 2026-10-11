using System;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Video;
using VEngine;

public class FullScreenVideoManager : SingletonBehaviour<FullScreenVideoManager>
{
	public VideoPlayer mVideoPlayer;

	private Camera mVideoCamera;

	private UniversalAdditionalCameraData mCameraData;

	private RawAssetFile rawFileAsync;

	private Action<Transform, VideoPlayer> firstFrameReadyCallback;

	private Action videoStoppedCallback;

	private Action videoStartedCallback;

	private bool mIsLooping;

	private Canvas mCanvas;

	private AudioSource mAudioSource;

	private bool mUseExternalAudio;

	public void LoadVideo(string vPath, bool isLoop, Action<Transform, VideoPlayer> firstFrameReadyCallback, Action videoStoppedCallback)
	{
		LoadVideoInternal(vPath, isLoop, firstFrameReadyCallback, videoStoppedCallback, useExternalAudio: false);
	}

	public void LoadVideo(string vPath, bool isLoop, Action<Transform, VideoPlayer> firstFrameReadyCallback, Action videoStoppedCallback, bool useExternalAudio, Action videoStartedCallback)
	{
		LoadVideoInternal(vPath, isLoop, firstFrameReadyCallback, videoStoppedCallback, useExternalAudio, videoStartedCallback);
	}

	private void LoadVideoInternal(string vPath, bool isLoop, Action<Transform, VideoPlayer> firstFrameReadyCallback, Action videoStoppedCallback, bool useExternalAudio, Action videoStartedCallback = null)
	{
		mIsLooping = isLoop;
		StopAndReleaseVideo();
		mUseExternalAudio = useExternalAudio;
		if (mVideoPlayer == null)
		{
			CreateVideoPlayer();
		}
		else
		{
			mVideoPlayer.enabled = true;
			mVideoPlayer.isLooping = isLoop;
		}
		ConfigureAudioMode();
		if (mVideoCamera == null)
		{
			CreateVideoCamera();
		}
		else
		{
			mVideoCamera.enabled = true;
		}
		if (mCanvas == null)
		{
			CreateCanvas();
		}
		else
		{
			mCanvas.enabled = true;
		}
		this.firstFrameReadyCallback = firstFrameReadyCallback;
		this.videoStoppedCallback = videoStoppedCallback;
		this.videoStartedCallback = videoStartedCallback;
		if (mVideoPlayer == null)
		{
			Log.Error("VideoPlayer component not found!");
			return;
		}
		mVideoPlayer.targetCamera = mVideoCamera;
		rawFileAsync = RawAssetFile.LoadAsync(vPath);
		if (rawFileAsync == null)
		{
			Log.Error("Video loadAsync is null");
			return;
		}
		RawAssetFile rawAssetFile = rawFileAsync;
		rawAssetFile.completed = (Action<RawAssetFile>)Delegate.Combine(rawAssetFile.completed, new Action<RawAssetFile>(OnRawCompleted));
	}

	private void OnRawCompleted(RawAssetFile rawFile)
	{
		if (rawFileAsync != null)
		{
			if (rawFile != null && rawFile.isDone && rawFile.status == LoadableStatus.SuccessToLoad)
			{
				OnVideoLoaded(rawFile.rawSavePath);
			}
			else
			{
				Log.Error("Video file loading failed or incomplete.");
			}
			rawFileAsync.Release();
			rawFileAsync = null;
		}
	}

	public void OnVideoLoaded(string url)
	{
		mVideoPlayer.url = url;
		SetVideoAlpha(0f);
		mVideoPlayer.Prepare();
		mVideoPlayer.prepareCompleted += OnVideoPrepared;
	}

	public void OnVideoPrepared(VideoPlayer vp)
	{
		SetVideoAlpha(1f);
		mVideoPlayer.loopPointReached += OnVideoStopped;
		Log.Info("OnVideoPrepared");
		if (firstFrameReadyCallback != null)
		{
			firstFrameReadyCallback(mCanvas.transform, vp);
			firstFrameReadyCallback = null;
		}
		if (mAudioSource != null)
		{
			mAudioSource.mute = !GameEntry.Setting.GetBool("isEffectMusicOn");
		}
		vp.Play();
		if (videoStartedCallback != null)
		{
			videoStartedCallback();
			videoStartedCallback = null;
		}
	}

	public void OnVideoStopped(VideoPlayer vp)
	{
		if (videoStoppedCallback != null)
		{
			videoStoppedCallback();
			videoStoppedCallback = null;
		}
		if (!mIsLooping)
		{
			StopAndReleaseVideo();
		}
	}

	public void PauseVideo()
	{
		if (mVideoPlayer != null)
		{
			mVideoPlayer.Pause();
		}
	}

	public void StopAndReleaseVideo()
	{
		videoStartedCallback = null;
		if (rawFileAsync != null)
		{
			rawFileAsync.Release();
			rawFileAsync = null;
		}
		if (mVideoPlayer != null)
		{
			mVideoPlayer.Pause();
			mVideoPlayer.Stop();
			mVideoPlayer.targetCamera = null;
			mVideoPlayer.clip = null;
			mVideoPlayer.enabled = false;
			mVideoPlayer.prepareCompleted -= OnVideoPrepared;
			mVideoPlayer.loopPointReached -= OnVideoStopped;
		}
		if (mVideoCamera != null)
		{
			mVideoCamera.enabled = false;
		}
		if (mCanvas != null)
		{
			UnityEngine.Object.Destroy(mCanvas.gameObject);
			mCanvas = null;
		}
	}

	public void CreateVideoPlayer()
	{
		if (mVideoPlayer == null)
		{
			mVideoPlayer = base.gameObject.GetComponent<VideoPlayer>();
			if (mVideoPlayer == null)
			{
				mVideoPlayer = base.gameObject.AddComponent<VideoPlayer>();
			}
			mVideoPlayer.playOnAwake = false;
			mVideoPlayer.waitForFirstFrame = true;
			mVideoPlayer.skipOnDrop = true;
			mVideoPlayer.isLooping = mIsLooping;
			mVideoPlayer.source = VideoSource.Url;
			mVideoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
			mVideoPlayer.aspectRatio = VideoAspectRatio.FitVertically;
			base.gameObject.layer = LayerMask.NameToLayer("UI");
		}
	}

	private void ConfigureAudioMode()
	{
		if (mVideoPlayer == null)
		{
			return;
		}
		if (mUseExternalAudio)
		{
			mVideoPlayer.audioOutputMode = VideoAudioOutputMode.None;
			mVideoPlayer.EnableAudioTrack(0, enabled: false);
			return;
		}
		mVideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		mVideoPlayer.EnableAudioTrack(0, enabled: true);
		if (mAudioSource == null)
		{
			mAudioSource = base.gameObject.GetComponent<AudioSource>();
			if (mAudioSource == null)
			{
				mAudioSource = base.gameObject.AddComponent<AudioSource>();
			}
			mAudioSource.playOnAwake = false;
			mAudioSource.outputAudioMixerGroup = GameEntry.Sound.GetTargetAudioMixerGroup("TIMELINE_Music");
		}
		mVideoPlayer.SetTargetAudioSource(0, mAudioSource);
	}

	private void CreateVideoCamera()
	{
		if (!(mVideoCamera != null))
		{
			mVideoCamera = base.gameObject.GetComponent<Camera>();
			if (mVideoCamera == null)
			{
				mVideoCamera = base.gameObject.AddComponent<Camera>();
			}
			mCameraData = base.gameObject.GetComponent<UniversalAdditionalCameraData>();
			if (mCameraData == null)
			{
				mCameraData = base.gameObject.AddComponent<UniversalAdditionalCameraData>();
			}
			mCameraData.renderType = CameraRenderType.UI;
			mVideoCamera.clearFlags = CameraClearFlags.Depth;
			mVideoCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
			mVideoCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
			mVideoCamera.depth = 100f;
			mVideoCamera.orthographicSize = 1f;
			mVideoCamera.orthographic = true;
		}
	}

	public void SetVideoAlpha(float alpha)
	{
		if (!(mVideoPlayer == null))
		{
			mVideoPlayer.targetCameraAlpha = alpha;
		}
	}

	private void CreateCanvas()
	{
		if (!(mCanvas != null) && mCanvas == null)
		{
			GameObject gameObject = new GameObject("FullVideoScreenCanvas");
			mCanvas = gameObject.AddComponent<Canvas>();
			mCanvas.renderMode = RenderMode.ScreenSpaceCamera;
			mCanvas.worldCamera = mVideoCamera;
			mCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
			gameObject.AddComponent<GraphicRaycaster>();
			CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = new Vector2(810f, 1440f);
			canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
		}
	}
}

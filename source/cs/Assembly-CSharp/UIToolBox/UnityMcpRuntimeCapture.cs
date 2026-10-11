using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UIToolBox;

public sealed class UnityMcpRuntimeCapture : MonoBehaviour
{
	[Serializable]
	public sealed class RuntimePreviewResult
	{
		public bool ok;

		public string pngPath;

		public string error;

		public string detail;
	}

	public sealed class CameraPreviewContext
	{
		public GameObject root;

		public GameObject canvasGO;

		public Camera camera;

		public RenderTexture renderTexture;

		public GameObject previewTarget;
	}

	private static UnityMcpRuntimeCapture _instance;

	private static bool _captureInFlight;

	private static float _nextCaptureAllowedAt;

	private const float MinCaptureIntervalSeconds = 0.12f;

	private static readonly Color PreviewBackdropBlack = new Color(0f, 0f, 0f, 1f);

	private static readonly Color PreviewBackdropWhite = new Color(1f, 1f, 1f, 1f);

	public static UnityMcpRuntimeCapture GetOrCreate()
	{
		if (_instance != null)
		{
			return _instance;
		}
		_instance = UnityEngine.Object.FindObjectOfType<UnityMcpRuntimeCapture>();
		if (_instance != null)
		{
			return _instance;
		}
		GameObject obj = new GameObject("__UnityMcpRuntimeCapture__")
		{
			hideFlags = HideFlags.DontSave
		};
		UnityEngine.Object.DontDestroyOnLoad(obj);
		_instance = obj.AddComponent<UnityMcpRuntimeCapture>();
		return _instance;
	}

	public Task<byte[]> CapturePngAsync(int targetWidth, int targetHeight, int maxFramesToWait = 5)
	{
		if (targetWidth <= 0)
		{
			throw new ArgumentOutOfRangeException("targetWidth");
		}
		if (targetHeight <= 0)
		{
			throw new ArgumentOutOfRangeException("targetHeight");
		}
		if (maxFramesToWait <= 0)
		{
			maxFramesToWait = 1;
		}
		TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();
		StartCoroutine(RunExclusiveCapture(CaptureRoutine(targetWidth, targetHeight, maxFramesToWait, tcs), delegate(Exception e)
		{
			tcs.TrySetException(e);
		}));
		return tcs.Task;
	}

	public Task<RuntimePreviewResult> RenderPrefabPreviewToPngAsync(GameObject prefab, string outPngPath, int width, int height, Color background, string cropMode, float padding, int maxFramesToWait = 3, int previewLayer = 31)
	{
		if (prefab == null)
		{
			throw new ArgumentNullException("prefab");
		}
		if (string.IsNullOrEmpty(outPngPath))
		{
			throw new ArgumentException("outPngPath is required", "outPngPath");
		}
		if (width <= 0)
		{
			throw new ArgumentOutOfRangeException("width");
		}
		if (height <= 0)
		{
			throw new ArgumentOutOfRangeException("height");
		}
		if (maxFramesToWait <= 0)
		{
			maxFramesToWait = 1;
		}
		TaskCompletionSource<RuntimePreviewResult> tcs = new TaskCompletionSource<RuntimePreviewResult>();
		bool flag = ShouldUseScreenshotPreview(prefab, Application.isPlaying);
		StartCoroutine(RunExclusiveCapture(flag ? RenderPrefabScreenshotRoutine(prefab, outPngPath, width, height, background, cropMode, padding, maxFramesToWait, previewLayer, tcs) : RenderPrefabCameraRoutine(prefab, outPngPath, width, height, background, cropMode, padding, maxFramesToWait, previewLayer, tcs), delegate(Exception e)
		{
			tcs.TrySetResult(new RuntimePreviewResult
			{
				ok = false,
				pngPath = outPngPath,
				error = "exception",
				detail = e.ToString()
			});
		}));
		return tcs.Task;
	}

	private static bool ShouldUseScreenshotPreview(GameObject prefab, bool isPlaying)
	{
		if (prefab == null)
		{
			return false;
		}
		if (prefab.GetComponent<RectTransform>() != null || prefab.GetComponentInChildren<Canvas>(includeInactive: true) != null)
		{
			return !isPlaying;
		}
		return false;
	}

	private static string NormalizeProjectRelativePath(string relativePath)
	{
		if (string.IsNullOrWhiteSpace(relativePath))
		{
			throw new ArgumentException("Path must be a non-empty project-relative path.", "relativePath");
		}
		string text = relativePath.Trim().Replace('\\', '/');
		if (Path.IsPathRooted(text))
		{
			throw new ArgumentException("Absolute host paths are not supported here: '" + relativePath + "'.", "relativePath");
		}
		return text;
	}

	private static string GetProjectRootPath()
	{
		string dataPath = Application.dataPath;
		return (Directory.GetParent(dataPath) ?? throw new InvalidOperationException("Unable to resolve Unity project root from Application.dataPath='" + dataPath + "'.")).FullName;
	}

	private static StringComparison GetPathComparison()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor)
		{
			return StringComparison.Ordinal;
		}
		return StringComparison.OrdinalIgnoreCase;
	}

	private static string ResolveProjectRelativePath(string relativePath)
	{
		string path = NormalizeProjectRelativePath(relativePath);
		string fullPath = Path.GetFullPath(GetProjectRootPath());
		string fullPath2 = Path.GetFullPath(Path.Combine(fullPath, path));
		string text = fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		char directorySeparatorChar = Path.DirectorySeparatorChar;
		string value = text + directorySeparatorChar;
		StringComparison pathComparison = GetPathComparison();
		if (!fullPath2.StartsWith(value, pathComparison) && !string.Equals(fullPath2, fullPath, pathComparison))
		{
			throw new ArgumentException("Path '" + relativePath + "' escapes the Unity project root and is not allowed.", "relativePath");
		}
		return fullPath2;
	}

	private static string ResolveProjectOutputPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			throw new ArgumentException("Path must be a non-empty project path.", "path");
		}
		string text = path.Trim().Replace('\\', '/');
		if (!Path.IsPathRooted(text))
		{
			return ResolveProjectRelativePath(text);
		}
		string fullPath = Path.GetFullPath(GetProjectRootPath());
		string fullPath2 = Path.GetFullPath(text);
		string text2 = fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		char directorySeparatorChar = Path.DirectorySeparatorChar;
		string value = text2 + directorySeparatorChar;
		StringComparison pathComparison = GetPathComparison();
		if (!fullPath2.StartsWith(value, pathComparison) && !string.Equals(fullPath2, fullPath, pathComparison))
		{
			throw new ArgumentException("Path '" + path + "' escapes the Unity project root and is not allowed.", "path");
		}
		return fullPath2;
	}

	private static IEnumerator RunExclusiveCapture(IEnumerator inner, Action<Exception> onUnhandledError)
	{
		while (_captureInFlight || Time.realtimeSinceStartup < _nextCaptureAllowedAt)
		{
			yield return null;
		}
		_captureInFlight = true;
		try
		{
			while (true)
			{
				bool flag;
				try
				{
					flag = inner.MoveNext();
				}
				catch (Exception obj)
				{
					onUnhandledError?.Invoke(obj);
					break;
				}
				if (flag)
				{
					yield return inner.Current;
					continue;
				}
				break;
			}
		}
		finally
		{
			_captureInFlight = false;
			_nextCaptureAllowedAt = Time.realtimeSinceStartup + 0.12f;
		}
	}

	private static IEnumerator CaptureRoutine(int targetWidth, int targetHeight, int maxFramesToWait, TaskCompletionSource<byte[]> tcs)
	{
		Texture2D texture2D = null;
		Exception error = null;
		byte[] bytes = null;
		int prevAA = QualitySettings.antiAliasing;
		if (prevAA != 0)
		{
			QualitySettings.antiAliasing = 0;
		}
		try
		{
			for (int i = 0; i < maxFramesToWait; i++)
			{
				yield return new WaitForEndOfFrame();
				texture2D = TryCaptureScreenshotAsTextureSafe();
				if (texture2D != null)
				{
					break;
				}
			}
			try
			{
				if (texture2D == null)
				{
					throw new Exception($"CaptureScreenshotAsTexture returned null (after retries). Screen={Screen.width}x{Screen.height} frame={Time.frameCount}.");
				}
				bytes = ((texture2D.width == targetWidth && texture2D.height == targetHeight) ? texture2D.EncodeToPNG() : ResizeToPng(texture2D, targetWidth, targetHeight));
			}
			catch (Exception ex)
			{
				error = ex;
			}
			finally
			{
				if (texture2D != null)
				{
					UnityEngine.Object.Destroy(texture2D);
				}
			}
		}
		finally
		{
			if (QualitySettings.antiAliasing != prevAA)
			{
				QualitySettings.antiAliasing = prevAA;
			}
		}
		if (error != null)
		{
			tcs.TrySetException(error);
		}
		else
		{
			tcs.TrySetResult(bytes);
		}
	}

	public static CameraPreviewContext CreateCameraPreviewContext(GameObject instance, int width, int height, Color background, string cropMode, float padding, HideFlags hideFlags, int? previewLayer = null, string rootName = "__UnityMcpRuntimePreview__", string cameraName = "__PreviewCamera__", string canvasName = "__PreviewCanvas__")
	{
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		CameraPreviewContext cameraPreviewContext = new CameraPreviewContext();
		cameraPreviewContext.root = new GameObject(rootName);
		cameraPreviewContext.root.hideFlags = hideFlags;
		GameObject gameObject = new GameObject(cameraName);
		gameObject.hideFlags = hideFlags;
		gameObject.transform.SetParent(cameraPreviewContext.root.transform, worldPositionStays: false);
		cameraPreviewContext.camera = gameObject.AddComponent<Camera>();
		cameraPreviewContext.camera.orthographic = true;
		cameraPreviewContext.camera.clearFlags = CameraClearFlags.Color;
		cameraPreviewContext.camera.backgroundColor = background;
		cameraPreviewContext.camera.cullingMask = -1;
		cameraPreviewContext.camera.nearClipPlane = 0.1f;
		cameraPreviewContext.camera.farClipPlane = 100f;
		cameraPreviewContext.camera.allowMSAA = false;
		cameraPreviewContext.camera.allowHDR = false;
		cameraPreviewContext.camera.forceIntoRenderTexture = true;
		cameraPreviewContext.camera.pixelRect = new Rect(0f, 0f, width, height);
		cameraPreviewContext.renderTexture = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
		cameraPreviewContext.camera.targetTexture = cameraPreviewContext.renderTexture;
		instance.hideFlags = hideFlags;
		cameraPreviewContext.previewTarget = ResolvePreviewTarget(instance);
		if (ShouldUsePrefabCanvasAsPreviewRoot(instance, cameraPreviewContext.previewTarget))
		{
			instance.transform.SetParent(cameraPreviewContext.root.transform, worldPositionStays: false);
		}
		else
		{
			cameraPreviewContext.canvasGO = new GameObject(canvasName, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
			cameraPreviewContext.canvasGO.hideFlags = hideFlags;
			cameraPreviewContext.canvasGO.transform.SetParent(cameraPreviewContext.root.transform, worldPositionStays: false);
			Canvas component = cameraPreviewContext.canvasGO.GetComponent<Canvas>();
			component.renderMode = RenderMode.ScreenSpaceCamera;
			component.worldCamera = cameraPreviewContext.camera;
			component.planeDistance = 1f;
			cameraPreviewContext.canvasGO.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
			CanvasScaler component2 = cameraPreviewContext.canvasGO.GetComponent<CanvasScaler>();
			component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component2.referenceResolution = new Vector2(width, height);
			component2.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			component2.referencePixelsPerUnit = 100f;
			instance.transform.SetParent(cameraPreviewContext.canvasGO.transform, worldPositionStays: false);
		}
		if (previewLayer.HasValue && previewLayer.Value >= 0 && previewLayer.Value <= 31)
		{
			SetLayerRecursively(cameraPreviewContext.root, previewLayer.Value);
			cameraPreviewContext.camera.cullingMask = 1 << previewLayer.Value;
		}
		ForceAllCanvasesToCamera(cameraPreviewContext.previewTarget, cameraPreviewContext.camera);
		RectTransform rectTransform = ((cameraPreviewContext.previewTarget != null) ? cameraPreviewContext.previewTarget.GetComponent<RectTransform>() : null);
		PrepareRectTransformForStandardPreview(rectTransform);
		NormalizeCanvasPreviewTarget(cameraPreviewContext.previewTarget, width, height);
		Transform canvasTransform = ((cameraPreviewContext.canvasGO != null) ? cameraPreviewContext.canvasGO.transform : ((cameraPreviewContext.previewTarget != null) ? cameraPreviewContext.previewTarget.transform : cameraPreviewContext.root.transform));
		Canvas.ForceUpdateCanvases();
		if (rectTransform != null)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
		}
		Canvas.ForceUpdateCanvases();
		ConfigurePreviewCameraFraming(cameraPreviewContext.camera, canvasTransform, rectTransform, cropMode, padding, width, height);
		return cameraPreviewContext;
	}

	private static bool ShouldUsePrefabCanvasAsPreviewRoot(GameObject instance, GameObject previewTarget)
	{
		if (instance == null || previewTarget == null)
		{
			return false;
		}
		if (previewTarget.GetComponent<Canvas>() == null)
		{
			return false;
		}
		if ((object)previewTarget == instance)
		{
			return true;
		}
		if (previewTarget.transform.parent == instance.transform)
		{
			return instance.transform.childCount == 1;
		}
		return false;
	}

	public static GameObject ResolvePreviewTarget(GameObject instance)
	{
		if (instance == null)
		{
			return null;
		}
		if (instance.GetComponent<Canvas>() != null)
		{
			return instance;
		}
		Transform transform = instance.transform;
		if (transform.childCount == 1)
		{
			Transform child = transform.GetChild(0);
			if (child != null && child.GetComponent<Canvas>() != null)
			{
				return child.gameObject;
			}
		}
		return instance;
	}

	public static void CleanupCameraPreviewContext(CameraPreviewContext context, bool immediate)
	{
		if (context == null)
		{
			return;
		}
		if (context.root != null)
		{
			if (immediate)
			{
				UnityEngine.Object.DestroyImmediate(context.root);
			}
			else
			{
				UnityEngine.Object.Destroy(context.root);
			}
		}
		if (context.renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(context.renderTexture);
		}
	}

	public static void ConfigurePreviewCameraFraming(Camera previewCamera, Transform canvasTransform, RectTransform instanceRT, string cropMode, float padding, int width, int height)
	{
		if (previewCamera == null)
		{
			throw new ArgumentNullException("previewCamera");
		}
		if (canvasTransform == null)
		{
			throw new ArgumentNullException("canvasTransform");
		}
		if (string.Equals(cropMode, "canvas", StringComparison.OrdinalIgnoreCase))
		{
			Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(canvasTransform);
			float num = (float)width / (float)height;
			float num2 = Mathf.Max(bounds.extents.y, bounds.extents.x / num);
			num2 *= 1f + Mathf.Clamp01(padding);
			previewCamera.aspect = num;
			previewCamera.orthographicSize = Mathf.Max(1f, num2);
			previewCamera.transform.position = bounds.center + new Vector3(0f, 0f, -10f);
			previewCamera.transform.rotation = Quaternion.identity;
		}
		else
		{
			Bounds bounds2 = CalculateBounds(canvasTransform, instanceRT, cropMode);
			float num3 = (float)width / (float)height;
			float num4 = Mathf.Max(bounds2.extents.y, bounds2.extents.x / num3);
			num4 *= 1f + Mathf.Clamp01(padding);
			previewCamera.orthographicSize = Mathf.Max(1f, num4);
			previewCamera.transform.position = bounds2.center + new Vector3(0f, 0f, -10f);
			previewCamera.transform.rotation = Quaternion.identity;
		}
	}

	public static void CaptureCameraPreviewToPng(Camera previewCamera, RenderTexture renderTexture, string resolvedOutPngPath, string prefabName, int width, int height, Color background, string mode, bool destroyImmediate = false)
	{
		if (previewCamera == null)
		{
			throw new ArgumentNullException("previewCamera");
		}
		if (renderTexture == null)
		{
			throw new ArgumentNullException("renderTexture");
		}
		if (string.IsNullOrEmpty(resolvedOutPngPath))
		{
			throw new ArgumentException("resolvedOutPngPath is required", "resolvedOutPngPath");
		}
		Texture2D texture2D = null;
		try
		{
			RenderTexture active = RenderTexture.active;
			try
			{
				previewCamera.Render();
				RenderTexture.active = renderTexture;
				texture2D = new Texture2D(width, height, TextureFormat.RGBA32, mipChain: false);
				texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
				texture2D.Apply();
			}
			finally
			{
				RenderTexture.active = active;
			}
			byte[] bytes = texture2D.EncodeToPNG();
			Directory.CreateDirectory(Path.GetDirectoryName(resolvedOutPngPath) ?? ".");
			File.WriteAllBytes(resolvedOutPngPath, bytes);
		}
		finally
		{
			if (texture2D != null)
			{
				if (destroyImmediate)
				{
					UnityEngine.Object.DestroyImmediate(texture2D);
				}
				else
				{
					UnityEngine.Object.Destroy(texture2D);
				}
			}
		}
	}

	private static IEnumerator RenderPrefabCameraRoutine(GameObject prefab, string outPngPath, int width, int height, Color background, string cropMode, float padding, int maxFramesToWait, int previewLayer, TaskCompletionSource<RuntimePreviewResult> tcs)
	{
		CameraPreviewContext previewContext = null;
		Exception error = null;
		int prevAA = QualitySettings.antiAliasing;
		string resolvedOutPngPath = null;
		if (prevAA != 0)
		{
			QualitySettings.antiAliasing = 0;
		}
		try
		{
			try
			{
				resolvedOutPngPath = ResolveProjectOutputPath(outPngPath);
				GameObject instance = UnityEngine.Object.Instantiate(prefab);
				previewContext = CreateCameraPreviewContext(instance, width, height, background, cropMode, padding, HideFlags.None, previewLayer);
			}
			catch (Exception ex)
			{
				error = ex;
			}
			if (error != null)
			{
				yield break;
			}
			yield return null;
			for (int i = 0; i < maxFramesToWait; i++)
			{
				yield return new WaitForEndOfFrame();
			}
			Canvas.ForceUpdateCanvases();
			if (previewContext == null || previewContext.camera == null)
			{
				error = new Exception("Preview camera was not created.");
				yield break;
			}
			if (previewContext.renderTexture == null)
			{
				error = new Exception("Preview render texture was not created.");
				yield break;
			}
			try
			{
				CaptureCameraPreviewToPng(previewContext.camera, previewContext.renderTexture, resolvedOutPngPath, (prefab != null) ? prefab.name : null, width, height, background, "play_camera_rt");
			}
			catch (Exception ex2)
			{
				error = ex2;
			}
		}
		finally
		{
			CleanupCameraPreviewContext(previewContext, immediate: false);
			tcs.TrySetResult((error != null) ? new RuntimePreviewResult
			{
				ok = false,
				pngPath = outPngPath,
				error = "exception",
				detail = error.ToString()
			} : new RuntimePreviewResult
			{
				ok = true,
				pngPath = outPngPath
			});
			if (QualitySettings.antiAliasing != prevAA)
			{
				QualitySettings.antiAliasing = prevAA;
			}
		}
	}

	public static void ForceAllCanvasesToCamera(GameObject root, Camera camera)
	{
		if (root == null || camera == null)
		{
			return;
		}
		Canvas[] componentsInChildren = root.GetComponentsInChildren<Canvas>(includeInactive: true);
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		Canvas[] array = componentsInChildren;
		foreach (Canvas canvas in array)
		{
			if (!(canvas == null))
			{
				canvas.renderMode = RenderMode.ScreenSpaceCamera;
				canvas.worldCamera = camera;
				canvas.planeDistance = 1f;
			}
		}
	}

	public static void ForceAllCanvasesToOverlay(GameObject root, int baseSortingOrder)
	{
		if (root == null)
		{
			return;
		}
		Canvas[] componentsInChildren = root.GetComponentsInChildren<Canvas>(includeInactive: true);
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		foreach (Canvas canvas in componentsInChildren)
		{
			if (!(canvas == null))
			{
				int sortingOrder = canvas.sortingOrder;
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				canvas.worldCamera = null;
				canvas.planeDistance = 0f;
				canvas.overrideSorting = true;
				canvas.sortingOrder = baseSortingOrder + sortingOrder;
			}
		}
	}

	private static void SetLayerRecursively(GameObject go, int layer)
	{
		if (!(go == null))
		{
			go.layer = layer;
			Transform transform = go.transform;
			for (int i = 0; i < transform.childCount; i++)
			{
				SetLayerRecursively(transform.GetChild(i).gameObject, layer);
			}
		}
	}

	public static void PrepareRectTransformForStandardPreview(RectTransform instanceRT)
	{
		if (!(instanceRT == null))
		{
			instanceRT.localScale = Vector3.one;
			instanceRT.localRotation = Quaternion.identity;
		}
	}

	public static void NormalizeCanvasPreviewTarget(GameObject previewTarget, int width, int height)
	{
		if (previewTarget == null || previewTarget.GetComponent<Canvas>() == null)
		{
			return;
		}
		RectTransform component = previewTarget.GetComponent<RectTransform>();
		if (!(component == null))
		{
			Vector2 sizeDelta = component.rect.size;
			if (sizeDelta.x < 1f || sizeDelta.y < 1f)
			{
				sizeDelta = new Vector2(Mathf.Max(1f, width), Mathf.Max(1f, height));
			}
			component.anchorMin = new Vector2(0.5f, 0.5f);
			component.anchorMax = new Vector2(0.5f, 0.5f);
			component.pivot = new Vector2(0.5f, 0.5f);
			component.anchoredPosition = Vector2.zero;
			component.sizeDelta = sizeDelta;
			component.localPosition = Vector3.zero;
		}
	}

	private static IEnumerator RenderPrefabScreenshotRoutine(GameObject prefab, string outPngPath, int width, int height, Color background, string cropMode, float padding, int maxFramesToWait, int previewLayer, TaskCompletionSource<RuntimePreviewResult> tcs)
	{
		GameObject previewRoot = null;
		GameObject backdrop = null;
		GameObject instance = null;
		Texture2D captured = null;
		try
		{
			string resolvedOutPngPath = ResolveProjectOutputPath(outPngPath);
			previewRoot = new GameObject("__UnityMcpRuntimeScreenshotPreview__", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))
			{
				hideFlags = HideFlags.DontSave
			};
			Canvas component = previewRoot.GetComponent<Canvas>();
			component.renderMode = RenderMode.ScreenSpaceOverlay;
			component.overrideSorting = true;
			component.sortingOrder = 32757;
			CanvasScaler component2 = previewRoot.GetComponent<CanvasScaler>();
			component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component2.referenceResolution = new Vector2(width, height);
			component2.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			component2.referencePixelsPerUnit = 100f;
			RectTransform component3 = previewRoot.GetComponent<RectTransform>();
			component3.anchorMin = Vector2.zero;
			component3.anchorMax = Vector2.one;
			component3.pivot = new Vector2(0.5f, 0.5f);
			component3.anchoredPosition = Vector2.zero;
			component3.sizeDelta = Vector2.zero;
			component3.localScale = Vector3.one;
			component3.localRotation = Quaternion.identity;
			backdrop = new GameObject("__UnityMcpRuntimeScreenshotBackdrop__", typeof(RectTransform), typeof(Image));
			backdrop.hideFlags = HideFlags.DontSave;
			backdrop.transform.SetParent(previewRoot.transform, worldPositionStays: false);
			RectTransform component4 = backdrop.GetComponent<RectTransform>();
			component4.anchorMin = Vector2.zero;
			component4.anchorMax = Vector2.one;
			component4.pivot = new Vector2(0.5f, 0.5f);
			component4.anchoredPosition = Vector2.zero;
			component4.sizeDelta = Vector2.zero;
			Image backdropImage = backdrop.GetComponent<Image>();
			backdropImage.color = PreviewBackdropBlack;
			backdropImage.raycastTarget = false;
			instance = UnityEngine.Object.Instantiate(prefab);
			instance.hideFlags = HideFlags.DontSave;
			instance.transform.SetParent(previewRoot.transform, worldPositionStays: false);
			GameObject gameObject = ResolvePreviewTarget(instance);
			RectTransform rectTransform = ((gameObject != null) ? gameObject.GetComponent<RectTransform>() : null);
			PrepareRectTransformForStandardPreview(rectTransform);
			NormalizeCanvasPreviewTarget(gameObject, width, height);
			ForceAllCanvasesToOverlay(instance, component.sortingOrder + 1);
			if (rectTransform != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}
			Canvas.ForceUpdateCanvases();
			yield return null;
			for (int i = 0; i < maxFramesToWait; i++)
			{
				yield return new WaitForEndOfFrame();
			}
			Canvas.ForceUpdateCanvases();
			Texture2D capturedOnBlack = TryCaptureScreenshotAsTextureSafe();
			if (capturedOnBlack == null)
			{
				throw new Exception($"CaptureScreenshotAsTexture returned null. Screen={Screen.width}x{Screen.height} frame={Time.frameCount}.");
			}
			backdropImage.color = PreviewBackdropWhite;
			Canvas.ForceUpdateCanvases();
			yield return null;
			yield return new WaitForEndOfFrame();
			Texture2D texture2D = TryCaptureScreenshotAsTextureSafe();
			if (texture2D == null)
			{
				UnityEngine.Object.Destroy(capturedOnBlack);
				throw new Exception($"CaptureScreenshotAsTexture returned null for white backdrop. Screen={Screen.width}x{Screen.height} frame={Time.frameCount}.");
			}
			captured = ReconstructTransparencyFromBlackAndWhite(capturedOnBlack, texture2D);
			UnityEngine.Object.Destroy(capturedOnBlack);
			UnityEngine.Object.Destroy(texture2D);
			byte[] bytes = ((captured.width == width && captured.height == height) ? captured.EncodeToPNG() : CropAndResizeToPng(captured, width, height));
			Directory.CreateDirectory(Path.GetDirectoryName(resolvedOutPngPath) ?? ".");
			File.WriteAllBytes(resolvedOutPngPath, bytes);
			tcs.TrySetResult(new RuntimePreviewResult
			{
				ok = true,
				pngPath = outPngPath
			});
		}
		finally
		{
			if (captured != null)
			{
				UnityEngine.Object.Destroy(captured);
			}
			if (instance != null)
			{
				UnityEngine.Object.Destroy(instance);
			}
			if (backdrop != null)
			{
				UnityEngine.Object.Destroy(backdrop);
			}
			if (previewRoot != null)
			{
				UnityEngine.Object.Destroy(previewRoot);
			}
		}
	}

	private static Texture2D ReconstructTransparencyFromBlackAndWhite(Texture2D blackCapture, Texture2D whiteCapture)
	{
		if (blackCapture == null)
		{
			throw new ArgumentNullException("blackCapture");
		}
		if (whiteCapture == null)
		{
			throw new ArgumentNullException("whiteCapture");
		}
		if (blackCapture.width != whiteCapture.width || blackCapture.height != whiteCapture.height)
		{
			throw new ArgumentException("Black and white captures must have the same dimensions.");
		}
		Color32[] pixels = blackCapture.GetPixels32();
		Color32[] pixels2 = whiteCapture.GetPixels32();
		Texture2D texture2D = new Texture2D(blackCapture.width, blackCapture.height, TextureFormat.RGBA32, mipChain: false);
		Color32[] array = new Color32[pixels.Length];
		for (int i = 0; i < pixels.Length; i++)
		{
			Color32 color = pixels[i];
			Color32 color2 = pixels2[i];
			float num = (float)(int)color.r / 255f;
			float num2 = (float)(int)color.g / 255f;
			float num3 = (float)(int)color.b / 255f;
			float num4 = (float)(int)color2.r / 255f;
			float num5 = (float)(int)color2.g / 255f;
			float num6 = (float)(int)color2.b / 255f;
			float value = 1f - Mathf.Max(num4 - num, Mathf.Max(num5 - num2, num6 - num3));
			value = Mathf.Clamp01(value);
			if (value <= 0.0001f)
			{
				array[i] = new Color32(0, 0, 0, 0);
				continue;
			}
			float r = Mathf.Clamp01(num / value);
			float g = Mathf.Clamp01(num2 / value);
			float b = Mathf.Clamp01(num3 / value);
			array[i] = new Color(r, g, b, value);
		}
		texture2D.SetPixels32(array);
		texture2D.Apply();
		return texture2D;
	}

	public static Bounds CalculateBounds(Transform canvasTransform, RectTransform instanceRT, string cropMode)
	{
		if (string.Equals(cropMode, "canvas", StringComparison.OrdinalIgnoreCase))
		{
			return RectTransformUtility.CalculateRelativeRectTransformBounds(canvasTransform);
		}
		if (instanceRT == null)
		{
			return RectTransformUtility.CalculateRelativeRectTransformBounds(canvasTransform);
		}
		Bounds result = RectTransformUtility.CalculateRelativeRectTransformBounds(canvasTransform, instanceRT);
		if (result.size.sqrMagnitude < 0.0001f)
		{
			return RectTransformUtility.CalculateRelativeRectTransformBounds(canvasTransform);
		}
		return result;
	}

	private static Texture2D TryCaptureScreenshotAsTextureSafe()
	{
		try
		{
			return ScreenCapture.CaptureScreenshotAsTexture();
		}
		catch
		{
			return null;
		}
	}

	private static byte[] CropAndResizeToPng(Texture2D source, int targetWidth, int targetHeight)
	{
		int width = source.width;
		int height = source.height;
		if (width <= 0 || height <= 0)
		{
			throw new Exception("Invalid source texture size.");
		}
		float num = (float)targetWidth / (float)targetHeight;
		int num2;
		int num3;
		int x;
		int y;
		if ((float)width / (float)height > num)
		{
			num2 = height;
			num3 = Mathf.RoundToInt((float)num2 * num);
			x = (width - num3) / 2;
			y = 0;
		}
		else
		{
			num3 = width;
			num2 = Mathf.RoundToInt((float)num3 / num);
			x = 0;
			y = (height - num2) / 2;
		}
		Texture2D texture2D = new Texture2D(num3, num2, TextureFormat.RGBA32, mipChain: false);
		try
		{
			Color[] pixels = source.GetPixels(x, y, num3, num2);
			texture2D.SetPixels(pixels);
			texture2D.Apply();
			return (num3 == targetWidth && num2 == targetHeight) ? texture2D.EncodeToPNG() : ResizeToPng(texture2D, targetWidth, targetHeight);
		}
		finally
		{
			UnityEngine.Object.Destroy(texture2D);
		}
	}

	private static byte[] ResizeToPng(Texture2D source, int targetWidth, int targetHeight)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
		RenderTexture active = RenderTexture.active;
		try
		{
			Graphics.Blit(source, temporary);
			RenderTexture.active = temporary;
			Texture2D texture2D = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, mipChain: false);
			try
			{
				texture2D.ReadPixels(new Rect(0f, 0f, targetWidth, targetHeight), 0, 0);
				texture2D.Apply();
				return texture2D.EncodeToPNG();
			}
			finally
			{
				UnityEngine.Object.Destroy(texture2D);
			}
		}
		finally
		{
			RenderTexture.active = active;
			RenderTexture.ReleaseTemporary(temporary);
		}
	}

	internal static int SampleNonBackground(Texture2D tex, Color background)
	{
		if (tex == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 1; i <= 4; i++)
		{
			for (int j = 1; j <= 4; j++)
			{
				int x = Mathf.Clamp(tex.width * j / 5, 0, tex.width - 1);
				int y = Mathf.Clamp(tex.height * i / 5, 0, tex.height - 1);
				if (!Approximately(tex.GetPixel(x, y), background))
				{
					num++;
				}
			}
		}
		return num;
	}

	private static bool Approximately(Color a, Color b)
	{
		if (Mathf.Abs(a.r - b.r) < 0.01f && Mathf.Abs(a.g - b.g) < 0.01f && Mathf.Abs(a.b - b.b) < 0.01f)
		{
			return Mathf.Abs(a.a - b.a) < 0.01f;
		}
		return false;
	}

	private static string ColorToRgba(Color c)
	{
		int num = Mathf.RoundToInt(c.r * 255f);
		int num2 = Mathf.RoundToInt(c.g * 255f);
		int num3 = Mathf.RoundToInt(c.b * 255f);
		int num4 = Mathf.RoundToInt(c.a * 255f);
		return $"{num:X2}{num2:X2}{num3:X2}{num4:X2}";
	}
}

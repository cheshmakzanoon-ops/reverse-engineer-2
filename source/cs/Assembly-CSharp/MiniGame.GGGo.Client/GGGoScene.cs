using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.GGGo.Client;

public class GGGoScene : MonoBehaviour
{
	public Transform MapRoot;

	public Transform PlayerRoot;

	public Transform EffectRoot;

	public Transform DynamicRoot;

	public GGGoRollBackground RollRoot;

	public Camera Camera;

	public Transform BgStart;

	public float OrthographicScale;

	public Transform LevelRoot { get; set; }

	public float Gfx2LogicScale { get; private set; }

	public float Logic2GfxScale => 1f / Gfx2LogicScale;

	public float Physics2GfxScale { get; private set; }

	public float PreUnit { get; private set; }

	public void AdjustScale()
	{
		if (LevelRoot == null)
		{
			LevelRoot = base.gameObject.transform;
		}
		float num = (PreUnit = Object.FindObjectOfType<CanvasScaler>().referencePixelsPerUnit);
		PlayerRoot.localScale = new Vector3(num, num, num);
		DynamicRoot = base.transform.Find("DynamicRoot");
		DynamicRoot.localScale = new Vector3(num, num, num);
		Gfx2LogicScale = (base.transform.parent.lossyScale * num).x;
		Physics2GfxScale = Logic2GfxScale * num * OrthographicScale;
	}
}

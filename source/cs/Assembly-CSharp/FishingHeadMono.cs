using Protobuf;
using UnityEngine;

public class FishingHeadMono : MonoBehaviour
{
	[SerializeField]
	private UIPlayerHead head;

	public void SetData(FishPlayerInfo data)
	{
		head.SetData(data.Uid, data.Pic, data.PicVer);
	}
}

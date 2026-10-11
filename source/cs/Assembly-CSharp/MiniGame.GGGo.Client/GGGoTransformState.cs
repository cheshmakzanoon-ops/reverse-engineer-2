using UnityEngine;

namespace MiniGame.GGGo.Client;

public struct GGGoTransformState : IGGGoCacheableResourceState
{
	public Vector3 Scale;

	public Vector3 Rotation;

	public Vector3 Position;
}

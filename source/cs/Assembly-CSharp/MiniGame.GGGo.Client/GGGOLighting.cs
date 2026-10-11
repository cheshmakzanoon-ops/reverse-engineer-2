using UnityEngine;

namespace MiniGame.GGGo.Client;

public class GGGOLighting : MonoBehaviour
{
	public Transform Target;

	public Vector3 Offset;

	public float Scale;

	public string ShaderProp = "_HeroPos0";

	private void LateUpdate()
	{
		if (Target == null)
		{
			Target = base.transform;
		}
		Vector3 vector = (Target.position + Offset) * Scale;
		Shader.SetGlobalVector(ShaderProp, vector);
	}
}

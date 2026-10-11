using Leopotam.EcsLite;
using UnityEngine;

namespace MiniGame.GGGo.Client;

[ExecuteInEditMode]
public class WorldDrawManager : MonoBehaviour
{
	private static WorldDrawManager ms_instance;

	private MiniBox2DGame _miniGame;

	private float _scale = 1f;

	public static void SetupDebugWorld(MiniBox2DGame world)
	{
		if (ms_instance == null)
		{
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			ms_instance = main.gameObject.AddComponent<WorldDrawManager>();
		}
		ms_instance._miniGame = world;
		ms_instance._scale = 1f;
	}

	public static void SetupDebugWorld(EcsWorld world, GameObject root, float scale, bool useCustomColor = false, Color color = default(Color))
	{
		if (!(root == null))
		{
			if (!root.TryGetComponent<WorldDrawManager>(out var component))
			{
				component = root.AddComponent<WorldDrawManager>();
			}
			component._scale = scale;
			component._miniGame = new MiniBox2DGame
			{
				World = world,
				Scale = scale
			};
		}
	}

	private void OnDrawGizmos()
	{
		if (_miniGame != null && _miniGame != null)
		{
			_miniGame.DebugDraw();
		}
	}
}

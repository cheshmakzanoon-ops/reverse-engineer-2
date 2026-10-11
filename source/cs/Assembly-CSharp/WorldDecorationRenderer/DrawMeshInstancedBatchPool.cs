using System.Collections.Generic;
using UnityEngine;

namespace WorldDecorationRenderer;

public static class DrawMeshInstancedBatchPool
{
	public const int MatrixMaxCount = 512;

	private static readonly List<Matrix4x4[]> matricesPool = new List<Matrix4x4[]>(8);

	private static readonly List<float[]> floatsPool = new List<float[]>(8);

	private static readonly List<Vector4[]> vector4Pool = new List<Vector4[]>(8);

	public static Matrix4x4[] GetMatrices()
	{
		int count = matricesPool.Count;
		if (count == 0)
		{
			return new Matrix4x4[512];
		}
		int index = count - 1;
		Matrix4x4[] result = matricesPool[index];
		matricesPool.RemoveAt(index);
		return result;
	}

	public static void ReleaseMatrices(Matrix4x4[] array)
	{
		if (array != null)
		{
			matricesPool.Add(array);
		}
	}

	public static float[] GetFloats()
	{
		int count = floatsPool.Count;
		if (count == 0)
		{
			return new float[512];
		}
		int index = count - 1;
		float[] result = floatsPool[index];
		floatsPool.RemoveAt(index);
		return result;
	}

	public static void ReleaseFloats(float[] array)
	{
		if (array != null)
		{
			floatsPool.Add(array);
		}
	}

	public static Vector4[] GetVector4()
	{
		int count = vector4Pool.Count;
		if (count == 0)
		{
			return new Vector4[512];
		}
		int index = count - 1;
		Vector4[] result = vector4Pool[index];
		vector4Pool.RemoveAt(index);
		return result;
	}

	public static void ReleaseVector4(Vector4[] array)
	{
		if (array != null)
		{
			vector4Pool.Add(array);
		}
	}
}

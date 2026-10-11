using System.Collections.Generic;
using UnityEngine;

namespace WorldDecorationRenderer;

public class DrawMeshInstancedBatch
{
	public const int MatrixMaxCount = 512;

	private static readonly int AtlasIndexID = Shader.PropertyToID("_AtlasIndex");

	private static readonly int AtlasSTID = Shader.PropertyToID("_AtlasST");

	public int lastUsedFrameId = -1;

	public readonly MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

	public readonly DrawMeshInstGraphic graphicsInfo;

	public readonly List<Matrix4x4[]> matrices = new List<Matrix4x4[]>(2);

	public readonly List<int> counts = new List<int>(2);

	public readonly List<float[]> bufferFloat = new List<float[]>(2);

	public readonly List<Vector4[]> bufferVector4 = new List<Vector4[]>(2);

	public DrawMeshInstancedBatch(DrawMeshInstGraphic graphicsInfo)
	{
		this.graphicsInfo = graphicsInfo;
	}

	public void AddBuffer(Matrix4x4 matrix)
	{
		int num = matrices.Count - 1;
		if (num < 0 || counts[num] >= 512)
		{
			matrices.Add(DrawMeshInstancedBatchPool.GetMatrices());
			counts.Add(0);
			num = matrices.Count - 1;
		}
		int num2 = counts[num];
		matrices[num][num2] = matrix;
		counts[num] = num2 + 1;
	}

	public void AddBuffer(Matrix4x4 matrix, int atlasIndex)
	{
		int num = matrices.Count - 1;
		if (num < 0 || counts[num] >= 512)
		{
			matrices.Add(DrawMeshInstancedBatchPool.GetMatrices());
			counts.Add(0);
			bufferFloat.Add(DrawMeshInstancedBatchPool.GetFloats());
			num = matrices.Count - 1;
		}
		int num2 = counts[num];
		matrices[num][num2] = matrix;
		counts[num] = num2 + 1;
		bufferFloat[num][num2] = atlasIndex;
	}

	public void AddBuffer(Matrix4x4 matrix, Vector4 v4)
	{
		int num = matrices.Count - 1;
		if (num < 0 || counts[num] >= 512)
		{
			matrices.Add(DrawMeshInstancedBatchPool.GetMatrices());
			counts.Add(0);
			bufferVector4.Add(DrawMeshInstancedBatchPool.GetVector4());
			num = matrices.Count - 1;
		}
		int num2 = counts[num];
		matrices[num][num2] = matrix;
		counts[num] = num2 + 1;
		bufferVector4[num][num2] = v4;
	}

	public void SetPropertyBuffer(int batchIndex)
	{
		propertyBlock.Clear();
		if (batchIndex >= 0 && batchIndex < bufferFloat.Count)
		{
			propertyBlock.SetFloatArray(AtlasIndexID, bufferFloat[batchIndex]);
		}
	}

	public void SetPropertyBufferVector4(int batchIndex)
	{
		propertyBlock.Clear();
		if (batchIndex >= 0 && batchIndex < bufferVector4.Count)
		{
			propertyBlock.SetVectorArray(AtlasSTID, bufferVector4[batchIndex]);
		}
	}

	public void ClearData()
	{
		for (int i = 0; i < matrices.Count; i++)
		{
			DrawMeshInstancedBatchPool.ReleaseMatrices(matrices[i]);
		}
		matrices.Clear();
		counts.Clear();
		for (int j = 0; j < bufferFloat.Count; j++)
		{
			DrawMeshInstancedBatchPool.ReleaseFloats(bufferFloat[j]);
		}
		bufferFloat.Clear();
		for (int k = 0; k < bufferVector4.Count; k++)
		{
			DrawMeshInstancedBatchPool.ReleaseVector4(bufferVector4[k]);
		}
		bufferVector4.Clear();
		propertyBlock.Clear();
	}
}

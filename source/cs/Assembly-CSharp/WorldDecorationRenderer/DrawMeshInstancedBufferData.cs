using System.Collections.Generic;

namespace WorldDecorationRenderer;

public class DrawMeshInstancedBufferData
{
	public readonly List<DrawMeshInstancedBatch> batches = new List<DrawMeshInstancedBatch>(128);

	public readonly List<int> activeBatchIndices = new List<int>(128);

	private readonly Dictionary<DrawKey, int> _keyToIndex = new Dictionary<DrawKey, int>(128);

	private readonly Stack<int> _freeIndices = new Stack<int>(32);

	private int _frameId;

	public void BeginFrame()
	{
		_frameId++;
		for (int i = 0; i < activeBatchIndices.Count; i++)
		{
			int num = activeBatchIndices[i];
			((num >= 0 && num < batches.Count) ? batches[num] : null)?.ClearData();
		}
		activeBatchIndices.Clear();
	}

	public DrawMeshInstancedBatch GetOrCreateBatch(in DrawKey drawKey, DrawMeshInstGraphic g)
	{
		if (_keyToIndex.TryGetValue(drawKey, out var value))
		{
			DrawMeshInstancedBatch drawMeshInstancedBatch = batches[value];
			if (drawMeshInstancedBatch.lastUsedFrameId != _frameId)
			{
				drawMeshInstancedBatch.lastUsedFrameId = _frameId;
				activeBatchIndices.Add(value);
			}
			return drawMeshInstancedBatch;
		}
		int num = ((_freeIndices.Count > 0) ? _freeIndices.Pop() : batches.Count);
		DrawMeshInstancedBatch drawMeshInstancedBatch2 = new DrawMeshInstancedBatch(g)
		{
			lastUsedFrameId = _frameId
		};
		if (num == batches.Count)
		{
			batches.Add(drawMeshInstancedBatch2);
		}
		else
		{
			batches[num] = drawMeshInstancedBatch2;
		}
		_keyToIndex.Add(drawKey, num);
		activeBatchIndices.Add(num);
		return drawMeshInstancedBatch2;
	}

	public void RemoveBatch(in DrawKey drawKey)
	{
		if (_keyToIndex.TryGetValue(drawKey, out var value))
		{
			batches[value]?.ClearData();
			batches[value] = null;
			_keyToIndex.Remove(drawKey);
			_freeIndices.Push(value);
		}
	}
}

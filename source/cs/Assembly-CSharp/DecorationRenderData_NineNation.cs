using System;
using System.Collections.Generic;
using UnityEngine;

public class DecorationRenderData_NineNation : ScriptableObject
{
	[Serializable]
	public class AtlasInfo
	{
		public string atlasName;

		public List<Vector4> stList;

		public List<string> textureGuidList;
	}

	public List<int> guidList;

	public List<string> assetPathList;

	public List<string> prefabPathList;

	public List<AtlasInfo> atlasList;
}

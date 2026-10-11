using System;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public interface IGameSaveFilter
{
	void Init(GameWorld world);

	bool FilterEntity(EcsEntitySnapshot snapshot);

	bool FilterComponent(int entity, int pool, Type type, ref object data);
}

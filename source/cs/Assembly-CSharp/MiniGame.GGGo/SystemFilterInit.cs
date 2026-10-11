using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemFilterInit : IEcsInitSystem, IEcsSystem
{
	private EcsFilterInject<Inc<ComponentRegion>> _filterRegion;

	private EcsFilterInject<Inc<ComponentData>> _filterData;

	private EcsFilterInject<Inc<ComponentRegionSpawned>> _filterSwawned;

	private EcsFilterInject<Inc<ComponentPlayer>> _filterPlayer;

	private EcsFilterInject<Inc<ComponentCollider>> _filterCollider;

	private EcsFilterInject<Inc<ComponentColliderTrigger>> _filterColliderTrigger;

	private EcsFilterInject<Inc<ComponentResource>, Exc<ComponentUniqueIDManager, ComponentActivedUniqueID>> _filterVerify;

	private EcsFilterInject<Inc<ComponentPlayer, ComponentPosition>> _filterPlayerPos;

	public void Init(IEcsSystems systems)
	{
	}
}

using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.GGGo;

[TitleAndCategory("碰撞的层", "碰撞")]
public struct CollectionLayerCondition : ICondition
{
	public ColliderLayer Layer;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		CollectionLayerCondition collectionLayerCondition = ((condition is CollectionLayerCondition) ? ((CollectionLayerCondition)(object)condition) : default(CollectionLayerCondition));
		EcsPool<ComponentCollider> pool = world.GetPool<ComponentCollider>();
		EcsPackedEntity ecsPackedEntity = world.PackEntity(entity);
		EcsPackedEntity packed = ((e.Sender == ecsPackedEntity) ? e.Target : e.Sender);
		if (packed.IsValid(world) && pool.Has(packed.Id))
		{
			ComponentCollider componentCollider = pool.Get(packed.Id);
			return collectionLayerCondition.Layer.HasFlag(componentCollider.Layer);
		}
		if (!(e is EventTriggerEnter eventTriggerEnter))
		{
			return false;
		}
		ColliderLayer colliderLayer = ((packed == e.Sender) ? eventTriggerEnter.SenderLayer : eventTriggerEnter.TargetLayer);
		return collectionLayerCondition.Layer.HasFlag(colliderLayer);
	}
}

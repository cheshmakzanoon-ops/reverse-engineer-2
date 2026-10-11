public interface IDynamicLoadBuilding
{
	void OnDynamicModelLoad(int skinId, SimpleTimelinePlayer buildingTimeline, SimpleAnimation buildingAnim, WorldBuildingAniEffect buildingAniEffect, WorldBuildingAniEffectAni buildingAniEffectAnim);

	void OnDynamicModelUnload();
}

using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.GGGo;

public class SystemHpRegen : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<GGGoEnv> _env;

	protected readonly EcsFilterInject<Inc<ComponentData>> _filter;

	public void Run(IEcsSystems systems)
	{
		EcsWorld value = _world.Value;
		GGGoEnv value2 = _env.Value;
		if (value == null || value2 == null)
		{
			return;
		}
		FP y = value2.LogicTickDelta;
		if (y <= FP.Zero)
		{
			return;
		}
		foreach (int item in _filter.Value)
		{
			FP x = FuncData.GetFpData(value, item, PropertyID.CurHp);
			if (x <= FP.Zero)
			{
				continue;
			}
			FP fpData = FuncData.GetFpData(value, item, PropertyID.MaxHp);
			if (x >= fpData)
			{
				continue;
			}
			FP x2 = FuncData.GetFpData(value, item, PropertyID.HpRegen);
			if (!(x2 <= FP.Zero))
			{
				FP fP = FP.Min(fpData, x + x2 * y) - x;
				if (!(fP <= FP.Zero))
				{
					FuncData.ChangeData_HP(value, item, fP);
				}
			}
		}
	}
}

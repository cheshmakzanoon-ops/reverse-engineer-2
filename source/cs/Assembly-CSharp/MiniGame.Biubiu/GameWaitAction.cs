using System.Runtime.InteropServices;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[TitleAndCategory("等待输入", "功能")]
public struct GameWaitAction : IAction
{
	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
	}
}

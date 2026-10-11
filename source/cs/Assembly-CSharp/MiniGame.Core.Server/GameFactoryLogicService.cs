using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Joker;

namespace MiniGame.Core.Server;

public class GameFactoryLogicService : GameLogicService, IService
{
	private class LogicLogger : IGameLogicLogger
	{
		public void Debug(string message)
		{
			Log.Debug(message);
		}

		public void Info(string message)
		{
			Log.Info(message);
		}

		public void Warning(string message)
		{
			Log.Warning(message);
		}

		public void Error(string message)
		{
			Log.Error(message);
		}

		public void Exception(Exception ex)
		{
			Log.Exception(ex);
		}
	}

	public Dictionary<string, IGameLogicFactory> _factories = new Dictionary<string, IGameLogicFactory>();

	public void Awake()
	{
	}

	public void Startup()
	{
	}

	public void Shutdown()
	{
	}

	public void Destroy()
	{
	}

	public override IGameLogic CreateGame(string type, string logicVersion, string resourceVersioon)
	{
		Type type2 = null;
		if (_factories.TryGetValue(logicVersion, out var value))
		{
			return value.CreateGame(type);
		}
		type2 = FindLogicFactoryByVersion(logicVersion) ?? FindLogicFactoryByLogic(type);
		if (type2 == null)
		{
			LoadLogicPlugins();
			type2 = FindLogicFactoryByVersion(logicVersion) ?? FindLogicFactoryByLogic(type);
		}
		if (type2 == null)
		{
			Log.Error("Game logic factory could not be loaded." + logicVersion);
			return null;
		}
		value = Activator.CreateInstance(type2) as IGameLogicFactory;
		value.InitLogger(new LogicLogger());
		value.InitWorkspace(null);
		_factories.Add(logicVersion, value);
		return value.CreateGame(type);
	}

	private void LoadLogicPlugins()
	{
		string[] files = Directory.GetFiles(AppContext.BaseDirectory, "MiniGame.*.Server.dll");
		for (int i = 0; i < files.Length; i++)
		{
			Assembly.LoadFrom(files[i]);
		}
	}

	private Type FindLogicFactoryByVersion(string version)
	{
		Type typeFromHandle = typeof(IGameLogicFactory);
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			Type type = assemblies[i].GetType(version);
			if (type != null && type.IsClass && typeFromHandle.IsAssignableFrom(type))
			{
				return type;
			}
		}
		return null;
	}

	private Type FindLogicFactoryByLogic(string logic)
	{
		Type typeFromHandle = typeof(IGameLogicFactory);
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			Type[] types = assemblies[i].GetTypes();
			Type type = null;
			Type[] array = types;
			foreach (Type type2 in array)
			{
				if (type2.Name.Contains(logic))
				{
					type = type2;
					break;
				}
			}
			if (type == null)
			{
				continue;
			}
			array = types;
			foreach (Type type3 in array)
			{
				if (type3.IsClass && typeFromHandle.IsAssignableFrom(type3))
				{
					return type3;
				}
			}
		}
		return null;
	}
}

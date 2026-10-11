using System;
using System.Collections.Generic;
using System.Reflection;

namespace Joker;

public class AssemblyServiceStatic : AssemblyService
{
	private readonly Dictionary<(string, string), MethodInfo> _methodDict = new Dictionary<(string, string), MethodInfo>();

	private readonly Action _clientInit;

	private readonly Action _serverInit;

	private readonly Action _sharedInit;

	public AssemblyServiceStatic()
	{
	}

	public AssemblyServiceStatic(Action sharedInit)
		: this(null, null, sharedInit, null)
	{
	}

	public AssemblyServiceStatic(Action clientInit, Action serverInit, MethodInfo[] methods)
		: this(clientInit, serverInit, null, methods)
	{
	}

	public AssemblyServiceStatic(Action clientInit, Action serverInit, Action sharedInit, MethodInfo[] methods)
	{
		if (methods != null)
		{
			foreach (MethodInfo methodInfo in methods)
			{
				Type declaringType = methodInfo.DeclaringType;
				_methodDict.Add((declaringType.Name, methodInfo.Name), methodInfo);
			}
		}
		_clientInit = clientInit;
		_serverInit = serverInit;
		_sharedInit = sharedInit;
	}

	public override void Startup()
	{
		_clientInit?.Invoke();
		_serverInit?.Invoke();
		_sharedInit?.Invoke();
	}

	public override Action GetInvokeAction(string dll, string clzName, string methodName, object self = null, params object[] args)
	{
		if (_methodDict.TryGetValue((clzName, methodName), out var method))
		{
			return delegate
			{
				method.Invoke(self, args);
			};
		}
		return null;
	}

	public override object Invoke(string dll, string clzName, string methodName, object self = null, params object[] args)
	{
		if (_methodDict.TryGetValue((clzName, methodName), out var value))
		{
			return value.Invoke(self, args);
		}
		return null;
	}

	public override void LoadAotMetadata(string dll)
	{
		throw new NotImplementedException("LoadAotMetadata not implemented in AssemblyServiceStatic");
	}

	public override void LoadHotfix(string dll)
	{
		throw new NotImplementedException("LoadHotfix not implemented in AssemblyServiceStatic");
	}
}

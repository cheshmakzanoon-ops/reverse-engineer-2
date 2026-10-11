using System;
using System.Threading;
using SQLite4Unity3d;

internal class ExecuteTask2 : DatabaseActionTask
{
	private Action<DBExecResult> callback;

	private string sql;

	private DBExecResult sql_result;

	public ExecuteTask2(SQLiteConnection dbConnection, string cmd, Action<DBExecResult> callback = null)
		: base(dbConnection)
	{
		sql = cmd;
		this.callback = callback;
	}

	public override void Process(CancellationToken token)
	{
		sql_result = ExeHelper.ExecSql(dbConnection, sql);
		base.Process(token);
	}

	protected internal override void CallBack()
	{
		callback?.Invoke(sql_result);
	}
}

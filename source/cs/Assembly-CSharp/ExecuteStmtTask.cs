using System;
using System.Collections.Generic;
using System.Threading;
using SQLite4Unity3d;

internal class ExecuteStmtTask : DatabaseActionTask
{
	private Action<DBExecResult> callback;

	private string sql;

	private DBExecResult sql_result;

	private List<List<DBAnyValue>> values;

	public ExecuteStmtTask(SQLiteConnection dbConnection, string cmd, List<List<DBAnyValue>> values, Action<DBExecResult> callback = null)
		: base(dbConnection)
	{
		sql = cmd;
		this.values = values;
		this.callback = callback;
	}

	public override void Process(CancellationToken token)
	{
		sql_result = ExeHelper.ExecStmt(dbConnection, sql, values);
		base.Process(token);
	}

	protected internal override void CallBack()
	{
		callback?.Invoke(sql_result);
	}
}

namespace Leopotam.EcsLite;

public interface IEcsPoolSnapshotRuntime
{
	void TakeSnapshot(IEcsPool pool);

	void RestoreSnapshot(IEcsPool pool);

	void RestoreEntityDelegate(IEcsPool pool, int entity);
}

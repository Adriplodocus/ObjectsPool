using System;
using Game.ObjectPool;

public class TestPrefab : PooledObject<TestPrefab, TestPrefabInfo>
{
    public override void ResetValues()
    {
        throw new NotImplementedException();
    }
}
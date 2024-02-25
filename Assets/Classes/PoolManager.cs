using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public struct PoolInfo<T> where T : GameItem
    {
        public string prefabName;
        public T prefab;
        public int capacity;
    }

    public class PoolManager<T> where T : GameItem
    {
        private Dictionary<string,ObjectPool<T>> objectPools;

        public ObjectPool<T> GetOrCreatePool(PoolInfo<T> info)
        {
            if(objectPools == null)
            {
                objectPools = new();
            }

            if(!objectPools.ContainsKey(info.prefabName))
            {
                var pool = new ObjectPool<T>(info.capacity, info.prefabName);
                objectPools.Add(info.prefabName, pool);
                pool.Prefill(info.prefab);
                return pool;
            }

            return GetPoolById(info.prefabName);

        }

        public ObjectPool<T> GetPoolById(string id)
        {
            if (!objectPools.ContainsKey(id))
            {
                Debug.LogError("Pool by Id \"" + id + "\" doesn't exist");
                return null;    
            }

            return objectPools[id];
        }

        public void DeletePool(string poolId)
        {
            if (!objectPools.ContainsKey(poolId))
            {
                Debug.LogError("objectPool with Id \"" + poolId + "\" doesn't found");
                return;
            }

            objectPools.Remove(poolId);
        }
    }
}

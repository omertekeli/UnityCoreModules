using UnityEngine;
using System.Collections.Generic;

namespace UnityCoreModules.Services.ObjectPool
{
    public class PoolManager : IPoolManager
    {
        #region Fields
        private readonly Dictionary<GameObject, ObjectPool> _pools = new Dictionary<GameObject, ObjectPool>();
        private readonly Dictionary<GameObject, ObjectPool> _activeObjects = new Dictionary<GameObject, ObjectPool>();
        private readonly Transform _poolContainer;
        #endregion

        public PoolManager()
        {
            _poolContainer = new GameObject("[POOLS]").transform;
            Object.DontDestroyOnLoad(_poolContainer.gameObject);
        }

        public void CreatePool(GameObject prefab, int initialSize)
        {
            if (prefab == null)
            {
                Debug.LogError("PoolManager: Cannot create a pool for a null prefab.");
                return;
            }
            if (_pools.ContainsKey(prefab))
            {
                Debug.LogWarning($"Pool for '{prefab.name}' already exists.");
                return;
            }

            var poolParent = new GameObject(prefab.name + "_Pool").transform;
            poolParent.SetParent(_poolContainer);

            _pools[prefab] = new ObjectPool(prefab, poolParent, initialSize);
            Debug.Log($"Created pool for '{prefab.name}' with {initialSize} objects.");
        }

        public GameObject Get(GameObject prefab)
        {
            if (_pools.TryGetValue(prefab, out var pool))
            {
                var instance = pool.Get();
                _activeObjects[instance] = pool; // Track which pool this active object belongs to.
                return instance;
            }

            Debug.LogError($"PoolManager: Pool for prefab '{prefab.name}' does not exist. Create it first in InitLoader.");
            return null;
        }

        public void Return(GameObject instance)
        {
            if (instance == null)
                return;

            // Find which pool this instance belongs to from our tracking dictionary.
            if (_activeObjects.TryGetValue(instance, out var pool))
            {
                pool.Return(instance);
                _activeObjects.Remove(instance);
            }
            else
            {
                Debug.LogWarning($"Trying to return object '{instance.name}' which was not created by the PoolManager. Destroying it instead.");
                Object.Destroy(instance);
            }
        }
    }
}
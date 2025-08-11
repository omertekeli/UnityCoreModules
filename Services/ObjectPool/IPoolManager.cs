using UnityEngine;

namespace UnityCoreModules.Services.ObjectPool
{
    public interface IPoolManager
    {
        void CreatePool(GameObject prefab, int initialSize);
        GameObject Get(GameObject prefab);
        void Return(GameObject instance);
    }
}
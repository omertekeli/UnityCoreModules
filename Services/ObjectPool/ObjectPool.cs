using UnityEngine;
using System.Collections.Generic;

namespace UnityCoreModules.Services.ObjectPool
{
    public class ObjectPool
    {
        #region Fields
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private readonly Queue<GameObject> _pool = new Queue<GameObject>();
        #endregion

        public ObjectPool(GameObject prefab, Transform parent, int initialSize)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                CreateAndPoolObject();
            }
        }

        public GameObject Get()
        {
            if (_pool.Count == 0)
            {
                Debug.LogWarning($"Pool for {_prefab.name} ran out of objects. Instantiating a new one.");
                CreateAndPoolObject();
            }

            GameObject obj = _pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        public void Return(GameObject obj)
        {
            obj.transform.SetParent(_parent);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }

        private void CreateAndPoolObject()
        {
            GameObject obj = Object.Instantiate(_prefab, _parent);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}
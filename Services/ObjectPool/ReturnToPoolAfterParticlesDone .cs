using UnityEngine;

namespace UnityCoreModules.Services.ObjectPool
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ReturnToPoolAfterParticlesDone  : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private IPoolManager _poolManager;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            _poolManager = ServiceLocator.Get<IPoolManager>();
        }

        private void OnEnable()
        {
            var main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            if (_poolManager == null)
            {
                Destroy(gameObject);
                return;
            }
            _poolManager.Return(this.gameObject);
        }
    }
}
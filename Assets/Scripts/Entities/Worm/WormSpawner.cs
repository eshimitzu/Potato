using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Potato.Entities.Worm
{
    public class WormSpawner : MonoBehaviour
    {
        [SerializeField] private WormConfig _config;
        [SerializeField] private GameObject _wormPrefab;
        [SerializeField] private Transform[] _spawnPoints;

        private readonly List<WormController> _activeWorms = new();

        public IReadOnlyList<WormController> ActiveWorms => _activeWorms;
        public bool AnyAlive => _activeWorms.Count > 0;

        public event Action<bool> OnAnyAliveChanged;

        private void Start() => StartCoroutine(SpawnLoop());

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_config.spawnInterval);
                if (_activeWorms.Count < _config.maxWorms)
                    SpawnOne();
            }
        }

        private void SpawnOne()
        {
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            GameObject go = Instantiate(_wormPrefab, spawnPoint.position, Quaternion.identity);
            WormController worm = go.GetComponent<WormController>();
            worm.Initialize(_config);
            worm.OnDied += OnWormDied;
            _activeWorms.Add(worm);
            if (_activeWorms.Count == 1)
                OnAnyAliveChanged?.Invoke(true);
        }

        private void OnWormDied(WormController worm)
        {
            _activeWorms.Remove(worm);
            if (_activeWorms.Count == 0)
                OnAnyAliveChanged?.Invoke(false);
        }
    }
}

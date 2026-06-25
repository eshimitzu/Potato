using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Potato.Entities.Worm
{
    public class WormSpawner : MonoBehaviour
    {
        [SerializeField] private WormConfig _config;
        [SerializeField] private GameObject _wormPrefab;
        [SerializeField] private Transform _potatoTarget;
        [SerializeField] private Transform[] _spawnPoints;

        private readonly List<WormController> _activeWorms = new();

        private void Start() => StartCoroutine(SpawnLoop());

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_config.spawnInterval);
                SpawnWave();
            }
        }

        private void SpawnWave()
        {
            for (int i = 0; i < _config.spawnCount; i++)
            {
                Transform spawnPoint = _spawnPoints[i % _spawnPoints.Length];
                GameObject go = Instantiate(_wormPrefab, spawnPoint.position, Quaternion.identity);
                WormController worm = go.GetComponent<WormController>();
                worm.Initialize(_config, _potatoTarget);
                worm.OnDied += OnWormDied;
                _activeWorms.Add(worm);
            }
        }

        private void OnWormDied(WormController worm) => _activeWorms.Remove(worm);

        public IReadOnlyList<WormController> ActiveWorms => _activeWorms;
    }
}

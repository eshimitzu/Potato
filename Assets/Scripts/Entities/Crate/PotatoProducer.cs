using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Potato.Entities.Potato;

namespace Potato.Entities.Crate
{
    public class PotatoProducer : MonoBehaviour
    {
        [SerializeField] private PotatoUpgrader _upgrader;
        [SerializeField] private CrateController _crate;
        [SerializeField] private PotatoController _potato;
        [SerializeField] private Transform _spawnPoint;

        private float ProduceInterval => 60f / _upgrader.CurrentData.potatoesPerMinute;

        private void Start() => StartCoroutine(ProduceLoop());

        private void Awake()
        {
            _crate.Initialize(_upgrader);
        }

        
        private IEnumerator ProduceLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(ProduceInterval);
                if (!_crate.IsFull && _potato.CurrentState != PotatoController.State.Sad)
                    SpawnPotato();
            }
        }

        private void SpawnPotato()
        {
            var cfg = _upgrader.Config;
            GameObject go = Instantiate(cfg.potatoPrefab, _spawnPoint.position, Quaternion.identity);
            go.transform
                .DOJump(_crate.transform.position, cfg.flyJumpPower, 1, cfg.flyDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    _crate.AddPotato();
                    Destroy(go);
                });
        }
    }
}

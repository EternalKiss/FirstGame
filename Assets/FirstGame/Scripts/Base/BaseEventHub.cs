using FirstGame.Spawner;
using System;
using UnityEngine;

namespace FirstGame.Base
{
    public class BaseEventHub : MonoBehaviour
    {
        [SerializeField] private BaseSpawner _baseSpawner;

        public event Action PlayerEnteredBase;
        public event Action PlayerLeftBase;

        private void OnEnable()
        {
            _baseSpawner.PlayerEnteredBase += HandlePlayerEntered;
            _baseSpawner.PlayerLeftBase += HandlePlayerLeft;
        }

        private void OnDisable()
        {
            _baseSpawner.PlayerEnteredBase -= HandlePlayerEntered;
            _baseSpawner.PlayerLeftBase -= HandlePlayerLeft;
        }

        private void HandlePlayerEntered() => PlayerEnteredBase?.Invoke();
        private void HandlePlayerLeft() => PlayerLeftBase?.Invoke();
    }
}
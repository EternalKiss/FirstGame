using FirstGame.Base;
using System;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class BaseSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerBase _basePrefab;
        [SerializeField] private float _spawnHeight = 0.1f;

        private BaseTriggerBinder _triggerBinder;

        public event Action PlayerEnteredBase;
        public event Action PlayerLeftBase;

        public Vector3 BaseEntranceOffset => _basePrefab.EntrancePosition - _basePrefab.transform.position;

        private void Awake()
        {
            _triggerBinder = new BaseTriggerBinder(HandlePlayerExited, HandlePlayerEntered);
        }

        public PlayerBase SpawnBase(Vector3 position)
        {
            position.y = _spawnHeight;
            return Instantiate(_basePrefab, position, Quaternion.identity);
        }

        public void DestroyBase(PlayerBase targetBase)
        {
            if (targetBase != null)
            {
                _triggerBinder.Unbind(targetBase);
                Destroy(targetBase.gameObject);
            }
        }

        public void BindBase(PlayerBase targetBase)
        {
            _triggerBinder.Bind(targetBase);
        }

        public void UnbindBase(PlayerBase targetBase)
        {
            _triggerBinder.Unbind(targetBase);
        }

        private void HandlePlayerEntered()
        {
            Debug.Log("[BaseSpawner] Игрок ВОШЕЛ на базу!");
            PlayerEnteredBase?.Invoke();
        }

        private void HandlePlayerExited(Vector3 playerPosition)
        {
            Debug.Log("[BaseSpawner] Игрок ВЫШЕЛ с базы!");
            PlayerLeftBase?.Invoke();
        }
    }
}

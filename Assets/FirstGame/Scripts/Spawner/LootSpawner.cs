using FirstGame.Loot;
using FirstGame.ObjectPool;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private GameObjectPool<LootPiece> _lootPool;
        [SerializeField] private int _totalPieces = 5;
        [SerializeField] private int _collectableCount = 2;
        [SerializeField] private float _minExplosionForce = 3f;
        [SerializeField] private float _maxExplosionForce = 6f;

        public void SpawnLootExplosion(Vector3 position)
        {
            Debug.Log($"[LootSpawner] МЕТОД ВЫЗВАН для {name} в точке {position}");

            if (_lootPool == null)
            {
                Debug.LogError($"[LootSpawner] КРИТИЧЕСКАЯ ОШИБКА: Ссылка на _lootPool равна NULL на объекте {name}!");
                return;
            }

            for (int i = 0; i < _totalPieces; i++)
            {
                LootPiece piece = _lootPool.Get();

                if (piece == null)
                {
                    Debug.LogError("[LootSpawner] КРИТИЧЕСКАЯ ОШИБКА: Из пула вернулся NULL вместо осколка!");
                    continue;
                }

                // ЖЕЛЕЗОБЕТОННАЯ ЗАЩИТА: Принудительно включаем объект в Unity ПЕРЕД любыми действиями
                piece.gameObject.SetActive(true);

                Vector3 spawnOffset = new Vector3(Random.Range(-0.2f, 0.2f), 0.5f, Random.Range(-0.2f, 0.2f));
                piece.transform.position = position + spawnOffset;
                piece.transform.rotation = Random.rotation;

                Vector3 randomDirection = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(0.5f, 1.5f),
                    Random.Range(-1f, 1f)
                ).normalized;

                float force = Random.Range(_minExplosionForce, _maxExplosionForce);
                bool isCollectable = i < _collectableCount;

                Debug.Log($"[LootSpawner] Запускаю осколок #{i}: {piece.name}. Активен на сцене: {piece.gameObject.activeInHierarchy}");

                piece.Launch(randomDirection, force, isCollectable);
            }
        }
    }
}
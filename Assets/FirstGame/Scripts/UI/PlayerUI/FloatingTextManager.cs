using FirstGame.ObjectPool;
using FirstGame.PlayerUI;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.UI
{
    public class FloatingTextManager : MonoBehaviour
    {
        public static FloatingTextManager Instance { get; private set; }

        [SerializeField] private FloatingTextPool _textPool;
        [SerializeField] private Color _stoneColor = Color.gray;
        [SerializeField] private Color _treeColor = Color.green;

        [SerializeField] private float _duration = 0.8f;
        [SerializeField] private float _speed = 2.5f;

        private readonly List<FloatingText> _activeTexts = new List<FloatingText>(20);

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (_textPool != null) _textPool.Initialize();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            for (int i = _activeTexts.Count - 1; i >= 0; i--)
            {
                FloatingText txt = _activeTexts[i];

                if (txt == null || !txt.gameObject.activeInHierarchy)
                {
                    _activeTexts.RemoveAt(i);
                    continue;
                }

                txt.TickAge(deltaTime);
                float progress = txt.Age / _duration;

                if (progress >= 1f)
                {
                    _activeTexts.RemoveAt(i);
                    txt.ReleaseToPool();

                    continue;
                }

                txt.CachedTransform.position = txt.StartPosition + (Vector3.up * (_speed * progress));

                if (txt.CachedTextMesh != null)
                {
                    Color c = txt.CachedTextMesh.color;
                    c.a = 1f - progress;
                    txt.CachedTextMesh.color = c;
                }
            }
        }

        public void SpawnText(string value, Vector3 worldPosition, Loot.LootType type)
        {
            if (_textPool == null) return;

            FloatingText textInstance = _textPool.Get();
            Color targetColor = type == Loot.LootType.Stone ? _stoneColor : _treeColor;

            Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(1.5f, 1.8f), Random.Range(-0.3f, 0.3f));
            textInstance.SetupText(value, worldPosition + randomOffset, targetColor);

            _activeTexts.Add(textInstance);
        }
    }
}
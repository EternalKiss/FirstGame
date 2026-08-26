using System;
using TMPro;
using UnityEngine;
using FirstGame.Interfaces;

namespace FirstGame.PlayerUI
{
    public class FloatingText : MonoBehaviour, IDestructible
    {
        private TextMeshPro _textMesh;
        private Transform _transform;
        private Vector3 _startPosition;
        private float _age;

        public Transform CachedTransform => _transform;
        public TextMeshPro CachedTextMesh => _textMesh;
        public Vector3 StartPosition => _startPosition;
        public float Age => _age;

        public event Action<IDestructible> OnReadyToRelease;

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshPro>();
            _transform = transform;
        }

        public void SetupText(string content, Vector3 spawnPos, Color textColor)
        {
            _transform.position = spawnPos;
            _startPosition = spawnPos;
            _age = 0f;

            if (_textMesh != null)
            {
                _textMesh.text = content;
                _textMesh.color = textColor;
            }
        }

        public void TickAge(float deltaTime)
        {
            _age += deltaTime;
        }

        public void ReleaseToPool()
        {
            OnReadyToRelease?.Invoke(this);
        }
    }
}

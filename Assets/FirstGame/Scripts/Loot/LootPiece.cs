using System;
using System.Collections;
using UnityEngine;

namespace FirstGame.Loot
{
    public class LootPiece : MonoBehaviour, IDestructible
    {
        private static readonly int BaseColorPropertyId = Shader.PropertyToID("_BaseColor");

        [SerializeField] private float _lifeTime = 4f;
        [SerializeField] private float _fadeDuration = 0.5f;

        private Rigidbody _rigidbody;
        private Collider _collider;
        private Renderer _renderer;
        private Vector3 _originalScale;
        private Color _originalColor;
        private bool _isCollectable;
        private bool _isMagnetized;
        private bool _hasBaseColorProperty;

        public bool IsCollectable => _isCollectable;
        public bool IsMagnetized => _isMagnetized;
        public event Action<IDestructible> OnReadyToRelease;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _renderer = GetComponentInChildren<Renderer>();
            _originalScale = transform.localScale;

            if (_renderer != null)
            {
                _hasBaseColorProperty = _renderer.material.HasProperty(BaseColorPropertyId);

                if (_hasBaseColorProperty)
                {
                    _originalColor = _renderer.material.GetColor(BaseColorPropertyId);
                }
                else
                {
                    _originalColor = _renderer.material.color;
                }
            }
        }

        public void Launch(Vector3 forceDirection, float forceMagnitude, bool isCollectable)
        {
            _isCollectable = isCollectable;
            _isMagnetized = false;
            transform.localScale = _originalScale;

            if (_renderer != null)
            {
                if (_renderer.material.HasProperty(BaseColorPropertyId))
                    _renderer.material.SetColor(BaseColorPropertyId, _originalColor);
                else
                    _renderer.material.color = _originalColor;
            }

            if (_collider != null)
            {
                _collider.enabled = true;
                _collider.isTrigger = false;
            }

            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = false;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            }

            StartCoroutine(LifeRoutine());
        }

        public void Magnetize()
        {
            if (_isMagnetized) return;
            _isMagnetized = true;

            if (_rigidbody != null) _rigidbody.isKinematic = true;
            if (_collider != null) _collider.isTrigger = true;
        }

        public void Collect()
        {
            StopAllCoroutines();
            if (_rigidbody != null) _rigidbody.isKinematic = true;
            if (_collider != null) _collider.enabled = false;

            OnReadyToRelease?.Invoke(this);
        }

        private IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(_lifeTime - _fadeDuration);

            if (_isMagnetized) yield break;

            float elapsed = 0f;
            Vector3 startScale = transform.localScale;

            while (elapsed < _fadeDuration)
            {
                if (_isMagnetized) yield break;

                elapsed += Time.deltaTime;
                float progress = elapsed / _fadeDuration;

                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);

                if (_renderer != null)
                {
                    Color fadeColor = _originalColor;
                    fadeColor.a = Mathf.Lerp(1f, 0f, progress);

                    if (_renderer.material.HasProperty(BaseColorPropertyId))
                        _renderer.material.SetColor(BaseColorPropertyId, fadeColor);
                    else
                        _renderer.material.color = fadeColor;
                }

                yield return null;
            }

            Collect();
        }
    }
}

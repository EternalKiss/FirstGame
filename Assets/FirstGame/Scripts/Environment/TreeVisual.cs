using UnityEngine;
using System.Collections;

namespace FirstGame.Environment
{
    public class TreeVisual : ResourceVisual
    {
        [Header("Hit Animation Settings")]
        [SerializeField] private float _hitDuration = 0.4f;
        [SerializeField] private float _maxTiltAngle = 12f;
        [SerializeField] private float _wobbleSpeed = 4f;

        [Header("Death Animation Settings")]
        [SerializeField] private float _deathDuration = 0.8f;
        [SerializeField] private float _fallAngle = 80f;

        private Quaternion _originalRotation;
        private Vector3 _originalScale;
        private Vector3 _originalPosition;

        private Coroutine _hitCoroutine;
        private bool _isDying = false;
        private Resource _resourceRoot;

        private void Awake()
        {
            _originalRotation = transform.localRotation;
            _originalScale = transform.localScale;
            _originalPosition = transform.localPosition;
            _resourceRoot = GetComponentInParent<Resource>();
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            _hitCoroutine = null;
            _isDying = false;
            ResetVisuals();
        }

        public override void PlayHitVisual()
        {
            if (_isDying || !gameObject.activeInHierarchy) return;

            if (_hitCoroutine != null)
            {
                StopCoroutine(_hitCoroutine);
            }

            _hitCoroutine = StartCoroutine(HitRoutine());
        }

        private IEnumerator HitRoutine()
        {
            float elapsed = 0f;
            Vector3 randomTiltAxis = Random.Range(0, 2) == 0 ? Vector3.forward : Vector3.right;

            while (elapsed < _hitDuration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / _hitDuration;

                float damping = 1f - progress;
                float angle = Mathf.Sin(progress * _wobbleSpeed * Mathf.PI * 2f) * _maxTiltAngle * damping;

                transform.localRotation = _originalRotation * Quaternion.AngleAxis(angle, randomTiltAxis);

                yield return null;
            }

            transform.localRotation = _originalRotation;
            _hitCoroutine = null;
        }

        public override void PlayDeathVisual()
        {
            if (_isDying || !gameObject.activeInHierarchy) return;
            _isDying = true;

            if (_hitCoroutine != null)
            {
                StopCoroutine(_hitCoroutine);
                _hitCoroutine = null;
            }

            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            float elapsed = 0f;

            float randomFallDirectionY = Random.Range(0f, 360f);
            Quaternion targetLookRotation = Quaternion.Euler(0f, randomFallDirectionY, 0f);
            Quaternion finalFallRotation = targetLookRotation * _originalRotation * Quaternion.Euler(_fallAngle, 0f, 0f);

            Vector3 targetPosition = _originalPosition - new Vector3(0f, 0.5f, 0f);

            while (elapsed < _deathDuration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / _deathDuration;

                float t = progress * progress;

                transform.localRotation = Quaternion.Slerp(_originalRotation, finalFallRotation, t);
                transform.localScale = Vector3.Lerp(_originalScale, Vector3.zero, progress);
                transform.localPosition = Vector3.Lerp(_originalPosition, targetPosition, progress);

                yield return null;
            }

            if (_resourceRoot != null)
            {
                _resourceRoot.CompleteDestruction();
            }
        }

        private void ResetVisuals()
        {
            transform.localRotation = _originalRotation;
            transform.localScale = _originalScale;
            transform.localPosition = _originalPosition;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _hitCoroutine = null;
            _isDying = false;
        }
    }
}
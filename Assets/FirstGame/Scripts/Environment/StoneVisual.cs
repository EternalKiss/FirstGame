using UnityEngine;
using System.Collections;

namespace FirstGame.Environment
{
    public class StoneVisual : ResourceVisual
    {
        [SerializeField] private float _hitDuration = 0.4f;
        [SerializeField] private float _maxTiltAngle = 12f;
        [SerializeField] private float _wobbleSpeed = 3f;

        [SerializeField] private float _deathDuration = 0.1f;
        [SerializeField] private float _fallAngle = 80f;
        [SerializeField] private AnimationCurve _scaleCurve;

        private readonly Quaternion _originalRotation = Quaternion.Euler(0f, 0f, 0f);
        private Vector3 _originalScale;

        private Coroutine _hitCoroutine;
        private bool _isDying = false;
        private Resource _resourceRoot;

        private void Reset()
        {
            _scaleCurve = new AnimationCurve(
                new Keyframe(0f, 1f),
                new Keyframe(0.2f, 1.15f),
                new Keyframe(1f, 0f)
            );
        }

        private void Awake()
        {
            _originalScale = transform.localScale;
            _resourceRoot = GetComponentInParent<Resource>();

            if (_scaleCurve == null || _scaleCurve.length == 0)
            {
                _scaleCurve = new AnimationCurve(
                    new Keyframe(0f, 1f),
                    new Keyframe(0.2f, 1.15f),
                    new Keyframe(1f, 0f)
                );
            }
        }

        private void OnEnable()
        {
            StopAllCoroutines();

            _hitCoroutine = null;
            _isDying = false;

            ResetVisuals();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _hitCoroutine = null;
            _isDying = false;
        }

        public override void PlayHitVisual()
        {
            if (!gameObject.activeInHierarchy || _isDying) return;

            if (_hitCoroutine != null)
            {
                StopCoroutine(_hitCoroutine);
            }

            _hitCoroutine = StartCoroutine(HitRoutine());
        }

        public override void PlayDeathVisual()
        {
            if (!gameObject.activeInHierarchy || _isDying) return;
            _isDying = true;

            if (_hitCoroutine != null)
            {
                StopCoroutine(_hitCoroutine);
                _hitCoroutine = null;
            }

            StartCoroutine(DeathRoutine());
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

        private IEnumerator DeathRoutine()
        {
            float elapsed = 0f;

            float randomFallDirectionY = Random.Range(0f, 360f);
            Quaternion targetLookRotation = Quaternion.Euler(0f, randomFallDirectionY, 0f);
            Quaternion finalFallRotation = targetLookRotation * Quaternion.Euler(_fallAngle - 90f, 0f, 0f);

            Vector3 targetPosition = new Vector3(0f, -0.5f, 0f);

            while (elapsed < _deathDuration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / _deathDuration;

                float t = progress * progress;
                float scaleMultiplier = _scaleCurve.Evaluate(progress);

                transform.localRotation = Quaternion.Slerp(_originalRotation, finalFallRotation, t);
                transform.localScale = _originalScale * scaleMultiplier;
                transform.localPosition = Vector3.Lerp(Vector3.zero, targetPosition, progress);

                yield return null;
            }

            transform.localScale = Vector3.zero;

            if (_resourceRoot != null)
            {
                _resourceRoot.CompleteDestruction();
            }
        }

        private void ResetVisuals()
        {
            transform.localRotation = _originalRotation;
            transform.localScale = _originalScale;
            transform.localPosition = Vector3.zero;
        }
    }
}
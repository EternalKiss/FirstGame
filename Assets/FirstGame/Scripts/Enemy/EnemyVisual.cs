using UnityEngine;
using System.Collections;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private float _hitDuration = 0.15f;
    [SerializeField] private float _tiltAngle = -15f;
    [SerializeField] private Color _flashColor = Color.red;

    private Renderer _renderer;
    private Color _originalColor;

    private Coroutine _hitCoroutine;
    private bool _isDead = false;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();

        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }
    }

    private void OnEnable()
    {
        _isDead = false;
    }

    public void PlayHitVisual()
    {
        if (_isDead) return;

        if (_hitCoroutine != null)
        {
            StopCoroutine(_hitCoroutine);
        }

        _hitCoroutine = StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        float elapsed = 0f;

        while (elapsed < _hitDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / _hitDuration;
            float wave = Mathf.Sin(progress * Mathf.PI);

            float currentTilt = Mathf.Lerp(0f, _tiltAngle, wave);

            Vector3 currentEuler = transform.localEulerAngles;
            transform.localRotation = Quaternion.Euler(currentTilt, currentEuler.y, currentEuler.z);

            if (_renderer != null)
            {
                _renderer.material.color = Color.Lerp(_originalColor, _flashColor, wave);
            }

            yield return null;
        }

        if (_renderer != null)
        {
            _renderer.material.color = _originalColor;
        }

        _hitCoroutine = null;
    }

    public void PlayDeathVisual()
    {
        _isDead = true;
        if (_hitCoroutine != null)
        {
            StopCoroutine(_hitCoroutine);
        }

        if (_renderer != null)
        {
            _renderer.material.color = _originalColor;
        }
    }

    private void OnDisable()
    {
        if (_hitCoroutine != null)
        {
            StopCoroutine(_hitCoroutine);
            _hitCoroutine = null;
        }

        if (_renderer != null)
        {
            _renderer.material.color = _originalColor;
        }
    }
}

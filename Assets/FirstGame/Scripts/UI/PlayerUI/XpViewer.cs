using FirstGame.Common;
using FirstGame.Players.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstGame.PlayerUI
{
    public class XpViewer : MonoBehaviour
    {
        [SerializeField] private Experience _experience;
        [SerializeField] private Renderer _ringRenderer;

        [SerializeField] private float _smoothSpeed = 5f;

        private Material _ringMaterial;
        private float _targetFillAmount;
        private float _currentFillAmount;
        private readonly UniversalEventBinder _eventBinder = new UniversalEventBinder();

        private void Awake()
        {
            if (_ringRenderer != null)
            {
                _ringMaterial = _ringRenderer.material;
            }

            if (_experience == null)
            {
                _experience = GetComponentInParent<Experience>();
            }
        }

        private void OnEnable()
        {
            if (_experience == null)
            {
                return;
            }

            _eventBinder.Bind<float, float>(
                callback => _experience.OnExperienceChanged += callback,
                callback => _experience.OnExperienceChanged -= callback,
                HandleExperienceChanged);

            HandleExperienceChanged(_experience.CurrentXp, _experience.RequiredXpForNextLevel);
        }

        private void OnDisable()
        {
            _eventBinder.UnbindAll();
        }

        private void Update()
        {
            if (_ringMaterial == null)
            {
                return;
            }

            _currentFillAmount = Mathf.Lerp(_currentFillAmount, _targetFillAmount, _smoothSpeed * Time.deltaTime);
            _ringMaterial.SetFloat("_FillAmount", _currentFillAmount);
        }

        private void HandleExperienceChanged(float currentXp, float maxXp)
        {
            if (maxXp <= 0f)
            {
                _targetFillAmount = 0f;
                return;
            }

            _targetFillAmount = currentXp / maxXp;
        }
    }
}

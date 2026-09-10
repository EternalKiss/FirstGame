using FirstGame.Common;
using FirstGame.Players.Level;
using TMPro;
using UnityEngine;

namespace FirstGame.PlayerUI
{
    public class LevelTextViewer : MonoBehaviour
    {
        [SerializeField] private Experience _experience;
        [SerializeField] private TMP_Text _levelText;

        private readonly UniversalEventBinder _eventBinder = new UniversalEventBinder();

        private void Awake()
        {
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

            _eventBinder.Bind<int>(
                callback => _experience.OnLevelUp += callback,
                callback => _experience.OnLevelUp -= callback,
                HandleLevelUp);

            HandleLevelUp(_experience.CurrentLevel);
        }

        private void OnDisable()
        {
            _eventBinder.UnbindAll();
        }

        private void HandleLevelUp(int level)
        {
            if (_levelText != null)
            {
                _levelText.text = $"LVL {level}";
            }
        }
    }
}

using FirstGame.Players.Abilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstGame.PlayerUI
{
    public class AbilityCardView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Button _selectButton;

        private AbilityBase _ability;
        private Action<AbilityBase> _onSelected;

        private void Awake()
        {
            if (_selectButton != null)
            {
                _selectButton.onClick.AddListener(HandleClick);
            }
        }

        private void OnDestroy()
        {
            if (_selectButton != null)
            {
                _selectButton.onClick.RemoveListener(HandleClick);
            }
        }

        public void Setup(AbilityBase ability, string displayName, string description, Sprite icon, Action<AbilityBase> onSelected)
        {
            _ability = ability;
            _onSelected = onSelected;

            if (_icon != null && icon != null)
            {
                _icon.sprite = icon;
            }

            if (_nameText != null)
            {
                _nameText.text = displayName;
            }

            if (_descriptionText != null)
            {
                _descriptionText.text = description;
            }
        }

        private void HandleClick()
        {
            _onSelected?.Invoke(_ability);
        }
    }
}

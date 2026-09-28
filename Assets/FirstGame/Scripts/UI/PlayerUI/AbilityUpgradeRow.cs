using FirstGame.Players.Abilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstGame.PlayerUI
{
    public class AbilityUpgradeRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private Button _upgradeButton;

        private AbilityBase _ability;
        private int _cost;
        private Action<AbilityBase> _onUpgrade;

        private void Awake()
        {
            if(_upgradeButton != null)
            {
                _upgradeButton.onClick.AddListener(HandleClick);
            }
        }

        private void OnDestroy()
        {
            if( _upgradeButton != null )
            {
                _upgradeButton.onClick.RemoveAllListeners();
            }
        }

        public void Setup(AbilityBase ability, int cost, Action<AbilityBase> onUpgrade)
        {
            _ability = ability;
            _cost = cost;
            _onUpgrade = onUpgrade;

            if (_nameText != null)
            {
                _nameText.text = ability.name;
            }

            if (_levelText != null)
            {
                _levelText.text = "LVL " + ability.CurrentLevel;
            }

            if (_costText != null)
            {
                if (ability.IsMaxLevel)
                {
                    _costText.text = "MAX";
                }
                else
                {
                    _costText.text = cost + " G";
                }
            }

            if (_upgradeButton != null)
            {
                _upgradeButton.interactable = ability.IsMaxLevel == false;
            }
        }

        private void HandleClick()
        {
            _onUpgrade?.Invoke(_ability);
        }
    }
}

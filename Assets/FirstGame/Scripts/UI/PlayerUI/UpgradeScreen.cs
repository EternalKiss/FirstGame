using FirstGame.Players.Abilities;
using FirstGame.Players.Inventore;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

namespace FirstGame.PlayerUI
{
    public class UpgradeScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _rootPanel;
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private Transform _upgradeContainer;
        [SerializeField] private AbilityUpgradeRow _upgradeRowPrefab;
        [SerializeField] private int _upgradeBaseCost = 5;

        private Inventory _inventory;
        private AbilitySlotController _slotController;

        private readonly List<AbilityUpgradeRow> _spawnedRows = new List<AbilityUpgradeRow>(4);

        private void Awake()
        {
            if (_rootPanel != null) _rootPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_inventory != null)
            {
                _inventory.GoldAdded -= OnInventoryChanged;
            }
        }

        public void Initialize(Inventory invetory, AbilitySlotController abilitySlotController)
        {
            _inventory = invetory;
            _slotController = abilitySlotController;

            if(_inventory != null)
            {
                _inventory.GoldAdded += OnInventoryChanged;
            }
        }

        public void Open()
        {
            if (_rootPanel != null) _rootPanel.SetActive(true);

            RefreshUI();
            RefreshUpgradeRows();
        }

        public void Close()
        {
            if (_rootPanel != null) _rootPanel.SetActive(false);
        }

        private void HandleUpgrade(AbilityBase ability)
        {
            if (_inventory == null || _slotController == null) return;
            if (ability == null || ability.IsMaxLevel) return;

            int cost = GetUpgradeCost(ability);

            if(_inventory.TrySpendGold(cost) == false) return;

            ability.TryUpgrade();

            RefreshUI();
            RefreshUpgradeRows();
        }

        private int GetUpgradeCost(AbilityBase ability)
        {
            return _upgradeBaseCost * ability.CurrentLevel;
        }

        private void RefreshUpgradeRows()
        {
            ClearUpgradeRows();

            if (_slotController == null || _upgradeRowPrefab == null || _upgradeContainer == null)
            {
                return;
            }

            var unlocked = _slotController.UnlockedAbilities;

            for (int index = 0; index < unlocked.Count; index++)
            {
                AbilityBase ability = unlocked[index];
                AbilityUpgradeRow row = Instantiate(_upgradeRowPrefab, _upgradeContainer);

                row.Setup(ability, GetUpgradeCost(ability), HandleUpgrade);
                _spawnedRows.Add(row);
            }
        }

        private void ClearUpgradeRows()
        {
            for (int index = 0; index < _spawnedRows.Count; index++)
            {
                if (_spawnedRows[index] != null)
                {
                    Destroy(_spawnedRows[index].gameObject);
                }
            }

            _spawnedRows.Clear();
        }

        private void OnInventoryChanged(int count)
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            if (_inventory == null) return;
            if (_goldText != null) _goldText.text = _inventory.GoldCount.ToString();
        }
    }
}

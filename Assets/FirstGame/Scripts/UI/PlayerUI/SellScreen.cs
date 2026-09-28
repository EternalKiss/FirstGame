using FirstGame.Players.Inventore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstGame.PlayerUI
{
    public class SellScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _rootPanel;
        [SerializeField] private TMP_Text _stoneText;
        [SerializeField] private TMP_Text _treeText;
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private Button _sellButton;
        [SerializeField] private int _stoneGoldValue = 1;
        [SerializeField] private int _treeGoldValue = 2;

        private Inventory _inventory;

        private void Awake()
        {
            if (_rootPanel != null) _rootPanel.SetActive(false);
            if (_sellButton != null) _sellButton.onClick.AddListener(HandleSell);
        }

        private void OnDestroy()
        {
            if (_sellButton != null) _sellButton.onClick.RemoveAllListeners();

            if (_inventory != null)
            {
                _inventory.StoneAdded -= OnInventoryChanged;
                _inventory.TreeAdded -= OnInventoryChanged;
                _inventory.GoldAdded -= OnInventoryChanged;
            }
        }

        public void Initialize(Inventory inventory)
        {
            Debug.Log("[SellScreen] Initialize, inventory = " + (inventory != null));
            _inventory = inventory;

            if (_inventory != null)
            {
                _inventory.StoneAdded += OnInventoryChanged;
                _inventory.TreeAdded += OnInventoryChanged;
                _inventory.GoldAdded += OnInventoryChanged;
            }
        }

        public void Open()
        {
            if (_rootPanel != null) _rootPanel.SetActive(true);
            RefreshUI();
        }

        public void Close()
        {
            if(_rootPanel != null) _rootPanel.SetActive(false); 
        }

        private void HandleSell()
        {
            Debug.Log("[SellScreen] HandleSell, inventory = " + (_inventory != null));

            if (_inventory == null) return;

            int stoneCount = _inventory.RemoveAllStone();
            int treeCount = _inventory.RemoveAllTree();
            Debug.Log("[SellScreen] stone = " + stoneCount + ", tree = " + treeCount);

            int goldEarned = stoneCount * _stoneGoldValue + treeCount * _treeGoldValue;
            Debug.Log("[SellScreen] goldEarned = " + goldEarned);

            if (goldEarned > 0)
            {
                _inventory.AddGold(goldEarned);
            }

            RefreshUI();
        }

        private void OnInventoryChanged(int count)
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            if (_inventory == null) return;

            if (_stoneText != null) _stoneText.text = _inventory.StoneCount.ToString();
            if (_treeText != null) _treeText.text = _inventory.TreeCount.ToString();
            if (_goldText != null) _goldText.text = _inventory.GoldCount.ToString();
        }
    }
}
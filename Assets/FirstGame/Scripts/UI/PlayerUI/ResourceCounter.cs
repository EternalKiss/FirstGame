using FirstGame.Players.Inventore;
using TMPro;
using UnityEngine;

namespace FirstGame.PlayerUI
{
    public class ResourceCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _stoneCount;
        [SerializeField] private TextMeshProUGUI _treeCount;
        [SerializeField] private TextMeshProUGUI _goldCount;

        private Inventory _inventory;

        public void Initialize(Inventory playerInventory)
        {
            _inventory = playerInventory;

            if (_inventory != null)
            {
                _inventory.StoneAdded += UpdateStoneCount;
                _inventory.TreeAdded += UpdateTreeCount;
                _inventory.GoldAdded += UpdateGoldCount;

                UpdateStoneCount(_inventory.StoneCount);
                UpdateTreeCount(_inventory.TreeCount);
            }
        }

        private void OnDestroy()
        {
            if (_inventory != null)
            {
                _inventory.StoneAdded -= UpdateStoneCount;
                _inventory.TreeAdded -= UpdateTreeCount;
                _inventory.GoldAdded -= UpdateGoldCount;
            }
        }

        private void UpdateTreeCount(int count)
        {
            if (_treeCount != null)
            {
                _treeCount.text = count.ToString();
            }
        }

        private void UpdateStoneCount(int count)
        {
            if (_stoneCount != null)
            {
                _stoneCount.text = count.ToString();
            }
        }

        private void UpdateGoldCount(int count)
        {
            if( _goldCount != null)
            {
                _goldCount.text = count.ToString();
            }    
        }
    }
}
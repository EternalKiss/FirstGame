using System;
using UnityEngine;

namespace FirstGame.Players.Inventore
{
    public class Inventory : MonoBehaviour
    {
        private int _stoneCount;
        private int _treeCount;
        private int _goldCount;

        public int StoneCount => _stoneCount;
        public int TreeCount => _treeCount;
        public int GoldCount => _goldCount;

        public event Action<int> StoneAdded;
        public event Action<int> TreeAdded;
        public event Action<int> GoldAdded;

        public void AddStone(int amount)
        {
            if (amount <= 0) return;

            _stoneCount += amount;
            StoneAdded?.Invoke(_stoneCount);
        }

        public void AddTree(int amount)
        {
            if (amount <= 0) return;

            _treeCount += amount;
            TreeAdded?.Invoke(_treeCount);
        }
        
        public void AddGold(int amount)
        {
            if(amount <= 0) return;

            _goldCount += amount;
            GoldAdded?.Invoke(_goldCount);
        }    

        public bool TrySpendGold(int amount)
        {
            if (amount < 0) return false;
            if(amount > _goldCount) return false;

            _goldCount = amount;
            GoldAdded?.Invoke(_goldCount);

            return true;
        }

        public int RemoveAllStone()
        {
            int count = _stoneCount;
            _stoneCount = 0;
            StoneAdded?.Invoke(_stoneCount);
            return count;
        }

        public int RemoveAllTree()
        {
            int count = _treeCount;
            _treeCount = 0;
            TreeAdded?.Invoke(_treeCount);
            return count;
        }

        public void Clear()
        {
            _stoneCount = 0;
            _treeCount = 0;
            _goldCount = 0;

            StoneAdded?.Invoke(_stoneCount);
            TreeAdded?.Invoke(_treeCount);
            GoldAdded?.Invoke(_goldCount);
        }
    }
}

using System;
using UnityEngine;

namespace FirstGame.Players.Inventore
{
    public class Inventory : MonoBehaviour
    {
        private int _stoneCount;
        private int _treeCount;

        public int StoneCount => _stoneCount;
        public int TreeCount => _treeCount;

        public event Action<int> StoneAdded;
        public event Action<int> TreeAdded;

        public void AddStone(int amount)
        {
            if (amount <= 0) return;

            _stoneCount += amount;
            StoneAdded?.Invoke(_stoneCount);

            Debug.Log($"[Инвентарь] Камень добавлен! Всего: {_stoneCount}");
        }

        public void AddTree(int amount)
        {
            if (amount <= 0) return;

            _treeCount += amount;
            TreeAdded?.Invoke(_treeCount);

            Debug.Log($"[Инвентарь] Дерево добавлено! Всего: {_treeCount}");
        }

        public void Clear()
        {
            _stoneCount = 0;
            _treeCount = 0;
            StoneAdded?.Invoke(_stoneCount);
            TreeAdded?.Invoke(_treeCount);
        }
    }
}

using UnityEngine;

namespace FirstGame.GamePlay.Systems
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private int _currentLevel = 1;

        public int CurrentLevel => _currentLevel;

        public void SetLevel(int level)
        {
            if (level > 0)
            {
                _currentLevel = level;
            }
        }
    }
}
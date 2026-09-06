using UnityEngine;

namespace FirstGame.GamePlay.Systems
{
    public class XPRewardCalculator : MonoBehaviour
    {
        [SerializeField] private float _baseXpReward = 20f;
        [SerializeField] private float _xpMultiplierPerLevel = 0.15f;

        private Level _level;

        private void Awake()
        {
            _level = GetComponent<Level>();
        }

        public float CalculateReward()
        {
            if (_level == null)
            {
                return _baseXpReward;
            }

            return _baseXpReward + (_baseXpReward * _xpMultiplierPerLevel * (_level.CurrentLevel - 1));
        }
    }
}

using System;
using UnityEngine;

namespace FirstGame.GamePlay.Systems
{
    public class GameEventBroker : MonoBehaviour
    {
        private static GameEventBroker _instance;

        public static GameEventBroker Instance => _instance;

        public event Action<float> OnXpEarned;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        public void FireXpEarned(float amount)
        {
            OnXpEarned?.Invoke(amount);
        }
    }
}
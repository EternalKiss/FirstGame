using FirstGame.Gameplay.Events;
using FirstGame.GamePlay.Systems;
using System;
using UnityEngine;

namespace FirstGame.Environment
{
    public class XPReward : MonoBehaviour
    {
        [SerializeField] private FloatGameEvent _xpEarnedEvent;

        private Resource _resource;
        private XPRewardCalculator _calculator;

        private void Awake()
        {
            _resource = GetComponent<Resource>();
            _calculator = GetComponent<XPRewardCalculator>();
        }

        private void OnEnable()
        {
            if (_resource != null)
            {
                _resource.OnReadyToRelease += HandleResourceReleased;
            }
        }

        private void OnDisable()
        {
            if (_resource != null)
            {
                _resource.OnReadyToRelease -= HandleResourceReleased;
            }
        }

        private void HandleResourceReleased(IDestructible destructible)
        {
            if (_calculator != null && _xpEarnedEvent != null)
            {
                float xpAmount = _calculator.CalculateReward();
                _xpEarnedEvent.Raise(xpAmount);
            }
        }
    }
}

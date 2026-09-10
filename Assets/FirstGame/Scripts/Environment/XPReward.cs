using FirstGame.Gameplay.Events;
using System;
using UnityEngine;

namespace FirstGame.Environment
{
    public class XPReward : MonoBehaviour
    {
        [SerializeField] private FloatGameEvent _xpEarnedEvent;
        [SerializeField] private float _xpAmount = 10f;

        private Resource _resource;

        private void Awake()
        {
            _resource = GetComponent<Resource>();
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
            if (_xpEarnedEvent != null)
            {
                _xpEarnedEvent.Raise(_xpAmount);
            }
        }
    }
}

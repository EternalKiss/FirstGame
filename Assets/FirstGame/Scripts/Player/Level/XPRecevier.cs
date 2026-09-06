using FirstGame.Gameplay.Events;
using UnityEngine;

namespace FirstGame.Players.Level
{
    public class XPRecevier : MonoBehaviour
    {
        [SerializeField] private FloatGameEvent _xpEarnedEvent;

        private Experience _experience;

        private void Awake()
        {
            _experience = GetComponent<Experience>();
        }

        private void OnEnable()
        {
            if (_xpEarnedEvent != null)
            {
                _xpEarnedEvent.RegisterListener(HandleXpEarned);
            }
        }

        private void OnDisable()
        {
            if (_xpEarnedEvent != null)
            {
                _xpEarnedEvent.UnregisterListener(HandleXpEarned);
            }
        }

        private void HandleXpEarned(float amount)
        {
            if (_experience != null)
            {
                _experience.AddExperience(amount);
            }
        }
    }
}
using System;
using UnityEngine;

namespace FirstGame.Gameplay.Events
{
    [CreateAssetMenu(fileName = "NewFloatEvent", menuName = "Events/Float Event")]
    public class FloatGameEvent : ScriptableObject
    {
        private event Action<float> _onEventRaised;

        public void RegisterListener(Action<float> listener)
        {
            _onEventRaised += listener;
        }

        public void UnregisterListener(Action<float> listener)
        {
            _onEventRaised -= listener;
        }

        public void Raise(float value)
        {
            _onEventRaised?.Invoke(value);
        }
    }
}

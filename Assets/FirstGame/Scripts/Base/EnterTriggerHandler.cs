using FirstGame.Players;
using System;
using UnityEngine;

namespace FirstGame.Base
{
    public class EnterTriggerHandler : MonoBehaviour
    {
        public event Action EnteredInBase;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out Player player))
            {
                EnteredInBase?.Invoke();
            }
        }
    }
}

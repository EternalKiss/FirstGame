using FirstGame.Players;
using System;
using UnityEngine;

namespace FirstGame.Base
{
    public class ExitTriggerHandler : MonoBehaviour
    {
        public event Action<Vector3> OnPlayerExited;

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                OnPlayerExited?.Invoke(player.transform.position);
            }
        }
    }
}

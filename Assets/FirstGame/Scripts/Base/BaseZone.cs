using FirstGame.Players;
using FirstGame.PlayerUI;
using System;
using UnityEngine;
namespace FirstGame.Base
{
    public class BaseZone : MonoBehaviour
    {
        [SerializeField] private BaseZoneType _zoneType;

        private bool _isPlayerInside;

        public BaseZoneType ZoneType => _zoneType;

        private void OnTriggerEnter(Collider other)
        {
            TryHandleEnter(other);
        }

        private void OnTriggerStay(Collider other)
        {
            TryHandleEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Player>(out _) == false)
            {
                return;
            }

            if (_isPlayerInside == false)
            {
                return;
            }

            _isPlayerInside = false;

            if (BaseZoneUIOpener.Instance != null)
            {
                BaseZoneUIOpener.Instance.HandleZoneLeft(_zoneType);
            }
        }

        private void TryHandleEnter(Collider other)
        {
            if (_isPlayerInside) return;

            if (other.TryGetComponent<Player>(out _) == false) return;

            _isPlayerInside = true;

            Debug.Log("[BaseZone] Enter, zone = " + _zoneType + ", opener = " + (BaseZoneUIOpener.Instance != null));

            if (BaseZoneUIOpener.Instance != null)
            {
                BaseZoneUIOpener.Instance.HandleZoneEntered(_zoneType);
            }
        }
    }
}

using FirstGame.Players;
using FirstGame.PlayerUI;
using System;
using UnityEngine;
namespace FirstGame.Base
{
    public class BaseZone : MonoBehaviour
    {
        [SerializeField] private BaseZoneType _zoneType;

        public BaseZoneType ZoneType => _zoneType;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out _) == false)
            {
                return;
            }

            if (BaseZoneUIOpener.Instance != null)
            {
                BaseZoneUIOpener.Instance.HandleZoneEntered(_zoneType);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Player>(out _) == false)
            {
                return;
            }

            if (BaseZoneUIOpener.Instance != null)
            {
                BaseZoneUIOpener.Instance.HandleZoneLeft(_zoneType);
            }
        }
    }
}

using FirstGame.Base;
using UnityEngine;

namespace FirstGame.PlayerUI
{
    public class BaseZoneUIOpener : MonoBehaviour
    {
        public static BaseZoneUIOpener Instance { get; private set; }

        [SerializeField] private SellScreen _sellScreen;
        [SerializeField] private UpgradeScreen _upgradeScreen;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }    
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void HandleZoneEntered(BaseZoneType zoneType)
        {
            if (zoneType == BaseZoneType.Sell)
            {
                if (_sellScreen != null) _sellScreen.Open();
            }
            else if (zoneType == BaseZoneType.Upgrade)
            {
                if (_upgradeScreen != null) _upgradeScreen.Open();
            }
        }

        public void HandleZoneLeft(BaseZoneType zoneType)
        {
            if (zoneType == BaseZoneType.Sell)
            {
                if (_sellScreen != null) _sellScreen.Close();
            }
            else if (zoneType == BaseZoneType.Upgrade)
            {
                if (_upgradeScreen != null) _upgradeScreen.Close();
            }
        }
    }
}

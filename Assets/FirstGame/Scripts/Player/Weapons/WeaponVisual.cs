using FirstGame.Environment;
using UnityEngine;

namespace FirstGame.Players.Weapon
{
    public class WeaponVisual : MonoBehaviour
    {
        [SerializeField] private GameObject _axeModel;
        [SerializeField] private GameObject _pickaxeModel;

        private PlayerCombatController _cachedCombatController;

        private void Awake()
        {
            HideWeapon();
        }

        private void OnDestroy()
        {
            if (_cachedCombatController != null)
            {
                _cachedCombatController.OnAttackStarted -= EquipByTarget;
                _cachedCombatController.OnAttackStopped -= HideWeapon;
            }
        }

        public void BindToPlayer(Player player)
        {
            if (player == null)
            {
                return;
            }

            _cachedCombatController = player.GetComponent<PlayerCombatController>();

            if (_cachedCombatController != null)
            {
                _cachedCombatController.OnAttackStarted += EquipByTarget;
                _cachedCombatController.OnAttackStopped += HideWeapon;
            }
        }

        public void EquipByTarget(Component target)
        {
            HideWeapon();

            if (target == null)
            {
                return;
            }

            if (target.GetComponentInChildren<StoneVisual>() != null)
            {
                if (_pickaxeModel != null)
                {
                    _pickaxeModel.SetActive(true);
                }
            }
            else if (target.GetComponentInChildren<TreeVisual>() != null)
            {
                if (_axeModel != null)
                {
                    _axeModel.SetActive(true);
                }
            }
        }

        public void HideWeapon()
        {
            if (_axeModel != null)
            {
                _axeModel.SetActive(false);
            }

            if (_pickaxeModel != null)
            {
                _pickaxeModel.SetActive(false);
            }
        }
    }
}
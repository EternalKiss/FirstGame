using UnityEngine;

namespace FirstGame.Base
{
    public class BaseBackWallActivator : MonoBehaviour
    {
        [SerializeField] private Wall _backWall;
        [SerializeField] private EnterTriggerHandler _enterTrigger;

        private void Awake()
        {
            if (_backWall != null)
            {
                _backWall.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            if (_enterTrigger != null)
            {
                _enterTrigger.EnteredInBase += HandleEntered;
            }
        }

        private void OnDisable()
        {
            if (_enterTrigger != null)
            {
                _enterTrigger.EnteredInBase -= HandleEntered;
            }
        }

        private void HandleEntered()
        {
            if (_backWall != null)
            {
                _backWall.gameObject.SetActive(true);
            }
        }
    }
}
using UnityEngine;

namespace FirstGame.Base
{
    public class PlayerBase : MonoBehaviour
    {
        [SerializeField] private ExitTriggerHandler _exitTrigger;
        [SerializeField] private EnterTriggerHandler _enterTrigger;

        [SerializeField] private Transform _entranceGateMarker;
        [SerializeField] private Transform _exitGateMarker;

        public ExitTriggerHandler ExitTrigger => _exitTrigger;
        public EnterTriggerHandler EnterTrigger => _enterTrigger;

        public Vector3 EntrancePosition => _entranceGateMarker.position;
        public Vector3 ExitPosition => _exitGateMarker.position;
    }
}

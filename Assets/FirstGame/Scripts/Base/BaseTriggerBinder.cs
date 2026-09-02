using System;
using UnityEngine;

namespace FirstGame.Base
{
    public class BaseTriggerBinder
    {
        private readonly Action<Vector3> _onExited;
        private readonly Action _onEntered;

        public BaseTriggerBinder(Action<Vector3> onExited, Action onEntered)
        {
            _onExited = onExited;
            _onEntered = onEntered;
        }

        public void Bind(PlayerBase targetBase)
        {
            if (targetBase == null)
            {
                return;
            }

            targetBase.ExitTrigger.OnPlayerExited += _onExited;

            if (targetBase.EnterTrigger != null)
            {
                targetBase.EnterTrigger.EnteredInBase += _onEntered;
            }
        }

        public void Unbind(PlayerBase targetBase)
        {
            if (targetBase == null)
            {
                return;
            }

            targetBase.ExitTrigger.OnPlayerExited -= _onExited;

            if (targetBase.EnterTrigger != null)
            {
                targetBase.EnterTrigger.EnteredInBase -= _onEntered;
            }
        }
    }
}
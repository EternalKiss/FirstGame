using System;
using System.Collections.Generic;

namespace FirstGame.Common
{
    public class UniversalEventBinder
    {
        private readonly List<Action> _unbindActions = new List<Action>(16);

        public void Bind(Action<Action> subscribe, Action<Action> unsubscribe, Action callback)
        {
            subscribe?.Invoke(callback);
            _unbindActions.Add(() => unsubscribe?.Invoke(callback));
        }

        public void Bind<T>(Action<Action<T>> subscribe, Action<Action<T>> unsubscribe, Action<T> callback)
        {
            subscribe?.Invoke(callback);
            _unbindActions.Add(() => unsubscribe?.Invoke(callback));
        }

        public void Bind<T1, T2>(Action<Action<T1, T2>> subscribe, Action<Action<T1, T2>> unsubscribe, Action<T1, T2> callback)
        {
            subscribe?.Invoke(callback);
            _unbindActions.Add(() => unsubscribe?.Invoke(callback));
        }

        public void UnbindAll()
        {
            for (int index = _unbindActions.Count - 1; index >= 0; index--)
            {
                _unbindActions[index]?.Invoke();
            }

            _unbindActions.Clear();
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Loot
{
    public class LootObject : MonoBehaviour, IDestructible
    {
        public event Action<IDestructible> OnReadyToRelease;
    }
}

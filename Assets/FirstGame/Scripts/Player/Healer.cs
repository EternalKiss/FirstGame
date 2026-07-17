using System;
using UnityEngine;

namespace FirstGame.Player
{
    public class Healer : MonoBehaviour
    {
        public Action<float> Healed { get; internal set; }
    }
}
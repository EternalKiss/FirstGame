using System;
using UnityEngine;

namespace FirstGame.Players
{
    public class Healer : MonoBehaviour
    {
        public Action<float> Healed { get; internal set; }
    }
}
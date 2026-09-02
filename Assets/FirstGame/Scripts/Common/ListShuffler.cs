using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Common
{
    public static class ListShuffler
    {
        public static void Shuffle<T>(List<T> list)
        {
            int count = list.Count;
            for (int i = count - 1; i > 0; i--)
            {
                int rnd = Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[rnd];
                list[rnd] = temp;
            }
        }
    }
}
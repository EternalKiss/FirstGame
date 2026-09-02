using UnityEngine;

namespace FirstGame.Base
{
    public static class BasePositionCalculator
    {
        public static Vector3 CalculatePlacementPosition(Vector3 lastExitPosition, float corridorLength, Vector3 basePivotOffset)
        {
            Vector3 nextEntrancePos = lastExitPosition + Vector3.forward * corridorLength;

            return nextEntrancePos - basePivotOffset;
        }
    }
}

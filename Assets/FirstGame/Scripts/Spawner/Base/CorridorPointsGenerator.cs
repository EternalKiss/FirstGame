using System.Collections.Generic;
using UnityEngine;

namespace FistGame.Spawner
{
    public class CorridorPointsGenerator
    {
        private readonly float _width;
        private readonly float _cellSize;
        private readonly float _startOffset;
        private readonly float _baseSafetyRadiusSqr;

        public CorridorPointsGenerator(float width, float cellSize, float startOffset, float baseSafetyRadius)
        {
            _width = width;
            _cellSize = cellSize;
            _startOffset = startOffset;
            _baseSafetyRadiusSqr = baseSafetyRadius * baseSafetyRadius;
        }

        public void FillPoints(List<Vector3> points, Vector3 center, Vector3 direction, float length, float heightOffset)
        {
            points.Clear();
            int columns = Mathf.FloorToInt(_width / _cellSize);
            int rows = Mathf.FloorToInt(length / _cellSize);

            Vector3 corridorRight = new Vector3(direction.z, 0f, -direction.x);
            float startLeftOffset = -(_width * 0.5f) + (_cellSize * 0.5f);
            float halfCellSize = _cellSize * 0.15f;

            for (int r = 0; r < rows; r++)
            {
                float distanceForward = _startOffset + (r * _cellSize);

                for (int c = 0; c < columns; c++)
                {
                    float distanceSide = startLeftOffset + (c * _cellSize);

                    Vector3 point = new Vector3(
                        center.x + (direction.x * distanceForward) + (corridorRight.x * distanceSide),
                        center.y + heightOffset,
                        center.z + (direction.z * distanceForward) + (corridorRight.z * distanceSide)
                    );

                    if ((point - center).sqrMagnitude < _baseSafetyRadiusSqr)
                    {
                        continue;
                    }

                    point.x += Random.Range(-halfCellSize, halfCellSize);
                    point.z += Random.Range(-halfCellSize, halfCellSize);

                    points.Add(point);
                }
            }
        }
    }
}

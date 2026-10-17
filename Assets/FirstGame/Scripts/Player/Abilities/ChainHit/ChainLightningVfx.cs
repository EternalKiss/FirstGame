using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class ChainLightningVfx : MonoBehaviour
    {
        private static ChainLightningVfx _instance;

        [SerializeField] private LineRenderer _linePrefab;
        [SerializeField] private float _displayDuration = 0.4f;
        [SerializeField] private float _segmentDelay = 0.1f;

        private readonly List<LineRenderer> _spawnedLines = new List<LineRenderer>(8);
        private Coroutine _showCoroutine;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public static float SegmentDelay
        {
            get
            {
                if (_instance == null)
                {
                    return 0.1f;
                }

                return _instance._segmentDelay;
            }
        }

        public static void Play(List<Vector3> points)
        {
            if (_instance == null)
            {
                return;
            }

            if (_instance._showCoroutine != null)
            {
                _instance.StopCoroutine(_instance._showCoroutine);
            }

            _instance._showCoroutine = _instance.StartCoroutine(_instance.ShowLinesRoutine(points));
        }

        private IEnumerator ShowLinesRoutine(List<Vector3> points)
        {
            ClearLines();

            if (_linePrefab == null)
            {
                yield break;
            }

            for (int index = 0; index < points.Count - 1; index++)
            {
                LineRenderer line = Instantiate(_linePrefab, transform);

                line.positionCount = 2;
                line.SetPosition(0, points[index]);
                line.SetPosition(1, points[index + 1]);
                line.enabled = true;

                _spawnedLines.Add(line);

                if (index < points.Count - 2)
                {
                    yield return new WaitForSeconds(_segmentDelay);
                }
            }

            yield return new WaitForSeconds(_displayDuration);

            ClearLines();
            _showCoroutine = null;
        }

        private void ClearLines()
        {
            for (int index = 0; index < _spawnedLines.Count; index++)
            {
                if (_spawnedLines[index] != null)
                {
                    Destroy(_spawnedLines[index].gameObject);
                }
            }

            _spawnedLines.Clear();
        }
    }
}
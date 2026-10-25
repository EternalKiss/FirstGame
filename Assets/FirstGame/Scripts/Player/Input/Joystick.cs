using UnityEngine;
using UnityEngine.EventSystems;

namespace FirstGame.Players.Input
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;
        [SerializeField] private float _radius = 100f;
        [SerializeField] private float _deadZone = 0.1f;

        private Vector2 _direction;

        public Vector2 Direction => _direction;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_background == null || _handle == null)
            {
                return;
            }

            Vector2 localPoint;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _background,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint) == false)
            {
                return;
            }

            Vector2 rectCenter = _background.rect.size * (new Vector2(0.5f, 0.5f) - _background.pivot);
            Vector2 pointFromCenter = localPoint - rectCenter;
            Vector2 clamped = Vector2.ClampMagnitude(pointFromCenter, _radius);

            _handle.anchoredPosition = clamped;

            Vector2 normalized = clamped / _radius;

            if (normalized.magnitude < _deadZone)
            {
                normalized = Vector2.zero;
            }

            _direction = normalized;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _direction = Vector2.zero;

            if (_handle != null)
            {
                _handle.anchoredPosition = Vector2.zero;
            }
        }
    }
}
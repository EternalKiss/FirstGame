using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0,15f, -10f);

    [SerializeField] private bool _smoothing;
    [SerializeField] private float _smoothTime = 0.2f;

    private Vector3 _velocity;

    private void Start()
    {
        if (_target == null) return;
    }
     
    private void LateUpdate()
    {
        Vector3 desired = _target.position + _offset;

        if(_smoothing == false)
        {
            transform.position = desired;
            _velocity = Vector3.zero;
            return;
        }

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, _smoothTime);
    }
}

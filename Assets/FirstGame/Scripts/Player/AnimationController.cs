using UnityEngine;

namespace FirstGame.Players
{
    public class AnimationController : MonoBehaviour
    {
        private const string SpeedParam = "MoveSpeed";
        private const string IsRunningParam = "IsRunning";
        private const string AttackTrigger = "Attack";
        private const string AttackSpeedParam = "AttackSpeed";

        [SerializeField] private Animator _animator;

        private int _speedHash;
        private int _isRunningHash;
        private int _attackHash;
        private int _attackSpeedHash;

        private void Awake()
        {
            _speedHash = Animator.StringToHash(SpeedParam);
            _isRunningHash = Animator.StringToHash(IsRunningParam);
            _attackHash = Animator.StringToHash(AttackTrigger);
            _attackSpeedHash = Animator.StringToHash(AttackSpeedParam);
        }

        public void SetAttackSpeed(float value)
        {
            if (_animator != null)
            {
                _animator.SetFloat(_attackSpeedHash, value);
            }
        }

        public void Attack()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Attack"))
            {
                return;
            }

            _animator.SetTrigger(_attackHash);
        }

        public void SetSpeed(float value)
        {
            _animator.SetFloat(_speedHash, value);
        }

        public void SetIsRunning(bool isRunning)
        {
            _animator.SetBool(_isRunningHash, isRunning);
        }
    }
}

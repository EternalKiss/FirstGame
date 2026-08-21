using UnityEngine;

namespace FirstGame.Players
{
    public class AnimationController : MonoBehaviour
    {
        public const string AttackTrigger = "Attack";
        public const string SpeedParam = "MoveSpeed";
        public const string IsRunningParam = "IsRunning";
        public const string AttackSpeedParam = "AttackSpeed";
        public const string AttackEventMethodName = "OnAttackHitEvent";

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

            if (stateInfo.IsName(AttackTrigger))
            {
                return;
            }

            _animator.SetTrigger(_attackHash);
        }

        public void SynchronizeAnimationSpeed(float attackInterval, string clipName = AttackTrigger)
        {
            if (_animator == null) return;

            float originalClipLength = 1f;

            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Contains(clipName))
                {
                    originalClipLength = clip.length;
                    break;
                }
            }

            float calculatedSpeed = originalClipLength / attackInterval;

            _animator.SetFloat(_attackSpeedHash, calculatedSpeed);
        }

        public void SetSpeed(float value)
        {
            _animator.SetFloat(_speedHash, value);
        }

        public void SetIsRunning(bool isRunning)
        {
            _animator.SetBool(_isRunningHash, isRunning);
        }

        public void AddAttackEventViaCode(string clipName, string functionName, float hitTimePercent = 0.5f)
        {
            if (_animator == null) return;

            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Contains(clipName))
                {
                    if (HasEventAlready(clip, functionName)) return;

                    AnimationEvent attackEvent = new AnimationEvent();

                    attackEvent.functionName = functionName;

                    attackEvent.time = clip.length * Mathf.Clamp01(hitTimePercent);

                    clip.AddEvent(attackEvent);

                    Debug.Log($"[AnimationController] Событие {functionName} успешно добавлено в код клипа {clip.name} на {attackEvent.time} сек.");
                    break;
                }
            }
        }
        private bool HasEventAlready(AnimationClip clip, string functionName)
        {
            foreach (var ev in clip.events)
            {
                if (ev.functionName == functionName) return true;
            }
            return false;
        }
    }
}

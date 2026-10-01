using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Players
{
    public class AnimationController : MonoBehaviour
    {
        public const string AttackTrigger = "Attack";
        public const string MeleeAbilityTrigger = "MeleeAbility";
        public const string SpeedParam = "MoveSpeed";
        public const string IsRunningParam = "IsRunning";
        public const string AttackSpeedParam = "AttackSpeed";
        public const string AttackEventMethodName = "OnAttackHitEvent";
        public const string MeleeAbilityEventMethodName = "OnAbilityHitEvent";

        [SerializeField] private Animator _animator;

        private readonly HashSet<int> _availableParameters = new HashSet<int>();

        private int _speedHash;
        private int _isRunningHash;
        private int _attackHash;
        private int _attackSpeedHash;
        private int _meleeAbilityHash;

        private void Awake()
        {
            _speedHash = Animator.StringToHash(SpeedParam);
            _isRunningHash = Animator.StringToHash(IsRunningParam);
            _attackHash = Animator.StringToHash(AttackTrigger);
            _attackSpeedHash = Animator.StringToHash(AttackSpeedParam);
            _meleeAbilityHash = Animator.StringToHash(MeleeAbilityTrigger);

            CacheAvailableParameters();
        }

        private void CacheAvailableParameters()
        {
            _availableParameters.Clear();

            if (_animator == null)
            {
                return;
            }

            AnimatorControllerParameter[] parameters = _animator.parameters;

            for (int index = 0; index < parameters.Length; index++)
            {
                _availableParameters.Add(parameters[index].nameHash);
            }
        }

        private bool HasParameter(int hash)
        {
            return _availableParameters.Contains(hash);
        }

        public void PlayMeleeAbility()
        {
            if (_animator == null)
            {
                return;
            }

            if (HasParameter(_meleeAbilityHash) == false)
            {
                return;
            }

            _animator.SetTrigger(_meleeAbilityHash);
        }

        public void RegisterMeleeAbilityEvent(string clipName, float hitTimePercent)
        {
            AddAttackEventViaCode(clipName, MeleeAbilityEventMethodName, hitTimePercent);
        }

        public void SetAttackSpeed(float value)
        {
            if (_animator == null)
            {
                return;
            }

            if (HasParameter(_attackSpeedHash) == false)
            {
                return;
            }

            _animator.SetFloat(_attackSpeedHash, value);
        }

        public void Attack()
        {
            if (_animator == null)
            {
                return;
            }

            if (HasParameter(_attackHash) == false)
            {
                return;
            }

            if (HasParameter(_attackSpeedHash))
            {
                _animator.SetFloat(_attackSpeedHash, _animator.GetFloat(_attackSpeedHash));
            }

            _animator.SetTrigger(_attackHash);
        }

        public void ResetAttackTrigger()
        {
            if (_animator == null)
            {
                return;
            }

            if (HasParameter(_attackHash) == false)
            {
                return;
            }

            _animator.ResetTrigger(_attackHash);
        }

        public void SynchronizeAnimationSpeed(float attackInterval, string clipName = AttackTrigger)
        {
            if (_animator == null)
            {
                return;
            }

            if (HasParameter(_attackSpeedHash) == false)
            {
                return;
            }

            AnimationClip clip = FindClip(clipName);

            float originalClipLength = clip != null ? clip.length : 1f;
            float calculatedSpeed = originalClipLength / attackInterval;

            _animator.SetFloat(_attackSpeedHash, calculatedSpeed);
        }

        public float GetClipLength(string clipName)
        {
            AnimationClip clip = FindClip(clipName);

            if (clip == null)
            {
                return 1f;
            }

            return clip.length;
        }

        public void SetSpeed(float value)
        {
            if (_animator == null)
            {
                return;
            }

            if (HasParameter(_speedHash) == false)
            {
                return;
            }

            _animator.SetFloat(_speedHash, value);
        }

        public void SetIsRunning(bool isRunning)
        {
            if (_animator == null)
            {
                return;
            }

            if (HasParameter(_isRunningHash) == false)
            {
                return;
            }

            _animator.SetBool(_isRunningHash, isRunning);
        }

        public void AddAttackEventViaCode(string clipName, string functionName, float hitTimePercent = 0.5f)
        {
            AnimationClip clip = FindClip(clipName);

            if (clip == null)
            {
                return;
            }

            if (HasEventAlready(clip, functionName))
            {
                return;
            }

            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.functionName = functionName;
            animationEvent.time = clip.length * Mathf.Clamp01(hitTimePercent);
            clip.AddEvent(animationEvent);
        }

        private AnimationClip FindClip(string clipName)
        {
            if (_animator == null)
            {
                return null;
            }

            if (_animator.runtimeAnimatorController == null)
            {
                return null;
            }

            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Contains(clipName))
                {
                    return clip;
                }
            }

            return null;
        }

        private bool HasEventAlready(AnimationClip clip, string functionName)
        {
            foreach (var animationEvent in clip.events)
            {
                if (animationEvent.functionName == functionName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

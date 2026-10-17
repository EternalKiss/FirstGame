using FirstGame.Environment;
using FirstGame.Interfaces;
using UnityEngine;

namespace FirstGame.Sound
{
    public class AudioService : MonoBehaviour
    {
        public static AudioService Instance { get; private set; }

        [Header("Source")]
        [SerializeField] private AudioSource _sfxSource;

        [Header("Player")]
        [SerializeField] private AudioClip _levelUpClip;
        [SerializeField] private AudioClip _playerDeathClip;
        [SerializeField] private AudioClip _playerHitClip;

        [Header("Enemy")]
        [SerializeField] private AudioClip _enemyDeathClip;
        [SerializeField] private AudioClip _enemyHitClip;

        [Header("World")]
        [SerializeField] private AudioClip _nightStartClip;
        [SerializeField] private AudioClip _sunriseClip;

        [Header("Resource Hit")]
        [SerializeField] private AudioClip _stoneHitClip;
        [SerializeField] private AudioClip _treeHitClip;

        [Header("Ability")]
        [SerializeField] private AudioClip _areaChopClip;
        [SerializeField] private AudioClip _chainHitClip;

        [Header("Loot")]
        [SerializeField] private AudioClip _lootPickupClip;

        [Header("UI")]
        [SerializeField] private AudioClip _uiClickClip;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void PlayLevelUp()
        {
            Play(_levelUpClip);
        }

        public void PlayPlayerDeath()
        {
            Play(_playerDeathClip);
        }

        public void PlayPlayerHit()
        {
            Play(_playerHitClip);
        }

        public void PlayStoneHit()
        {
            Play(_stoneHitClip);
        }

        public void PlayTreeHit()
        {
            Play(_treeHitClip);
        }

        public void PlayEnemyDeath()
        {
            Play(_enemyDeathClip);
        }

        public void PlayEnemyHit()
        {
            Play(_enemyHitClip);
        }

        public void PlayNightStart()
        {
            Play(_nightStartClip);
        }

        public void PlaySunrise()
        {
            Play(_sunriseClip);
        }

        public void PlayLootPickup()
        {
            Play(_lootPickupClip);
        }

        public void PlayUIClick()
        {
            Play(_uiClickClip);
        }

        public void PlayAreaChop()
        {
            Play(_areaChopClip);
        }

        public void PlayChainHit()
        {
            Play(_chainHitClip);
        }

        public void PlayHitOnTarget(IDamageable target)
        {
            if (target is not Component component)
            {
                return;
            }

            if (component.GetComponentInChildren<StoneVisual>() != null)
            {
                PlayStoneHit();
                return;
            }

            if (component.GetComponentInChildren<TreeVisual>() != null)
            {
                PlayTreeHit();
                return;
            }

            PlayEnemyHit();
        }

        private void Play(AudioClip clip)
        {
            if (clip == null) return;
            if (_sfxSource == null) return;

            _sfxSource.PlayOneShot(clip);
        }
    }
}

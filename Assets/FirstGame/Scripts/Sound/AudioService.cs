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

        [Header("World")]
        [SerializeField] private AudioClip _nightStartClip;
        [SerializeField] private AudioClip _sunriseClip;

        [Header("Resource Hit")]
        [SerializeField] private AudioClip _stoneHitClip;
        [SerializeField] private AudioClip _treeHitClip;

        [Header("Loot")]
        [SerializeField] private AudioClip _lootPickupClip;

        [Header("UI")]
        [SerializeField] private AudioClip _uiClickClip;

        private void Awake()
        {
            if(Instance == null)
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

        private void Play(AudioClip clip)
        {
            if (clip == null) return;
            if (_sfxSource == null) return;

            Debug.Log("[Audio] Play: " + clip.name);
            _sfxSource.PlayOneShot(clip);
        }
    }
}

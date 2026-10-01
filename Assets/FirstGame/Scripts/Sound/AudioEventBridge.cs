using FirstGame.Combat;
using FirstGame.Common;
using FirstGame.Environment;
using FirstGame.LevelManager;
using FirstGame.Players;
using FirstGame.Players.Inventore;
using FirstGame.Players.Level;
using UnityEngine;

namespace FirstGame.Sound
{
    public class AudioEventBridge : MonoBehaviour
    {
        [SerializeField] private AudioService _audioService;

        private readonly UniversalEventBinder _eventBinder = new UniversalEventBinder();

        public void BindToPlayer(Player player)
        {
            if (player == null) return;
            if (_audioService == null) return;

            Health health = player.GetComponent<Health>();
            Experience experience = player.GetComponent<Experience>();
            Inventory inventory = player.GetComponent<Inventory>();
            PlayerCombatController combatController = player.GetComponent<PlayerCombatController>();
            PlayerDamageReceiver damageRecevier = player.GetComponent<PlayerDamageReceiver>();

            if (combatController != null)
            {
                _eventBinder.Bind<Component>(
                    callback => combatController.OnAttackHit += callback,
                    callback => combatController.OnAttackHit -= callback,
                    HandleAttackHit);
            }

            if (health != null)
            {
                _eventBinder.Bind(
                    callback => health.Died += callback,
                    callback => health.Died -= callback,
                    HandlePlayerDied);

                _eventBinder.Bind(
                    callback => damageRecevier.Damaged += callback,
                    callback => damageRecevier.Damaged -= callback,
                    HandleHealthChanged);
            }

            if (experience != null)
            {
                _eventBinder.Bind<int>(
                    callback => experience.OnLevelUp += callback,
                    callback => experience.OnLevelUp -= callback,
                    HandleLevelUp);
            }

            if (inventory != null)
            {
                _eventBinder.Bind<int>(
                    callback => inventory.StoneAdded += callback,
                    callback => inventory.StoneAdded -= callback,
                    HandleLootPickup);

                _eventBinder.Bind<int>(
                    callback => inventory.TreeAdded += callback,
                    callback => inventory.TreeAdded -= callback,
                    HandleLootPickup);
            }
        }

        public void BindToDayNight(DayNightCycle dayNightCycle)
        {
            if (dayNightCycle == null) return;
            if (_audioService == null) return;

            _eventBinder.Bind(
                callback => dayNightCycle.NightStarted += callback,
                callback => dayNightCycle.NightStarted -= callback,
                HandleNightStarted);

            _eventBinder.Bind(
                callback => dayNightCycle.SunriseStarted += callback,
                callback => dayNightCycle.SunriseStarted -= callback,
                HandleSunrise);
        }

        private void OnDisable()
        {
            _eventBinder.UnbindAll();
        }

        private void HandlePlayerDied()
        {
            _audioService.PlayPlayerDeath();
        }

        private void HandleAttackHit(Component target)
        {
            if (target == null) return;

            if (target.GetComponentInChildren<StoneVisual>() != null)
            {
                _audioService.PlayStoneHit();
                return;
            }

            if (target.GetComponentInChildren<TreeVisual>() != null)
            {
                _audioService.PlayTreeHit();
            }
        }

        private void HandleHealthChanged()
        {
            _audioService.PlayPlayerHit();
        }

        private void HandleLevelUp(int level)
        {
            _audioService.PlayLevelUp();
        }

        private void HandleLootPickup(int count)
        {
            _audioService.PlayLootPickup();
        }

        private void HandleNightStarted()
        {
            _audioService.PlayNightStart();
        }

        private void HandleSunrise()
        {
            _audioService.PlaySunrise();
        }
    }
}
using FirstGame.Players.Level;
using FirstGame.Players.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.PlayerUI
{
    public class LevelUpScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _rootPanel;
        [SerializeField] private Transform _cardsContainer;
        [SerializeField] private AbilityCardView _cardPrefab;
        [SerializeField] private AbilityDatabase _database;
        [SerializeField] private int _cardsCount = 3;
        [SerializeField] private bool _pauseGame = true;

        private Experience _experience;
        private AbilitySlotController _slotController;

        private readonly List<AbilityCardView> _spawnedCards = new List<AbilityCardView>(3);

        private void Awake()
        {
            if (_rootPanel != null)
            {
                _rootPanel.SetActive(false);
            }
        }

        public void Initialize(Experience experience, AbilitySlotController slotController)
        {
            _experience = experience;
            _slotController = slotController;

            if (_experience != null)
            {
                _experience.OnLevelUp += HandleLevelUp;
            }
        }

        private void OnDestroy()
        {
            if (_experience != null)
            {
                _experience.OnLevelUp -= HandleLevelUp;
            }
        }

        private void HandleLevelUp(int level)
        {
            if (_slotController == null || _slotController.HasFreeSlot == false)
            {
                return;
            }

            Open();
        }

        private void Open()
        {
            ClearCards();

            List<AbilityCardData> cards = _database.GetRandomCards(_cardsCount, _slotController);

            if (cards.Count == 0)
            {
                return;
            }

            if (_rootPanel != null)
            {
                _rootPanel.SetActive(true);
            }

            if (_pauseGame)
            {
                Time.timeScale = 0f;
            }

            for (int index = 0; index < cards.Count; index++)
            {
                AbilityCardData cardData = cards[index];

                AbilityCardView view = Instantiate(_cardPrefab, _cardsContainer);

                string displayName = cardData.Ability.name;
                string description = _database.DefaultDescription;
                Sprite icon = _database.DefaultIcon;

                view.Setup(cardData.Ability, displayName, description, icon, HandleCardSelected);
                _spawnedCards.Add(view);
            }
        }

        private void HandleCardSelected(AbilityBase ability)
        {
            if (_slotController != null)
            {
                _slotController.TryUnlock(ability);
            }

            Close();
        }

        private void Close()
        {
            ClearCards();

            if (_rootPanel != null)
            {
                _rootPanel.SetActive(false);
            }

            if (_pauseGame)
            {
                Time.timeScale = 1f;
            }
        }

        private void ClearCards()
        {
            for (int index = 0; index < _spawnedCards.Count; index++)
            {
                if (_spawnedCards[index] != null)
                {
                    Destroy(_spawnedCards[index].gameObject);
                }
            }

            _spawnedCards.Clear();
        }
    }
}

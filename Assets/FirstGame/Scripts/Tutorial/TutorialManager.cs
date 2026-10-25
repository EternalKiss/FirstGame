using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

namespace FirstGame.GameTutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }

        [Header("UI Elements")]
        [SerializeField] private GameObject _tutorialRoot;
        [SerializeField] private Image _darkOverlay;
        [SerializeField] private TMP_Text _tutorialText;
        [SerializeField] private RectTransform _handPointer;

        private int _step = 0;
        private bool _isTutorialActive = false;

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

        private void Start()
        {
            // ВРЕМЕННО ДЛЯ ТЕСТА: принудительно сбрасываем сохранение
            YG2.saves.isTutorialCompleted = 0;
            // -----------------------------

            if (YG2.saves.isTutorialCompleted == 1)
            {
                _tutorialRoot.SetActive(false);
                return;
            }

            StartTutorial();
        }

        private void StartTutorial()
        {
            Debug.Log("=== TUTORIAL FORCED TO START ===");
            _isTutorialActive = true;

            if (_tutorialRoot == null) Debug.LogError("TutorialRoot is NULL!");
            if (_darkOverlay == null) Debug.LogError("DarkOverlay is NULL!");
            if (_tutorialText == null) Debug.LogError("TutorialText is NULL!");

            if (_tutorialRoot != null) _tutorialRoot.SetActive(true);
            if (_darkOverlay != null) _darkOverlay.gameObject.SetActive(true);

            Time.timeScale = 0f;
            SetStep(1);
        }

        public void SetStep(int stepIndex)
        {
            _step = stepIndex;

            if (_tutorialText == null) return;

            switch (_step)
            {
                case 1:
                    _tutorialText.text = "Двигайся с помощью джойстика";
                    if (_handPointer != null) _handPointer.gameObject.SetActive(true);
                    break;
                case 2:
                    _tutorialText.text = "Подойди к дереву, чтобы рубить его";
                    if (_handPointer != null) _handPointer.gameObject.SetActive(false);
                    break;
                case 3:
                    _tutorialText.text = "Успей добежать до базы до наступления ночи!";
                    break;
                case 4:
                    _tutorialText.text = "Зайди на базу и улучши способность";
                    break;
            }
        }

        public void OnPlayerMoved()
        {
            if (!_isTutorialActive) return;

            if (_step == 1)
            {
                Time.timeScale = 1f;
                if (_darkOverlay != null) _darkOverlay.gameObject.SetActive(false);
                SetStep(2);
            }
        }

        public void OnTreeChopped()
        {
            if (!_isTutorialActive) return;
            if (_step == 2) SetStep(3);
        }

        public void OnBaseEntered()
        {
            if (!_isTutorialActive) return;
            if (_step == 3) SetStep(4);
        }

        public void OnUpgradePurchased()
        {
            if (!_isTutorialActive) return;

            if (_step == 4)
            {
                CompleteTutorial();
            }
        }

        public void SkipTutorial()
        {
            if (!_isTutorialActive) return;
            CompleteTutorial();
        }

        private void CompleteTutorial()
        {
            _isTutorialActive = false;

            if (_tutorialRoot != null) _tutorialRoot.SetActive(false);
            Time.timeScale = 1f;

            YG2.saves.isTutorialCompleted = 1;
            YG2.SaveProgress();
        }
    }
}
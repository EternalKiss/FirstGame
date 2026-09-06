using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstGame.PlayerUI
{
    public class XpViewer : MonoBehaviour
    {
        [SerializeField] private Slider _xpSlider;
        [SerializeField] private TMP_Text _levelText;

        public void UpdateXpBar(float currentXp, float maxXp)
        {
            if (_xpSlider != null)
            {
                _xpSlider.maxValue = maxXp;
                _xpSlider.value = currentXp;
            }
        }

        public void UpdateLevelText(int currentLevel)
        {
            if (_levelText != null)
            {
                _levelText.text = $"LVL {currentLevel}";
            }
        }
    }
}

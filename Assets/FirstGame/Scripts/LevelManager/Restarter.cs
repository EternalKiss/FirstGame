using UnityEngine;
using UnityEngine.SceneManagement;

namespace FirstGame.LevelManager
{
    public class Restarter : MonoBehaviour
    {
        public void RestartLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}

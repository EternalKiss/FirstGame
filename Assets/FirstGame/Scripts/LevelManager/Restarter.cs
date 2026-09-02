using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace FirstGame.LevelManager
{
    public class Restarter : MonoBehaviour
    {
        public async void RestartLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneIndex);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }
        }
    }
}

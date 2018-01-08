using Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ApplicationSetUp : MonoBehaviour
    {
        void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        public static void LoadScreen(string sceneName)
        {
            ScreenManager manager = FindObjectOfType<ScreenManager>();
            if (manager != null)
            {
                manager.LoadScreen(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        public void ResetStars()
        {
            int index = 1;
            string key = LevelSettings.GetStarsInfoKey(index);
            while (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetInt(key, 0);
                key = LevelSettings.GetStarsInfoKey(++index);
            }
            LoadScreen("MainMenu");
        }
    }
}

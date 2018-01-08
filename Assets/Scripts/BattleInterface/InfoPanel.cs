using BattleInterface.Structures;
using Level;
using Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleInterface
{
    public class InfoPanel : MonoBehaviour
    {
        public Text scores;
        public Transform objectives;
        public GameObject endWindow;
        public Slider fuel;

        public void Init()
        {
            endWindow.SetActive(false);

            fuel.maxValue = 1;
            fuel.minValue = 0;
        }

        public void SetScores(int scoresValue)
        {
            scores.text = scoresValue.ToString();
        }

        public void UpdateStarsCount()
        {
            if (objectives == null) return;
            int starsCount = LevelSettings.GetStarsCount();

            int initialChildCount = objectives.childCount;
            for (int i = 0; i < initialChildCount; i++)
            {
                objectives.GetChild(i).GetChild(1).gameObject.SetActive(i < starsCount);
            }
        }

        public void ManuallyEndLevel()
        {
            ShipBehaviour ship = FindObjectOfType<ShipBehaviour>();
            ship.Die();
            ShowEndWnidow();
        }

        public void ShowEndWnidow()
        {
            ShowEndWnidowFor(LevelSettings.GetLevelId(), LevelSettings.GetConditions(), 
                FindActiveScene());
        }

        private void ShowEndWnidowFor(string levelId, StarCondition[] conditions, string activeScene)
        {
            endWindow.SetActive(true);
            endWindow.GetComponent<EndLevelWindow>().Init(levelId, conditions, activeScene);
        }

        private string FindActiveScene()
        {
            Scene scene;
            int size = SceneManager.sceneCount;
            for (int i = 0; i < size; i++)
            {
                scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded && scene.name != "LoadingScreen") 
                    return scene.name;
            }
            return "MainMenu";
        }

        public void ShowFuel(float value)
        {
            fuel.value = value;
        }
    }
}
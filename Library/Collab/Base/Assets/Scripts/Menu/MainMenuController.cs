using DG.Tweening;
using Level;
using UnityEngine;

namespace Menu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        protected string infiniteLevelScene = "InfiniteCosmos";
        
        [SerializeField]
        protected Transform ship;

        private string currentLevel;
        
        public void StartInfiniteLevel()
        {
            LevelSettings.SetLevelId(infiniteLevelScene);
            MoveForward();
        }

        private void MoveForward()
        {
            ship.DOMoveX(16f, 1f).SetEase(Ease.InFlash).OnComplete(LoadCurrentLevel);
        }

        void LoadCurrentLevel()
        {
            ApplicationSetUp.LoadScreen(LevelSettings.GetLevelId());
        }
    }
}
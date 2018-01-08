using DigitalRuby.Tween;
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
            ship.gameObject.Tween("MoveShip", ship.transform.position.x,
                14f, 
                1f, 
                TweenScaleFunctions.QuarticEaseIn, 
                (t) => { ship.Translate(t.CurrentValue - ship.transform.position.x, 0f, 0f); },
                (t) => { LoadCurrentLevel(); }
            );
        }

        void LoadCurrentLevel()
        {
            ApplicationSetUp.LoadScreen(LevelSettings.GetLevelId());
        }
    }
}
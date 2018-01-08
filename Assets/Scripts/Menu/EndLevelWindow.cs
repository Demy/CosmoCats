using BattleInterface.Structures;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class EndLevelWindow : StartLevelWindow
    {
        [SerializeField] private Transform stars;
        private bool[] conditionsMask;

        public override void Init(string levelName, StarCondition[] conditions, string sceneName)
        {
            conditionsMask = LevelSettings.GetConditionsMask();
            FillStars(stars, conditionsMask);
            base.Init(levelName, conditions, sceneName);
        }

        private void FillStars(Transform stars, bool[] mask)
        {
            int starsCount = 0;
            for (int i = 0; i < mask.Length; i++) if (mask[i]) ++starsCount;
            int length = stars.childCount;
            for (int i = 0; i < length; i++)
            {
                stars.GetChild(i).GetChild(1).gameObject.SetActive(i < starsCount);
            }
        }

        protected override void FillObjectives(StartLevelCondition[] startLevelConditions, StarCondition[] conditions)
        {
            if (conditions == null) return;
            int length = objectives.Length;
            int conditionsCount = conditions.Length;
            for (int i = 0; i < length; i++)
            {
                if (conditionsCount <= i)
                {
                    objectives[i].gameObject.SetActive(false);
                }
                else
                {
                    objectives[i].gameObject.SetActive(true);
                    objectives[i].FillCondition(conditions[i], conditionsMask[i]);
                }
            }
        }

        public void RepeatPressed()
        {
            ApplicationSetUp.LoadScreen(sceneName ?? LevelSettings.GetLevelId());
        }

        public override void OkPressed()
        {
            ApplicationSetUp.LoadScreen("Editor");
        }
    }
}
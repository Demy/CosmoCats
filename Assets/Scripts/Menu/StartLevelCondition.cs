using BattleInterface.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class StartLevelCondition : MonoBehaviour
    {
        [SerializeField] private Text title;
        [SerializeField] private Transform icons;
        [SerializeField] private Transform check;

        public void FillCondition(StarCondition condition, bool isAchieved = false)
        {
            title.text = condition.GetTypeName();
            SetIcon(condition.type);
            if (check != null) check.gameObject.SetActive(isAchieved);
        }

        private void SetIcon(StarCondition.Type conditionType)
        {
            if (conditionType == StarCondition.Type.Objectives) ShowIcon(0);
            if (conditionType == StarCondition.Type.Scores) ShowIcon(1);
            if (conditionType == StarCondition.Type.LevelComplete) ShowIcon(2);
        }

        private void ShowIcon(int index)
        {
            for (int i = 0; i < icons.childCount; i++)
            {
                icons.GetChild(i).gameObject.SetActive(i == index);
            }
        }
    }
}
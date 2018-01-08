using System;

namespace BattleInterface.Structures
{
    [Serializable]
    public class StarCondition
    {
        public enum Type
        {
            LevelComplete, Scores, Objectives, ShotsCount
        }

        public Type type;
        public int value;

        public bool IsAchieved(int scoresValue, int objectivesValue, int shotsFired, bool isLevelFinished)
        {
            bool result = false;
            switch (type)
            {
                case Type.LevelComplete:
                    result = isLevelFinished;
                    break;
                case Type.Scores:
                    result = scoresValue >= value;
                    break;
                case Type.Objectives:
                    result = objectivesValue >= value;
                    break;
                case Type.ShotsCount:
                    result = shotsFired <= value;
                    break;
            }
            return result;
        }

        public string GetTypeName()
        {
            string result = "";
            switch (type)
            {
                case Type.LevelComplete:
                    result = "Complete Level";
                    break;
                case Type.Scores:
                    result = "Collect " + value + " coins";
                    break;
                case Type.Objectives:
                    result = "Save " + value + " cats";
                    break;
                case Type.ShotsCount:
                    result = "Make only " + value + " shots";
                    break;
            }
            return result;
        }
    }
}
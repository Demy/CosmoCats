using BattleInterface.Structures;
using UnityEngine;

namespace Level
{
    public class LevelSettings
    {
        private static StarCondition[] _conditions;

        private static int _levelIndex;
        private static string _levelId;
        private static LevelList _levelStructure;

        private static int scoresValue;
        private static int objectivesValue;
        private static int shotsFired;
        private static bool isLevelFinished;
        
        public static bool isLevelRandomOn = true;
        public static bool firstPieceIsEmpty = true;

        public static void SetUpLevel(int levelIndex, string levelId, StarCondition[] conditions)
        {
            _levelIndex = levelIndex;
            _levelId = levelId;
            _conditions = conditions;

            scoresValue = 0;
            objectivesValue = 0;
            shotsFired = 0;
            isLevelFinished = false;
        }

        public static void SetLevelId(string value)
        {
            _levelId = value;
        }

        public static void SetScoresCount(int value)
        {
            scoresValue = value;
        }

        public static void SetObjectivesCount(int value)
        {
            objectivesValue = value;
        }

        public static void AddShotsCount(int value)
        {
            shotsFired += value;
        }

        public static void SetLevelFinished(bool value)
        {
            isLevelFinished = value;
        }

        public static string GetLevelId()
        {
            return _levelId;
        }

        public static StarCondition[] GetConditions()
        {
            return _conditions;
        }

        public static int GetStarsCount()
        {
            if (_conditions == null) return 0;
            int starsCount = 0;
            for (int i = 0; i < _conditions.Length; i++)
                starsCount +=
                    _conditions[i].IsAchieved(scoresValue, objectivesValue, shotsFired, isLevelFinished) ? 1 : 0;
            return starsCount;
        }

        public static bool[] GetConditionsMask()
        {
            bool[] mask = new bool[] { false, false, false };
            if (_conditions == null) return mask;
            for (int i = 0; i < _conditions.Length; i++)
                mask[i] = 
                    _conditions[i].IsAchieved(scoresValue, objectivesValue, shotsFired, isLevelFinished);
            return mask;
        }

        public static void SaveStarsCountForLevel()
        {
            int starsCount = GetStarsCount();
            string starsKey = GetStarsInfoKey(_levelIndex);
            if (PlayerPrefs.GetInt(starsKey) < starsCount) PlayerPrefs.SetInt(starsKey, starsCount);
        }

        public static string GetStarsInfoKey(int levelIndex)
        {
            return "Level" + levelIndex + "Stars";
        }

        public static void SetStructureOfPieces(LevelList list)
        {
            _levelStructure = list;
        }

        public static LevelList GetStructureOfPieces()
        {
            return _levelStructure;
        }
    }
}
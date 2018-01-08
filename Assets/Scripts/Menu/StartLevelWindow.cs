using BattleInterface.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
	public class StartLevelWindow : MonoBehaviour
	{
		[SerializeField] protected Text title;
		[SerializeField] protected StartLevelCondition[] objectives;

		protected string sceneName;

		public virtual void Init(string levelName, StarCondition[] conditions, string sceneName)
		{
			this.sceneName = sceneName;
			title.text = levelName;

			FillObjectives(objectives, conditions);
		}

		protected virtual void FillObjectives(StartLevelCondition[] startLevelConditions, StarCondition[] conditions)
		{
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
					objectives[i].FillCondition(conditions[i]);
				}
			}
		}

		public virtual void OkPressed()
		{
			ApplicationSetUp.LoadScreen(sceneName);
		}
	}
}

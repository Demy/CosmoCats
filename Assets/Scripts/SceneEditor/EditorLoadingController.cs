using UnityEngine;
using Level;
using User;

namespace SceneEditor
{
	[RequireComponent(typeof(LoadingController))]
	public class EditorLoadingController : MonoBehaviour
	{
		private LoadingController loader;
		private EditorInterfaceController editor;

		void Start()
		{
			loader = GetComponent<LoadingController>();
			loader.CreateSlots();
			
			UserProfile profile = new UserProfile(0, PlayerPrefs.GetInt("UserScore"));
			LevelSettings.SetUserProfile(profile);

			editor = FindObjectOfType<EditorInterfaceController>();
			editor.ShowSlots(loader.GetSlots(), LoadSlot);
			editor.ShowLoading(true);
			LoadSlot(loader.GetCurrentSlot());
		}

		public void SaveLevels(LevelList list)
		{
			if (list != null)
			{
				loader.SaveLevels(list, delegate
				{
					FindObjectOfType<ButtonIconChanger>().TemporaryChange();
				});
			}
		}

		public void LoadSlot(int slot)
		{
			loader.LoadSlot(slot, delegate(LevelList levelList)
			{
				editor.ShowLevels(levelList);
				editor.ShowLoading(false);
			});
		}
	}
}
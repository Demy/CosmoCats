using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine.Networking;

namespace SceneEditor
{
	public class LoadingController : MonoBehaviour
	{
		public struct SaveSlot
		{
			public string name;
			public string url;
		}
		
		private const string URL_BASE = "http://andminigames.ru/stuff/cosmocats/";
		private string writeUrl = "write.php";
		private List<SaveSlot> slots = new List<SaveSlot>();

		private static int currentSlot;

		private EditorInterfaceController editor;

		void Start()
		{
			CreateSlots();

			editor = FindObjectOfType<EditorInterfaceController>();
			editor.ShowSlots(slots, LoadSlot);
			editor.ShowLoading(true);
			LoadSlot(currentSlot);
		}

		public void SaveCurrent()
		{
			
		}

		public IEnumerator SaveLevels(LevelList list)
		{
			if (list != null)
			{
				WWWForm form = new WWWForm();
				form.AddField("levels", JsonUtility.ToJson(list));
				form.AddField("source", slots[currentSlot].url);

				UnityWebRequest www = UnityWebRequest.Post(URL_BASE + writeUrl, form);
				yield return www.Send();
 
				FindObjectOfType<ButtonIconChanger>().TemporaryChange();
			}
		}

		public void LoadSlot(int slot)
		{
			currentSlot = slot;
			StartCoroutine(GetLevesIn(slot));
		}

		public IEnumerator GetLevesIn(int slot)
		{
			UnityWebRequest www = UnityWebRequest.Get(URL_BASE + slots[slot].url);
			yield return www.Send();
 
			editor.ShowLevels(JsonUtility.FromJson<LevelList>(www.downloadHandler.text));
			editor.ShowLoading(false);
		}

		private void CreateSlots()
		{
			SaveSlot timur = new SaveSlot();
			timur.name = "Timur";
			timur.url = "timur.json";
			slots.Add(timur);
			SaveSlot godzilla = new SaveSlot();
			godzilla.name = "Godzilla";
			godzilla.url = "godzilla.json";
			slots.Add(godzilla);
			SaveSlot mySlot = new SaveSlot();
			mySlot.name = "Lena";
			mySlot.url = "lena.json";
			slots.Add(mySlot);
		}

	}
}
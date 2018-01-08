using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;
using UnityEngine.Networking;

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

    public IEnumerator SaveLevels(LevelList list, Action callback)
    {
        if (list != null)
        {
            WWWForm form = new WWWForm();
            form.AddField("levels", JsonUtility.ToJson(list));
            form.AddField("source", slots[currentSlot].url);

            UnityWebRequest www = UnityWebRequest.Post(URL_BASE + writeUrl, form);
            yield return www.Send();

            callback();
        }
    }

    public void LoadSlot(int slot, Action<LevelList> callback)
    {
        currentSlot = slot;
        StartCoroutine(GetLevesIn(slot, callback));
    }

    public IEnumerator GetLevesIn(int slot, Action<LevelList> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL_BASE + slots[slot].url);
        yield return www.Send();

        callback(JsonUtility.FromJson<LevelList>(www.downloadHandler.text));
    }

    public void CreateSlots()
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

    public List<SaveSlot> GetSlots()
    {
        return slots;
    }

    public int GetCurrentSlot()
    {
        return currentSlot;
    }
}
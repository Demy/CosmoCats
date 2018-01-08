using System;
using System.Collections.Generic;
using System.IO;
using Level;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    public struct SaveSlot
    {
        public string name;
        public string path;
    }
    
    private List<SaveSlot> slots = new List<SaveSlot>();

    private static int currentSlot;

    public void SaveLevels(LevelList list, Action callback)
    {
        if (list != null)
        {
            string dataAsJson = JsonUtility.ToJson(list);

            File.WriteAllText (GetSaveFileForSlot(currentSlot), dataAsJson);
            
            callback();
        }
    }

    public void LoadSlot(int slot, Action<LevelList> callback)
    {
        currentSlot = slot;
        GetLevesIn(slot, callback);
    }

    public void GetLevesIn(int slot, Action<LevelList> callback)
    {
        string filePath = GetSaveFileForSlot(slot);

        if(File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath); 
            callback(JsonUtility.FromJson<LevelList>(dataAsJson));
        }
        else
        {
            Debug.LogError("Cannot load level data from " + filePath);
        }
    }

    private string GetSaveFileForSlot(int slot)
    {
        return Path.Combine(Application.streamingAssetsPath, slots[slot].path);
    }

    public void CreateSlots()
    {
        SaveSlot defaultSlot = new SaveSlot();
        defaultSlot.name = "Default";
        defaultSlot.path = "default.json";
        slots.Add(defaultSlot);
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
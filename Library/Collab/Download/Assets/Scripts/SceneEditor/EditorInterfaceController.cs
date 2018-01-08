using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneEditor
{
    [RequireComponent(typeof(SceneSchemeController))]
    public class EditorInterfaceController : MonoBehaviour
    {
        public RectTransform loadingScreen;
        public GameObject selectPieceMenu;
        public Dropdown saveSlotList;
        public Dropdown levelList;

        public static int editedPiece;
        
        private SceneSchemeController scene;
        private Action<int> slotLoader;
        private LevelList levels;

        private void Start()
        {   
            selectPieceMenu.SetActive(false);
            scene = GetComponent<SceneSchemeController>();
        }

        public void ShowSlots(List<LoadingController.SaveSlot> slots, Action<int> loadSlot)
        {
            slotLoader = loadSlot;
            
            saveSlotList.onValueChanged.RemoveAllListeners();
            saveSlotList.options = new List<Dropdown.OptionData>();
            foreach (LoadingController.SaveSlot slot in slots)
            {
                saveSlotList.options.Add(new Dropdown.OptionData(slot.name));
            }
            saveSlotList.captionText.text = saveSlotList.options[0].text;
            saveSlotList.onValueChanged.AddListener(delegate
            {
                slotLoader(saveSlotList.value);
            });
        }

        public void ShowLevels(LevelList list)
        {
            levels = list;
            levelList.onValueChanged.RemoveAllListeners();
            levelList.options = new List<Dropdown.OptionData>();
            foreach (LevelPiece level in list.levels)
            {
                levelList.options.Add(new Dropdown.OptionData(level.getName()));   
            }
            levelList.value = 0;
            levelList.captionText.text = levelList.options[0].text;
            levelList.onValueChanged.AddListener(delegate 
            {
                LoadSelectedLevel();
            });
            levelList.value = editedPiece;
            LoadSelectedLevel();
        }

        private void LoadSelectedLevel()
        {
            editedPiece = levelList.value;
            scene.Clear();
            scene.Load(levels.levels[levelList.value]);
        }

        public void SaveAll()
        {
            StartCoroutine(FindObjectOfType<LoadingController>().SaveLevels(levels));
        }

        public void OpenSelectPieceMenu()
        {
            LevelSettings.SetStructureOfPieces(levels);
            selectPieceMenu.SetActive(true);
        }

        public void Play()
        {
            LevelSettings.SetStructureOfPieces(levels);
            SaveAll();
            SceneManager.LoadScene("InfiniteLong");
        }

        public void ShowLoading(bool show)
        {
            loadingScreen.gameObject.SetActive(show);
        }
    }
}
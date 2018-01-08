using System.Collections.Generic;
using Effects;
using Level;
using UnityEngine;
using UnityEngine.UI;
using User;

namespace BattleInterface
{
    public class ChooseBoostDialog : MonoBehaviour
    {
        [SerializeField] private Text pointsCount;
        [SerializeField] private Transform boostListContainer;
        [SerializeField] private Button startButton;
        
        [SerializeField] private GameObject boostCellPrefab;
        
        private void Start()
        {
            LevelSettings.selectedBoosts = new List<Boost>();
        }

        private void OnEnable()
        {   
            startButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            PlayerPrefs.SetInt("UserScore", LevelSettings.GetUserProfile().GetPoints());
            Destroy(gameObject);
            FindObjectOfType<ShipBehaviour>().StartGame();
        }

        public void Init()
        {
            UpdatePoints();
            ShowBoostList();
        }

        public void UpdatePoints()
        {
            int points = LevelSettings.GetUserProfile().GetPoints();
            pointsCount.text = "You have " + 
                               (points == 0 ? "no" : points.ToString()) + 
                               " parts to spend";
        }

        private void ShowBoostList()
        {
            foreach (Boost boost in Boost.allBoosts)
            {
                BoostView boostView = Instantiate(boostCellPrefab).GetComponent<BoostView>();
                boostView.SetBoost(boost);
                boostView.transform.SetParent(boostListContainer, false);
            }
        }
    }
}
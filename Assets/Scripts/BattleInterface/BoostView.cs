using Effects;
using Level;
using UnityEngine;
using UnityEngine.UI;

namespace BattleInterface
{
    public class BoostView : MonoBehaviour
    {
        [SerializeField] private Text title;
        [SerializeField] private Text cost;
        [SerializeField] private Image icon;
        private Toggle toggle;
        private Boost _boost;

        private void Start()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(CheckPriceAndTake);
        }

        private void CheckPriceAndTake(bool isOn)
        {
            if (isOn)
            {
                if (!LevelSettings.selectedBoosts.Contains(_boost) && 
                    LevelSettings.GetUserProfile().GetPoints() - _boost.GetCost() >= 0)
                {
                    LevelSettings.GetUserProfile().ChangePointsBy(-_boost.GetCost());
                    LevelSettings.selectedBoosts.Add(_boost);
                    FindObjectOfType<ChooseBoostDialog>().UpdatePoints();
                }
                else
                {
                    toggle.isOn = false;
                }
            }
            else
            {
                if (LevelSettings.selectedBoosts.Contains(_boost))
                {
                    LevelSettings.GetUserProfile().ChangePointsBy(_boost.GetCost());
                    LevelSettings.selectedBoosts.Remove(_boost);
                    FindObjectOfType<ChooseBoostDialog>().UpdatePoints();
                }
            }
        }

        public void SetBoost(Boost boost)
        {
            _boost = boost;
            title.text = boost.GetName();
            cost.text = boost.GetCost() + "pts";
        }

        public Boost GetBoost()
        {
            return _boost;
        }
    }
}
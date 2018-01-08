using UnityEngine;

namespace BattleInterface
{
    public class BattleUI : MonoBehaviour
    {
        [SerializeField] private GameObject preloader;
        [SerializeField] private GameObject boostMenu;

        public void ShowLoading(bool show)
        {
            preloader.SetActive(show);
        }

        public void ShowStartDialog()
        {
            boostMenu.SetActive(true);
            boostMenu.GetComponent<ChooseBoostDialog>().Init();
        }
    }
}
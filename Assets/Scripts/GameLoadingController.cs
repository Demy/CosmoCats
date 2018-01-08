using BattleInterface;
using Level;
using UnityEngine;
using User;

[RequireComponent(typeof(LoadingController))]
public class GameLoadingController : MonoBehaviour
{
    private LoadingController loader;
    private BattleUI ui;

    void Start()
    {
        loader = GetComponent<LoadingController>();
        loader.CreateSlots();

        ui = FindObjectOfType<BattleUI>();

        if (LevelSettings.GetStructureOfPieces() == null)
        {
            ui.ShowLoading(true);
            LoadSlot(loader.GetCurrentSlot());   
        }
        else
        {
            ShowStartDialog();
        }
    }

    private void ShowStartDialog()
    {
        ui.ShowLoading(false);
        ui.ShowStartDialog();
    }

    public void LoadSlot(int slot)
    {
        loader.LoadSlot(slot, delegate(LevelList levelList)
        {
            LevelSettings.SetStructureOfPieces(levelList);
            LoadUserProfile();
        });
    }

    private void LoadUserProfile()
    {
        UserProfile profile = new UserProfile(0, PlayerPrefs.GetInt("UserScore"));
        LevelSettings.SetUserProfile(profile);
        ShowStartDialog();
    }
}
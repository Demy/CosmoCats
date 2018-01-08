using UnityEngine;

namespace SceneEditor
{
    public class ButtonIconChanger:MonoBehaviour
    {
        public GameObject defaultIcon;
        public GameObject changedIcon;

        public void TemporaryChange()
        {
            ShowChanged();
            Invoke("ShowDefault", 3f);
        }

        public void ShowChanged()
        {
            ShowSkins(false);
        }

        public void ShowDefault()
        {
            ShowSkins(true);
        }

        private void ShowSkins(bool normal)
        {
            defaultIcon.SetActive(normal);
            changedIcon.SetActive(!normal);
        }
    }
}
using Level;
using UnityEngine;
using UnityEngine.UI;

namespace SceneEditor
{
    public class PieceListCell : MonoBehaviour
    {
        public Toggle isSelected;
        public InputField pieceName;
        public Button delete;
        
        private LevelPiece data;

        private void Start()
        {
            isSelected.onValueChanged.AddListener(delegate {OnSelect(); });
            pieceName.onValueChanged.AddListener(delegate { OnNameChanged(); });
            delete.onClick.AddListener(DeleteThisPiece);
        }

        void OnSelect()
        {
            if (data != null)
            {
                data.SetSelected(isSelected.isOn);
            }
        }

        void OnNameChanged()
        {
            data.SetName(pieceName.text);
        }

        public void SetData(LevelPiece value)
        {
            data = value;
            UpdateView();
        }

        private void UpdateView()
        {
            isSelected.isOn = data.IsSelected();
            pieceName.text = data.getName();
        }

        public void DeleteThisPiece()
        {
            FindObjectOfType<PiecesEditor>().DeletePiece(data);
        }
    }
}
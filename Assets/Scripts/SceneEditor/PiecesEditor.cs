using System.Collections.Generic;
using Level;
using UnityEngine;
using UnityEngine.UI;

namespace SceneEditor
{
    public class PiecesEditor : MonoBehaviour
    {
        public Transform pieceViewTemplate;
        public Transform pieceList;
        public Toggle random;
        public Toggle firstIsEmpty;
        
        private List<LevelPiece> pieces;

        void Start()
        {
            random.onValueChanged.AddListener(delegate { ToggleRandom(); });
            firstIsEmpty.onValueChanged.AddListener(delegate { ToggleFirstEmpty(); });
        }

        private void ToggleRandom()
        {
            LevelSettings.isLevelRandomOn = random.isOn;
        }

        private void ToggleFirstEmpty()
        {
            LevelSettings.firstPieceIsEmpty = firstIsEmpty.isOn;
        }

        private void OnEnable()
        {
            if (LevelSettings.GetStructureOfPieces() != null)
            {
                random.isOn = LevelSettings.isLevelRandomOn;
                firstIsEmpty.isOn = LevelSettings.firstPieceIsEmpty;
                pieces = LevelSettings.GetStructureOfPieces().levels;
                UpdateView();
                if (AreAllDeselected(pieces)) SelectAll();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void UpdateView()
        {
            int cellsCount = pieceList.childCount;
            int length = pieces.Count;
            for (int i = 0; i < length; i++)
            {
                Transform pieceView;
                if (i < cellsCount)
                {
                    pieceView = pieceList.GetChild(i);
                }
                else
                {
                    pieceView = Instantiate(pieceViewTemplate, pieceList);
                }
                pieceView.GetComponent<PieceListCell>().SetData(pieces[i]);
            }
            while (pieceList.childCount > length)
            {
                Transform extraChild = pieceList.GetChild(length);
                extraChild.SetParent(null);
                Destroy(extraChild.gameObject);
            }
        }

        public void SelectAll()
        {
            SetEveryPieceSelected(pieces, !AreAllSelected(pieces));
            if (AreAllDeselected(pieces)) pieces[EditorInterfaceController.editedPiece].SetSelected(true);
            UpdateView();
        }

        private void SetEveryPieceSelected(List<LevelPiece> levelPieces, bool isSelected)
        {
            foreach (LevelPiece piece in levelPieces)
            {
                piece.SetSelected(isSelected);
            }
        }

        private bool AreAllSelected(List<LevelPiece> levelPieces)
        {
            foreach (LevelPiece piece in levelPieces)
            {
                if (!piece.IsSelected()) return false;
            }
            return true;
        }

        private bool AreAllDeselected(List<LevelPiece> levelPieces)
        {
            foreach (LevelPiece piece in levelPieces)
            {
                if (piece.IsSelected()) return false;
            }
            return true;
        }

        public void AddNew()
        {
            LevelPiece newPiece = new LevelPiece("Piece" + (pieces.Count + 1));
            pieces.Add(newPiece);
            UpdateView();
        }
        
        public void DeletePiece(LevelPiece piece)
        {
            pieces.Remove(piece);
            UpdateView();
        }
        
        public void Close()
        {
            if (AreAllDeselected(pieces)) pieces[EditorInterfaceController.editedPiece].SetSelected(true);
            gameObject.SetActive(false);
        }
    }
}
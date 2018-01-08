using System;
using System.Collections.Generic;
using Level;
using UnityEngine;
using UnityEngine.UI;

namespace SceneEditor
{
    public class SceneSchemeController : MonoBehaviour
    {
        private const int COLS = 16;
        private const int ROWS = 3;
        
        [SerializeField] private RectTransform selectedPanel;
        [SerializeField] private RectTransform grid;
        [SerializeField] private Transform itemsList;
        
        [SerializeField] private GameObject cellExample;
        [SerializeField] private GameObject rockExample;
        [SerializeField] private GameObject coinExample;
        [SerializeField] private GameObject sawExample;
        [SerializeField] private GameObject monsterExample;
        [SerializeField] private GameObject accWallExample;

        private LevelPiece data;
        private Transform currentCell;

        private void Start()
        {
            FillList();
        }

        private void FillGrid(int cols, int rows)
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    GameObject cell = Instantiate(cellExample);
                    cell.transform.SetParent(grid);
                    cell.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        SelectCell(cell.transform);
                    });
                }
            }
        }

        private void FillList()
        {
            AddItemToListByType(LevelPiece.ItemType.Rock);
            AddItemToListByType(LevelPiece.ItemType.Coin);
            AddItemToListByType(LevelPiece.ItemType.Saw);
            AddItemToListByType(LevelPiece.ItemType.Monster);
            AddItemToListByType(LevelPiece.ItemType.AccelerationWall);
        }

        private void AddItemToListByType(LevelPiece.ItemType type)
        {
            GameObject cell = Instantiate(cellExample, cellExample.transform.position, cellExample.transform.rotation);
            cell.transform.SetParent(itemsList, false);
            CreatViewForItemType(type).SetParent(cell.transform, false);
            cell.GetComponent<Button>().onClick.AddListener(delegate
            {
                AddItemToGrid(type);
            });
        }

        private void AddItemToGrid(LevelPiece.ItemType type)
        {
            int x = GetSelectedX();
            int y = GetSelectedY();
            if (data.HasItemOfTypeAt(type, x, y)) return;
            
            LevelPiece.Item newItem = new LevelPiece.Item();
            newItem.rotation = 0;
            newItem.type = type;
            newItem.x = x;
            newItem.y = y;
            data.Add(newItem);
            
            AddItemToCell(currentCell, type, newItem.rotation);
        }

        public void Clear()
        {
            for (int i = 0; i < grid.transform.childCount; i++)
            {
                RemoveChildrenFrom(grid.transform.GetChild(i));
            }
        }

        private void RemoveChildrenFrom(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            } 
        }

        public void Load(LevelPiece level)
        {
            FillGrid(COLS, ROWS);
            data = level;
            List<LevelPiece.Item> items = level.GetItems();
            int size = items.Count;
            LevelPiece.Item item;
            Button cell;
            for (int i = 0; i < size; i++)
            {
                item = items[i];
                cell = grid.GetChild(item.x + item.y * COLS).GetComponent<Button>();
                AddItemToCell(cell.transform, item.type, item.rotation);
            }
            currentCell = grid.GetChild(0);
        }

        private void AddItemToCell(Transform cell, LevelPiece.ItemType itemType, float rotation)
        {
            Transform itemView = CreatViewForItemType(itemType);
            itemView.Rotate(0, 0, rotation);
            itemView.transform.SetParent(cell, false);
        }

        private void SelectCell(Transform cell)
        {
            currentCell = cell;

            bool show = data.IsOccupied(GetSelectedX(), GetSelectedY());
            selectedPanel.gameObject.SetActive(show);
            if (show)
            {
                selectedPanel.transform.Translate(-selectedPanel.transform.position + cell.position);
                Transform selected = cell.GetChild(cell.childCount - 1);
                selectedPanel.GetComponent<ItemEditPanel>().SetItem(selected, delegate
                {
                    OnMoved(selected, GetSelectedX(), GetSelectedY());
                });   
            }
        }

        private int GetSelectedX()
        {
            int index = currentCell.GetSiblingIndex();
            return index % COLS;
        }

        private int GetSelectedY()
        {
            int index = currentCell.GetSiblingIndex();
            return (int)Math.Floor((double)index / COLS);
        }

        private void OnMoved(Transform selected, int x, int y)
        {
            LevelPiece.Item item = data.RemoveItemAt(x, y);
            
            item.x = CalculatePositionX(selected.localPosition.x, COLS);
            item.y = CalculatePositionY(selected.localPosition.y, ROWS);
            data.Add(item);

            Transform cell = GetCellAt(item.x, item.y);
            selected.SetParent(cell);
            selected.Translate(-selected.localPosition, selected.parent);
            SelectCell(cell);
        }

        private Transform GetCellAt(int x, int y)
        {
            return grid.GetChild(y * COLS + x);
        }

        private int CalculatePositionX(float localPositionX, int max)
        {
            int result = (int)Math.Floor(localPositionX / 140);
            result = Math.Max(0, result);
            result = Math.Min(max - 1, result);
            return result;
        }

        private int CalculatePositionY(float localPositionY, int max)
        {
            int result = 1 - (int)Math.Round(localPositionY / 140);
            result = Math.Max(0, result);
            result = Math.Min(max - 1, result);
            return result;
        }

        public void DeleteSelectedItem()
        {
            LevelPiece.Item item = data.RemoveItemAt(GetSelectedX(), GetSelectedY());
            selectedPanel.gameObject.SetActive(false);
        }

        public void RotateSelectedItem()
        {
            data.RotateItemAt(-90f, GetSelectedX(), GetSelectedY());
        }

        public void DuplicateSelectedItem()
        {
            LevelPiece.Item item = data.GetItemAt(GetSelectedX(), GetSelectedY());
            while (item.x++ < COLS && data.HasItemOfTypeAt(item.type, item.x, item.y)) {}
            if (item.x < COLS)
            {
                data.Add(item);
                AddItemToCell(GetCellAt(item.x, item.y), item.type, item.rotation);
            }
        }

        private Transform CreatViewForItemType(LevelPiece.ItemType itemType)
        {
            switch (itemType)
            {
                case LevelPiece.ItemType.Rock:
                    return Instantiate(rockExample).transform;
                break;
                case LevelPiece.ItemType.Coin:
                    return Instantiate(coinExample).transform;
                break;
                case LevelPiece.ItemType.Saw:
                    return Instantiate(sawExample).transform;
                break;
                case LevelPiece.ItemType.Monster:
                    return Instantiate(monsterExample).transform;
                break;
                case LevelPiece.ItemType.AccelerationWall:
                    return Instantiate(accWallExample).transform;
                break;
            }
            return Instantiate(rockExample).transform;
        }
    }
}
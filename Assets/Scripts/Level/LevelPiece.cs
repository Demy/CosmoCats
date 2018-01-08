using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [Serializable]
    public class LevelPiece
    {
        [Serializable]
        public enum ItemType
        {
            Coin, Rock, Monster, AccelerationWall, Accelerator, Saw
        }
        [Serializable]
        public struct Item
        {
            public int x;
            public int y;
            public float rotation;
            public ItemType type;
        }
        
        public static Item noItem = new Item() {x = -1, y = -1, type = ItemType.Rock};
        [SerializeField]
        private string _name;
        [SerializeField]
        private List<Item> items;
        [SerializeField] 
        private bool _isSelected;

        public LevelPiece(string name)
        {
            _name = name;
            items = new List<Item>();
            _isSelected = true;
        }

        public void Add(Item item)
        {
            items.Add(item);
        }

        public Item RemoveItemAt(int col, int row)
        {
            int index = GetItemIndex(col, row);
            Item item = items[index];
            if (index >= 0) items.RemoveAt(index);
            return item;
        }

        public Boolean IsOccupied(int col, int row)
        {
            return GetItemIndex(col, row) >= 0;
        }

        public Item GetItemAt(int col, int row)
        {
            int index = GetItemIndex(col, row);
            if (index >= 0) return items[index];
            return noItem;
        }

        public int GetItemIndex(int col, int row)
        {
            Item cell;
            int size = items.Count;
            for (int i = 0; i < size; i++)
            {
                cell = items[i];
                if (cell.x == col && cell.y == row) return i;
            }
            return -1;
        }
        
        public String getName()
        {
            return _name;
        }

        public List<Item> GetItems()
        {
            return items;
        }

        public bool HasItemOfTypeAt(ItemType type, int x, int y)
        {
            Item cell;
            int size = items.Count;
            for (int i = 0; i < size; i++)
            {
                cell = items[i];
                if (cell.x == x && cell.y == y && cell.type == type) return true;
            }
            return false;
        }

        public void RotateItemAt(float angleZ, int x, int y)
        {
            Item item = RemoveItemAt(x, y);
            item.rotation += angleZ;
            Add(item);
        }

        public bool IsSelected()
        {
            return _isSelected;
        }

        public void SetSelected(bool value)
        {
            _isSelected = value;
        }

        public void SetName(string value)
        {
            _name = value;
        }
    }
}
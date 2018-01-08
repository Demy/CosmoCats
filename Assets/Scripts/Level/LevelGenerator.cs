using System;
using System.Collections.Generic;
using Effects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Level
{
    public class LevelGenerator : MonoBehaviour
    {
        public float pieceLength = 168f;
        public float cellSize = 4.2f;
        public GameObject levelPieceBack;
        public GameObject rockSource;
        public GameObject coinSource;
        public GameObject accWallSource;
        public GameObject sawSource;
        public GameObject monsterSource;
        private List<LevelPiece> pieces;
        public int lastPieceIndex = -1;
        public int piecesAhead = 1;

        [SerializeField]
        private Transform ship;
        private ItemsCollector bonusRegisterer;
        private float lastPosition;

        private Vector3 piecePosition;
        
        private System.Random random;

        private void Start()
        {
            random = CreateSeed();

            bonusRegisterer = ship.GetComponent<ItemsCollector>();

            LevelList structure = LevelSettings.GetStructureOfPieces();
            if (structure == null)
            {
                pieces = new List<LevelPiece>();
                pieces.Add(new LevelPiece("EmptyPiece"));
            }
            else
            {
                pieces = RemoveNotSelected(LevelSettings.GetStructureOfPieces().levels);   
            }
            
            piecePosition = new Vector3();
            lastPosition = GenerateChunk(ship.position.x, 0);
        }

        private List<LevelPiece> RemoveNotSelected(List<LevelPiece> levelPieces)
        {
            List<LevelPiece> result = new List<LevelPiece>();
            foreach (LevelPiece levelPiece in levelPieces)
            {
                if (levelPiece.IsSelected())
                {
                    result.Add(levelPiece);
                }
            }
            return result;
        }

        private GameObject CreatePieceWithItems(List<LevelPiece.Item> items)
        {
            GameObject piece = Instantiate(levelPieceBack);
            Transform itemsPlace = piece.transform.Find("Items");
            int size = items.Count;
            for (int j = 0; j < size; j++)
            {
                GameObject item = Instantiate(GetItemSource(items[j].type), itemsPlace) as GameObject;
                item.transform.localPosition = new Vector3(items[j].x * cellSize, -items[j].y * cellSize, 0);
                item.transform.Rotate(0, 0, items[j].rotation);
                if (item.CompareTag(Collectable.BONUS_TAG)) bonusRegisterer.AddItem(item);
            }
            return piece;
        }

        private Object GetItemSource(LevelPiece.ItemType type)
        {
            GameObject result;
            switch (type)
            {
                case LevelPiece.ItemType.Rock:
                    result = rockSource;   
                break;
                case LevelPiece.ItemType.Coin:
                    result = coinSource;   
                break;
                case LevelPiece.ItemType.AccelerationWall:
                    result = accWallSource;   
                break;
                case LevelPiece.ItemType.Monster:
                    result = monsterSource;   
                break;
                case LevelPiece.ItemType.Saw:
                    result = sawSource;   
                break;
                default:
                    result = new GameObject();
                break;
            }
            return result;
        }

        private System.Random CreateSeed()
        {
            DateTime date = DateTime.Today;
            int seed = date.Month * 100000 + date.Year * 10 + (int)Math.Floor((double) date.Day / 7);
            return new System.Random(seed);
        }

        private float GenerateChunk(float currentPosition, float position)
        {
            if (LevelSettings.firstPieceIsEmpty && position <= 0f)
            {
                CreateFirstPiece();
                position = pieceLength;
            }
            int pieceCount = piecesAhead + (int)Math.Ceiling((currentPosition - position) / pieceLength);
            for (int i = 0; i < pieceCount; i++)
            {
                GameObject newPiece = CreatePieceWithItems(
                    pieces[GetNextIndex(pieces.Count)].GetItems());
                SetNewPieceAt(newPiece, position + i * pieceLength);
            }
            if (pieceCount > 0) ClearPassed(ship.position.x);
            return position + pieceCount * pieceLength;
        }

        private void CreateFirstPiece()
        {
            SetNewPieceAt(Instantiate(levelPieceBack), 0);
        }

        private void SetNewPieceAt(GameObject piece, float position)
        {
            piece.transform.SetParent(transform);
            piecePosition.Set(position, 0f, 0f);
            piece.transform.Translate(piecePosition);
        }

        private int GetNextIndex(int piecesLength)
        {
            if (LevelSettings.isLevelRandomOn)
            {
                return random.Next(piecesLength);
            }
            if (++lastPieceIndex >= piecesLength)
            {
                lastPieceIndex = 0;
            }
            return lastPieceIndex;
        }

        private void Update()
        {
            lastPosition = GenerateChunk(ship.position.x, lastPosition);
        }

        private void ClearPassed(float positionX)
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.position.x < positionX - pieceLength) 
                    Destroy(child.gameObject);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    public class ItemsCollector : MonoBehaviour
    {
        public const float SMALL_RADIUS = 2f;
        public const float MID_RADIUS = 5f;
        public const float BIG_RADIUS = 7f;
        
        private ShipBehaviour behaviour;
        private List<GameObject> items;
        private float radius = SMALL_RADIUS;
        private float sqrRadius;

        void Start()
        {
            sqrRadius = radius * radius;
            items = new List<GameObject>();
            behaviour = GetComponent<ShipBehaviour>();
        }

        void FixedUpdate()
        {
            CollectInRadius();
        }

        private void CollectInRadius()
        {
            int size = items.Count;
            GameObject item;
            for (int i = 0; i < size; i++)
            {
                item = items[i];
                if (item == null)
                {
                    items.RemoveAt(i--);
                    --size;
                    continue;
                }
                float xDif = (item.transform.position.x - behaviour.transform.position.x);
                float yDif = (item.transform.position.y - behaviour.transform.position.y);
                if (xDif * xDif + yDif * yDif <= sqrRadius)
                {
                    Attract(item);
                    items.RemoveAt(i--);
                    --size;
                }
            }
        }

        private void Attract(GameObject item)
        {
            item.AddComponent<FlyToBeCollected>().SetTarget(behaviour);
        }

        public void AddItem(GameObject item)
        {
            items.Add(item);
        }

        public void SetRadius(float value)
        {
            radius = value;
            sqrRadius = radius * radius;
        }
    }
}
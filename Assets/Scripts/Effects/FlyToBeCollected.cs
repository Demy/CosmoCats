using UnityEngine;

namespace Effects
{
    public class FlyToBeCollected : MonoBehaviour
    {
        private ShipBehaviour collector;
        private Vector3 attractionOffset = new Vector3(-3f, 0, 0);
        private float attractionSpeed = 0.85f;
        private float collectionRadius = 0.6f;
        private float sqrCollectionRadius;

        void Start()
        {
            sqrCollectionRadius = collectionRadius * collectionRadius;
        }

        public void SetTarget(ShipBehaviour collector)
        {
            this.collector = collector;
            transform.SetParent(collector.transform.parent);
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            Animator anim = GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Collect");
        }

        void FixedUpdate()
        {
            if (collector == null) return;
            
            Vector3 dist = collector.transform.position + attractionOffset - transform.position;
            transform.Translate(dist.sqrMagnitude < attractionSpeed * attractionSpeed ? 
                dist : dist.normalized * attractionSpeed);
            
            dist = collector.transform.position + attractionOffset - transform.position;
            if (dist.sqrMagnitude <= sqrCollectionRadius)
            {
                collector.Collect(gameObject);
            }
        }
    }
}
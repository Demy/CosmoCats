using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SceneEditor
{
    public class ItemEditPanel:MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {   
        private Transform selectedView;
        private UnityAction onItemMoved;
        
        public void SetItem(Transform itemView, UnityAction onItemMoved)
        {
            selectedView = itemView;
            this.onItemMoved = onItemMoved;
        }

        public void OnBeginDrag(PointerEventData data)
        {
            selectedView.SetParent(selectedView.parent.parent.parent);
        }

        public void OnDrag(PointerEventData data)
        {
            selectedView.Translate(data.delta, selectedView.parent);
            transform.Translate(data.delta);
        }

        public void OnEndDrag(PointerEventData data)
        {
            onItemMoved.Invoke();
        }

        public void Delete()
        {
            Destroy(selectedView.gameObject);
        }

        public void Rotate()
        {
            selectedView.Rotate(0, 0, -90f);
        }
    }
}
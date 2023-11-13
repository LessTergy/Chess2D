using UnityEngine;
using UnityEngine.EventSystems;

namespace LessTergy.UI
{
    public class InteractiveObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public delegate void EventDataEvent(PointerEventData e);

        // pointer
        public event EventDataEvent onPointerDown;
        public event EventDataEvent onPointerUp;
        
        // drag
        public event EventDataEvent onBeginDrag;
        public event EventDataEvent onDrag;
        public event EventDataEvent onEndDrag;
        

        public bool interactable = true;
    
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }
            onPointerDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }
            onPointerUp?.Invoke(eventData);
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }
            onBeginDrag?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }
            onDrag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }
            onEndDrag?.Invoke(eventData);
        }
    }
}
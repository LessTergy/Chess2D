using Lesstergy.Time;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lesstergy.UI
{
    public class InteractiveObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public delegate void InteractiveDelegate(PointerEventData eventData);

        public event InteractiveDelegate OnTouchDown = delegate { };
        public event InteractiveDelegate OnTouchUp = delegate { };
        public event InteractiveDelegate OnDoubleClick = delegate { };

        public event InteractiveDelegate OnMoveStart = delegate { };
        public event InteractiveDelegate OnMove = delegate { };
        public event InteractiveDelegate OnMoveEnd = delegate { };
        
        private readonly TimeSince clickTime = new TimeSince();
        private float _doubleClickDelay = 0.25f;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnTouchDown(eventData);

            if (clickTime.delta < _doubleClickDelay)
            {
                OnDoubleClick(eventData);
                clickTime.Reset();
            }
            else
            {
                clickTime.Fixate();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnTouchUp(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnMoveStart(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnMove(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnMoveEnd(eventData);
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Lesstergy.Time;

namespace Lesstergy.UI {

    [RequireComponent(typeof(EventTrigger))]
    public class InteractiveObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        public event Action<InteractiveEventArgs> OnTouchDown = delegate { };
        public event Action<InteractiveEventArgs> OnTouchUp = delegate { };
        public event Action<InteractiveEventArgs> OnDoubleClick = delegate { };

        public event Action<InteractiveEventArgs> OnMoveStart = delegate { };
        public event Action<InteractiveEventArgs> OnMove = delegate { };
        public event Action<InteractiveEventArgs> OnMoveEnd = delegate { };
        public event Action<InteractiveEventArgs> OnHoldTouch = delegate { };

        public bool IsTouchOn { get; private set; }

        public readonly TimeSince holdTime = new TimeSince();
        private readonly TimeSince clickTime = new TimeSince();
        private float doubleClickDelay = 0.25f;

        protected virtual void Awake() {
            EventTrigger trigger = GetComponent<EventTrigger>();

            EventTrigger.Entry entry1 = new EventTrigger.Entry();
            entry1.eventID = EventTriggerType.PointerDown;
            entry1.callback.AddListener((data) => { TouchDownHandler((PointerEventData)data); });
            trigger.triggers.Add(entry1);

            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerUp;
            entry2.callback.AddListener((data) => { TouchUpHandler((PointerEventData)data); });
            trigger.triggers.Add(entry2);
        }

        private void Update() {
            UpdateHold();
        }

        private void UpdateHold() {
            if (IsTouchOn) {
                OnHoldTouch(CreateArgs(null));
            }
        }

        private void TouchDownHandler(PointerEventData data) {
            IsTouchOn = true;
            holdTime.Fixate();

            OnTouchDown(CreateArgs(data));

            if (clickTime.delta < doubleClickDelay) {
                OnDoubleClick(CreateArgs(data));
                clickTime.Reset();
            } else {
                clickTime.Fixate();
            }
        }

        private void TouchUpHandler(PointerEventData data) {
            IsTouchOn = false;
            holdTime.Reset();

            OnTouchUp(CreateArgs(data));
        }

        public void OnBeginDrag(PointerEventData data) {
            OnMoveStart(CreateArgs(data));
        }

        public void OnDrag(PointerEventData data) {
            OnMove(CreateArgs(data));
        }

        public void OnEndDrag(PointerEventData data) {
            OnMoveEnd(CreateArgs(data));
        }

        private InteractiveEventArgs CreateArgs(PointerEventData data) {
            return new InteractiveEventArgs(gameObject, this, data);
        }
    }

    public class InteractiveEventArgs {
        public readonly GameObject sender;
        public readonly InteractiveObject interactive;
        public readonly PointerEventData data;

        public InteractiveEventArgs(GameObject go, InteractiveObject io, PointerEventData ped) {
            sender = go;
            data = ped;

            this.interactive = io;
        }
    }

}

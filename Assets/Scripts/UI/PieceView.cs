using Chess2D.Model;
using LessTergy.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Chess2D.UI
{
    public class PieceView : MonoBehaviour
    {
        public event PieceViewDelegate OnPointerDown;
        public event PieceViewDelegate OnPointerUp;
        public event PieceViewDelegate OnDrag;

        [Header("Components")]
        [SerializeField] private Image _image;
        [SerializeField] private InteractiveObject _interactiveObject;

        public PieceState Piece { get; private set; }

        public bool IsActive
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public bool IsInteractive
        {
            get => _image.raycastTarget;
            set => _image.raycastTarget = value;
        }

        private void Awake()
        {
            _interactiveObject.onPointerDown += ForwardPointerDown;
            _interactiveObject.onPointerUp += ForwardPointerUp;
            _interactiveObject.onDrag += ForwardDrag;
        }

        public void Construct(PieceState piece)
        {
            Piece = piece;
        }

        public void SetSprite(Sprite sprite, Color color)
        {
            _image.sprite = sprite;
            _image.color = color;
        }

        private void ForwardPointerDown(PointerEventData eventData) => OnPointerDown?.Invoke(this, eventData);
        private void ForwardPointerUp(PointerEventData eventData) => OnPointerUp?.Invoke(this, eventData);
        private void ForwardDrag(PointerEventData eventData) => OnDrag?.Invoke(this, eventData);
    }
}

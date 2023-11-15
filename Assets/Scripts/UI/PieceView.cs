using System.Collections.Generic;
using Chess2D.Model;
using Chess2D.PieceMovement;
using LessTergy.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Chess2D.UI
{
    public class PieceView : MonoBehaviour
    {
        public delegate void PieceViewMoveDelegate(PieceView pieceView, PointerEventData eventData);

        public event PieceViewMoveDelegate OnPointerDown;
        public event PieceViewDelegate OnPointerUp;
        public event PieceViewMoveDelegate OnDrag;
        
        [Header("Components")] 
        [SerializeField] private Image _image;
        [SerializeField] private InteractiveObject _interactiveObject;
        
        [Header("Properties")]
        public Vector2Int cellCoord;
        public bool isTarget;
        public bool isWasMoving;
        public bool isLastMoving;
        
        // getters
        public PieceType Type { get; private set; }
        public PlayerType PlayerType { get; private set; }

        public List<PieceMoveAlgorithm> MoveAlgorithms { get; private set; }

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
            _interactiveObject.onPointerDown += InteractiveObject_onPointerDown;
            _interactiveObject.onPointerUp += InteractiveObject_onPointerUp;
            _interactiveObject.onDrag += InteractiveObject_onDrag;
        }
        
        public void Construct(PieceType type, PlayerType playerType, List<PieceMoveAlgorithm> moveAlgorithms)
        {
            Type = type;
            MoveAlgorithms = moveAlgorithms;
            PlayerType = playerType;
        }

        public void SetSprite(Sprite sprite, Color color)
        {
            _image.sprite = sprite;
            _image.color = color;
        }
        
        private void InteractiveObject_onPointerDown(PointerEventData e)
        {
            OnPointerDown?.Invoke(this, e);
        }
        
        private void InteractiveObject_onPointerUp(PointerEventData e)
        {
            OnPointerUp?.Invoke(this);
        }
        
        private void InteractiveObject_onDrag(PointerEventData e)
        {
            OnDrag?.Invoke(this, e);
        }
    }
}
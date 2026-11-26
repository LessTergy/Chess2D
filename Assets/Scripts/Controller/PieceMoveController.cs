using System;
using System.Collections.Generic;
using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chess2D.Controller
{
    public class PieceMoveController
    {
        public event Action OnMoveAccepted;

        private readonly BoardController _board;
        private readonly GameModel _game;
        private readonly List<CellView> _destinations = new();
        private Camera _camera;
        private CellView _targetCell;
        private PieceView _draggedPiece;
        private int _pointerId;
        private Vector2 _startMovePosition;

        public PieceMoveController(BoardController board, GameModel game)
        {
            _board = board;
            _game = game;
        }

        public void Initialize()
        {
            _camera = Camera.main;
        }

        public void BindPiece(PieceView view)
        {
            view.OnPointerDown += OnPointerDown;
            view.OnDrag += OnDrag;
            view.OnPointerUp += OnPointerUp;
        }

        public void UnbindPiece(PieceView view)
        {
            view.OnPointerDown -= OnPointerDown;
            view.OnDrag -= OnDrag;
            view.OnPointerUp -= OnPointerUp;
        }

        private void OnPointerDown(PieceView view, PointerEventData eventData)
        {
            CancelDrag();
            _draggedPiece = view;
            _pointerId = eventData.pointerId;
            _startMovePosition = GetLocalPosition(eventData);
            view.transform.SetAsLastSibling();

            foreach (var square in _game.GetCandidateDestinations(view.Piece.Square))
            {
                var cell = _board.GetCell(square);
                cell.Image.enabled = true;
                _destinations.Add(cell);
            }
        }

        private void OnDrag(PieceView view, PointerEventData eventData)
        {
            if (_draggedPiece != view || _pointerId != eventData.pointerId || _destinations.Count == 0)
            {
                return;
            }

            var delta = GetLocalPosition(eventData) - _startMovePosition;
            var startPosition = _board.GetCell(view.Piece.Square).transform.localPosition;
            view.transform.localPosition = startPosition + (Vector3)delta;

            _targetCell = null;
            foreach (var cell in _destinations)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(cell.RectT, eventData.position, _camera))
                {
                    _targetCell = cell;
                    break;
                }
            }
        }

        private void OnPointerUp(PieceView view, PointerEventData eventData)
        {
            if (_draggedPiece != view || _pointerId != eventData.pointerId)
            {
                return;
            }

            var target = _targetCell;
            ClearDrag();
            if (target != null && _game.TryMove(view.Piece.Square, target.Coord))
            {
                OnMoveAccepted?.Invoke();
            }
            else
            {
                _board.SnapToSquare(view);
            }
        }

        public void CancelDrag()
        {
            var view = _draggedPiece;
            ClearDrag();
            if (view != null)
            {
                _board.SnapToSquare(view);
            }
        }

        private void ClearDrag()
        {
            _draggedPiece = null;
            _targetCell = null;
            foreach (var cell in _destinations)
            {
                cell.Image.enabled = false;
            }

            _destinations.Clear();
        }

        private Vector2 GetLocalPosition(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_board.BoardRect, eventData.position, _camera, out var position);
            return position;
        }
    }
}

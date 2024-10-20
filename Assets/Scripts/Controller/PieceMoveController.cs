﻿using Chess2D.Commands;
using System;
using System.Collections.Generic;
using Chess2D.PieceMovement;
using Chess2D.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chess2D.Controller
{
    public class PieceMoveController
    {
        public event Action<PieceView> OnMakeMove = delegate { };
        public event Action<PieceView> OnFinishMove = delegate { };

        // fields
        private Camera _camera;
        private List<MoveData> _availableMoves = new();
        private CellView _targetCell;

        private PieceView _lastMovePiece;
        private ICommand _lastMoveCommand;
        private Vector2 _startMovePosition;
        
        // getters
        private bool HasMoves => _availableMoves.Count > 0;

        // Inject
        private readonly BoardController _boardController;

        public PieceMoveController(BoardController boardController)
        {
            _boardController = boardController;
        }

        public void Initialize()
        {
            _camera = Camera.main;
        }

        public void OnPieceCreated(PieceView pieceView)
        {
            pieceView.OnPointerDown += PieceView_OnPointerDown;
            pieceView.OnPointerUp += PieceView_OnTouchUp;
            pieceView.OnDrag += Piece_OnMove;
        }

        // Start moving
        private void PieceView_OnPointerDown(PieceView pieceView, PointerEventData eventData)
        {
            pieceView.transform.SetAsLastSibling();
            _startMovePosition = GetLocalPosition(eventData);

            UpdateAvailableMoves(pieceView);
            SetHighlightCells(true);
        }

        private void UpdateAvailableMoves(PieceView pieceView)
        {
            _availableMoves = new List<MoveData>();

            foreach (PieceMoveAlgorithm moveAlgorithm in pieceView.MoveAlgorithms)
            {
                _availableMoves.AddRange(moveAlgorithm.GetAvailableMoves(pieceView, _boardController));
            }
        }

        private void Piece_OnMove(PieceView pieceView, PointerEventData eventData)
        {
            if (!HasMoves)
            {
                return;
            }

            Vector2 currentPosition = GetLocalPosition(eventData);
            Vector3 delta = currentPosition - _startMovePosition;
            Vector3 startPosition = _boardController.GetCell(pieceView.cellCoord).transform.localPosition;

            pieceView.transform.localPosition = startPosition + delta;
            _targetCell = GetTargetCell(eventData.position);
        }

        private Vector2 GetLocalPosition(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_boardController.BoardRect,
                eventData.position, _camera, out Vector2 localPoint);
            return localPoint;
        }

        private CellView GetTargetCell(Vector2 screenPosition)
        {
            foreach (MoveData moveInfo in _availableMoves)
            {
                bool pieceOnCell = RectTransformUtility.RectangleContainsScreenPoint(moveInfo.cellView.RectT, screenPosition, _camera);
                if (!pieceOnCell)
                {
                    continue;
                }

                return moveInfo.cellView;
            }

            return null;
        }

        // Move Logic
        private void PieceView_OnTouchUp(PieceView pieceView)
        {
            _lastMovePiece = pieceView;
            SetHighlightCells(false);

            if (_targetCell == null)
            {
                // replace piece at own start position
                _boardController.PlacePieceOnCell(pieceView, pieceView.cellCoord);
            }
            else
            {
                MakeMove();
            }
        }

        private void MakeMove()
        {
            foreach (MoveData moveInfo in _availableMoves)
            {
                if (!moveInfo.cellView.Equals(_targetCell))
                {
                    continue;
                }

                _lastMoveCommand = moveInfo.moveCommand;
                moveInfo.moveCommand.Execute();
                OnMakeMove(_lastMovePiece);
                break;
            }
        }
        
        public void ApplyMove()
        {
            _lastMovePiece.isWasMoving = true;
            _lastMovePiece.isLastMoving = true;

            OnFinishMove(_lastMovePiece);
        }

        public void CancelMove()
        {
            _lastMoveCommand.Undo();

            _lastMoveCommand = null;
            _lastMovePiece = null;
        }

        private void SetHighlightCells(bool flag)
        {
            foreach (MoveData move in _availableMoves)
            {
                move.cellView.Image.enabled = flag;
            }
        }
    }
}
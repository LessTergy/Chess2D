using Chess2D.Commands;
using Chess2D.Model.PieceMove;
using Lesstergy.Chess2D;
using System;
using System.Collections.Generic;
using Chess2D.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chess2D.Controller
{

    public class PieceMoveController : MonoBehaviour, IController
    {

        public event Action<Piece> OnMakeMove = delegate { };
        public event Action<Piece> OnFinishMove = delegate { };

        private List<MoveInfo> _availableMoves = new List<MoveInfo>();
        private bool IsHaveMoves => _availableMoves.Count > 0;
        private Cell _targetCell;

        private Camera _camera;
        private Piece _lastMovePiece;
        private ICommand _lastMoveAction;
        private Vector2 _startMovePosition;

        // Inject
        private BoardController _boardController;
        private Board _board;
        private PieceController _pieceController;

        public void Construct(BoardController bc, PieceController pc)
        {
            _boardController = bc;
            _board = _boardController.GetBoard();
            _pieceController = pc;
        }

        public void Initialize()
        {
            _camera = Camera.main;
            _pieceController.OnPieceCreated += PieceController_OnPieceCreated;
        }

        public void PieceController_OnPieceCreated(Piece piece)
        {
            piece.interactive.OnTouchDown += delegate (PointerEventData eventData) { Piece_OnTouchDown(eventData, piece); };
            piece.interactive.OnTouchUp += delegate { Piece_OnTouchUp(piece); };
            piece.interactive.OnMove += delegate (PointerEventData eventData) { Piece_OnMove(eventData, piece); };
        }

        // Start moving
        private void Piece_OnTouchDown(PointerEventData eventData, Piece piece)
        {
            piece.transform.SetAsLastSibling();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_board.rectT, eventData.position, _camera, out _startMovePosition);

            GetAvailableMoves(piece);
            SetHighlightCells(true);
        }

        private void GetAvailableMoves(Piece piece)
        {
            _availableMoves = new List<MoveInfo>();

            foreach (PieceMoveAlgorithm move in piece.moves)
            {
                _availableMoves.AddRange(move.GetAvailableMoves(piece, _boardController));
            }
        }

        //Move
        private void Piece_OnMove(PointerEventData eventData, Piece piece)
        {
            if (!IsHaveMoves)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_board.rectT, eventData.position, _camera, out Vector2 currentPosition);
            Vector3 delta = currentPosition - _startMovePosition;
            Vector3 startPosition = _boardController.GetCell(piece.cellCoord).transform.localPosition;

            piece.transform.localPosition = startPosition + delta;
            _targetCell = GetTargetCell(eventData.position);
        }

        private Cell GetTargetCell(Vector2 screenPosition)
        {
            foreach (MoveInfo moveInfo in _availableMoves)
            {
                bool pieceOnCell = RectTransformUtility.RectangleContainsScreenPoint(moveInfo.cell.rectT, screenPosition, _camera);
                if (!pieceOnCell)
                {
                    continue;
                }

                return moveInfo.cell;
            }

            return null;
        }

        // Finish move
        private void Piece_OnTouchUp(Piece piece)
        {
            _lastMovePiece = piece;
            SetHighlightCells(false);

            if (_targetCell == null)
            {
                // replace piece at own start position
                _boardController.ReplacePiece(piece, piece.cellCoord);
            }
            else
            {
                MakeMove();
            }
        }

        private void MakeMove()
        {
            foreach (MoveInfo moveInfo in _availableMoves)
            {
                if (!moveInfo.cell.Equals(_targetCell))
                {
                    continue;
                }

                _lastMoveAction = moveInfo.moveAction;
                moveInfo.moveAction.Execute();
                OnMakeMove(_lastMovePiece);
                break;
            }
        }

        // Help logic
        public void ApplyMove()
        {
            _lastMovePiece.isWasMoving = true;
            _lastMovePiece.isLastMoving = true;

            OnFinishMove(_lastMovePiece);
        }

        public void CancelMove()
        {
            _lastMoveAction.Undo();

            _lastMoveAction = null;
            _lastMovePiece = null;
        }

        private void SetHighlightCells(bool flag)
        {
            foreach (MoveInfo move in _availableMoves)
            {
                move.cell.image.enabled = flag;
            }
        }
    }

}
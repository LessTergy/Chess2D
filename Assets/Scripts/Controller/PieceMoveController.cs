using Chess2D.Commands;
using Chess2D.Model.PieceMove;
using System;
using System.Collections.Generic;
using Chess2D.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chess2D.Controller
{
    public class PieceMoveController : MonoBehaviour
    {
        public event Action<PieceView> OnMakeMove = delegate { };
        public event Action<PieceView> OnFinishMove = delegate { };

        private List<MoveInfo> _availableMoves = new();
        private bool IsHaveMoves => _availableMoves.Count > 0;
        private CellView _targetCell;

        private Camera _camera;
        private PieceView _lastMovePiece;
        private ICommand _lastMoveAction;
        private Vector2 _startMovePosition;

        // Inject
        private BoardController _boardController;
        private BoardView _boardView;
        private PieceController _pieceController;

        public void Construct(BoardController boardController, PieceController pieceController)
        {
            _boardController = boardController;
            _boardView = _boardController.BoardView;
            _pieceController = pieceController;
        }

        public void Initialize()
        {
            _camera = Camera.main;
            _pieceController.OnPieceCreated += PieceController_OnPieceCreated;
        }

        public void PieceController_OnPieceCreated(PieceView pieceView)
        {
            pieceView.OnPointerDown += PieceView_OnPointerDown;
            pieceView.OnPointerUp += PieceView_OnTouchUp;
            pieceView.OnDrag += Piece_OnMove;
        }

        // Start moving
        private void PieceView_OnPointerDown(PieceView pieceView, PointerEventData eventData)
        {
            pieceView.transform.SetAsLastSibling();
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_boardView.RectTransform,
                eventData.position, _camera, out _startMovePosition);

            GetAvailableMoves(pieceView);
            SetHighlightCells(true);
        }

        private void GetAvailableMoves(PieceView piece)
        {
            _availableMoves = new List<MoveInfo>();

            foreach (PieceMoveAlgorithm move in piece.Moves)
            {
                _availableMoves.AddRange(move.GetAvailableMoves(piece, _boardController));
            }
        }

        private void Piece_OnMove(PieceView pieceView, PointerEventData eventData)
        {
            if (!IsHaveMoves)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_boardView.RectTransform, 
                eventData.position, _camera, out Vector2 currentPosition);
            
            Vector3 delta = currentPosition - _startMovePosition;
            Vector3 startPosition = _boardController.GetCell(pieceView.cellCoord).transform.localPosition;

            pieceView.transform.localPosition = startPosition + delta;
            _targetCell = GetTargetCell(eventData.position);
        }

        private CellView GetTargetCell(Vector2 screenPosition)
        {
            foreach (MoveInfo moveInfo in _availableMoves)
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

        // Finish move
        private void PieceView_OnTouchUp(PieceView pieceView)
        {
            _lastMovePiece = pieceView;
            SetHighlightCells(false);

            if (_targetCell == null)
            {
                // replace piece at own start position
                _boardController.ReplacePiece(pieceView, pieceView.cellCoord);
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
                if (!moveInfo.cellView.Equals(_targetCell))
                {
                    continue;
                }

                _lastMoveAction = moveInfo.moveCommand;
                moveInfo.moveCommand.Execute();
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
                move.cellView.Image.enabled = flag;
            }
        }
    }

}
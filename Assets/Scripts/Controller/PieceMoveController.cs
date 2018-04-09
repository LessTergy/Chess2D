using Lesstergy.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PieceMoveController : MonoBehaviour, IController {

        public event Action<Piece> OnMakeMove = delegate { };
        public event Action<Piece> OnFinishMove = delegate { };

        private List<MoveInfo> availableMoves = new List<MoveInfo>();
        private bool isHaveMoves {
            get { return availableMoves.Count > 0; }
        }
        private Cell targetCell;

        private Piece lastMovePiece;
        private ICommand lastMoveAction;

        private BoardController boardController;
        private PieceController pieceController;

        public void Inject(BoardController bc, PieceController pc) {
            this.boardController = bc;
            this.pieceController = pc;
        }

        public void Initialize() {
            pieceController.OnPieceCreated += PieceController_OnPieceCreated;
        }

        public void PieceController_OnPieceCreated(Piece piece) {
            piece.interactive.OnTouchDown += delegate { Piece_OnTouchDown(piece); };
            piece.interactive.OnTouchUp += delegate { Piece_OnTouchUp(piece); };
            piece.interactive.OnMove += delegate (InteractiveEventArgs args) { Piece_OnMove(args, piece); };
        }

        //Start moving
        private void Piece_OnTouchDown(Piece piece) {
            piece.transform.SetAsLastSibling();

            GetAvailableMoves(piece);
            SetHighlightCells(true);
        }

        private void GetAvailableMoves(Piece piece) {
            availableMoves = new List<MoveInfo>();

            foreach (PieceMoveAlgorithm move in piece.moves) {
                availableMoves.AddRange(move.GetAvailableMoves(piece, boardController));
            }
        }

        //Move
        private void Piece_OnMove(InteractiveEventArgs eventArgs, Piece piece) {
            if (isHaveMoves) {
                eventArgs.sender.transform.position += (Vector3)eventArgs.data.delta;
                UpdateTargetCell();
            }
        }

        private void UpdateTargetCell() {
            targetCell = null;

            foreach (MoveInfo moveInfo in availableMoves) {
                if (RectTransformUtility.RectangleContainsScreenPoint(moveInfo.cell.rectT, Input.mousePosition)) {
                    targetCell = moveInfo.cell;
                    break;
                }
            }
        }

        //Finish move
        private void Piece_OnTouchUp(Piece piece) {
            lastMovePiece = piece;
            SetHighlightCells(false);

            if (targetCell == null) {
                //replace piece at own start position
                boardController.ReplacePiece(piece, piece.cellCoord);
            } else {
                MakeMove();
            }
        }

        private void MakeMove() {
            foreach (MoveInfo moveInfo in availableMoves) {
                if (moveInfo.cell.Equals(targetCell)) {
                    lastMoveAction = moveInfo.moveAction;

                    moveInfo.moveAction.Execute();
                    OnMakeMove(lastMovePiece);
                    break;
                }
            }
        }

        //Help logic
        public void ApplyMove() {
            lastMovePiece.isWasMoving = true;
            lastMovePiece.isLastMoving = true;

            OnFinishMove(lastMovePiece);
        }

        public void CancelMove() {
            lastMoveAction.Undo();

            lastMoveAction = null;
            lastMovePiece = null;
        }

        private void SetHighlightCells(bool flag) {
            foreach (var move in availableMoves) {
                move.cell.image.enabled = flag;
            }
        }
    }

}

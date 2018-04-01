using System.Collections.Generic;
using UnityEngine;
using Lesstergy.UI;

namespace Lesstergy.Chess2D {

    public class PieceMoveController : MonoBehaviour, IController {

        private List<MoveInfo> availableMoves;
        private Cell targetCell;

        private BoardController boardController;

        public void Inject(BoardController bc) {
            this.boardController = bc;
        }

        public void Initialize() {
        }

        public void InitPiece(Piece piece) {
            piece.interactive.OnTouchDown += delegate { Piece_OnTouchDown(piece); };
            piece.interactive.OnTouchUp += delegate { Piece_OnTouchDown(piece); };
            piece.interactive.OnMove += delegate (InteractiveEventArgs args) { Piece_OnMove(args, piece); };
        }

        //Start moving
        private void Piece_OnTouchDown(Piece piece) {
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
            eventArgs.sender.transform.position += (Vector3)eventArgs.data.delta;

            foreach (MoveInfo moveInfo in availableMoves) {
                if (RectTransformUtility.RectangleContainsScreenPoint(moveInfo.cell.rectT, Input.mousePosition)) {
                    targetCell = moveInfo.cell;
                    break;
                }
            }
        }

        //Finish move
        private void Piece_OnTouchUp(Piece piece) {
            targetCell = null;
        }
        
        //Other
        private void SetHighlightCells(bool flag) {
            foreach (var move in availableMoves) {
                move.cell.enabled = flag;
            }
        }
    }

}

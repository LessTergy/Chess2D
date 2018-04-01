using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lesstergy.UI;
using System;

namespace Lesstergy.Chess2D {

    public class PieceMoveController : MonoBehaviour, IController {

        private List<Cell> availableCellForMove;
        private Cell targetCell;

        public void Initialize() {
        }

        public void InitPiece(Piece piece) {
            piece.interactive.OnTouchDown += delegate { Piece_OnTouchDown(piece); };
            piece.interactive.OnTouchUp += delegate { Piece_OnTouchDown(piece); };
            piece.interactive.OnMove += delegate (InteractiveEventArgs args) { Piece_OnMove(args, piece); };
        }
        

        private void Piece_OnTouchDown(Piece piece) {
            GetAvailableCellsForMove(piece);
        }

        private void GetAvailableCellsForMove(Piece piece) {
            availableCellForMove = new List<Cell>();

            foreach (PieceMoveAlgorithm move in piece.moves) {

            }
        }

        private void Piece_OnTouchUp(Piece piece) {
            targetCell = null;
        }

        private void Piece_OnMove(InteractiveEventArgs eventArgs, Piece piece) {
            eventArgs.sender.transform.position += (Vector3)eventArgs.data.delta;

            foreach (Cell cell in availableCellForMove) {
                if (RectTransformUtility.RectangleContainsScreenPoint(cell.rectT, Input.mousePosition)) {
                    targetCell = cell;
                    break;
                }
            }
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {
    
    public class PieceMoveAlgorithm {

        protected Vector3Int moveVector = new Vector3Int(1, 1, 1);

        public virtual List<MoveInfo> GetAvailableMoves(Piece piece, IBoardContoller boardController) {
            List<MoveInfo> moves = new List<MoveInfo>();

            //Horizontal
            FillCellPath(moves, piece, boardController, 1, 0, moveVector.x);
            FillCellPath(moves, piece, boardController, -1, 0, moveVector.x);

            //Vertical
            FillCellPath(moves, piece, boardController, 0, 1, moveVector.y);
            FillCellPath(moves, piece, boardController, 0, -1, moveVector.y);

            //Diagonal 1
            FillCellPath(moves, piece, boardController, 1, 1, moveVector.z);
            FillCellPath(moves, piece, boardController, -1, -1, moveVector.z);

            //Diagonal 2
            FillCellPath(moves, piece, boardController, -1, 1, moveVector.z);
            FillCellPath(moves, piece, boardController, 1, -1, moveVector.z);

            return moves;
        }

        protected virtual void FillCellPath(List<MoveInfo> moves, Piece piece, IBoardContoller boardController, int xDirection, int yDirection, int movement) {
            Cell targetCell = boardController.GetCell(piece.coord.x, piece.coord.y);

            int currentX = piece.coord.x;
            int currentY = piece.coord.y;

            for (int i = 0; i < movement; i++) {
                currentX += xDirection;
                currentY += yDirection;

                Cell currentCell = boardController.GetCell(currentX, currentY);
                Cell.State cellState = boardController.GetCellStateForPiece(currentX, currentY, piece);

                if (cellState == Cell.State.Enemy) {
                    PieceKillCommand killCommand = new PieceKillCommand();
                    PieceMoveCommand moveCommand = new PieceMoveCommand();

                    MoveInfo move = new MoveInfo(currentCell, moveCommand);
                    moves.Add(move);
                    break;
                }
                if (cellState == Cell.State.Free) {
                    PieceMoveCommand moveCommand = new PieceMoveCommand();
                    MoveInfo move = new MoveInfo(currentCell, moveCommand);

                    moves.Add(move);
                    break;
                }
            }
        }

    }
}


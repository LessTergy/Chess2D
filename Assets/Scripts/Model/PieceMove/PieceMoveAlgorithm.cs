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
            int currentX = piece.coord.x;
            int currentY = piece.coord.y;

            for (int i = 0; i < movement; i++) {
                currentX += xDirection;
                currentY += yDirection;
                
                Cell.State cellState = boardController.GetCellStateForPiece(currentX, currentY, piece);

                if (cellState == Cell.State.OutOfBounds || cellState == Cell.State.Friendly) {
                    return;
                }

                Cell currentCell = boardController.GetCell(currentX, currentY);

                if (cellState == Cell.State.Enemy) {
                    PieceKillCommand killCommand = new PieceKillCommand(boardController, currentCell, currentCell.currentPiece);
                    PieceMoveCommand moveCommand = new PieceMoveCommand(boardController, piece.coord, new Vector2Int(currentX, currentY));

                    CommandContainer container = new CommandContainer(killCommand, moveCommand);

                    MoveInfo move = new MoveInfo(currentCell, container);
                    moves.Add(move);

                    return;
                } else
                if (cellState == Cell.State.Free) {
                    PieceMoveCommand moveCommand = new PieceMoveCommand(boardController, piece.coord, new Vector2Int(currentX, currentY));
                    MoveInfo move = new MoveInfo(currentCell, moveCommand);
                    moves.Add(move);
                }

            }
        }
    }
}


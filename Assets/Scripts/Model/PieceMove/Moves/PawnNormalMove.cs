using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lesstergy.Chess2D {

    public class PawnNormalMove : PieceMoveAlgorithm {

        int whiteStartRow = 1;
        int blackStartRow = 6;

        public PawnNormalMove() {
            moveVector = new Vector3Int(0, 1, 0);
        }

        public override List<MoveInfo> GetAvailableMoves(Piece piece, IBoardContoller boardController) {
            List<MoveInfo> moves = new List<MoveInfo>();

            Vector3Int pawnMoveVector = moveVector;
            pawnMoveVector *= (piece.teamType == ChessTeam.Type.White) ? 1 : -1;

            int startRow = (piece.teamType == ChessTeam.Type.White) ? whiteStartRow : blackStartRow;
            bool standAtStart = (piece.coord.y == startRow);

            int movement = (standAtStart) ? 2 : 1;
            FillCellPath(moves, piece, boardController, moveVector.y, movement);

            return moves;
        }

        public void FillCellPath(List<MoveInfo> moves, Piece piece, IBoardContoller boardController, int yDirection, int movement) {
            int currentX = piece.coord.x;
            int currentY = piece.coord.y;

            for (int i = 0; i < movement; i++) {
                currentY += yDirection;

                Cell.State cellState = boardController.GetCellStateForPiece(currentX, currentY, piece);

                if (cellState == Cell.State.Free) {
                    Cell currentCell = boardController.GetCell(currentX, currentY);

                    PieceMoveCommand moveCommand = new PieceMoveCommand(boardController, piece.coord, new Vector2Int(currentX, currentY));
                    MoveInfo move = new MoveInfo(currentCell, moveCommand);
                    moves.Add(move);
                } else {
                    return;
                }
            }

        }

    }

}

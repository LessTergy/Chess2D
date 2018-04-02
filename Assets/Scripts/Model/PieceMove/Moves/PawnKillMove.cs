using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PawnKillMove : PieceMoveAlgorithm {

        public PawnKillMove() {
            moveVector = new Vector3Int(0, 0, 1);
        }

        public override List<MoveInfo> GetAvailableMoves(Piece piece, IBoardContoller boardController) {
            List<MoveInfo> moves = new List<MoveInfo>();
            
            Vector3Int pawnMoveVector = moveVector;
            pawnMoveVector *= (piece.teamType == ChessTeam.Type.White) ? 1 : -1;

            FillCellPath(moves, piece, boardController, piece.coord.x - moveVector.z, piece.coord.y + moveVector.z);
            FillCellPath(moves, piece, boardController, piece.coord.x + moveVector.z, piece.coord.y + moveVector.z);

            return moves;
        }

        private void FillCellPath(List<MoveInfo> moves, Piece piece, IBoardContoller boardController, int targetX, int targetY) {
            Cell.State cellState = boardController.GetCellStateForPiece(targetX, targetY, piece);

            if (cellState == Cell.State.Enemy) {
                Cell currentCell = boardController.GetCell(targetX, targetY);

                PieceKillCommand killCommand = new PieceKillCommand(boardController, currentCell, currentCell.currentPiece);
                PieceMoveCommand moveCommand = new PieceMoveCommand(boardController, piece.coord, new Vector2Int(targetX, targetY));

                CommandContainer container = new CommandContainer(killCommand, moveCommand);

                MoveInfo move = new MoveInfo(currentCell, container);
                moves.Add(move);
            }
        }

    }
}

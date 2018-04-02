using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PawnEnPassantMove : PieceMoveAlgorithm {

        int whiteEnemyPawnRow = 3;
        int blackEnemyPawnRow = 4;

        public PawnEnPassantMove() {
            moveVector = new Vector3Int(0, 1, 0);
        }

        public override List<MoveInfo> GetAvailableMoves(Piece movingPiece, IBoardContoller boardController) {
            List<MoveInfo> moves = new List<MoveInfo>();

            int enemyRow = (movingPiece.teamType == ChessTeam.Type.White) ? blackEnemyPawnRow : whiteEnemyPawnRow;

            if (movingPiece.coord.y == enemyRow) {
                Vector3Int pMoveVector = InvertVectorMoveByTeam(moveVector, movingPiece.teamType);

                FillCellMove(moves, boardController, movingPiece, -1, moveVector.y);
                FillCellMove(moves, boardController, movingPiece, 1, moveVector.y);
            }

            return moves;
        }

        private new void FillCellMove(List<MoveInfo> moves, IBoardContoller boardController, Piece movingPiece, int xOffset, int yOffset) {
            Vector2Int coord = movingPiece.coord;

            Cell.State moveCellState = boardController.GetCellStateForPiece(coord.x + xOffset, coord.y + yOffset, movingPiece);
            Cell.State enemyPawnCellState = boardController.GetCellStateForPiece(coord.x + xOffset, coord.y, movingPiece);

            if (moveCellState == Cell.State.Free && enemyPawnCellState == Cell.State.Enemy) {
                Cell moveCell = boardController.GetCell(coord.x + xOffset, coord.y + yOffset);
                Cell enemyCell = boardController.GetCell(coord.x + xOffset, coord.y);

                Piece enemyPiece = enemyCell.currentPiece;

                if (enemyPiece.type == Piece.Type.Pawn && enemyPiece.isLastMoving) {
                    PieceKillCommand killCommand = new PieceKillCommand(boardController, enemyCell, enemyPiece);
                    PieceMoveCommand moveCommand = new PieceMoveCommand(boardController, movingPiece.coord, moveCell.coord);

                    MoveInfo moveInfo = new MoveInfo(moveCell, new CommandContainer(killCommand, moveCommand));
                    moves.Add(moveInfo);
                }
            }
        }
    }

}

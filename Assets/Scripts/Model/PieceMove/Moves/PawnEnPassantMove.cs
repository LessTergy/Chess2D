using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{

    public class PawnEnPassantMove : PieceMoveAlgorithm
    {
        private const int WhiteEnemyPawnRow = 3;
        private const int BlackEnemyPawnRow = 4;

        protected override Vector3Int GetMoveVector()
        {
            return new Vector3Int(0, 1, 0);
        }

        public override List<MoveInfo> GetAvailableMoves(PieceView movingPiece, IBoardController boardController)
        {
            var moves = new List<MoveInfo>();

            int enemyRow = (movingPiece.TeamType == TeamType.White) ? BlackEnemyPawnRow : WhiteEnemyPawnRow;

            if (movingPiece.cellCoord.y == enemyRow)
            {
                Vector3Int pMoveVector = InvertVectorMoveByTeam(MoveVector, movingPiece.TeamType);

                FillCellMove(moves, boardController, movingPiece, -1, pMoveVector.y);
                FillCellMove(moves, boardController, movingPiece, 1, pMoveVector.y);
            }

            return moves;
        }

        private new void FillCellMove(List<MoveInfo> moves, IBoardController boardController, PieceView movingPiece, int xOffset, int yOffset)
        {
            Vector2Int coord = movingPiece.cellCoord;

            CellState moveCellState = boardController.GetCellStateForPiece(coord.x + xOffset, coord.y + yOffset, movingPiece);
            CellState enemyPawnCellState = boardController.GetCellStateForPiece(coord.x + xOffset, coord.y, movingPiece);

            if (moveCellState == CellState.Free && enemyPawnCellState == CellState.Enemy)
            {
                CellView moveCell = boardController.GetCell(coord.x + xOffset, coord.y + yOffset);
                CellView enemyCell = boardController.GetCell(coord.x + xOffset, coord.y);

                PieceView enemyPiece = enemyCell.CurrentPiece;

                if (enemyPiece.Type == PieceType.Pawn && enemyPiece.isLastMoving)
                {
                    var killCommand = new PieceKillCommand(boardController, enemyPiece);
                    var moveCommand = new PieceMoveCommand(boardController, movingPiece, moveCell.Coord);

                    var moveInfo = new MoveInfo(moveCell, new CommandContainer(killCommand, moveCommand));
                    moves.Add(moveInfo);
                }
            }
        }
    }

}
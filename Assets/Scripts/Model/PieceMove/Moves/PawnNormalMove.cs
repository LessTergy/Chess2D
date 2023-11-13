using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public class PawnNormalMove : PieceMoveAlgorithm
    {
        private const int WhiteStartRow = 1;
        private const int BlackStartRow = 6;
        
        protected override Vector3Int GetMoveVector()
        {
            return new Vector3Int(0, 1, 0);
        }

        public override List<MoveInfo> GetAvailableMoves(PieceView movingPiece, IBoardController boardController)
        {
            var moves = new List<MoveInfo>();
            Vector3Int pMoveVector = InvertVectorMoveByTeam(MoveVector, movingPiece.TeamType);

            int startRow = (movingPiece.TeamType == TeamType.White) ? WhiteStartRow : BlackStartRow;
            bool standAtStart = (movingPiece.cellCoord.y == startRow);

            int movement = (standAtStart) ? 2 : 1;
            FillCellPath(moves, movingPiece, boardController, pMoveVector.y, movement);

            return moves;
        }

        public void FillCellPath(List<MoveInfo> moves, PieceView piece, IBoardController boardController, int yDirection, int movement)
        {
            int currentX = piece.cellCoord.x;
            int currentY = piece.cellCoord.y;

            for (var i = 0; i < movement; i++)
            {
                currentY += yDirection;

                CellState cellState = boardController.GetCellStateForPiece(currentX, currentY, piece);

                if (cellState == CellState.Free)
                {
                    CellView currentCell = boardController.GetCell(currentX, currentY);

                    var moveCommand = new PieceMoveCommand(boardController, piece, new Vector2Int(currentX, currentY));
                    var move = new MoveInfo(currentCell, moveCommand);
                    moves.Add(move);
                }
                else
                {
                    return;
                }
            }
        }
    }
}
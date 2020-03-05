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

        public override List<MoveInfo> GetAvailableMoves(Piece movingPiece, IBoardController boardController)
        {
            List<MoveInfo> moves = new List<MoveInfo>();
            Vector3Int pMoveVector = InvertVectorMoveByTeam(moveVector, movingPiece.teamType);

            int startRow = (movingPiece.teamType == ChessTeam.Type.White) ? WhiteStartRow : BlackStartRow;
            bool standAtStart = (movingPiece.cellCoord.y == startRow);

            int movement = (standAtStart) ? 2 : 1;
            FillCellPath(moves, movingPiece, boardController, pMoveVector.y, movement);

            return moves;
        }

        public void FillCellPath(List<MoveInfo> moves, Piece piece, IBoardController boardController, int yDirection, int movement)
        {
            int currentX = piece.cellCoord.x;
            int currentY = piece.cellCoord.y;

            for (int i = 0; i < movement; i++)
            {
                currentY += yDirection;

                Cell.State cellState = boardController.GetCellStateForPiece(currentX, currentY, piece);

                if (cellState == Cell.State.Free)
                {
                    Cell currentCell = boardController.GetCell(currentX, currentY);

                    PieceMoveCommand moveCommand = new PieceMoveCommand(boardController, piece, new Vector2Int(currentX, currentY));
                    MoveInfo move = new MoveInfo(currentCell, moveCommand);
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
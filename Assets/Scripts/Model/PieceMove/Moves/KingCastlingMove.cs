using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{

    public class KingCastlingMove : PieceMoveAlgorithm
    {
        private const int KingXPosition = 4;
        private const int RookLeftXPosition = 0;
        private const int RookRightXPosition = 7;

        private const int WhiteYRow = 0;
        private const int BlackYRow = 7;
        
        protected override Vector3Int GetMoveVector()
        {
            return Vector3Int.one;
        }

        public override List<MoveInfo> GetAvailableMoves(PieceView piece, IBoardController boardController)
        {
            var moves = new List<MoveInfo>();

            int yCoord = (piece.TeamType == TeamType.White) ? WhiteYRow : BlackYRow;
            var kingStartPos = new Vector2Int(KingXPosition, yCoord);

            //king stand at start position, never was moving and isn't in check
            if (piece.cellCoord == kingStartPos && !piece.isWasMoving && !piece.isTarget)
            {
                FillCellPath(moves, boardController, piece, true);
                FillCellPath(moves, boardController, piece, false);
            }

            return moves;
        }

        private void FillCellPath(List<MoveInfo> moves, IBoardController boardController, PieceView kingPiece, bool isLeft)
        {
            int rookXPosition = (isLeft) ? RookLeftXPosition : RookRightXPosition;
            var rookCoord = new Vector2Int(rookXPosition, kingPiece.cellCoord.y);

            CellView rookCell = boardController.GetCell(rookCoord);
            CellState cellState = boardController.GetCellStateForPiece(rookCoord.x, rookCoord.y, kingPiece);

            if (cellState != CellState.Friendly)
            {
                return;
            }

            PieceView rookPiece = rookCell.CurrentPiece;

            //rook never was moving and doesn't have anything between
            if (rookPiece.Type == PieceType.Rook && !rookPiece.isWasMoving && IsEmptyBetweenCoords(boardController, kingPiece.cellCoord, rookPiece.cellCoord))
            {
                int kingXOffset = (isLeft) ? -2 : 2;
                var newKingCoord = new Vector2Int(kingPiece.cellCoord.x + kingXOffset, kingPiece.cellCoord.y);

                int rookXOffset = (isLeft) ? 1 : -1;
                var newRookCoord = new Vector2Int(newKingCoord.x + rookXOffset, newKingCoord.y);

                var kingMoveCommand = new PieceMoveCommand(boardController, kingPiece, newKingCoord);
                var rookMoveCommand = new PieceMoveCommand(boardController, rookPiece, newRookCoord);

                var container = new CommandContainer(kingMoveCommand, rookMoveCommand);

                var move = new MoveInfo(boardController.GetCell(newKingCoord), container);
                moves.Add(move);
            }
        }


        private bool IsEmptyBetweenCoords(IBoardController boardController, Vector2Int coordA, Vector2Int coordB)
        {
            int yCoord = coordA.y;
            int xStart = Mathf.Min(coordA.x, coordB.x);
            int xEnd = Mathf.Max(coordA.x, coordB.x);

            for (int xCoord = xStart + 1; xCoord < xEnd; xCoord++)
            {
                CellView cell = boardController.GetCell(xCoord, yCoord);
                if (!cell.IsEmpty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
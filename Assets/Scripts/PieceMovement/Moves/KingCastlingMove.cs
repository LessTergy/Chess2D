using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.PieceMovement
{
    // https://en.wikipedia.org/wiki/Castling
    public class KingCastlingMove : PieceMoveAlgorithm
    {
        private const int KingXIndex = 4;
        private const int WhiteYIndex = GameConstants.StartIndex;
        private const int BlackYIndex = GameConstants.FinishIndex;
        
        private const int RookLeftXPosition = GameConstants.StartIndex;
        private const int RookRightXPosition = GameConstants.FinishIndex;

        protected override Vector3Int MoveVector => Vector3Int.one;

        public override List<MoveData> GetAvailableMoves(PieceView piece, BoardController boardController)
        {
            var moves = new List<MoveData>();

            Vector2Int kingStartCoord = GetKingStartCoord(piece.PlayerType);
            // king stand at start coord, never was moving and isn't in check
            bool validMove = piece.cellCoord == kingStartCoord && !piece.isWasMoving && !piece.isTarget;
            if (!validMove) return moves;
            
            FillCellPath(moves, boardController, piece, true);
            FillCellPath(moves, boardController, piece, false);
            return moves;
        }

        private Vector2Int GetKingStartCoord(PlayerType playerType)
        {
            int yCoord = (playerType == PlayerType.White) ? WhiteYIndex : BlackYIndex;
            return new Vector2Int(KingXIndex, yCoord);
        }

        private void FillCellPath(List<MoveData> moves, BoardController boardController, PieceView kingPiece, bool isLeft)
        {
            int rookXPosition = (isLeft) ? RookLeftXPosition : RookRightXPosition;
            var rookCoord = new Vector2Int(rookXPosition, kingPiece.cellCoord.y);

            CellView rookCell = boardController.GetCell(rookCoord);
            CellState cellState = boardController.GetCellStateForMove(rookCoord.x, rookCoord.y, kingPiece);

            if (cellState != CellState.Friendly)
            {
                return;
            }

            PieceView rookPiece = rookCell.CurrentPiece;
            if (rookPiece.Type != PieceType.Rook)
            {
                return;
            }

            //rook never was moving and doesn't have anything between
            bool isEmptyBetween = IsEmptyBetweenCoords(boardController, kingPiece.cellCoord, rookPiece.cellCoord);
            bool validMove = !rookPiece.isWasMoving && isEmptyBetween;
            if (!validMove) return;
            
            int kingXOffset = (isLeft) ? -2 : 2;
            var newKingCoord = new Vector2Int(kingPiece.cellCoord.x + kingXOffset, kingPiece.cellCoord.y);

            int rookXOffset = (isLeft) ? 1 : -1;
            var newRookCoord = new Vector2Int(newKingCoord.x + rookXOffset, newKingCoord.y);

            var kingMoveCommand = new PieceMoveCommand(boardController, kingPiece, newKingCoord);
            var rookMoveCommand = new PieceMoveCommand(boardController, rookPiece, newRookCoord);

            var container = new CommandContainer(kingMoveCommand, rookMoveCommand);

            var move = new MoveData(boardController.GetCell(newKingCoord), container);
            moves.Add(move);
        }

        private bool IsEmptyBetweenCoords(BoardController boardController, Vector2Int coordA, Vector2Int coordB)
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
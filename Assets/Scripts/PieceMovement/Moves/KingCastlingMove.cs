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

            if (piece.cellCoord != GetKingStartCoord(piece.PlayerType) || piece.isWasMoving || boardController.IsKingInCheck(piece))
            {
                return moves;
            }
            
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

            if (rookPiece.isWasMoving || !IsEmptyBetweenCoords(boardController, kingPiece.cellCoord, rookPiece.cellCoord))
            {
                return;
            }

            var direction = isLeft ? -1 : 1;
            var kingTransitCoord = new Vector2Int(kingPiece.cellCoord.x + direction, kingPiece.cellCoord.y);
            var kingTransitCommand = new PieceMoveCommand(boardController, kingPiece, kingTransitCoord);
            if (LeavesKingInCheck(kingTransitCommand, kingPiece, boardController))
            {
                return;
            }

            var newKingCoord = new Vector2Int(kingTransitCoord.x + direction, kingTransitCoord.y);
            var kingMoveCommand = new PieceMoveCommand(boardController, kingPiece, newKingCoord);
            var rookMoveCommand = new PieceMoveCommand(boardController, rookPiece, kingTransitCoord);
            var castlingCommand = new CommandContainer(kingMoveCommand, rookMoveCommand);
            if (!LeavesKingInCheck(castlingCommand, kingPiece, boardController))
            {
                moves.Add(new MoveData(boardController.GetCell(newKingCoord), castlingCommand));
            }
        }

        private static bool LeavesKingInCheck(ICommand command, PieceView king, BoardController boardController)
        {
            command.Execute();
            try
            {
                return boardController.IsKingInCheck(king);
            }
            finally
            {
                command.Undo();
            }
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
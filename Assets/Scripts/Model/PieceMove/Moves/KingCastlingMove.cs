using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.Model;
using Chess2D.Model.PieceMove;
using Chess2D.UI;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class KingCastlingMove : PieceMoveAlgorithm {

        private int kingXPosition = 4;
        private int rookLeftXPosition = 0;
        private int rookRightXPosition = 7;

        private int whiteYRow = 0;
        private int blackYRow = 7;

        public override List<MoveInfo> GetAvailableMoves(Piece piece, IBoardController boardController) {
            List<MoveInfo> moves = new List<MoveInfo>();

            int yCoord = (piece.teamType == ChessTeam.Type.White) ? whiteYRow : blackYRow;
            Vector2Int kingStartPos = new Vector2Int(kingXPosition, yCoord);

            //king stand at start position, never was moving and isn't in check
            if (piece.cellCoord == kingStartPos && !piece.isWasMoving && !piece.isTarget) {
                FillCellPath(moves, boardController, piece, true);
                FillCellPath(moves, boardController, piece, false);
            }

            return moves;
        }

        private void FillCellPath(List<MoveInfo> moves, IBoardController boardController, Piece kingPiece, bool isLeft) {
            int rookXPosition = (isLeft) ? rookLeftXPosition : rookRightXPosition;
            Vector2Int rookCoord = new Vector2Int(rookXPosition, kingPiece.cellCoord.y);

            Cell rookCell = boardController.GetCell(rookCoord);
            Cell.State cellState = boardController.GetCellStateForPiece(rookCoord.x, rookCoord.y, kingPiece);
            
            if (cellState == Cell.State.Friendly) {
                Piece rookPiece = rookCell.currentPiece;

                //rook never was moving and doesn't have anything between
                if (rookPiece.type == Piece.Type.Rook && !rookPiece.isWasMoving && IsEmptyBetweenCoords(boardController, kingPiece.cellCoord, rookPiece.cellCoord)) {
                    int kingXOffset = (isLeft) ? -2 : 2;
                    Vector2Int newKingCoord = new Vector2Int(kingPiece.cellCoord.x + kingXOffset, kingPiece.cellCoord.y);

                    int rookXOffset = (isLeft) ? 1 : -1;
                    Vector2Int newRookCoord = new Vector2Int(newKingCoord.x + rookXOffset, newKingCoord.y);

                    PieceMoveCommand kingMoveCommand = new PieceMoveCommand(boardController, kingPiece, newKingCoord);
                    PieceMoveCommand rookMoveCommand = new PieceMoveCommand(boardController, rookPiece, newRookCoord);

                    CommandContainer container = new CommandContainer(kingMoveCommand, rookMoveCommand);

                    MoveInfo move = new MoveInfo(boardController.GetCell(newKingCoord), container);
                    moves.Add(move);
                }
            }
        }


        private bool IsEmptyBetweenCoords(IBoardController boardController, Vector2Int coordA, Vector2Int coordB) {
            int yCoord = coordA.y;
            int xStart = Mathf.Min(coordA.x, coordB.x);
            int xEnd = Mathf.Max(coordA.x, coordB.x);

            for (int xCoord = xStart + 1; xCoord < xEnd; xCoord++) {
                Cell cell = boardController.GetCell(xCoord, yCoord);
                if (!cell.isEmpty) {
                    return false;
                }
            }

            return true;
        }
    }

}

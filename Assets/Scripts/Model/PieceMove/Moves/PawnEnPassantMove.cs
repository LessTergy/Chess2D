using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public class PawnEnPassantMove : PieceMoveAlgorithm
    {
        private const int WhiteOpponentYIndex = 3;
        private const int BlackOpponentYIndex = 4;
        protected override Vector3Int MoveVector => new(0, 1, 0);

        public override List<MoveData> GetAvailableMoves(PieceView movingPiece, BoardController boardController)
        {
            var moves = new List<MoveData>();
            int opponentYIndex = GetOpponentYIndex(movingPiece.PlayerType);

            if (movingPiece.cellCoord.y == opponentYIndex)
            {
                Vector3Int pMoveVector = InvertMoveVector(MoveVector, movingPiece.PlayerType);

                FillCellMove(moves, boardController, movingPiece, -1, pMoveVector.y);
                FillCellMove(moves, boardController, movingPiece, 1, pMoveVector.y);
            }
            return moves;
        }

        private int GetOpponentYIndex(PlayerType playerType)
        {
            return playerType == PlayerType.White ? BlackOpponentYIndex : WhiteOpponentYIndex;
        }

        private new void FillCellMove(List<MoveData> moves, BoardController boardController, PieceView movingPiece, int xOffset, int yOffset)
        {
            Vector2Int coord = movingPiece.cellCoord;

            CellState moveCellState = boardController.GetCellStateForMove(coord.x + xOffset, coord.y + yOffset, movingPiece);
            CellState opponentPawnCellState = boardController.GetCellStateForMove(coord.x + xOffset, coord.y, movingPiece);

            bool validMove = moveCellState == CellState.Free && opponentPawnCellState == CellState.Opponent;
            if (!validMove) return;
            
            CellView opponentCell = boardController.GetCell(coord.x + xOffset, coord.y);
            PieceView opponentPiece = opponentCell.CurrentPiece;
            
            validMove = opponentPiece.Type == PieceType.Pawn && opponentPiece.isLastMoving;
            if (!validMove) return;
            
            CellView moveCell = boardController.GetCell(coord.x + xOffset, coord.y + yOffset);
            var killCommand = new PieceKillCommand(boardController, opponentPiece);
            var moveCommand = new PieceMoveCommand(boardController, movingPiece, moveCell.Coord);
            var moveInfo = new MoveData(moveCell, new CommandContainer(killCommand, moveCommand));
            
            moves.Add(moveInfo);
        }
    }
}
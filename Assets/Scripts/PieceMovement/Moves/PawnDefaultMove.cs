﻿using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.PieceMovement
{
    public class PawnDefaultMove : PieceMoveAlgorithm
    {
        private const int WhiteStartYIndex = GameConstants.StartIndex + 1;
        private const int BlackStartYIndex = GameConstants.FinishIndex - 1;
        
        protected override Vector3Int MoveVector => new(0, 1, 0);

        public override List<MoveData> GetAvailableMoves(PieceView movingPiece, BoardController boardController)
        {
            var moves = new List<MoveData>();
            Vector3Int pMoveVector = InvertMoveVector(MoveVector, movingPiece.PlayerType);

            int startYIndex = (movingPiece.PlayerType == PlayerType.White) ? WhiteStartYIndex : BlackStartYIndex;
            bool standAtStart = (movingPiece.cellCoord.y == startYIndex);

            int movement = (standAtStart) ? 2 : 1;
            FillCellPath(moves, movingPiece, boardController, pMoveVector.y, movement);

            return moves;
        }

        private void FillCellPath(List<MoveData> moves, PieceView piece, BoardController boardController, int yDirection, int movement)
        {
            int currentX = piece.cellCoord.x;
            int currentY = piece.cellCoord.y;

            for (var i = 0; i < movement; i++)
            {
                currentY += yDirection;

                CellState cellState = boardController.GetCellStateForMove(currentX, currentY, piece);

                if (cellState == CellState.Free)
                {
                    CellView currentCell = boardController.GetCell(currentX, currentY);

                    var moveCommand = new PieceMoveCommand(boardController, piece, new Vector2Int(currentX, currentY));
                    var move = new MoveData(currentCell, moveCommand);
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
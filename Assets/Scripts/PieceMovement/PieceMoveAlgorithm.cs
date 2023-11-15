using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.PieceMovement
{
    public abstract class PieceMoveAlgorithm
    {
        protected abstract Vector3Int MoveVector { get; }

        public virtual List<MoveData> GetAvailableMoves(PieceView movingPiece, BoardController boardController)
        {
            var moves = new List<MoveData>();

            //Horizontal
            FillCellsPath(moves, boardController, movingPiece, 1, 0, MoveVector.x);
            FillCellsPath(moves, boardController, movingPiece, -1, 0, MoveVector.x);

            //Vertical
            FillCellsPath(moves, boardController, movingPiece, 0, 1, MoveVector.y);
            FillCellsPath(moves, boardController, movingPiece, 0, -1, MoveVector.y);

            //Diagonal 1
            FillCellsPath(moves, boardController, movingPiece, 1, 1, MoveVector.z);
            FillCellsPath(moves, boardController, movingPiece, -1, -1, MoveVector.z);

            //Diagonal 2
            FillCellsPath(moves, boardController, movingPiece, -1, 1, MoveVector.z);
            FillCellsPath(moves, boardController, movingPiece, 1, -1, MoveVector.z);

            return moves;
        }

        private void FillCellsPath(List<MoveData> moves, BoardController boardController, PieceView movingPiece, 
            int xDirection, int yDirection, int movementCount)
        {
            int currentX = movingPiece.cellCoord.x;
            int currentY = movingPiece.cellCoord.y;

            for (var i = 0; i < movementCount; i++)
            {
                currentX += xDirection;
                currentY += yDirection;

                bool canMove = FillCellMove(moves, boardController, movingPiece, currentX, currentY);
                if (!canMove)
                {
                    return;
                }
            }
        }

        //return true, if can continue path
        protected bool FillCellMove(List<MoveData> moves, BoardController boardController, PieceView movingPiece, int targetX, int targetY)
        {
            CellState cellState = boardController.GetCellStateForMove(targetX, targetY, movingPiece);
            if (cellState is CellState.OutOfBounds or CellState.Friendly)
            {
                return false;
            }

            CellView currentCell = boardController.GetCell(targetX, targetY);
            if (cellState == CellState.Opponent)
            {
                FillKillMove(moves, boardController, movingPiece, targetX, targetY);
                return false;
            }
            
            if (cellState == CellState.Free)
            {
                var moveCommand = new PieceMoveCommand(boardController, movingPiece, new Vector2Int(targetX, targetY));
                var move = new MoveData(currentCell, moveCommand);
                moves.Add(move);
                return true;
            }
            
            return false;
        }

        protected void FillKillMove(List<MoveData> moves, BoardController boardController, PieceView movingPiece, int targetX, int targetY)
        {
            CellState cellState = boardController.GetCellStateForMove(targetX, targetY, movingPiece);
            if (cellState != CellState.Opponent) return;
            
            CellView currentCell = boardController.GetCell(targetX, targetY);

            var killCommand = new PieceKillCommand(boardController, currentCell.CurrentPiece);
            var moveCommand = new PieceMoveCommand(boardController, movingPiece, new Vector2Int(targetX, targetY));

            var container = new CommandContainer(killCommand, moveCommand);
            var move = new MoveData(currentCell, container);

            moves.Add(move);
        }

        protected Vector3Int InvertMoveVector(Vector3Int moveVector, PlayerType playerType)
        {
            if (playerType == PlayerType.Black)
            {
                return moveVector * -1;
            }
            return moveVector;
        }
    }
}
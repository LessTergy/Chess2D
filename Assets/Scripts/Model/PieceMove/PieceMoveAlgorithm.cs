using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public abstract class PieceMoveAlgorithm
    {
        private Vector3Int ?_cachedMoveVector;
        protected Vector3Int MoveVector
        {
            get
            {
                if (_cachedMoveVector == null)
                {
                    _cachedMoveVector = GetMoveVector();
                }
                return _cachedMoveVector.Value;
            }
        }

        protected abstract Vector3Int GetMoveVector();

        public virtual List<MoveInfo> GetAvailableMoves(PieceView movingPiece, IBoardController boardController)
        {
            var moves = new List<MoveInfo>();

            //Horizontal
            FillCellPath(moves, boardController, movingPiece, 1, 0, MoveVector.x);
            FillCellPath(moves, boardController, movingPiece, -1, 0, MoveVector.x);

            //Vertical
            FillCellPath(moves, boardController, movingPiece, 0, 1, MoveVector.y);
            FillCellPath(moves, boardController, movingPiece, 0, -1, MoveVector.y);

            //Diagonal 1
            FillCellPath(moves, boardController, movingPiece, 1, 1, MoveVector.z);
            FillCellPath(moves, boardController, movingPiece, -1, -1, MoveVector.z);

            //Diagonal 2
            FillCellPath(moves, boardController, movingPiece, -1, 1, MoveVector.z);
            FillCellPath(moves, boardController, movingPiece, 1, -1, MoveVector.z);

            return moves;
        }

        protected void FillCellPath(List<MoveInfo> moves, IBoardController boardController, PieceView movingPiece, int xDirection, int yDirection, int movement)
        {
            int currentX = movingPiece.cellCoord.x;
            int currentY = movingPiece.cellCoord.y;

            for (var i = 0; i < movement; i++)
            {
                currentX += xDirection;
                currentY += yDirection;

                if (!FillCellMove(moves, boardController, movingPiece, currentX, currentY))
                {
                    return;
                }
            }
        }

        //return true, if can continue cell path
        protected bool FillCellMove(List<MoveInfo> moves, IBoardController boardController, PieceView movingPiece, int targetX, int targetY)
        {
            CellState cellState = boardController.GetCellStateForPiece(targetX, targetY, movingPiece);

            if (cellState is CellState.OutOfBounds or CellState.Friendly)
            {
                return false;
            }

            CellView currentCell = boardController.GetCell(targetX, targetY);

            if (cellState == CellState.Enemy)
            {
                FillKillMove(moves, boardController, movingPiece, targetX, targetY);
                return false;

            }
            else
            if (cellState == CellState.Free)
            {
                var moveCommand = new PieceMoveCommand(boardController, movingPiece, new Vector2Int(targetX, targetY));
                var move = new MoveInfo(currentCell, moveCommand);
                moves.Add(move);
            }

            return true;
        }

        protected void FillKillMove(List<MoveInfo> moves, IBoardController boardController, PieceView movingPiece, int targetX, int targetY)
        {
            CellState cellState = boardController.GetCellStateForPiece(targetX, targetY, movingPiece);

            if (cellState == CellState.Enemy)
            {
                CellView currentCell = boardController.GetCell(targetX, targetY);

                var killCommand = new PieceKillCommand(boardController, currentCell.CurrentPiece);
                var moveCommand = new PieceMoveCommand(boardController, movingPiece, new Vector2Int(targetX, targetY));

                var container = new CommandContainer(killCommand, moveCommand);
                var move = new MoveInfo(currentCell, container);

                moves.Add(move);
            }
        }

        protected Vector3Int InvertVectorMoveByTeam(Vector3Int moveVector, TeamType teamType)
        {
            return (teamType == TeamType.White) ? moveVector : (moveVector * -1);
        }
    }
}
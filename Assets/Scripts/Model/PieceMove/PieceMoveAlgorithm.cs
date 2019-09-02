using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.UI;
using Lesstergy.Chess2D;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{

    public class PieceMoveAlgorithm
    {

        protected Vector3Int moveVector = new Vector3Int(1, 1, 1);

        public virtual List<MoveInfo> GetAvailableMoves(Piece movingPiece, IBoardController boardController)
        {
            List<MoveInfo> moves = new List<MoveInfo>();

            //Horizontal
            FillCellPath(moves, boardController, movingPiece, 1, 0, moveVector.x);
            FillCellPath(moves, boardController, movingPiece, -1, 0, moveVector.x);

            //Vertical
            FillCellPath(moves, boardController, movingPiece, 0, 1, moveVector.y);
            FillCellPath(moves, boardController, movingPiece, 0, -1, moveVector.y);

            //Diagonal 1
            FillCellPath(moves, boardController, movingPiece, 1, 1, moveVector.z);
            FillCellPath(moves, boardController, movingPiece, -1, -1, moveVector.z);

            //Diagonal 2
            FillCellPath(moves, boardController, movingPiece, -1, 1, moveVector.z);
            FillCellPath(moves, boardController, movingPiece, 1, -1, moveVector.z);

            return moves;
        }

        protected void FillCellPath(List<MoveInfo> moves, IBoardController boardController, Piece movingPiece, int xDirection, int yDirection, int movement)
        {
            int currentX = movingPiece.cellCoord.x;
            int currentY = movingPiece.cellCoord.y;

            for (int i = 0; i < movement; i++)
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
        protected bool FillCellMove(List<MoveInfo> moves, IBoardController boardController, Piece movingPiece, int targetX, int targetY)
        {
            Cell.State cellState = boardController.GetCellStateForPiece(targetX, targetY, movingPiece);

            if (cellState == Cell.State.OutOfBounds || cellState == Cell.State.Friendly)
            {
                return false;
            }

            Cell currentCell = boardController.GetCell(targetX, targetY);

            if (cellState == Cell.State.Enemy)
            {
                FillKillMove(moves, boardController, movingPiece, targetX, targetY);
                return false;

            }
            else
            if (cellState == Cell.State.Free)
            {
                PieceMoveCommand moveCommand = new PieceMoveCommand(boardController, movingPiece, new Vector2Int(targetX, targetY));
                MoveInfo move = new MoveInfo(currentCell, moveCommand);
                moves.Add(move);
            }

            return true;
        }

        protected void FillKillMove(List<MoveInfo> moves, IBoardController boardController, Piece movingPiece, int targetX, int targetY)
        {
            Cell.State cellState = boardController.GetCellStateForPiece(targetX, targetY, movingPiece);

            if (cellState == Cell.State.Enemy)
            {
                Cell currentCell = boardController.GetCell(targetX, targetY);

                PieceKillCommand killCommand = new PieceKillCommand(boardController, currentCell.currentPiece);
                PieceMoveCommand moveCommand = new PieceMoveCommand(boardController, movingPiece, new Vector2Int(targetX, targetY));

                CommandContainer container = new CommandContainer(killCommand, moveCommand);
                MoveInfo move = new MoveInfo(currentCell, container);

                moves.Add(move);
            }

        }

        protected Vector3Int InvertVectorMoveByTeam(Vector3Int moveVec, ChessTeam.Type teamType)
        {
            return (teamType == ChessTeam.Type.White) ? moveVec : (moveVec * -1);
        }
    }
}
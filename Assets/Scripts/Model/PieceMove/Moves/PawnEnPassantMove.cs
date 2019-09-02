﻿using System.Collections.Generic;
using Chess2D.Commands;
using Chess2D.Controller;
using Chess2D.Model;
using Chess2D.Model.PieceMove;
using Chess2D.UI;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PawnEnPassantMove : PieceMoveAlgorithm {

        int whiteEnemyPawnRow = 3;
        int blackEnemyPawnRow = 4;

        public PawnEnPassantMove() {
            moveVector = new Vector3Int(0, 1, 0);
        }

        public override List<MoveInfo> GetAvailableMoves(Piece movingPiece, IBoardController boardController) {
            List<MoveInfo> moves = new List<MoveInfo>();

            int enemyRow = (movingPiece.teamType == ChessTeam.Type.White) ? blackEnemyPawnRow : whiteEnemyPawnRow;

            if (movingPiece.cellCoord.y == enemyRow) {
                Vector3Int pMoveVector = InvertVectorMoveByTeam(moveVector, movingPiece.teamType);

                FillCellMove(moves, boardController, movingPiece, -1, pMoveVector.y);
                FillCellMove(moves, boardController, movingPiece, 1, pMoveVector.y);
            }

            return moves;
        }

        private new void FillCellMove(List<MoveInfo> moves, IBoardController boardController, Piece movingPiece, int xOffset, int yOffset) {
            Vector2Int coord = movingPiece.cellCoord;

            Cell.State moveCellState = boardController.GetCellStateForPiece(coord.x + xOffset, coord.y + yOffset, movingPiece);
            Cell.State enemyPawnCellState = boardController.GetCellStateForPiece(coord.x + xOffset, coord.y, movingPiece);

            if (moveCellState == Cell.State.Free && enemyPawnCellState == Cell.State.Enemy) {
                Cell moveCell = boardController.GetCell(coord.x + xOffset, coord.y + yOffset);
                Cell enemyCell = boardController.GetCell(coord.x + xOffset, coord.y);

                Piece enemyPiece = enemyCell.currentPiece;

                if (enemyPiece.type == Piece.Type.Pawn && enemyPiece.isLastMoving) {
                    PieceKillCommand killCommand = new PieceKillCommand(boardController, enemyPiece);
                    PieceMoveCommand moveCommand = new PieceMoveCommand(boardController, movingPiece, moveCell.coord);

                    MoveInfo moveInfo = new MoveInfo(moveCell, new CommandContainer(killCommand, moveCommand));
                    moves.Add(moveInfo);
                }
            }
        }
    }

}

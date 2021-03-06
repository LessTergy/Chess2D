﻿using System.Collections.Generic;
using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model.PieceMove
{
    public class PawnKillMove : PieceMoveAlgorithm
    {
        protected override Vector3Int GetMoveVector()
        {
            return new Vector3Int(0, 0, 1);
        }

        public override List<MoveInfo> GetAvailableMoves(Piece movingPiece, IBoardController boardController)
        {
            List<MoveInfo> moves = new List<MoveInfo>();

            Vector3Int pMoveVector = InvertVectorMoveByTeam(moveVector, movingPiece.teamType);

            Vector2Int coord = movingPiece.cellCoord;

            FillKillMove(moves, boardController, movingPiece, coord.x - pMoveVector.z, coord.y + pMoveVector.z);
            FillKillMove(moves, boardController, movingPiece, coord.x + pMoveVector.z, coord.y + pMoveVector.z);

            return moves;
        }
    }
}
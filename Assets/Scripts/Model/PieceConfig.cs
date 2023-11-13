﻿using Chess2D.Model.PieceMove;
using Chess2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess2D.Model
{
    [CreateAssetMenu(fileName = "PiecePrefabBuilder", menuName = "Chess2D/Piece Prefab Builder")]
    public class PieceConfig : ScriptableObject
    {
        [Header("Prefab")]
        [SerializeField] private PieceView piecePrefab;
        [SerializeField] private List<PieceSpritePair> pieceSprites;

        [Space(10)]
        [Header("Colors")]
        [SerializeField] private Color whiteColor;
        [SerializeField] private Color blackColor;

        public PieceView CreatePieceView(PieceType type, TeamType teamType, Transform parent)
        {
            PieceView pieceView = Instantiate(piecePrefab, parent);
            Sprite sprite = GetSprite(type);
            Color color = GetColor(teamType);
            List<PieceMoveAlgorithm> moves = GetMoves(type);

            pieceView.Initialize(type, sprite, moves, teamType, color);
            return pieceView;
        }

        public Sprite GetSprite(PieceType type)
        {
            return GetPieceInfo(type).sprite;
        }

        public PieceSpritePair GetPieceInfo(PieceType type)
        {
            return pieceSprites.FirstOrDefault(p => p.type == type);
        }

        public Color GetColor(TeamType teamType)
        {
            Color color = (teamType == TeamType.White) ? whiteColor : blackColor;
            return color;
        }

        // TODO: to factory
        private List<PieceMoveAlgorithm> GetMoves(PieceType pieceType)
        {
            var moves = new List<PieceMoveAlgorithm>();

            if (pieceType == PieceType.Pawn)
            {
                moves.Add(new PawnNormalMove());
                moves.Add(new PawnKillMove());
                moves.Add(new PawnEnPassantMove());
            }
            else
            if (pieceType == PieceType.Rook)
            {
                moves.Add(new RookMove());
            }
            else
            if (pieceType == PieceType.Knight)
            {
                moves.Add(new KnightMove());
            }
            else
            if (pieceType == PieceType.Bishop)
            {
                moves.Add(new BishopMove());
            }
            else
            if (pieceType == PieceType.Queen)
            {
                moves.Add(new QueenMove());
            }
            else
            if (pieceType == PieceType.King)
            {
                moves.Add(new KingNormalMove());
                moves.Add(new KingCastlingMove());
            }

            return moves;
        }

    }

    [Serializable]
    public class PieceSpritePair
    {
        public PieceType type;
        public Sprite sprite;
    }
}
using Chess2D.Model.PieceMove;
using Chess2D.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess2D.Model
{
    [CreateAssetMenu(fileName = "PiecePrefabBuilder", menuName = "Chess2D/Piece Prefab Builder")]
    public class PiecePrefabBuilder : ScriptableObject
    {

        [Header("Prefab")]
        [SerializeField]
        private Piece piecePrefab;

        [SerializeField]
        private List<PiecePrefabPrefs> piecePreferenceList;

        [Space(10)]
        [Header("Colors")]
        public Color whiteColor;
        public Color blackColor;

        private Dictionary<Piece.Type, PiecePrefabPrefs> _piecesPrefsDict;

        public void Init()
        {
            _piecesPrefsDict = new Dictionary<Piece.Type, PiecePrefabPrefs>();

            foreach (PiecePrefabPrefs info in piecePreferenceList)
            {
                _piecesPrefsDict.Add(info.type, info);
            }
        }

        public Piece CreatePiece(Piece.Type type, ChessTeam.Type teamType)
        {
            Piece piece = Instantiate(piecePrefab);
            PiecePrefabPrefs prefs = _piecesPrefsDict[type];
            Color color = GetColor(teamType);

            piece.Initialize(prefs.type, prefs.sprite, GetMoves(type), teamType, color);
            return piece;
        }

        public PiecePrefabPrefs GetPrefabPrefs(Piece.Type type)
        {
            return _piecesPrefsDict[type];
        }

        public Color GetColor(ChessTeam.Type teamType)
        {
            Color color = (teamType == ChessTeam.Type.White) ? whiteColor : blackColor;
            return color;
        }

        private List<PieceMoveAlgorithm> GetMoves(Piece.Type pieceType)
        {
            var moves = new List<PieceMoveAlgorithm>();

            if (pieceType == Piece.Type.Pawn)
            {
                moves.Add(new PawnNormalMove());
                moves.Add(new PawnKillMove());
                moves.Add(new PawnEnPassantMove());
            }
            else
            if (pieceType == Piece.Type.Rook)
            {
                moves.Add(new RookMove());
            }
            else
            if (pieceType == Piece.Type.Knight)
            {
                moves.Add(new KnightMove());
            }
            else
            if (pieceType == Piece.Type.Bishop)
            {
                moves.Add(new BishopMove());
            }
            else
            if (pieceType == Piece.Type.Queen)
            {
                moves.Add(new QueenMove());
            }
            else
            if (pieceType == Piece.Type.King)
            {
                moves.Add(new KingNormalMove());
                moves.Add(new KingCastlingMove());
            }

            return moves;
        }

    }

    [Serializable]
    public class PiecePrefabPrefs
    {
        public Piece.Type type;
        public Sprite sprite;
    }

}
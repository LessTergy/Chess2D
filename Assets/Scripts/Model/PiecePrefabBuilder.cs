using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lesstergy.Chess2D {

    [CreateAssetMenu(fileName = "PiecePrefabBuilder", menuName = "Chess2D/Piece Prefab Builder")]
    public class PiecePrefabBuilder : ScriptableObject {
        
        [Header("Prefab")]
        [SerializeField]
        private Piece piecePrefab;
        [SerializeField]
        private List<PiecePrefs> piecePreferenceList;

        [Space(10)]
        [Header("Colors")]
        public Color whiteColor;
        public Color blackColor;

        private Dictionary<Piece.Type, PiecePrefs> piecesPrefsDict;
        
        public void Init() {
            piecesPrefsDict = new Dictionary<Piece.Type, PiecePrefs>();

            foreach (var info in piecePreferenceList) {
                piecesPrefsDict.Add(info.type, info);
            }
        }

        public Piece CreatePiece(Piece.Type type, ChessTeam.Type teamType) {
            Piece piece = Instantiate(piecePrefab);
            PiecePrefs prefs = piecesPrefsDict[type];
            Color color = (teamType == ChessTeam.Type.White) ? whiteColor : blackColor;

            piece.Initialize(prefs.type, prefs.sprite, GetMoves(type), teamType, color);
            return piece;
        }


        private List<PieceMoveAlgorithm> GetMoves(Piece.Type pieceType) {
            List<PieceMoveAlgorithm> moves = new List<PieceMoveAlgorithm>();
            
            if (pieceType == Piece.Type.Pawn) {
                moves.Add(new PawnNormalMove());
                moves.Add(new PawnKillMove());
            } else
            if (pieceType == Piece.Type.Rook) {
                moves.Add(new RookMove());
            } else
            if (pieceType == Piece.Type.Knight) {
                moves.Add(new KnightMove());
            } else
            if (pieceType == Piece.Type.Bishop) {
                moves.Add(new BishopMove());
            } else
            if (pieceType == Piece.Type.Queen) {
                moves.Add(new QueenMove());
            } else
            if (pieceType == Piece.Type.King) {
                moves.Add(new KingNormalMove());
                moves.Add(new KingCastlingMove());
            }

            return moves;
        }

        
        [Serializable]
        public class PiecePrefs {
            public Piece.Type type;
            public Sprite sprite;
        }
    }

}

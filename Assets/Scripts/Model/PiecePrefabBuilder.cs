using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lesstergy.Chess2D {

    [CreateAssetMenu(fileName = "PiecePrefabBuilder", menuName = "Chess2D/Piece Prefab Builder")]
    public class PiecePrefabBuilder : ScriptableObject {

        [SerializeField]
        private Piece piecePrefab;

        [SerializeField]
        private List<PiecePrefs> piecePreferenceList;
        private Dictionary<Piece.Type, PiecePrefs> piecesPrefsDict;
        
        public void Init() {
            piecesPrefsDict = new Dictionary<Piece.Type, PiecePrefs>();

            foreach (var info in piecePreferenceList) {
                piecesPrefsDict.Add(info.type, info);
            }
        }

        public Piece CreatePiece(Piece.Type type) {
            Piece piece = Instantiate(piecePrefab);

            PiecePrefs prefs = piecesPrefsDict[type];
            piece.Initialize(prefs.type, prefs.sprite);
            return piece;
        }

        
        [Serializable]
        public class PiecePrefs {
            public Piece.Type type;
            public Sprite sprite;
        }
    }

}

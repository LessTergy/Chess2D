using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Lesster.Chess2D {

    [CreateAssetMenu(fileName = "PiecePrefabContainer", menuName = "Chess2D/PiecePrefabContainer")]
    public class PiecePrefabContainer : ScriptableObject {

        [SerializeField]
        private List<PiecePair> pieces;
        private Dictionary<Piece.Type, Piece> piecesPrefabDict;
        
        public void Init() {
            piecesPrefabDict = new Dictionary<Piece.Type, Piece>();

            foreach (var piecePair in pieces) {
                piecesPrefabDict.Add(piecePair.type, piecePair.prefab);
            }
        }

        public Piece CreatePiece(Piece.Type type) {
            return Instantiate(piecesPrefabDict[type]);
        }
        
        [Serializable]
        public class PiecePair {
            public Piece.Type type;
            public Piece prefab;
        }
    }

}

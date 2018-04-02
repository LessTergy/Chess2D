using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class ChessTeam {

        public enum Type {
            White,
            Black
        }

        public readonly Type type;
        public List<Piece> pieces = new List<Piece>();

        public Piece king { get; private set; }

        public ChessTeam(Type type) {
            this.type = type;
        }

        public void SetKing(Piece piece) {
            king = piece;
        }
    }

}

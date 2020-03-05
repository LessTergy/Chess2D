using Chess2D.UI;
using System.Collections.Generic;

namespace Chess2D.Model
{
    public class ChessTeam
    {
        public enum Type
        {
            White,
            Black
        }

        public readonly Type type;
        public List<Piece> pieces = new List<Piece>();

        public Piece king { get; private set; }

        public ChessTeam(Type type)
        {
            this.type = type;
        }

        public void SetKing(Piece piece)
        {
            king = piece;
        }

        public void SetInteractive(bool value)
        {
            foreach (Piece piece in pieces)
            {
                piece.isInteractive = value;
            }
        }

        public void ResetLastMove()
        {
            foreach (Piece piece in pieces)
            {
                piece.isLastMoving = false;
            }
        }
    }
}
using Chess2D.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Chess2D.Model
{
    public class PlayerModel
    {
        public readonly PlayerType type;
        public readonly List<PieceView> pieces = new();

        public PieceView King { get; private set; }

        public PlayerModel(PlayerType type)
        {
            this.type = type;
        }

        public void SetKing(PieceView pieceView)
        {
            if (pieceView.Type != PieceType.King)
            {
                Debug.LogError("PieceView is not King type");
                return;
            }
            King = pieceView;
        }

        public void SetInteractive(bool value)
        {
            foreach (PieceView piece in pieces)
            {
                piece.IsInteractive = value;
            }
        }

        public void ResetLastMove()
        {
            foreach (PieceView piece in pieces)
            {
                piece.isLastMoving = false;
            }
        }
    }
}
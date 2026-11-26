using System.Collections.Generic;

namespace Chess2D.Model
{
    internal sealed class PlayerModel
    {
        public List<PieceState> Pieces { get; } = new();
        public PieceState King { get; set; }
    }
}

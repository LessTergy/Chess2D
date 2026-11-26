namespace Chess2D.Model
{
    public sealed class PieceState
    {
        public PieceType Type { get; internal set; }
        public PlayerType PlayerType { get; }
        public Square Square { get; internal set; }
        public bool HasMoved { get; internal set; }

        public PieceState(PieceType type, PlayerType playerType, Square square)
        {
            Type = type;
            PlayerType = playerType;
            Square = square;
        }
    }
}

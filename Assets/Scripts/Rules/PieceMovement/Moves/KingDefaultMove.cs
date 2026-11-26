namespace Chess2D.PieceMovement
{
    internal sealed class KingDefaultMove : PieceMoveAlgorithm
    {
        protected override (int Horizontal, int Vertical, int Diagonal) Range => (1, 1, 1);
    }
}

namespace Chess2D.PieceMovement
{
    internal sealed class RookMove : PieceMoveAlgorithm
    {
        protected override (int Horizontal, int Vertical, int Diagonal) Range =>
            (GameConstants.FinishIndex, GameConstants.FinishIndex, 0);
    }
}

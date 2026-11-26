namespace Chess2D.PieceMovement
{
    internal sealed class BishopMove : PieceMoveAlgorithm
    {
        protected override (int Horizontal, int Vertical, int Diagonal) Range => (0, 0, GameConstants.FinishIndex);
    }
}

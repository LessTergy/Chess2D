namespace Chess2D.PieceMovement
{
    internal sealed class QueenMove : PieceMoveAlgorithm
    {
        protected override (int Horizontal, int Vertical, int Diagonal) Range =>
            (GameConstants.FinishIndex, GameConstants.FinishIndex, GameConstants.FinishIndex);
    }
}

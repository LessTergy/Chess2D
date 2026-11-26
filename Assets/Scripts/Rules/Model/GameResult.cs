namespace Chess2D.Model
{
    public enum GameResult
    {
        Ongoing,
        Checkmate,
        Stalemate,
        InsufficientMaterial,
        ThreefoldRepetition,
        FivefoldRepetition,
        FiftyMoveRule,
        SeventyFiveMoveRule,
        DrawByAgreement
    }
}

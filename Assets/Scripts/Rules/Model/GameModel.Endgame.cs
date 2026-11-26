using System.Collections.Generic;
using System.Linq;
using Chess2D.PieceMovement;

namespace Chess2D.Model
{
    public sealed partial class GameModel
    {
        private readonly Dictionary<string, int> _repetitions = new();
        private int _halfMoves;
        private int _completedMoves;
        private GameResult _claimableDraw;

        public GameResult Result { get; private set; }
        public PieceState CurrentKing => _players[(int)CurrentPlayer].King;
        public bool CanPlay => Result == GameResult.Ongoing && PendingPromotion == null;
        public PlayerType? DrawOfferBy { get; private set; }
        public bool CanOfferDraw => CanPlay && _completedMoves > 0 && DrawOfferBy == null;
        public bool CanClaimDraw => CanPlay && _claimableDraw != GameResult.Ongoing;
        public bool ClaimOnNextMove { get; private set; }
        public bool CanClaimWithMove => CanPlay && !ClaimOnNextMove &&
            (_halfMoves >= 99 || _repetitions.Values.Any(count => count >= 2));

        public bool TryOfferDraw()
        {
            if (!CanOfferDraw)
            {
                return false;
            }

            DrawOfferBy = CurrentPlayer;
            Changed?.Invoke();
            return true;
        }

        public bool TryAcceptDraw()
        {
            if (!CanPlay || DrawOfferBy == null || DrawOfferBy == CurrentPlayer)
            {
                return false;
            }

            Result = GameResult.DrawByAgreement;
            Changed?.Invoke();
            return true;
        }

        public bool TryClaimDraw()
        {
            if (CanClaimDraw)
            {
                Result = _claimableDraw;
            }
            else if (CanClaimWithMove)
            {
                ClaimOnNextMove = true;
            }
            else
            {
                return false;
            }

            Changed?.Invoke();
            return true;
        }

        private void FinishTurn()
        {
            if (DrawOfferBy != CurrentPlayer)
            {
                DrawOfferBy = null;
            }

            CurrentPlayer = OpponentOf(CurrentPlayer);
            _completedMoves++;
            EvaluatePosition();
            if (ClaimOnNextMove && Result == GameResult.Ongoing)
            {
                Result = _claimableDraw;
                if (Result == GameResult.Ongoing)
                {
                    DrawOfferBy = OpponentOf(CurrentPlayer);
                }
            }

            ClaimOnNextMove = false;
        }

        private void EvaluatePosition()
        {
            var hasMove = Pieces.Where(piece => piece.PlayerType == CurrentPlayer)
                .Any(piece => GetMoves(piece).Any(move => !LeavesKingInCheck(move.Command, CurrentPlayer)));
            if (!hasMove)
            {
                Result = IsKingInCheck(CurrentPlayer) ? GameResult.Checkmate : GameResult.Stalemate;
                return;
            }

            if (HasInsufficientMaterial())
            {
                Result = GameResult.InsufficientMaterial;
                return;
            }

            var key = GetPositionKey();
            _repetitions.TryGetValue(key, out var count);
            _repetitions[key] = ++count;
            if (count >= 5)
            {
                Result = GameResult.FivefoldRepetition;
            }
            else if (_halfMoves >= 150)
            {
                Result = GameResult.SeventyFiveMoveRule;
            }

            _claimableDraw = count >= 3 ? GameResult.ThreefoldRepetition :
                _halfMoves >= 100 ? GameResult.FiftyMoveRule : GameResult.Ongoing;
        }

        private bool HasInsufficientMaterial()
        {
            var material = Pieces.Where(piece => piece.Type != PieceType.King).ToArray();
            if (material.Length == 0 || (material.Length == 1 && material[0].Type == PieceType.Knight))
            {
                return true;
            }

            var squareColor = (material[0].Square.X + material[0].Square.Y) % 2;
            return material.All(piece => piece.Type == PieceType.Bishop &&
                (piece.Square.X + piece.Square.Y) % 2 == squareColor);
        }

        private string GetPositionKey()
        {
            const int squares = GameConstants.CellCount * GameConstants.CellCount;
            var key = new char[squares + 3];
            foreach (var piece in Pieces)
            {
                var index = piece.Square.Y * GameConstants.CellCount + piece.Square.X;
                key[index] = (char)(1 + (int)piece.Type * 2 + (int)piece.PlayerType);
            }

            key[squares] = (char)CurrentPlayer;
            var rights = 0;
            foreach (var player in _players)
            {
                var king = player.King;
                var shift = (int)king.PlayerType * 2;
                if (KingCastlingMove.GetCastlingRook(this, king, GameConstants.StartIndex) != null)
                {
                    rights |= 1 << shift;
                }
                if (KingCastlingMove.GetCastlingRook(this, king, GameConstants.FinishIndex) != null)
                {
                    rights |= 2 << shift;
                }
            }

            key[squares + 1] = (char)rights;
            // An en passant right distinguishes positions only if the capture is legal.
            if (LastDoubleStepPawn != null && Pieces.Any(piece => piece.PlayerType == CurrentPlayer &&
                piece.Type == PieceType.Pawn && EnPassant.GetAvailableMoves(piece, this)
                    .Any(move => !LeavesKingInCheck(move.Command, CurrentPlayer))))
            {
                key[squares + 2] = (char)(1 + LastDoubleStepPawn.Square.X);
            }

            return new string(key);
        }
    }
}

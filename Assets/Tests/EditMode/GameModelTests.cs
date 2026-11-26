using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Chess2D.Model;
using static Chess2D.Model.PieceType;
using static Chess2D.Model.PlayerType;

namespace Chess2D.Tests
{
    public class GameModelTests
    {
        [Test]
        public void OpeningHasTwentyCandidateMovesAndWhiteMovesFirst()
        {
            var game = StartingPosition();

            Assert.That(game.Pieces.Count(), Is.EqualTo(32));
            Assert.That(game.Pieces.Sum(piece => game.GetCandidateDestinations(piece.Square).Count), Is.EqualTo(20));
            Assert.That(TryMove(game, "e7", "e5"), Is.False);
            Assert.That(TryMove(game, "e2", "e2"), Is.False);
            Assert.That(TryMove(game, "e2", "e4"), Is.True);
            Assert.That(game.CurrentPlayer, Is.EqualTo(Black));
            Assert.That(At(game, "e4").Type, Is.EqualTo(Pawn));
            Assert.That(At(game, "e2"), Is.Null);
        }

        [TestCase(Rook, "d7", "b6")]
        [TestCase(Bishop, "b6", "d6")]
        [TestCase(Queen, "g7", "e6")]
        [TestCase(Knight, "e6", "d6")]
        public void PieceAcceptsItsOwnGeometry(PieceType type, string valid, string invalid)
        {
            var game = WithKings((type, White, "d4"), (Pawn, Black, "a7"));

            Assert.That(TryMove(game, "d4", invalid), Is.False);
            Assert.That(TryMove(game, "d4", valid), Is.True);
        }

        [Test]
        public void KingMovesOneSquareAndCannotMoveNextToOpponent()
        {
            var game = Position((King, White, "e4"), (King, Black, "e6"), (Pawn, Black, "a7"));

            Assert.That(TryMove(game, "e4", "e2"), Is.False);
            Assert.That(TryMove(game, "e4", "e5"), Is.False);
            Assert.That(TryMove(game, "e4", "f4"), Is.True);
        }

        [Test]
        public void SlidingPieceStopsAtFriendlyPieceAndAtCapture()
        {
            var game = WithKings((Rook, White, "d4"), (Pawn, White, "d5"), (Knight, Black, "f4"));
            var captured = At(game, "f4");

            Assert.That(TryMove(game, "d4", "d5"), Is.False);
            Assert.That(TryMove(game, "d4", "d6"), Is.False);
            Assert.That(TryMove(game, "d4", "g4"), Is.False);
            Assert.That(TryMove(game, "d4", "f4"), Is.True);
            Assert.That(At(game, "f4").Type, Is.EqualTo(Rook));
            CollectionAssert.DoesNotContain(game.Pieces, captured);
        }

        [Test]
        public void KnightJumpsOverAdjacentPieces()
        {
            var game = WithKings((Knight, White, "b1"), (Pawn, White, "b2"), (Pawn, White, "c2"));

            Assert.That(TryMove(game, "b1", "c3"), Is.True);
            Assert.That(At(game, "c3").Type, Is.EqualTo(Knight));
        }

        [Test]
        public void PawnCannotJumpOrTakeForwardAndCapturesDiagonally()
        {
            var game = WithKings((Pawn, White, "e2"), (Pawn, Black, "e3"), (Knight, Black, "f3"));

            Assert.That(TryMove(game, "e2", "e3"), Is.False);
            Assert.That(TryMove(game, "e2", "e4"), Is.False);
            Assert.That(TryMove(game, "e2", "d3"), Is.False);
            Assert.That(TryMove(game, "e2", "f3"), Is.True);
            Assert.That(At(game, "f3").PlayerType, Is.EqualTo(White));
        }

        [Test]
        public void CaptureExposingOwnKingRestoresBothPiecesAndTurn()
        {
            var game = Position((King, White, "e1"), (King, Black, "a8"),
                (Rook, White, "e2"), (Rook, Black, "e8"), (Knight, Black, "f2"));
            var defender = At(game, "e2");
            var target = At(game, "f2");

            Assert.That(game.IsKingInCheck(White), Is.False);
            Assert.That(TryMove(game, "e2", "f2"), Is.False);
            Assert.That(At(game, "e2"), Is.SameAs(defender));
            Assert.That(At(game, "f2"), Is.SameAs(target));
            Assert.That(defender.HasMoved, Is.False);
            Assert.That(game.CurrentPlayer, Is.EqualTo(White));
        }

        [Test]
        public void KingCannotEnterSquareAttackedByPinnedPiece()
        {
            var game = Position((King, White, "e1"), (King, Black, "f8"),
                (Rook, White, "f1"), (Bishop, Black, "f3"));

            Assert.That(TryMove(game, "e1", "e2"), Is.False);
            Assert.That(At(game, "e1").Type, Is.EqualTo(King));
        }

        [Test]
        public void KingInCheckMustEscapeBeforeAnotherPieceCanMove()
        {
            var game = Position((King, White, "e1"), (King, Black, "a8"),
                (Rook, Black, "e8"), (Pawn, White, "a2"));

            Assert.That(game.IsKingInCheck(White), Is.True);
            Assert.That(TryMove(game, "a2", "a3"), Is.False);
            Assert.That(TryMove(game, "e1", "d1"), Is.True);
            Assert.That(game.IsKingInCheck(White), Is.False);
        }

        [TestCase("c1", "a1", "d1")]
        [TestCase("g1", "h1", "f1")]
        public void CastlingMovesKingAndRookAndRecordsBothMoves(string target, string rookFrom, string rookTo)
        {
            var game = Position((King, White, "e1"), (King, Black, "e8"),
                (Rook, White, "a1"), (Rook, White, "h1"));
            var king = At(game, "e1");
            var rook = At(game, rookFrom);

            Assert.That(game.GetCandidateDestinations(SquareAt("e1")), Does.Contain(SquareAt(target)));
            Assert.That(At(game, "e1"), Is.SameAs(king));
            Assert.That(At(game, rookFrom), Is.SameAs(rook));
            Assert.That(king.HasMoved, Is.False);
            Assert.That(rook.HasMoved, Is.False);
            Assert.That(TryMove(game, "e1", target), Is.True);
            Assert.That(At(game, target), Is.SameAs(king));
            Assert.That(At(game, rookTo), Is.SameAs(rook));
            Assert.That(At(game, "e1"), Is.Null);
            Assert.That(At(game, rookFrom), Is.Null);
            Assert.That(king.HasMoved && rook.HasMoved, Is.True);
            Assert.That(game.CurrentPlayer, Is.EqualTo(Black));
        }

        [TestCase("e8")]
        [TestCase("f8")]
        [TestCase("g8")]
        public void CastlingRejectsAttackOnAnySquareOfKingsPath(string attackerSquare)
        {
            var game = Position((King, White, "e1"), (King, Black, "a8"),
                (Rook, White, "h1"), (Rook, Black, attackerSquare));

            CollectionAssert.DoesNotContain(game.GetCandidateDestinations(SquareAt("e1")), SquareAt("g1"));
            Assert.That(TryMove(game, "e1", "g1"), Is.False);
            Assert.That(At(game, "e1").HasMoved, Is.False);
            Assert.That(At(game, "h1").HasMoved, Is.False);
        }

        [Test]
        public void EnemyPawnDiagonalAttackPreventsCastlingThroughEmptySquare()
        {
            var game = Position((King, White, "e1"), (King, Black, "a8"),
                (Rook, White, "h1"), (Pawn, Black, "e2"));

            Assert.That(TryMove(game, "e1", "g1"), Is.False);
        }

        [Test]
        public void LongCastlingAllowsAttackOnBFileButRequiresBFileEmpty()
        {
            var clear = Position((King, White, "e1"), (King, Black, "h8"),
                (Rook, White, "a1"), (Rook, Black, "b8"));
            var blocked = Position((King, White, "e1"), (King, Black, "h8"),
                (Rook, White, "a1"), (Knight, White, "b1"));

            Assert.That(TryMove(clear, "e1", "c1"), Is.True);
            Assert.That(TryMove(blocked, "e1", "c1"), Is.False);
        }

        [TestCase("e1", "e2")]
        [TestCase("h1", "h2")]
        public void ReturningKingOrRookDoesNotRestoreCastling(string start, string away)
        {
            var game = Position((King, White, "e1"), (King, Black, "a8"), (Rook, White, "h1"));

            Play(game, (start, away), ("a8", "a7"), (away, start), ("a7", "a8"));

            Assert.That(TryMove(game, "e1", "g1"), Is.False);
        }

        [Test]
        public void BlackCanCastleLong()
        {
            var game = Position((King, White, "h1"), (King, Black, "e8"),
                (Pawn, White, "h2"), (Rook, Black, "a8"));
            Play(game, ("h2", "h3"));

            Assert.That(TryMove(game, "e8", "c8"), Is.True);
            Assert.That(At(game, "d8").Type, Is.EqualTo(Rook));
        }

        [Test]
        public void EnPassantIsRejectedAfterASingleStepInTheOpening()
        {
            var game = StartingPosition();
            Play(game, ("e2", "e4"), ("a7", "a6"), ("e4", "e5"), ("d7", "d6"), ("g1", "f3"), ("d6", "d5"));

            CollectionAssert.DoesNotContain(game.GetCandidateDestinations(SquareAt("e5")), SquareAt("d6"));
            Assert.That(TryMove(game, "e5", "d6"), Is.False);
            Assert.That(At(game, "e5").PlayerType, Is.EqualTo(White));
            Assert.That(At(game, "d5").PlayerType, Is.EqualTo(Black));
        }

        [Test]
        public void EnPassantRemovesThePawnFromTheAdjacentSquare()
        {
            var game = StartingPosition();
            Play(game, ("e2", "e4"), ("a7", "a6"), ("e4", "e5"), ("d7", "d5"));

            Assert.That(TryMove(game, "e5", "d6"), Is.True);
            Assert.That(At(game, "d6").PlayerType, Is.EqualTo(White));
            Assert.That(At(game, "d5"), Is.Null);
        }

        [Test]
        public void BlackCanCaptureEnPassant()
        {
            var game = WithKings((Pawn, White, "d2"), (Pawn, Black, "e4"));
            Play(game, ("d2", "d4"));

            Assert.That(TryMove(game, "e4", "d3"), Is.True);
            Assert.That(At(game, "d3").PlayerType, Is.EqualTo(Black));
            Assert.That(At(game, "d4"), Is.Null);
        }

        [Test]
        public void EnPassantExpiresAfterTheReply()
        {
            var game = StartingPosition();
            Play(game, ("e2", "e4"), ("a7", "a6"), ("e4", "e5"), ("d7", "d5"), ("g1", "f3"), ("a6", "a5"));

            Assert.That(TryMove(game, "e5", "d6"), Is.False);
        }

        [Test]
        public void EnPassantExposingOwnKingRevertsWithoutConsumingTheReply()
        {
            var game = Position((King, White, "a5"), (King, Black, "h8"), (Rook, Black, "h5"),
                (Knight, White, "b1"), (Pawn, White, "e5"), (Pawn, Black, "d7"));
            Play(game, ("b1", "a3"), ("d7", "d5"));

            Assert.That(TryMove(game, "e5", "d6"), Is.False);
            Assert.That(At(game, "e5").PlayerType, Is.EqualTo(White));
            Assert.That(At(game, "d5").PlayerType, Is.EqualTo(Black));
            Assert.That(At(game, "d6"), Is.Null);
            Assert.That(game.GetCandidateDestinations(SquareAt("e5")), Does.Contain(SquareAt("d6")));
            Assert.That(game.CurrentPlayer, Is.EqualTo(White));
        }

        [TestCase(Queen)]
        [TestCase(Rook)]
        [TestCase(Knight)]
        [TestCase(Bishop)]
        public void PromotionWaitsForAValidChoiceBeforePassingTheTurn(PieceType choice)
        {
            var game = WithKings((Pawn, White, "d7"));
            var pawn = At(game, "d7");
            Assert.That(game.TryPromote(choice), Is.False);

            Play(game, ("d7", "d8"));
            Assert.That(game.PendingPromotion, Is.SameAs(pawn));
            Assert.That(game.CurrentPlayer, Is.EqualTo(White));
            Assert.That(TryMove(game, "a1", "b1"), Is.False);
            Assert.That(TryMove(game, "h8", "h7"), Is.False);
            Assert.That(game.TryPromote(King), Is.False);
            Assert.That(game.TryPromote(Pawn), Is.False);
            Assert.That(game.TryPromote(choice), Is.True);

            Assert.That(At(game, "d8"), Is.SameAs(pawn));
            Assert.That(pawn.Type, Is.EqualTo(choice));
            Assert.That(game.PendingPromotion, Is.Null);
            Assert.That(game.CurrentPlayer, Is.EqualTo(Black));
        }

        [Test]
        public void BlackPromotesAfterACapture()
        {
            var game = WithKings((Pawn, Black, "e2"), (Knight, White, "d1"));
            Play(game, ("a1", "b1"), ("e2", "d1"));

            Assert.That(game.PendingPromotion.PlayerType, Is.EqualTo(Black));
            Assert.That(game.TryPromote(Knight), Is.True);
            Assert.That(At(game, "d1").Type, Is.EqualTo(Knight));
            Assert.That(At(game, "d1").PlayerType, Is.EqualTo(Black));
            Assert.That(game.CurrentPlayer, Is.EqualTo(White));
        }

        [Test]
        public void PositionRejectsMissingKingsAndDuplicateOrOutOfBoundsSquares()
        {
            Assert.Throws<ArgumentException>(() => Position((King, White, "e1")));
            Assert.Throws<ArgumentException>(() => WithKings((King, White, "e1")));
            Assert.Throws<ArgumentException>(() => WithKings((Pawn, White, "a1")));
            Assert.Throws<ArgumentException>(() => WithKings((Pawn, White, "i2")));
        }

        [Test]
        public void StartingPiecesCanBeReusedForIndependentGames()
        {
            var pieces = new[] { new PieceState(King, White, SquareAt("a1")), new PieceState(King, Black, SquareAt("h8")),
                new PieceState(Pawn, White, SquareAt("e2")) };
            var first = new GameModel(pieces);
            var second = new GameModel(pieces);

            Assert.That(TryMove(first, "a1", "b1"), Is.True);
            Assert.That(At(second, "a1").Type, Is.EqualTo(King));
            Assert.That(second.CurrentPlayer, Is.EqualTo(White));
        }

        [Test]
        public void FoolsMateEndsTheGameAndRejectsFurtherInput()
        {
            var game = StartingPosition();
            Play(game, ("f2", "f3"), ("e7", "e5"), ("g2", "g4"), ("d8", "h4"));

            Assert.That(game.Result, Is.EqualTo(GameResult.Checkmate));
            Assert.That(game.CurrentKing.PlayerType, Is.EqualTo(White));
            Assert.That(game.GetCandidateDestinations(SquareAt("e2")), Is.Empty);
            Assert.That(TryMove(game, "e2", "e4"), Is.False);
            Assert.That(game.TryOfferDraw() || game.TryClaimDraw(), Is.False);
        }

        [TestCase("c2", GameResult.Stalemate)]
        [TestCase("b2", GameResult.Checkmate)]
        public void TerminalStartingPositionsAreRecognized(string queenSquare, GameResult expected)
        {
            var game = Position((King, White, "a1"), (King, Black, "c3"), (Queen, Black, queenSquare));
            Assert.That(game.Result, Is.EqualTo(expected));
        }

        [Test]
        public void StalemateAfterAMoveHasNoWinner()
        {
            var game = Position((King, White, "c6"), (King, Black, "a8"), (Queen, White, "d7"));
            Play(game, ("d7", "c7"));
            Assert.That(game.Result, Is.EqualTo(GameResult.Stalemate));
            Assert.That(game.IsKingInCheck(Black), Is.False);
            Assert.That(TryMove(game, "a8", "a7"), Is.False);
        }

        [TestCase(Queen, GameResult.Checkmate)]
        [TestCase(Rook, GameResult.Checkmate)]
        [TestCase(Bishop, GameResult.InsufficientMaterial)]
        public void ResultDependsOnTheChosenPromotion(PieceType choice, GameResult expected)
        {
            var game = Position((King, White, "b6"), (King, Black, "a8"), (Pawn, White, "c7"));
            Play(game, ("c7", "c8"));
            Assert.That(game.Result, Is.EqualTo(GameResult.Ongoing));
            Assert.That(game.CanPlay, Is.False);
            Assert.That(game.TryPromote(choice), Is.True);
            Assert.That(game.Result, Is.EqualTo(expected));
        }

        [Test]
        public void EnPassantCanBeTheOnlyLegalReply()
        {
            var game = Position((King, White, "a1"), (King, Black, "c2"), (Bishop, Black, "b3"),
                (Pawn, White, "e4"), (Pawn, Black, "e6"), (Pawn, Black, "d7"));
            Play(game, ("e4", "e5"), ("d7", "d5"));
            Assert.That(game.Result, Is.EqualTo(GameResult.Ongoing));
            Play(game, ("e5", "d6"));
        }

        [Test]
        public void DeadMaterialAndMaterialThatCanStillMate()
        {
            Assert.That(WithKings().Result, Is.EqualTo(GameResult.InsufficientMaterial));
            Assert.That(WithKings((Knight, White, "e3")).Result, Is.EqualTo(GameResult.InsufficientMaterial));
            Assert.That(WithKings((Bishop, White, "e3"), (Bishop, Black, "f4")).Result,
                Is.EqualTo(GameResult.InsufficientMaterial));
            Assert.That(WithKings((Bishop, White, "e3"), (Bishop, Black, "e4")).Result, Is.EqualTo(GameResult.Ongoing));
            Assert.That(WithKings((Knight, White, "e3"), (Knight, White, "e4")).Result, Is.EqualTo(GameResult.Ongoing));
        }

        [TestCase(true, GameResult.ThreefoldRepetition)]
        [TestCase(false, GameResult.FivefoldRepetition)]
        public void ThreefoldIsClaimableAndFivefoldIsAutomatic(bool claim, GameResult expected)
        {
            var game = StartingPosition();
            for (var cycle = 1; cycle <= (claim ? 2 : 4); cycle++)
            {
                Play(game, ("g1", "f3"), ("g8", "f6"), ("f3", "g1"), ("f6", "g8"));
                if (cycle == 2)
                {
                    Assert.That(game.Result, Is.EqualTo(GameResult.Ongoing));
                    Assert.That(game.CanClaimDraw, Is.True);
                }
            }
            if (claim)
            {
                Assert.That(game.TryClaimDraw(), Is.True);
            }
            Assert.That(game.Result, Is.EqualTo(expected));
        }

        [Test]
        public void ClaimCanBeAnnouncedBeforeTheRepeatedPosition()
        {
            var game = StartingPosition();
            Play(game, ("g1", "f3"), ("g8", "f6"), ("f3", "g1"), ("f6", "g8"),
                ("g1", "f3"), ("g8", "f6"), ("f3", "g1"));
            Assert.That(game.CanClaimDraw, Is.False);
            Assert.That(game.TryClaimDraw(), Is.True);
            Assert.That(TryMove(game, "f6", "f4"), Is.False);
            Assert.That(game.ClaimOnNextMove, Is.True);
            Play(game, ("f6", "g8"));
            Assert.That(game.Result, Is.EqualTo(GameResult.ThreefoldRepetition));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void OnlyLegalEnPassantDistinguishesRepeatedPositions(bool pinned)
        {
            var pieces = new List<(PieceType, PlayerType, string)>
            {
                (King, White, "a5"), (King, Black, "h8"), (Pawn, White, "e5"), (Pawn, Black, "d7"),
                (Knight, White, "b1"), (Knight, Black, "b8")
            };
            if (pinned)
            {
                pieces.Add((Rook, Black, "h5"));
            }
            var game = Position(pieces.ToArray());
            Play(game, ("b1", "a3"), ("d7", "d5"));
            for (var cycle = 0; cycle < 2; cycle++)
            {
                Play(game, ("a3", "b1"), ("b8", "a6"), ("b1", "a3"), ("a6", "b8"));
            }
            Assert.That(game.CanClaimDraw, Is.EqualTo(pinned));
        }

        [Test]
        public void LosingCastlingRightsDistinguishesRepeatedPositions()
        {
            var game = Position((King, White, "e1"), (King, Black, "a8"), (Rook, White, "h1"));
            for (var cycle = 0; cycle < 2; cycle++)
            {
                Play(game, ("h1", "h2"), ("a8", "a7"), ("h2", "h1"), ("a7", "a8"));
            }
            Assert.That(game.CanClaimDraw, Is.False);
        }

        [TestCase(99, GameResult.FiftyMoveRule)]
        [TestCase(100, GameResult.FiftyMoveRule)]
        [TestCase(150, GameResult.SeventyFiveMoveRule)]
        [TestCase(150, GameResult.Checkmate)]
        public void QuietMoveLimitsAndMatePrecedence(int plies, GameResult expected)
        {
            var game = QuietPosition();
            PlayQuietMoves(game, expected == GameResult.Checkmate ? plies - 1 : plies);
            if (expected == GameResult.Checkmate)
            {
                Play(game, ("b6", "b2"));
            }
            else if (expected == GameResult.FiftyMoveRule)
            {
                Assert.That(game.CanClaimDraw, Is.EqualTo(plies == 100));
                Assert.That(game.TryClaimDraw(), Is.True);
                if (plies == 99)
                {
                    Play(game, ("h2", "f1"));
                }
            }
            Assert.That(game.Result, Is.EqualTo(expected));
        }

        [TestCase("a3", "a4")]
        [TestCase("b6", "a6")]
        public void PawnMoveOrCaptureResetsTheQuietMoveClock(string from, string to)
        {
            var game = QuietPosition((Pawn, White, "a3"), (Pawn, White, "a6"));
            PlayQuietMoves(game, from == "a3" ? 100 : 101);
            Assert.That(game.CanClaimDraw, Is.True);
            Play(game, (from, to));
            Assert.That(game.CanClaimDraw, Is.False);
        }

        private static GameModel QuietPosition(params (PieceType, PlayerType, string)[] extra) =>
            Position(new[] { (King, White, "a1"), (King, Black, "c2"), (Queen, Black, "b6"),
                (Knight, White, "c5"), (Knight, Black, "f1") }.Concat(extra).ToArray());

        private static void PlayQuietMoves(GameModel game, int plies)
        {
            // Different circuit lengths keep every position below five occurrences for 150 plies.
            var white = new[] { "c5", "d7", "e5", "c6", "e7", "d5", "c7", "e6" };
            var black = new[] { "f1", "g3", "h1", "f2", "h3", "f4", "g2", "h4", "f3", "h2" };
            for (var ply = 0; ply < plies; ply++)
            {
                var route = ply % 2 == 0 ? white : black;
                Play(game, (route[ply / 2 % route.Length], route[(ply / 2 + 1) % route.Length]));
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void OpponentCanAcceptAnOfferOrDeclineByMoving(bool accept)
        {
            var game = StartingPosition();
            Play(game, ("e2", "e4"));
            Assert.That(game.TryOfferDraw(), Is.True);
            Assert.That(game.TryAcceptDraw(), Is.False);
            Play(game, ("e7", "e5"));
            if (accept)
            {
                Assert.That(game.TryAcceptDraw(), Is.True);
                Assert.That(game.Result, Is.EqualTo(GameResult.DrawByAgreement));
                Assert.That(TryMove(game, "g1", "f3"), Is.False);
            }
            else
            {
                Play(game, ("g1", "f3"));
                Assert.That(game.TryAcceptDraw(), Is.False);
                Assert.That(game.DrawOfferBy, Is.Null);
            }
        }

        private static GameModel Position(params (PieceType Type, PlayerType Side, string Square)[] pieces) =>
            new GameModel(pieces.Select(piece => new PieceState(piece.Type, piece.Side, SquareAt(piece.Square))));

        private static GameModel WithKings(params (PieceType Type, PlayerType Side, string Square)[] pieces) =>
            Position(new[] { (King, White, "a1"), (King, Black, "h8") }.Concat(pieces).ToArray());

        private static GameModel StartingPosition()
        {
            var pieces = new List<PieceState>();
            var backRank = new[] { Rook, Knight, Bishop, Queen, King, Bishop, Knight, Rook };
            for (var file = 0; file < GameConstants.CellCount; file++)
            {
                pieces.Add(new PieceState(backRank[file], White, new Square(file, 0)));
                pieces.Add(new PieceState(Pawn, White, new Square(file, 1)));
                pieces.Add(new PieceState(backRank[file], Black, new Square(file, 7)));
                pieces.Add(new PieceState(Pawn, Black, new Square(file, 6)));
            }

            return new GameModel(pieces);
        }

        private static void Play(GameModel game, params (string From, string To)[] moves)
        {
            foreach (var move in moves)
            {
                Assert.That(TryMove(game, move.From, move.To), Is.True, $"{move.From} → {move.To}");
            }
        }

        private static PieceState At(GameModel game, string square) => game.GetPieceAt(SquareAt(square));
        private static bool TryMove(GameModel game, string from, string to) => game.TryMove(SquareAt(from), SquareAt(to));
        private static Square SquareAt(string name) => new Square(name[0] - 'a', name[1] - '1');
    }
}

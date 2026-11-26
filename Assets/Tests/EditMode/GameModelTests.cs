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
            var game = WithKings((type, White, "d4"));

            Assert.That(TryMove(game, "d4", invalid), Is.False);
            Assert.That(TryMove(game, "d4", valid), Is.True);
        }

        [Test]
        public void KingMovesOneSquareAndCannotMoveNextToOpponent()
        {
            var game = Position((King, White, "e4"), (King, Black, "e6"));

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
            var pieces = new[] { new PieceState(King, White, SquareAt("a1")), new PieceState(King, Black, SquareAt("h8")) };
            var first = new GameModel(pieces);
            var second = new GameModel(pieces);

            Assert.That(TryMove(first, "a1", "b1"), Is.True);
            Assert.That(At(second, "a1").Type, Is.EqualTo(King));
            Assert.That(second.CurrentPlayer, Is.EqualTo(White));
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

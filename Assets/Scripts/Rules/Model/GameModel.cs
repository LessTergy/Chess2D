using System;
using System.Collections.Generic;
using System.Linq;
using Chess2D.Commands;
using Chess2D.PieceMovement;

namespace Chess2D.Model
{
    public sealed class GameModel
    {
        private static readonly PieceMoveAlgorithm[] PawnMoves = { new PawnDefaultMove(), new PawnKillMove(), new PawnEnPassantMove() };
        private static readonly PieceMoveAlgorithm[] RookMoves = { new RookMove() };
        private static readonly PieceMoveAlgorithm[] KnightMoves = { new KnightMove() };
        private static readonly PieceMoveAlgorithm[] BishopMoves = { new BishopMove() };
        private static readonly PieceMoveAlgorithm[] QueenMoves = { new QueenMove() };
        private static readonly PieceMoveAlgorithm[] KingMoves = { new KingDefaultMove(), new KingCastlingMove() };
        private static readonly PieceMoveAlgorithm[] KingAttacks = { KingMoves[0] };

        private readonly PieceState[,] _board = new PieceState[GameConstants.CellCount, GameConstants.CellCount];
        private readonly PlayerModel[] _players = { new(), new() };

        public PlayerType CurrentPlayer { get; private set; } = PlayerType.White;
        public PieceState PendingPromotion { get; private set; }
        internal PieceState LastDoubleStepPawn { get; private set; }

        public IEnumerable<PieceState> Pieces => _players.SelectMany(player => player.Pieces)
            .Where(piece => GetPieceAt(piece.Square) == piece);

        public GameModel(IEnumerable<PieceState> startingPieces)
        {
            foreach (var setup in startingPieces)
            {
                var piece = new PieceState(setup.Type, setup.PlayerType, setup.Square);
                if (!IsOnBoard(piece.Square) || GetPieceAt(piece.Square) != null)
                {
                    throw new ArgumentException("Each piece must start on its own square on the board.", nameof(startingPieces));
                }

                var player = _players[(int)piece.PlayerType];
                if (piece.Type == PieceType.King)
                {
                    if (player.King != null)
                    {
                        throw new ArgumentException("Each side must start with exactly one king.", nameof(startingPieces));
                    }

                    player.King = piece;
                }

                player.Pieces.Add(piece);
                PlacePiece(piece);
            }

            if (_players.Any(player => player.King == null))
            {
                throw new ArgumentException("Each side must start with exactly one king.", nameof(startingPieces));
            }
        }

        public PieceState GetPieceAt(Square square) => IsOnBoard(square) ? _board[square.X, square.Y] : null;

        public IReadOnlyList<Square> GetCandidateDestinations(Square from)
        {
            var piece = GetMovablePiece(from);
            return piece == null ? Array.Empty<Square>() : GetMoves(piece).Select(move => move.Target).ToArray();
        }

        public bool TryMove(Square from, Square to)
        {
            var piece = GetMovablePiece(from);
            if (piece == null)
            {
                return false;
            }

            foreach (var move in GetMoves(piece))
            {
                if (move.Target != to)
                {
                    continue;
                }

                move.Command.Execute();
                if (IsKingInCheck(CurrentPlayer))
                {
                    move.Command.Undo();
                    return false;
                }

                piece.HasMoved = true;
                if (move.CastlingRook != null)
                {
                    move.CastlingRook.HasMoved = true;
                }

                LastDoubleStepPawn = piece.Type == PieceType.Pawn && Math.Abs(to.Y - from.Y) == 2 ? piece : null;
                if (piece.Type == PieceType.Pawn && to.Y == (CurrentPlayer == PlayerType.White ? GameConstants.FinishIndex : GameConstants.StartIndex))
                {
                    PendingPromotion = piece;
                }
                else
                {
                    CurrentPlayer = OpponentOf(CurrentPlayer);
                }

                return true;
            }

            return false;
        }

        public bool TryPromote(PieceType type)
        {
            if (PendingPromotion == null || !GameConstants.PromotionPieces.Contains(type))
            {
                return false;
            }

            PendingPromotion.Type = type;
            PendingPromotion = null;
            CurrentPlayer = OpponentOf(CurrentPlayer);
            return true;
        }

        public bool IsKingInCheck(PlayerType side)
        {
            var kingSquare = _players[(int)side].King.Square;
            foreach (var attacker in _players[(int)OpponentOf(side)].Pieces)
            {
                if (GetPieceAt(attacker.Square) != attacker)
                {
                    continue;
                }

                var algorithms = attacker.Type == PieceType.King ? KingAttacks : AlgorithmsFor(attacker.Type);
                foreach (var algorithm in algorithms)
                {
                    // Pawn advances and en passant target empty squares, so neither can hit a king.
                    if (algorithm.GetAvailableMoves(attacker, this).Any(move => move.Target == kingSquare))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal bool LeavesKingInCheck(ICommand command, PlayerType side)
        {
            command.Execute();
            try
            {
                return IsKingInCheck(side);
            }
            finally
            {
                command.Undo();
            }
        }

        internal CellState GetCellStateForMove(Square square, PieceState movingPiece)
        {
            if (!IsOnBoard(square))
            {
                return CellState.OutOfBounds;
            }

            var occupant = GetPieceAt(square);
            if (occupant == null)
            {
                return CellState.Free;
            }

            return occupant.PlayerType == movingPiece.PlayerType ? CellState.Friendly : CellState.Opponent;
        }

        internal void MovePiece(PieceState piece, Square to)
        {
            RemovePiece(piece);
            piece.Square = to;
            PlacePiece(piece);
        }

        internal void RemovePiece(PieceState piece)
        {
            _board[piece.Square.X, piece.Square.Y] = null;
        }

        internal void PlacePiece(PieceState piece)
        {
            _board[piece.Square.X, piece.Square.Y] = piece;
        }

        private PieceState GetMovablePiece(Square from)
        {
            var piece = GetPieceAt(from);
            return PendingPromotion == null && piece?.PlayerType == CurrentPlayer ? piece : null;
        }

        private IEnumerable<MoveData> GetMoves(PieceState piece)
        {
            foreach (var algorithm in AlgorithmsFor(piece.Type))
            {
                foreach (var move in algorithm.GetAvailableMoves(piece, this))
                {
                    if (GetPieceAt(move.Target)?.Type != PieceType.King)
                    {
                        yield return move;
                    }
                }
            }
        }

        private static PieceMoveAlgorithm[] AlgorithmsFor(PieceType type) => type switch
        {
            PieceType.Pawn => PawnMoves,
            PieceType.Rook => RookMoves,
            PieceType.Knight => KnightMoves,
            PieceType.Bishop => BishopMoves,
            PieceType.Queen => QueenMoves,
            PieceType.King => KingMoves,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        private static bool IsOnBoard(Square square) => square.X >= 0 && square.X < GameConstants.CellCount &&
                                                        square.Y >= 0 && square.Y < GameConstants.CellCount;
        private static PlayerType OpponentOf(PlayerType side) => side == PlayerType.White ? PlayerType.Black : PlayerType.White;
    }
}

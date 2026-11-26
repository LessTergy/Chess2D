using System;
using Chess2D.Model;
using Chess2D.UI;

namespace Chess2D.Controller
{
    public class PawnPromotionController : IDisposable
    {
        private readonly GameModel _game;
        private readonly PawnPromotionPopup _popup;
        private readonly BoardController _board;
        private readonly PieceController _pieces;
        private readonly PieceMoveController _moves;

        public PawnPromotionController(GameModel game, PawnPromotionPopup popup, BoardController board,
            PieceController pieces, PieceMoveController moves)
        {
            _game = game;
            _popup = popup;
            _board = board;
            _pieces = pieces;
            _moves = moves;
        }

        public void Initialize()
        {
            _moves.OnMoveAccepted += ShowPendingPromotion;
            _popup.OnPieceSelected += OnPieceSelected;
        }

        public void Dispose()
        {
            _moves.OnMoveAccepted -= ShowPendingPromotion;
            _popup.OnPieceSelected -= OnPieceSelected;
        }

        private void ShowPendingPromotion()
        {
            var pawn = _game.PendingPromotion;
            if (pawn == null)
            {
                return;
            }

            _board.SetInteractive(false);
            _popup.SetContent(pawn.PlayerType);
            _popup.Visible = true;
        }

        private void OnPieceSelected(PieceType type)
        {
            var pawn = _game.PendingPromotion;
            if (!_game.TryPromote(type))
            {
                return;
            }

            _popup.Visible = false;
            _board.SetInteractive(_game.CanPlay);
            _pieces.RefreshAfterPromotion(pawn);
        }
    }
}

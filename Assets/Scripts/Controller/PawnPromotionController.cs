using System;
using Chess2D.Model;
using Chess2D.UI;

namespace Chess2D.Controller
{
    public class PawnPromotionController
    {
        public event Action OnPawnPromoted = delegate { };

        private PieceView _promotedPawn;

        // Inject
        private readonly PawnPromotionPopup _pawnPromotionPopup;
        private readonly BoardController _boardController;
        private readonly PieceController _pieceController;
        private readonly PieceMoveController _pieceMoveController;

        public PawnPromotionController(PawnPromotionPopup pawnPromotionPopup, BoardController boardController, 
            PieceController pieceController, PieceMoveController pieceMoveController)
        {
            _pawnPromotionPopup = pawnPromotionPopup;
            _boardController = boardController;
            _pieceController = pieceController;
            _pieceMoveController = pieceMoveController;
        }

        public void Initialize()
        {
            _pawnPromotionPopup.OnPieceSelected += PawnPromotionPopup_OnPieceSelected;
            _pieceMoveController.OnFinishMove += PieceMoveController_OnFinishMove;
        }

        private void PawnPromotionPopup_OnPieceSelected(PieceType newPieceType)
        {
            _pawnPromotionPopup.Visible = false;

            _boardController.HidePiece(_promotedPawn);
            _pieceController.CreatePiece(newPieceType, _promotedPawn.PlayerType, _promotedPawn.cellCoord);

            _boardController.SetInteractive(true);

            OnPawnPromoted();
        }

        private void PieceMoveController_OnFinishMove(PieceView movingPiece)
        {
            if (movingPiece.Type != PieceType.Pawn)
            {
                return;
            }
            
            int promotionYIndex = GetPromotionYIndex(movingPiece.PlayerType);
            bool isPromotion = movingPiece.cellCoord.y == promotionYIndex;

            if (isPromotion)
            {
                StartPromotePawn(movingPiece);
            }
        }

        private void StartPromotePawn(PieceView pawn)
        {
            _promotedPawn = pawn;
            _pawnPromotionPopup.SetContent(pawn.PlayerType);

            _pawnPromotionPopup.Visible = true;
            _boardController.SetInteractive(false);
        }

        private int GetPromotionYIndex(PlayerType playerType)
        {
            return (playerType == PlayerType.White) ? GameConstants.FinishIndex : GameConstants.StartIndex;
        }
    }
}
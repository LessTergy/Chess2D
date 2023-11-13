using System;
using System.Collections.Generic;
using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{
    public class PawnPromotionController : MonoBehaviour
    {
        public event Action OnPawnPromoted = delegate { };

        private PieceView _promotedPawn;

        // Inject
        private PawnPromotionPopup _pawnPromotionPopup;
        private IBoardController _boardController;
        private PieceController _pieceController;
        private PieceMoveController _pieceMoveController;

        public void Construct(PawnPromotionPopup pawnPromotionPopup, IBoardController boardController, 
            PieceController pieceController, PieceMoveController pieceMoveController)
        {
            _pawnPromotionPopup = pawnPromotionPopup;
            _boardController = boardController;
            _pieceController = pieceController;
            _pieceMoveController = pieceMoveController;
        }

        public void Initialize()
        {
            _pawnPromotionPopup.OnPieceChoose += PawnPromotionPopupOnPieceChoose;
            _pieceMoveController.OnFinishMove += PieceMoveController_OnFinishMove;
        }

        private void PawnPromotionPopupOnPieceChoose(PieceType newPieceType)
        {
            _pawnPromotionPopup.IsOpen = false;

            _boardController.HidePiece(_promotedPawn);
            _pieceController.CreatePiece(newPieceType, _promotedPawn.TeamType, _promotedPawn.cellCoord);

            _boardController.BoardView.SetInteractive(true);

            OnPawnPromoted();
        }

        private void PieceMoveController_OnFinishMove(PieceView movingPiece)
        {
            int promotionRow = GetPromotionRowByTeam(movingPiece.TeamType);
            bool isPromotionPawn = (movingPiece.Type == PieceType.Pawn) && (movingPiece.cellCoord.y == promotionRow);

            if (isPromotionPawn)
            {
                StartPromotePawn(movingPiece);
            }
        }

        private void StartPromotePawn(PieceView pawn)
        {
            _promotedPawn = pawn;
            _pawnPromotionPopup.SetContent(pawn.TeamType);

            _pawnPromotionPopup.IsOpen = true;
            _boardController.BoardView.SetInteractive(false);
        }

        private int GetPromotionRowByTeam(TeamType teamType)
        {
            return (teamType == TeamType.White) ? GameConstants.FinishIndex : GameConstants.StartIndex;
        }
    }

}
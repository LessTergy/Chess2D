using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class PawnPromotionController : MonoBehaviour, IController {

        public event Action OnPawnPromoted = delegate { };

        private Piece promotedPawn;
        private List<PiecePrefabPrefs> piecePrefsList;

        //Inject
        private PieceChooseView pieceChooseView;
        private IBoardContoller boardController;
        private PieceController pieceController;
        private PieceMoveController pieceMoveController;
        private PiecePrefabBuilder piecePrefabBuilder;

        public void Inject(PieceChooseView pcV, IBoardContoller bc, PieceController pc, PieceMoveController pmc, PiecePrefabBuilder ppb) {
            pieceChooseView = pcV;
            boardController = bc;
            pieceController = pc;
            pieceMoveController = pmc;
            piecePrefabBuilder = ppb;
        }

        public void Initialize() {
            InitPrefsList();

            pieceChooseView.OnPieceChoose += PieceChooseView_OnPieceChoose;
            pieceMoveController.OnFinishMove += PieceMoveController_OnFinishMove;
        }

        private void InitPrefsList() {
            piecePrefsList = new List<PiecePrefabPrefs>();

            piecePrefsList.Add(piecePrefabBuilder.GetPrefabPrefs(Piece.Type.Queen));
            piecePrefsList.Add(piecePrefabBuilder.GetPrefabPrefs(Piece.Type.Rook));
            piecePrefsList.Add(piecePrefabBuilder.GetPrefabPrefs(Piece.Type.Knight));
            piecePrefsList.Add(piecePrefabBuilder.GetPrefabPrefs(Piece.Type.Bishop));
        }

        private void PieceChooseView_OnPieceChoose(Piece.Type newPieceType) {
            pieceChooseView.isOpen = false;
            pieceChooseView.Clear();

            boardController.HidePiece(promotedPawn);
            pieceController.CreatePiece(newPieceType, promotedPawn.teamType, promotedPawn.coord);

            boardController.GetBoard().SetInteractive(true);

            OnPawnPromoted();
        }

        private void PieceMoveController_OnFinishMove(Piece movingPiece) {
            int promotionRow = GetPromotionRowByTeam(movingPiece.teamType);
            bool isPromotionPawn = (movingPiece.type == Piece.Type.Pawn) && (movingPiece.coord.y == promotionRow);

            if (isPromotionPawn) {
                StartPromotePawn(movingPiece);
            }
        }

        private void StartPromotePawn(Piece pawn) {
            promotedPawn = pawn;
            pieceChooseView.SetContent(piecePrefsList, piecePrefabBuilder.GetColor(pawn.teamType));

            pieceChooseView.isOpen = true;
            boardController.GetBoard().SetInteractive(false);
        }

        private int GetPromotionRowByTeam(ChessTeam.Type teamType) {
            return (teamType == ChessTeam.Type.White) ? 7 : 0;
        }
    }

}

using System;
using System.Collections.Generic;
using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{

    public class PawnPromotionController : MonoBehaviour, IController
    {

        public event Action OnPawnPromoted = delegate { };

        private Piece _promotedPawn;
        private List<PiecePrefabPrefs> _piecePrefsList;

        // Inject
        private PieceChooseView _pieceChooseView;
        private IBoardController _boardController;
        private PieceController _pieceController;
        private PieceMoveController _pieceMoveController;
        private PiecePrefabBuilder _piecePrefabBuilder;

        public void Construct(PieceChooseView pcV, IBoardController bc, PieceController pc, PieceMoveController pmc, PiecePrefabBuilder ppb)
        {
            _pieceChooseView = pcV;
            _boardController = bc;
            _pieceController = pc;
            _pieceMoveController = pmc;
            _piecePrefabBuilder = ppb;
        }

        public void Initialize()
        {
            InitPrefsList();

            _pieceChooseView.OnPieceChoose += PieceChooseView_OnPieceChoose;
            _pieceMoveController.OnFinishMove += PieceMoveController_OnFinishMove;
        }

        private void InitPrefsList()
        {
            _piecePrefsList = new List<PiecePrefabPrefs>
            {
                _piecePrefabBuilder.GetPrefabPrefs(Piece.Type.Queen),
                _piecePrefabBuilder.GetPrefabPrefs(Piece.Type.Rook),
                _piecePrefabBuilder.GetPrefabPrefs(Piece.Type.Knight),
                _piecePrefabBuilder.GetPrefabPrefs(Piece.Type.Bishop)
            };

        }

        private void PieceChooseView_OnPieceChoose(Piece.Type newPieceType)
        {
            _pieceChooseView.isOpen = false;
            _pieceChooseView.Clear();

            _boardController.HidePiece(_promotedPawn);
            _pieceController.CreatePiece(newPieceType, _promotedPawn.teamType, _promotedPawn.cellCoord);

            _boardController.GetBoard().SetInteractive(true);

            OnPawnPromoted();
        }

        private void PieceMoveController_OnFinishMove(Piece movingPiece)
        {
            int promotionRow = GetPromotionRowByTeam(movingPiece.teamType);
            bool isPromotionPawn = (movingPiece.type == Piece.Type.Pawn) && (movingPiece.cellCoord.y == promotionRow);

            if (isPromotionPawn)
            {
                StartPromotePawn(movingPiece);
            }
        }

        private void StartPromotePawn(Piece pawn)
        {
            _promotedPawn = pawn;
            _pieceChooseView.SetContent(_piecePrefsList, _piecePrefabBuilder.GetColor(pawn.teamType));

            _pieceChooseView.isOpen = true;
            _boardController.GetBoard().SetInteractive(false);
        }

        private int GetPromotionRowByTeam(ChessTeam.Type teamType)
        {
            return (teamType == ChessTeam.Type.White) ? Board.FinishIndex : Board.StartIndex;
        }
    }

}
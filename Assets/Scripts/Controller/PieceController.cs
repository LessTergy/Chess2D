using System.Collections.Generic;
using Chess2D.Model;
using Chess2D.PieceMovement;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{
    public class PieceController
    {
        // Inject
        private readonly BoardController _boardController;
        private readonly PieceMoveController _pieceMoveController;
        private readonly ArrangementConfig _arrangementConfig;
        private readonly PieceFactory _pieceFactory;
        private readonly GameObject _piecesParent;

        public PieceController(BoardController boardController, PieceMoveController pieceMoveController,
            ArrangementConfig arrangementConfig, PieceFactory pieceFactory, GameObject pieceParent)
        {
            _boardController = boardController;
            _pieceMoveController = pieceMoveController;
            _arrangementConfig = arrangementConfig;
            _pieceFactory = pieceFactory;
            _piecesParent = pieceParent;
        }

        public void CreatePlayerPieces(GameModel gameModel)
        {
            foreach (PlayerModel playerModel in gameModel.players)
            {
                CreatePlayerPieces(playerModel);
            }
        }

        private void CreatePlayerPieces(PlayerModel playerModel)
        {
            List<CellInfo> cellInfoList = _arrangementConfig.GetCells(playerModel.type);
            
            foreach (CellInfo cellInfo in cellInfoList)
            {
                CreatePieceView(playerModel, cellInfo.pieceType, cellInfo.coord);
            }
        }

        public void CreatePieceView(PlayerModel playerModel, PieceType type, Vector2Int cellCoord)
        {
            // Create
            PieceView pieceView = _pieceFactory.Create(type, playerModel.type, _piecesParent.transform);
            playerModel.pieces.Add(pieceView);
            _pieceMoveController.OnPieceCreated(pieceView);
            
            if (pieceView.Type == PieceType.King)
            {
                playerModel.SetKing(pieceView);
            }
            
            // Cell setup
            pieceView.cellCoord = cellCoord;
            CellView cellView = _boardController.GetCell(cellCoord);
            cellView.SetPiece(pieceView);
            
            var pieceRect = pieceView.transform as RectTransform;
            pieceRect.sizeDelta = cellView.RectT.sizeDelta;
            pieceView.transform.position = cellView.transform.position;
        }

        public void UpdatePieceTarget(PieceView friendlyPiece, PieceView opponentPiece)
        {
            if (!opponentPiece.IsActive)
            {
                return;
            }

            foreach (PieceMoveAlgorithm moveAlgorithm in opponentPiece.MoveAlgorithms)
            {
                List<MoveData> moves = moveAlgorithm.GetAvailableMoves(opponentPiece, _boardController);

                foreach (MoveData moveInfo in moves)
                {
                    if (moveInfo.cellView.Coord != friendlyPiece.cellCoord)
                    {
                        continue;
                    }
                    friendlyPiece.isTarget = true;
                    return;
                }
            }
        }
    }
}
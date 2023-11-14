using System.Collections.Generic;
using Chess2D.Model;
using Chess2D.Model.PieceMove;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{
    public class PieceController
    {
        public event PieceViewDelegate OnPieceCreated;
        
        // Inject
        private readonly BoardController _boardController;
        private readonly ArrangementConfig _arrangementConfig;
        private readonly PieceConfig _pieceConfig;
        private readonly GameObject _piecesParent;

        public PieceController(BoardController boardController, ArrangementConfig arrangementConfig, 
            PieceConfig pieceConfig, GameObject pieceParent)
        {
            _boardController = boardController;
            _arrangementConfig = arrangementConfig;
            _pieceConfig = pieceConfig;
            _piecesParent = pieceParent;
        }

        public void Initialize()
        {
            CreatePlayerPieces(_arrangementConfig.whitePieceCells, PlayerType.White);
            CreatePlayerPieces(_arrangementConfig.blackPieceCells, PlayerType.Black);
        }

        private void CreatePlayerPieces(List<CellInfo> cellInfoList, PlayerType playerType)
        {
            foreach (CellInfo cellInfo in cellInfoList)
            {
                CreatePiece(cellInfo.pieceType, playerType, cellInfo.coord);
            }
        }

        public void CreatePiece(PieceType type, PlayerType playerType, Vector2Int cellCoord)
        {
            // Create
            PieceView pieceView = _pieceConfig.CreatePieceView(type, playerType, _piecesParent.transform);
            pieceView.name = $"{playerType} {type}";
            pieceView.cellCoord = cellCoord;
            
            // Cell setup
            CellView cellView = _boardController.GetCell(cellCoord);
            cellView.SetPiece(pieceView);
            
            var pieceRect = pieceView.transform as RectTransform;
            pieceRect.sizeDelta = cellView.RectT.sizeDelta;
            pieceView.transform.position = cellView.transform.position;

            OnPieceCreated?.Invoke(pieceView);
        }

        public void UpdatePieceTarget(PieceView friendlyPiece, PieceView opponentPiece)
        {
            if (!opponentPiece.IsActive)
            {
                return;
            }

            foreach (PieceMoveAlgorithm moveAlgorithm in opponentPiece.Moves)
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
using System.Collections.Generic;
using Chess2D.Model;
using Chess2D.Model.PieceMove;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{
    public class PieceController : MonoBehaviour
    {
        public event PieceViewDelegate OnPieceCreated;
        
        // Inject
        private IBoardController _boardController;
        private ArrangementConfig _arrangementConfig;
        private PieceConfig _pieceConfig;
        private GameObject _piecesParent;

        public void Construct(IBoardController boardController, ArrangementConfig arrangementConfig, 
            PieceConfig pieceConfig, GameObject pieceParent)
        {
            _boardController = boardController;
            _arrangementConfig = arrangementConfig;
            _pieceConfig = pieceConfig;
            _piecesParent = pieceParent;
        }

        public void Initialize()
        {
            CreateTeamPieces(_arrangementConfig.whitePieceCells, TeamType.White);
            CreateTeamPieces(_arrangementConfig.blackPieceCells, TeamType.Black);
        }

        private void CreateTeamPieces(List<CellInfo> cellInfoList, TeamType teamType)
        {
            foreach (CellInfo cellInfo in cellInfoList)
            {
                CreatePiece(cellInfo.pieceType, teamType, cellInfo.coord);
            }
        }

        public void CreatePiece(PieceType type, TeamType teamType, Vector2Int cellCoord)
        {
            // Create
            PieceView pieceView = _pieceConfig.CreatePieceView(type, teamType, _piecesParent.transform);
            pieceView.name = $"{teamType} {type}";
            pieceView.cellCoord = cellCoord;


            // Cell setup
            CellView cellView = _boardController.GetCell(cellCoord);
            cellView.SetPiece(pieceView);
            
            var pieceRect = pieceView.transform as RectTransform;
            pieceRect.sizeDelta = cellView.RectT.sizeDelta;
            pieceView.transform.position = cellView.transform.position;

            OnPieceCreated?.Invoke(pieceView);
        }

        public void UpdatePieceTargetByEnemy(PieceView friendlyPiece, PieceView enemyPiece)
        {
            if (!enemyPiece.IsActive)
            {
                return;
            }

            foreach (PieceMoveAlgorithm moveAlgorithm in enemyPiece.Moves)
            {
                List<MoveInfo> moves = moveAlgorithm.GetAvailableMoves(enemyPiece, _boardController);

                foreach (MoveInfo moveInfo in moves)
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
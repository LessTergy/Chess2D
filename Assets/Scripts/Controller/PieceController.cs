using System;
using System.Collections.Generic;
using Chess2D.Model;
using Chess2D.Model.PieceMove;
using Chess2D.UI;
using Lesstergy.Chess2D;
using UnityEngine;

namespace Chess2D.Controller
{

    public class PieceController : MonoBehaviour, IController
    {

        public event Action<Piece> OnPieceCreated = delegate { };
        
        // Inject
        private IBoardController _boardController;
        private ArrangementOfPieces _arrangementOfPieces;
        private PiecePrefabBuilder _piecePrefabBuilder;
        private GameObject _piecesParent;

        public void Construct(IBoardController bc, ArrangementOfPieces aop, PiecePrefabBuilder ppb, GameObject pieceParent)
        {
            _boardController = bc;
            _arrangementOfPieces = aop;
            _piecePrefabBuilder = ppb;
            _piecesParent = pieceParent;
        }

        public void Initialize()
        {
            _piecePrefabBuilder.Init();

            CreateTeamPieces(_arrangementOfPieces.whitePieceCells, ChessTeam.Type.White);
            CreateTeamPieces(_arrangementOfPieces.blackPieceCells, ChessTeam.Type.Black);
        }

        private void CreateTeamPieces(List<CellInfo> cellInfoList, ChessTeam.Type teamType)
        {
            foreach (CellInfo cellInfo in cellInfoList)
            {
                CreatePiece(cellInfo.pieceType, teamType, cellInfo.coord);
            }
        }

        public void CreatePiece(Piece.Type type, ChessTeam.Type teamType, Vector2Int cellCoord)
        {
            Cell actualCell = _boardController.GetCell(cellCoord);

            //Init
            Piece piece = _piecePrefabBuilder.CreatePiece(type, teamType);
            piece.name = $"{teamType} {type}";
            piece.cellCoord = cellCoord;

            actualCell.SetPiece(piece);

            //Size and position
            RectTransform pieceRect = piece.transform as RectTransform;
            pieceRect.sizeDelta = actualCell.rectT.sizeDelta;

            piece.transform.SetParent(_piecesParent.transform);
            piece.transform.position = actualCell.transform.position;
            piece.transform.localScale = Vector3.one;

            OnPieceCreated(piece);
        }

        public void UpdatePieceTargetByEnemy(Piece friendlyPiece, Piece enemyPiece)
        {
            if (!enemyPiece.isEnable)
            {
                return;
            }

            foreach (PieceMoveAlgorithm moveAlgorithm in enemyPiece.moves)
            {
                List<MoveInfo> moves = moveAlgorithm.GetAvailableMoves(enemyPiece, _boardController);

                foreach (MoveInfo moveInfo in moves)
                {
                    if (moveInfo.cell.coord == friendlyPiece.cellCoord)
                    {
                        friendlyPiece.isTarget = true;
                        return;
                    }
                }
            }
        }

    }
}
using System.Collections.Generic;
using Chess2D.PieceMovement;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Model
{
    [CreateAssetMenu(fileName = "PieceFactory", menuName = "Chess2D/Piece Factory")]
    public class PieceFactory : ScriptableObject
    {
        [Header("Main")]
        [SerializeField] private PieceView _pieceViewPrefab;
        
        [Header("Config")] 
        [SerializeField] private PieceConfig _pieceConfig;
        
        public PieceView Create(PieceType type, PlayerType playerType, Transform parent)
        {
            PieceView pieceView = Instantiate(_pieceViewPrefab, parent);
            pieceView.name = $"{playerType} {type}";
            
            List<PieceMoveAlgorithm> moves = GetMoves(type);
            pieceView.Construct(type, playerType, moves);
            
            Sprite sprite = _pieceConfig.GetSprite(type);
            Color color = _pieceConfig.GetColor(playerType);
            pieceView.SetSprite(sprite, color);
            
            return pieceView;
        }
        
        private List<PieceMoveAlgorithm> GetMoves(PieceType pieceType)
        {
            var moves = new List<PieceMoveAlgorithm>();

            switch (pieceType)
            {
                case PieceType.Pawn:
                    moves.Add(new PawnDefaultMove());
                    moves.Add(new PawnKillMove());
                    moves.Add(new PawnEnPassantMove());
                    break;
                
                case PieceType.Rook:
                    moves.Add(new RookMove());
                    break;
                
                case PieceType.Knight:
                    moves.Add(new KnightMove());
                    break;
                
                case PieceType.Bishop:
                    moves.Add(new BishopMove());
                    break;
                
                case PieceType.Queen:
                    moves.Add(new QueenMove());
                    break;
                
                case PieceType.King:
                    moves.Add(new KingDefaultMove());
                    moves.Add(new KingCastlingMove());
                    break;
            }
            return moves;
        }

    }
}
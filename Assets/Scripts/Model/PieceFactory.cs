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

        public PieceView Create(PieceState piece, Transform parent)
        {
            var view = Instantiate(_pieceViewPrefab, parent);
            view.Construct(piece);
            RefreshAppearance(view);
            return view;
        }

        public void RefreshAppearance(PieceView view)
        {
            var piece = view.Piece;
            view.name = $"{piece.PlayerType} {piece.Type}";
            view.SetSprite(_pieceConfig.GetSprite(piece.Type), _pieceConfig.GetColor(piece.PlayerType));
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Chess2D.UI
{
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class CellView : MonoBehaviour
    {
        public PieceView CurrentPiece { get; private set; }
        public Vector2Int Coord { get; private set; }
        public bool IsEmpty => CurrentPiece == null;

        //Components
        public Image Image { get; private set; }
        public RectTransform RectT { get; private set; }

        private void Awake()
        {
            Image = GetComponent<Image>();
            RectT = transform as RectTransform;
        }

        public void Construct(int xIndex, int yIndex, float width, float height)
        {
            name = $"Cell {xIndex}:{yIndex}";
            Coord = new Vector2Int(xIndex, yIndex);
            
            RectT.sizeDelta = new Vector2(width, height);
        }

        public void SetPiece(PieceView piece)
        {
            if (IsEmpty)
            {
                CurrentPiece = piece;
            }
            else
            {
                Debug.LogError("Can't set piece if not empty");
            }
        }

        public void ClearPiece()
        {
            CurrentPiece = null;
        }
    }
}
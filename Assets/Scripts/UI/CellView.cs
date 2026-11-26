using Chess2D.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Chess2D.UI
{
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    public class CellView : MonoBehaviour
    {
        public Square Coord { get; private set; }
        public Image Image { get; private set; }
        public RectTransform RectT { get; private set; }

        private void Awake()
        {
            Image = GetComponent<Image>();
            RectT = (RectTransform)transform;
        }

        public void Construct(Square square, float width, float height)
        {
            name = $"Cell {square.X}:{square.Y}";
            Coord = square;
            RectT.sizeDelta = new Vector2(width, height);
        }
    }
}

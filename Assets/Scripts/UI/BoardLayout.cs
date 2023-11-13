using UnityEngine;

namespace Chess2D
{
    [RequireComponent(typeof(RectTransform))]
    public class BoardLayout : MonoBehaviour
    {
        public float RectWidth => RectTransform.rect.width;
        public float CellWidth => RectWidth / GameConstants.CellCount;

        public float RectHeight => RectTransform.rect.height;
        public float CellHeight => RectHeight / GameConstants.CellCount;

        public Vector2 CellPositionOffset => new(RectWidth * RectTransform.pivot.x, RectHeight * RectTransform.pivot.y);

        private RectTransform _rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = transform as RectTransform;
                }
                return _rectTransform;
            }
        }
    }
}

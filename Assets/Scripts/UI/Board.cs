using UnityEngine;

namespace Chess2D.UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class Board : MonoBehaviour
    {
        public const int CellCount = 8;

        public const int StartIndex = 0;
        public const int FinishIndex = CellCount - 1;

        [SerializeField]
        private Vector2 _offset;
        public float xOffset => rectWidth * _offset.x;
        public float yOffset => rectHeight * _offset.y;

        public float rectWidth => rectT.rect.width;
        public float cellsWidth => rectWidth - (xOffset * 2);

        public float rectHeight => rectT.rect.height;
        public float cellsHeight => rectHeight - (yOffset * 2);

        public RectTransform rectT { get; private set; }
        private CanvasGroup _canvasGroup;

        public void Awake()
        {
            rectT = transform as RectTransform;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetInteractive(bool value)
        {
            _canvasGroup.blocksRaycasts = value;
        }

        #region Debug
        [HideInInspector]
        [SerializeField]
        private Texture debugGridTexture;

        private void OnDrawGizmosSelected()
        {
            //Look at grid interactive
            rectT = transform as RectTransform;
            Rect rect = rectT.rect;

            rect.width = cellsWidth * transform.lossyScale.x;
            rect.height = cellsHeight * transform.lossyScale.y;
            rect.position = new Vector2(rectT.position.x - rect.width * rectT.pivot.x, rectT.position.y - rect.height * rectT.pivot.y);

            Gizmos.DrawGUITexture(rect, debugGridTexture);
        }
        #endregion
    }
}
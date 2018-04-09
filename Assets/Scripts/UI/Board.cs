using UnityEngine;

namespace Lesstergy.Chess2D {
    public class Board : MonoBehaviour {

        public const int CellCount = 8;

        public const int StartIndex = 0;
        public const int FinishIndex = CellCount - 1;

        [SerializeField]
        private Vector2 _offset;
        public float xOffset { get { return rectWidth * _offset.x; } }
        public float yOffset { get { return rectHeight * _offset.y; } }
        
        public float rectWidth { get { return rectT.rect.width; } }
        public float cellsWidth { get { return rectWidth - (xOffset * 2); } }

        public float rectHeight { get { return rectT.rect.height; } }
        public float cellsHeight { get { return rectHeight - (yOffset * 2); } }
        
        public RectTransform rectT { get; private set; }
        private CanvasGroup canvasGroup;

        public void Awake() {
            rectT = transform as RectTransform;
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetInteractive(bool value) {
            canvasGroup.blocksRaycasts = value;
        }

        #region Debug
        [HideInInspector]
        [SerializeField]
        private Texture debugGridTexture;

        private void OnDrawGizmosSelected() {
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
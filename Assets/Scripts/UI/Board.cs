using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {
    public class Board : MonoBehaviour {

        public const int CELL_COUNT = 8;

        [SerializeField]
        private Vector2 _offset;
        public float xOffset { get { return width * _offset.x; } }
        public float yOffset { get { return height * _offset.y; } }
        
        public float width { get { return rectT.rect.width; } }
        public float widthOffset { get { return width - (xOffset * 2); } }

        public float height { get { return rectT.rect.height; } }
        public float heightOffset { get { return height - (yOffset * 2); } }
        
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

            rect.width = widthOffset * transform.lossyScale.x;
            rect.height = heightOffset * transform.lossyScale.y;
            rect.position = new Vector2(rectT.position.x - rect.width * rectT.pivot.x, rectT.position.y - rect.height * rectT.pivot.y);

            Gizmos.DrawGUITexture(rect, debugGridTexture);
        }
        #endregion
    }
}
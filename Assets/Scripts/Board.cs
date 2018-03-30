using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesster.Chess2D {
    public class Board : MonoBehaviour {

        public const int CELL_COUNT = 8;

        [SerializeField]
        private Vector2 _offset;
        public Vector2 offset { get { return _offset; } }
        
        public RectTransform rectT { get; private set; }

        public void Awake() {
            rectT = transform as RectTransform;
        }

        #region Debug
        [HideInInspector]
        [SerializeField]
        private Texture debugGridTexture;

        private void OnDrawGizmosSelected() {
            //Look at grid interactive
            RectTransform rectT = transform as RectTransform;
            Rect rect = rectT.rect;

            rect.width = (rect.width - offset.x * 2) * transform.lossyScale.x;
            rect.height = (rect.height - offset.y * 2) * transform.lossyScale.y;
            rect.position = new Vector2(rectT.position.x - rect.width * rectT.pivot.x, rectT.position.y - rect.height * rectT.pivot.y);

            Gizmos.DrawGUITexture(rect, debugGridTexture);
        }
        #endregion
    }
}
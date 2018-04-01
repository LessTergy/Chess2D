using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesstergy.Chess2D {

    [RequireComponent(typeof(Image))]
    public class Cell : MonoBehaviour {

        [HideInInspector]
        public Piece currentPiece;
        public Vector2Int coord { get; private set; }

        //Components
        private Image image;
        public RectTransform rectT { get; private set; }

        private void Awake() {
            image = GetComponent<Image>();
            rectT = transform as RectTransform;
        }

        public void Init(Vector2Int coord) {
            this.coord = coord;
        }

        public enum State {
            OutOfBounds,
            Free,
            Friendly,
            Enemy
        }
    }
}

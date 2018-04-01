using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesstergy.Chess2D {

    [RequireComponent(typeof(Image))]
    public class Cell : MonoBehaviour {
        
        public Piece currentPiece { get; private set; }
        public Vector2Int coord { get; private set; }

        //Components
        public Image image { get; private set; }
        public RectTransform rectT { get; private set; }

        private void Awake() {
            image = GetComponent<Image>();
            rectT = transform as RectTransform;
        }

        public void Init(Vector2Int coord) {
            this.coord = coord;
        }

        public void SetPiece(Piece piece) {
            if (currentPiece == null) {
                currentPiece = piece;
            } else {
                Debug.Log("Can't set piece if not empty");
            }
        }

        public enum State {
            OutOfBounds,
            Free,
            Friendly,
            Enemy
        }
    }
}

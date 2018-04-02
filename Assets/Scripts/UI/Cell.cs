using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesstergy.Chess2D {

    [RequireComponent(typeof(Image))]
    public class Cell : MonoBehaviour {
        
        public Piece currentPiece { get; private set; }
        public Vector2Int coord { get; private set; }
        public bool isEmpty { get { return currentPiece == null; } }

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
            if (isEmpty) {
                currentPiece = piece;
            } else {
                Debug.Log("Can't set piece if not empty");
            }
        }

        public void ClearPiece() {
            currentPiece = null;
        }

        public enum State {
            OutOfBounds,
            Free,
            Friendly,
            Enemy
        }
    }
}

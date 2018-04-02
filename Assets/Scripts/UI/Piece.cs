using Lesstergy.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesstergy.Chess2D {

    [RequireComponent(typeof(Image))]
    public class Piece : MonoBehaviour {

        public enum Type {
            King,
            Queen,
            Rook,
            Knight,
            Bishop,
            Pawn
        }
        
        public Type type { get; private set; }
        public ChessTeam.Type teamType { get; private set; }

        public List<PieceMoveAlgorithm> moves { get; private set; }

        public Vector2Int coord;
        public bool isTarget;
        public bool isWasMoving;
        public bool isLastMoving; //this piece was the last one to make move

        public bool isEnable {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }

        public bool isInteractive {
            get { return image.raycastTarget; }
            set { image.raycastTarget = value; }
        }

        //Components
        private Image image;
        public InteractiveObject interactive;

        private void Awake() {
            image = GetComponent<Image>();
            interactive = GetComponent<InteractiveObject>();
        }

        public void Initialize(Type type, Sprite sprite, List<PieceMoveAlgorithm> moves, ChessTeam.Type teamType, Color color) {
            this.type = type;
            image.sprite = sprite;

            this.moves = moves;

            this.teamType = teamType;
            image.color = color;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lesstergy.UI;

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
        public ChessTeam.Type sideType { get; private set; }

        public List<PieceMoveAlgorithm> moves { get; private set; }

        public Vector2Int cellCoord;

        //Components
        private Image image;
        public InteractiveObject interactive;

        private void Awake() {
            image = GetComponent<Image>();
            interactive = GetComponent<InteractiveObject>();
        }

        public void Initialize(Type type, Sprite sprite, List<PieceMoveAlgorithm> moves) {
            this.type = type;
            image.sprite = sprite;

            this.moves = moves;
        }

        public void InitChessTeam(ChessTeam.Type teamType, Color teamColor) {
            this.sideType = teamType;
            image.color = teamColor;
        }
    }
}

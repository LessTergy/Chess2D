using System.Collections.Generic;
using Chess2D.Model;
using Chess2D.Model.PieceMove;
using Lesstergy.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Chess2D.UI
{

    [RequireComponent(typeof(Image))]
    public class Piece : MonoBehaviour
    {

        public enum Type
        {
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

        public Vector2Int cellCoord;
        public bool isTarget;
        public bool isWasMoving;
        public bool isLastMoving; // this piece was the last one to make move

        public bool isEnable
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public bool isInteractive
        {
            get => _image.raycastTarget;
            set => _image.raycastTarget = value;
        }

        // Components
        private Image _image;
        public InteractiveObject interactive;

        private void Awake()
        {
            _image = GetComponent<Image>();
            interactive = GetComponent<InteractiveObject>();
        }

        public void Initialize(Type type, Sprite sprite, List<PieceMoveAlgorithm> moves, ChessTeam.Type teamType, Color color)
        {
            this.type = type;
            _image.sprite = sprite;

            this.moves = moves;

            this.teamType = teamType;
            _image.color = color;
        }
    }
}

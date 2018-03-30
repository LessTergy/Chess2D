using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesster.Chess2D {

    [RequireComponent(typeof(Image))]
    public class Piece : MonoBehaviour {

        public enum Type {
            King, //Король
            Queen, //Ферзь
            Rook, //Ладья
            Knight, //Конь
            Bishop, //Слон
            Pawn //Пешка
        }

        [SerializeField]
        private Type _type;
        public Type type { get { return _type; } }

        public ChessSide.Type sideType { get; private set; }

        private Image image;

        private void Awake() {
            image = GetComponent<Image>();
        }

        public void Initialize(ChessSide.Type sideType, Color sideColor) {
            this.sideType = sideType;
            image.color = sideColor;
        }
    }
}

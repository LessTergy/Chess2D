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


    }
}

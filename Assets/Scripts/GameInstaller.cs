using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesster.Chess2D {

    public class GameInstaller : MonoBehaviour {

        public BoardController boardController;
        public PieceController pieceController;
        
        private void Awake() {
            boardController.Initialize();
            pieceController.Initialize();
        }
    }

}

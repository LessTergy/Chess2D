using UnityEngine;
using System;
using System.Collections.Generic;

namespace Lesstergy.Chess2D {

    [CreateAssetMenu(fileName = "ArrangmentOfPieces", menuName = "Chess2D/Arrangment Of Pieces")]
    public class ArrangmentOfPieces : ScriptableObject {
        public List<CellInfo> whitePieceCells;
        public List<CellInfo> blackPieceCells;
    }
}

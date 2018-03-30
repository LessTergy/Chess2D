using UnityEngine;
using System;
using System.Collections.Generic;

namespace Lesster.Chess2D {

    [CreateAssetMenu(fileName = "ArrangmentOfPieces", menuName = "Chess2D/ArrangmentOfPieces")]
    public class ArrangmentOfPieces : ScriptableObject {
        public List<CellInfo> whitePieceCells;
        public List<CellInfo> blackPieceCells;
    }
}

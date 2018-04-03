using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    [CreateAssetMenu(fileName = "ArrangmentOfPieces", menuName = "Chess2D/Arrangment Of Pieces")]
    public class ArrangmentOfPieces : ScriptableObject {
        public List<CellInfo> whitePieceCells;
        public List<CellInfo> blackPieceCells;
    }
}

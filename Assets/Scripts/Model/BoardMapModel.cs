using UnityEngine;
using System;
using System.Collections.Generic;

namespace Lesster.Chess2D {

    [CreateAssetMenu(fileName = "MapModel", menuName = "Chess2D/MapModel")]
    public class BoardMapModel : ScriptableObject {
        public List<CellInfo> whitePieceCells;
        public List<CellInfo> blackPieceCells;
    }
}

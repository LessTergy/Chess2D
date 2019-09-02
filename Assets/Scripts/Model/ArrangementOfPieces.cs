using System.Collections.Generic;
using UnityEngine;

namespace Chess2D.Model
{

    [CreateAssetMenu(fileName = "ArrangementOfPieces", menuName = "Chess2D/Arrangement Of Pieces")]
    public class ArrangementOfPieces : ScriptableObject
    {
        public List<CellInfo> whitePieceCells;
        public List<CellInfo> blackPieceCells;
    }
}
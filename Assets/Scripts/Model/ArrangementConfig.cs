using System.Collections.Generic;
using UnityEngine;

namespace Chess2D.Model
{
    [CreateAssetMenu(fileName = "ArrangementConfig", menuName = "Chess2D/Arrangement Config")]
    public class ArrangementConfig : ScriptableObject
    {
        public List<CellInfo> whitePieceCells;
        public List<CellInfo> blackPieceCells;
    }
}
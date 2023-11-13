using Chess2D.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Chess2D.Model.Editor
{
    [CustomEditor(typeof(ArrangementConfig))]
    public class ArrangementConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CheckArrangement();
        }

        private void CheckArrangement()
        {
            var arrangement = (ArrangementConfig) target;

            if (arrangement.whitePieceCells == null || arrangement.blackPieceCells == null)
            {
                return;
            }

            CheckKingCount(arrangement.whitePieceCells);
            CheckKingCount(arrangement.blackPieceCells);

            CheckSameCells(arrangement.whitePieceCells, arrangement.blackPieceCells);

            CheckCoordBounds(arrangement.whitePieceCells);
            CheckCoordBounds(arrangement.blackPieceCells);
        }

        private void CheckKingCount(List<CellInfo> cells)
        {
            IEnumerable<CellInfo> kings = cells.Where(c => c.pieceType == PieceType.King);
            int count = kings.Count();
            
            if (count != 1)
            {
                EditorGUILayout.HelpBox($"King has incorrect count value - {count}", MessageType.Error);
            }
        }

        // On one cell should be only one piece
        private void CheckSameCells(List<CellInfo> whiteCells, List<CellInfo> blackCells)
        {
            var sameCellCount = new Dictionary<Vector2Int, int>();

            foreach (CellInfo cell in whiteCells)
            {
                if (!sameCellCount.ContainsKey(cell.coord))
                {
                    sameCellCount.Add(cell.coord, 1);
                }
                else
                {
                    sameCellCount[cell.coord] += 1;
                }
            }

            foreach (CellInfo cell in blackCells)
            {
                if (!sameCellCount.ContainsKey(cell.coord))
                {
                    sameCellCount.Add(cell.coord, 1);
                }
                else
                {
                    sameCellCount[cell.coord] += 1;
                }
            }

            foreach (KeyValuePair<Vector2Int, int> pair in sameCellCount)
            {
                bool isHaveDuplicates = (pair.Value > 1);
                if (isHaveDuplicates)
                {
                    EditorGUILayout.HelpBox($"Coord {pair.Key} have duplicates", MessageType.Error);
                }
            }
        }

        private void CheckCoordBounds(List<CellInfo> cells)
        {
            foreach (CellInfo cell in cells)
            {
                bool xIsIncorrect = cell.coord.x < GameConstants.StartIndex || cell.coord.x >= GameConstants.CellCount;
                bool yIsIncorrect = cell.coord.y < GameConstants.StartIndex || cell.coord.y >= GameConstants.CellCount;

                if (xIsIncorrect || yIsIncorrect)
                {
                    EditorGUILayout.HelpBox($"Coord {cell.coord} have incorrect value", MessageType.Error);
                }
            }
        }
    }
}
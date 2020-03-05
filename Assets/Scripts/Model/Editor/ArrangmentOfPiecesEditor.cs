using Chess2D.UI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RangeInt = Lesstergy.Math.RangeInt;

namespace Chess2D.Model.Editor
{

    [CustomEditor(typeof(ArrangementOfPieces))]
    public class ArrangementOfPiecesEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CheckArrangement();
        }

        private void CheckArrangement()
        {
            var arrangement = (ArrangementOfPieces) target;

            if (arrangement.whitePieceCells == null || arrangement.blackPieceCells == null)
            {
                return;
            }

            CheckChessCount(arrangement.whitePieceCells);
            CheckChessCount(arrangement.blackPieceCells);

            CheckSameCells(arrangement.whitePieceCells, arrangement.blackPieceCells);

            CheckCoordBounds(arrangement.whitePieceCells);
            CheckCoordBounds(arrangement.blackPieceCells);
        }

        private void CheckChessCount(List<CellInfo> cells)
        {
            var chessCountDict = new Dictionary<Piece.Type, int>();

            foreach (CellInfo cell in cells)
            {
                if (!chessCountDict.ContainsKey(cell.pieceType))
                {
                    chessCountDict.Add(cell.pieceType, 1);
                }
                else
                {
                    chessCountDict[cell.pieceType] += 1;
                }
            }

            foreach (KeyValuePair<Piece.Type, int> pair in chessCountDict)
            {
                RangeInt pieceCountRange = GameInfo.PieceCountDict[pair.Key];
                if (!pieceCountRange.IsInRange(pair.Value))
                {
                    EditorGUILayout.HelpBox($"Piece {pair.Key} ave incorrect count value", MessageType.Error);
                }
            }
        }

        //On one cell should be only one piece
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
                    EditorGUILayout.HelpBox($"coord {pair.Key} have duplicates", MessageType.Error);
                }
            }
        }

        private void CheckCoordBounds(List<CellInfo> cells)
        {
            foreach (CellInfo cell in cells)
            {
                bool xIsIncorrect = cell.coord.x < Board.StartIndex || cell.coord.x >= Board.CellCount;
                bool yIsIncorrect = cell.coord.y < Board.StartIndex || cell.coord.y >= Board.CellCount;

                if (xIsIncorrect || yIsIncorrect)
                {
                    EditorGUILayout.HelpBox($"coord {cell.coord} have incorrect value", MessageType.Error);
                }
            }
        }
    }
}

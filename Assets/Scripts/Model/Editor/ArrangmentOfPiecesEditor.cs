using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RangeInt = Lesstergy.Math.RangeInt;

namespace Lesstergy.Chess2D {

    [CustomEditor(typeof(ArrangmentOfPieces))]
    public class ArrangmentOfPiecesEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            CheckArrangment();
        }

        private void CheckArrangment() {
            ArrangmentOfPieces arrangment = (ArrangmentOfPieces)target;

            if (arrangment.whitePieceCells == null || arrangment.blackPieceCells == null) {
                return;
            }

            CheckChessCount(arrangment.whitePieceCells);
            CheckChessCount(arrangment.blackPieceCells);

            CheckSameCells(arrangment.whitePieceCells, arrangment.blackPieceCells);

            CheckCoordBounds(arrangment.whitePieceCells);
            CheckCoordBounds(arrangment.blackPieceCells);
        }

        private void CheckChessCount(List<CellInfo> cells) {
            Dictionary<Piece.Type, int> chessCountDict = new Dictionary<Piece.Type, int>();

            foreach (var cell in cells) {
                if (!chessCountDict.ContainsKey(cell.pieceType)) {
                    chessCountDict.Add(cell.pieceType, 1);
                } else {
                    chessCountDict[cell.pieceType] += 1;
                }
            }

            foreach (var pair in chessCountDict) {
                RangeInt pieceCountRange = GameInfo.pieceCountDict[pair.Key];
                if (!pieceCountRange.IsInRange(pair.Value)) {
                    EditorGUILayout.HelpBox("Piece " + pair.Key.ToString() + " have incorrect count value", MessageType.Error);
                }
            }
        }

        //On one cell should be only one piece
        private void CheckSameCells(List<CellInfo> whiteCells, List<CellInfo> blackCells) {
            Dictionary<Vector2Int, int> sameCellCount = new Dictionary<Vector2Int, int>();

            foreach (var cell in whiteCells) {
                if (!sameCellCount.ContainsKey(cell.coord)) {
                    sameCellCount.Add(cell.coord, 1);
                } else {
                    sameCellCount[cell.coord] += 1;
                }
            }

            foreach (var cell in blackCells) {
                if (!sameCellCount.ContainsKey(cell.coord)) {
                    sameCellCount.Add(cell.coord, 1);
                } else {
                    sameCellCount[cell.coord] += 1;
                }
            }

            foreach (var pair in sameCellCount) {
                bool isHaveDuplicates = (pair.Value > 1);
                if (isHaveDuplicates) {
                    EditorGUILayout.HelpBox("Coord " + pair.Key.ToString() + " have duplicates", MessageType.Error);
                }
            }
        }

        private void CheckCoordBounds(List<CellInfo> cells) {
            foreach (var cell in cells) {
                bool xIsIncorrect = cell.coord.x < Board.StartIndex || cell.coord.x >= Board.CellCount;
                bool yIsIncorrect = cell.coord.y < Board.StartIndex || cell.coord.y >= Board.CellCount;

                if (xIsIncorrect || yIsIncorrect) {
                    EditorGUILayout.HelpBox("Coord " + cell.coord.ToString() + " have incorrect value", MessageType.Error);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RangeInt = Lesster.Math.RangeInt;

namespace Lesster.Chess2D {

    [CustomEditor(typeof(BoardMapModel))]
    public class BoardMapModelEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            CheckBoardOnCorrect();
        }

        private void CheckBoardOnCorrect() {
            BoardMapModel mapModel = (BoardMapModel)target;

            CheckChessCount(mapModel.whitePieceCells);
            CheckChessCount(mapModel.blackPieceCells);

            CheckSameCells(mapModel.whitePieceCells, mapModel.blackPieceCells);
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
                if (pair.Value > 1) {
                    EditorGUILayout.HelpBox("Coord " + pair.Key.ToString() + " have incorrect count value", MessageType.Error);
                }
            }
        }
    }
}

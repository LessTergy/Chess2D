using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesstergy.Chess2D {

    public class BoardController : IBoardContoller, IController {

        #region Injections
        private Board board;
        
        private Cell cellPrefab;
        private GameObject cellParent;
        #endregion

        private Cell[,] cells;


        public void Inject(Board board, Cell cellPrefab, GameObject cellParent) {
            this.board = board;
            this.cellPrefab = cellPrefab;
            this.cellParent = cellParent;
        }
        
        public void Initialize() {
            CreateCells();
        }

        private void CreateCells() {
            int cellCount = Board.CELL_COUNT;
            cells = new Cell[cellCount, cellCount];

            float cellWidth = board.widthOffset / cellCount;
            float cellHeight = board.heightOffset / cellCount;

            for (int xIndex = 0; xIndex < cellCount; xIndex++) {
                for (int yIndex = 0; yIndex < cellCount; yIndex++) {
                    Cell cell = InitCell(xIndex, yIndex, cellWidth, cellHeight);
                    cells[xIndex, yIndex] = cell;
                }
            }
        }

        private Cell InitCell(int xIndex, int yIndex, float width, float height) {
            Cell cell = Instantiate(cellPrefab, cellParent.transform);
            cell.name = "Cell " + xIndex + ":" + yIndex; 
            cell.transform.localScale = Vector3.one;

            Vector3 position = new Vector3(xIndex * width, yIndex * height);

            position.x -= (board.width * board.rectT.pivot.x - board.xOffset);
            position.y -= (board.height * board.rectT.pivot.y - board.yOffset);
            cell.transform.localPosition = position;

            RectTransform cellRectT = cell.transform as RectTransform;
            cellRectT.sizeDelta = new Vector2(width, height);

            return cell;
        }

        public override Cell GetCell(int x, int y) {
            return cells[x, y];
        }
    }

}

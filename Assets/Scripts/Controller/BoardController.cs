using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesster.Chess2D {

    public abstract class IBoardContoller : MonoBehaviour {

        public abstract Cell GetCell(int x, int y);
    }

    public class BoardController : IBoardContoller, IController {

        [SerializeField]
        private GameObject cellsParent;

        [SerializeField]
        private Cell cellPrefab;

        [SerializeField]
        private Board board;

        private Cell[,] cells;
        
        public void Initialize() {
            CreateCells();
        }

        private void CreateCells() {
            int cellCount = Board.CELL_COUNT;
            cells = new Cell[cellCount, cellCount];

            float cellWidth = (board.rectT.rect.width - board.offset.x * 2) / cellCount;
            float cellHeight = (board.rectT.rect.height - board.offset.y * 2) / cellCount;

            for (int xIndex = 0; xIndex < cellCount; xIndex++) {
                for (int yIndex = 0; yIndex < cellCount; yIndex++) {
                    Cell cell = InitCell(xIndex, yIndex, cellWidth, cellHeight);
                    cells[xIndex, yIndex] = cell;
                }
            }
        }

        private Cell InitCell(int xIndex, int yIndex, float width, float height) {
            Cell cell = Instantiate(cellPrefab, cellsParent.transform);
            cell.name = "Cell " + xIndex + ":" + yIndex; 
            cell.transform.localScale = Vector3.one;

            Vector3 position = new Vector3(xIndex * width, yIndex * height);
            position.x -= (board.rectT.rect.width * board.rectT.pivot.x - board.offset.x);
            position.y -= (board.rectT.rect.height * board.rectT.pivot.y - board.offset.y);
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

using Chess2D.Controller;
using Chess2D.UI;
using UnityEngine;

namespace Lesstergy.Chess2D
{

    public class BoardController : IBoardController, IController
    {

        // fields
        private Cell[,] _cells;

        // Inject
        private Board _board;
        private Cell _cellPrefab;
        private GameObject _cellParent;

        public void Construct(Board board, Cell cellPrefab, GameObject cellParent)
        {
            _board = board;
            _cellPrefab = cellPrefab;
            _cellParent = cellParent;
        }

        #region Initialize
        public void Initialize()
        {
            CreateCells();
        }

        private void CreateCells()
        {
            const int cellCount = Board.CellCount;
            _cells = new Cell[cellCount, cellCount];

            float cellWidth = _board.cellsWidth / cellCount;
            float cellHeight = _board.cellsHeight / cellCount;

            for (var xIndex = 0; xIndex < cellCount; xIndex++)
            {
                for (var yIndex = 0; yIndex < cellCount; yIndex++)
                {
                    Cell cell = InitCell(xIndex, yIndex, cellWidth, cellHeight);
                    _cells[xIndex, yIndex] = cell;
                }
            }
        }

        private Cell InitCell(int xIndex, int yIndex, float width, float height)
        {
            Cell cell = Instantiate(_cellPrefab, _cellParent.transform);
            cell.name = $"Cell {xIndex}:{yIndex}";
            cell.Init(new Vector2Int(xIndex, yIndex));

            cell.transform.localScale = Vector3.one;
            Vector3 position = new Vector3(xIndex * width, yIndex * height);

            position.x -= (_board.rectWidth * _board.rectT.pivot.x - _board.xOffset);
            position.y -= (_board.rectHeight * _board.rectT.pivot.y - _board.yOffset);
            cell.transform.localPosition = position;

            var cellRectT = cell.transform as RectTransform;
            cellRectT.sizeDelta = new Vector2(width, height);

            return cell;
        }
        #endregion

        public override Board GetBoard()
        {
            return _board;
        }

        public override Cell GetCell(int x, int y)
        {
            return _cells[x, y];
        }

        public override Cell.State GetCellStateForPiece(int x, int y, Piece piece)
        {
            bool xIsOutOfBounds = (x < Board.StartIndex || x > Board.FinishIndex);
            bool yIsOutOfBounds = (y < Board.StartIndex || y > Board.FinishIndex);

            if (xIsOutOfBounds || yIsOutOfBounds)
            {
                return Cell.State.OutOfBounds;
            }

            Cell targetCell = GetCell(x, y);

            if (!targetCell.isEmpty)
            {

                if (piece.teamType == targetCell.currentPiece.teamType)
                {
                    return Cell.State.Friendly;
                }

                if (piece.teamType != targetCell.currentPiece.teamType)
                {
                    return Cell.State.Enemy;
                }
            }

            return Cell.State.Free;
        }

        public override void ReplacePiece(Piece piece, Vector2Int cellCoord)
        {
            Cell currentCell = GetCell(piece.cellCoord);
            Cell moveCell = GetCell(cellCoord);

            piece.cellCoord = moveCell.coord;
            currentCell.ClearPiece();

            moveCell.SetPiece(piece);
            piece.transform.position = moveCell.transform.position;
        }

        public override void HidePiece(Piece piece)
        {
            Cell cell = GetCell(piece.cellCoord);
            cell.ClearPiece();
            piece.isEnable = false;
        }

        public override void ShowPiece(Piece piece)
        {
            Cell cell = GetCell(piece.cellCoord);
            cell.SetPiece(piece);
            piece.isEnable = true;
        }
    }

}
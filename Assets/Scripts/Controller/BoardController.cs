using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{
    public class BoardController : MonoBehaviour, IBoardController
    {
        // fields
        private CellView[,] _cells;

        // Inject
        private CellView _cellPrefab;
        private GameObject _cellParent;

        public BoardView BoardView { get; private set; }

        public void Construct(BoardView boardView, CellView cellPrefab, GameObject cellParent)
        {
            BoardView = boardView;
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
            const int cellCount = GameConstants.CellCount;
            _cells = new CellView[cellCount, cellCount];

            float cellWidth = BoardView.Layout.CellWidth;
            float cellHeight = BoardView.Layout.CellHeight;

            for (var xIndex = 0; xIndex < cellCount; xIndex++)
            {
                for (var yIndex = 0; yIndex < cellCount; yIndex++)
                {
                    CellView cell = CreateCell(xIndex, yIndex, cellWidth, cellHeight);
                    _cells[xIndex, yIndex] = cell;
                }
            }
        }

        private CellView CreateCell(int xIndex, int yIndex, float width, float height)
        {
            CellView cell = Instantiate(_cellPrefab, _cellParent.transform);
            cell.Construct(xIndex, yIndex, width, height);

            var position = new Vector2(xIndex * width, yIndex * height);
            position -= BoardView.Layout.CellPositionOffset;
            cell.transform.localPosition = position;

            return cell;
        }
        #endregion

        public CellView GetCell(int x, int y)
        {
            return _cells[x, y];
        }

        public CellState GetCellStateForPiece(int x, int y, PieceView piece)
        {
            bool xIsOutOfBounds = (x < GameConstants.StartIndex || x > GameConstants.FinishIndex);
            bool yIsOutOfBounds = (y < GameConstants.StartIndex || y > GameConstants.FinishIndex);

            if (xIsOutOfBounds || yIsOutOfBounds)
            {
                return CellState.OutOfBounds;
            }

            CellView targetCell = GetCell(x, y);

            if (!targetCell.IsEmpty)
            {

                if (piece.TeamType == targetCell.CurrentPiece.TeamType)
                {
                    return CellState.Friendly;
                }

                if (piece.TeamType != targetCell.CurrentPiece.TeamType)
                {
                    return CellState.Enemy;
                }
            }

            return CellState.Free;
        }

        public void ReplacePiece(PieceView piece, Vector2Int cellCoord)
        {
            CellView currentCell = this.GetCell(piece.cellCoord);
            CellView moveCell = this.GetCell(cellCoord);

            piece.cellCoord = moveCell.Coord;
            currentCell.ClearPiece();

            moveCell.SetPiece(piece);
            piece.transform.position = moveCell.transform.position;
        }

        public void HidePiece(PieceView piece)
        {
            CellView cell = this.GetCell(piece.cellCoord);
            cell.ClearPiece();
            piece.IsActive = false;
        }

        public void ShowPiece(PieceView piece)
        {
            CellView cell = this.GetCell(piece.cellCoord);
            cell.SetPiece(piece);
            piece.IsActive = true;
        }
    }

}
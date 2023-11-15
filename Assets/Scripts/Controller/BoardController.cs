using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{
    public class BoardController
    {
        // fields
        private CellView[,] _cells;
        
        // getters
        public RectTransform BoardRect => _boardView.RectTransform;

        // Inject
        private readonly BoardView _boardView;
        private readonly CellView _cellPrefab;
        private readonly GameObject _cellParent;

        public BoardController(BoardView boardView, CellView cellPrefab, GameObject cellParent)
        {
            _boardView = boardView;
            _cellPrefab = cellPrefab;
            _cellParent = cellParent;
        }

        #region Initialize
        public void CreateCells()
        {
            const int cellCount = GameConstants.CellCount;
            _cells = new CellView[cellCount, cellCount];

            float cellWidth = _boardView.Layout.CellWidth;
            float cellHeight = _boardView.Layout.CellHeight;

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
            CellView cell = Object.Instantiate(_cellPrefab, _cellParent.transform);
            cell.Construct(xIndex, yIndex, width, height);

            var position = new Vector2(xIndex * width, yIndex * height);
            position -= _boardView.Layout.CellPositionOffset;
            cell.transform.localPosition = position;

            return cell;
        }
        #endregion

        public CellView GetCell(int x, int y)
        {
            return _cells[x, y];
        }
        
        public CellView GetCell(Vector2Int coord)
        {
            return GetCell(coord.x, coord.y);
        }

        public CellState GetCellStateForMove(int x, int y, PieceView movingPiece)
        {
            bool xIsOutOfBounds = (x < GameConstants.StartIndex || x > GameConstants.FinishIndex);
            bool yIsOutOfBounds = (y < GameConstants.StartIndex || y > GameConstants.FinishIndex);

            if (xIsOutOfBounds || yIsOutOfBounds)
            {
                return CellState.OutOfBounds;
            }

            CellView targetCell = GetCell(x, y);
            if (targetCell.IsEmpty) return CellState.Free;
            
            if (movingPiece.PlayerType == targetCell.CurrentPiece.PlayerType)
            {
                return CellState.Friendly;
            }
            if (movingPiece.PlayerType != targetCell.CurrentPiece.PlayerType)
            {
                return CellState.Opponent;
            }
            return CellState.Free;
        }

        public void PlacePieceOnCell(PieceView pieceView, Vector2Int cellCoord)
        {
            CellView currentCell = GetCell(pieceView.cellCoord);
            CellView moveCell = GetCell(cellCoord);

            pieceView.cellCoord = moveCell.Coord;
            currentCell.ClearPiece();

            moveCell.SetPiece(pieceView);
            pieceView.transform.position = moveCell.transform.position;
        }

        public void KillPieceView(PieceView piece)
        {
            CellView cell = GetCell(piece.cellCoord);
            cell.ClearPiece();
            piece.IsActive = false;
        }

        public void PlacePieceView(PieceView piece)
        {
            CellView cell = GetCell(piece.cellCoord);
            cell.SetPiece(piece);
            piece.IsActive = true;
        }

        public void SetInteractive(bool value)
        {
            _boardView.SetInteractive(value);
        }
    }
}
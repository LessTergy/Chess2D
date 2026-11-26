using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{
    public class BoardController
    {
        private readonly CellView[,] _cells = new CellView[GameConstants.CellCount, GameConstants.CellCount];
        private readonly BoardView _boardView;
        private readonly CellView _cellPrefab;
        private readonly GameObject _cellParent;

        public RectTransform BoardRect => _boardView.RectTransform;

        public BoardController(BoardView boardView, CellView cellPrefab, GameObject cellParent)
        {
            _boardView = boardView;
            _cellPrefab = cellPrefab;
            _cellParent = cellParent;
        }

        public void CreateCells()
        {
            var layout = _boardView.Layout;
            var width = layout.CellWidth;
            var height = layout.CellHeight;
            var offset = layout.CellPositionOffset;
            for (var x = 0; x < GameConstants.CellCount; x++)
            {
                for (var y = 0; y < GameConstants.CellCount; y++)
                {
                    var cell = Object.Instantiate(_cellPrefab, _cellParent.transform);
                    cell.Construct(new Square(x, y), width, height);
                    cell.transform.localPosition = new Vector2(x * width, y * height) - offset;
                    _cells[x, y] = cell;
                }
            }
        }

        public CellView GetCell(Square square) => _cells[square.X, square.Y];

        public void SnapToSquare(PieceView view)
        {
            view.transform.position = GetCell(view.Piece.Square).transform.position;
        }

        public void SetInteractive(bool value)
        {
            _boardView.SetInteractive(value);
        }
    }
}

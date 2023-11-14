using Chess2D.Model;
using Chess2D.UI;

namespace Chess2D.Controller
{
    public class GameCycleController
    {
        private Player _playerWhite;
        private Player _playerBlack;

        private Player _currentPlayer;

        private readonly PieceController _pieceController;
        private readonly PieceMoveController _pieceMoveController;
        private readonly PawnPromotionController _pawnPromotionController;

        public GameCycleController(PieceController pieceController, PieceMoveController pieceMoveController, 
            PawnPromotionController pawnPromotionController)
        {
            _pieceController = pieceController;
            _pieceMoveController = pieceMoveController;
            _pawnPromotionController = pawnPromotionController;
        }

        public void Initialize()
        {
            _playerWhite = new Player(PlayerType.White);
            _playerBlack = new Player(PlayerType.Black);

            _pieceController.OnPieceCreated += PieceController_OnPieceCreated;
            _pieceMoveController.OnMakeMove += PieceMoveController_OnFinishMove;
            _pawnPromotionController.OnPawnPromoted += PawnPromotionController_OnPawnPromoted;
        }

        public void StartGame()
        {
            SetPlayer(_playerWhite);
        }

        //Events
        private void PieceController_OnPieceCreated(PieceView piece)
        {
            Player player = GetPlayer(piece.PlayerType);
            player.pieces.Add(piece);

            if (piece.Type == PieceType.King)
            {
                player.SetKing(piece);
            }
        }

        private void PieceMoveController_OnFinishMove(PieceView movingPiece)
        {
            UpdateKingCheck();
            FinishPlayerMove();
        }

        private void PawnPromotionController_OnPawnPromoted()
        {
            UpdateCurrentPlayer();
        }

        private void SwitchPlayer()
        {
            Player opponentPlayer = GetOpponentPlayer();
            SetPlayer(opponentPlayer);
        }

        private void SetPlayer(Player player)
        {
            _currentPlayer = player;
            UpdateCurrentPlayer();
        }

        private void UpdateCurrentPlayer()
        {
            UpdateKingCheck();
            _currentPlayer.SetInteractive(true);
            
            Player opponentPlayer = GetOpponentPlayer();
            opponentPlayer.SetInteractive(false);

            _currentPlayer.ResetLastMove();
        }

        private void UpdateKingCheck()
        {
            _currentPlayer.King.isTarget = false;
            Player opponentPlayer = GetOpponentPlayer();

            foreach (PieceView piece in opponentPlayer.pieces)
            {
                _pieceController.UpdatePieceTarget(_currentPlayer.King, piece);
                if (_currentPlayer.King.isTarget)
                {
                    return;
                }
            }
        }

        private void FinishPlayerMove()
        {
            //King under check, you can't move
            if (_currentPlayer.King.isTarget)
            {
                _pieceMoveController.CancelMove();
            }
            else
            {
                _pieceMoveController.ApplyMove();
                SwitchPlayer();
            }
        }

        private Player GetOpponentPlayer()
        {
            return (_currentPlayer.type == PlayerType.White) ? _playerBlack : _playerWhite;
        }

        private Player GetPlayer(PlayerType playerType)
        {
            return playerType switch
            {
                PlayerType.White => _playerWhite,
                PlayerType.Black => _playerBlack,
                _ => null
            };
        }
    }
}
using Chess2D.Model;
using Chess2D.UI;

namespace Chess2D.Controller
{
    public class GameCycleController
    {
        private PlayerModel CurrentPlayer => _gameModel.CurrentPlayer;
        
        private readonly GameModel _gameModel;
        private readonly BoardController _boardController;
        private readonly PieceMoveController _pieceMoveController;
        private readonly PawnPromotionController _pawnPromotionController;

        public GameCycleController(GameModel gameModel, BoardController boardController,
            PieceMoveController pieceMoveController, PawnPromotionController pawnPromotionController)
        {
            _gameModel = gameModel;
            _boardController = boardController;
            _pieceMoveController = pieceMoveController;
            _pawnPromotionController = pawnPromotionController;
        }

        public void Initialize()
        {
            _pieceMoveController.OnMakeMove += PieceMoveController_OnMakeMove;
            _pawnPromotionController.OnPawnPromoted += PawnPromotionController_OnPawnPromoted;
        }

        public void StartGame()
        {
            SetCurrentPlayer(PlayerType.White);
        }

        //Events
        private void PieceMoveController_OnMakeMove(PieceView _)
        {
            if (_boardController.IsKingInCheck(CurrentPlayer.King))
            {
                _pieceMoveController.CancelMove();
                return;
            }

            _pieceMoveController.ApplyMove();
            SwitchPlayer();
        }

        private void PawnPromotionController_OnPawnPromoted()
        {
            UpdateCurrentPlayer();
        }

        private void SwitchPlayer()
        {
            PlayerType opponentPlayerType = _gameModel.GetOpponentPlayerType();
            SetCurrentPlayer(opponentPlayerType);
        }

        private void SetCurrentPlayer(PlayerType playerType)
        {
            _gameModel.SetCurrentPlayer(playerType);
            UpdateCurrentPlayer();
        }

        private void UpdateCurrentPlayer()
        {
            CurrentPlayer.SetInteractive(true);
            CurrentPlayer.ResetLastMove();

            PlayerModel opponentPlayer = _gameModel.GetOpponentPlayer();
            opponentPlayer.SetInteractive(false);
        }
    }
}
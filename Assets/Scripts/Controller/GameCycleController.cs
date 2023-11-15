using Chess2D.Model;
using Chess2D.UI;

namespace Chess2D.Controller
{
    public class GameCycleController
    {
        private PlayerModel CurrentPlayer => _gameModel.CurrentPlayer;
        
        private readonly GameModel _gameModel;
        private readonly PieceController _pieceController;
        private readonly PieceMoveController _pieceMoveController;
        private readonly PawnPromotionController _pawnPromotionController;

        public GameCycleController(GameModel gameModel, PieceController pieceController, 
            PieceMoveController pieceMoveController, PawnPromotionController pawnPromotionController)
        {
            _gameModel = gameModel;
            _pieceController = pieceController;
            _pieceMoveController = pieceMoveController;
            _pawnPromotionController = pawnPromotionController;
        }

        public void Initialize()
        {
            _pieceMoveController.OnMakeMove += PieceMoveController_OnFinishMove;
            _pawnPromotionController.OnPawnPromoted += PawnPromotionController_OnPawnPromoted;
        }

        public void StartGame()
        {
            SetCurrentPlayer(PlayerType.White);
        }

        //Events
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
            
            UpdateKingCheck();
        }

        private void UpdateKingCheck()
        {
            CurrentPlayer.King.isTarget = false;
            PlayerModel opponentPlayer = _gameModel.GetOpponentPlayer();

            foreach (PieceView piece in opponentPlayer.pieces)
            {
                _pieceController.UpdatePieceTarget(CurrentPlayer.King, piece);
                if (CurrentPlayer.King.isTarget)
                {
                    return;
                }
            }
        }

        private void FinishPlayerMove()
        {
            //King under check, you can't move
            if (CurrentPlayer.King.isTarget)
            {
                _pieceMoveController.CancelMove();
            }
            else
            {
                _pieceMoveController.ApplyMove();
                SwitchPlayer();
            }
        }
    }
}
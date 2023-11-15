using System.Collections.Generic;
using Chess2D.Model;

namespace Chess2D.Controller
{
    public class GameBuilder
    {
        public GameModel GameModel { get; private set; }

        private readonly BoardController _boardController;
        private readonly PieceController _pieceController;
        
        public GameBuilder(BoardController boardController, PieceController pieceController)
        {
            _boardController = boardController;
            _pieceController = pieceController;
        }
        
        public void Build()
        {
            GameModel = CreateGameModel();
            
            _boardController.CreateCells();
            _pieceController.CreatePlayerPieces(GameModel);
        }

        private GameModel CreateGameModel()
        {
            var gameModel = new GameModel
            {
                players = new List<PlayerModel>()
            };

            var playerWhite = new PlayerModel(PlayerType.White);
            gameModel.players.Add(playerWhite);
            
            var playerBlack = new PlayerModel(PlayerType.Black);
            gameModel.players.Add(playerBlack);

            return gameModel;
        }
    }
}
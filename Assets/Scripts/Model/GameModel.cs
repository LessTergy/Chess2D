using System.Collections.Generic;
using System.Linq;

namespace Chess2D.Model
{
    public class GameModel
    {
        public List<PlayerModel> players;
        public PlayerModel CurrentPlayer { get; private set; }
        
        public void SetCurrentPlayer(PlayerType playerType)
        {
            PlayerModel player = GetPlayer(playerType);
            CurrentPlayer = player;
        }
        
        public PlayerModel GetOpponentPlayer()
        {
            PlayerType opponentType = GetOpponentPlayerType();
            return GetPlayer(opponentType);
        }

        public PlayerType GetOpponentPlayerType()
        {
            return (CurrentPlayer.type == PlayerType.White) ? PlayerType.Black : PlayerType.White;
        }

        public PlayerModel GetPlayer(PlayerType playerType)
        {
            return players.FirstOrDefault(p => p.type == playerType);
        }
    }
}

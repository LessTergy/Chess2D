using Chess2D.Model;
using Chess2D.UI;
using UnityEngine;

namespace Chess2D.Controller
{

    public class TeamController : MonoBehaviour, IController
    {

        private ChessTeam _whiteTeam;
        private ChessTeam _blackTeam;

        private ChessTeam _currentTeamMove;

        private PieceController _pieceController;
        private PieceMoveController _pieceMoveController;
        private PawnPromotionController _pawnPromotionController;

        public void Construct(PieceController pc, PieceMoveController pmc, PawnPromotionController ppc)
        {
            _pieceController = pc;
            _pieceMoveController = pmc;
            _pawnPromotionController = ppc;
        }

        public void Initialize()
        {
            _whiteTeam = new ChessTeam(ChessTeam.Type.White);
            _blackTeam = new ChessTeam(ChessTeam.Type.Black);

            _pieceController.OnPieceCreated += PieceController_OnPieceCreated;
            _pieceMoveController.OnMakeMove += PieceMoveController_OnFinishMove;
            _pawnPromotionController.OnPawnPromoted += PawnPromotionController_OnPawnPromoted;
        }

        public void StartGame()
        {
            SetTeamMove(_whiteTeam);
        }

        //Events
        private void PieceController_OnPieceCreated(Piece piece)
        {
            ChessTeam team = GetTeam(piece.teamType);
            team.pieces.Add(piece);

            if (piece.type == Piece.Type.King)
            {
                team.SetKing(piece);
            }
        }

        private void PieceMoveController_OnFinishMove(Piece movingPiece)
        {
            UpdateKingCheck();
            FinishTeamMove();
        }

        private void PawnPromotionController_OnPawnPromoted()
        {
            UpdateCurrentTeam();
        }

        //Team turn switch
        private void SwitchTeam()
        {
            SetTeamMove(GetEnemyTeam());
        }

        private void SetTeamMove(ChessTeam team)
        {
            _currentTeamMove = team;
            UpdateCurrentTeam();
        }

        private void UpdateCurrentTeam()
        {
            UpdateKingCheck();
            _currentTeamMove.SetInteractive(true);
            GetEnemyTeam().SetInteractive(false);

            _currentTeamMove.ResetLastMove();
        }

        private void UpdateKingCheck()
        {
            _currentTeamMove.king.isTarget = false;
            ChessTeam enenmyTeam = GetEnemyTeam();

            foreach (Piece piece in enenmyTeam.pieces)
            {
                _pieceController.UpdatePieceTargetByEnemy(_currentTeamMove.king, piece);

                if (_currentTeamMove.king.isTarget)
                {
                    return;
                }
            }
        }

        private void FinishTeamMove()
        {
            //King under check, you can't move
            if (_currentTeamMove.king.isTarget)
            {
                _pieceMoveController.CancelMove();
            }
            else
            {
                _pieceMoveController.ApplyMove();
                SwitchTeam();
            }
        }

        //Team Getters
        private ChessTeam GetEnemyTeam()
        {
            return (_currentTeamMove.type == ChessTeam.Type.White) ? _blackTeam : _whiteTeam;
        }

        private ChessTeam GetTeam(ChessTeam.Type teamType)
        {
            return (teamType == ChessTeam.Type.White) ? _whiteTeam : _blackTeam;
        }
    }

}
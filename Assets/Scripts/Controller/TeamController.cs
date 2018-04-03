using UnityEngine;

namespace Lesstergy.Chess2D {

    public class TeamController : MonoBehaviour, IController {

        private ChessTeam whiteTeam;
        private ChessTeam blackTeam;

        private ChessTeam currentTeamMove;

        private PieceController pieceController;
        private PieceMoveController pieceMoveController;
        private IBoardContoller boardController;
        private PawnPromotionController pawnPromotionController;

        public void Inject(PieceController pc, PieceMoveController pmc, IBoardContoller bc, PawnPromotionController ppc) {
            this.pieceController = pc;
            this.pieceMoveController = pmc;
            this.boardController = bc;
            this.pawnPromotionController = ppc;
        }

        public void Initialize() {
            whiteTeam = new ChessTeam(ChessTeam.Type.White);
            blackTeam = new ChessTeam(ChessTeam.Type.Black);

            pieceController.OnPieceCreated += PieceController_OnPieceCreated;
            pieceMoveController.OnMakeMove += PieceMoveController_OnFinishMove;
            pawnPromotionController.OnPawnPromoted += PawnPromotionController_OnPawnPromoted;
        }

        public void StartGame() {
            SetTeamMove(whiteTeam);
        }

        //Events
        private void PieceController_OnPieceCreated(Piece piece) {
            ChessTeam team = GetTeam(piece.teamType);
            team.pieces.Add(piece);

            if (piece.type == Piece.Type.King) {
                team.SetKing(piece);
            }
        }

        private void PieceMoveController_OnFinishMove(Piece movingPiece) {
            UpdateKingCheck();
            FinishTeamMove();
        }

        private void PawnPromotionController_OnPawnPromoted() {
            UpdateCurrentTeam();
        }

        //Team turn switch
        private void SwitchTeam() {
            SetTeamMove(GetEnemyTeam());
        }

        private void SetTeamMove(ChessTeam team) {
            currentTeamMove = team;
            UpdateCurrentTeam();
        }

        private void UpdateCurrentTeam() {
            UpdateKingCheck();
            currentTeamMove.SetPieceInteractive(true);
            GetEnemyTeam().SetPieceInteractive(false);

            currentTeamMove.ResetLastMoving();
        }

        private void UpdateKingCheck() {
            currentTeamMove.king.isTarget = false;
            ChessTeam enenmyTeam = GetEnemyTeam();

            foreach (Piece piece in enenmyTeam.pieces) {
                pieceController.UpdatePieceTargetByEnenmy(currentTeamMove.king, piece);

                if (currentTeamMove.king.isTarget) {
                    return;
                }
            }
        }

        private void FinishTeamMove() {
            //King under check, you can't move
            if (currentTeamMove.king.isTarget) {
                pieceMoveController.CancelMove();
            } else {
                pieceMoveController.ApplyMove();
                SwitchTeam();
            }
        }

        //Team Getters
        private ChessTeam GetEnemyTeam() {
            return (currentTeamMove.type == ChessTeam.Type.White) ? blackTeam : whiteTeam;
        }

        private ChessTeam GetTeam(ChessTeam.Type teamType) {
            return (teamType == ChessTeam.Type.White) ? whiteTeam : blackTeam;
        }
    }

}

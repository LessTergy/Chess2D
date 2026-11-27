# Chess2D

Chess2D is a Unity chess prototype for two players sharing one device. It started as a job application exercise built with Unity UI (uGUI).

![Chess2D gameplay demo](https://media.giphy.com/media/gxrHsWFvIY9RHX8UY3/source.gif)

## Run locally

1. Open the project folder in Unity `2022.3.62f3`. The required editor version is recorded in [ProjectVersion.txt](ProjectSettings/ProjectVersion.txt).
2. Open [Assets/Scenes/Main.unity](Assets/Scenes/Main.unity) and enter Play Mode.
3. Drag a piece to a highlighted square. White moves first. Use the restart button to reset the board.

The repository also contains a [previous Android build](Builds/Chess2D.apk).

## Gameplay

- Picking up a piece highlights its candidate destinations. Dropping it on a square that leaves your king in check returns the piece to its original square.
- Castling checks the king's starting, transit, and destination squares. En passant is available immediately after an adjacent opposing pawn advances two squares. When a pawn reaches the last rank, choose a queen, rook, knight, or bishop.
- On checkmate, the losing king's square lights up red, a white flag appears beside the king, and the status panel names the winner. To try it from the starting position, play **1. f3 e5 2. g4 Qh4#**.
- Stalemate, five occurrences of the same position, or 75 moves by each side without a pawn move or capture end the game in a draw. The game also detects bare kings, a single bishop or knight against a bare king, and positions where all remaining non-king pieces are bishops on the same square color.
- Use **Заявить ничью** to claim three occurrences of a position or 50 moves by each side without a pawn move or capture. To claim with an intended move, select **Заявить ничью с ходом** before playing it. Repetition includes the side to move, castling rights, and any legal en passant capture.
- To agree on a draw, choose **Предложить ничью** before making your move. The other player can select **Принять ничью** or continue by moving a piece.
- Once the game ends, the status panel shows the result and the board stops accepting moves. The restart button starts a new game.

## Try a specific position

On the `GameInstaller` object in `Main`, set **Default Arrangement** before entering Play Mode. Use [CastlingTest](Assets/Models/Arrangments/Test/CastlingTest.asset), [PawnPromotionTest](Assets/Models/Arrangments/Test/PawnPromotionTest.asset), [CheckmateTest](Assets/Models/Arrangments/Test/CheckmateTest.asset), or [StalemateTest](Assets/Models/Arrangments/Test/StalemateTest.asset) to inspect a specific rule or result. Restore [DefaultArrangment](Assets/Models/Arrangments/DefaultArrangment.asset) for a standard game.

To create a position, use **Assets → Create → Chess2D → Arrangement Config**. The Inspector flags missing or duplicate kings, overlapping pieces, and coordinates outside the board.

## Architecture

[GameInstaller](Assets/Scripts/GameInstaller.cs) builds the game from an arrangement asset, wires the controllers to their views, and creates the board and pieces. [Chess2D.Rules](Assets/Scripts/Rules) is a plain C# assembly that owns piece positions, turns, and the game result. Move strategies generate candidates, and reversible commands let the model test king safety.

### Script flow

```text
ArrangementConfig ──> GameBuilder ──> GameModel

PieceView ──drag/drop──> PieceMoveController ──TryMove──> GameModel
                         └──OnMoveAccepted──┬──> PieceController refreshes piece views
                                            └──> PawnPromotionController opens the popup

PawnPromotionPopup ──choice──> PawnPromotionController ──TryPromote──> GameModel
                               └──> PieceController updates the promoted piece

GameStatusView ──draw offer, acceptance, or claim──> GameModel
GameModel ──Changed──> GameStatusView ──> Draw actions, result banner, and mate effects
```

The [GameStatusView prefab](Assets/Prefabs/GameStatusView.prefab) contains the draw buttons and result banner. Edit the [red square highlight](Assets/Prefabs/MateHighlight.prefab) and [white flag](Assets/Prefabs/WhiteFlag.prefab) in their own prefabs. The view uses those effects when the game ends in checkmate.

### Project layout

```text
Assets/
├── Animations/             Pawn promotion popup transitions
├── Libs/                   Shared pointer and drag event forwarding
├── Models/                 Piece appearance, factory, and starting-position assets
│   └── Arrangments/        Default board and rule-specific positions
├── Prefabs/                Board, pieces, cells, popups, and game result UI
├── Scenes/                 Main playable scene
├── Scripts/
│   ├── GameInstaller.cs    Scene startup and dependency wiring
│   ├── Controller/         Board and piece setup, input, promotion, and restart
│   ├── Model/              Unity configuration and piece view factory
│   │   └── Editor/         Position validation and Inspector fields
│   ├── Rules/              Chess2D.Rules assembly
│   │   ├── Commands/       Apply and undo moves and captures
│   │   ├── Model/          Piece state, turn flow, and game results
│   │   └── PieceMovement/  Movement and capture strategies
│   └── UI/                 Board, piece, promotion, and game status views
├── Sprites/                Artwork for the board, pieces, and controls
└── Tests/
    └── EditMode/           Automated tests for the chess rules
```

## Automated tests

Open **Window → General → Test Runner**, select **EditMode**, and run `Chess2D.Rules.Tests`. The [tests](Assets/Tests/EditMode) exercise the C# rules directly without loading a scene. They cover movement and captures, check and rollback, castling, en passant, promotion, checkmate, stalemate, and draw claims and automatic draws.

## Known limitations

- Dead-position detection uses the material cases listed above. Other dead positions, such as a permanently locked pawn barrier, can require a draw by agreement.

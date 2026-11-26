# Chess2D

Chess2D is a Unity chess prototype for two players sharing one device. It started as a job application exercise built with Unity UI (uGUI).

![Chess2D gameplay demo](https://media.giphy.com/media/gxrHsWFvIY9RHX8UY3/source.gif)

## Run locally

1. Open the project folder in the Unity version listed in [ProjectVersion.txt](ProjectSettings/ProjectVersion.txt). The updated project uses `2022.3.62f3`. The original revision used `2021.3.27f1`.
2. Open [Assets/Scenes/Main.unity](Assets/Scenes/Main.unity) and enter Play Mode.
3. Drag a piece to a highlighted square. White moves first. Use the restart button to reset the board.

The repository also contains a [previous Android build](Builds/Chess2D.apk).

## Gameplay

- All six piece types have movement and capture rules. The board highlights candidate destinations when you pick up a piece.
- A move that leaves your king in check is rolled back. Some highlighted destinations can therefore be rejected on drop.
- Castling checks the king's starting, transit, and destination squares. En passant is available immediately after an adjacent opposing pawn advances two squares. When a pawn reaches the last rank, choose a queen, rook, knight, or bishop.

## Try a specific position

On the `GameInstaller` object in `Main`, set **Default Arrangement** to [CastlingTest](Assets/Models/Arrangments/Test/CastlingTest.asset) or [PawnPromotionTest](Assets/Models/Arrangments/Test/PawnPromotionTest.asset) before entering Play Mode. Restore [DefaultArrangment](Assets/Models/Arrangments/DefaultArrangment.asset) for a standard game.

To create a position, use **Assets → Create → Chess2D → Arrangement Config**. The Inspector flags missing or duplicate kings, overlapping pieces, and coordinates outside the board.

## Code map

- [GameInstaller](Assets/Scripts/GameInstaller.cs) wires the scene and starts the game. [Controllers](Assets/Scripts/Controller) handle input, turns, and promotion.
- [PieceMovement](Assets/Scripts/PieceMovement) holds the movement rules. [Commands](Assets/Scripts/Commands) apply and undo moves and captures. Composite commands group changes for moves such as castling.
- [Model](Assets/Scripts/Model) holds player and game state and the configurable starting positions. Board occupancy and piece state are stored in Unity UI components.

## Known limitations

- The game does not detect checkmate, stalemate, or other draw conditions.
- Clicking a piece without dragging can commit a move to the previous drop square if that square is among the piece's candidate destinations.
- The sample positions support manual checks. The repository contains no automated gameplay tests.

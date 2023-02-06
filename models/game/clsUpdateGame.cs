namespace chessAPI.models.game;

public sealed class clsUpdateGame {
    public clsUpdateGame() {
        turn = false;
        winner = 0;
    }

    public bool turn { get; set; }
    public int winner { get; set; }
}
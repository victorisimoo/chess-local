using chessAPI.dataAccess.models;
using chessAPI.models.game;

namespace chessAPI.dataAccess.repositores;

public interface IGameRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    Task<TI> createGame(clsNewGame game);
    Task<clsGameEntityModel<TI, TC>>? getGameById(TI id);
    Task<TI>? updateGame(TI id, clsUpdateGame newGame);
    Task deleteGame(TI id);
}
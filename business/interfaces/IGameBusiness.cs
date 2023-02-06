using chessAPI.models.game;

namespace chessAPI.business.interfaces;

public interface IGameBusiness<TI> 
    where TI : struct, IEquatable<TI>
{
    Task<clsGame<TI>> createGame(clsNewGame newGame);
    Task<clsGame<TI>>? getGameById(TI id);
    Task<clsGame<TI>>? updateGame(TI id, clsUpdateGame newGame);
}
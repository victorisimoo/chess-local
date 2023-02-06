using chessAPI.business.interfaces;
using chessAPI.dataAccess.repositores;
using chessAPI.models.game;

namespace chessAPI.business.impl;

public sealed class clsGameBusiness<TI, TC> : IGameBusiness<TI> 
    where TI : struct, IEquatable<TI>
    where TC : struct
{
    internal readonly IGameRepository<TI, TC> gameRepository;

    public clsGameBusiness(IGameRepository<TI, TC> gameRepository)
    {
        this.gameRepository = gameRepository;
    }

    public async Task<clsGame<TI>> createGame(clsNewGame newGame)
    {
        var x = await gameRepository.createGame(newGame).ConfigureAwait(false);
        if (x.Equals(default(TI))) return null;
        var y = await gameRepository.getGameById(x).ConfigureAwait(false);
        return new clsGame<TI>(y.id, y.started, y.whites, y.blacks, y.turn, y.winner);
    }

    public async Task<clsGame<TI>>? getGameById(TI id)
    {
        var x = await gameRepository.getGameById(id).ConfigureAwait(false);
        if (x != null) return new clsGame<TI>(x.id, x.started, x.whites, x.blacks, x.turn, x.winner);
        return null;
    }
    public async Task<clsGame<TI>>? updateGame(TI id, clsUpdateGame newGame)
    {
        var newId = await gameRepository.updateGame(id, newGame).ConfigureAwait(false);
        var y = await gameRepository.getGameById(newId).ConfigureAwait(false);
        return new clsGame<TI>(y.id, y.started, y.whites, y.blacks, y.turn, y.winner);
   }
}
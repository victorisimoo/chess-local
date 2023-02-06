using chessAPI.business.interfaces;
using chessAPI.dataAccess.repositores;
using chessAPI.models.player;

namespace chessAPI.business.impl;

public sealed class clsPlayerBusiness<TI, TC> : IPlayerBusiness<TI> 
    where TI : struct, IEquatable<TI>
    where TC : struct
{
    internal readonly IPlayerRepository<TI, TC> playerRepository;

    public clsPlayerBusiness(IPlayerRepository<TI, TC> playerRepository)
    {
        this.playerRepository = playerRepository;
    }

    public async Task<clsPlayer<TI>> addPlayer(clsNewPlayer newPlayer)
    {
        var x = await playerRepository.addPlayer(newPlayer).ConfigureAwait(false);
        return new clsPlayer<TI>(x, newPlayer.email);
    }

    public async Task<clsPlayer<TI>>? getPlayerById(TI id)
    {
        var x = await playerRepository.getPlayerById(id).ConfigureAwait(false);
        if (x != null) return new clsPlayer<TI>(id, x.email);
        return null;
    }
    public async Task<clsPlayer<TI>>? updatePlayer(TI id, clsNewPlayer newPlayer)
    {
        var newId = await playerRepository.updatePlayer(id, newPlayer).ConfigureAwait(false);
        if (newId.Equals(default(TI))) return null;
        return new clsPlayer<TI>(id, newPlayer.email);
    }
}
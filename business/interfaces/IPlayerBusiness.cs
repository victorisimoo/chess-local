using chessAPI.models.player;

namespace chessAPI.business.interfaces;

public interface IPlayerBusiness<TI> 
    where TI : struct, IEquatable<TI>
{
    Task<clsPlayer<TI>> addPlayer(clsNewPlayer newPlayer);
    Task<clsPlayer<TI>>? getPlayerById(TI id);
    Task<clsPlayer<TI>>? updatePlayer(TI id, clsNewPlayer newPlayer);
}
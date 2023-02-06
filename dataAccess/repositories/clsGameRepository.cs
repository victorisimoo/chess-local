using chessAPI.dataAccess.common;
using chessAPI.dataAccess.interfaces;
using chessAPI.dataAccess.models;
using chessAPI.models.game;
using chessAPI.dataAccess.queries.postgreSQL;
using Dapper;

namespace chessAPI.dataAccess.repositores;

public sealed class clsGameRepository<TI, TC> : clsDataAccess<clsGameEntityModel<TI, TC>, TI, TC>, IGameRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    private qGame gameQueries;

    public clsGameRepository(IRelationalContext<TC> rkm,
                               ISQLData queries,
                               ILogger<clsGameRepository<TI, TC>> logger) : base(rkm, queries, logger)
    {
        gameQueries = new qGame();
    }

    public async Task<TI> createGame(clsNewGame game)
    {
        var p = new DynamicParameters();
        p.Add("ID", game.whites);
        var teamExists = await set<TI>(p, null, gameQueries.TeamExists, null).ConfigureAwait(false);
        if (teamExists.Equals(default(TI))) return default(TI);
        return await add<TI>(p).ConfigureAwait(false);
    }

    public async Task<clsGameEntityModel<TI, TC>>? getGameById(TI id)
    {
        return await getEntity(id).ConfigureAwait(false);
    }

    public async Task<TI>? updateGame(TI id, clsUpdateGame newGame)
    {
        var gameExists = await getEntity(id).ConfigureAwait(false);
        if (gameExists == null) return default(TI);
        var p = new DynamicParameters();
        p.Add("ID", id);
        p.Add("TURN", newGame.turn);
        p.Add("TURN", newGame.turn);
        p.Add("WINNER", newGame.winner);
        await set(p, null);
        return id;
    }

    public Task deleteGame(TI id)
    {
        throw new NotImplementedException();
    }

    protected override DynamicParameters fieldsAsParams(clsGameEntityModel<TI, TC> entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        var p = new DynamicParameters();
        p.Add("ID", entity.id);
        p.Add("STARTED", entity.started);
        p.Add("WHITES", entity.whites);
        p.Add("BLACKS", entity.blacks);
        p.Add("TURN", entity.turn);
        p.Add("WINNER", entity.winner);
        return p;
    }

    protected override DynamicParameters keyAsParams(TI key)
    {
        var p = new DynamicParameters();
        p.Add("ID", key);
        return p;
    }
}
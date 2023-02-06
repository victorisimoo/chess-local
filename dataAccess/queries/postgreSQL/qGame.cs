namespace chessAPI.dataAccess.queries.postgreSQL;

public sealed class qGame : IQGame
{
    private const string _selectAll = @"
    SELECT id, started, whites, blacks, turn, winner 
    FROM public.game";

    private const string _selectOne = @"
    SELECT id, started, whites, blacks, turn, winner 
    FROM public.game
    WHERE id=@ID";

    private const string _add = @"
    INSERT INTO public.game (whites, blacks, turn, winner)
	VALUES (@ID, 0, TRUE, 0) RETURNING id";

    private const string _delete = @"
    DELETE FROM public.game 
    WHERE id = @ID";
    
    private const string _update = @"
    UPDATE public.game
	SET turn=@TURN, winner=@WINNER
	WHERE id=@ID";

    private const string _exisTeam = @"
    SELECT id 
    FROM public.team
    WHERE id=@ID";

    public string SQLGetAll => _selectAll;

    public string SQLDataEntity => _selectOne;

    public string NewDataEntity => _add;

    public string DeleteDataEntity => _delete;

    public string UpdateWholeEntity => _update;

    public string TeamExists => _exisTeam;
}
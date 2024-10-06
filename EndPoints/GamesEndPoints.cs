using GameStore.Api.Dtos;

namespace GameStore.Api.EndPoints;

public static class GamesEndPoints
{
    const string GetGameEndPointName = "GetGame";

    private static readonly List<GameDto> games = [
        new(
            1,
            "Street Fighter",
            "Fighting",
            19.99M,
            new DateOnly(1992,7,15)),
        new(
            2,
            "Warframe",
            "MMORPG",
            31M,
            new DateOnly(2007,6,7)),
        new(
            3,
            "MLBB",
            "MOBA",
            77M,
            new DateOnly(2009,2,17))
    ];

    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();
        // GET /games
        group.MapGet("/", () => games);

        //GET /games/1
        group.MapGet("/{id}",(int id) => 
        {
            GameDto? game = games.Find(game=>game.Id==id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndPointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame)=>
        {


            GameDto game= new(
                games.Count+1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);
            
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndPointName,new {id=game.Id},game);
        })
        .WithParameterValidation();

        //PUT /games/1
        group.MapPut("/{id}",(int id,UpdateGameDto updatedGame)=>{

            
            var index=games.FindIndex(game=>game.Id==id);

            if(index == -1)
            {
                return Results.NotFound();
            };

            games[index]=new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        //DELETE /games/1
        group.MapDelete("/{id}",(int id)=>{
            games.RemoveAll(game=>game.Id==id);

            return Results.NoContent();
        });

        return group;
    }
}

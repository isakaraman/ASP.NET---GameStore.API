using GameStore.Api;
using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

const string GetGameEndPointName = "GetGame";

List<GameDto> games = [
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
        2,
        "MLBB",
        "MOBA",
        77M,
        new DateOnly(2009,2,17))
];

// GET /games
app.MapGet("games", () => games);

//GET /games/1
app.MapGet("games/{id}",(int id) => games.Find(game=>game.Id==id))
.WithName(GetGameEndPointName);

//POST /games
app.MapPost("games", (CreateGameDto newGame)=>
{
    GameDto game= new(
        games.Count+1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);
    
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndPointName,new {id=game.Id},game);
});

app.Run();
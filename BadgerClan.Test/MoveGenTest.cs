
using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

namespace BadgerClan.Test;


public class MoveGenTest
{

    private GameEngine engine;

    public MoveGenTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public async Task BasicTest()
    {
        var state = new GameState();
        var bot = new RunAndGun();
        var archer1 = Unit.Factory("Archer", 1, Coordinate.Offset(10, 10));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(20, 20));
        state.AddUnit(archer1);
        state.AddUnit(knight2);

        var moves = await bot.PlanMovesAsync(state);
        GameEngine.ProcessTurn(state, moves);
        Assert.True(true);
    }

    [Fact]
    public async Task OneTurn()
    {
        var state = new GameState();

        var bot1 = new RunAndGun();
        var team1 = new Team("Team 1", "red", bot1);
        state.AddTeam(team1);

        var bot2 = new RunAndGun();
        var team2 = new Team("Team 2", "red", bot2);
        state.AddTeam(team2);

        var squad = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        state.LayoutStartingPositions(squad);

        var moves = await bot1.PlanMovesAsync(state);
        Assert.Equal(6, moves.Count);
        GameEngine.ProcessTurn(state, moves);
    }


    [Fact]
    public async Task TwoTurns()
    {
        var state = new GameState();

        var bot1 = new RunAndGun();
        var team1 = new Team("Team 1", "red", bot1);
        state.AddTeam(team1);

        var bot2 = new RunAndGun();
        var team2 = new Team("Team 1", "red", bot2);
        state.AddTeam(team2);

        var squad = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        state.LayoutStartingPositions(squad);

        var moves = await bot1.PlanMovesAsync(state);
        Assert.Equal(6, moves.Count);
        GameEngine.ProcessTurn(state, moves);

        //Incomplete and possibly unnecessary
        // moves = bot2.PlanMoves(state);
        // Assert.Equal(6, moves.Count);
        // state = engine.ProcessTurn(state, moves);
    }

    [Fact]
    public async Task ScatterTest()
    {
        var state = new GameState();

        var bot1 = new Scatter();
        var archer1 = Unit.Factory("Archer", 1, Coordinate.Offset(10, 10));
        var team1 = new Team("Team 1", "red", bot1);
        state.AddTeam(team1);
        state.AddUnit(archer1);

        var bot2 = new NothingBot();
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(10, 11));
        var team2 = new Team("Team 1", "red", bot2);
        state.AddTeam(team2);
        state.AddUnit(knight2);

        Assert.Equal(2, state.Units.Count);

        var moves1 = await bot1.PlanMovesAsync(state);

        Assert.Single(moves1);
        GameEngine.ProcessTurn(state, moves1);

        Assert.Equal(Coordinate.Offset(9, 9), state.Units[0].Location);
        Assert.Equal(Coordinate.Offset(10, 11), state.Units[1].Location);
    }
}
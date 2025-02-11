namespace BadgerClan.Client.Logic.Bot;


public interface IBot
{
    Task<List<Move>> PlanMovesAsync(GameState state);

}
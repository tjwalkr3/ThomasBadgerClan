
namespace BadgerClan.Logic.Bot;

public class Scatter : IBot
{
    public Task<List<Move>> PlanMovesAsync(GameState state)
    {
        // Return an empty list if my team is null
        var myteam = state.TeamList.FirstOrDefault(t => t.Id == state.CurrentTeamId);
        if (myteam is null)
            return Task.FromResult(new List<Move>());

        var moves = new List<Move>();
        foreach (var unit in state.Units.Where(u => u.Team == state.CurrentTeamId))
        {
            var allOtherPlayers = state.Units.Where(u => u != unit);
            var closest = allOtherPlayers.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (closest != null)
            {
                moves.Add(SharedMoves.StepAwayFromClosest(unit, closest, state));
            }
        }
        return Task.FromResult(moves);
    }

}
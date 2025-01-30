
namespace BadgerClan.Logic.Bot;

public class Scatter : IBot
{
    public Task<List<Move>> PlanMovesAsync(GameState state)
    {
        var myteam = state.TeamList.FirstOrDefault(t => t.Id == state.CurrentTeamId);
        if (myteam is null)
            return Task.FromResult(new List<Move>());

        var moves = new List<Move>();
        foreach (var unit in state.Units.Where(u => u.Team == state.CurrentTeamId))
        {
            var allOtherPlayers = state.Units.Where(u => u != unit);
            var closest = allOtherPlayers.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            Coordinate? closestWall = GetClosestWall(unit.Location, state.Dimension);

            if (closest != null && closestWall is not null)
            {
                if (unit.Location.Distance(closestWall) < 3)
                {
                    // Move away from the wall if too close
                    var moveAwayFromWall = unit.Location.Away(closestWall);
                    moves.Add(new Move(MoveType.Walk, unit.Id, moveAwayFromWall));
                }
                else
                {
                    // Move away from the closest player
                    moves.Add(SharedMoves.StepAwayFromClosest(unit, closest, state));
                }
            }
        }
        return Task.FromResult(moves);
    }

    private Coordinate? GetClosestWall(Coordinate location, int boardSize)
    {
        var walls = new List<Coordinate>
        {
            new Coordinate(0, location.R), // Left wall
            new Coordinate(boardSize - 1, location.R), // Right wall
            new Coordinate(location.Q, 0), // Top wall
            new Coordinate(location.Q, boardSize - 1) // Bottom wall
        };

        return walls.OrderBy(w => w.Distance(location)).FirstOrDefault();
    }
}
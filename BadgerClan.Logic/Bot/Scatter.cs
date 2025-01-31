
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
                if (unit.Location.Distance(closestWall) < unit.Location.Distance(closest.Location))
                {
                    var moveAwayFromWall = unit.Location.Away(closestWall);
                    moves.Add(new Move(MoveType.Walk, unit.Id, moveAwayFromWall));
                }
                else if (unit.Location.Distance(closestWall) > unit.Location.Distance(closest.Location))
                {
                    moves.Add(StepAwayFromClosest(unit, closest, state));
                }
                else
                {
                    var perpendicularMove = GetPerpendicularMove(unit.Location, closest.Location, closestWall);
                    moves.Add(new Move(MoveType.Walk, unit.Id, perpendicularMove));
                }
            }
        }
        return Task.FromResult(moves);
    }

    private Coordinate? GetClosestWall(Coordinate location, int boardSize)
    {
        var walls = new List<Coordinate>
        {
            new Coordinate(0, location.R),
            new Coordinate(boardSize - 1, location.R), // Right wall
            new Coordinate(location.Q, 0), // Top wall
            new Coordinate(location.Q, boardSize - 1) // Bottom wall
        };

        return walls.OrderBy(w => w.Distance(location)).FirstOrDefault();
    }

    public static Move StepAwayFromClosest(Unit unit, Unit closest, GameState state)
    {
        Random rnd = new Random();

        var target = unit.Location.Away(closest.Location);

        var neighbors = unit.Location.Neighbors();

        while (state.Units.Any(u => u.Location == target))
        {
            if (neighbors.Any())
            {
                var i = rnd.Next(0, neighbors.Count() - 1);
                target = neighbors[i];
                neighbors.RemoveAt(i);
            }
            else
            {
                neighbors = unit.Location.MoveEast(1).Neighbors();
            }
        }

        var move = new Move(MoveType.Walk, unit.Id, target);
        return move;
    }

    private Coordinate GetPerpendicularMove(Coordinate unitLocation, Coordinate enemyLocation, Coordinate wallLocation)
    {
        // Calculate the vector from the enemy to the wall
        var vectorToWall = new Coordinate(wallLocation.Q - enemyLocation.Q, wallLocation.R - enemyLocation.R);

        // Calculate the perpendicular vector
        var perpendicularVector = new Coordinate(-vectorToWall.R, vectorToWall.Q);

        // Calculate the new location by adding the perpendicular vector to the unit's location
        var newLocation = new Coordinate(unitLocation.Q + perpendicularVector.Q, unitLocation.R + perpendicularVector.R);

        return newLocation;
    }
}
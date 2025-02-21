namespace BadgerClan.Client.Logic.Bot;

public class AdvancedBot : IBot
{
    public Task<List<Move>> PlanMovesAsync(GameState state)
    {
        var myteam = state.TeamList.FirstOrDefault(t => t.Id == state.CurrentTeamId);
        if (myteam is null)
            return Task.FromResult(new List<Move>());

        var moves = new List<Move>();
        var archers = state.Units.Where(u => u.Team == state.CurrentTeamId && u.Type == UnitType.Archer.ToString()).ToList();
        var knights = state.Units.Where(u => u.Team == state.CurrentTeamId && u.Type == UnitType.Knight.ToString()).ToList();

        foreach (var archer in archers)
        {
            var adjacentPositions = archer.Location.Neighbors();
            foreach (var knight in knights)
            {
                if (adjacentPositions.Any(pos => pos.Equals(knight.Location)))
                {
                    // Knight is already in position
                    continue;
                }

                var emptyPosition = adjacentPositions.FirstOrDefault(pos => !state.Units.Any(u => u.Location.Equals(pos)));
                bool within5OfArcher = archers.Any(a => a.Location.Distance(knight.Location) <= 5);
                if (emptyPosition is not null)
                {
                    moves.Add(SharedMoves.StepToClosest(knight, Unit.Factory("", 0, emptyPosition), state));
                    knight.Moves -= 1;
                }
            }

            // if this archer is mmore than 5 spaces from the other archer, move closer
            var otherArcher = archers.FirstOrDefault(a => a.Id != archer.Id);
            if (otherArcher != null && archer.Location.Distance(otherArcher.Location) > 1)
            {
                moves.Add(SharedMoves.StepToClosest(archer, otherArcher, state));
                archer.Moves -= 1;
            }
        }

        return Task.FromResult(moves);
    }
}

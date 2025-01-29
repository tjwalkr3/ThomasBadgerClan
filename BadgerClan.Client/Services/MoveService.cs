﻿using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

namespace BadgerClan.Client.Services;

public class MoveService : IMoveService
{
    private PlayMode _playMode = PlayMode.Stop;

    public bool SetPlayMode(int playMode)
    {
        if (playMode >= 0 && playMode <= 2)
        {
            _playMode = (PlayMode)playMode;
            return true;
        }
        else
        {
            return false;
        }
    }

    private GameState GetGameState(MoveRequest request, IBot bot)
    {
        List<Unit> allUnits = request.Units
            .Select(dto => Unit.Factory(
                dto.Type,
                dto.Id,
                dto.Attack,
                dto.AttackDistance,
                dto.Health,
                dto.MaxHealth,
                dto.Moves,
                dto.MaxMoves,
                dto.Location,
                dto.Team))
            .ToList();

        return new GameState(
            request.GameId,
            request.BoardSize,
            request.TurnNumber,
            allUnits, request.TeamIds,
            new Team(request.YourTeamId));
    }

    private async Task<List<Move>> GetMoves(MoveRequest request)
    {
        IBot bot = new NothingBot();

        switch (_playMode)
        {
            case PlayMode.Attack:
                bot = new RunAndGun();
                break;
            case PlayMode.Defend:
                bot = new Turtle();
                break;
            case PlayMode.Stop:
                break;
        }

        GameState gameState = GetGameState(request, bot);
        return await bot.PlanMovesAsync(gameState);
    }

    public async Task<MoveResponse> GetResponse(MoveRequest request)
    {
        List<Move> moves = await GetMoves(request);
        return new MoveResponse(moves);
    }
}

enum PlayMode { Attack, Defend, Stop }
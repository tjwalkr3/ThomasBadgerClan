using BadgerClan.Logic;

namespace BadgerClan.Client.Services;

public class MoveService : IMoveService
{
    private PlayMode _playMode = PlayMode.Stop;

    public bool SetPlayMode(int playMode)
    {
        if (playMode >= 0 && playMode <= 3)
        {
            _playMode = (PlayMode)playMode;
            return true;
        }
        else
        {
            return false;
        }
    }

    public MoveResponse GetMoves(MoveRequest request)
    {
        List<Move> myMoves = [];
        switch (_playMode)
        {
            case PlayMode.Attack:
                Attack(request, myMoves);
                break;
            case PlayMode.Defend:
                Defend(request, myMoves);
                break;
            case PlayMode.Run:
                Run(request, myMoves);
                break;
            case PlayMode.Stop:
                break;
        }
        return new MoveResponse(myMoves);
    }

    private void Attack(MoveRequest request, List<Move> myMoves)
    {
        
    }

    private void Defend(MoveRequest request, List<Move> myMoves)
    {
        
    }

    private void Run(MoveRequest request, List<Move> myMoves)
    {
        
    }
}

enum PlayMode { Attack, Defend, Run, Stop }
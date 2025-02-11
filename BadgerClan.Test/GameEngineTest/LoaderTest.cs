using BadgerClan.Client.Logic;
using BadgerClan.Client.Logic.Bot;

namespace BadgerClan.Test.GameEngineTest;

public class LoaderTest
{

    //[Fact]
    public void FirstGame()
    {
        var state = GameLoader.CreateGame(4, Turtle.Make);

        Assert.Equal(4, state.TeamCount);
    }



}
using System;
using System.Collections;
using UnityEditor;

public interface IGameEvents
{
    void onStartGame(object sender, EventArgs e);
    void onNewRound(object sender, EventArgs e);
    void onDiscussion(object sender, EventArgs e);
    void onVote(object sender, EventArgs e);
    void onSubmitVote(object sender, EventArgs e);
    void onResult(object sender, EventArgs e);
}

public interface IGameEventsTimeElapsed
{
    public void onStartGameTimeElapsed(object sender, int e);
    public void onNewRoundTimeElapsed(object sender, int e);
    public void onDiscussionTimeElapsed(object sender, int e);
    public void onVoteTimeElapsed(object sender, int e);
    public void onSubmitVoteTimeElapsed(object sender, int e);
    public void onResultTimeElapsed(object sender, int e);
}



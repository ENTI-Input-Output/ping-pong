using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
    private int _turnID;

    [SerializeField]
    private int[] _scores;

    //Start
    private void Start()
    {
        _turnID = 0;
        _scores = new int[] { 0, 0 };
    }

    //Update
    private void Update()
    {
        //TODO
    }

    public int GetPlayerScore(int playerID)
    {
        return _scores[playerID];
    }

    public int[] GetGlobalScore()
    {
        return _scores;
    }

    public void UpdateScore(int playerID)
    {
        _scores[playerID]++;
    }

    public void UpdateScore(int playerID, int amount)
    {
        _scores[playerID] += amount;
    }

    public void ChangeTurn()
    {
        if (_turnID == 0)
        {
            _turnID = 1;
        }
        else
        {
            _turnID = 0;
        }
    }


    //************************************ LOGIC *********************************************
    //TODO
    enum surfaceType { side, field, floor, paddle, net };
    surfaceType lastHitSurface;
    surfaceType currentHitSurface;

    private void OnBallCollision()
    {
        switch (currentHitSurface)
        {
            case surfaceType.floor:
                //Check lastHitSurface
                //Was it my paddle?
                //  if(true) => score++ for the other player, as I sent the ball too far
                //Was it the other player's field?
                //  if(true) => score++ for me, as I've just scored a point (the other player missed the hit)
                break;

            case surfaceType.side:
                //Check player in turn
                //score++ for the other player, as the player in turn missed the hit
                break;

            case surfaceType.field:
                //What field?
                //is it owned by the player in turn?
                //  if(true) => score++ for the other player, as it is the second time it bounces on this field
                //  else => just wait
                break;

            case surfaceType.net:
                //just wait
                break;

            case surfaceType.paddle:
                //Did the ball hit my field first?
                //  if(true) => just wait
                //  else => Is my paddle over my field?
                //      if(true) => score++ for the other player, as I should've waited for it to bounce
                //      else => score++ for me, as the ball was going to hit the floor
                break;
        }

        lastHitSurface = currentHitSurface;
    }


    //enum surfaceType {side, field, floor, paddle, net}
    //surfaceType lastHitSurface
    //surfaceType currentHitSurface
    //int currentTurn
    //int scorePlayer0
    //int scorePlayer1

    //onBallCollision()
    //{
    //  switch(currentHitSurface)
    //  if(hit field of current turn)
    //  {
    //      if(currentTurn == 0)
    //          scorePlayer1++;
    //      else
    //          scorePlayer0++;
    //  }
    //}




}


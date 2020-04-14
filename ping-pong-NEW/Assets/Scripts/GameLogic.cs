using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum surfaceType { side, field, floor, paddle, net };


public class GameLogic : MonoBehaviour
{
    [System.Serializable]
    struct Game
    {
        public int id;
        public Vector2 score;
        public int winnerID;
    }

    private surfaceType _lastHitSurface;
    private surfaceType _currentHitSurface;
    private bool _gameActive;
    private int _currentGame;
    [SerializeField]
    private int _turnID;
    [Header("Games and score")]
    public int MaxGames;
    public int MaxGamePoints;
    public int AdvThreshold;
    [SerializeField]
    private List<Game> _games;


    //Start
    private void Start()
    {
        _turnID = 0;
        //_scores = new int[] { 0, 0 };
    }

    //Update
    private void Update()
    {
        if((_games[_currentGame].score.x >= MaxGamePoints && _games[_currentGame].score.x - _games[_currentGame].score.y >= AdvThreshold)
            || (_games[_currentGame].score.y >= MaxGamePoints && _games[_currentGame].score.y - _games[_currentGame].score.x >= AdvThreshold))
        {
            //Game finished
            //Reset ball
            //_currentGame++;
            //Set _games[_currentGame].winner
        }
    }

    public int GetPlayerScore(int playerID)
    {
        //TODO
        //return _scores[playerID];
        return 0;
    }

    public int[] GetGlobalScore()
    {
        //TODO
        //return _scores;
        return new int[2];
    }

    public void UpdateScore(int playerID)
    {
        //TODO
        //_scores[playerID]++;
    }

    public void UpdateScore(int playerID, int amount)
    {
        //TODO
        //_scores[playerID] += amount;
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


    private void OnBallCollision()
    {
        //if ()
        //{

        //}

        switch (_currentHitSurface)
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

        _lastHitSurface = _currentHitSurface;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameLogic : MonoBehaviour
{
    [System.Serializable]
    struct Game
    {
        public int id;
        public int[] score;
        public int winnerID;
    }

    [SerializeField]
    private Surface _lastHitSurface;
    [SerializeField]
    private Surface _currentHitSurface;
    //private bool _gameActive;
    private int _currentGame;
    [SerializeField]
    private int _turnID;

    [Header("Intermission Times")]
    public float PointIntermissionTime = 3f;
    public float GameIntermissionTime = 5f;

    [Header("Game Configuration")]
    public int GamesToWin = 3;
    public int MaxGames = 5;
    public int MaxGamePoints = 11;
    public int AdvThreshold = 2;

    [Header("Score")]
    [SerializeField]
    private List<Game> _games;

    #region Singleton
    public static GameLogic Instance;

    //Awake
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    //Start
    private void Start()
    {
        _turnID = 0;
        _currentGame = 0;
        _currentHitSurface = _lastHitSurface = null;
        _games = new List<Game>();
    }

    //Update
    private void Update()
    {
        //if((_games[_currentGame].score.x >= MaxGamePoints && _games[_currentGame].score.x - _games[_currentGame].score.y >= AdvThreshold)
        //    || (_games[_currentGame].score.y >= MaxGamePoints && _games[_currentGame].score.y - _games[_currentGame].score.x >= AdvThreshold))
        //{
        //    //Game finished
        //    //Reset ball
        //    //_currentGame++;
        //    //Set _games[_currentGame].winner
        //}
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

    private void ResetBall()
    {
        //TODO
        //Put the ball in the field of the player in turn
    }


    //************************************ LOGIC *********************************************
    //TODO


    public void OnBallCollision(Surface surface)
    {
        _currentHitSurface.SurfaceType = surface.SurfaceType;

        switch (_currentHitSurface.SurfaceType)
        {
            case SurfaceType.Floor:
                Debug.Log("The ball hit the floor");
                switch (_lastHitSurface.SurfaceType)
                {
                    case SurfaceType.Paddle:
                        if (_lastHitSurface.transform.parent.GetComponent<PlayerController>().PlayerID == 0)
                        {
                            _games[_currentGame].score[1]++;
                        }
                        else
                        {
                            _games[_currentGame].score[0]++;
                        }
                        break;

                    case SurfaceType.Field:
                    case SurfaceType.Net:
                        if (_turnID == 0)
                        {
                            _games[_currentGame].score[1]++;
                        }
                        else
                        {
                            _games[_currentGame].score[0]++;
                        }
                        break;

                    default:
                        Debug.Log("Surface case not controlled. Ball hit: " + _lastHitSurface.SurfaceType);
                        break;
                }
                ResetBall();
                break;

            case SurfaceType.Field:
                Debug.Log("The ball hit the field number " + surface.FieldNum);
                //Check lastHitSurface
                //Was it a field?
                //is it owned by the player in turn?
                //  if(true) => score++ for the other player, as it is the second time it bounces on this field
                //  else => just wait
                break;

            case SurfaceType.Net:
                Debug.Log("The ball hit the net");
                //just wait
                break;

            case SurfaceType.Paddle:
                Debug.Log("The ball hit a paddle");
                //Did the ball hit my field first?
                //  if(true) => just wait
                //  else => Is my paddle over my field?
                //      if(true) => score++ for the other player, as I should've waited for it to bounce
                //      else => score++ for me, as the ball was going to hit the floor
                break;

            default:
                Debug.Log("Surface not recognized");
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

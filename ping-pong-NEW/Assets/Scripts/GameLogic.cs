﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CustomPlayer
{
    public int PhotonUserID;
    public int OwnID;

    public CustomPlayer(int photonUserID, int ownID)
    {
        PhotonUserID = photonUserID;
        OwnID = ownID;
    }
}


public class GameLogic : MonoBehaviour
{
    [System.Serializable]
    public class Game
    {
        public int GameID;
        public int[] Score;
        public int WinnerID;

        public Game(int id)
        {
            GameID = id;
            Score = new int[] { 0, 0 };
            WinnerID = -1;
        }
    }

    //public int LocalPlayerID;
    public int OpponentID;

    [SerializeField]
    private SurfaceType _lastHitSurface;
    [SerializeField]
    private SurfaceType _currentHitSurface;

    private GameObject _lastPaddle;

    //private bool _gameActive;
    private int _currentGame;
    [SerializeField]
    private int _turnID;

    [SerializeField]
    private bool _isFirstHit;
    //public bool[] PaddleOverField = new bool[] { false, false };
    public Dictionary<int, bool> PaddleOverField = new Dictionary<int, bool>();
    private BallController _ballReference;

    [Header("Intermission Times")]
    public float PointIntermissionTime = 3f;
    public float GameIntermissionTime = 5f;
    private float _pointTimer;
    private float _gameTimer;
    private bool _nextGame, _nextPoint;

    [Header("Game Configuration")]
    public int GamesToWin = 3;
    public int MaxGames = 5;
    public int MaxGamePoints = 11;
    public int PointsDiff = 2;
    private int _matchWinner;

    [Header("Score")]
    [SerializeField]
    private List<Game> _games;
    private ScoreBoardNetwork _scoreBoard;

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
        _turnID = -1;
        _currentGame = 1;
        _currentHitSurface = _lastHitSurface = SurfaceType.None;
        _games = new List<Game>();
        _matchWinner = -1;
        _nextGame = _nextPoint = false;
        _isFirstHit = true;

        _gameTimer = _pointTimer = 0;

        _ballReference = GameObject.FindGameObjectWithTag("ball").GetComponent<BallController>();

        //Insert first game
        _games.Add(new Game(_currentGame));

        _scoreBoard = GameObject.Find("ScoreBoard").GetComponent<ScoreBoardNetwork>();

        //PROVISIONAL
        PaddleOverField[1] = false;
        PaddleOverField[2] = false;
    }

    //Update
    private void Update()
    {
        if (_nextPoint)
        {
            _pointTimer += Time.deltaTime;
            if (_pointTimer >= PointIntermissionTime)
            {
                _pointTimer = 0;
                _nextPoint = false;
                _ballReference.IsLocked = false;
            }
        }

        if (_nextGame)
        {
            _gameTimer += Time.deltaTime;
            if (_gameTimer >= GameIntermissionTime)
            {
                _gameTimer = 0;
                _nextGame = false;
                _ballReference.IsLocked = false;
            }
        }
    }

    public int GetPlayerScore(int playerID)
    {
        return _games[_currentGame].Score[playerID];
    }

    public int[] GetGameScore()
    {
        return _games[_currentGame].Score;
    }

    public int GetPlayerWins(int playerID)
    {
        int wins = 0;

        foreach (Game game in _games)
        {
            if (game.WinnerID == playerID)
            {
                wins++;
            }
        }
        return wins;
    }

    public int GetCurrentGame()
    {
        return _currentGame;
    }

    //TODO: CHECK THIS FUNCTION => ITERATE OVER THE LIST OF PLAYERS IN THE ROOM
    private void ChangeTurn()
    {
        if (_turnID == 2)
            _turnID = 1;
        else
            _turnID = 2;
    }

    public void AssignTurn(int playerID)
    {
        _turnID = playerID;
    }

    private void EndMatch()
    {
        //TODO: SHOW WHATEVER WE WANT AND RETURN TO MENU/PLAY AGAIN
        Debug.Log("Match has ended");
    }

    //private void SetScore(int playerID)
    //{
    //    _games[_currentGame].Score[playerID]++;
    //    if (_games[_currentGame].Score[playerID] >= MaxGamePoints && _games[_currentGame].Score[playerID] - _games[_currentGame].Score[GetOtherPlayer()] >= PointsDiff)    //This game has ended
    //    {
    //        _games[_currentGame].WinnerID = playerID;

    //        int player0Wins = GetPlayerWins(0);
    //        int player1Wins = GetPlayerWins(1);

    //        if (player0Wins >= Mathf.RoundToInt(MaxGames / 2))
    //        {
    //            _matchWinner = 0;
    //        }
    //        else if (player1Wins >= Mathf.RoundToInt(MaxGames / 2))
    //        {
    //            _matchWinner = 1;
    //        }
    //        else
    //        {
    //            //Add new game
    //            _games.Add(new Game(++_currentGame));

    //            _isFirstHit = true;

    //            _nextGame = true;
    //            _ballReference.IsLocked = true;
    //        }
    //    }
    //    else
    //    {
    //        _nextPoint = true;
    //        _ballReference.IsLocked = true;
    //    }
    //}


    //************************************ LOGIC *********************************************
    public void OnBallCollision(Surface surface)
    {
        _currentHitSurface = surface.SurfaceType;

        switch (_currentHitSurface)
        {
            case SurfaceType.Floor:
                Debug.Log("The ball hit the floor");

                switch (_lastHitSurface)
                {
                    //If the ball hit a field, a paddle or the net, the point goes to the player who doesn't have the turn.
                    case SurfaceType.Field:
                    case SurfaceType.Paddle:
                    case SurfaceType.Net:
                        //If the local id does not match the turn id, add a point to the local player and send it to the other player
                        if (_turnID != PhotonNetwork.CurrentRoom.masterClientId)
                        {
                            _scoreBoard.UpdateLocalPlayerScore();
                        }
                        break;

                    default:
                        Debug.Log("Surface case not controlled. Last hit was: " + _lastHitSurface);
                        break;
                }
                break;

            case SurfaceType.Field:
                Debug.Log("The ball hit the field with number " + surface.FieldNum);

                //If the ball hits the field of the player that has the turn, the point goes for the other player
                if (surface.FieldNum == _turnID && _turnID != PhotonNetwork.CurrentRoom.masterClientId)
                {
                    if (!_isFirstHit)
                    {
                        _scoreBoard.UpdateLocalPlayerScore();
                    }
                }
                else
                {
                    //TODO: CHANGE TURN (I NEED THE OPPONENT ID => WILL WE HAVE OBSERVERS? IF NOT, MANUALLY ASSIGN TO 1 OR 2 (DEPENDING ON THE LOCAL ID))
                    ChangeTurn();
                }
                break;

            case SurfaceType.Net:
                Debug.Log("The ball hit the net");
                break;

            case SurfaceType.Paddle:
                Debug.Log("The ball hit a paddle");

                //HOW IT SHOULD BE STRUCTURED
                switch (_lastHitSurface)
                {
                    case SurfaceType.Net:
                    case SurfaceType.Paddle:
                        //The local player has the turn
                        if (_turnID == PhotonNetwork.CurrentRoom.masterClientId)
                        {
                            //The opponent hit the ball
                            if (surface.transform.parent.GetComponent<PlayerController>() && surface.transform.parent.GetComponent<PlayerController>().PlayerID != PhotonNetwork.CurrentRoom.masterClientId)
                            {
                                //The opponent had the paddle over the field
                                if (PaddleOverField[OpponentID])
                                {
                                    _scoreBoard.UpdateLocalPlayerScore();
                                }
                            }

                        }
                        else //The local doesn't have the turn
                        {
                            //The local player hit the ball
                            if (surface.transform.parent.GetComponent<PlayerController>() && surface.transform.parent.GetComponent<PlayerController>().PlayerID == PhotonNetwork.CurrentRoom.masterClientId)
                            {
                                //The local player didn't have the paddle over the field
                                if (!PaddleOverField[PhotonNetwork.CurrentRoom.masterClientId])
                                {
                                    _scoreBoard.UpdateLocalPlayerScore();
                                }
                            }
                            else
                            {
                                _scoreBoard.UpdateLocalPlayerScore();
                            }
                        }
                        break;

                    default:
                        Debug.Log("Surface case not controlled. Last hit was: " + _lastHitSurface);
                        break;
                }

                //It is no more the first hit
                if (_isFirstHit)
                    _isFirstHit = false;
                break;

            default:
                Debug.Log("Surface not recognized");
                break;
        }

        _lastHitSurface = _currentHitSurface;
    }
}

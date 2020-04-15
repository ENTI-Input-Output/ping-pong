using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private bool IsFirstHit = true;
    public bool[] PaddleOverField = new bool[] { false, false };

    [Header("Intermission Times")]
    public float PointIntermissionTime = 3f;
    public float GameIntermissionTime = 5f;

    [Header("Game Configuration")]
    public int GamesToWin = 3;
    public int MaxGames = 5;
    public int MaxGamePoints = 11;
    public int AdvThreshold = 2;
    private int _matchWinner;

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
        _currentGame = 1;
        _currentHitSurface = _lastHitSurface = SurfaceType.None;
        _games = new List<Game>();
        _matchWinner = -1;

        //Insert first game
        _games.Add(new Game(_currentGame));
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
        return _games[_currentGame].Score[playerID];
    }

    public int[] GetGameScore()
    {
        return _games[_currentGame].Score;
    }

    public int GetMatchScore()
    {
        //TODO
        //return how many games has each player won
        return 0;
    }

    public int GetCurrentGame()
    {
        return _currentGame;
    }

    private int GetOtherPlayer()
    {
        if (_turnID == 0)
            return 1;

        return 0;
    }

    private void ChangeTurn()
    {
        _turnID = GetOtherPlayer();
    }

    private void SetScore(int playerID)
    {
        //TODO
        _games[_currentGame].Score[playerID]++;
        if (_games[_currentGame].Score[playerID] >= MaxGamePoints && _games[_currentGame].Score[playerID] - _games[_currentGame].Score[GetOtherPlayer()] >= AdvThreshold)    //This game has ended
        {
            if (_currentGame >= MaxGames)
            {
                //TODO
            }
            else
            {
                _games[_currentGame].WinnerID = playerID;

                //Add new game
                _games.Add(new Game(++_currentGame));

                IsFirstHit = true;
            }
        }

        //Start new point/game
        //Set serve
    }


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
                    case SurfaceType.Paddle:
                    case SurfaceType.Field:
                    case SurfaceType.Net:
                        _games[_currentGame].Score[GetOtherPlayer()]++;
                        break;

                    default:
                        Debug.Log("Surface case not controlled. Last hit was: " + _lastHitSurface);
                        break;
                }
                break;

            case SurfaceType.Field:
                Debug.Log("The ball hit the field with number " + surface.FieldNum);
                if (surface.FieldNum == _turnID)
                {
                    if (!IsFirstHit)
                        _games[_currentGame].Score[GetOtherPlayer()]++;
                }
                else
                {
                    ChangeTurn();
                }
                break;

            case SurfaceType.Net:
                Debug.Log("The ball hit the net");
                break;

            case SurfaceType.Paddle:
                Debug.Log("The ball hit a paddle");
                if (_lastHitSurface != SurfaceType.Field)
                {
                    if (PaddleOverField[_turnID])
                    {
                        _games[_currentGame].Score[GetOtherPlayer()]++;
                    }
                    else
                    {
                        _games[_currentGame].Score[_turnID]++;
                    }
                }

                if (IsFirstHit)
                    IsFirstHit = false;
                break;

            default:
                Debug.Log("Surface not recognized");
                break;
        }

        _lastHitSurface = _currentHitSurface;
    }
}

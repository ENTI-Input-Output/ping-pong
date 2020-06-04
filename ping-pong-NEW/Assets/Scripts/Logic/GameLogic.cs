using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

[System.Serializable]
public class Game
{
    public int GameID;
    public Dictionary<int, int> Score;
    public int WinnerID;

    public Game(int id, int playerId, int opponentId)
    {
        GameID = id;
        Score = new Dictionary<int, int>();
        WinnerID = -1;

        Score[playerId] = 0;
        Score[opponentId] = 0;
    }
}


public abstract class GameLogic : MonoBehaviour
{
    [Header("Players & Surfaces")]
    //public int LocalPlayerID;
    public int OpponentID;
    protected bool _lookForOponent;

    [SerializeField]
    protected SurfaceType _lastHitSurface;
    [SerializeField]
    protected SurfaceType _currentHitSurface;

    protected GameObject _lastPaddle;

    //private bool _gameActive;
    public int CurrentGame;
    [SerializeField]
    protected int _turnID;

    [SerializeField]
    protected bool _isFirstHit;
    public int ServeTurnID = 1;
    //public bool[] PaddleOverField = new bool[] { false, false };
    public Dictionary<int, bool> PaddleOverField = new Dictionary<int, bool>();
    [SerializeField]
    protected BallController _ballReference;

    [Header("Intermission Times")]
    public float PointIntermissionTime = 3f;
    public float GameIntermissionTime = 5f;
    protected float _pointTimer;
    protected float _gameTimer;
    [SerializeField]
    protected bool _nextGame, _nextPoint;

    [Header("Game Configuration")]
    public int GamesToWin = 3;
    public int MaxGames = 5;
    public int MaxGamePoints = 11;
    public int PointsDiff = 2;
    protected int _matchWinner;
    public int ServePointsDiff = 2;

    [Header("Score")]
    public List<Game> Games;
    protected ScoreBoardNetwork _scoreBoard;
    //[HideInInspector]
    public bool MatchEnded = false;

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
    public virtual void Start()
    {
        _turnID = -1;
        CurrentGame = 0;
        _matchWinner = -1;
        _gameTimer = _pointTimer = 0;

        _nextGame = _nextPoint = false;
        _isFirstHit = true;
        _lookForOponent = true;

        _currentHitSurface = _lastHitSurface = SurfaceType.None;
        Games = new List<Game>();
        //Games.Add(new Game(CurrentGame, PhotonNetwork.LocalPlayer.ActorNumber, 2));


        _ballReference = GameObject.FindGameObjectWithTag("ball").GetComponent<BallController>();
        _scoreBoard = GameObject.Find("ScoreBoard").GetComponent<ScoreBoardNetwork>();

    }

    //Update
    //private void Update()
    //{
    //    //Check if there's another player and get its ID to set the OpponentID in GameLogic
    //    if (_lookForOponent)
    //    {
    //        foreach (Player player in PhotonNetwork.PlayerListOthers)
    //        {
    //            if (player.NickName == "Player")
    //            {
    //                OpponentID = player.ActorNumber;
    //                _lookForOponent = false;

    //                //Initialize data that uses player IDs
    //                //Paddle Over Field
    //                PaddleOverField[PhotonNetwork.LocalPlayer.ActorNumber] = false;
    //                PaddleOverField[OpponentID] = false;

    //                //Insert first game
    //                Games.Add(new Game(CurrentGame, PhotonNetwork.LocalPlayer.ActorNumber, OpponentID));
    //            }
    //        }
    //    }
    //}

    public int GetPlayerScore(int playerID)
    {
        return Games[CurrentGame].Score[playerID];
    }

    public Dictionary<int, int> GetGameScore()
    {
        return Games[CurrentGame].Score;
    }

    public int GetPlayerWins(int playerID)
    {
        int wins = 0;

        foreach (Game game in Games)
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
        return CurrentGame;
    }

    //TODO: CHECK THIS FUNCTION => ITERATE OVER THE LIST OF PLAYERS IN THE ROOM
    protected void ChangeHitTurn()
    {
        if (_turnID == PhotonNetwork.LocalPlayer.ActorNumber)
            _turnID = OpponentID;
        else
            _turnID = PhotonNetwork.LocalPlayer.ActorNumber;
    }

    protected void ChangeServeTurn()
    {
        if (ServeTurnID == PhotonNetwork.LocalPlayer.ActorNumber)
            ServeTurnID = OpponentID;
        else
            ServeTurnID = PhotonNetwork.LocalPlayer.ActorNumber;
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


    public abstract void SetScore();

    //************************************ LOGIC *********************************************
    public abstract void OnBallCollision(Surface surface);
}

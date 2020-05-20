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


public class GameLogic : MonoBehaviour
{
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

    [Header("Players & Surfaces")]
    //public int LocalPlayerID;
    public int OpponentID;
    private bool _lookForOponent;

    [SerializeField]
    private SurfaceType _lastHitSurface;
    [SerializeField]
    private SurfaceType _currentHitSurface;

    private GameObject _lastPaddle;

    //private bool _gameActive;
    public int CurrentGame;
    [SerializeField]
    private int _turnID;

    [SerializeField]
    private bool _isFirstHit;
    public int ServeTurnID = 1;
    //public bool[] PaddleOverField = new bool[] { false, false };
    public Dictionary<int, bool> PaddleOverField = new Dictionary<int, bool>();
    [SerializeField]
    private BallController _ballReference;

    [Header("Intermission Times")]
    public float PointIntermissionTime = 3f;
    public float GameIntermissionTime = 5f;
    private float _pointTimer;
    private float _gameTimer;
    [SerializeField]
    private bool _nextGame, _nextPoint;

    [Header("Game Configuration")]
    public int GamesToWin = 3;
    public int MaxGames = 5;
    public int MaxGamePoints = 11;
    public int PointsDiff = 2;
    private int _matchWinner;

    [Header("Score")]
    [SerializeField]
    public List<Game> Games;
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
        CurrentGame = 0;
        _matchWinner = -1;
        _gameTimer = _pointTimer = 0;

        _nextGame = _nextPoint = false;
        _isFirstHit = true;
        _lookForOponent = true;

        _currentHitSurface = _lastHitSurface = SurfaceType.None;
        Games = new List<Game>();


        _ballReference = GameObject.FindGameObjectWithTag("ball").GetComponent<BallController>();
        _scoreBoard = GameObject.Find("ScoreBoard").GetComponent<ScoreBoardNetwork>();

    }

    //Update
    private void Update()
    {
        //Check if there's another player and get its ID to set the OpponentID in GameLogic
        if (_lookForOponent)
        {
            foreach (Player player in PhotonNetwork.PlayerListOthers)
            {
                if (player.NickName == "Player")
                {
                    OpponentID = player.ActorNumber;
                    _lookForOponent = false;

                    //Initialize data that uses player IDs
                    //Paddle Over Field
                    PaddleOverField[PhotonNetwork.LocalPlayer.ActorNumber] = false;
                    PaddleOverField[OpponentID] = false;

                    //Insert first game
                    Games.Add(new Game(CurrentGame, PhotonNetwork.LocalPlayer.ActorNumber, OpponentID));
                }
            }
        }


        if (_nextPoint)
        {
            _pointTimer += Time.deltaTime;
            if (_pointTimer >= PointIntermissionTime)
            {
                _pointTimer = 0;
                _nextPoint = false;
                //_ballReference.IsLocked = false;
            }
        }

        if (_nextGame)
        {
            _gameTimer += Time.deltaTime;
            if (_gameTimer >= GameIntermissionTime)
            {
                _gameTimer = 0;
                _nextGame = false;
                //_ballReference.IsLocked = false;
            }
        }
    }

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
    private void ChangeTurn()
    {
        if (_turnID == PhotonNetwork.LocalPlayer.ActorNumber)
            _turnID = OpponentID;
        else
            _turnID = PhotonNetwork.LocalPlayer.ActorNumber;
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

    //TODO: CALL THIS FUNCTION FROM SCOREBOARD FUNCTION
    //public void SetScore()
    //{
    //    //Games[CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber]++;
    //    if (Games[CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber] >= MaxGamePoints && Games[CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber] - Games[CurrentGame].Score[OpponentID] >= PointsDiff)    //This game has ended
    //    {
    //        Games[CurrentGame].WinnerID = PhotonNetwork.LocalPlayer.ActorNumber;

    //        //UpdateLocalMatch => Scoreboard
    //        _scoreBoard.UpdateLocalMatchScore();

    //        int localPlayer = GetPlayerWins(PhotonNetwork.LocalPlayer.ActorNumber);
    //        int opponent = GetPlayerWins(OpponentID);

    //        if (localPlayer >= Mathf.RoundToInt(MaxGames / 2))
    //        {
    //            _matchWinner = PhotonNetwork.LocalPlayer.ActorNumber;
    //        }
    //        else if (opponent >= Mathf.RoundToInt(MaxGames / 2))
    //        {
    //            _matchWinner = OpponentID;
    //        }
    //        else
    //        {
    //            //Add new game
    //            Games.Add(new Game(++CurrentGame, PhotonNetwork.LocalPlayer.ActorNumber, OpponentID));

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

    //Will try setting the opponent's score
    public void SetScore()
    {
        //Lock the ball to avoid scoring more points until new serve
        _ballReference.IsLocked = true;

        //Games[CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber]++;
        if (Games[CurrentGame].Score[OpponentID] >= MaxGamePoints && Games[CurrentGame].Score[OpponentID] - Games[CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber] >= PointsDiff)    //This game has ended
        {
            Games[CurrentGame].WinnerID = OpponentID;

            //UpdateLocalMatch => Scoreboard
            _scoreBoard.UpdateLocalMatchScore();

            int localPlayer = GetPlayerWins(PhotonNetwork.LocalPlayer.ActorNumber);
            int opponent = GetPlayerWins(OpponentID);

            if (localPlayer >= Mathf.RoundToInt(MaxGames / 2))
            {
                _matchWinner = PhotonNetwork.LocalPlayer.ActorNumber;
            }
            else if (opponent >= Mathf.RoundToInt(MaxGames / 2))
            {
                _matchWinner = OpponentID;
            }
            else
            {
                //Add new game
                Games.Add(new Game(++CurrentGame, PhotonNetwork.LocalPlayer.ActorNumber, OpponentID));

                //_isFirstHit = true;

                _nextGame = true;
                //_ballReference.IsLocked = true;
            }
        }
        else
        {
            _nextPoint = true;
            //_ballReference.IsLocked = true;
        }

        _isFirstHit = true;
        _currentHitSurface = _lastHitSurface = SurfaceType.None;
    }


    //************************************ LOGIC *********************************************
    public void OnBallCollision(Surface surface)
    {
        if (!_ballReference.IsLocked)
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
                            if (_turnID == PhotonNetwork.LocalPlayer.ActorNumber)
                            {
                                //Add score and send it to all players in room
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
                    if (surface.FieldNum == _turnID && _turnID == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        if (!_isFirstHit)
                        {
                            //Add score and send it to all players in room
                            _scoreBoard.UpdateLocalPlayerScore();
                        }
                        else
                        {
                            //It is no more the first hit
                            _isFirstHit = false;
                        }
                    }
                    else
                    {
                        //CHANGE TURN
                        ChangeTurn();
                    }
                    break;

                case SurfaceType.Net:
                    Debug.Log("The ball hit the net");
                    break;

                case SurfaceType.Paddle:
                    Debug.Log("The ball hit a paddle");

                    switch (_lastHitSurface)
                    {
                        case SurfaceType.Net:
                        case SurfaceType.Paddle:
                            //The local player has the turn
                            if (_turnID == PhotonNetwork.LocalPlayer.ActorNumber)
                            {
                                //The opponent hit the ball
                                if (surface.transform.parent.GetComponent<PlayerController>() && surface.transform.parent.GetComponent<PlayerController>().PlayerID != PhotonNetwork.LocalPlayer.ActorNumber)
                                {
                                    //The opponent had the paddle over the field
                                    if (PaddleOverField[OpponentID])
                                    {
                                        //Add score and send it to all players in room
                                        _scoreBoard.UpdateLocalPlayerScore();
                                    }
                                }

                            }
                            else //The local doesn't have the turn
                            {
                                //The local player hit the ball
                                if (surface.transform.parent.GetComponent<PlayerController>() && surface.transform.parent.GetComponent<PlayerController>().PlayerID == PhotonNetwork.LocalPlayer.ActorNumber)
                                {
                                    //The local player didn't have the paddle over the field
                                    if (!PaddleOverField[PhotonNetwork.LocalPlayer.ActorNumber])
                                    {
                                        //Add score and send it to all players in room
                                        _scoreBoard.UpdateLocalPlayerScore();
                                    }
                                }
                                else
                                {
                                    //Add score and send it to all players in room
                                    _scoreBoard.UpdateLocalPlayerScore();
                                }
                            }
                            break;

                        default:
                            Debug.Log("Surface case not controlled. Last hit was: " + _lastHitSurface);
                            break;
                    }
                    break;

                default:
                    Debug.Log("Surface not recognized");
                    break;
            }

            _lastHitSurface = _currentHitSurface;
        }
    }
}

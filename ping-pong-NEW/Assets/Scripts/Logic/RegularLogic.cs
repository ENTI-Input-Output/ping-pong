using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RegularLogic : GameLogic
{
    //[Header("Players & Surfaces")]
    //public int LocalPlayerID;
    //public int OpponentID;
    //private bool _lookForOponent;

    //[SerializeField]
    //private SurfaceType _lastHitSurface;
    //[SerializeField]
    //private SurfaceType _currentHitSurface;

    //private GameObject _lastPaddle;

    //private bool _gameActive;
    //public int CurrentGame;
    //[SerializeField]
    //private int _turnID;

    //[SerializeField]
    //private bool _isFirstHit;
    //public int ServeTurnID = 1;
    ////public bool[] PaddleOverField = new bool[] { false, false };
    //public Dictionary<int, bool> PaddleOverField = new Dictionary<int, bool>();
    //[SerializeField]
    //private BallController _ballReference;

    //[Header("Intermission Times")]
    //public float PointIntermissionTime = 3f;
    //public float GameIntermissionTime = 5f;
    //private float _pointTimer;
    //private float _gameTimer;
    //[SerializeField]
    //private bool _nextGame, _nextPoint;

    //[Header("Game Configuration")]
    //public int GamesToWin = 3;
    //public int MaxGames = 5;
    //public int MaxGamePoints = 11;
    //public int PointsDiff = 2;
    //private int _matchWinner;
    //public int ServePointsDiff = 2;

    //[Header("Score")]
    //public List<Game> Games;
    //private ScoreBoardNetwork _scoreBoard;
    ////[HideInInspector]
    //public bool MatchEnded = false;

    //Start
    //private void Start()
    //{
    //    _turnID = -1;
    //    CurrentGame = 0;
    //    _matchWinner = -1;
    //    _gameTimer = _pointTimer = 0;

    //    _nextGame = _nextPoint = false;
    //    _isFirstHit = true;
    //    _lookForOponent = true;

    //    _currentHitSurface = _lastHitSurface = SurfaceType.None;
    //    Games = new List<Game>();
    //    //Games.Add(new Game(CurrentGame, PhotonNetwork.LocalPlayer.ActorNumber, 2));


    //    _ballReference = GameObject.FindGameObjectWithTag("ball").GetComponent<BallController>();
    //    _scoreBoard = GameObject.Find("ScoreBoard").GetComponent<ScoreBoardNetwork>();

    //}

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

        //Intermissions between points and games => Do whatever we want (e.g. show texts)
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

    //public int GetPlayerScore(int playerID)
    //{
    //    return Games[CurrentGame].Score[playerID];
    //}

    //public Dictionary<int, int> GetGameScore()
    //{
    //    return Games[CurrentGame].Score;
    //}

    //public int GetPlayerWins(int playerID)
    //{
    //    int wins = 0;

    //    foreach (Game game in Games)
    //    {
    //        if (game.WinnerID == playerID)
    //        {
    //            wins++;
    //        }
    //    }
    //    return wins;
    //}

    //public int GetCurrentGame()
    //{
    //    return CurrentGame;
    //}

    //TODO: CHECK THIS FUNCTION => ITERATE OVER THE LIST OF PLAYERS IN THE ROOM
    //private void ChangeHitTurn()
    //{
    //    if (_turnID == PhotonNetwork.LocalPlayer.ActorNumber)
    //        _turnID = OpponentID;
    //    else
    //        _turnID = PhotonNetwork.LocalPlayer.ActorNumber;
    //}

    //private void ChangeServeTurn()
    //{
    //    if (ServeTurnID == PhotonNetwork.LocalPlayer.ActorNumber)
    //        ServeTurnID = OpponentID;
    //    else
    //        ServeTurnID = PhotonNetwork.LocalPlayer.ActorNumber;
    //}

    //public void AssignTurn(int playerID)
    //{
    //    _turnID = playerID;
    //}

    private void EndMatch()
    {
        //TODO: SHOW WHATEVER WE WANT AND RETURN TO MENU/PLAY AGAIN
        Debug.Log("Match has ended");
    }

    //Will try setting the opponent's score
    public override void SetScore()
    {
        //foreach (KeyValuePair<int, int> score in Games[CurrentGame].Score)
        //    Debug.Log("Player ID = " + score.Key + "Score = " + score.Value);

        //Lock the ball to avoid scoring more points until new serve
        _ballReference.IsLocked = true;

        bool gameEnded = false;

        //Games[CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber]++;
        //if (Games[CurrentGame].Score[OpponentID] >= MaxGamePoints && Games[CurrentGame].Score[OpponentID] - Games[CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber] >= PointsDiff)    //This game has ended
        //{
        //    Games[CurrentGame].WinnerID = OpponentID;
        //    gameEnded = true;
        //}
        /*else*/
        if (Games[CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber] >= MaxGamePoints && Games[CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber] - Games[CurrentGame].Score[OpponentID] >= PointsDiff)
        {
            Games[CurrentGame].WinnerID = PhotonNetwork.LocalPlayer.ActorNumber;
            _scoreBoard.UpdateLocalMatchScore();
            gameEnded = true;
        }


        if (gameEnded)
        {
            //UpdateLocalMatch => Scoreboard

            int localPlayer = GetPlayerWins(PhotonNetwork.LocalPlayer.ActorNumber);
            int opponent = GetPlayerWins(OpponentID);

            if (localPlayer >= Mathf.RoundToInt(MaxGames / 2))
            {
                _matchWinner = PhotonNetwork.LocalPlayer.ActorNumber;
                Debug.Log("MATCH ENDED. YOU WON");
            }
            else if (opponent >= Mathf.RoundToInt(MaxGames / 2))
            {
                _matchWinner = OpponentID;
                Debug.Log("MATCH ENDED. YOU LOST");
            }
            else
            {
                //Add new game
                Games.Add(new Game(++CurrentGame, PhotonNetwork.LocalPlayer.ActorNumber, OpponentID));
                //Debug.Log("MATCH ENDED. YOU LOST");

                _nextGame = true;
            }
        }
        else
        {
            _nextPoint = true;

            if ((Games[CurrentGame].Score[PhotonNetwork.LocalPlayer.ActorNumber] + Games[CurrentGame].Score[OpponentID]) % 2 == 0)
            {
                ChangeServeTurn();
            }
        }

        _isFirstHit = true;
        _currentHitSurface = _lastHitSurface = SurfaceType.None;
    }

    public AudioClip[] SFXClips;
    public void PlaySFXOnBall(int WhatToPlay)
    {
        _ballReference.GetComponent<AudioSource>().PlayOneShot(SFXClips[WhatToPlay]);
    }
    //************************************ LOGIC *********************************************
    public override void OnBallCollision(Surface surface)
    {
        if (!_ballReference.IsLocked && !_lookForOponent)
        {
            _currentHitSurface = surface.SurfaceType;

            switch (_currentHitSurface)
            {
                case SurfaceType.Floor:
                    PlaySFXOnBall(2);
                    //Debug.Log("The ball hit the floor");

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
                                //_scoreBoard.UpdateLocalPlayerScore();
                                _scoreBoard.UpdateRemotePlayerScore(ScoreInc);
                            }
                            break;

                        default:
                            Debug.Log("Surface case not controlled. Last hit was: " + _lastHitSurface);
                            break;
                    }
                    break;

                case SurfaceType.Field:
                    PlaySFXOnBall(1);
                    //Debug.Log("The ball hit the field with number " + surface.FieldNum);

                    //If the ball hits the field of the player that has the turn, the point goes for the other player
                    if (surface.FieldNum == _turnID && _turnID == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        if (!_isFirstHit)
                        {
                            //Add score and send it to all players in room
                            _scoreBoard.UpdateRemotePlayerScore(ScoreInc);
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
                        ChangeHitTurn();
                    }
                    break;

                case SurfaceType.Net:
                    PlaySFXOnBall(1);
                    //Debug.Log("The ball hit the net");
                    break;

                case SurfaceType.Paddle:
                    //Debug.Log("The ball hit a paddle");
                    PlaySFXOnBall(0);
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
                                    if (!PaddleOverField[OpponentID])
                                    {
                                        //Add score and send it to all players in room
                                        _scoreBoard.UpdateRemotePlayerScore(ScoreInc);
                                    }
                                }
                                //Local player hit the ball
                                //else if(surface.transform.parent.GetComponent<PlayerController>() && surface.transform.parent.GetComponent<PlayerController>().PlayerID == PhotonNetwork.LocalPlayer.ActorNumber)
                                //{
                                //    //Add score and send it to all players in room
                                //    _scoreBoard.UpdateRemotePlayerScore();
                                //}

                            }
                            else //The local doesn't have the turn
                            {
                                //The local player hit the ball
                                if (surface.transform.parent.GetComponent<PlayerController>() && surface.transform.parent.GetComponent<PlayerController>().PlayerID == PhotonNetwork.LocalPlayer.ActorNumber)
                                {
                                    //The local player didn't have the paddle over the field
                                    if (PaddleOverField[PhotonNetwork.LocalPlayer.ActorNumber])
                                    {
                                        //Add score and send it to all players in room
                                        _scoreBoard.UpdateRemotePlayerScore(ScoreInc);
                                    }
                                }
                                else
                                {
                                    //Add score and send it to all players in room
                                    _scoreBoard.UpdateRemotePlayerScore(ScoreInc);
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

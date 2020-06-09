using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TargetLogic : GameLogic
{
    [SerializeField]
    private TargetSystem _targetSystem;


    //Update
    private void Update()
    {
        //Check if there's another player and get its ID to set the OpponentID in GameLogic
        if (_lookForOponent /*&& PhotonNetwork.IsMasterClient*/)
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

                    //Spawn Targets
                    _targetSystem = GameObject.Find("TargetSystem").GetComponent<TargetSystem>();
                    _targetSystem.SpawnInitialTargets();
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

    public override void OnBallCollision(Surface surface)
    {
        if (!_ballReference.IsLocked && !_lookForOponent)
        {
            _currentHitSurface = surface.SurfaceType;

            switch (_currentHitSurface)
            {
                case SurfaceType.Floor:
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
                                _scoreBoard.UpdateLocalPlayerScore(-5);
                            }
                            break;

                        default:
                            Debug.Log("Surface case not controlled. Last hit was: " + _lastHitSurface);
                            break;
                    }
                    break;

                //case SurfaceType.Field:
                //    //Debug only
                //    Debug.Log("ASDFADFSASDF");
                //    break;

                case SurfaceType.Target:
                    //TODO
                    Destroy(surface.gameObject);
                    _scoreBoard.UpdateLocalPlayerScore(surface.GetComponent<Target>().ScoreInc);
                    Debug.Log("Ball hit a target");
                    break;

                default:
                    Debug.Log("Surface not recognized");
                    break;
            }

            _lastHitSurface = _currentHitSurface;
        }
    }
}

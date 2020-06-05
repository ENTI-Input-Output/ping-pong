using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TargetLogic : GameLogic
{
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

    public override void SetScore()
    {
        throw new System.NotImplementedException();
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
                                _scoreBoard.UpdateRemotePlayerScore();
                            }
                            break;

                        default:
                            Debug.Log("Surface case not controlled. Last hit was: " + _lastHitSurface);
                            break;
                    }
                    break;

                case SurfaceType.Field:
                    //Debug only
                    Debug.Log("ASDFADFSASDF");
                    break;

                case SurfaceType.Target:
                    //TODO
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

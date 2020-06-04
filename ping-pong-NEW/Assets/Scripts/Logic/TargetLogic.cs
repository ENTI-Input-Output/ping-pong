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
                    //Debug.Log("The ball hit the field with number " + surface.FieldNum);

                    //If the ball hits the field of the player that has the turn, the point goes for the other player
                    if (surface.FieldNum == _turnID && _turnID == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        if (!_isFirstHit)
                        {
                            //Add score and send it to all players in room
                            _scoreBoard.UpdateRemotePlayerScore();
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
                    //Debug.Log("The ball hit the net");
                    break;

                case SurfaceType.Paddle:
                    //Debug.Log("The ball hit a paddle");

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
                                        _scoreBoard.UpdateRemotePlayerScore();
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
                                        _scoreBoard.UpdateRemotePlayerScore();
                                    }
                                }
                                else
                                {
                                    //Add score and send it to all players in room
                                    _scoreBoard.UpdateRemotePlayerScore();
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

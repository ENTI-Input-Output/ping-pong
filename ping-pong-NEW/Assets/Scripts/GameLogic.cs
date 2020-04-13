using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
    private int _turnID;

    private int[] _scores;

    private void Start()
    {
        _turnID = 0;
        _scores = new int[] { 0, 0 };
    }

    public int GetPlayerScore(int playerID)
    {
        return _scores[playerID];
    }

    public int[] GetGlobalScore()
    {
        return _scores;
    }

    public void UpdateScore(int playerID)
    {
        _scores[playerID]++;
    }

    public void UpdateScore(int playerID, int amount)
    {
        _scores[playerID] += amount;
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
}

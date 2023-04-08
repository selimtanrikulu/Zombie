using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IRankManager
{
    int GetCurrentRank();
    void RankUp();
    
}
public class RankManager : IRankManager
{
    private int _rank = 1;
    
    public int GetCurrentRank()
    {
        return _rank;
    }

    public void RankUp()
    {
        _rank++;
    }
}

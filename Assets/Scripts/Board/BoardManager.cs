using System;
using BJW;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GemData[] _gemDatasToUseInGame = null; //TODO: Scriptable of gems game collection to use.
    
    private Board _board = null;

    private void Start()
    {
        _board = new Board(8, 8, _gemDatasToUseInGame);
    }
}

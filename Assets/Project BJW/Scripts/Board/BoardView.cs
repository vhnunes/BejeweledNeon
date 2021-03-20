using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    #region Components

    [SerializeField] private GameManager gm;    // Need here beacuse of editor setup.

    [SerializeField] private SpriteRenderer _tileRenderer;
    [SerializeField] private SpriteRenderer _bgRenderer;

    #endregion

    #region MonoBehaviour

    private void OnDrawGizmos()
    {
        UpdateScales();
    }

    private void Start()
    {
        UpdateScales();
    }

    #endregion

    #region Methods

    private void UpdateScales()
    {
        var tileSize = _tileRenderer.size;
        var bgSize = _bgRenderer.size;

        var targetTileSize = new Vector2(gm.board.rowSize, gm.board.collumSize);
        var targetBGSize = new Vector2(gm.board.rowSize + 0.5f, gm.board.collumSize + 0.5f);

        _tileRenderer.size = targetTileSize;
        _bgRenderer.size = targetBGSize;
    }

    #endregion
}

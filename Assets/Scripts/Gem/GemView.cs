using System;
using System.Collections;
using System.Collections.Generic;
using BJW;
using UnityEngine;

public class GemView : MonoBehaviour
{
    #region Components

    [SerializeField] private GemData _gemData = null;
    private SpriteRenderer _renderer = null;

    #endregion

    #region MonoBehaviour
    
    private void Start()
    {
        InitializeRenderer();
    }

    #endregion
    
    #region Methods

    private void InitializeRenderer()
    {
        _renderer = this.gameObject.AddComponent<SpriteRenderer>();
        _renderer.sprite = _gemData.gemSprite;
        _renderer.color = _gemData.gemColor;
    }
    public void UpdatePosition(Vector2 newPosition)
    {
        this.transform.position = newPosition;
    }

    public void SetGemData(GemData newGemData)
    {
        _gemData = newGemData;
    }

    #endregion
}

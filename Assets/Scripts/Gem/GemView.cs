using System;
using System.Collections;
using System.Collections.Generic;
using BJW;
using UnityEngine;

public class GemView : MonoBehaviour
{
    #region Components

    [SerializeField] private GemData _gemData = null;

    private Gem _gem = null;
    private SpriteRenderer _renderer = null;

    #endregion

    #region MonoBehaviour
    
    private void Start()
    {
        GoToSpawnPosition();
        InitializeRenderer();
        InitializeCollider();
    }

    private void OnMouseDown()
    {
        _gem.OnClick?.Invoke();
    }

    #endregion
    
    #region Methods
    
    private void GoToSpawnPosition()
    {
        this.transform.position = _gem.boardPosition + (Vector2.up * 10);
    }
    private void InitializeRenderer()
    {
        _renderer = this.gameObject.AddComponent<SpriteRenderer>();
        _renderer.sprite = _gemData.gemSprite;
        _renderer.color = _gemData.gemColor;
    }
    private void InitializeCollider()
    {
        this.gameObject.AddComponent<BoxCollider2D>();
    }
    private IEnumerator MoveToBoardPositionRoutine()
    {
        yield return new WaitForSeconds(0.1f); // Fix any initialization race condition
        
        var targetPosition = (Vector3) _gem.boardPosition;
        
        while (this.transform.position != targetPosition)
        {
            yield return new WaitForSeconds(0.01f);
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, 0.1f);
        }
    }

    public void DisableView()
    {
        _renderer.enabled = false;
    }
    public void MoveToBoardPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToBoardPositionRoutine());
    }
    public void SetGem(Gem newGem)
    {
        _gem = newGem;
    }
    public void SetGemData(GemData newGemData)
    {
        _gemData = newGemData;
    }

    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using BJW;
using UnityEngine;

/// <summary>
/// Responsible to visual show all informations from a desired gem. Like his sprite, animations, movement, and so on.
/// </summary>
public class GemView : MonoBehaviour
{
    #region Variables

    private float _moveSpeed = 20f;

    #endregion
    
    #region Components

    [SerializeField] private GemData _gemData = null;

    private Gem _gem = null;
    private SpriteRenderer _renderer = null;

    #region Properties
    
    private GameManager _gameManager => GameManager.instance;
    
    private Animator _animator => this.gameObject.GetComponent<Animator>();

    #endregion
    
    
    #endregion

    #region MonoBehaviour
    
    private void Start()
    {
        InitializeCollider();
        _gameManager.OnGameOver += () => SetMatchAnimation(true);
        _gameManager.OnGameRestart += () => SetMatchAnimation(false);
        _gameManager.OnGameRestart += GoToSpawnPosition;
        _gameManager.OnGameRestart += MoveToBoardPosition;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _gem.OnClick?.Invoke();
        }
        
        else if (Input.GetMouseButton(0))
            _gem.OnMouseOver?.Invoke();
    }

    #endregion
    
    #region Methods
    
    private void GoToSpawnPosition()
    {
        this.transform.position = _gem.boardPosition + (Vector2.up * 10);
    }
    private void InitializeRenderer()
    {
        _renderer = this.gameObject.GetComponent<SpriteRenderer>();
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
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, 
                _moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void SetMoveSpeed(float newSpeed)
    {
        _moveSpeed = newSpeed;
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
        GoToSpawnPosition();    // Switched from start to here to create an awesome effect
        InitializeRenderer();
    }

    #region Animations

    public void SetSelectedAnimation(bool state)
    {
        _animator.SetBool("isSelected", state);
    }

    public void SetMatchAnimation(bool state)
    {
        _animator.SetBool("isOnMatch", state);
    }

    #endregion

    #endregion
}

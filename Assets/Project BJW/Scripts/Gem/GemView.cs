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
    private ParticleSystem _particle = null;

    #region Properties
    
    private GameManager _gameManager => GameManager.instance;
    
    private Animator _animator => this.gameObject.GetComponent<Animator>();

    #endregion
    
    
    #endregion

    #region Routines

    private IEnumerator movingRoutine = null;

    #endregion

    #region MonoBehaviour
    
    private void Start()
    {
        InitializeCollider();
        InitializeParticle();
        
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
        this.transform.position = new Vector3(_gem.boardPosition.x, _gameManager.board.collumSize + _gem.boardPosition.y);
    }

    private void InitializeParticle()
    {
        _particle = this.transform.GetChild(0).GetComponent<ParticleSystem>();
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
    private void SetParticleColor()
    {
        ParticleSystem.MainModule settings = _particle.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(_gemData.gemColor);
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

    public void PlayFX()
    {
        SetParticleColor();
        StartCoroutine(particleRoutine());
        
        // Routine needded because the particle play has a execution delay from itself, and this cause issues 
        // when moving this gemview back to top of board
        
        IEnumerator particleRoutine()
        {
            _particle.transform.SetParent(null);
            _particle.Play();
            yield return _particle.isPlaying;
            _particle.transform.SetParent(this.transform);
            _particle.transform.localPosition = new Vector3();
        }
    }
    public void SetMoveSpeed(float newSpeed)
    {
        _moveSpeed = newSpeed;
    }
    public void MoveToBoardPosition()
    {
        if (movingRoutine != null)
        {
            StopCoroutine(movingRoutine);
            movingRoutine = null;
        }
            
        movingRoutine = MoveToBoardPositionRoutine();
        StartCoroutine(movingRoutine);
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

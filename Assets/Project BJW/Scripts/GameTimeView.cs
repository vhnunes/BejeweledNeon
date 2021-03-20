using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GameTimeView : MonoBehaviour
{
    #region Components

    private Text _text => this.gameObject.GetComponent<Text>();
    private GameManager _gameManager;
    
    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _gameManager = GameManager.instance;
        InvokeRepeating(nameof(UpdateText), 0, 1f);
    }

    #endregion

    #region Methods

    private void UpdateText()
    {
        _text.text = _gameManager.gameTime.ToString("0");
    }

    #endregion

}

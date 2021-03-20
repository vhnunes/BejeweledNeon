using System;
using UnityEngine;

namespace BJW
{
    /// <summary>
    /// All UI behaviours of the game with his related views.
    /// </summary>
    [Serializable]
    public class UI
    {
        
        #region Components

        [Header("UI Objects")]
        [SerializeField] private GameObject _gameTitle = null;
        [SerializeField] private GameObject _gameOver = null;

        private GameManager _gameManager = null;

        #endregion

        #region Methods

        public void OnStart()
        {
            _gameManager = GameManager.instance;
            _gameManager.OnGameOver += EnableGameOver;
            _gameManager.OnGameRestart += EnableGameTitle;
        }

        private void EnableGameOver()
        {
            _gameOver.SetActive(true);
            _gameTitle.SetActive(false);
        }
        private void EnableGameTitle()
        {
            _gameTitle.SetActive(true);
            _gameOver.SetActive(false);
        }

        #endregion
    }
}
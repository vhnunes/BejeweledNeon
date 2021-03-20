using System;
using UnityEngine;

namespace BJW
{
    [Serializable]
    public class Score
    {
        #region Variables

        [SerializeField] private int _currentScore = 0;
        private int _highScore = 0;
        private const int _scoreLimit = 999999;

        [Header("Score Reward")] 
        [SerializeField] private int _normalGemReward;
        [SerializeField] private int _rareGemReward;
        [SerializeField] private int _superRareGemReward;
        
        private const string SAVE_KEY = "High Score";

        #region Properties

        public float currentScore => _currentScore;
        public float highScore => _highScore;

        #endregion
        
        #endregion

        #region Components

        private ScoreView _scoreView = null;
        private ScoreView _highScoreView = null;

        private GameManager _gameManager = null;

        #endregion

        #region Methods

        public void OnStart()
        {
            _gameManager = GameManager.instance;
            _gameManager.OnGameRestart += ResetScore;
            
            LoadData();
        }

        private void ResetScore()
        {
            // NOT HIGH SCORE
            _currentScore = 0;
            UpdateScoreView();
            UpdateHighScoreView();
        }

        #region Score

        public void AddGemScore(Gem gem, int multiplier = 1)
        {
            // Add score based on gem type.
            if (gem.gemType == GemType.Normal)
            {
                AddScore(_normalGemReward * multiplier);
            }
            
            else if (gem.gemType == GemType.Rare)
            {
                AddScore(_rareGemReward * multiplier);
            }
            
            else if (gem.gemType == GemType.SuperRare)
            {
                AddScore(_superRareGemReward * multiplier);
            }

            if (_currentScore > _scoreLimit)
                _currentScore = _scoreLimit;
        }
        private void AddScore(int amount)
        {
            _currentScore += amount;
            TryToSetHighScore();
            UpdateScoreView();
        }
        private void TryToSetHighScore()
        {
            if (_currentScore > _highScore)
            {
                _highScore = _currentScore;
                UpdateHighScoreView();
                SaveData();
            }
               
        }

        #endregion

        #region Save / Load

        private void SaveData()
        {
            PlayerPrefs.SetInt(SAVE_KEY, _highScore);
        }

        public void LoadData()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                _highScore = PlayerPrefs.GetInt(SAVE_KEY);
            }
                
        }

        #endregion

        #region View

        private void UpdateScoreView()
        {
            _scoreView.UpdateView();
        }

        private void UpdateHighScoreView()
        {
            _highScoreView.UpdateView();
        }
        
        public void ReceiveHighScoreView(ScoreView scoreView)
        {
            _highScoreView = scoreView;
            scoreView.UpdateView();
        }
        public void ReceiveScoreView(ScoreView scoreView)
        {
            _scoreView = scoreView;
            scoreView.UpdateView();
        }

        #endregion

        #endregion
    }
}
using UnityEngine;

namespace BJW
{
    public class ScoreManager
    {
        #region Variables

        [SerializeField] private float _currentScore = 0;
        [SerializeField] private float _highScore = 0;

        private const string SAVE_KEY = "High Score";

        #region Properties

        public float currentScore => _currentScore;
        public float highScore => _highScore;

        #endregion
        
        #endregion

        #region Components

        private ScoreView _scoreView = null;
        private ScoreView _highScoreView = null;

        #endregion

        #region Methods
        
        public void AddGemScore(Gem gem, int multiplier)
        {
            // Add score based on gem type.
            if (gem.gemType == GemType.Normal)
            {
                AddScore(100);
            }
        }
        
        private void AddScore(float amount)
        {
            _currentScore += amount;
            UpdateScoreView();
            TryToSetHighScore();
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

        #region Save / Load

        private void SaveData()
        {
            PlayerPrefs.SetFloat(SAVE_KEY, _highScore);
        }

        private void LoadData()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
                _highScore = PlayerPrefs.GetFloat(SAVE_KEY);
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
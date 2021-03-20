using System;
using UnityEngine;

namespace BJW
{
    /// <summary>
    /// The base gem class, contains his loaded data and related behaviours.
    /// </summary>
    public class Gem
    {
        #region Variables
        
        private GemType _gemType;
        private Color _gemColor;
        private Vector2 _boardPosition = new Vector2();

        #region Properties

        public GemType gemType => _gemType;
        public Color gemColor => _gemColor;
        public Vector2 boardPosition => _boardPosition;

        #endregion
        
        #endregion

        #region Components

        [SerializeField] private GemData _gemData = null;
        private GemView _gemView = null;

        #endregion

        #region Events / Actions

        public Action OnClick;
        public Action OnMouseOver;

        #endregion

        #region Methods
        
        // Constructor
        public Gem(GemData gemData)
        {
            _gemData = gemData;
            _gemType = gemData.gemType;
            _gemColor = gemData.gemColor;
            
            InstanceView();
        }

        public void TransformIntoNewGem(GemData newGemData)
        {
            _gemData = newGemData;
            _gemType = newGemData.gemType;
            _gemColor = newGemData.gemColor;
            
            _gemView.SetGem(this);
            _gemView.SetGemData(_gemData);
        }
        public void SetBoardPosition(Vector2 newPosition)
        {
            _boardPosition = newPosition;
            _gemView.MoveToBoardPosition();
        }
        public void SetViewSpeed(float speed)
        {
            _gemView.SetMoveSpeed(speed);
        }
        public bool IsCompatibleWith(Gem other)
        {
            if (other.gemColor == _gemColor)
            {
                return true;
            }
            
            return false;
        }
        
        private void InstanceView()
        {
            var gemViewObjInstance =  MonoBehaviour.Instantiate(_gemData.gemPrefab);
            
            _gemView = gemViewObjInstance.AddComponent<GemView>();
            _gemView.SetGem(this);
            _gemView.SetGemData(_gemData);
        }
        
        #region On States

        public void OnMatchStart()
        {
            _gemView.SetMatchAnimation(true);
        }
        public void OnMatchEnd()
        { 
            _gemView.PlayFX();
            _gemView.SetMatchAnimation(false);
            GameManager.instance.score.AddGemScore(this);
        }
        public void OnSelected()
        {
            _gemView.SetSelectedAnimation(true);
        }
        public void OnUnselected()
        {
            _gemView.SetSelectedAnimation(false);
        }

        #endregion

        #endregion
    }
    
    public enum GemType
    {
        Normal, Rare, SuperRare
    }
}
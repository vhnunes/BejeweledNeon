using System;
using UnityEngine;

namespace BJW
{
    public class Gem
    {
        #region Variables
        
        private GemType _gemType;
        private GemState _gemState;
        private Color _gemColor;
        private Vector2 _boardPosition = new Vector2();

        #region Properties

        public GemType gemType => _gemType;
        public GemState gemState => _gemState;
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

        #endregion

        #region Methods
        
        // Constructor
        public Gem(GemData gemData)
        {
            _gemData = gemData;
            _gemType = gemData.gemType;
            _gemColor = gemData.gemColor;
            
            CreateView();
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
        
        public void OnMatchStart()
        {
            _gemView.SetMatchAnimation(true);
        }
        public void OnMatchEnd()
        {
            _gemView.SetMatchAnimation(false);
           _gemView.DisableView();
           _gemState = GemState.Dead;
           // TODO: Transform this gem into another gem
        }
        public void OnSelected()
        {
            _gemView.SetSelectedAnimation(true);
        }
        public void OnUnselected()
        {
            _gemView.SetSelectedAnimation(false);
        }

        public bool IsCompatibleWith(Gem other)
        {
            if (_gemState != GemState.Idle)
                return false;
            
            if (other.gemColor == _gemColor)
            {
                if (other.gemState == GemState.Idle)
                    return true;
            }
            
            return false;
        }
        
        private void CreateView()
        {
            var gemViewObjInstance =  MonoBehaviour.Instantiate(_gemData.gemPrefab);
            
            _gemView = gemViewObjInstance.AddComponent<GemView>();
            _gemView.SetGem(this);
            _gemView.SetGemData(_gemData);
        }

        #endregion
    }
}
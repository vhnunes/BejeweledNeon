using System.Collections.Generic;

namespace BJW
{
    public struct GemMatch
    {
        private List<Gem> _gems;

        public void AddGem(Gem gem)
        {
            if (_gems == null) _gems = new List<Gem>();
            
            if (!_gems.Contains(gem))
                _gems.Add(gem);
        }
        public bool IsMatch()
        {
            if (_gems == null)
                return false;
            
            else if (_gems.Count > 1)
                return true;
            
            return false;
        }
    }
}
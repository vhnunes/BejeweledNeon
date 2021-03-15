using System.Collections.Generic;

namespace BJW
{
    public struct GemMatch
    {
        private List<Gem> _gems;

        public List<Gem> gems => _gems;
        
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
            
            else if (_gems.Count > 2)
                return true;
            
            return false;
        }

        public Gem GetHighestGem()
        {
            // Get the most Y position gem in match.

            Gem highHem = null;
            
            foreach (var gem in _gems)
            {
                if (highHem == null)
                    highHem = gem;
                else
                {
                    if (gem.boardPosition.y > highHem.boardPosition.y)
                        highHem = gem;
                }
            }

            return highHem;
        }
    }
}
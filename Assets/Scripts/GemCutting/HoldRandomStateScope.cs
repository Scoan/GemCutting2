using System;
using Random = UnityEngine.Random;

namespace GemCutting
{
    // Scope that restores unity's random state
    public class HoldRandomStateScope : IDisposable
    {
        private readonly Random.State m_state;

        public HoldRandomStateScope(int seed)
        {
            m_state = Random.state;
            UnityEngine.Random.InitState(seed);
        }
        
        public void Dispose()
        {
            Random.state = m_state;
        }
    }
}
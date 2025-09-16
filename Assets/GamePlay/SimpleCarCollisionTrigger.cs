using System;

namespace GamePlay
{
    public class SimpleCarCollisionTrigger : CarCollisionDetector
    {
        public event Action passed;
        protected override void OnPassed()
        {
            passed?.Invoke();
        }
    }
}

using UnityEngine;

namespace Common
{
    public interface IIsInputInsideChecker
    {
        public bool IsScreenPositionInside(Vector2 screenPosition);
    }
}

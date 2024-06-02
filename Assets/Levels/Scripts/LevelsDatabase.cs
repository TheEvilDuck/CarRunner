using System;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "Levels database", menuName = "Levels/New levels database")]
    public class LevelsDatabase : ScriptableObject
    {
        [SerializeField] private List<LevelData> _levels;

        public Level GetLevel(string id)
        {
            foreach (LevelData levelData in _levels)
            {
                if (string.Equals(id, levelData.LevelId))
                {
                    return levelData.Level;
                }
            }

            throw new ArgumentException($"There is no level with id: {id}");
        }

        [Serializable]
        private class LevelData
        {
            [field: SerializeField] public string LevelId {get; private set;}
            [field: SerializeField] public Level Level {get; private set;}
        } 
    }

}
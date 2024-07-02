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

        public string GetNextLevelId(string levelId)
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                if(string.Equals(levelId, _levels[i].LevelId))
                {
                    if(i == _levels.Count - 1)
                        return _levels[i].LevelId;
                    else 
                        return _levels[i + 1].LevelId;
                }
            }
            throw new ArgumentException($"There is no level with id: {levelId}");
        }

        public string[] GetAllLevels()
        {
            List<string> result = new List<string>();

            foreach (LevelData levelData in _levels)
            {
                result.Add(levelData.LevelId);
            }

            return result.ToArray();
        }

        [Serializable]
        private class LevelData
        {
            [field: SerializeField] public string LevelId {get; private set;}
            [field: SerializeField] public Level Level {get; private set;}
        } 
    }

}
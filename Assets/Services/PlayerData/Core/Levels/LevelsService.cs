using System;
using System.Collections;
using System.Collections.Generic;
using Common.CoroutinePerformer;
using Common.Reactive;
using Levels.Scripts;
using Unity.VisualScripting;

namespace Services.PlayerData.Core.Levels
{
    public class LevelsService: DataService<ILevelsData>, ILevelsService
    {
        public override event Action DataChanged;
        
        private readonly LevelsDatabase _levelsDatabase;

        private readonly HashSet<string> _passedLevels;

        private Observable<string> _selectedLevel;

        public IReadonlyObservable<string> SelectedLevel => _selectedLevel;
        public IEnumerable<string> PassedLevels => _passedLevels;
        
        public LevelsService(
            IDataProvider<ILevelsData> dataProvider, 
            ICoroutinePerformer coroutinePerformer, 
            LevelsDatabase levelsDatabase) : base(dataProvider, coroutinePerformer)
        {
            _levelsDatabase = levelsDatabase;
            
            _passedLevels = new HashSet<string>();
        }
        
        protected override IEnumerator InitializeInternal(ILevelsData data)
        {
            _passedLevels.Clear();
            _passedLevels.AddRange(data.PassedLevels);
            _selectedLevel = new Observable<string>(data.SelectedLevel);
            yield break;
        }

        public bool IsLevelPassed(string levelID) => _passedLevels.Contains(levelID);

        public void AddPassedLevel(string levelID)
        {
            if (IsLevelPassed(levelID))
                return;

            _passedLevels.Add(levelID);
            DataChanged?.Invoke();
        }

        public void SetSelectedLevel(string levelID)
        {
            if (string.Equals(levelID, _selectedLevel.Value))
                return;
            
            _selectedLevel.Value = levelID;
            DataChanged?.Invoke();
        }

        protected override IEnumerator LoadDefaultData(DataLoadCallback<ILevelsData> callback)
        {
            LevelsData levelsData = new LevelsData(_levelsDatabase.TutorialLevelId, new string[0]);
            
            callback?.Invoke(true, levelsData);
            yield break;
        }

        protected override ILevelsData GetData()
            => new LevelsData(_selectedLevel.Value, _passedLevels);
        
        private class LevelsData: ILevelsData
        {
            public string SelectedLevel { get; }
            public IEnumerable<string> PassedLevels { get; }
            
            public LevelsData(string selectedLevel, IEnumerable<string> passedLevels)
            {
                SelectedLevel = selectedLevel;
                PassedLevels = passedLevels;
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Common.CoroutinePerformer;
using Services.PlayerData.Core;
using Services.PlayerData.SavingStrategy;
using UnityEngine;

namespace Services.PlayerData
{
    public class SaveLoadService: IDisposable
    {
        private readonly HashSet<SaveLoadData> _saveLoadDatas = new HashSet<SaveLoadData>();
        private readonly ICoroutinePerformer _coroutinePerformer;

        public SaveLoadService(ICoroutinePerformer coroutinePerformer)
        {
            _coroutinePerformer = coroutinePerformer;
        }

        public void Register(IDataService dataService, ISavingStrategy savingStrategy)
        {
            void OnDisposeAction() => savingStrategy.SaveRequested -= OnSaveRequested;

            void OnSaveRequested() => _coroutinePerformer.StartCoroutine(dataService.Save(OnDataSaved));

            void OnDataSaved(bool success)
            {
                if (!success)
                    Debug.LogError($"Can't save data for {dataService.GetType().Name}");
            }

            savingStrategy.SaveRequested += OnSaveRequested;
            SaveLoadData saveLoadData = new SaveLoadData(dataService, OnDisposeAction);
            _saveLoadDatas.Add(saveLoadData);
        }

        public IEnumerator LoadAll()
        {
            foreach (SaveLoadData saveLoadData in _saveLoadDatas)
            {
                void OnDataLoaded(bool success)
                {
                    if (!success)
                        Debug.LogError($"Failed to load data from {saveLoadData.DataService.GetType().Name}");
                }

                yield return saveLoadData.DataService.LoadData(OnDataLoaded);
            }
        }

        public void Dispose()
        {
            foreach (SaveLoadData saveLoadData in _saveLoadDatas)
                saveLoadData.OnDisposeAction?.Invoke();
            
            _saveLoadDatas.Clear();
        }
        
        private readonly struct SaveLoadData
        {
            public readonly IDataService DataService;
            public readonly Action OnDisposeAction;

            public SaveLoadData(
                IDataService dataService, 
                Action disposeAction)
            {
                DataService = dataService;
                OnDisposeAction = disposeAction;
            }
        }
    }
}
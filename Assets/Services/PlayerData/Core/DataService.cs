using System;
using System.Collections;
using Common.CoroutineCallbacks;
using Common.CoroutinePerformer;
using Services.PlayerData.Core.Wallet;
using UnityEngine;

namespace Services.PlayerData.Core
{
    public abstract class DataService<TData>: IDataService
        where TData : IData
    {
        public abstract event Action DataChanged;
        
        private readonly IDataProvider<TData> _dataProvider;
        private readonly ICoroutinePerformer _coroutinePerformer;

        private SuccessCallback _dataLoadedCallback;

        public DataService(
            IDataProvider<TData> dataProvider, 
            ICoroutinePerformer coroutinePerformer)
        {
            _dataProvider = dataProvider;
            _coroutinePerformer = coroutinePerformer;
        }

        public IEnumerator LoadData(SuccessCallback dataLoadedCallback)
        {
            _dataLoadedCallback = dataLoadedCallback;
            yield return _dataProvider.Load(OnDataLoaded);
        }

        public IEnumerator Save(SuccessCallback callback)
        {
            yield return _dataProvider.Save(GetData(), callback);
        }
        
        protected abstract IEnumerator InitializeInternal(TData data);
        protected abstract IEnumerator LoadDefaultData(DataLoadCallback<TData> callback);
        protected abstract TData GetData();

        private void OnDataLoaded(bool success, TData data)
        {
            if (!success)
            {
                _coroutinePerformer.StartCoroutine(LoadDefaultData(OnDefaultDataLoaded));
                return;
            }
            
            _dataLoadedCallback?.Invoke(true);
            _coroutinePerformer.StartCoroutine(InitializeInternal(data));
        }

        private void OnDefaultDataLoaded(bool success, TData data)
        {
            if (!success)
            {
                Debug.LogError($"Can't load defaut data for {typeof(TData).Name}");
                _dataLoadedCallback?.Invoke(false);
            }
            
            _dataLoadedCallback?.Invoke(true);
            _coroutinePerformer.StartCoroutine(InitializeInternal(data));
        }
    }
}
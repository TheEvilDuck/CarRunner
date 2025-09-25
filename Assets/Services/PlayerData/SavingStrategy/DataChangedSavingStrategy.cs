using System;
using System.Collections.Generic;
using Services.PlayerData.Core;
using UnityEngine;

namespace Services.PlayerData.SavingStrategy
{
    public class DataChangedSavingStrategy: ISavingStrategy, IDisposable
    {
        public event Action SaveRequested;
        
        private readonly IEnumerable<IDataService> _dataServices;

        public DataChangedSavingStrategy(params IDataService[] dataServices)
        {
            _dataServices = dataServices;

            foreach (IDataService dataService in _dataServices)
                dataService.DataChanged += OnDataChanged;
        }

        public void Dispose()
        {
            foreach (IDataService dataService in _dataServices)
                dataService.DataChanged -= OnDataChanged;
        }

        private void OnDataChanged() => SaveRequested?.Invoke();
    }
}
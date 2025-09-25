using System;
using System.Collections;
using Common.CoroutineCallbacks;

namespace Services.PlayerData.Core
{
    public interface IDataService
    {
        public event Action DataChanged;
        public IEnumerator LoadData(SuccessCallback dataLoadedCallback);
        public IEnumerator Save(SuccessCallback callback);
    }
}
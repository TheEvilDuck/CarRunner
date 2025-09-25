using System.Collections;
using Common.CoroutineCallbacks;
using Services.PlayerData.Core.Wallet;

namespace Services.PlayerData.Core
{
    public interface IDataProvider<TData>
        where TData : IData
    {
        public IEnumerator Load(DataLoadCallback<TData> callback);
        public IEnumerator Save(TData data, SuccessCallback callback);
    }
}
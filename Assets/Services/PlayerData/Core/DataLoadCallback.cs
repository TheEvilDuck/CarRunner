namespace Services.PlayerData.Core
{
    public delegate void DataLoadCallback<in TData> 
        (bool success, TData data) where TData: IData;
}
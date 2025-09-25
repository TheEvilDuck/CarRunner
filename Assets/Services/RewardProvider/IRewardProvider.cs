namespace Services.RewardProvider
{
    public interface IRewardProvider
    {
        public int GetLevelCompletionReward(float remainingTime, string levelID);
        public int GetTutorialCompletionReward();
    }
}
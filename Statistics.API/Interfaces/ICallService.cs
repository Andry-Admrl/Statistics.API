namespace Statistics.API.Interfaces
{
    public interface ICallService
    {
        void AddCall();
        int GetCountAndResetCalls();
    }
}

using Statistics.API.Interfaces;

namespace Statistics.API.Services
{
    public class CallService : ICallService
    {
        object locker = new();
        private int count = 0;

        public CallService()
        {
        }

        public int GetCountAndResetCalls()
        {
            lock (locker)
            {
                var i = count;
                count = 0;
                return i;
            }
        }

        public void AddCall()
        {
            lock (locker)
            { count++; }
        }
    }
}

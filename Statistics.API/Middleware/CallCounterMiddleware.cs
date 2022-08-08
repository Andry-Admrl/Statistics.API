using Statistics.API.Interfaces;

namespace Statistics.API.Middleware
{
    public class CallCounterMiddleware
    {
        private readonly RequestDelegate _next;
        public CallCounterMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task InvokeAsync(HttpContext context, ICallService _callService)
        {
            _callService.AddCall();
            await _next.Invoke(context);
        }
    }
}

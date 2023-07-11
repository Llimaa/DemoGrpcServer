using Grpc.Core;

namespace DemoGrpcServer.Services;

public class CounterService : Counter.CounterBase
{
    public override async Task<CountResponse> Count(CountRequest request, ServerCallContext context)
    {
        var response = new CountResponse();
        for (int count = request.StartWith; count < request.EndWith; count++)
        {
            var item = new CountValue() {
                Value = count
            };

            response.Value.Add(item);
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        return await Task.FromResult(response);
    }

    public override async Task CountStream(CountRequest request, IServerStreamWriter<CountResponseItem> responseStream, ServerCallContext context)
    {
        var index = request.StartWith;
        while(! context.CancellationToken.IsCancellationRequested && index <= request.EndWith) 
        {
            var response = new CountResponseItem() {
                Value = index
            };

            index++;
            await Task.Delay(TimeSpan.FromSeconds(1));
            await responseStream.WriteAsync(response);
        }
    }
}
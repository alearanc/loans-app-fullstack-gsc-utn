using API.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace API.Services
{
    public class LoanServices : LoanService.LoanServiceBase
    {
        public override Task<LoanResponse> GetAll(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new LoanResponse() { });
        }
    }
}

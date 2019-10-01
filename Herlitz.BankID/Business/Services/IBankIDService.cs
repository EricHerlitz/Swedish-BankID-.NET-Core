using System.Threading.Tasks;

namespace Herlitz.BankID
{
    public interface IBankIDService
    {
        Task<IAuthResponse> Auth(IAuthRequest authRequest);
        Task<IAuthResponse> Sign(ISignRequest signRequest);
        Task<ICollectResponse> Collect(ICollectRequest collectRequest);
        Task<bool> Cancel(ICancelRequest signRequest);

    }
}
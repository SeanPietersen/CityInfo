using CityInfo.Application.Authentication;
using System.Threading.Tasks;

namespace CityInfo.Application.Contract
{
    public interface IAuthenticationContract
    {
        string Authenticate(AuthenticationRequestBody authenticationRequestBody);
    }
}

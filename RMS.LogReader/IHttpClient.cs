using System.Threading.Tasks;

namespace RMS.AWS
{
    public interface IHttpClient
    {
        bool IsValid();
        Task<bool> PostData(string MessageBody, int httpTimeout);
    }
}

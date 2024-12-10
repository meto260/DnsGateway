using System.Threading.Tasks;
using ARSoft.Tools.Net.Dns;

namespace DnsGateway {
    public interface IDnsQueryStrategy {
        Task<DnsMessage> ResolveDnsQuery(DnsMessage message);
    }
}
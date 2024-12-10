using System.Net;
using System.Threading.Tasks;
using ARSoft.Tools.Net.Dns;

namespace DnsGateway {
    public class DnsQueryStrategy : IDnsQueryStrategy {
        private readonly DnsClient _dnsClient;

        public DnsQueryStrategy() {
            _dnsClient = new DnsClient(IPAddress.Parse("8.8.8.8"), 2000);
        }

        public async Task<DnsMessage> ResolveDnsQuery(DnsMessage message) {
            return await _dnsClient.ResolveAsync(message.Questions[0].Name, message.Questions[0].RecordType, message.Questions[0].RecordClass);
        }
    }
}
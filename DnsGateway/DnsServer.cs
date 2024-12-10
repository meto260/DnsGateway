using System.Net;
using ARSoft.Tools.Net.Dns;

namespace DnsGateway {
    public class DnsServer : ARSoft.Tools.Net.Dns.DnsServer {
        public DnsServer(IPAddress localAddress, int udpListenerCount, int tcpListenerCount)
            : base(localAddress, udpListenerCount, tcpListenerCount) {
        }
    }
}
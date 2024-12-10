using System;
using System.Net;
using System.Threading.Tasks;
using ARSoft.Tools.Net.Dns;
using DnsGateway;
using DnsGateway.Utils;
using DnsServer = DnsGateway.DnsServer;

var externalIp = await NetworkHelper.GetExternalIpAddressAsync();
if (externalIp == null) {
    Console.WriteLine("Dış IP adresi alınamadı. Uygulama sonlandırılıyor.");
    return;
}

Console.WriteLine($"Dış IP Adresi: {externalIp}");

var blacklist = FileHelper.LoadBlacklist("blacklist.txt");
var loggingStrategy = new LoggingStrategy();
var blacklistStrategy = new BlacklistStrategy(blacklist);
var dnsQueryStrategy = new DnsQueryStrategy();

var rateLimiter = new RateLimiter(maxRequests: 100000, windowSize: TimeSpan.FromHours(1));

//var dnsServer = new DnsServer(IPAddress.Parse(externalIp), 10, 10);
var dnsServer = new DnsServer(IPAddress.Any, 10, 10);

dnsServer.QueryReceived += async (sender, e) => {
    var message = e.Query as DnsMessage;

    if (message == null)
        return;

    var clientIp = e.RemoteEndpoint.Address;

    var domain = message.Questions[0].Name.ToString();

    if (blacklistStrategy.IsBlacklisted(domain)) {
        Console.WriteLine($"Blacklist'e Alınmış: {domain}");
        e.Response = blacklistStrategy.CreateBlacklistResponse(message);

        loggingStrategy.LogDnsRequest(message, clientIp, isBlocked: true, blockedReason: "Blacklist'e alındı");
        return;
    }

    e.Response = await dnsQueryStrategy.ResolveDnsQuery(message);

    if (!rateLimiter.IsAllowed(clientIp)) {
        Console.WriteLine($"Rate Limiting Aşıldı: {clientIp}");
        return;
    }
    loggingStrategy.LogDnsRequest(message, clientIp);
};

dnsServer.Start();

Console.WriteLine("DNS Gateway Server Çalışıyor. Çıkmak için Ctrl+C tuşuna basın.");

await Task.Delay(-1);
using System;
using System.Collections.Generic;
using System.Net;
using ARSoft.Tools.Net;
using ARSoft.Tools.Net.Dns;

namespace DnsGateway {
    public class BlacklistStrategy {
        private readonly List<string> _blacklist;

        public BlacklistStrategy(List<string> blacklist) {
            _blacklist = blacklist;
        }

        public bool IsBlacklisted(string domain) {
            foreach (var blacklistedDomain in _blacklist) {
                if (domain.Contains(blacklistedDomain, StringComparison.OrdinalIgnoreCase)) {
                    Console.WriteLine($"Engellendi: {domain} -> {blacklistedDomain}");
                    return true;
                }
            }
            return false;
        }

        public DnsMessage CreateBlacklistResponse(DnsMessage message) {
            var response = message.CreateResponseInstance();
            var googleAnswer = new ARecord(DomainName.Parse("google.com"), 3600, IPAddress.Parse("142.250.184.238"));
            response.AnswerRecords.Add(googleAnswer);
            return response;
        }
    }
}
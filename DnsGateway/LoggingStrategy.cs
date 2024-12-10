using System;
using System.IO;
using System.Net;
using ARSoft.Tools.Net.Dns;

namespace DnsGateway {
    public class LoggingStrategy {
        public void LogDnsRequest(DnsMessage message, IPAddress clientIp, bool isBlocked = false, string blockedReason = null) {
            var logFileName = $"{clientIp}_{DateTime.Now:yyyyMMdd}.csv";
            var logFilePath = Path.Combine("Logs", logFileName);

            if (!Directory.Exists("Logs")) {
                Directory.CreateDirectory("Logs");
            }

            if (!File.Exists(logFilePath)) {
                using (var fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (var sw = new StreamWriter(fs)) {
                    sw.WriteLine("Tarih;TransactionID;SorguTuru;AlanAdi;IstemciIP;Engellendi;EngelNedeni");
                }
            }

            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss};{message.TransactionID};{message.Questions[0].RecordType};{message.Questions[0].Name};{clientIp};{isBlocked};{blockedReason}";
            Console.WriteLine(logEntry);
            using (var fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (var sw = new StreamWriter(fs)) {
                sw.WriteLine(logEntry);
            }
        }
    }
}


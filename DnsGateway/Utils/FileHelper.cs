using System;
using System.Collections.Generic;
using System.IO;

namespace DnsGateway.Utils {
    public static class FileHelper {
        public static List<string> LoadBlacklist(string filePath) {
            if (!File.Exists(filePath)) {
                Console.WriteLine($"Blacklist dosyası bulunamadı: {filePath}");
                return new List<string>();
            }

            return File.ReadAllLines(filePath).ToList();
        }
    }
}
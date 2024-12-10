using System;
using System.Collections.Concurrent;
using System.Net;

public class RateLimiter {
    private readonly ConcurrentDictionary<IPAddress, RequestCounter> _counters = new();
    private readonly int _maxRequests;
    private readonly TimeSpan _windowSize;

    public RateLimiter(int maxRequests, TimeSpan windowSize) {
        _maxRequests = maxRequests;
        _windowSize = windowSize;
    }

    public bool IsAllowed(IPAddress clientIp) {
        // RequestCounter'ı al veya oluştur
        var counter = _counters.GetOrAdd(clientIp, _ => new RequestCounter(_windowSize));

        // Eşiği aşıp aşmadığını kontrol et
        if (!counter.IncrementAndCheck(_maxRequests)) {
            // Eşiği aşan istekleri temizle
            CleanupOldCounters();
            return false;
        }

        return true;
    }

    private void CleanupOldCounters() {
        // Eski zaman pencerelerini temizle
        var now = DateTime.UtcNow;
        foreach (var entry in _counters) {
            if (now - entry.Value.LastRequestTime > _windowSize) {
                _counters.TryRemove(entry.Key, out _);
            }
        }
    }

    private class RequestCounter {
        private int _requestCount;
        private DateTime _windowStart;
        private readonly TimeSpan _windowSize;

        public DateTime LastRequestTime { get; private set; }

        public RequestCounter(TimeSpan windowSize) {
            _windowSize = windowSize;
            _windowStart = DateTime.UtcNow;
        }

        public bool IncrementAndCheck(int maxRequests) {
            var now = DateTime.UtcNow;
            LastRequestTime = now;

            // Zaman penceresini yeniden başlat
            if (now - _windowStart > _windowSize) {
                _requestCount = 0;
                _windowStart = now;
            }

            _requestCount++;

            // Eşiği aşıp aşmadığını kontrol et
            if (_requestCount > maxRequests) {
                return false;
            }

            return true;
        }
    }
}
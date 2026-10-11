using Ryujinx.Common.Logging;
using System;
using System.Diagnostics;
using System.Threading;

namespace Ryujinx.UI.App.Common
{
    /// <summary>
    /// Tracks how long a game has actually been running, excluding paused time, and periodically persists it
    /// so that a crash only loses the last <see cref="FlushInterval"/> of play time.
    /// </summary>
    public sealed class PlaytimeTracker : IDisposable
    {
        public static readonly TimeSpan FlushInterval = TimeSpan.FromSeconds(60);

        private readonly Func<TimeSpan> _clock;
        private readonly Action<string, TimeSpan> _save;
        private readonly bool _autoFlush;
        private readonly object _lock = new();

        private string _titleId;
        private TimeSpan? _runningSince;
        private TimeSpan _pending;
        private Timer _timer;

        public PlaytimeTracker() : this(null, null, true)
        {
        }

        public PlaytimeTracker(Func<TimeSpan> clock, Action<string, TimeSpan> save, bool autoFlush)
        {
            long origin = Stopwatch.GetTimestamp();

            _clock = clock ?? (() => Stopwatch.GetElapsedTime(origin));
            _save = save ?? SaveToMetadata;
            _autoFlush = autoFlush;
        }

        private static void SaveToMetadata(string titleId, TimeSpan delta)
        {
            ApplicationLibrary.LoadAndSaveMetaData(titleId, appMetadata => appMetadata.AddPlayTime(delta));
        }

        /// <summary>
        /// Begins tracking a game and marks it as played now.
        /// </summary>
        public void Start(string titleId)
        {
            lock (_lock)
            {
                Stop();

                _titleId = titleId;
                _pending = TimeSpan.Zero;
                _runningSince = _clock();

                _save(titleId, TimeSpan.Zero);

                if (_autoFlush)
                {
                    _timer = new Timer(_ => Flush(), null, FlushInterval, FlushInterval);
                }
            }
        }

        public void Pause()
        {
            lock (_lock)
            {
                if (_titleId == null)
                {
                    return;
                }

                Collect();
                _runningSince = null;
            }
        }

        public void Resume()
        {
            lock (_lock)
            {
                if (_titleId != null && _runningSince == null)
                {
                    _runningSince = _clock();
                }
            }
        }

        /// <summary>
        /// Persists the play time accumulated since the last flush.
        /// </summary>
        public void Flush()
        {
            lock (_lock)
            {
                if (_titleId == null)
                {
                    return;
                }

                Collect();

                TimeSpan delta = _pending;

                if (delta <= TimeSpan.Zero)
                {
                    return;
                }

                _pending = TimeSpan.Zero;

                try
                {
                    _save(_titleId, delta);
                }
                catch (Exception exception)
                {
                    _pending += delta;

                    Logger.Warning?.Print(LogClass.Application, $"Failed to save play time for {_titleId}: {exception.Message}");
                }
            }
        }

        /// <summary>
        /// Saves the remaining play time and stops tracking. Does nothing if no game is being tracked.
        /// </summary>
        public void Stop()
        {
            lock (_lock)
            {
                if (_titleId == null)
                {
                    return;
                }

                Flush();

                _timer?.Dispose();
                _timer = null;
                _runningSince = null;
                _titleId = null;
            }
        }

        private void Collect()
        {
            if (_runningSince is { } since)
            {
                TimeSpan now = _clock();

                _pending += now - since;
                _runningSince = now;
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}

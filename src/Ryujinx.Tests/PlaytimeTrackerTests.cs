using NUnit.Framework;
using Ryujinx.UI.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ryujinx.Tests
{
    public class PlaytimeTrackerTests
    {
        private TimeSpan _now;
        private List<TimeSpan> _saved;
        private PlaytimeTracker _tracker;

        [SetUp]
        public void Setup()
        {
            _now = TimeSpan.Zero;
            _saved = new List<TimeSpan>();
            _tracker = new PlaytimeTracker(() => _now, (_, delta) => _saved.Add(delta), false);
        }

        private TimeSpan Total => TimeSpan.FromTicks(_saved.Sum(delta => delta.Ticks));

        [Test]
        public void Flush_AddsRunningTime()
        {
            _tracker.Start("0100000000000000");
            _now += TimeSpan.FromMinutes(5);
            _tracker.Flush();

            Assert.AreEqual(TimeSpan.FromMinutes(5), Total);
        }

        [Test]
        public void PausedTime_IsExcluded()
        {
            _tracker.Start("0100000000000000");
            _now += TimeSpan.FromMinutes(2);
            _tracker.Pause();
            _now += TimeSpan.FromMinutes(30);
            _tracker.Resume();
            _now += TimeSpan.FromMinutes(3);
            _tracker.Stop();

            Assert.AreEqual(TimeSpan.FromMinutes(5), Total);
        }

        [Test]
        public void Stop_AfterFlush_DoesNotDoubleCount()
        {
            _tracker.Start("0100000000000000");
            _now += TimeSpan.FromMinutes(1);
            _tracker.Flush();
            _now += TimeSpan.FromMinutes(1);
            _tracker.Stop();
            _tracker.Stop();

            Assert.AreEqual(TimeSpan.FromMinutes(2), Total);
        }

        [Test]
        public void Stop_WithoutStart_IsNoOp()
        {
            _tracker.Stop();
            _tracker.Flush();

            Assert.IsEmpty(_saved);
        }

        [Test]
        public void ResumeWithoutPause_DoesNotResetClock()
        {
            _tracker.Start("0100000000000000");
            _now += TimeSpan.FromMinutes(1);
            _tracker.Resume();
            _now += TimeSpan.FromMinutes(1);
            _tracker.Stop();

            Assert.AreEqual(TimeSpan.FromMinutes(2), Total);
        }

        [Test]
        public void FailedSave_IsRetriedOnNextFlush()
        {
            bool fail = false;
            _tracker = new PlaytimeTracker(() => _now, (_, delta) =>
            {
                if (fail)
                {
                    throw new System.IO.IOException();
                }

                _saved.Add(delta);
            }, false);

            _tracker.Start("0100000000000000");
            _now += TimeSpan.FromMinutes(1);
            fail = true;
            _tracker.Flush();
            fail = false;
            _now += TimeSpan.FromMinutes(1);
            _tracker.Stop();

            Assert.AreEqual(TimeSpan.FromMinutes(2), Total);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;

namespace SourceIndexingSharp.Build
{
    public class Logger : ILogger, IEventSource
    {
        #region Fields

        IEventSource _eventSource;

        #endregion

        #region Ctor

        public Logger()
        {
            Verbosity = LoggerVerbosity.Diagnostic;
        }

        #endregion

        #region ILogger

        public string Parameters { get; set; }

        public void Initialize(IEventSource eventSource)
        {
            _eventSource = eventSource;
            _eventSource.AnyEventRaised += OnAnyEventRaised;
            _eventSource.BuildFinished += OnBuildFinished;
            _eventSource.BuildStarted += OnBuildStarted; 
            _eventSource.CustomEventRaised += OnCustomEventRaised;
            _eventSource.ErrorRaised += OnErrorRaised;
            _eventSource.MessageRaised += OnMessageRaised;
            _eventSource.ProjectFinished += OnProjectFinished;
            _eventSource.ProjectStarted += OnProjectStarted;
            _eventSource.StatusEventRaised += OnStatusEventRaised;
            _eventSource.TargetFinished += OnTargetFinished;
            _eventSource.TargetStarted += OnTargetStarted;
            _eventSource.TaskFinished += OnTaskFinished;
            _eventSource.TaskStarted += OnTaskStarted;
            _eventSource.WarningRaised += OnWarningRaised;
        }

        public void Shutdown()
        {
            _eventSource.AnyEventRaised -= OnAnyEventRaised;
            _eventSource.BuildFinished -= OnBuildFinished;
            _eventSource.BuildStarted -= OnBuildStarted;
            _eventSource.CustomEventRaised -= OnCustomEventRaised;
            _eventSource.ErrorRaised -= OnErrorRaised;
            _eventSource.MessageRaised -= OnMessageRaised;
            _eventSource.ProjectFinished -= OnProjectFinished;
            _eventSource.ProjectStarted -= OnProjectStarted;
            _eventSource.StatusEventRaised -= OnStatusEventRaised;
            _eventSource.TargetFinished -= OnTargetFinished;
            _eventSource.TargetStarted -= OnTargetStarted;
            _eventSource.TaskFinished -= OnTaskFinished;
            _eventSource.TaskStarted -= OnTaskStarted;
            _eventSource.WarningRaised -= OnWarningRaised;
            _eventSource = null;
        }

        #endregion

        #region IEventSource

        public LoggerVerbosity Verbosity { get; set; }

        public event AnyEventHandler AnyEventRaised;

        public event BuildFinishedEventHandler BuildFinished;

        public event BuildStartedEventHandler BuildStarted;

        public event CustomBuildEventHandler CustomEventRaised;

        public event BuildErrorEventHandler ErrorRaised;

        public event BuildMessageEventHandler MessageRaised;

        public event ProjectFinishedEventHandler ProjectFinished;

        public event ProjectStartedEventHandler ProjectStarted;

        public event BuildStatusEventHandler StatusEventRaised;

        public event TargetFinishedEventHandler TargetFinished;

        public event TargetStartedEventHandler TargetStarted;

        public event TaskFinishedEventHandler TaskFinished;

        public event TaskStartedEventHandler TaskStarted;

        public event BuildWarningEventHandler WarningRaised;

        #endregion

        #region Methods

        private void OnAnyEventRaised(object sender, BuildEventArgs e)
        {
            var handler = AnyEventRaised;
            if (handler != null)
                handler(this, e);
        }

        private void OnBuildFinished(object sender, BuildFinishedEventArgs e)
        {
            var handler = BuildFinished;
            if (handler != null)
                handler(this, e);
        }

        private void OnBuildStarted(object sender, BuildStartedEventArgs e)
        {
            var handler = BuildStarted;
            if (handler != null)
                handler(this, e);
        }

        private void OnCustomEventRaised(object sender, CustomBuildEventArgs e)
        {
            var handler = CustomEventRaised;
            if (handler != null)
                handler(this, e);
        }

        private void OnErrorRaised(object sender, BuildErrorEventArgs e)
        {
            var handler = ErrorRaised;
            if (handler != null)
                handler(this, e);
        }

        private void OnMessageRaised(object sender, BuildMessageEventArgs e)
        {
            var handler = MessageRaised;
            if (handler != null)
                handler(this, e);
        }

        private void OnProjectFinished(object sender, ProjectFinishedEventArgs e)
        {
            var handler = ProjectFinished;
            if (handler != null)
                handler(this, e);
        }

        private void OnProjectStarted(object sender, ProjectStartedEventArgs e)
        {
            var handler = ProjectStarted;
            if (handler != null)
                handler(this, e);
        }

        private void OnStatusEventRaised(object sender, BuildStatusEventArgs e)
        {
            var handler = StatusEventRaised;
            if (handler != null)
                handler(this, e);
        }

        private void OnTargetFinished(object sender, TargetFinishedEventArgs e)
        {
            var handler = TargetFinished;
            if (handler != null)
                handler(this, e);
        }

        private void OnTargetStarted(object sender, TargetStartedEventArgs e)
        {
            var handler = TargetStarted;
            if (handler != null)
                handler(this, e);
        }

        private void OnTaskFinished(object sender, TaskFinishedEventArgs e)
        {
            var handler = TaskFinished;
            if (handler != null)
                handler(this, e);
        }

        private void OnTaskStarted(object sender, TaskStartedEventArgs e)
        {
            var handler = TaskStarted;
            if (handler != null)
                handler(this, e);
        }

        private void OnWarningRaised(object sender, BuildWarningEventArgs e)
        {
            var handler = WarningRaised;
            if (handler != null)
                handler(this, e);
        }

        #endregion
    }
}

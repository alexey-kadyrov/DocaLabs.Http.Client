﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Defines methods to run tasks.
    /// </summary>
    public static class TaskUtils
    {
        /// <summary>
        /// Runs the action synchronously.
        /// </summary>
        public static Task RunSynchronously(Action action, CancellationToken cancellationToken)
        {
            var task = new Task(action, cancellationToken);

            task.RunSynchronously();

            return task;
        }

        /// <summary>
        /// Returns a completed task which does nothing.
        /// </summary>
        public static Task CompletedTask(CancellationToken cancellationToken)
        {
            var task = new Task(() => { }, cancellationToken);

            task.RunSynchronously();

            return task;
        }
    }
}
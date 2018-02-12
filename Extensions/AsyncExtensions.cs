namespace Extensions {

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    public static class AsyncExtensions {

        /// <summary>
        /// Provides a task-based mechanism for copying data from one stream to another.
        /// </summary>
        /// <remarks>Native to .NET 4.5</remarks>
        //public static async Task CopyToAsync(this Stream source, Stream destination) {
        //    //var buffer = new byte[0x1000];
        //    //int bytesRead;
        //    //while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length)) > 0) {
        //    //    await destination.WriteAsync(buffer, 0, bytesRead);
        //    //}

        //    //optimized - uses overlapped read/ write latencies
        //    int i = 0;
        //    var buffers = new[] { new byte[0x1000], new byte[0x1000] };
        //    Task writeTask = null;
        //    while (true) {
        //        var readTask = source.ReadAsync(buffers[i], 0, buffers[i].Length);
        //        if (writeTask != null) {
        //            await Task.WhenAll(readTask, writeTask);
        //        }
        //        int bytesRead = await readTask;
        //        if (bytesRead == 0) break;
        //        writeTask = destination.WriteAsync(buffers[i], 0, bytesRead);
        //        i ^= 1; // swap buffers
        //    }
        //}

        /// Waits asynchronously for the process to exit.
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return immediately as canceled.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        [DebuggerStepThroughAttribute]
        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken)) {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default(CancellationToken)) {
                cancellationToken.Register(() => { tcs.TrySetCanceled(); });
            }

            return tcs.Task;
        }
    }
}

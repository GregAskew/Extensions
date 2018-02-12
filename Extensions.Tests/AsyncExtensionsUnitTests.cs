namespace Extensions.Tests {

    #region Usings
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    #endregion

    [TestClass]
    public class AsyncExtensionsUnitTests {

        [TestMethod]
        public void Test_WaitForExitAsync_RanToCompletion() {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var taskToRun = this.RunProcessAsync(token);
            if (!taskToRun.Wait(TimeSpan.FromMinutes(1))) {
                tokenSource.Cancel();
            }

            Assert.IsNotNull(taskToRun.Result);
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: taskToRun.Status);

        }

        [TestMethod]
        public void Test_WaitForExitAsync_Canceled() {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var taskToRun = this.RunProcessAsync(token);

            Thread.Sleep(TimeSpan.FromMilliseconds(1));
            tokenSource.Cancel();

            while (taskToRun.Status == TaskStatus.WaitingForActivation) {
                Debug.WriteLine($"taskToRun not yet running. Status: {taskToRun.Status}");
            }

            Assert.AreEqual(expected: true, actual: taskToRun.IsCanceled);

        }

        private async Task<List<string>> RunProcessAsync(CancellationToken token) {

            var stopwatch = Stopwatch.StartNew();
            var lines = new List<string>();

            var processStartInfo = new ProcessStartInfo();
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.FileName = "netstat.exe";
            processStartInfo.Arguments = "-a -n -o";
            processStartInfo.WorkingDirectory = Environment.ExpandEnvironmentVariables(@"%systemroot%\system32");

            var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();

            string standardOutput = await process.StandardOutput.ReadToEndAsync()
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(standardOutput)) {
                lines = standardOutput.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            }

            await process.WaitForExitAsync(token)
                .ConfigureAwait(false);

            Debug.WriteLine($"Time required: {stopwatch.Elapsed}");

            return lines;
        }
    }
}

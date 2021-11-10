using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetAppSettings.Test
{
    internal static class SimpleExecuteHelper
    {
        public static async Task<string[]> ExecuteAsync(string executable, string arguments)
        {
            var output = new List<string>();

            using var process = new Process();
            process.StartInfo.FileName = executable;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.EnableRaisingEvents = true;

            process.ErrorDataReceived += (o, e) => { if (e.Data is { } data) { output.Add(data); } };
            process.OutputDataReceived += (o, e) => { if (e.Data is { } data) { output.Add(data); } };

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            await process.WaitForExitAsync(cts.Token);

            await Task.Delay(TimeSpan.FromSeconds(1));      // wait 1s for output result firing on slow computer

            return output.ToArray();
        }
    }

#if NETCOREAPP3_1
    internal static class ProcessExtensions
    {
        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<bool>();

            using var registration = cancellationToken.Register(() => tcs.TrySetCanceled());

            _ = Task.Run(() =>
            {
                Exception captureException = null;
                try
                {
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    captureException = ex;
                }

                if (captureException != null)
                {
                    tcs.TrySetException(captureException);
                }
                else
                {
                    tcs.TrySetResult(process.HasExited);
                }
            });

            return tcs.Task;
        }
    }
#endif
}

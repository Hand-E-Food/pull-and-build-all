using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PullAndBuildAll
{
    /// <summary>
    /// Restores NuGet packages for a solution.
    /// </summary>
    /// <remarks>
    /// Based on https://daveaglick.com/posts/exploring-the-nuget-v3-libraries-part-1
    /// </remarks>
    public class NuGetService
    {
        public IList<string> Restore(string solutionPath)
        {
            return RestoreViaProcess(solutionPath);
        }

        private IList<string> RestoreViaProcess(string solutionPath)
        {
            const string ApplicationPath = @"nuget.exe";
            const int TimeOut = 300000; // 5 minutes in milliseconds

            var workingDirectory = Path.GetDirectoryName(solutionPath);

            var log = new List<string>();
            void process_DataReceived(object sender, DataReceivedEventArgs e)
            {
                lock (log)
                    log.Add(e.Data);
            }

            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = ApplicationPath,
                    Arguments = $"restore \"{solutionPath}\"",
                    WorkingDirectory = workingDirectory,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                },
            };
            process.OutputDataReceived += process_DataReceived;
            process.ErrorDataReceived += process_DataReceived;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            if (!process.WaitForExit(300000)) // 5 minutes
                throw new TimeoutException($"{Path.GetFileName(ApplicationPath)} failed to complete within {TimeOut}ms.");

            if (process.ExitCode != 0)
                throw new ApplicationException($"{ApplicationPath} exited with error code {process.ExitCode}.");

            return log;
        }
    }
}

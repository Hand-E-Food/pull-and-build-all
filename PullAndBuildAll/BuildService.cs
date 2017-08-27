using System.Collections.Generic;
using System;
using System.IO;
using System.Diagnostics;

namespace PullAndBuildAll
{
    public class BuildService
    {
        private readonly string _msBuildPath;
        private readonly TimeSpan _msBuildTimeOut;

        public BuildService(string msBuildPath, TimeSpan msBuildTimeOut)
        {
            _msBuildPath = msBuildPath;
            _msBuildTimeOut = msBuildTimeOut;
        }

        public IList<string> Build(string solutionPath, string platform)
        {
            return BuildViaProcess(solutionPath, platform);
        }

        private IList<string> BuildViaProcess(string solutionPath, string platform)
        {
            var workingDirectory = Path.GetDirectoryName(solutionPath);

            var log = new List<string>();
            void process_DataReceived(object sender, DataReceivedEventArgs e)
            {
                lock (log)
                    log.Add(e.Data);
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _msBuildPath,
                    Arguments = $"\"{solutionPath}\" /p:Platform=\"{platform}\" /t:Build /v:q",
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
            if (!process.WaitForExit((int)_msBuildTimeOut.TotalMilliseconds))
                throw new TimeoutException($"{Path.GetFileName(_msBuildPath)} failed to complete within {_msBuildTimeOut}.");

            if (process.ExitCode != 0)
                throw new OperationFailedException(log);

            return log;
        }
    }
}
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Text;

namespace MoneyApp.Helper
{
    public class PostmanRunnerRevenge{

        public bool TestsPass()
        {
            var process = new Process();

            var arg = @"newman run C:\Users\dave\Desktop\runnerTest.json";
            process.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            var input = process.StandardInput;
            input.WriteLine(arg);
            input.Close();

            var reader = process.StandardOutput;
            process.WaitForExit(1000);
            var result = reader.ReadToEnd();
            
            var reEncodedOutput = Encoding.UTF8.GetString(Encoding.Default.GetBytes(result));
            process.Close();
            process.Dispose();

            return !reEncodedOutput.Contains("AssertionError");
        }
    }

    public class PostmanRunner : IDisposable
    {
        private readonly Process postmanProcess;

        public PostmanRunner()
        {
            postmanProcess = new Process();
            postmanProcess.StartInfo.UseShellExecute = false;
            postmanProcess.StartInfo.FileName = @"cmd.exe";
            postmanProcess.StartInfo.RedirectStandardOutput = true;
            postmanProcess.StartInfo.RedirectStandardInput = true;
            postmanProcess.StartInfo.RedirectStandardError = true;
            postmanProcess.StartInfo.CreateNoWindow = true;
        }

        public bool Start(string jsonTestFile, string jsonEnvironmentVariable, string csvTestData, int timeoutMs)
        {
            postmanProcess.Start();

            var nodePath = @"C:\Program Files\nodejs\node.exe";
            var newmanPath = @"%AppData%\npm\node_modules\newman\bin\newman.js";

            var commandText = $@" newman run {jsonTestFile} --environment {jsonEnvironmentVariable} --iteration-data {csvTestData} --delay-request 1000 --timeout-request 20000";  // <-- This will execute the command and wait to close

            var input = postmanProcess.StandardInput;

            input.WriteLine(commandText);
            input.Close();

            var reader = postmanProcess.StandardOutput;
            var errorReader = postmanProcess.StandardError;

            postmanProcess.WaitForExit(timeoutMs);

            return ReturnResult(reader.ReadToEnd(), errorReader.ReadToEnd());
        }

        private static bool ReturnResult(string output, string errorOutput)
        {
            var reEncodedOutput = Encoding.UTF8.GetString(Encoding.Default.GetBytes(output));

            var noErrorsOccurred = true;
            if (!string.IsNullOrEmpty(errorOutput))
            {
                noErrorsOccurred = false;
                Debug.Write($"Error:{errorOutput}");
                TestContext.Write($"Error:{errorOutput}");
            }

            // Figure out if the tests passed...
            var zeroAssertionFailures = reEncodedOutput.Contains("¦        0 ¦\n+-----------------------------------------------¦\n¦ total run duration:");
            var failureSummaryNotPresent = !reEncodedOutput.Contains("#  failure");

            var overallPass = noErrorsOccurred && failureSummaryNotPresent && zeroAssertionFailures;

            Debug.Write(reEncodedOutput);
            TestContext.Write(reEncodedOutput);

            return overallPass;
        }

        public void Dispose()
        {
            if (postmanProcess != null)
            {
                postmanProcess.Close();
                postmanProcess.Dispose();
            }
        }
    }
}
using System;
using System.Diagnostics;

namespace SammakEnterprise.MagazineStore.Utilities
{
    /// <summary>
    /// Provides function execution time measuring
    /// </summary>
    public class TimeMeasure
    {
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private readonly string _functionName;
        private string _beginMessage;
        private string _endMessage;

        /// <summary>
        /// Returns the duration of the measured operation
        /// </summary>
        public TimeSpan Duration { get; set; }

        private TimeMeasure(string functionName)
        {
            _functionName = functionName;
        }


        public string ResultMessage
        {
            get
            {
                var functionNameMessage = "Function: " + _functionName + Environment.NewLine;

                var beginMessage = string.Empty;
                if (!string.IsNullOrEmpty(_beginMessage))
                {
                    beginMessage = $"\tBegin Message: " + _beginMessage + Environment.NewLine;
                }

                var duration = "\tDuration " + Duration.ToString("c") + Environment.NewLine;
                var endMessage = string.Empty;
                if (!string.IsNullOrEmpty(_endMessage))
                {
                    endMessage = "\tEnd Message: " + _endMessage + Environment.NewLine;
                }

                return $"Timing for " + functionNameMessage + beginMessage + duration + endMessage;
            }
        }

        /// <summary>
        /// This static method starts the timer for operation time measurement
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="beginMessage"></param>
        /// <returns></returns>
        public static TimeMeasure BeginTiming(string functionName, string beginMessage = null)
        {
            var timeMeasure = new TimeMeasure(functionName) {_beginMessage = beginMessage};
            timeMeasure._stopWatch.Start();
            return timeMeasure;
        }

        /// <summary>
        /// This static method stops the timer for operation time measurement; builds a result string and returns the formatted result.
        /// </summary>
        /// <param name="endMessage"></param>
        /// <returns></returns>
        public string EndTiming(string endMessage = null)
        {
            _stopWatch.Stop();
            Duration = _stopWatch.Elapsed;
            _endMessage = endMessage;
            return ResultMessage;
        }

        /// <summary>
        /// This static method stops the timer for operation time measurement; builds a result string and displays the result on the provided display method.
        /// </summary>
        /// <param name="displayMethod"></param>
        /// <param name="endMessage"></param>
        /// <returns></returns>
        public void EndTimingAndDisplay(Action<string> displayMethod, string endMessage = null)
        {
            var resultMessage = EndTiming(endMessage);
            displayMethod(resultMessage);
        }

        /// <summary>
        /// This static method stops the timer for operation time measurement; builds a result string and displays the result on the Console.
        /// </summary>
        /// <param name="endMessage"></param>
        /// <returns></returns>
        public void EndTimingAndConsoleDisplay(string endMessage = null)
        {
            EndTimingAndDisplay(Console.WriteLine, endMessage);
            Console.WriteLine("============================================================");
            Console.WriteLine();
        }

    }
}

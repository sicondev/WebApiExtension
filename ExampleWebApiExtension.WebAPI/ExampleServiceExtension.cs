using Sicon.Sage200.Architecture.BLL.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWebApiExtension.WebAPI
{
    /// <summary>
    /// Example Web Api Windows Service Extemsion
    /// </summary>
    public class ExampleServiceExtension : Sicon.API.Sage200.WebAPI.Common.IWebAPIServiceExtension
    {
        private const string PRODUCT_NAME = "Example Extension";
        private Task _processingTask = null;
        private System.Threading.CancellationTokenSource _cancellationTokenSource = null;
        private TimeSpan _interval = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Starts the Service Extension
        /// </summary>
        /// <remarks>Extension start up times will be monitored and logged by the web api</remarks>
        public void StartServiceExtension()
        {
            try
            {
                Sicon.Sage200.Architecture.DAL.Logging.DALLogger.Init();
                Sicon.Sage200.Architecture.DAL.Logging.DALLogger.SetWriteToFile(true, PRODUCT_NAME);
                Sicon.Sage200.Architecture.DAL.Logging.DALLogger.SetWriteToEventLog(true, PRODUCT_NAME);

                this.LogInfo("Starting Up...");

                _cancellationTokenSource = new System.Threading.CancellationTokenSource();

                _processingTask = Task.Run(() => LongRunningProcess(), _cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                this.LogError(e);
            }
            finally
            {
                this.LogInfo("Started Up.");
            }
        }

        /// <summary>
        /// Stops the Service Extension
        /// </summary>
        /// <remarks>Extension shut down times will be monitored and logged by the web api</remarks>
        public void StopServiceExtension()
        {
            try
            {
                this.LogInfo("Shutting Down...");

                _cancellationTokenSource.Cancel();

                Task.WaitAll(_processingTask);
            }
            catch (Exception e)
            {
                this.LogError(e);
            }
            finally
            {
                this.LogInfo("Shut Down.");
            }
        }

        /// <summary>
        /// Perform the long running process
        /// </summary>
        /// <remarks>This process will be running from when the service starts, for its lifetime, until the service is stopped</remarks>
        private async void LongRunningProcess()
        {
            try
            {
                do
                {
                    try
                    {
                        //Check for Cancellation within the processing loop
                        if (_cancellationTokenSource.IsCancellationRequested) return;

                        //Do Processing, You are already connected to Sage at this point. One Extension is loaded per Sage 200 Company.

                        this.LogInfo("Processing...");

                        //Delay for the specified interval 
                        await Task.Delay(_interval, _cancellationTokenSource.Token);
                    }
                    catch (Exception e)
                    {
                        this.LogError(e);
                    }
                }
                while (!_cancellationTokenSource.IsCancellationRequested);
            }
            catch (TaskCanceledException)
            {
                //Do nothing
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Logs an Info Message
        /// </summary>
        /// <param name="message">The Message to Log</param>
        private void LogInfo(string message) => Sicon.Sage200.Architecture.BLL.Logging.Logger.LogInfo(message, PRODUCT_NAME);

        /// <summary>
        /// Logs a Warning Message
        /// </summary>
        /// <param name="message">The Message</param>
        private void LogWarning(string message) => Sicon.Sage200.Architecture.BLL.Logging.Logger.LogWarning(message, PRODUCT_NAME);

        /// <summary>
        /// Logs an Error Message
        /// </summary>
        /// <param name="ex">The Exception</param>
        /// <param name="additionalMessage">Any Additional Message</param>
        private void LogError(Exception ex, string additionalMessage = "") => Sicon.Sage200.Architecture.BLL.Logging.Logger.LogError(ex, PRODUCT_NAME, additionalMessage);

    }
}

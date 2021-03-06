using System;
using DevExpress.ExpressApp;

namespace Xpand.Quartz.Server {
    /// <summary>
    /// Service interface for core Quartz.NET server.
    /// </summary>
    public interface IQuartzServer : IDisposable {
        /// <summary>
        /// Initializes the instance of <see cref="IQuartzServer"/>.
        /// Initialization will only be called once in server's lifetime.
        /// </summary>
        void Initialize(XafApplication application);

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();

    }
}
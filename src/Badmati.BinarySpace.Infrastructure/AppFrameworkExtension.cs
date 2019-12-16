using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Badmati.BinarySpace.Infrastructure
{
    public static class ExEnums
    {
        private static ConcurrentDictionary<System.Enum, DescriptionAttribute> senumCache = new ConcurrentDictionary<System.Enum, DescriptionAttribute>();

        public static string GetEnumDescription(this System.Enum senum)
        {
            DescriptionAttribute descriptionAttribute = null;
            senumCache.TryGetValue(senum, out descriptionAttribute);
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }
            var attris = senum.GetType().GetField(senum.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attris.Length > 0)
            {
                descriptionAttribute = ((DescriptionAttribute)attris[0]);
            }
            if (descriptionAttribute == null)
            {
                descriptionAttribute = new DescriptionAttribute();
            }
            senumCache.TryAdd(senum, descriptionAttribute);
            return descriptionAttribute.Description;
        }
    }
    public static class LoggerFactoryExtensions
    {
        public static ILoggerFactory RegisterLoggerFactory(this ILoggerFactory loggerFactory, string configPath = "Config/log4net.config")
        {
            return loggerFactory.AddLog4Net(configPath);
        }
    }

    /// <summary>
    /// The log4net extensions class.
    /// </summary>
    public static class Log4NetExtensions
    {
        /// <summary>
        /// Adds the log4net.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="log4NetConfigFile">The log4net Config File.</param>
        /// <returns>The <see cref="ILoggerFactory"/>.</returns>
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string log4NetConfigFile = "Config/log4net.config")
        {
            factory.AddProvider(new Log4NetProvider(log4NetConfigFile));
            return factory;
        }

    }

    /// <summary>
    /// The log4net provider class.
    /// </summary>
    public class Log4NetProvider : ILoggerProvider
    {
        /// <summary>
        /// The log4net config file.
        /// </summary>
        private readonly string log4NetConfigFile;

        /// <summary>
        /// The loggers collection.
        /// </summary>
        private readonly ConcurrentDictionary<string, Log4NetLogger> loggers = new ConcurrentDictionary<string, Log4NetLogger>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetProvider"/> class.
        /// </summary>
        /// <param name="log4NetConfigFile">The log4NetConfigFile.</param>
        public Log4NetProvider(string log4NetConfigFile)
        {
            this.log4NetConfigFile = log4NetConfigFile;
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <returns>The <see cref="ILogger"/> instance.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return this.loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        /// <summary>
        /// Disposes the provider.
        /// </summary>
        public void Dispose()
        {
            this.loggers.Clear();
        }

        /// <summary>
        /// Parses log4net config file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The <see cref="XmlElement"/> with the log4net XML element.</returns>
        private static XmlElement Parselog4NetConfigFile(string filename)
        {
            var log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(filename));
            if (log4netConfig.DocumentElement.LocalName.Equals("configuration", StringComparison.OrdinalIgnoreCase))
            {
                XmlElement element = log4netConfig["configuration"];
                return element["log4net"];
            }
            else
            {
                return log4netConfig["log4net"];
            }
        }

        /// <summary>
        /// Creates the logger implementation.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="Log4NetLogger"/> instance.</returns>
        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(name, Parselog4NetConfigFile(log4NetConfigFile));
        }
    }
    /// <summary>
    /// The log4net logger class.
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        /// <summary>
        /// The name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The XML element.
        /// </summary>
        private readonly XmlElement xmlElement;

        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILog log;

        /// <summary>
        /// The logger repository.
        /// </summary>
        private ILoggerRepository loggerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="xmlElement">The XML Element.</param>
        public Log4NetLogger(string name, XmlElement xmlElement)
        {
            this.name = name;
            this.xmlElement = xmlElement;
            this.loggerRepository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            this.log = LogManager.GetLogger(loggerRepository.Name, name);
            log4net.Config.XmlConfigurator.Configure(loggerRepository, xmlElement);
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        /// An IDisposable that ends the logical operation scope on dispose.
        /// </returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// Determines whether the logging level is enabled.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <returns>The <see cref="bool"/> value indicating whether the logging level is enabled.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return log.IsDebugEnabled;
                case LogLevel.Error:
                    return log.IsErrorEnabled;
                case LogLevel.Information:
                    return log.IsInfoEnabled;
                case LogLevel.Warning:
                    return log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        /// <summary>
        /// Logs an exception into the log.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="eventId">The event Id.</param>
        /// <param name="state">The state.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="formatter">The formatter.</param>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <exception cref="ArgumentNullException">Throws when the <paramref name="formatter"/> is null.</exception>
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = null;
            if (null != formatter)
            {
                message = formatter(state, exception);
            }

            if (!string.IsNullOrEmpty(message)
                || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        log.Fatal(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        log.Debug(message);
                        break;
                    case LogLevel.Error:
                        log.Error(message);
                        break;
                    case LogLevel.Information:
                        log.Info(message);
                        break;
                    case LogLevel.Warning:
                        log.Warn(message);
                        break;
                    default:
                        log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        log.Info(message, exception);
                        break;
                }
            }
        }
    }

    public static class FrameworkUtilsConfigure
    {

        public static FrameworkUtils Configure(IServiceProvider serviceProvider, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            FrameworkUtils.Instance.Configuration = configuration;
            FrameworkUtils.Instance.ServiceProvider = serviceProvider;
            FrameworkUtils.Instance.LoggerFactory = loggerFactory;
            return FrameworkUtils.Instance;
        }
    }

    public class FrameworkUtils
    {
        public static readonly FrameworkUtils Instance = new FrameworkUtils();

        private FrameworkUtils() { }

        public IServiceProvider ServiceProvider { get; internal set; }


        public ILoggerFactory LoggerFactory { get; internal set; }


        public IConfiguration Configuration { get; internal set; }
    }
}

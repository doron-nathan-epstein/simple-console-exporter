// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if EXPOSE_EXPERIMENTAL_FEATURES && NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace SimpleConsoleExporter;

public static class ConsoleExporterLoggingExtensions
{
  ///// <summary>
  ///// Adds Console exporter with OpenTelemetryLoggerOptions.
  ///// </summary>
  ///// <param name="loggerOptions"><see cref="OpenTelemetryLoggerOptions"/> options to use.</param>
  ///// <returns>The instance of <see cref="OpenTelemetryLoggerOptions"/> to chain the calls.</returns>
  ///// todo: [Obsolete("Call LoggerProviderBuilder.AddConsoleExporter instead this method will be removed in a future version.")]
  //public static OpenTelemetryLoggerOptions AddConsoleExporter(this OpenTelemetryLoggerOptions loggerOptions)
  //    => loggerOptions.AddConsoleExporter();

  /// <summary>
  /// Adds Console exporter with OpenTelemetryLoggerOptions.
  /// </summary>
  /// <param name="loggerOptions"><see cref="OpenTelemetryLoggerOptions"/> options to use.</param>
  /// <param name="configure">Callback action for configuring <see cref="ConsoleExporterOptions"/>.</param>
  /// <returns>The instance of <see cref="OpenTelemetryLoggerOptions"/> to chain the calls.</returns>
  /// todo: [Obsolete("Call LoggerProviderBuilder.AddConsoleExporter instead this method will be removed in a future version.")]
  public static OpenTelemetryLoggerOptions AddConsoleExporter(this OpenTelemetryLoggerOptions loggerOptions)
  {
    //Guard.ThrowIfNull(loggerOptions);

    //var options = new ConsoleExporterOptions();
    //configure?.Invoke(options);
    return loggerOptions.AddProcessor(new SimpleLogRecordExportProcessor(new ConsoleLogRecordExporter()));
  }

//#if EXPOSE_EXPERIMENTAL_FEATURES
//    /// <summary>
//    /// Adds Console exporter with LoggerProviderBuilder.
//    /// </summary>
//    /// <remarks><b>WARNING</b>: This is an experimental API which might change or be removed in the future. Use at your own risk.</remarks>
//    /// <param name="loggerProviderBuilder"><see cref="LoggerProviderBuilder"/>.</param>
//    /// <returns>The supplied instance of <see cref="LoggerProviderBuilder"/> to chain the calls.</returns>
//#if NET8_0_OR_GREATER
//    [Experimental(DiagnosticDefinitions.LoggerProviderExperimentalApi, UrlFormat = DiagnosticDefinitions.ExperimentalApiUrlFormat)]
//#endif
//    public
//#else
//  /// <summary>
//  /// Adds Console exporter with LoggerProviderBuilder.
//  /// </summary>
//  /// <param name="loggerProviderBuilder"><see cref="LoggerProviderBuilder"/>.</param>
//  /// <returns>The supplied instance of <see cref="LoggerProviderBuilder"/> to chain the calls.</returns>
//  internal
//#endif
//      static LoggerProviderBuilder AddConsoleExporter(
//      this LoggerProviderBuilder loggerProviderBuilder)
//      => loggerProviderBuilder.AddConsoleExporter(name: null, configure: null);

//#if EXPOSE_EXPERIMENTAL_FEATURES
//    /// <summary>
//    /// Adds Console exporter with LoggerProviderBuilder.
//    /// </summary>
//    /// <remarks><inheritdoc cref="AddConsoleExporter(LoggerProviderBuilder)" path="/remarks"/></remarks>
//    /// <param name="loggerProviderBuilder"><see cref="LoggerProviderBuilder"/>.</param>
//    /// <param name="configure">Callback action for configuring <see cref="ConsoleExporterOptions"/>.</param>
//    /// <returns>The supplied instance of <see cref="LoggerProviderBuilder"/> to chain the calls.</returns>
//#if NET8_0_OR_GREATER
//    [Experimental(DiagnosticDefinitions.LoggerProviderExperimentalApi, UrlFormat = DiagnosticDefinitions.ExperimentalApiUrlFormat)]
//#endif
//    public
//#else
//  /// <summary>
//  /// Adds Console exporter with LoggerProviderBuilder.
//  /// </summary>
//  /// <param name="loggerProviderBuilder"><see cref="LoggerProviderBuilder"/>.</param>
//  /// <param name="configure">Callback action for configuring <see cref="ConsoleExporterOptions"/>.</param>
//  /// <returns>The supplied instance of <see cref="LoggerProviderBuilder"/> to chain the calls.</returns>
//  internal
//#endif
//      static LoggerProviderBuilder AddConsoleExporter(
//      this LoggerProviderBuilder loggerProviderBuilder,
//      Action<ConsoleExporterOptions> configure)
//      => loggerProviderBuilder.AddConsoleExporter(name: null, configure);

//#if EXPOSE_EXPERIMENTAL_FEATURES
//    /// <summary>
//    /// Adds Console exporter with LoggerProviderBuilder.
//    /// </summary>
//    /// <remarks><inheritdoc cref="AddConsoleExporter(LoggerProviderBuilder)" path="/remarks"/></remarks>
//    /// <param name="loggerProviderBuilder"><see cref="LoggerProviderBuilder"/>.</param>
//    /// <param name="name">Name which is used when retrieving options.</param>
//    /// <param name="configure">Callback action for configuring <see cref="ConsoleExporterOptions"/>.</param>
//    /// <returns>The supplied instance of <see cref="LoggerProviderBuilder"/> to chain the calls.</returns>
//#if NET8_0_OR_GREATER
//    [Experimental(DiagnosticDefinitions.LoggerProviderExperimentalApi, UrlFormat = DiagnosticDefinitions.ExperimentalApiUrlFormat)]
//#endif
//    public
//#else
//  /// <summary>
//  /// Adds Console exporter with LoggerProviderBuilder.
//  /// </summary>
//  /// <param name="loggerProviderBuilder"><see cref="LoggerProviderBuilder"/>.</param>
//  /// <param name="name">Name which is used when retrieving options.</param>
//  /// <param name="configure">Callback action for configuring <see cref="ConsoleExporterOptions"/>.</param>
//  /// <returns>The supplied instance of <see cref="LoggerProviderBuilder"/> to chain the calls.</returns>
//  internal
//#endif
//      static LoggerProviderBuilder AddConsoleExporter(
//      this LoggerProviderBuilder loggerProviderBuilder,
//      string name,
//      Action<ConsoleExporterOptions> configure)
//  {
//    //Guard.ThrowIfNull(loggerProviderBuilder);

//    name ??= Options.DefaultName;

//    if (configure != null)
//    {
//      loggerProviderBuilder.ConfigureServices(services => services.Configure(name, configure));
//    }

//    return loggerProviderBuilder.AddProcessor(sp =>
//    {
//      var options = sp.GetRequiredService<IOptionsMonitor<ConsoleExporterOptions>>().Get(name);

//      return new SimpleLogRecordExportProcessor(new ConsoleLogRecordExporter(options));
//    });
//  }
}

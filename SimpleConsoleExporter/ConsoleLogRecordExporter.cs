// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using System.IO;
using System;
using System.Runtime.CompilerServices;

namespace SimpleConsoleExporter;

public class ConsoleLogRecordExporter : BaseExporter<LogRecord>
{
  private const string LoglevelPadding = ": ";
  private static readonly string _messagePadding = new string(' ', GetLogLevelString(LogLevel.Information).Length + LoglevelPadding.Length);
  private static readonly string _newLineWithMessagePadding = Environment.NewLine + _messagePadding;

  private readonly object syncObject = new();
  private bool disposed;
  private string disposedStackTrace;
  private bool isDisposeMessageSent;

  public ConsoleLogRecordExporter()
  {
    FormatterOptions = new SimpleConsoleFormatterOptions
    {
      SingleLine = true,
      UseUtcTimestamp = true,
      TimestampFormat = "dd-MM-yyyy HH:mm:ss "
    };
  }

  internal SimpleConsoleFormatterOptions FormatterOptions { get; set; }

  public override ExportResult Export(in Batch<LogRecord> batch)
  {
    if (disposed)
    {
      if (!isDisposeMessageSent)
      {
        lock (syncObject)
        {
          if (isDisposeMessageSent)
          {
            return ExportResult.Failure;
          }

          isDisposeMessageSent = true;
        }

        Console.WriteLine("The console exporter is still being invoked after it has been disposed. This could be due to the application's incorrect lifecycle management of the LoggerFactory/OpenTelemetry .NET SDK.");
        Console.WriteLine(Environment.StackTrace);
        Console.WriteLine(Environment.NewLine + "Dispose was called on the following stack trace:");
        Console.WriteLine(disposedStackTrace);
      }

      return ExportResult.Failure;
    }

    foreach (var logRecord in batch)
    {
      var logLevel = logRecord.LogLevel;
      var category = logRecord.CategoryName;
      var eventId = logRecord.EventId;
      var message = logRecord.FormattedMessage ?? logRecord.Attributes?.ToString() ?? logRecord.Body ?? string.Empty;
      var exception = logRecord.Exception;

      ConsoleColors logLevelColors = GetLogLevelConsoleColors(logLevel);
      string logLevelString = GetLogLevelString(logLevel);

      string? timestamp = null;
      string? timestampFormat = FormatterOptions.TimestampFormat;
      if (timestampFormat != null)
      {
        DateTimeOffset dateTimeOffset = GetCurrentDateTime();
        timestamp = dateTimeOffset.ToString(timestampFormat);
      }
      if (timestamp != null)
      {
        Console.Write(timestamp);
      }
      if (logLevelString != null)
      {
        WriteColoredMessage(logLevelString, logLevelColors.Background, logLevelColors.Foreground);
      }

      bool singleLine = FormatterOptions.SingleLine;

      Console.Write(LoglevelPadding);
      Console.Write(category);
      Console.Write('[');

      Console.Write(eventId.ToString());

      Console.Write(']');
      if (!singleLine)
      {
        Console.Write(Environment.NewLine);
      }

      WriteMessage(message, singleLine);

      if (exception != null)
      {
        WriteMessage(exception.ToString(), singleLine);
      }
      if (singleLine)
      {
        Console.Write(Environment.NewLine);
      }
    }

    return ExportResult.Success;
  }

  private static void WriteMessage(string message, bool singleLine)
  {
    if (!string.IsNullOrEmpty(message))
    {
      if (singleLine)
      {
        Console.Write(' ');
        WriteReplacing(Environment.NewLine, " ", message);
      }
      else
      {
        Console.Write(_messagePadding);
        WriteReplacing(Environment.NewLine, _newLineWithMessagePadding, message);
        Console.Write(Environment.NewLine);
      }
    }

    static void WriteReplacing(string oldValue, string newValue, string message)
    {
      string newMessage = message.Replace(oldValue, newValue);
      Console.Write(newMessage);
    }
  }

  private DateTimeOffset GetCurrentDateTime()
  {
    return FormatterOptions.UseUtcTimestamp ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
  }

  private static string GetLogLevelString(LogLevel logLevel)
  {
    return logLevel switch
    {
      LogLevel.Trace => "trce",
      LogLevel.Debug => "dbug",
      LogLevel.Information => "info",
      LogLevel.Warning => "warn",
      LogLevel.Error => "fail",
      LogLevel.Critical => "crit",
      _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
    };
  }

  private ConsoleColors GetLogLevelConsoleColors(LogLevel logLevel)
  {
    // We must explicitly set the background color if we are setting the foreground color,
    // since just setting one can look bad on the users console.
    return logLevel switch
    {
      LogLevel.Trace => new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black),
      LogLevel.Debug => new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black),
      LogLevel.Information => new ConsoleColors(ConsoleColor.DarkGreen, ConsoleColor.Black),
      LogLevel.Warning => new ConsoleColors(ConsoleColor.Yellow, ConsoleColor.Black),
      LogLevel.Error => new ConsoleColors(ConsoleColor.Black, ConsoleColor.DarkRed),
      LogLevel.Critical => new ConsoleColors(ConsoleColor.White, ConsoleColor.DarkRed),
      _ => new ConsoleColors(null, null)
    };
  }

  private readonly struct ConsoleColors(ConsoleColor? foreground, ConsoleColor? background)
  {
    public ConsoleColor? Foreground { get; } = foreground;

    public ConsoleColor? Background { get; } = background;
  }


  protected override void Dispose(bool disposing)
  {
    if (!disposed)
    {
      disposed = true;
      disposedStackTrace = Environment.StackTrace;
    }

    base.Dispose(disposing);
  }

  public static void WriteColoredMessage(string message, ConsoleColor? background, ConsoleColor? foreground)
  {
    // Order: backgroundcolor, foregroundcolor, Message, reset foregroundcolor, reset backgroundcolor
    if (background.HasValue)
    {
      Console.BackgroundColor = background.Value;
    }
    if (foreground.HasValue)
    {
      Console.ForegroundColor = foreground.Value;
    }

    Console.Write(message);

    if (foreground.HasValue)
    {
      Console.ForegroundColor = ConsoleColor.White;
    }
    if (background.HasValue)
    {
      Console.BackgroundColor = ConsoleColor.Black;
    }
  }
}
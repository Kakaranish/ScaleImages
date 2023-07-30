using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScaleImages.App;

public record ConsoleArguments(
    string SourceDirPath,
    decimal DownscaleRatio
);

public class ConsoleArgumentsExtractor
{
    private static readonly Regex ConsoleArgRegex = new("^--(\\w+)=\"?(.+)\"?$");
    
    private static readonly string SourceDirArgName = "SourceDirPath";
    private static readonly string DownscaleRatioArgName = "DownscaleRatio";
    
    public static ConsoleArguments Extract(string[] consoleArgs)
    {
        var argValuesByArgNames = consoleArgs
            .Select(ExtractArgument)
            .ToDictionary(a => a.ArgumentName, a => a.ArgumentValue);

        return new ConsoleArguments(
            argValuesByArgNames[SourceDirArgName],
            decimal.Parse(argValuesByArgNames[DownscaleRatioArgName])
        );
    }

    private static ExtractedConsoleArgument ExtractArgument(string consoleArg)
    {
        var match = ConsoleArgRegex.Match(consoleArg);

        if (match.Groups.Count < 2)
        {
            throw new InvalidOperationException($"Cannot parse console argument '{consoleArg}'");
        }

        return new ExtractedConsoleArgument(match.Groups[1].Value, match.Groups[2].Value);
    }

    private record ExtractedConsoleArgument(
        string ArgumentName,
        string ArgumentValue
    );
}
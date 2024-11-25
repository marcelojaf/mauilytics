using System;

namespace Mauilytics;

/// <summary>
/// Constants for Firebase Analytics and Crashlytics events and parameters
/// </summary>
public static class FirebaseConstants
{
    /// <summary>
    /// Analytics event names
    /// </summary>
    public static class Events
    {
        public const string CounterIncremented = "counter_incremented";
        public const string NavigatedToAbout = "navigated_to_about";
        public const string NavigatedFromAbout = "navigated_from_about";
        public const string ApiCallCompleted = "api_call_completed";
        public const string ApiCallError = "api_call_error";
    }

    /// <summary>
    /// Analytics event parameters
    /// </summary>
    public static class Parameters
    {
        public const string CountValue = "count_value";
        public const string DurationMs = "duration_ms";
        public const string ErrorMessage = "error_message";
    }

    /// <summary>
    /// Crashlytics custom keys
    /// </summary>
    public static class CrashlyticsKeys
    {
        public const string Environment = "environment";
    }
}

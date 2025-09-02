using System.Diagnostics.Metrics;

namespace SWSM.SCM.Interface.Enums
{
    public enum OperationResultStatus
    {
        Failure,
        Success
    }
    public class OperationResult
    {
        // Represents the status of an operation.
        public OperationResultStatus OperationStatus { get; set; }
        // Indicates whether the operation was successful or resulted in no action.
        public bool IsSuccess
        {
            get
            {
                return OperationStatus == OperationResultStatus.Success;
            }
        }

        public bool IsFailure
        {
            get
            {
                return OperationStatus == OperationResultStatus.Failure;
            }
        }

        // Optional message providing additional information about the operation result.
        public string? OperationResultMessage { get; private set; }



        // Private constructor to initialize the OperationResult with a status and optional message.
        private OperationResult(OperationResultStatus status, string? messageString = null, object? operationResultData = null)
        {
            OperationStatus = status;
            OperationResultMessage = messageString;
            ResultData = operationResultData ?? new object();
        }

        // Returns a successful OperationResult instance.
        public static OperationResult Success(string message) => new OperationResult(OperationResultStatus.Success, message);


        // Returns an OperationResult instance representing failure, with a message.
        public static OperationResult Failure(string message) => new OperationResult(OperationResultStatus.Failure, message);


        // Returns a string representation of the operation result, including the message if present.
        public override string ToString()
        {
            return string.IsNullOrEmpty(OperationResultMessage) ? $"{OperationStatus}" : $"{OperationStatus}: {OperationResultMessage}";
        }

        public object ResultData { get; set; }
        public T Result<T>()
        {
            if (ResultData is T tValue)
            {
                return tValue;
            }
            if (ResultData is null)
            {
                throw new InvalidCastException("ResultData is null and cannot be cast.");
            }
            try
            {
                // Special handling for string to bool/int conversions, etc.
                if (typeof(T) == typeof(bool))
                {
                    if (ResultData is string strValue && bool.TryParse(strValue, out var boolValue))
                        return (T)(object)boolValue;
                    if (ResultData is int intValue)
                        return (T)(object)(intValue != 0);
                }
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)(ResultData?.ToString() ?? string.Empty);
                }
                return (T)Convert.ChangeType(ResultData, typeof(T));
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Cannot cast ResultData to type {typeof(T).Name}.", ex);
            }
        }
    }
}

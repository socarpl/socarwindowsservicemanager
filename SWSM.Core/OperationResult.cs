using System.Diagnostics.Metrics;

namespace SWSM.Core
{
    public enum OperationResultStatus
    {
        NoAction,
        Success,
        Failure
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
                return this.OperationStatus == OperationResultStatus.Success || this.OperationStatus == OperationResultStatus.NoAction;
            }
        }
        // Optional message providing additional information about the operation result.
        public string? Message { get; private set; }

        // Private constructor to initialize the OperationResult with a status and optional message.
        private OperationResult(OperationResultStatus status, string? messageString = null)
        {
            Message = messageString;
        }

        // Returns a successful OperationResult instance.
        public static OperationResult Success => new OperationResult(OperationResultStatus.Success);


        // Returns an OperationResult instance representing no action, with a message.
        public static OperationResult NoAction(string message)
        {
            return new OperationResult(OperationResultStatus.NoAction, message);
        }
        // Returns an OperationResult instance representing failure, with a message.
        public static OperationResult Failure(string message)
        {
            return new OperationResult(OperationResultStatus.Failure, message);
        }

        // Returns a string representation of the operation result, including the message if present.
        public override string ToString()
        {
            return string.IsNullOrEmpty(Message) ? $"{OperationStatus}" : $"{OperationStatus}: {Message}";
        }
    }
}

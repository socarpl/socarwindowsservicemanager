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
                return OperationStatus == OperationResultStatus.Success ;
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
        public string? Message { get; private set; }

        // Private constructor to initialize the OperationResult with a status and optional message.
        private OperationResult(OperationResultStatus status, string? messageString = null)
        {
            Message = messageString;
        }

        // Returns a successful OperationResult instance.
        public static OperationResult Success(string message) => new OperationResult(OperationResultStatus.Success, message);


        // Returns an OperationResult instance representing failure, with a message.
        public static OperationResult Failure(string message) => new OperationResult(OperationResultStatus.Failure, message);
   

        // Returns a string representation of the operation result, including the message if present.
        public override string ToString()
        {
            return string.IsNullOrEmpty(Message) ? $"{OperationStatus}" : $"{OperationStatus}: {Message}";
        }
    }
}

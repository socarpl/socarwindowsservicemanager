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
        public OperationResultStatus OperationStatus { get; set; }
        public bool IsSuccess { get { return this.OperationStatus == OperationResultStatus.Success || this.OperationStatus == OperationResultStatus.NoAction; } }
        public string? Message { get; private set; }

        private OperationResult(OperationResultStatus status, string? messageString = null)
        {            
            Message = messageString;
        }

        public static OperationResult Success => new OperationResult(OperationResultStatus.Success);


        public static OperationResult NoAction(string message)
        {
            return new OperationResult(OperationResultStatus.NoAction, message);
        }
        public static OperationResult Failure(string message)
        {
            return new OperationResult(OperationResultStatus.Failure, message);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Message) ? $"{OperationStatus}" : $"{OperationStatus}: {Message}";
        }
    }
}

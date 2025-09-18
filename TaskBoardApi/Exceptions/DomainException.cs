namespace TaskBoardApi.Exceptions
{
    public abstract partial class DomainException : Exception
    {
        public string ErrorCode { get; }

        protected DomainException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}

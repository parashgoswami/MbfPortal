namespace Application.Common.Exceptions
{
    [Serializable]
    internal class NotFoundException : Exception
    {
        private string v;
        private int id;

        public NotFoundException()
        {
        }

        public NotFoundException(string? message) : base(message)
        {
        }

        public NotFoundException(string v, int id)
        {
            this.v = v;
            this.id = id;
        }

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
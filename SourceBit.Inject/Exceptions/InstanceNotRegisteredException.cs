using System;

namespace SourceBit.Inject.Exceptions
{
    [Serializable]
    public class InstanceNotRegisteredException : Exception
    {
        public InstanceNotRegisteredException(string message)
            : base(message)
        {
        }
    }
}

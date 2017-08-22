using System;
using System.Runtime.Serialization;

namespace Analizer.Exception
{
    [Serializable]
    public class EmptyRulesException : System.Exception
    {
        public EmptyRulesException()
        {
        }

        public EmptyRulesException(string message) : base(message)
        {
        }

        public EmptyRulesException(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected EmptyRulesException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

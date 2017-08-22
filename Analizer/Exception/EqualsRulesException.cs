using System;
using System.Runtime.Serialization;

namespace Analizer.Exception
{
    [Serializable]
    public class EqualsRulesException : System.Exception
    {
        public EqualsRulesException()
        {
        }

        public EqualsRulesException(string message) : base(message)
        {
        }

        public EqualsRulesException(string message, System.Exception inner) : base(message, inner)
        {
        }

        protected EqualsRulesException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

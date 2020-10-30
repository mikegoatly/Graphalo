using System;
using System.Collections.Generic;
using System.Text;

namespace Graphalo
{

    [Serializable]

    public class CyclicGraphsNotSupportedException : Exception
    {
        public CyclicGraphsNotSupportedException() { }
        public CyclicGraphsNotSupportedException(string message) : base(message) { }
        public CyclicGraphsNotSupportedException(string message, Exception inner) : base(message, inner) { }
        protected CyclicGraphsNotSupportedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

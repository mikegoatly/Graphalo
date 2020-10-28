using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Graphalo
{

    [Serializable]
    public class GraphaloException : Exception
    {
        public GraphaloException() { }
        public GraphaloException(string message) : base(message) { }
        public GraphaloException(string message, Exception inner) : base(message, inner) { }
        protected GraphaloException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}

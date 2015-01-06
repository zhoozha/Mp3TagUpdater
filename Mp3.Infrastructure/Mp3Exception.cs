using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Mp3.Infrastructure.Interfaces;

namespace Mp3.Infrastructure
{
    public class Mp3Exception : ApplicationException
    {
        /// <summary>
        /// Publishes message in the log
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="logger">IMp3Logger instance; if null nothing is published</param>
        public void Log(IMp3Logger logger)
        {
            if (null == logger)
                return;
            logger.Log(this.ToString(), Microsoft.Practices.Prism.Logging.Category.Exception, Microsoft.Practices.Prism.Logging.Priority.High);
        }
        // Summary:
        //     Initializes a new instance of the System.ApplicationException class.
        public Mp3Exception(IMp3Logger logger = null)
            : base()
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.ApplicationException class with
        //     a specified error message.
        //
        // Parameters:
        //   message:
        //     A message that describes the error.
        public Mp3Exception(string message, IMp3Logger logger = null)
            : base(message)
        {
            Log(logger);
        }
        //
        // Summary:
        //     Initializes a new instance of the System.ApplicationException class with
        //     serialized data.
        //
        // Parameters:
        //   info:
        //     The object that holds the serialized object data.
        //
        //   context:
        //     The contextual information about the source or destination.
        protected Mp3Exception(SerializationInfo info, StreamingContext context, IMp3Logger logger = null)
            : base(info, context)
        {
            Log(logger);
        }
        //
        // Summary:
        //     Initializes a new instance of the System.ApplicationException class with
        //     a specified error message and a reference to the inner exception that is
        //     the cause of this exception.
        //
        // Parameters:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception. If the innerException
        //     parameter is not a null reference, the current exception is raised in a catch
        //     block that handles the inner exception.
        public Mp3Exception(string message, Exception innerException, IMp3Logger logger = null)
            : base(message, innerException)
        {
            Log(logger);
        }

        public override string ToString()
        {
            return this.Message + '\n' + this.StackTrace.ToString() + this.InnerException != null ? '\n' + this.InnerException.ToString() : string.Empty;
        }
    }
}

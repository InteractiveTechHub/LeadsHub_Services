
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Model;

namespace AdaptiveKitCore.Responses
{
    /// <summary>
    /// Base response 
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// A collection of messages related to the response.
        /// </summary>
        public List<Message> Messages { get; set; } = [];

        public bool HasAnyErrorMessage 
        { 
            get 
            {
                return HasMessageType(MessageTypeEnum.Error) 
                    || HasMessageType(MessageTypeEnum.Exception) 
                    || HasMessageType(MessageTypeEnum.Fatal);
            } 
        }

        /// <summary>
        /// Validates if has any error message
        /// </summary>
        public bool HasErrorMessage { get { return HasMessageType(MessageTypeEnum.Error); } }

        /// <summary>
        /// Validates if has any exception message
        /// </summary>
        public bool HasExceptionMessage { get { return HasMessageType(MessageTypeEnum.Exception); } }

        /// <summary>
        /// Validate if has any fatal message
        /// </summary>
        public bool HasFatalMessage { get { return HasMessageType(MessageTypeEnum.Fatal); } } 

        /// <summary>
        /// Validates if has any info message
        /// </summary>
        public bool HasInfoMessage { get { return HasMessageType(MessageTypeEnum.Info); } }

        /// <summary>
        /// Validates if has any info message
        /// </summary>
        public bool HasSuccessMessage { get { return HasMessageType(MessageTypeEnum.Success); } }

        /// <summary>
        /// Validate if has any warning message
        /// </summary>
        public bool HasWarningMessage { get { return HasMessageType(MessageTypeEnum.Warning); } }

        /// <summary>
        /// Collection of error message
        /// </summary>
        /// <param name="text">Text of error message</param>
        /// /// <param name="code">Code of the message</param>
        /// <returns>Collection of error message</returns>
        public BaseResponse AddErrorMessage(string text, string code = "")
        {
            this.Messages.Add(new Message(MessageTypeEnum.Error, text, code));

            return this;
        }

        /// <summary>
        /// Collection of exception message
        /// </summary>
        /// <param name="text">Text of exception message</param>
        /// <param name="code">Code of exception message</param>
        /// <returns></returns>
        public BaseResponse AddExceptionMessage(string text, string code = "")
        {
            this.Messages.Add(new Message(MessageTypeEnum.Exception, text));

            return this;
        }

        /// <summary>
        /// Collection of informational message
        /// </summary>
        /// <param name="text">Text of the informational message</param>
        /// <param name="code">Code of the informational message</param>
        /// <returns>Collection of info message</returns>
        public BaseResponse AddInfoMessage(string text, string code = "")
        {
            this.Messages.Add(new Message(MessageTypeEnum.Info, text, code));

            return this;
        }

        /// <summary>
        /// Success of warning message
        /// </summary>
        /// <param name="text">Text of the Success Message</param>
        /// <param name="code">Code of the Success Message</param>
        /// <returns></returns>
        public BaseResponse AddSuccessMessage(string text, string code = "")
        {
            this.Messages.Add(new Message(MessageTypeEnum.Success, text, code));

            return this;
        }

        /// <summary>
        /// Collection of warning message
        /// </summary>
        /// <param name="text">Text of the Warning message</param>
        /// <param name="code">Code of the Warning message</param>
        /// <returns></returns>
        public BaseResponse AddWarningMessage(string text)
        {
            this.Messages.Add(new Message(MessageTypeEnum.Warning, text));

            return this;
        }

        private bool HasMessageType(MessageTypeEnum messageType)
        {
            return Messages.Any(item => item.MessageType == messageType);
        }
    }
}

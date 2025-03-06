
namespace AdaptiveKitCore.Enums
{
    /// <summary>
    /// Enumerator used to identify what type of message the <see cref="Message"/> instance represents.
    /// </summary>
    public enum MessageTypeEnum
    {
        /// <summary> None, default </summary>
        None,

        /// <summary> Success Message </summary>
        Success,

        /// <summary> Informational Message </summary>
        Info,

        /// <summary> Warning message </summary>
        Warning,

        /// <summary> Error message  </summary>
        Error,

        /// <summary> Fatal message, it is also considered a System error. </summary>
        Fatal,

        /// <summary> Exception message, it is also considered a System error. </summary>
        Exception,

        /// <summary> Validation message  </summary>
        Validation
    }
}

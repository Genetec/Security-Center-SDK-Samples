using System;

namespace RMSerialization
{
    #region Classes

    public class ChatMessage
    {
        #region Properties

        public Guid AppGuid { get; set; }

        public string Content { get; set; }

        public DateTime TimeStamp { get; set; }

        #endregion

        #region Public Methods

        public override string ToString() => $"{TimeStamp.ToLocalTime():T} : {Content}";
     
        #endregion
    }

    #endregion
}


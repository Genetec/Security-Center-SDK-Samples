using Genetec.Sdk;
using System;
using System.Windows.Media;

namespace CredentialFormatsSample.Models
{
    public class CredentialModel
    {
        public Guid Guid { get; set; }
        public string EntityName { get; set; }
        public Guid FormatId { get; set; }
        public int BitLength { get; set; }
        public string EncodedData { get; set; }
        public CredentialType CredentialType { get; set; }
        public string UniqueId { get; set; }
        public ImageSource Icon { get; set; }
    }
}

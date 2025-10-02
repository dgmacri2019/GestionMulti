using System.Runtime.Serialization;
using System.ServiceModel;

namespace GestionComercial.Contract.Responses
{
    [DataContract]
    public class ReportResponse
    {
        [DataMember] 
        public bool Success { get; set; }

        [DataMember] 
        public string Message { get; set; }

        [DataMember] 
        public object Object { get; set; }

        [DataMember] 
        public byte[] Bytes { get; set; }

        [DataMember] 
        public string FileName { get; set; }

        [DataMember] 
        public string Extension { get; set; }
    }
}

namespace GestionComercial.Contract.Responses
{
    public class ReportResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public object Object { get; set; }

        public byte[] Bytes { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }
    }
}

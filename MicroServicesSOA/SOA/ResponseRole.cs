using System.Diagnostics.CodeAnalysis;

namespace MicroServicesSOA.SOA
{
    public class ResponseRole
    {
        public bool Info { get; set; }
        public ResponseData? Data { get; set; }
        [AllowNull]
        public AppRole Role { get; set; }
        public static ResponseRole MakeResponse(bool valid, string code, string msg, AppRole rol = null!)
        {
            return new ResponseRole
            {
                Info = valid,
                Data = new ResponseData
                {
                    Code = code,
                    Message = msg,
                },
                Role = rol
            };
        }
    }
}
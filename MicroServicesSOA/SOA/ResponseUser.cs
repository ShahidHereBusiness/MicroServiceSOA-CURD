using System.Diagnostics.CodeAnalysis;

namespace MicroServicesSOA.SOA
{
    public class ResponseUser
    {
        public bool Info { get; set; }
        public ResponseData? Data { get; set; }
        [AllowNull]
        public AppUser User { get; set; }
        public static ResponseUser MakeResponse(bool valid, string code, string msg, AppUser usr = null!)
        {
            return new ResponseUser
            {
                Info = valid,
                Data = new ResponseData
                {
                    Code = code,
                    Message = msg,
                },
                User = usr
            };
        }
    }
}
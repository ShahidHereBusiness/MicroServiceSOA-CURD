using System.Diagnostics.CodeAnalysis;

namespace MicroServicesSOA.SOA
{
    public class ResponseUsers
    {
        public bool Info { get; set; }
        public ResponseData? Data { get; set; }
        [AllowNull]
        public List<AppUser> Users { get; set; }

        public static ResponseUsers MakeResponse(bool valid, string code, string msg, List<AppUser> usrs = null!)
        {
            return new ResponseUsers
            {
                Info = valid,
                Data = new ResponseData
                {
                    Code = code,
                    Message = msg,
                },
                Users = usrs ?? []
            };
        }
    }
}
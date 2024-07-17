using System.Diagnostics.CodeAnalysis;

namespace MicroServicesSOA.SOA
{
    public class ResponseUserRoles
    {
        public bool Info { get; set; }
        public ResponseData? Data { get; set; }
        [AllowNull]
        public AppUser User { get; set; }
        [AllowNull]
        public List<AppRole> Roles { get; set; }
        public static ResponseUserRoles MakeResponse(bool valid, string code, string msg, AppUser usr = null!, List<AppRole> rols = null!)
        {
            return new ResponseUserRoles
            {
                Info = valid,
                Data = new ResponseData
                {
                    Code = code,
                    Message = msg,
                },
                User = usr,
                Roles = rols ?? []
            };
        }
    }
}
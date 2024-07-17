using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicesSOA.SOA
{
    public class ResponseRoles
    {
        public bool Info { get; set; }
        public ResponseData? Data { get; set; }
        [AllowNull]
        public List<AppRole> Roles { get; set; }
        public static ResponseRoles MakeResponse(bool valid, string code, string msg, List<AppRole> rols = null!)
        {
            return new ResponseRoles
            {
                Info = valid,
                Data = new ResponseData
                {
                    Code = code,
                    Message = msg,
                },
                Roles = rols ?? []
            };
        }
    }
}
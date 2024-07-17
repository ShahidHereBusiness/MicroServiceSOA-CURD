namespace MicroServicesSOA.SOA
{
    public class Confirm
    {
        public bool Error { get; set; }
        public ResponseData? Data { get; set; }
        public static Confirm MakeConfirm(string code, string msg, bool valid)
        {
            return valid ? new Confirm
            {
                Error = !valid,
                Data = null
            } :
             new Confirm
             {
                 Error = !valid,
                 Data = new ResponseData
                 {
                     Code = code,
                     Message = msg,
                 }
             };
        }
    }
}
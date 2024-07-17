using System.Globalization;
using MicroServicesSOA.DbContext;
using MicroServicesSOA.SOA;
using Newtonsoft.Json;

namespace MicroServicesSOA.SOAV.Components
{
    public class InstanceService : IDisposable
    {
        /// <summary>
        /// Login & Authenticate User Credentials, combinations are:
        /// AccessSpecifier & WatchWord
        /// Email & WatchWord
        /// PhoneNumber & WatchWord
        /// SuperLevel set to 'true' for Administrator
        /// </summary>
        /// <param name="user">InstanceUser Object</param>
        /// <param name="_context">DB Context Object</param>
        /// <returns>ResponseUser Object</returns>MO
        public ResponseUser Indorse(InstanceUser user, AppDbContext _context)
        {
            #region Business Instances
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fidelity
            ResponseUser resp = new ResponseUser { Info = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" }, User = null };
            // Indorse By Id
            AppUser? listado = null;// _context.Users.Find(user.Surrogate);
            // Indorse By AccessSpecifier, Email, Phone and WatchWord
            if (listado == null && user.SuperLevel)// Super Level
                listado = _context.Users.FirstOrDefault(e => (e.UserName == user.AccessSpecifier || e.Email == user.MailingAddress || e.PhoneNumber == user.ISDN) && e.PasswordHash == SubRoutineDb.MD5Db(user.WatchWord, false) && e.ConcurrencyStamp != null);
            else if (listado == null && !user.SuperLevel)// App User
                listado = _context.Users.FirstOrDefault(e => (e.UserName == user.AccessSpecifier || e.Email == user.MailingAddress || e.PhoneNumber == user.ISDN) && e.PasswordHash == SubRoutineDb.MD5Db(user.WatchWord, false) && e.ConcurrencyStamp == null);
            // Not Found
            if (listado == null || user.SuperLevel)
                return ResponseUser.MakeResponse(false, "1", "Your request isn't registered with us!");
            if (user.Duration < 1)
                return ResponseUser.MakeResponse(false, "1", $"Your request is invalid Duration:{user.Duration}!");
            #endregion
            try
            {
                //Let's Find Log File
                if (SubRoutineDb.LocalizeDb(Diminish.connectionString, Diminish.AppName, Diminish.AppNameSuffix))
                {
                    #region Let's log record receipt                                
                    Diminish.AppCode = resp.Data.Code;
                    resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(resp)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion
                    // Handling the HTTP request pipeline.
                    listado.SecurityStamp = SubRoutineDb.MD5Db(user.WatchWord, true);
                    listado.LockoutEnd = DateTimeOffset.Now.AddMinutes(user.Duration);
                    int val = _context.SaveChanges();
                    resp.User = listado;
                    resp.Data.Message = "Authenticated";

                    #region Let's log record acknowledge                
                    resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(resp)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    if (val != 1)
                    {
                        resp.Data.Message = "Security Stamp Invalidated";
                        return ResponseUser.MakeResponse(false, resp.Data.Code, resp.Data.Message, null!);
                    }
                    else
                        return ResponseUser.MakeResponse(true, resp.Data.Code, resp.Data.Message, resp.User);
                }
                else
                    return ResponseUser.MakeResponse(false, resp.Data.Code, "Diminish Fall apart");
            }
            catch (Exception e)
            {
                #region Let's log record Utopia              
                resp.Data.Message = e.ToString();
                resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(resp)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return ResponseUser.MakeResponse(false, resp.Data.Code, "Opps somethings is broken!");
            }
        }
        /// <summary>
        /// Authorize & Account Summary
        /// </summary>
        /// <param name="user">InstanceUser object</param>
        /// <param name="_context">DB Context</param>
        /// <returns></returns>
        public ResponseUserRoles Substantiate(InstanceUser user, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fidelity
            ResponseUserRoles resp = new ResponseUserRoles { Info = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" }, User = null };
            // Find By Correlation
            AppUser? listado = _context.Users.FirstOrDefault(e => (e.SecurityStamp == user.Correlation && e.ConcurrencyStamp == null));
            // Not Found Or Expired Correlation
            if (listado == null || DateTime.Now > listado.LockoutEnd)
                return ResponseUserRoles.MakeResponse(false, "1", "Your request isn't registered with us!");
            try
            {
                //Let's Find Log File
                if (SubRoutineDb.LocalizeDb(Diminish.connectionString, Diminish.AppName, Diminish.AppNameSuffix))
                {
                    #region Let's log record receipt                                
                    Diminish.AppCode = resp.Data.Code;
                    resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(resp)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion
                    // Handling the HTTP request pipeline.
                    resp.User = listado;

                    #region List All Roles of User
                    var listadoall = (from users in _context.Users
                                      join userRoles in _context.UserRoles on users.Id equals userRoles.UserId
                                      join role in _context.Roles on userRoles.RoleId equals role.Id
                                      where users.Id == listado.Id
                                      select role)
                                      .ToList();
                    resp.Roles = listadoall;
                    #endregion

                    resp.Data.Message = "Read";

                    #region Let's log record acknowledge                
                    resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(resp)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    return ResponseUserRoles.MakeResponse(true, resp.Data.Code, resp.Data.Message, resp.User, resp.Roles);
                }
                else
                    return ResponseUserRoles.MakeResponse(false, resp.Data.Code, "Diminish Fall apart");
            }
            catch (Exception e)
            {
                #region Let's log record Utopia              
                resp.Data.Message = e.ToString();
                resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(resp)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return ResponseUserRoles.MakeResponse(false, resp.Data.Code, "Opps somethings is broken!");
            }
        }
        public void Dispose() { return; }
    }
}
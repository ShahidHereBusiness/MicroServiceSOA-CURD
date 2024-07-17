using Newtonsoft.Json;
using MicroServicesSOA.DbContext;
using MicroServicesSOA.SOA;
using static MicroServicesSOA.SOAV.Validate;

namespace MicroServicesSOA.SOAV.Components
{
    public class InstanceUserRole : IDisposable
    {
        public string? SurrogateUser { get; set; }
        public string? SurrogateRole { get; set; }
        public string? AccessSpecifier { get; set; }
        public string? Role { get; set; }
        public Confirm Tag(InstanceUserRole tag, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fedility
            Confirm? con = new Confirm { Error = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" } };
            // New Instance
            AppUserRole? listadoId;
            try
            {
                //Let's Find Log File
                if (SubRoutineDb.LocalizeDb(Diminish.connectionString, Diminish.AppName, Diminish.AppNameSuffix))
                {
                    string msg = string.Empty;
                    #region Validations
                    AppUser? listadoUser;
                    AppRole? listadoRole;
                    // Update By Names
                    if (!RoleNameError(tag.Role ?? "", ref msg) && !UserNameError(tag.AccessSpecifier ?? "", ref msg))
                    {
                        listadoUser = _context.Users.FirstOrDefault(e => string.Equals(e.UserName ?? "".ToLower(), tag.AccessSpecifier ?? "".Trim().ToLower()));
                        if (listadoUser == null)
                            return Confirm.MakeConfirm("1", "User requested isn't registered with us!", false);
                        listadoRole = _context.Roles.FirstOrDefault(e => string.Equals(e.Name ?? "".ToLower(), tag.Role ?? "".Trim().ToLower()));
                        if (listadoRole == null)
                            return Confirm.MakeConfirm("1", "Role requested isn't registered with us!", false);
                    }
                    // Update By Surrogate
                    else if (!RoleIdError(tag.SurrogateRole ?? "", ref msg) && !UserIdError(tag.SurrogateUser ?? "", ref msg))
                    {
                        listadoUser = _context.Users.FirstOrDefault(e => e.Id == tag.SurrogateUser);
                        if (listadoUser == null)
                            return Confirm.MakeConfirm("1", "User Surrogate isn't registered with us!", false);
                        listadoRole = _context.Roles.FirstOrDefault(e => e.Id == tag.SurrogateRole);
                        if (listadoRole == null)
                            return Confirm.MakeConfirm("1", "Role Surrogate isn't registered with us!", false);
                    }
                    else
                        return Confirm.MakeConfirm("1", msg, false);
                    // Already Exists
                    listadoId = _context.UserRoles.FirstOrDefault(e => e.UserId == listadoUser.Id && e.RoleId == listadoRole.Id);
                    if (listadoId != null)
                        return Confirm.MakeConfirm("1", $"Tagging already reserved with us.", false);
                    //New Instance
                    listadoId = new AppUserRole();
                    // Role Id
                    listadoId.RoleId = listadoRole.Id;
                    // User Id
                    listadoId.UserId = listadoUser.Id;
                    #endregion

                    #region Let's log record receipt                                
                    Diminish.AppCode = con.Data.Code;
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(tag)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    // Handling the HTTP request pipeline.
                    _context.UserRoles.Add(listadoId);
                    int val = _context.SaveChanges();
                    con.Data.Message = "Attach";

                    #region Let's log record acknowledge                
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(tag)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    if (val != 1)
                        return Confirm.MakeConfirm(con.Data.Code, con.Data.Message, false);
                    else
                        return Confirm.MakeConfirm(con.Data.Code, con.Data.Message, true);
                }
                else
                    return Confirm.MakeConfirm(con.Data.Code, "Diminish Fall apart", false);
            }
            catch (Exception e)
            {
                #region Let's log record Utopia              
                con.Data.Message = e.ToString();
                con.Data.Timespan = Compute.ComputeTimeSpan(start);
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(tag)}|{JsonConvert.SerializeObject(con)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return Confirm.MakeConfirm(con.Data.Code, "Opps somethings is broken!", false);
            }
        }
        public Confirm DeTag(InstanceUserRole tag, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fedility
            Confirm? con = new Confirm { Error = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" } };
            // New Instance
            AppUserRole? listadoId;
            try
            {
                //Let's Find Log File
                if (SubRoutineDb.LocalizeDb(Diminish.connectionString, Diminish.AppName, Diminish.AppNameSuffix))
                {
                    string msg = string.Empty;
                    #region Validations
                    //New Instance
                    listadoId = new AppUserRole();
                    AppUser? listadoUser;
                    AppRole? listadoRole;
                    // Validate Names
                    if (!RoleNameError(tag.Role ?? "", ref msg) && !UserNameError(tag.AccessSpecifier ?? "", ref msg))
                    {
                        listadoUser = _context.Users.FirstOrDefault(e => string.Equals(e.UserName ?? "".ToLower(), tag.AccessSpecifier ?? "".Trim().ToLower()));
                        if (listadoUser == null)
                            return Confirm.MakeConfirm("1", "User requested isn't registered with us!", false);
                        listadoRole = _context.Roles.FirstOrDefault(e => string.Equals(e.Name ?? "".ToLower(), tag.Role ?? "".Trim().ToLower()));
                        if (listadoRole == null)
                            return Confirm.MakeConfirm("1", "Role requested isn't registered with us!", false);
                    }
                    //Validate Surrogates
                    else if (!RoleIdError(tag.SurrogateRole ?? "", ref msg) && !UserIdError(tag.SurrogateUser ?? "", ref msg))
                    {
                        listadoUser = _context.Users.FirstOrDefault(e => e.Id == tag.SurrogateUser);
                        if (listadoUser == null)
                            return Confirm.MakeConfirm("1", "User Surrogate isn't registered with us!", false);
                        listadoRole = _context.Roles.FirstOrDefault(e => e.Id == tag.SurrogateRole);
                        if (listadoRole == null)
                            return Confirm.MakeConfirm("1", "Role Surrogate isn't registered with us!", false);
                    }
                    else
                        return Confirm.MakeConfirm("1", msg, false);
                    // Already Exists
                    listadoId = _context.UserRoles.FirstOrDefault(e => e.UserId == listadoUser.Id && e.RoleId == listadoRole.Id);
                    if (listadoId == null)
                        return Confirm.MakeConfirm("1", $"Tagging already revoked with us.", false);
                    // Role Id
                    listadoId.RoleId = listadoRole.Id;
                    // User Id
                    listadoId.UserId = listadoUser.Id;
                    #endregion

                    #region Let's log record receipt                                
                    Diminish.AppCode = con.Data.Code;
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(tag)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion
                    // Handling Detaggin
                    int val = -1;
                    // Handling the HTTP request pipeline.
                    _context.UserRoles.Remove(listadoId);
                    val = _context.SaveChanges();
                    con.Data.Message = "Detach";

                    #region Let's log record acknowledge                
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(tag)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    if (val != 1)
                        return Confirm.MakeConfirm(con.Data.Code, con.Data.Message, false);
                    else
                        return Confirm.MakeConfirm(con.Data.Code, con.Data.Message, true);
                }
                else
                    return Confirm.MakeConfirm(con.Data.Code, "Diminish Fall apart", false);
            }
            catch (Exception e)
            {
                #region Let's log record Utopia              
                con.Data.Message = e.ToString();
                con.Data.Timespan = Compute.ComputeTimeSpan(start);
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(tag)}|{JsonConvert.SerializeObject(con)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return Confirm.MakeConfirm(con.Data.Code, "Opps somethings is broken!", false);
            }
        }
        public void Dispose() { return; }
    }
}
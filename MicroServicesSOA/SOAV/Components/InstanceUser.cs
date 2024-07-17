using Newtonsoft.Json;
using MicroServicesSOA.DbContext;
using MicroServicesSOA.SOA;
using static MicroServicesSOA.SOAV.Validate;

namespace MicroServicesSOA.SOAV.Components
{
    public class InstanceUser : IDisposable
    {
        public string? Surrogate { get; set; }
        public string? AccessSpecifier { get; set; }
        public string? MailingAddress { get; set; }
        public string? WatchWord { get; set; }
        public string? ISDN { get; set; }
        public bool SuperLevel { get; set; }
        /// <summary>
        /// Security Pass Key or Encryption Key
        /// </summary>
        public string? Correlation { get; set; }
        /// <summary>
        /// Timeout scale
        /// </summary>
        public uint Duration { get; set; } 
        public string DemoUser()
        {
            InstanceUser? u = JsonConvert.DeserializeObject<InstanceUser>("{\"Surrogate\":\"13ca8af5-b699-4a7e-8bda-90926fb151e4\",\"AccessSpecifier\":\"ShahidMuhammad\",\"MailingAddress\":\"shahid.here.business@gmail.com\",\"WatchWord\":\"SamplingCore*8899#\",\"ISDN\":\"+92-3335930849\",\"SuperLevel\":false}");
            return JsonConvert.SerializeObject(u);
        }
        public Confirm CreateUser(InstanceUser user, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fedility
            Confirm? con = new Confirm { Error = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" } };
            try
            {
                //Let's Find Log File
                if (SubRoutineDb.LocalizeDb(Diminish.connectionString, Diminish.AppName, Diminish.AppNameSuffix))
                {
                    string msg = string.Empty;
                    #region Validations
                    if (UserNameError(user.AccessSpecifier ?? "", ref msg) ||
                        EmailError(user.MailingAddress ?? "", ref msg) ||
                        PasswordError(user.WatchWord ?? "", ref msg) ||
                        PhoneNumberError(user.ISDN ?? "", ref msg))
                        return Confirm.MakeConfirm("1", msg, false);
                    // Already Exists
                    AppUser? listado = _context.Users.FirstOrDefault(e => e.Email == user.MailingAddress);
                    if (listado != null)
                        return Confirm.MakeConfirm("1", $"{listado.NormalizedEmail} already reserved with us.", false);
                    listado = _context.Users.FirstOrDefault(e => e.PhoneNumber == user.ISDN);
                    if (listado != null && listado.PhoneNumberConfirmed)
                        return Confirm.MakeConfirm("1", $"{listado.PhoneNumber} already reserved with us.", false);
                    listado = _context.Users.FirstOrDefault(e => string.Equals(e.UserName ?? "".ToUpper(), user.AccessSpecifier ?? "".Trim().ToUpper()));
                    if (listado != null)
                        return Confirm.MakeConfirm("1", $"{listado.UserName} already reserved with us.", false);
                    // Object Instantiation
                    listado = new AppUser();
                    //Email Normalized
                    listado.NormalizedEmail = user.MailingAddress ?? "".Trim().ToUpper();
                    //UserName Normalized
                    listado.NormalizedUserName = user.AccessSpecifier?.Trim().ToUpper();
                    // Get All Params                    
                    listado.UserName = user.AccessSpecifier;
                    // Account Expiry
                    listado.LockoutEnd = DateTimeOffset.Now.AddDays(Diminish.AccountExpiry);
                    // Super Administrator Rights
                    if (user.SuperLevel && listado.ConcurrencyStamp==null)
                        listado.ConcurrencyStamp = Guid.NewGuid().ToString();
                    else if (!user.SuperLevel)
                    {
                        listado.ConcurrencyStamp = null;
                        listado.LockoutEnabled = true;
                    }
                    // MD5
                    listado.PasswordHash = SubRoutineDb.MD5Db(user.WatchWord, false);
                    listado.Email = user.MailingAddress;
                    listado.PhoneNumber = user.ISDN;
                    listado.SecurityStamp = SubRoutineDb.MD5Db(user.WatchWord, true);
                    #endregion                    

                    #region Let's log record receipt                                
                    Diminish.AppCode = con.Data.Code;
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    // Handling the HTTP request pipeline.
                    _context.Users.Add(listado);
                    int val = _context.SaveChanges();
                    con.Data.Message = "Create";

                    #region Let's log record acknowledge                
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(con)}";// Make String
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
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(con)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return Confirm.MakeConfirm(con.Data.Code, "Opps somethings is broken!", false);
            }
        }
        public Confirm UpdateUser(InstanceUser user, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fedility
            Confirm? con = new Confirm { Error = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" } };
            // Update By Id
            AppUser? listado = _context.Users.Find(user.Surrogate);
            // Update By AccessSpecifier and WatchWord
            if (listado == null)
                listado = _context.Users.FirstOrDefault(e => e.UserName == user.AccessSpecifier && e.PasswordHash == SubRoutineDb.MD5Db(user.WatchWord, false));
            if (listado == null)
                return Confirm.MakeConfirm("1", "Your request isn't registered with us!", false);
            try
            {
                AppUser? validado;
                //Let's Find Log File
                if (SubRoutineDb.LocalizeDb(Diminish.connectionString, Diminish.AppName, Diminish.AppNameSuffix))
                {
                    string msg = string.Empty;
                    #region Validations
                    if (!UserNameError(user.AccessSpecifier ?? "", ref msg))
                    {
                        validado = _context.Users.FirstOrDefault(e => string.Equals(e.UserName ?? "".ToLower(), user.AccessSpecifier ?? "".Trim().ToLower()) && e.Id != listado.Id);
                        if (validado != null)
                            return Confirm.MakeConfirm("1", $"Old:{listado.UserName}, New:{user.AccessSpecifier} already reserved with us.", false);
                        if (listado.UserName != user.AccessSpecifier)
                        {
                            listado.UserName = user.AccessSpecifier;
                            //UserName Normalized
                            listado.NormalizedUserName = user.AccessSpecifier?.Trim().ToUpper();
                            // Account Expiry on New User
                            listado.LockoutEnd = DateTimeOffset.Now.AddDays(Diminish.AccountExpiry);
                            listado.LockoutEnabled = false;
                        }
                    }
                    else if (!string.IsNullOrEmpty(user.AccessSpecifier))
                        return Confirm.MakeConfirm("1", msg, false);
                    if (!EmailError(user.MailingAddress ?? "", ref msg))
                    {
                        // Already Exists
                        validado = _context.Users.FirstOrDefault(e => string.Equals(e.Email ?? "".ToLower(), user.MailingAddress ?? "".Trim().ToLower()) && e.Id != listado.Id);
                        if (validado != null)
                            return Confirm.MakeConfirm("1", $"Old: {listado.NormalizedEmail}, New: {user.MailingAddress} already reserved with us.", false);

                        listado.Email = user.MailingAddress;
                        //Email Normalized
                        listado.NormalizedEmail = user.MailingAddress ?? "".Trim().ToUpper();
                    }
                    else if (!string.IsNullOrEmpty(user.MailingAddress))
                        return Confirm.MakeConfirm("1", msg, false);
                    if (!PasswordError(user.WatchWord ?? "", ref msg))
                    {
                        if (listado.PasswordHash != SubRoutineDb.MD5Db(user.WatchWord))
                        {
                            // MD5                        
                            listado.PasswordHash = SubRoutineDb.MD5Db(user.WatchWord);
                            // Account Expiry on New User
                            listado.LockoutEnd = DateTimeOffset.Now.AddDays(Diminish.AccountExpiry);
                            listado.LockoutEnabled = false;
                        }
                    }
                    else if (!string.IsNullOrEmpty(user.WatchWord))
                        return Confirm.MakeConfirm("1", msg, false);
                    if (!PhoneNumberError(user.ISDN ?? "", ref msg))
                    {
                        validado = _context.Users.FirstOrDefault(e => e.PhoneNumber == user.ISDN && e.Id != listado.Id);
                        if (validado != null && validado.PhoneNumberConfirmed)
                            return Confirm.MakeConfirm("1", $"Old:{listado.PhoneNumber}, New:{user.ISDN} already reserved with us.", false);

                        listado.PhoneNumber = user.ISDN;
                    }
                    else if (!string.IsNullOrEmpty(user.ISDN))
                        return Confirm.MakeConfirm("1", msg, false);
                    // Get All Params
                    // Super Administrator Rights
                    if (user.SuperLevel && listado.ConcurrencyStamp==null)
                        listado.ConcurrencyStamp = Guid.NewGuid().ToString();
                    else if (!user.SuperLevel)
                        listado.ConcurrencyStamp = null;

                    #endregion                    

                    #region Let's log record receipt                                
                    Diminish.AppCode = con.Data.Code;
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    // Handling the HTTP request pipeline.                                     
                    listado.SecurityStamp = SubRoutineDb.MD5Db(user.WatchWord, true);
                    int val = _context.SaveChanges();
                    con.Data.Message = "Update";

                    #region Let's log record acknowledge                
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(con)}";// Make String
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
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(con)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return Confirm.MakeConfirm(con.Data.Code, "Opps somethings is broken!", false);
            }
        }
        public ResponseUsers List(InstanceUser user, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fidelity
            ResponseUsers resp = new ResponseUsers { Info = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" }, Users = null! };
            // Update By Id
            AppUser? listado = _context.Users.Find(user.Surrogate);
            // Update By AccessSpecifier and WatchWord
            if (listado == null)
                listado = _context.Users.FirstOrDefault(e => e.UserName == user.AccessSpecifier && e.PasswordHash == SubRoutineDb.MD5Db(user.WatchWord, false));
            if (listado == null)
                return ResponseUsers.MakeResponse(false, "1", "Your request isn't registered with us!");
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
                    // Validate User with Full Control
                    var listadoall = (from users in _context.Users
                                      join userRoles in _context.UserRoles on users.Id equals userRoles.UserId
                                      join role in _context.Roles on userRoles.RoleId equals role.Id
                                      where users.Id == listado.Id && role.ConcurrencyStamp != null
                                      select new { UserSurrogate = users.Id, AccessSpecifier = users.UserName, RoleSurrogate = role.Id, RoleName = role.Name })
                                      .ToList();
                    if (listadoall.Count > 0)
                    {
                        // Handling the HTTP request pipeline.
                        resp.Data.Message = "ReadAll";
                        resp.Users = _context.Users.ToList();
                    }
                    else
                    {
                        resp.Data.Message = "Access denied to ReadAll.";
                    }

                    #region Let's log record acknowledge                
                    resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(resp)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    if (listadoall.Count > 0)
                        return ResponseUsers.MakeResponse(true, resp.Data.Code, resp.Data.Message, resp.Users);
                    else
                        return ResponseUsers.MakeResponse(false, resp.Data.Code, resp.Data.Message, resp.Users);
                }
                else
                    return ResponseUsers.MakeResponse(false, resp.Data.Code, "Diminish Fall apart");
            }
            catch (Exception e)
            {
                #region Let's log record Utopia              
                resp.Data.Message = e.ToString();
                resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(resp)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return ResponseUsers.MakeResponse(false, resp.Data.Code, "Opps somethings is broken!");
            }
        }
        public ResponseUserRoles Find(InstanceUser user, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fidelity
            ResponseUserRoles resp = new ResponseUserRoles { Info = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" }, User = null };
            // Update By Id
            AppUser? listado = _context.Users.Find(user.Surrogate);
            // Update By AccessSpecifier and WatchWord
            if (listado == null)
                listado = _context.Users.FirstOrDefault(e => (e.UserName == user.AccessSpecifier || e.Email==user.MailingAddress || e.PhoneNumber==user.ISDN) && e.PasswordHash == SubRoutineDb.MD5Db(user.WatchWord, false));
            if (listado == null)
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
                    var val = listadoall.Count;
                    if (val > 0)
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
        public Confirm Remove(InstanceUser user, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fedility
            Confirm? con = new Confirm { Error = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" } };
            // Update By Id
            AppUser? listado = _context.Users.Find(user.Surrogate);
            // Update By AccessSpecifier and WatchWord
            if (listado == null)
                listado = _context.Users.FirstOrDefault(e => e.UserName == user.AccessSpecifier && e.PasswordHash == SubRoutineDb.MD5Db(user.WatchWord, false));
            if (listado == null)
                return Confirm.MakeConfirm("1", "Your request isn't registered with us!", false);
            try
            {
                //Let's Find Log File
                if (SubRoutineDb.LocalizeDb(Diminish.connectionString, Diminish.AppName, Diminish.AppNameSuffix))
                {
                    #region Let's log record receipt                                
                    Diminish.AppCode = con.Data.Code;
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    var usr = _context.Users.Find(listado.Id);
                    int val = -1;
                    if (usr != null)
                    {
                        // Handling the HTTP request pipeline.
                        _context.Users.Remove(usr);
                        val = _context.SaveChanges();
                        con.Data.Message = "Delete";
                    }

                    #region Let's log record acknowledge                
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    if (val != 1)
                        return Confirm.MakeConfirm(con.Data.Code, con.Data.Message ?? "", false);
                    else
                        return Confirm.MakeConfirm(con.Data.Code, con.Data.Message ?? "", true);
                }
                else
                    return Confirm.MakeConfirm(con.Data.Code, "Diminish Fall apart", false);
            }
            catch (Exception e)
            {
                #region Let's log record Utopia              
                con.Data.Message = e.ToString();
                con.Data.Timespan = Compute.ComputeTimeSpan(start);
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(user)}|{JsonConvert.SerializeObject(con)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return Confirm.MakeConfirm(con.Data.Code, "Opps somethings is broken!", false);
            }
        }
        public void Dispose() { return; }
    }
}
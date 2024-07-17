using Newtonsoft.Json;
using MicroServicesSOA.DbContext;
using MicroServicesSOA.SOA;
using static MicroServicesSOA.SOAV.Validate;

namespace MicroServicesSOA.SOAV.Components
{
    public class InstanceRole : IDisposable
    {
        public string? Surrogate { get; set; }
        public string? AccessLevel { get; set; }
        public bool SuperLevel { get; set; }
        public static string DemoRole()
        {
            InstanceRole? r = JsonConvert.DeserializeObject<InstanceRole>("{\r\n  \"surrogate\": \"ea0854f9-86ee-46eb-9a93-f450fba7cfb0\",\r\n  \"accessLevel\": \"AUX\",\r\n  \"superLevel\": \"false\"\r\n}");
            return JsonConvert.SerializeObject(r);
        }
        public Confirm CreateRole(InstanceRole role, AppDbContext _context)
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
                    if (RoleNameError(role.AccessLevel ?? "", ref msg))
                        return Confirm.MakeConfirm("1", "Role Name format allowed -, Alphabet & Number!", false);
                    // Already Exists
                    AppRole? listado = _context.Roles.FirstOrDefault(e => string.Equals(e.Name ?? "".ToLower(), role.AccessLevel ?? "".Trim().ToLower()));
                    if (listado != null)
                        return Confirm.MakeConfirm("1", $"{listado.Name} already reserved with us.", false);
                    // Object Instantiation
                    listado = new AppRole();
                    // Role Normalized
                    listado.NormalizedName = role.AccessLevel?.Trim().ToUpper() ?? string.Empty;
                    // Get All Params                    
                    listado.Name = role.AccessLevel?.Trim() ?? string.Empty;
                    if (role.SuperLevel)
                        listado.ConcurrencyStamp = Guid.NewGuid().ToString();
                    #endregion                    

                    #region Let's log record receipt                                
                    Diminish.AppCode = con.Data.Code;
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    // Handling the HTTP request pipeline.
                    _context.Roles.Add(listado);
                    int val = _context.SaveChanges();
                    con.Data.Message = "Create";

                    #region Let's log record acknowledge                
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(con)}";// Make String
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
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(con)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return Confirm.MakeConfirm(con.Data.Code, "Opps somethings is broken!", false);
            }
        }
        public Confirm UpdateRole(InstanceRole role, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fedility
            Confirm? con = new Confirm { Error = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" } };
            // Update By Id
            AppRole? listado = _context.Roles.Find(role.Surrogate);
            // Update By Name already exists
            if (listado == null)
                listado = _context.Roles.FirstOrDefault(e => string.Equals(e.Name ?? "".ToLower(), role.AccessLevel ?? "".Trim().ToLower()));
            if (listado == null)
                return Confirm.MakeConfirm("1", "Your request isn't registered with us!", false);
            try
            {
                AppRole? validado;
                //Let's Find Log File
                if (SubRoutineDb.LocalizeDb(Diminish.connectionString, Diminish.AppName, Diminish.AppNameSuffix))
                {
                    string msg = string.Empty;
                    #region Validations
                    if (RoleNameError(role.AccessLevel ?? "", ref msg))
                        return Confirm.MakeConfirm("1", "Role Name format allowed -, Alphabet & Number!", false);
                    validado = _context.Roles.FirstOrDefault(e => string.Equals(e.Name ?? "".ToUpper(), role.AccessLevel ?? "".Trim().ToUpper()));
                    if (validado != null)
                        return Confirm.MakeConfirm("1", $"{listado.Name} already reserved with us.", false);
                    // Role Normalized
                    listado.NormalizedName = role.AccessLevel?.Trim().ToUpper() ?? string.Empty;
                    // Get All Params                    
                    listado.Name = role.AccessLevel?.Trim() ?? string.Empty;
                    if (role.SuperLevel)
                        listado.ConcurrencyStamp = Guid.NewGuid().ToString();
                    #endregion                    

                    #region Let's log record receipt                                
                    Diminish.AppCode = con.Data.Code;
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    // Handling the HTTP request pipeline.                                     
                    int val = _context.SaveChanges();
                    con.Data.Message = "Update";

                    #region Let's log record acknowledge                
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(con)}";// Make String
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
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(con)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return Confirm.MakeConfirm(con.Data.Code, "Opps somethings is broken!", false);
            }
        }
        public ResponseRoles List(InstanceRole rol, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fidelity
            ResponseRoles resp = new ResponseRoles { Info = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" }, Roles = null! };
            // Update By Id
            AppRole? listado = _context.Roles.Find(rol.Surrogate);
            // Update By Name
            if (listado == null)
                listado = _context.Roles.FirstOrDefault(e => e.Name == rol.AccessLevel);
            if (listado == null)
                return ResponseRoles.MakeResponse(false, "1", "Your request isn't registered with us!");
            try
            {
                //Let's Find Log File
                if (SubRoutineDb.LocalizeDb(Diminish.connectionString, Diminish.AppName, Diminish.AppNameSuffix))
                {
                    #region Let's log record receipt                                
                    Diminish.AppCode = resp.Data.Code;
                    resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(rol)}|{JsonConvert.SerializeObject(resp)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion
                    // Validate User with Full Control
                    var listadoall = (from role in _context.Roles
                                      where role.Id == listado.Id && role.ConcurrencyStamp != null
                                      select new { Surrogate = role.Id, role.Name, role.NormalizedName })
                                      .ToList();
                    if (listadoall.Count > 0)
                    {
                        // Handling the HTTP request pipeline.
                        resp.Data.Message = "ReadAll";
                        resp.Roles = _context.Roles.ToList();
                    }
                    else
                        resp.Data.Message = "Access denied to ReadAll.";

                    #region Let's log record acknowledge  
                    resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(rol)}|{JsonConvert.SerializeObject(resp)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    if (listadoall.Count > 0)
                        return ResponseRoles.MakeResponse(true, resp.Data.Code, resp.Data.Message, resp.Roles);
                    else
                        return ResponseRoles.MakeResponse(false, resp.Data.Code, resp.Data.Message, resp.Roles);
                }
                else
                    return ResponseRoles.MakeResponse(false, resp.Data.Code, "Diminish Fall apart");
            }
            catch (Exception e)
            {
                #region Let's log record Utopia              
                resp.Data.Message = e.ToString();
                resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(rol)}|{JsonConvert.SerializeObject(resp)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return ResponseRoles.MakeResponse(false, resp.Data.Code, "Opps somethings is broken!");
            }
        }
        public ResponseRole Find(InstanceRole role, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fedility
            ResponseRole resp = new ResponseRole { Info = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" }, Role = null };
            // Find By Id
            AppRole? listado = _context.Roles.Find(role.Surrogate);
            // Find By Name
            if (listado == null)
                listado = _context.Roles.FirstOrDefault(e => e.Name == role.AccessLevel);
            if (listado == null)
                return ResponseRole.MakeResponse(false,"1", "Your request isn't registered with us!");
            try
            {
                //Let's Find Log File
                if (SubRoutineDb.LocalizeDb(Diminish.connectionString, Diminish.AppName, Diminish.AppNameSuffix))
                {
                    #region Let's log record receipt                                
                    Diminish.AppCode = resp.Data.Code;
                    resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(resp)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion
                    // Handling the HTTP request pipeline.
                    resp.Role = listado;//Single Role
                    resp.Data.Message = "Read";

                    #region Let's log record acknowledge    
                    resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(resp)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    return ResponseRole.MakeResponse(true, resp.Data.Code, resp.Data.Message, resp.Role);
                }
                else
                    return ResponseRole.MakeResponse(false, resp.Data.Code, "Diminish Fall apart");
            }
            catch (Exception e)
            {
                #region Let's log record Utopia              
                resp.Data.Message = e.ToString();
                resp.Data.Timespan = Compute.ComputeTimeSpan(start);
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(resp)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return ResponseRole.MakeResponse(false, resp.Data.Code, "Opps somethings is broken!");
            }
        }
        public Confirm Remove(InstanceRole role, AppDbContext _context)
        {
            // Start Time
            DateTime start = DateTime.Now;
            // Make Diminish Fedility
            Confirm? con = new Confirm { Error = false, Data = new ResponseData { Code = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{new Random().Next()}" } };
            // Update By Id
            AppRole? listado = _context.Roles.Find(role.Surrogate);
            // Update By Name
            if (listado == null)
                listado = _context.Roles.FirstOrDefault(e => e.Name == role.AccessLevel);
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
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(con)}";// Make String
                    SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                    #endregion

                    var rol = _context.Roles.Find(listado.Id);
                    int val = -1;
                    if (rol != null)
                    {
                        // Handling the HTTP request pipeline.
                        _context.Roles.Remove(rol);
                        val = _context.SaveChanges();
                        con.Data.Message = "Delete";
                    }

                    #region Let's log record acknowledge                
                    con.Data.Timespan = Compute.ComputeTimeSpan(start);
                    Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(con)}";// Make String
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
                Diminish.AppLogs = $"{JsonConvert.SerializeObject(role)}|{JsonConvert.SerializeObject(con)}";// Make String
                SubRoutineDb.DiminishDb(Diminish.connectionString ?? "", Diminish.AppCode, $"{Diminish.AppLogs}", Diminish.AppName, Diminish.AppNameSuffix ?? "");
                #endregion
                return Confirm.MakeConfirm(con.Data.Code, "Opps somethings is broken!", false);
            }
        }
        public void Dispose() { return; }
    }
}
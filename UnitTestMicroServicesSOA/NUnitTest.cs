using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MicroServicesSOA.DbContext;
using MicroServicesSOA.SOA;
using MicroServicesSOA.SOAV.Components;

namespace UnitTestMicroServicesSOA
{
    public class NUnitTest
    {
        IConfiguration configuration;
        private AppDbContext context;
        private string? AppUserCorrelation { set; get; }
        private string? SuperLevelCorrelation { set; get; }
        private Double Duration = 1;// Min

        [SetUp]
        public void Setup()
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Tools&Utilities\\appsettings.json")
                .Build();
            // Build DBContext
            context = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(configuration.GetConnectionString("DefaultConnection")).Options
                );
            //Diminish
            Diminish.AppName = configuration.GetSection("ConnectionStrings:AppName").Value;
            Diminish.AppCode = configuration.GetSection("ConnectionStrings:APICode").Value?.ToString() ?? "MineCode";
            Diminish.AppDiminishPath = configuration.GetSection("ConnectionStrings:MapPath").Value;
            Diminish.AppTurnaround = Double.Parse(configuration.GetSection("ConnectionStrings:APILoad").Value?.ToString() ?? "1");
            Diminish.AppNameSuffix = $"{DateTime.Now.ToString("yyyyMMdd")}";
            Diminish.connectionString = $"Data Source={Diminish.AppDiminishPath}{Diminish.AppCode}{Diminish.AppTurnaround}{Diminish.AppName}{Diminish.AppNameSuffix}.db;Version=3;";
            Directory.CreateDirectory(Diminish.AppDiminishPath??"");
        }
        [Test, Order(1)]
        public void TestRole()
        {
            //return;//Mark Skip
            // Create Role Audios
            using (InstanceRole? r = JsonConvert.DeserializeObject<InstanceRole>("{\r\n  \"surrogate\": \"307f6e01-d7b4-425c-abb2-62dfc06e16c0\",\r\n  \"accessLevel\": \"Audios\"\r\n}"))
            {
                Console.WriteLine($"Demo Create Role:{JsonConvert.SerializeObject(r?.CreateRole(r, context))}");
            }
            // Create Role Video, update to Videos
            // Nested Find Audios, update to Audio, &
            // Find Videos Details
            // Final Result: New Role -> Video & Audio
            using (InstanceRole r = new InstanceRole())
            {
                r.AccessLevel = "Video";
                Console.WriteLine($"Demo Create Role:{JsonConvert.SerializeObject(r.CreateRole(r, context))}");
                // Update Role New Role Name
                r.AccessLevel = "Videos";
                Console.WriteLine($"Demo Update Role:{JsonConvert.SerializeObject(r.UpdateRole(r, context))}");

                // Find Role Id of Audios & update to Audio
                using (InstanceRole? ra = new InstanceRole())
                {
                    ra.Surrogate = string.Empty;
                    ra.AccessLevel = "Audios";
                    ResponseRole resp = ra.Find(ra, context);
                    if (resp != null)
                    {
                        Console.WriteLine($"Demo Find Role Id:{resp.Role?.Id}");
                        ra.Surrogate = resp.Role?.Id;// Get Id
                        // Update Role Audio to Admin_R0le by Surrogate
                        ra.AccessLevel = "Admin_R0le";
                        Console.WriteLine($"Demo Formate Name Error, Update Role:{JsonConvert.SerializeObject(ra.UpdateRole(ra, context))}");
                        ra.AccessLevel = "Audio";
                        Console.WriteLine($"Demo Update Role:{JsonConvert.SerializeObject(ra.UpdateRole(ra, context))}");
                    }
                }
                // find Role Details
                Console.WriteLine($"Demo Role Details:{JsonConvert.SerializeObject(r.Find(r, context))}");
            }
            // Create Role Admin_R0le
            using (InstanceRole? r = JsonConvert.DeserializeObject<InstanceRole>("{\r\n  \"surrogate\": \"307f6e01-d7b4-425c-abb2-62dfc06e16c0\",\r\n  \"accessLevel\": \"Admin_R0le\"\r\n}"))
            {
                Console.WriteLine($"Demo Formate Name Error, Create Role:{JsonConvert.SerializeObject(r?.CreateRole(r, context))}");
            }
            Assert.Pass();
        }
        [Test, Order(2)]
        public void TestClient()
        {
            //return;//Mark Skip
            // Create Blank Super Level User: shahidMuhamad
            using (InstanceUser? u = JsonConvert.DeserializeObject<InstanceUser>("{\"surrogate\": \"13ca8af5-b699-4a7e-8bda-90926fb151e4\",\"accessSpecifier\": \"shahidMuhamad\",\"mailingAddress\": \"shahid.Muhamad@gmail.com\",\"watchWord\": \"SamplingOre*8989#\",\"isdn\": \"092-3335930849\",\"superlevel\": \"true\"}"))
            {
                if (u != null)
                    Console.WriteLine($"Demo Create Blank Super Level User:{JsonConvert.SerializeObject(u.CreateUser(u, context))}");
            }
            // Create Blank User: shahidMohamad
            using (InstanceUser? u = JsonConvert.DeserializeObject<InstanceUser>("{\"surrogate\": \"13ca8af5-b699-4a7e-8bda-90926fb151e4\",\"accessSpecifier\": \"shahidMohamad\",\"mailingAddress\": \"shahid.Mohamad@gmail.com\",\"watchWord\": \"SamplingOre*8989#\",\"isdn\": \"092-3335930849\"}"))
            {
                if (u != null)
                    Console.WriteLine($"Demo Create Blank User:{JsonConvert.SerializeObject(u.CreateUser(u, context))}");
            }
            // Create User: shahidMohammad,
            // Update User ISDN, MailingAddress
            using (InstanceUser? u = JsonConvert.DeserializeObject<InstanceUser>("{\"surrogate\": \"13ca8af5-b699-4a7e-8bda-90926fb151e4\",\"accessSpecifier\": \"shahidMohammad\",\"mailingAddress\": \"shahid.here@yahoo.com\",\"watchWord\": \"SamplingOre*8899#\",\"isdn\": \"092-3335930849\"}"))
            {
                if (u != null)
                {
                    Console.WriteLine($"Demo Create User:{JsonConvert.SerializeObject(u.CreateUser(u, context))}");
                    // Update ISDN & Email
                    u.ISDN = "+92-3335930849";
                    u.MailingAddress = "shahid.here.business@gmail.com";
                    Console.WriteLine($"Demo Update User Details:{JsonConvert.SerializeObject(u.UpdateUser(u, context))}");
                }
            }
            // Find User Id from Credentials,
            // Update Credentials with Super Administrator rights,
            // Tag roles Audio & Video,
            // Find User Details
            using (InstanceUser? u = new InstanceUser())
            {
                u.AccessSpecifier = "shahidMohammad";
                u.WatchWord = "SamplingOre*8899#";
                ResponseUserRoles resp = u.Find(u, context);
                // Update Credentials
                if (resp.User != null)
                {
                    u.Surrogate = resp.User?.Id;// Get Id
                    Console.WriteLine($"Demo Find User Id:{u.Surrogate}");
                    //Update User Access Specifier
                    u.AccessSpecifier = "ShahidMuhammad";
                    Console.WriteLine($"Demo Update User Access Specifier:{JsonConvert.SerializeObject(u.UpdateUser(u, context))}");
                    //Update User WatchWord
                    u.WatchWord = "SamplingCore*8899#";
                    Console.WriteLine($"Demo Update User WatchWord:{JsonConvert.SerializeObject(u.UpdateUser(u, context))}");
                    //Update User Super Administrator Rights
                    u.SuperLevel = true;// Super Access Level
                    Console.WriteLine($"Demo Update User Access Level:{JsonConvert.SerializeObject(u.UpdateUser(u, context))}");
                }
                // Tag Instance
                using (InstanceUserRole tag = new InstanceUserRole())
                {
                    // Assign Audio Role
                    tag.AccessSpecifier = resp.User?.UserName;
                    tag.Role = "Audio";
                    Console.WriteLine($"Demo Assign Role Audio:{JsonConvert.SerializeObject(tag.Tag(tag, context))}");
                    // Assign Videos Role
                    tag.AccessSpecifier = resp.User?.UserName;
                    tag.Role = "Video";
                    Console.WriteLine($"Demo Assign Role Video:{JsonConvert.SerializeObject(tag.Tag(tag, context))}");
                    //Find User Details
                    Console.WriteLine($"Demo User Details:{JsonConvert.SerializeObject(u.Find(u, context))}");
                }
            }
            // Find User with Credentials
            using (InstanceUser? u = new InstanceUser())
            {
                u.AccessSpecifier = "ShahidMuhammad";
                u.WatchWord = "SamplingCore*8899#";
                ResponseUserRoles ur = u.Find(u, context);
                Console.WriteLine($"Demo User Details:{JsonConvert.SerializeObject(ur)}");
                SuperLevelCorrelation = ur?.User.SecurityStamp??string.Empty;
            }            

            Assert.Pass();
        }
        [Test, Order(3)]
        public void TestClientRole()
        {
            //return;//Mark Skip
            // Create Role AUX,
            // Assign Role to User ShahidMuhammad
            // Assign Invalid Role Video to ShahidMuhammad
            using (InstanceRole r = new InstanceRole())
            {
                r.AccessLevel = "AUX";
                r.SuperLevel = true;// Super Access Level
                Console.WriteLine($"Demo Create Role:{JsonConvert.SerializeObject(r.CreateRole(r, context))}");
                // Assign Role AUX & Invalid Video
                using (InstanceUserRole tag = new InstanceUserRole())
                {
                    tag.AccessSpecifier = "ShahidMuhammad";
                    tag.Role = r.AccessLevel;
                    Console.WriteLine($"Demo Assign Role 'AUX' to User:{JsonConvert.SerializeObject(tag.Tag(tag, context))}");
                    // Find and Assign Role
                    r.AccessLevel = "Video";
                    ResponseRole resp = r.Find(r, context);
                    if (!resp.Info)
                    {
                        tag.AccessSpecifier = "ShahidMuhammad";
                        tag.Role = r.AccessLevel;
                        Console.WriteLine($"Demo Assign Role 'Video' to User:{JsonConvert.SerializeObject(tag.Tag(tag, context))}");
                    }
                }
            }
        }
        [Test, Order(4)]
        public void TestListAndRemove()
        {
            // Remove Role Audios,
            // Fetch User,
            // User Access Level Administrator Rights,
            // Fetch List of Users,
            // Remove Users,
            // Fetch Roles,
            // Remove Roles
            using (InstanceRole r = new InstanceRole())
            {
                r.AccessLevel = "Audios";
                Console.WriteLine($"Demo Delete Role:{JsonConvert.SerializeObject(r.Remove(r, context))}");
                // Make Clean
                return; //Mark Skip
                // Fetch User
                using (InstanceUser? u = JsonConvert.DeserializeObject<InstanceUser>("{\r\n  \"surrogate\": \"13ca8af5-b699-4a7e-8bda-90926fb151e4\",\r\n  \"accessSpecifier\": \"ShahidMuhammad\",\r\n  \"mailingAddress\": \"shahid.here@yahoo.com\",\r\n  \"watchWord\": \"SamplingCore*8899#\",\r\n  \"isdn\": \"+92-3335930849\"\r\n}"))
                {
                    if (u != null)
                    {
                        // Assign Administrator Access Level Credentials
                        u.AccessSpecifier = "ShahidMuhammad";
                        u.WatchWord = "SamplingCore*8899#";
                        ResponseUserRoles res = u.Find(u, context);
                        // Fetch List of Users
                        ResponseUsers resp = u.List(u, context);
                        if (resp.Users != null)
                        {
                            // Remove Users
                            foreach (AppUser? au in resp.Users)
                            {
                                u.Surrogate = au?.Id;
                                Console.WriteLine($"Demo Delete User:{JsonConvert.SerializeObject(u.Remove(u, context))}");
                            }
                        }
                        //Fetch List of Roles            
                        r.AccessLevel = "AUX";
                        ResponseRoles respR = r.List(r, context);
                        if (respR.Roles != null)
                        {
                            // Remove Roles
                            foreach (AppRole? ar in respR.Roles)
                            {
                                r.Surrogate = ar?.Id;
                                Console.WriteLine($"Demo Delete Role:{JsonConvert.SerializeObject(r.Remove(r, context))}");
                            }
                        }
                    }
                }
            }
        }
        [Test, Order(5)]
        public void TestUserLogin()//Confirm/Resend/Reset/Forget
        {
            using (InstanceService? srv = new InstanceService())
            {
                Console.WriteLine($"Demo Invalid App User:{JsonConvert.SerializeObject(
                    srv.Indorse(JsonConvert.DeserializeObject<InstanceUser>
                    ("{\"surrogate\": \"13ca8af5-b699-4a7e-8bda-90926fb151e4\",\"accessSpecifier\": \"shahidMohammad\",\"mailingAddress\": \"shahid.hare@yahoo.com\",\"watchWord\": \"Samplingore*8899#\",\"isdn\": \"92-3335930849\"}")!, context))}");
                ResponseUser ru = srv.Indorse(JsonConvert.DeserializeObject<InstanceUser>
                    ($"{{\"surrogate\": \"13ca8af5-b699-4a7e-8bda-90926fb151e4\",\"accessSpecifier\": \"shahidMohammad\",\"mailingAddress\": \"shahid.here@yahoo.com\",\"watchWord\": \"SamplingOre*8899#\",\"isdn\": \"092-3335930849\",\"Duration\": \"{Duration}\"}}")!, context);
                AppUserCorrelation = ru?.User?.SecurityStamp ?? string.Empty;
                Console.WriteLine($"Demo Indorse App User:{JsonConvert.SerializeObject(ru)}");

                Console.WriteLine($"Demo Indorse Trivial Administrator:{JsonConvert.SerializeObject(
                    srv.Indorse(JsonConvert.DeserializeObject<InstanceUser>
                    ("{\"surrogate\": \"13ca8af5-b699-4a7e-8bda-90926fb151e4\",\"accessSpecifier\": \"shahidMohammad\",\"mailingAddress\": \"shahid.here@yahoo.com\",\"watchWord\": \"SamplingOre*8899#\",\"isdn\": \"092-3335930849\",\"superlevel\": \"true\"}")!, context))}");

                #region Indorse Multiplicity
                // Valid Surrogate
                Console.WriteLine($"Demo Indorse App Administrator:{JsonConvert.SerializeObject(
                    srv.Indorse(JsonConvert.DeserializeObject<InstanceUser>
                    ("{\"surrogate\": \"4b103269-6307-4872-8b48-59b2dc018be8\",\"superlevel\": \"true\"}")!, context))}");
                // Valid AccessSpecifier & WatchWord
                Console.WriteLine($"Demo Indorse App Administrator:{JsonConvert.SerializeObject(
                    srv.Indorse(JsonConvert.DeserializeObject<InstanceUser>
                    ("{\"accessSpecifier\": \"ShahidMuhammad\",\"watchWord\": \"SamplingCore*8899#\",\"superlevel\": \"true\"}")!, context))}");
                // Valid MailingAddress & WatchWord
                Console.WriteLine($"Demo Indorse App Administrator:{JsonConvert.SerializeObject(
                    srv.Indorse(JsonConvert.DeserializeObject<InstanceUser>
                    ("{\"mailingAddress\": \"shahid.here.business@gmail.com\",\"watchWord\": \"SamplingCore*8899#\",\"superlevel\": \"true\"}")!, context))}");
                // Valid ISDN & WatchWord
                Console.WriteLine($"Demo Indorse App Administrator:{JsonConvert.SerializeObject(
                    srv.Indorse(JsonConvert.DeserializeObject<InstanceUser>
                    ("{\"watchWord\": \"SamplingCore*8899#\",\"isdn\": \"+92-3335930849\",\"superlevel\": \"true\"}")!, context))}");
                #endregion
            }
        }
        [Test, Order(6)]
        public void TestUserTokenization()//Header Token
        {
            using (InstanceService? srv = new InstanceService())
            {
                // App Administrator
                Console.WriteLine($"Demo User & Role Authorization via Administrator:{JsonConvert.SerializeObject(
                    srv.Substantiate(JsonConvert.DeserializeObject<InstanceUser>
                    ($"{{\"Correlation\": \"{SuperLevelCorrelation}\"}}")!, context))}");
                // App User
                ResponseUserRoles r = srv.Substantiate(JsonConvert.DeserializeObject<InstanceUser>
                ($"{{\"Correlation\": \"{AppUserCorrelation}\"}}")!, context);
                Console.WriteLine($"Demo App User & Role Authorization:{JsonConvert.SerializeObject(r)}");
                foreach (var info in r.Roles)
                    Console.WriteLine($"App User has Role:{info.NormalizedName}");

                // App User Recursion
                int count = 0;
                do
                {
                    r = srv.Substantiate(JsonConvert.DeserializeObject<InstanceUser>
                    ($"{{\"Correlation\": \"{AppUserCorrelation}\"}}")!, context);
                    foreach (var info in r.Roles)
                        if (count == 0) Console.WriteLine($"App User Recursions has Role:{info.NormalizedName}");
                    count++;
                } while (r.Info);
                Console.WriteLine($"App User Recursions:{count}; TPS:{Math.Round(count / Duration / 60, 0)}");
            }
        }
        [Test, Order(7)]
        public void TestRoleClaim()//Super Administrator
        { }
        [Test, Order(8)]
        public void TestUserClaim()//JWT/OWIN/TwoFA/TTE
        { }
        [TearDown]
        public void FinishTest()
        {
            context.Dispose();
        }
    }
}
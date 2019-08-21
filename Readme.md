# CZGL.Auth

CZGL.Auth 是一个基于 Jwt 实现的快速角色授权库，ASP.Net Core 的 Identity 默认的授权是 Cookie。而 Jwt 授权只提供了基础实现和接口，需要自己实现角色授权和上下文拦截等。

使用第三方开源类库，例如 IdentityServer4 ，过于复杂，学习成本和开发成本较高。

于是空闲时间，写了这个库。

- 基于角色授权
- 每个API均可授权
- 实时更新权限
- 快速配置

使用方法:

Nuget 中搜索 CZGL.Auth ，安装 1.0.0版本，适用于 ASP.NET Core 2.x。

#### 注入服务

在 Startup.cs 中

```c#
using CZGL.Auth.Services;
```

 

ConfigureServices 中，注入服务

```c#
            services.AddRoleService();
```

#### 配置服务

在 Program 文件中创建一个方法，在启动网站前配置角色授权服务：

使用 AuthBuilder 可以配置授权认证的配置

引入

```c#
using CZGL.Auth.Services;
using CZGL.Auth.Models;
using CZGL.Auth.Interface;
```

你可以像这样快速配置：

```c#
            new AuthBuilder()
               .Security() 
               .Jump()
               .Time(TimeSpan.FromMinutes(20))
               .DefaultRole("user")
               .End();
// 无需接收返回值，直接这样写即可
```

Security 中配置 密钥、默认用户的角色、Token颁发者、Token订阅者。

​	密钥应当使用私钥证书的文本内容；请设定一个无用的默认角色或者乱填一个无用的字符串，在认证失效或其它原因是，会使用此角色；这个默认角色是存放在系统中的。

Jump 中填写登录URL、无权访问时跳转URL和是否开启跳转功能。

如果不开启，则在失败时直接返回 401 ；如果开启，在用户没有登录或凭证已经失效时，会跳转到相应页面。

Time 中填写凭证失效的时间，即颁发的凭证有效时间，可以以分钟、秒为单位。一般都是设置20/30分钟。

DefaultRole 设置默认角色，这个默认角色是给为登录或凭证失效时设置，或者颁发凭证后系统删除了这个角色等使用。乱填就行，不要跟真正的用户角色名称一致即可。



#### 角色授权

使用 RolePermission.AddRole() 可以增加一个角色，

```c#
            var usera = new Role()
            {
                RoleName = "supperadmin",
                Apis = new List<IApiPermission>
                {
                new ApiPermission{Name="A",Url="/api/Test/A" },
                new ApiPermission{Name="AB",Url="/api/Test/AB" },
                new ApiPermission{Name="AC",Url="/api/Test/AC" },
                new ApiPermission{Name="ABC",Url="/api/Test/ABC" }
                }
            };
```

```c#
            RolePermission.AddRole(usera);
```

RoleName ：角色名称

Apis：角色能够访问的API

IApiPermission：一个API，Name API名称，Url API地址。

校验角色和API地址时，不区分大小写。

角色会存储到内存中，你可以随时添加或删除角色。例如从数据库中读取权限存储到系统中。

为了安全和避免同步问题，只允许以角色为单位操作。

RolePermission 中可以添加或删除角色。



#### 登录、颁发凭证

创建 AccountController API控制器

```c#
        private readonly AuthorizationRequirement _requirement;
        public AccountController(AuthorizationRequirement requirement)
        {
            _requirement = requirement;
        }
```

如果你不是用 AuthorizationRequirement 注入，那么颁发的会是上面设置的默认用户，这可能会导致授权问题。

登录：

```c#
        [HttpPost("Login")]
        public JsonResult Login(string username, string password)
        {
            // 中数据库中判断账号密码是否正确，并获取用户所属角色等角色
            var user = UserModel.Users.FirstOrDefault(x => x.UserName == username && x.UserPossword == password);
            
            if (user == null)
                return new JsonResult(
                    new ResponseModel
                    {
                        Code = 0,
                        Message = "登陆失败!"
                    });

            // 实例化加密和颁发 Token的类
            EncryptionHash hash = new EncryptionHash();

            // 将用户标识存储到系统中
            _requirement.SetUserRole(user.Role);


            //// 配置用户标识
            //// 方法一
            //var userClaims = new Claim[]
            //{
            //    new Claim(ClaimTypes.Name,user.UserName),
            //    new Claim(ClaimTypes.Role,user.Role),
            //    new Claim(ClaimTypes.Expiration,DateTime.Now.AddMinutes(TimeSpan.FromMinutes(20)).ToString()),
            //};

            // 方法二
            var userClaims = hash.GetClaims(username, user.Role);


            // 颁发 token
            var identity = hash.GetIdentity(userClaims);
            var jwt = hash.BuildJwtToken(userClaims);
            var token = hash.BuildJwtResponseToken(jwt);


            return new JsonResult(
                new ResponseModel
                {
                    Code = 200,
                    Message = "登陆成功!请注意保存你的 Token 凭证!",
                    Data = token
                });
        }
```


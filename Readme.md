[TOC]
6666
### 说明

ASP.NET Core 3.0 一个 jwt 的轻量角色/用户、单个API控制的授权认证库

最近得空，重新做一个角色授权库，而之前做了一个角色授权库，是利用微软的默认接口做的，查阅了很多文档，因为理解不够，所以最终做出了有问题。

之前的旧版本 https://github.com/whuanle/CZGL.Auth/tree/1.0.0

如果要使用微软的默认接口，我个人认为过于繁杂，而且对于这部分的资料较少。。。

使用默认接口实现授权认证，可以参考我另一篇文章

[ASP.NET Core 使用 JWT 自定义角色/策略授权需要实现的接口](https://www.cnblogs.com/whuanle/p/11377792.html)

得益于大笨熊哥的引导，利用放假时间重新做了一个，利用微软本身的授权认证，在此基础上做拓展。特点是使用十分简便，无需过多配置；因为本身没有“造轮子”，所以如果需要改造，也十分简单。

此库更新到 .Net Core 3.0 了，如果需要在 2.2X 上使用，可以到仓库下载项目，然后把 Nuget 包换成 2.2 的。

感谢大笨熊哥的指导。

项目仓库地址 https://github.com/whuanle/CZGL.Auth

## 一、定义角色、API、用户

随便新建一个网站或API项目，例如 MyAuth。

Nuget 里搜索 CZGL.Auth，按照 2.0.1 版本，或者使用 Package Manager 命令

```powershell
Install-Package CZGL.Auth -Version 2.0.1
```

CZGL.Auth 设计思路是，网站可以存在多个角色、多个用户、多个API，

一个角色拥有一些 API，可以添加或删除角色或修改角色所有权访问的 API；

一个用户可以同时属于几个角色。

第一步要考虑网站的角色、用户、API设计， 

CZGL.Auth 把这些信息存储到内存中，一个用户拥有那几个角色、一个角色具有哪些API的访问权限。

角色跟 API 是对应关系，用户跟角色是多对多关系。

新建一个类 RoleService.cs ，引入 `using CZGL.Auth.Services;`，RoleService 继承 ManaRole。

通过以下接口操作角色权限信息

```c#
        protected bool AddRole(RoleModel role);
        protected bool AddUser(UserModel user);
        protected bool RemoveRole(string roleName);
        protected bool RemoveUser(string userName);
```

很明显，添加/移除一个角色，添加/移除一个用户

假如有 A、B、C 三个角色，
有 /A、/B、/C、/AB、/AC、/BC、/ABC 共7个API，设定权限

A 可以访问 A、AB、AC、ABC

B 可以访问 B、AB、BC、ABC

C 可以访问 C、AC、BC、ABC

这里采用模拟数据的方法，不从数据库里面加载实际数据。

在 RoleService 里面增加一个方法

```c#
        /// <summary>
        /// 用于加载角色禾API
        /// </summary>
        public void UpdateRole()
        {
            List<RoleModel> roles = new List<RoleModel>
            {
                new RoleModel
                {
                    RoleName="A",
                    Apis=new List<OneApiModel>
                    {
                        new OneApiModel
                        {
                            ApiName="A",
                            ApiUrl="/A"
                        },
                        new OneApiModel
                        {
                            ApiName="AB",
                            ApiUrl="/AB"
                        },
                        new OneApiModel
                        {
                            ApiName="AC",
                            ApiUrl="/AC"
                        },
                        new OneApiModel
                        {
                            ApiName="ABC",
                            ApiUrl="/ABC"
                        }
                    }
                },
                new RoleModel
                {
                    RoleName="B",
                    Apis=new List<OneApiModel>
                    {
                        new OneApiModel
                        {
                            ApiName="B",
                            ApiUrl="/B"
                        },
                        new OneApiModel
                        {
                            ApiName="AB",
                            ApiUrl="/AB"
                        },
                        new OneApiModel
                        {
                            ApiName="BC",
                            ApiUrl="/BC"
                        },
                        new OneApiModel
                        {
                            ApiName="ABC",
                            ApiUrl="/ABC"
                        }
                    }
                },
                new RoleModel
                {
                    RoleName="A",
                    Apis=new List<OneApiModel>
                    {
                        new OneApiModel
                        {
                            ApiName="A",
                            ApiUrl="/A"
                        },
                        new OneApiModel
                        {
                            ApiName="AB",
                            ApiUrl="/AB"
                        },
                        new OneApiModel
                        {
                            ApiName="AC",
                            ApiUrl="/AC"
                        },
                        new OneApiModel
                        {
                            ApiName="ABC",
                            ApiUrl="/ABC"
                        }
                    }
                }
            };
            foreach (var item in roles)
            {
                AddRole(item);
            }

        }
```

有了角色禾对应的API信息，就要添加用户了，

假设有 aa、bb、cc 三个用户，密码都是 123456，aa 属于 A 角色， bb 属于 B角色...

```c#
        public void UpdateUser()
        {
            AddUser(new UserModel { UserName = "aa", BeRoles = new List<string> { "A" } });
            AddUser(new UserModel { UserName = "bb", BeRoles = new List<string> { "B" } });
            AddUser(new UserModel { UserName = "cc", BeRoles = new List<string> { "C" } });
        }
```

为了能够把角色和用户加载进 CZGL.Auth ，你需要在程序启动时，例如在 Program 里，使用

```c#
            RoleService roleService = new RoleService();
            roleService.UpdateRole();
            roleService.UpdateUser();
```



## 二、添加自定义事件

授权是，可能会有各种情况，你可以添加自定义事件记录下用户访问的授权信息、影响授权结果。

引用 `using CZGL.Auth.Interface;`，

添加一个类 RoleEvents 继承 IRoleEventsHadner 

```c#
    public class RoleEvents : IRoleEventsHadner
    {
        public async Task Start(HttpContext httpContext)
        {
            await Task.CompletedTask;
        }
        public void TokenEbnormal(object eventsInfo)
        {
        }
        public void TokenIssued(object eventsInfo)
        {
        }
        public void NoPermissions(object eventsInfo)
        {
        }
        public void Success(object eventsInfo)
        {
        }
        public async Task End(HttpContext httpContext)
        {
            await Task.CompletedTask;
        }
    }
```

在 CZGL.Auth 开始验证授权前调用 Start，结束时调用 End，传入传参数是 HttpContext 类型，你可以在里面添加自定义授权的信息，在里面可以影响请求管道。

其他几个方法含义如下：

TokenEbnormal 客户端携带的 Token 不是有效的 Jwt 令牌，将不能被解析

TokenIssued 令牌解码后，issuer 或 audience不正确

NoPermissions 无权访问此 API

在授权认证的各个阶段将会调用上面的方法。



## 三、注入授权服务和中间件

使用 CZGL.Auth ，你需要注入以下两个服务

```c#
            services.AddRoleService(authOptions);
            services.AddSingleton<IRoleEventsHadner, RoleEvents>();
```

`AddRoleService` 是注入授权服务，`AddSingleton` 注入你的事件。

AddRoleService 需要一个 AuthConfigModel 类型作参数。

你可以这样配置

```c#
            var authOptions = new AuthBuilder()
                .Security("aaaafsfsfdrhdhrejtrjrt", "ASPNETCORE", "ASPNETCORE")
                .Jump("accoun/login", "account/error", false, false)
                .Time(TimeSpan.FromMinutes(20))
                .InfoScheme(new CZGL.Auth.Models.AuthenticateScheme
                {
                    TokenEbnormal = "Login authentication failed!",
                    TokenIssued = "Login authentication failed!",
                    NoPermissions = "Login authentication failed!"
                }).Build();
            services.AddRoleService(authOptions);

            services.AddSingleton<IRoleEventsHadner, RoleEvents>();
```

Security 配置密钥相关，参数分别是密钥字符串、颁发者、订阅者。

Jump 配置授权失败时，跳转地址。参数分别是未授权时跳转、授权无效跳转，后面两个 bool 可以设置跳转或跳转。

Time 配置 Token 有效期。

InfoScheme 授权失败提示信息，例如

![](https://img2018.cnblogs.com/blog/1315495/201910/1315495-20191026152753056-91051073.png)

上图的是时间过期的提示消息，用户请求API失败时返回 401 状态码，Header 会携带提示消息，CZGL.Auth 里面设置了三种情况下，自定义头部：

TokenEbnormal 客户端携带的 Token 不是有效的 Jwt 令牌，将不能被解析

TokenIssued 令牌解码后，issuer 或 audience不正确

NoPermissions 无权访问此 API



添加三个中间件

```c#
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<RoleMiddleware>();
```

`app.UseAuthorization();`是微软授权认证的中间件，CZGL.Auth 会先让，默认的验证管道过滤一些无效请求和认证信息，再由 CZGL.Auth 来校验授权。



## 三、如何设置API的授权

很简单，CZGL.Auth 的认证授权，你只需在 Controller 或 Action上 添加 `[Authorize]`。

CZGL.Auth 只会对使用了 `[Authorize]` 特性的 Controller  或 Action 生效。

如果一个 Controller 已经设置了 `[Authorize]` ，但是你想里面的 Action 跳过授权认证，则使用     `[AllowAnonymous]` 修饰 Action。

使用方法跟微软的默认的完全一致。这样无需过多配置。

如果你想另外定义一个特性用来另外设置 授权的话，可以到我的仓库提 Issue 或者直接联系我微信。

添加一个 APIController ，

```c#
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet("/A")]
        public JsonResult A()
        {
            return new JsonResult(new { Code = 200, Message = "Success!" });
        }

        [HttpGet("/B")]
        public JsonResult B()
        {
            return new JsonResult(new { Code = 200, Message = "Success!" });
        }

        [HttpGet("/C")]
        public JsonResult C()
        {
            return new JsonResult(new { Code = 200, Message = "Success!" });
        }
        [HttpGet("/AB")]
        public JsonResult AB()
        {
            return new JsonResult(new { Code = 200, Message = "Success!" });
        }
        [HttpGet("/BC")]
        public JsonResult BC()
        {
            return new JsonResult(new { Code = 200, Message = "Success!" });
        }
        [HttpGet("/AC")]
        public JsonResult AC()
        {
            return new JsonResult(new { Code = 200, Message = "Success!" });
        }

        [HttpGet("/ABC")]
        public JsonResult ABC()
        {
            return new JsonResult(new { claims = User.Claims });
        }


        /// <summary>
        /// 任何人都不能访问
        /// </summary>
        /// <returns></returns>
        [HttpGet("D")]
        public JsonResult D()
        {
            return new JsonResult(new { Code = 200, Message = "Success!" });
        }

        [HttpGet("error")]
        public JsonResult Denied()
        {
            return new JsonResult(
                new
                {
                    Code = 0,
                    Message = "访问失败!",
                    Data = "此账号无权访问！"
                });
        }
    }
```

## 四、添加登录颁发 Token 

添加一个 AccountController.cs 用来颁发登录、 Token。

```c#
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("/Login")]
        public async Task<JsonResult> Login([FromQuery]string username, string password, string rolename)
        {
            // 用户名密码是否正确
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(rolename))
            {
                return new JsonResult(new 
                {
                    Code = 0,
                    Message = "尼玛，上传什么垃圾信息",
                });
            }

            if(!((username=="aa"||username=="bb"||username=="cc")&&password=="123456"))
            {
                return new JsonResult(new
                {
                    Code = 0,
                    Message = "账号或密码错误",
                });
            }

            // 你自己定义的角色/用户信息服务
            RoleService roleService = new RoleService();

            // 检验用户是否属于此角色
            var role = roleService.IsUserToRole(username,rolename);

            // CZGL.Auth 中一个用于加密解密的类
            EncryptionHash hash = new EncryptionHash();

            // 设置用户标识
            var userClaims = hash.BuildClaims(username, rolename);

            //// 自定义构建配置用户标识
            /// 自定义的话，至少包含如下标识
            //var userClaims = new Claim[]
            //{
            //new Claim(ClaimTypes.Name, userName),
            //    new Claim(ClaimTypes.Role, roleName),
            //    new Claim(JwtRegisteredClaimNames.Aud, Audience),
            //    new Claim(ClaimTypes.Expiration, TimeSpan.TotalSeconds.ToString()),
            //    new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString())
            //};
            /*
            iss (issuer)：签发人
            exp (expiration time)：过期时间
            sub (subject)：主题
            aud (audience)：受众
            nbf (Not Before)：生效时间
            iat (Issued At)：签发时间
            jti (JWT ID)：编号
            */

            // 方法一，直接颁发 Token
            ResponseToken token = hash.BuildToken(userClaims);


            //方法二，拆分多步，颁发 token，方便调试
            //var identity = hash.GetIdentity(userClaims);
            //var jwt = hash.BuildJwtToken(userClaims);
            //var token = hash.BuildJwtResponseToken(jwt);

            return new JsonResult(token);
        }
    }
```



## 五、部分说明

注入 Jwt 服务、颁发 Token

CZGL.Auth 把使用 jwt 的服务和颁发 Token 的代码封装好了，这个库不是在“造轮子”，所以实际上你可以很轻松的把这部分的代码抽出来，另外设计。

这部分的代码所在位置 RoleServiceExtension.cs 、EncryptionHash.cs。

授权中间件

```c#
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<RoleMiddleware>();
```

我的写法是利用 ASP.NET Core 的 jwt 完成基础的认证授权，然后在下一个管道中实现拓展的认证。但是本身的认证是在 app.UseAuthorization(); 做了拓展，所以使用 CZGL.Auth，只需要按照平常 jwt 的方式去使用，只是加了一个 RoleMiddleware 中间件。

CZGL.Auth 只是我受到新思路启发临时写出来的。。。最好不要由于生产，去 github 库把项目下载下来，按照自己应用场景改一下~。

## 六、验证

先使用 aa 用户登录，登录时选择 A 角色。

![](https://img2018.cnblogs.com/blog/1315495/201910/1315495-20191026152808251-1987959054.png)

因为 A 用户只能访问 “带有 A ” 的API， "/A"、"/AB" 等，所以我们可以试试。

![](https://img2018.cnblogs.com/blog/1315495/201910/1315495-20191026152814666-1965964026.png)



继续用这个 Token 访问一下 "/B"

![](https://img2018.cnblogs.com/blog/1315495/201910/1315495-20191026152821376-2088997109.png)

可以继续尝试添加 API 或者使用其他用户登录，访问不同的 API。

由于别人对前端不熟，所以就不写带页面的示例了~。

可以用 Postman 就行测试。

什么示例的 项目可以到仓库里下载，名称是 MyAuth。

一般上，用户权限、角色权限信息是存储在数据库里面的，另一个示例是 CZGL.Auth.Sample2。

这个库只是较为粗略的授权认证，与更丰富的需求请自行下载源码修改~

有问题要讨论，可以在俱乐部里面找到我。

深圳、广州、长沙、上海的群等我都在，嘿嘿嘿，嘿嘿嘿。

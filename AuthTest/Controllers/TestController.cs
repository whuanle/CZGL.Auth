using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZGL.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthTest.Controllers
{
    [Authorize(Policy = "Permission")]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet("A")]
        public JsonResult A()
        {
            return new JsonResult(
                new ResponseModel
                {
                    Code = 200,
                    Message = "Success!"
                });
        }

        [HttpGet("B")]
        public JsonResult B()
        {
            return new JsonResult(
                new ResponseModel
                {
                    Code = 200,
                    Message = "Success!"
                });
        }

        [HttpGet("C")]
        public JsonResult C()
        {
            return new JsonResult(
                new ResponseModel
                {
                    Code = 200,
                    Message = "Success!"
                });
        }
        [HttpGet("AB")]
        public JsonResult AB()
        {
            return new JsonResult(
                new ResponseModel
                {
                    Code = 200,
                    Message = "Success!"
                });
        }
        [HttpGet("BC")]
        public JsonResult BC()
        {
            return new JsonResult(
                new ResponseModel
                {
                    Code = 200,
                    Message = "Success!"
                });
        }
        [HttpGet("AC")]
        public JsonResult AC()
        {
            return new JsonResult(
                new ResponseModel
                {
                    Code = 200,
                    Message = "Success!"
                });
        }
        [HttpGet("ABC")]
        public JsonResult ABC()
        {
            return new JsonResult(
                new ResponseModel
                {
                    Code = 200,
                    Message = "Success!"
                });
        }


        /// <summary>
        /// 任何人都不能访问
        /// </summary>
        /// <returns></returns>
        [HttpGet("D")]
        public JsonResult D()
        {
            return new JsonResult(
                new ResponseModel
                {
                    Code = 200,
                    Message = "Success!"
                });
        }

        [HttpGet("error")]
        public JsonResult Denied()
        {
            return new JsonResult(
                new ResponseModel
                {
                    Code = 0,
                    Message = "访问失败!",
                    Data = "此账号无权访问！"
                });
        }
    }
}
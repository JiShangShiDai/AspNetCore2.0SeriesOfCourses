﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Working.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Working.Models.Repository;
using Working.Models.DataModel;

namespace Working.Controllers
{
    [Authorize(Roles = "Manager,Leader,Employee")]
    public class HomeController : BaseController
    {
        /// <summary>
        /// 日记类
        /// </summary>

        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// 用户仓储
        /// </summary>
        readonly IUserRepository _userRepository;
        /// <summary>
        /// 部门仓储
        /// </summary>
        readonly IDepartmentRepository _departmentRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IDepartmentRepository departmentRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("这是HomeController下的Index Action");

            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region 登录
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.returnUrl = returnUrl;
            }
            return View();
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(string userName, string password, string returnUrl)
        {
            try
            {

                var userRole = _userRepository.Login(userName, password);
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Role,userRole.RoleName),
                    new Claim(ClaimTypes.Name,userRole.Name),
                    new Claim(ClaimTypes.Sid,userRole.ID.ToString())
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims)));
                return new RedirectResult(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);

            }
            catch (Exception exc)
            {
                ViewBag.error = exc.Message;
                return View();
            }
        }
        #endregion


        [HttpGet("departments")]
        public IActionResult Departments()
        {
            return View();
        }
        /// <summary>
        /// 获取全部带父级部门的部门
        /// </summary>
        /// <returns></returns>
        [HttpGet("getallpdepartment")]
        public IActionResult GetAllPDepartments()
        {
            try
            {
                var list = _departmentRepository.GetAllPDepartment();
                return ToJson(BackResult.Success, data: list);
            
            }
            catch(Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);           
            }
        }

        /// <summary>
        /// 获取全部带父级部门的部门
        /// </summary>
        /// <returns></returns>
        [HttpGet("getalldepartment")]
        public IActionResult GetAllDepartments()
        {
            try
            {
                var list = _departmentRepository.GetAllDepartment();
                return ToJson(BackResult.Success, data: list);

            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="deparment">部门</param>
        /// <returns></returns>
        [HttpPost("adddepartment")]
        public IActionResult AddDepartment(Department deparment)
        {
            try
            {
                var result = _departmentRepository.AddDepartment(deparment);
                return ToJson(result?BackResult.Success:BackResult.Fail,message:result?"添加成功":"添加失败");

            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
    }
}

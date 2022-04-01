using Entities;
using Entities.Enum;
using Entities.Model.Common;
using Entities.Model.Search;
using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Utlis.Extension;
using WebSite.Models;

namespace WebSite.Areas.SystemManagement.Controllers
{
    public class UserController : Common.BaseController
    {
        public UserController(IUserBll _userBll,IRoleBll _roleBll)
        {
            userBll = _userBll;
            roleBll = _roleBll;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetUserList(UserSearch info, int pageIndex, int pageSize, string orderby)
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                OrderConditions = new List<OrderCondition>
                {
                    new OrderCondition{ OrderbyField="UserName",IsAsc=true }
                }
            };
            var result = await userBll.GetUserList(pageModel, info);
            return Json(result);
        }

        public ActionResult ViewUser(string userId)
        {
            return View();
        }

        public ActionResult Add()
        {
            B_User model = new B_User();
            return View(model);
        }

        public async Task<ActionResult> Edit(string userId)
        {
            B_User model = new B_User();
            model = await userBll.GetUserById(userId.ToGuid());
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Save(B_User user)
        {
            var result = new AjaxResult();
            try
            {
                var olduser = await userBll.GetUserByUserName(user.UserName);
                if (olduser.IsNotNull() && olduser.Id != user.Id)
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "用户名已存在";
                }
                else
                {
                    //如果是新增，初始化密码
                    if (user.Id.IsNull())
                    {
                        user.Password = "1";
                    }

                    if (await userBll.SaveUser(user))
                    {
                        result.Status = AjaxStatus.Success;
                        result.Message = "保存成功";
                    }
                    else
                    {
                        result.Status = AjaxStatus.UnSuccess;
                        result.Message = "保存失败";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(string userId)
        {
            var result = new AjaxResult();

            try
            {
                if (await userBll.ResetPassword(userId.ToGuid(), "1"))
                {
                    result.Status = AjaxStatus.Success;
                    result.Message = "重置密码成功";
                }
                else
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "重置密码失败";
                }
            }
            catch (Exception ex)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var result = new AjaxResult();

            try
            {
                if (await userBll.DeleteUser(userId.ToGuid()))
                {
                    result.Status = AjaxStatus.Success;
                    result.Message = "停用用户成功";
                }
                else
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "停用用户失败";
                }
            }
            catch (Exception ex)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = ex.Message;
            }

            return Json(result);
        }

        public async Task<ActionResult> EditUserRole(string userId)
        {
            ViewBag.UserId = userId;
            var roleList = (await roleBll.GetRoleListAll()).OrderBy(d => d.RoleCode);
            var rrlist = roleList.Select(d => new SelectListItem
            {
                Text = "(" + d.RoleCode + ")" + d.RoleName,
                Value = d.Id.ToString()
            }).ToList();

            rrlist.Insert(0, new SelectListItem
            {
                Text = "请选择",
                Value = ""
            });
            ViewBag.RoleList = rrlist;
            ViewBag.RoleTypeList = await GetRoleTypeSelectList();
            return View();
        }

        private async Task<IEnumerable<SelectListItem>> GetRoleTypeSelectList()
        {
            var rlist = await roleBll.GetRoleTypeAll();
            var rrlist = rlist.Select(d => new SelectListItem
            {
                Text = d.RoleTypeName,
                Value = d.Id.ToString()
            }).ToList();

            rrlist.Insert(0, new SelectListItem
            {
                Text = "请选择",
                Value = ""
            });

            return rrlist;
        }

        public async Task<ActionResult> GetUserRoleList(string userId)
        {
            var userRoleList = await userBll.GetUserRoleByUserId(userId.ToGuid());
            var roleIds = userRoleList.ToList().Select(d => d.RoleId).ToList();
            var roleList = await roleBll.GetRoleListByRoleIds(roleIds);

            PageResult<B_Role> results = new PageResult<B_Role>
            {
                Data = roleList.OrderBy(d=>d.RoleCode).ToList()
            };
            return Json(results);
        }

        public async Task<ActionResult> AddUserRole(string userId, string roleId)
        {
            var result = new AjaxResult();
            if (userId == null || roleId == null)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = "新增角色失败,请选择需要添加的角色";
            }
            else
            {
                var userRole_old = await userBll.GetUserRoleByUserIdAndRoleId(userId.ToGuid(), roleId.ToGuid());
                if (userRole_old != null)
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "新增角色失败,用户已存在对应角色";
                }
                else
                {
                    B_UserRole userRole = new B_UserRole
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId.ToGuid(),
                        RoleId = roleId.ToGuid()
                    };
                    if (await userBll.SaveUserRole(userRole))
                    {
                        result.Status = AjaxStatus.Success;
                        result.Message = "新增角色成功";
                    }
                    else
                    {
                        result.Status = AjaxStatus.UnSuccess;
                        result.Message = "新增角色失败";
                    }
                }
            }

            return Json(result);
        }

        public async Task<ActionResult> DeleteUserRole(string userId,string roleId)
        {
            var result = new AjaxResult();
            if (userId == null || roleId == null)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = "保存失败,请选择需要删除的角色";
            }
            else
            {
                var userRole = await userBll.GetUserRoleByUserIdAndRoleId(userId.ToGuid(), roleId.ToGuid());
                if (userRole == null)
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "删除失败,用户不存在对应角色";
                }
                else
                {
                    if (await userBll.DeleteUserRole(userRole))
                    {
                        result.Status = AjaxStatus.Success;
                        result.Message = "删除成功";
                    }
                    else
                    {
                        result.Status = AjaxStatus.UnSuccess;
                        result.Message = "删除失败";
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> SaveUserRole(string userId, List<string> roleId)
        {
            var result = new AjaxResult();

            try
            {
                List<B_UserRole> userRoleList = new List<B_UserRole>();
                Guid userGuidId = userId.ToGuid();
                foreach (string s in roleId)
                {
                    B_UserRole userRole = new B_UserRole
                    {
                        Id = Guid.NewGuid(),
                        UserId = userGuidId,
                        RoleId = s.ToGuid()
                    };
                    userRoleList.Add(userRole);
                }
                if (await userBll.SaveUserRoles(userGuidId, userRoleList))
                {
                    result.Status = AjaxStatus.Success;
                    result.Message = "保存成功";
                }
                else
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "保存失败";
                }
            }
            catch (Exception ex)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = ex.Message;
            }

            return Json(result);
        }
    }
}
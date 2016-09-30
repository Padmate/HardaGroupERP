using HardaGroup.ERP.DataAccess;
using HardaGroup.ERP.Entities;
using HardaGroup.ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Service
{
    public class B_UserType
    {
        D_UserType _dUserType = new D_UserType();

        public M_UserType GetById(int id)
        {
            var usertype = _dUserType.GetUserTypeById(id);
            var result = ConverEntityToModel(usertype);
            return result;
        }

        public M_UserType GetByCode(string code)
        {
            var usertype = _dUserType.GetUserTypeByCode(code);
            var result = ConverEntityToModel(usertype);
            return result;
        }

        public List<M_UserType> GetAllData()
        {
            var usertypes = _dUserType.GetAll();
            var result = usertypes.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="usertype"></param>
        /// <returns></returns>
        public List<M_UserType> GetPageData(M_UserType usertype)
        {
            UserType searchModel = new UserType()
            {
                Code = usertype.Code,
                Name = usertype.Name
            };

            var offset = usertype.offset;
            var limit = usertype.limit;


            var pageResult = _dUserType.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_UserType usertype)
        {
            UserType searchModel = new UserType()
            {
                Code = usertype.Code,
                Name = usertype.Name
            };

            var totalCount = _dUserType.GetPageDataTotalCount(searchModel);
            return totalCount;
        }

        private M_UserType ConverEntityToModel(UserType usertype)
        {
            if (usertype == null) return null;

            var model = new M_UserType()
            {
                Id = usertype.Id.ToString(),
                Name = usertype.Name,
                Code = usertype.Code,
            };
            return model;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message AddUserType(M_UserType model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "数据新增成功";

            try
            {
                //新增联系人
                var usertype = new UserType()
                {
                    Code = model.Code,
                    Name = model.Name
                };

                message.ReturnId = _dUserType.AddUserType(usertype);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "数据新增失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Message EditUserType(M_UserType model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "数据修改成功";

            try
            {
                var usertype = new UserType()
                {
                    Code = model.Code,
                    Name = model.Name
                };

                message.ReturnId = _dUserType.EditUserType(System.Convert.ToInt32(model.Id), usertype);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "数据修改失败，异常：" + e.Message;
            }
            return message;
        }

        public Message DeleteById(int id)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "数据删除成功";

            try
            {
                _dUserType.DeleteUserType(id);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "数据删除失败：" + e.Message;
            }

            return message;
        }

        public Message BatchDeleteByIds(List<int> ids)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "数据删除成功";

            try
            {
                _dUserType.BatchDeleteUserType(ids);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "数据删除失败：" + e.Message;
            }

            return message;
        }
    }
}

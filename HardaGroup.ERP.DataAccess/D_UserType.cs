using HardaGroup.ERP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.DataAccess
{
    public class D_UserType
    {
        DBContext _dbContext = new DBContext();

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="usertype"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<UserType> GetPageData(UserType usertype, int skip, int limit)
        {
            var query = _dbContext.UserTypes.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(usertype.Code))
                query = query.Where(a => a.Code.Contains(usertype.Code));
            if (!string.IsNullOrEmpty(usertype.Name))
                query = query.Where(a => a.Name.Contains(usertype.Name));

            #endregion

            var result = query.OrderBy(a => new { a.Id })
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;
        }

        public List<UserType> GetAll()
        {
            var result = _dbContext.UserTypes.OrderBy(a => a.Id).ToList();
            return result;
        }

        public int GetPageDataTotalCount(UserType usertype)
        {
            var query = _dbContext.UserTypes.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(usertype.Code))
                query = query.Where(a => a.Code.Contains(usertype.Code));
            if (!string.IsNullOrEmpty(usertype.Name))
                query = query.Where(a => a.Name.Contains(usertype.Name));

            #endregion

            var result = query.ToList().Count();

            return result;
        }

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserType GetUserTypeById(int id)
        {
            var usertype = _dbContext.UserTypes.FirstOrDefault(a => a.Id == id);
            return usertype;
        }

        public UserType GetUserTypeByCode(string code)
        {
            var usertype = _dbContext.UserTypes.FirstOrDefault(a => a.Code == code);
            return usertype;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="usertype"></param>
        /// <returns></returns>
        public int AddUserType(UserType usertype)
        {
            _dbContext.UserTypes.Add(usertype);
            _dbContext.SaveChanges();
            return usertype.Id;
        }

        public int EditUserType(int id, UserType model)
        {
            var usertype = _dbContext.UserTypes.FirstOrDefault(a => a.Id == id);

            usertype.Name = model.Name;
            usertype.Code = model.Code;

            _dbContext.SaveChanges();
            return usertype.Id;
        }


        public void DeleteUserType(int id)
        {
            var usertype = _dbContext.UserTypes.Where(i => i.Id == id).FirstOrDefault();
            if (usertype != null)
            {
                _dbContext.UserTypes.Remove(usertype);
                _dbContext.SaveChanges();
            }
        }

        public void BatchDeleteUserType(List<int> ids)
        {
            var contactScopes = _dbContext.UserTypes.Where(i => ids.Contains(i.Id)).ToList();
            if (contactScopes.Count > 0)
            {
                _dbContext.UserTypes.RemoveRange(contactScopes);
                _dbContext.SaveChanges();
            }
        }
    }
}

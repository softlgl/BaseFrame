using System;

namespace BaseFrame.Entity
{
    //用户表
    public class User
    {
        public User()
        {

        }

        private int _id;
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _usercode;
        /// <summary>
        /// 用户编号
        /// </summary>	
        public string UserCode
        {
            get { return _usercode; }
            set { _usercode = value; }
        }
        private string _username;
        /// <summary>
        /// 用户姓名
        /// </summary>	
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        private string _phone;
        /// <summary>
        /// 手机号
        /// </summary>	
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        private string _loginname;
        /// <summary>
        /// 登录账号
        /// </summary>	
        public string LoginName
        {
            get { return _loginname; }
            set { _loginname = value; }
        }
        private string _loginpwd;
        /// <summary>
        /// 密码
        /// </summary>	
        public string LoginPwd
        {
            get { return _loginpwd; }
            set { _loginpwd = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司ID
        /// </summary>	
        public int CompanyId
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _usertype;
        /// <summary>
        /// 用户类型  (0:超级管理员  1:经销商  2:首选用户  3:管理员)
        /// </summary>	
        public int UserType
        {
            get { return _usertype; }
            set { _usertype = value; }
        }
        private int _isvalid;
        /// <summary>
        /// 是否启用 (0:启用  1:禁用)
        /// </summary>	
        public int IsValid
        {
            get { return _isvalid; }
            set { _isvalid = value; }
        }
        private int _createuserid;
        /// <summary>
        /// 创建人ID
        /// </summary>	
        public int CreateUserId
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime _createtime;
        /// <summary>
        /// 创建时间
        /// </summary>	
        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int _updateuserid;
        /// <summary>
        /// 修改人ID
        /// </summary>	
        public int UpdateUserId
        {
            get { return _updateuserid; }
            set { _updateuserid = value; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>		
        private DateTime _updatetime;
        public DateTime UpdateTime
        {
            get { return _updatetime; }
            set { _updatetime = value; }
        }

        /// <summary>
        /// 点代码
        /// </summary>
        public string ShopCode { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 角色Ids
        /// </summary>
        public string RoleIds { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldLoginPwd { get; set; }

        /// <summary>
        /// 经销商状态
        /// </summary>

        public int? CompanyStatus{ get; set; }
    }
}

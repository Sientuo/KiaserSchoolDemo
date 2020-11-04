﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiaserModel.ViewModel
{
    public class UserInfo
    {
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string  Password { get; set; }
        /// <summary>
        /// 是否记住
        /// </summary>
        public string IsRemember { get; set; }
    }
}

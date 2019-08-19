using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.Common
{
    public class TreeModel
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        public string id;

        /// <summary>
        /// 父节点Id
        /// </summary>
        public string pId;

        /// <summary>
        /// 节点名
        /// </summary>
        public string name;

        /// <summary>
        /// 是否父节点
        /// </summary>
        public bool isParent;

        /// <summary>
        /// 节点url
        /// </summary>
        public string url;

        /// <summary>
        /// 是否展开,默认为否
        /// </summary>
        public bool open;

        /// <summary>
        /// 是否显示checkBox/radio
        /// </summary>
        public bool nocheck;

        /// <summary>
        /// 是否选中,默认false
        /// </summary>
        public bool isChecked;

        public bool halfCheck;

        /// <summary>
        /// 是否禁用checkbox/radio
        /// </summary>
        public bool chkDisabled;

        /// <summary>
        /// 是否隐藏,默认false
        /// </summary>
        public bool isHidden;
    }
}

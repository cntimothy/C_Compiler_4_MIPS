using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIPS246.Compiler.ErrorHandling
{
    public class MccBaseException:Exception
    {
        #region Private Field
        private int stage;  //发生的阶段
        #endregion

        #region Constructor
        public MccBaseException()
        { }

        public MccBaseException(string message, int stage, Exception inner):base(message, inner)
        {
            this.stage = stage;
        }
        #endregion

        #region Virtual Method
        /// <summary>
        /// 显示Message
        /// </summary>
        /// <returns></returns>
        public virtual string ShowMessage()
        {
            string returnValue = "";
            returnValue = "阶段" + this.stage + "发生异常：" + this.Message;
            return returnValue;
        }
        #endregion
    }
}

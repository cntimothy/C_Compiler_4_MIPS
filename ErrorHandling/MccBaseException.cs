using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIPS246.Compiler.ErrorHandling
{
    public class MccBaseException:Exception
    {
        #region Constructor
        public MccBaseException()
        { }

        public MccBaseException(string message, Exception inner):base(message, inner)
        {
            
        }
        #endregion

        #region Virtual Method
        /// <summary>
        /// 显示Message
        /// </summary>
        /// <returns></returns>
        public virtual string ShowMessage()
        {
            return this.Message;
        }
        #endregion
    }
}

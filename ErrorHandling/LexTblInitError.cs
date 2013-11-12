using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIPS246.Compiler.ErrorHandling
{
    public class LexTblInitError:MccBaseException
    {
        private static string message = "发生异常：词法转换表初始化异常";

        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stage">发生异常的阶段</param>
        /// <param name="inner"></param>
        public LexTblInitError(int stage, Exception inner)
            : base(message, stage, inner)
        {
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns></returns>
        public override string ShowMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ShowMessage());
            
            return sb.ToString();
        }
        #endregion
    }
}

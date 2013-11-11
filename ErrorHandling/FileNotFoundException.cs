using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIPS246.Compiler.ErrorHandling
{
    public class FileNotFoundException:MccBaseException
    {
        private static string message = "发生异常：指定的文件不存在!";
        private string filePath;    //文件名

        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="stage">发生异常的阶段</param>
        /// <param name="inner"></param>
        public FileNotFoundException(string filePath, int stage, Exception inner)
            : base(message, stage, inner)
        {
            this.filePath = filePath;
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
            sb.AppendLine("文件名：" + filePath);
            
            return sb.ToString();
        }
        #endregion
    }
}

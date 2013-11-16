using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIPS246.Compiler.ErrorHandling
{
    public class TokenInitException:MccBaseException
    {
        private static string message = "Token初始化异常";
        private static int stage = 1;
        private string word;
        private int lineNo;

        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="c">未能识别的字符</param>
        /// <param name="inner"></param>
        public TokenInitException(string word, int lineNo, Exception inner)
            : base(message, stage, inner)
        {
            this.word = word;
            this.lineNo = lineNo;
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
            sb.AppendLine(base.ShowMessage())
                .AppendLine("行" + lineNo + "，" + this.word + "不是系统单词");

            return sb.ToString();
        }
        #endregion
    }
}

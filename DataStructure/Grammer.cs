using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIPS246.Compiler.DataStructure
{
    public class Grammer
    {
        public string Left { set; get; }
        public List<string> Right { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="GrammerStr"></param>
        public Grammer(string GrammerStr)
        {
            string[] grammerStrArray = GrammerStr.Trim().Split(':');
            this.Left = grammerStrArray[0].Trim();
            this.Right = grammerStrArray[1].Trim().Split(' ').ToList();
        }

        /// <summary>
        /// 返回字符串表示
        /// </summary>
        /// <returns></returns>
        public string Show()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Left)
                .Append(" -> ");
            foreach (string item in this.Right)
            {
                sb.Append(item + " ");
            }
            return sb.ToString();
        }
    }
}

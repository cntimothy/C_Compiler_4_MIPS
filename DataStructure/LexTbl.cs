using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIPS246.Compiler.ErrorHandling;

namespace MIPS246.Compiler.DataStructure
{
    public class LexTbl
    {
        #region Private Field
        private Dictionary<int, Dictionary<char, int>> table;
        #endregion

        #region Constructor
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LexTbl()
        {
            table = new Dictionary<int, Dictionary<char, int>>();
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="statusArray">状态数组</param>
        /// <param name="terminalCharArray">终结符数组</param>
        /// <param name="table">二维数组</param>
        public LexTbl(int[] statusArray,  int[][] table)
        {
            if (statusArray.Length == 0 || table.Length == 0 ||table[0].Length == 0)   //如果数组为空，则抛出异常
            { 
                int stage = 1;
                throw (new LexTblInitExcpetion(stage, new Exception()));
            }
            if (statusArray.Length != table.Length || SystemWord.terminalCharList.Count != table[0].Length)      //如果数组长度不正确，抛出异常
            {
                int stage = 1;
                throw (new LexTblInitExcpetion(stage, new Exception()));
            }
            
            for (int i = 0; i < statusArray.Length; i++)
            {
                Dictionary<char, int> dic = new Dictionary<char, int>();
                for (int j = 0; j < SystemWord.terminalCharList.Count; j++)
                {
                    dic.Add(SystemWord.terminalCharList[j], table[i][j]);
                }
                this.table.Add(statusArray[i], dic);
            }
        }
        #endregion

        #region PublicMethod
        /// <summary>
        /// 根据当前状态和下一个字符返回下一个状态
        /// </summary>
        /// <param name="curStatus">当前状态</param>
        /// <param name="nextChar">下一个字符</param>
        /// <returns></returns>
        public int getNextStatus(int curStatus, char nextChar)
        {
            return table[curStatus][nextChar];
        }

        /// <summary>
        /// 向table中增加一行
        /// </summary>
        /// <param name="statusArray"></param>
        public void AddNewLine(int[] statusArray)
        {
            int currentRow = this.table.Keys.Count + 1;
            Dictionary<char, int> dic = new Dictionary<char, int>();
            for(int i = 0; i != SystemWord.terminalCharList.Count; i++ )
            {
                dic.Add(SystemWord.terminalCharList[i], statusArray[i]);
            }
            this.table.Add(currentRow, dic);
        }

        /// <summary>
        /// 输出去词法分析表
        /// </summary>
        /// <returns></returns>
        public string Show()
        {
            StringBuilder sb = new StringBuilder();
            foreach (int status in table.Keys)
            {
                foreach (char c in table[status].Keys)
                {
                    sb.Append(table[status][c] + " ");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
        #endregion
    }
}

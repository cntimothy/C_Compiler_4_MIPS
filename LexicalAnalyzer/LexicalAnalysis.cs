using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIPS246.Compiler.DataStructure;

namespace MIPS246.Compiler.LexicalAnalyzer
{
    public class LexicalAnalysis
    {
        #region Public Method
        /// <summary>
        /// 对输入的单词流进行词法分析
        /// </summary>
        /// <param name="contentList">单词流</param>
        /// <param name="lexTbl">词法分析表</param>
        /// <param name="keyWordTbl">关键字表</param>
        /// <param name="showResult">是否输出阶段结果</param>
        /// <returns></returns>
        public static List<Token> Analysis(List<string> contentList, LexTbl lexTbl, KeyWordTbl keyWordTbl, bool showResult)
        {
            List<Token> tokenList = new List<Token>();
            Console.WriteLine("进行词法分析");
            return tokenList;
        }
        #endregion

        #region Private Method
        #endregion
    }
}

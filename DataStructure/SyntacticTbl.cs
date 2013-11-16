using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIPS246.Compiler.DataStructure
{
    public class SyntacticTbl
    {
        #region Private Field
        private Dictionary<string, Dictionary<string, Grammar>> table;
        #endregion

        #region Constructor
        public SyntacticTbl(Dictionary<Grammar, List<string>> firstDicForGrammar, Dictionary<string, List<string>> followDic, List<string> unterminatorList)
        {
            table = new Dictionary<string, Dictionary<string, Grammar>>();
            foreach (string unterminator in unterminatorList)
            {
                Dictionary<string, Grammar> row = new Dictionary<string, Grammar>();
                foreach (string terminator in SystemWord.TerminatorList)
                {
                    row.Add(terminator, findGrammar(firstDicForGrammar, followDic, terminator, unterminator));
                }
                table.Add(unterminator, row);
            }
        }
        #endregion

        #region Public Method
        #endregion

        #region Private Method
        /// <summary>
        /// 根据终结符查找合适的文法产生式
        /// </summary>
        /// <param name="firstDicForGrammar">文法的first集</param>
        /// <param name="followDic">follow集</param>
        /// <param name="terminator">终结符</param>
        /// <param name="unterminator">非终结符</param>
        /// <returns></returns>
        private Grammar findGrammar(Dictionary<Grammar, List<string>> firstDicForGrammar, Dictionary<string, List<string>> followDic, string terminator, string unterminator)
        {
            Grammar returnGrammar = null;
            bool flag = true;   //指示是否需要在follow集中寻找
            foreach (KeyValuePair<Grammar, List<string>> pair in firstDicForGrammar)
            {
                if (pair.Key.Left == unterminator)
                {
                    if (pair.Value.Contains(terminator))
                    {
                        returnGrammar = pair.Key;
                        flag = false;
                        break;
                    }
                }
            }
            if (flag)
            {
                if (followDic[unterminator].Contains(terminator))
                {
                    returnGrammar = new Grammar(unterminator + ": EMPTY");
                }
            }
            
            return returnGrammar;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIPS246.Compiler.DataStructure;
using MIPS246.Compiler.ErrorHandling;
using System.IO;

namespace MIPS246.Compiler.SyntacticAnalyzer
{
    public class SyntacticAnalysis
    {
        #region Public Method
        public static void Analysis(ref AST ast, List<Token> tokenList, bool showStageResult)
        {
            List<Grammar> grammarList = getGrammer();   //获取文法
            //获取终结符List
            List<string> unTerminatorList = new List<string>();
            foreach (Grammar item in grammarList)
            {
                if (!unTerminatorList.Contains(item.Left))
                {
                    unTerminatorList.Add(item.Left);
                }
            }

            //获取可空的非终结符List
            List<string> nullableUnTerminatorList = getNullableUnTerminatorList(grammarList);
            foreach (string item in nullableUnTerminatorList)
            {
                Console.Write(item + " ");
            }      
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 从文件中读取文法
        /// </summary>
        /// <returns></returns>
        private static List<Grammar> getGrammer()
        {
            string filePath = "grammar.inf";
            List<Grammar> grammerList = new List<Grammar>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("gb2312")))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line == "")
                        {
                            continue;
                        }
                        Grammar grammer = new Grammar(line);
                        grammerList.Add(grammer);
                    }
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                int stage = 1;
                throw (new MIPS246.Compiler.ErrorHandling.FileNotFoundException(filePath, stage, e));
            }
            return grammerList;
        }

        /// <summary>
        /// 获取可空终结符List
        /// </summary>
        /// <param name="grammarList"></param>
        /// <returns></returns>
        private static List<string> getNullableUnTerminatorList(List<Grammar> grammarList)
        {
            List<string> nullableUnterminatorList = new List<string>();
            foreach (Grammar item in grammarList)
            {
                if (item.Right.Count == 1 && item.Right[0] == "EMPTY")
                {
                    nullableUnterminatorList.Add(item.Left);
                }
            }
            bool flag = true;
            while (flag)
            {
                int i = 0;
                for (; i != grammarList.Count; i++)
                { 
                    if(isSubList(grammarList[i].Right, nullableUnterminatorList))
                    {
                        nullableUnterminatorList.Add(grammarList[i].Left);
                        break;
                    }
                }
                if (i == grammarList.Count)
                {
                    flag = false;
                }
                
            }
            
            return nullableUnterminatorList;
        }

        /// <summary>
        /// 检测第一个List是不是第二个List的子List，是则返回true，否则返回false
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        private static bool isSubList(List<string> list1, List<string> list2)
        {
            bool flag = true;
            foreach (string item in list1)
            {
                if (!list2.Contains(item))
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        
        #endregion
    }
}

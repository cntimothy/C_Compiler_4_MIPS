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
            List<string> unterminatorList = new List<string>();
            foreach (Grammar item in grammarList)
            {
                if (!unterminatorList.Contains(item.Left))  //避免重复添加
                {
                    unterminatorList.Add(item.Left);
                }
            }
            

            //获取可空的非终结符List
            List<string> nullableUnterminatorList = getNullableUnTerminatorList(grammarList);            

            //获取First字典
            Dictionary<string, List<string>> firstDic = getFirstDic(grammarList, nullableUnterminatorList);

            //获取Follow字典
            Dictionary<string, List<string>> followDic = getFollowDic(grammarList, nullableUnterminatorList);
           
            //输出结果
            if (showStageResult)
            {
                showResult(grammarList, unterminatorList, nullableUnterminatorList, firstDic, followDic);
                
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
            List<Grammar> grammarList = new List<Grammar>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("gb2312")))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)  //循环读每一行
                    {
                        line = line.Trim();
                        if (line == "") //跳过空行
                        {
                            continue;
                        }
                        Grammar grammer = new Grammar(line);
                        grammarList.Add(grammer);
                    }
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                int stage = 1;
                throw (new MIPS246.Compiler.ErrorHandling.FileNotFoundException(filePath, stage, e));
            }
            return grammarList;
        }

        /// <summary>
        /// 获取可空终结符List
        /// </summary>
        /// <param name="grammarList"></param>
        /// <returns></returns>
        private static List<string> getNullableUnTerminatorList(List<Grammar> grammarList)
        {
            List<string> nullableUnterminatorList = new List<string>();
            foreach (Grammar item in grammarList)   //先读取直接可空非终结符
            {
                if (item.Right.Count == 1 && item.Right[0] == "EMPTY")
                {
                    nullableUnterminatorList.Add(item.Left);
                }
            }
            bool flag = true;   //指示是否还有新增的可空非终结符
            while (flag)
            {
                flag = false;
                for (int i = 0; i != grammarList.Count; i++)
                { 
                    if(isSubList(grammarList[i].Right, nullableUnterminatorList) 
                        && !nullableUnterminatorList.Contains(grammarList[i].Left))   //如果该非终结符可空，且尚未加入到可空非终结符集合中
                    {
                        nullableUnterminatorList.Add(grammarList[i].Left);
                        flag = true;
                    }
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

        /// <summary>
        /// 生成first集合
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, List<string>> getFirstDic(List<Grammar> grammarList, List<string> nullableUnterminatorList)
        {
            Dictionary<string, List<string>> firstDic = new Dictionary<string, List<string>>();
            foreach (Grammar grammar in grammarList)
            {
                if (!firstDic.Keys.Contains(grammar.Left))  //如果非终结符是第一次遇到，在字典中增加一条记录
                {
                    firstDic.Add(grammar.Left, new List<string>());
                }

                if (grammar.Right.Count == 1 & grammar.Right[0] == "EMPTY")  //如果是文法的右边是EMPTY，则跳过
                {
                    continue;
                }

                else if (SystemWord.TerminatorList.Contains(grammar.Right[0]))   //如果文法右边的第一个字符是终结符，则加入到非终结符的First集中
                {
                    if (!firstDic[grammar.Left].Contains(grammar.Right[0])) //避免重复添加
                    {
                        firstDic[grammar.Left].Add(grammar.Right[0]);
                    }
                }
                else
                {
                    firstDic[grammar.Left].Add(grammar.Right[0].ToUpper());  //如果不是，则将右边第一个非终结符的First集加到非终结符的First集中
                    for (int i = 0; i != (grammar.Right.Count - 1); i++)
                    {
                        if (nullableUnterminatorList.Contains(grammar.Right[i]))
                        {
                            if (SystemWord.TerminatorList.Contains(grammar.Right[i + 1]))
                            {
                                firstDic[grammar.Left].Add(grammar.Right[i + 1]);
                            }
                            else
                            {
                                firstDic[grammar.Left].Add(grammar.Right[i + 1].ToUpper());
                            }
                        }
                    }
                }
            }

            List<string> finishedList = new List<string>();
            foreach (string unterminator in firstDic.Keys)
            { 
                if(isSubList(firstDic[unterminator], SystemWord.TerminatorList))
                {
                    finishedList.Add(unterminator);
                }
            }

            bool flag = true;
            while (flag)
            {
                flag = false;
                Dictionary<string, List<string>> newFirstDic = new Dictionary<string,List<string>>();   //构造新的临时first集合
                foreach (KeyValuePair<string, List<string>> pair in firstDic)
                {
                    if (isSubList(pair.Value, SystemWord.TerminatorList))   //如果是已经完成构建first集合的非终结符，则直接添加
                    {
                        newFirstDic.Add(pair.Key, pair.Value);
                        continue;
                    }
                    else //否则就要新建一个临时值
                    {
                        List<string> newValue = new List<string>();
                        foreach (string item in pair.Value)
                        {
                            if (finishedList.Contains(item.ToLower()))  //替换掉已经完成构建first集合的非终结符占位符
                            {
                                newValue.AddRange(firstDic[item.ToLower()]);
                                newValue = newValue.Distinct().ToList();    //去掉重复元素
                            }
                            else
                            {
                                newValue.Add(item);
                            }
                        }
                        newFirstDic.Add(pair.Key, newValue);
                        if (isSubList(newValue, SystemWord.TerminatorList))
                        {
                            finishedList.Add(pair.Key); //有新增的完成构建first集合的非终结符，则继续循环，直到没有新增
                            flag = true;
                        }
                    }
                }
                firstDic = newFirstDic;
            }
            
            return firstDic;
        }
        
        /// <summary>
        /// 生成follow集合
        /// </summary>
        /// <param name="grammarList"></param>
        /// <param name="nullableUnterminatorList"></param>
        /// <returns></returns>
        private static Dictionary<string, List<string>> getFollowDic(List<Grammar> grammarList, List<string> nullableUnterminatorList)
        {
            Dictionary<string, List<string>> followDic = new Dictionary<string, List<string>>();

            return followDic;
        }

        /// <summary>
        /// 输出结果
        /// </summary>
        /// <param name="grammarList">文法</param>
        /// <param name="unterminatorList">非终结符集合</param>
        /// <param name="nullableUnterminatorList">可空非终结符结合</param>
        /// <param name="firstDic">first集合</param>
        /// <param name="followDic">follow集合</param>
        private static void showResult(List<Grammar> grammarList, List<string> unterminatorList, List<string> nullableUnterminatorList, Dictionary<string, List<string>> firstDic, Dictionary<string, List<string>> followDic)
        {
            Console.WriteLine();
            Console.WriteLine("扫描到的文法：");
            foreach (Grammar grammar in grammarList)
            {
                Console.WriteLine(grammar.Show());
            }

            Console.WriteLine();
            Console.WriteLine("非终结符结合：");
            foreach (string item in unterminatorList)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("可空非终结符集合：");
            foreach (string item in nullableUnterminatorList)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("First字典：");
            foreach (string item in firstDic.Keys)
            {
                Console.Write(item + ":");
                foreach (string first in firstDic[item])
                {
                    Console.Write(first + " ");
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}

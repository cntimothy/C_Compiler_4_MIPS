using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIPS246.Compiler.DataStructure;
using MIPS246.Compiler.ErrorHandling;
using System.IO;
using System.Diagnostics;

namespace MIPS246.Compiler.SyntacticAnalyzer
{
    public class SyntacticAnalysis
    {
        #region Public Method
        public static void Analysis(ref AST ast, List<Token> tokenList, bool showStageResult)
        {
            List<Grammar> grammarList = getGrammer();   //获取语法

            //获取非终结符List
            List<string> unterminatorList = getUntermiatorList(grammarList); 

            //获取可空的非终结符List
            List<string> nullableUnterminatorList = getNullableUnTerminatorList(grammarList, unterminatorList);

            //获取First字典
            Dictionary<string, List<string>> firstDicForUnterminator = getFirstDicForUnterminator(grammarList, nullableUnterminatorList);  //非终结符
            Dictionary<Grammar, List<string>> firstDicForGrammar = getFirstDicForGrammar(grammarList, nullableUnterminatorList, firstDicForUnterminator);     //语法

            //获取Follow字典
            //Dictionary<string, List<string>> followDic = getFollowDic(grammarList, unterminatorList, nullableUnterminatorList, firstDicForUnterminator);

            //根据first字典和follow字典获取语法分析表
            //SyntacticTbl syntacticTbl = new SyntacticTbl(firstDicForGrammar, followDic, unterminatorList);

            //输出结果
            if (showStageResult)
            {
                //showResult(grammarList, unterminatorList, nullableUnterminatorList, firstDicForUnterminator, firstDicForGrammar, followDic);
                Console.WriteLine();
                Console.WriteLine("可空非终结符集合：");
                foreach (string item in nullableUnterminatorList)
                {
                    Console.Write(item + " ");
                }

                Console.WriteLine();
                Console.WriteLine("非终结符的First字典：");
                foreach (string item in firstDicForUnterminator.Keys)
                {
                    Console.Write(item + ":");
                    foreach (string first in firstDicForUnterminator[item])
                    {
                        Console.Write(first + " ");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("语法的First字典：");
                foreach (Grammar item in firstDicForGrammar.Keys)
                {
                    Console.Write("( " + item.Show() + " ):");
                    foreach (string first in firstDicForGrammar[item])
                    {
                        Console.Write(first + " ");
                    }
                    Console.WriteLine();
                }
            }

            //主处理
            try
            {
                //process(ast, tokenList, syntacticTbl);
            }
            catch (MccBaseException e)
            {
                throw e;
            }

            
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 获取非终结符集合
        /// </summary>
        /// <param name="grammarList"></param>
        /// <returns></returns>
        private static List<string> getUntermiatorList(List<Grammar> grammarList)
        {
            List<string> unterminatorList = new List<string>();
            foreach (Grammar item in grammarList)
            {
                if (!unterminatorList.Contains(item.Left))  //避免重复添加
                {
                    unterminatorList.Add(item.Left);
                }
            }
            return unterminatorList;
        }

        /// <summary>
        /// 从文件中读取语法
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
        private static List<string> getNullableUnTerminatorList(List<Grammar> grammarList, List<string> unterminatorList)
        {
            List<string> nullableUnterminatorList = new List<string>();
            bool flag = false; //指示在循环中是否有变化
            do
            {
                flag = false;
                foreach (Grammar grammar in grammarList)
                {
                    if (isNullable(grammar, nullableUnterminatorList) && (!nullableUnterminatorList.Contains(grammar.Left)))
                    {
                        nullableUnterminatorList.Add(grammar.Left);
                        flag = true;
                    }
                }
            } while (flag);

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
        /// 生成非终结符first集合
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, List<string>> getFirstDicForUnterminator(List<Grammar> grammarList, List<string> nullableUnterminatorList)
        {
            Dictionary<string, List<string>> firstDicForUnterminator = new Dictionary<string, List<string>>();
            //初始化first集
            foreach (Grammar grammar in grammarList)
            {
                if (!firstDicForUnterminator.Keys.Contains(grammar.Left))
                {
                    firstDicForUnterminator.Add(grammar.Left, new List<string>());
                }
            }

            bool flag = true;   //指示在一次循环中是first集是否有变化
            do
            {
                flag = false;
                foreach (Grammar grammar in grammarList)
                {
                    if (isNullGrammar(grammar)) //如果是空语法则跳过
                    {
                        continue;
                    }
                    flag = flag || addToFirstDicForUnterminator(grammar.Right[0], grammar.Left, firstDicForUnterminator);
                    for (int i = 0; i < grammar.Right.Count - 1; i++)
                    {
                        if (isNullable(grammar.Right[i], nullableUnterminatorList))
                        {
                            flag = flag || addToFirstDicForUnterminator(grammar.Right[i + 1], grammar.Left, firstDicForUnterminator);
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            while (flag);
            return firstDicForUnterminator;
        }

        /// <summary>
        /// 生成语法产生式的first集合
        /// </summary>
        /// <param name="grammarList"></param>
        /// <param name="nullableUnterminatorList"></param>
        /// <param name="firstDicForUnterminator"></param>
        /// <returns></returns>
        private static Dictionary<Grammar, List<string>> getFirstDicForGrammar(List<Grammar> grammarList, List<string> nullableUnterminatorList, Dictionary<string, List<string>> firstDicForUnterminator)
        {
            Dictionary<Grammar, List<string>> firstDicForGrammar = new Dictionary<Grammar, List<string>>();
            foreach (Grammar grammar in grammarList)
            {
                if (!firstDicForGrammar.Keys.Contains(grammar)) //如果是第一次遇到的语法产生式，则像first集中添加一条记录
                {
                    firstDicForGrammar.Add(grammar, new List<string>());
                }
                if (isNullGrammar(grammar)) //如果是空语法则跳过
                {
                    continue;
                }
                addToFirstDicForGrammar(grammar.Right[0], grammar, firstDicForGrammar, firstDicForUnterminator);
                for (int i = 0; i < grammar.Right.Count - 1; i++)
                {
                    if (isNullable(grammar.Right[i], nullableUnterminatorList))
                    {
                        addToFirstDicForGrammar(grammar.Right[i+1], grammar, firstDicForGrammar, firstDicForUnterminator);
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            return firstDicForGrammar;
        }
        /// <summary>
        /// 检测元素是否为可空
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nullableUnterminatorList"></param>
        /// <returns></returns>
        private static bool isNullable(string item, List<string> nullableUnterminatorList)
        {
            bool returnValue = true;
            if (isTerminator(item) || !nullableUnterminatorList.Contains(item))
            {
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// 将元素添加进指定非终结符的first集，并返回是否进行过修改
        /// </summary>
        /// <param name="factor">语法右部的某个元素</param>
        /// <param name="unterminator">语法左部的非终结符</param>
        /// <param name="firstDicForUnterminator">非终结符first集</param>
        private static bool addToFirstDicForUnterminator(string factor, string unterminator, Dictionary<string, List<string>> firstDicForUnterminator)
        {
            bool flag = false; //指示是否进行过修改
            if (isTerminator(factor))    //如果是新增终结符，则将其添加到字典中
            {
                if (!firstDicForUnterminator[unterminator].Contains(factor))
                {
                    firstDicForUnterminator[unterminator].Add(factor);
                    flag = true;
                }
            }
            else    //如果是非终结符，则将其first集增加到指定非终结符的first集中
            {
                foreach (string item in firstDicForUnterminator[factor])
                {
                    if (!firstDicForUnterminator[unterminator].Contains(item))
                    {
                        firstDicForUnterminator[unterminator].Add(item);
                        flag = true;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 将元素添加进指定语法产生式的first集
        /// </summary>
        /// <param name="factor">元素</param>
        /// <param name="grammar">指定语法产生式</param>
        /// <param name="firstDicForGrammar">语法产生式的first集</param>
        /// <param name="firstDicForUnterminator">非终结符的first集</param>
        private static void addToFirstDicForGrammar(string factor, Grammar grammar, Dictionary<Grammar, List<string>> firstDicForGrammar, Dictionary<string, List<string>> firstDicForUnterminator)
        {
            if (isTerminator(factor))    //如果是新增终结符，则将其添加到字典中
            {
                if (!firstDicForGrammar[grammar].Contains(factor))
                {
                    firstDicForGrammar[grammar].Add(factor);                }
            }

            else    //如果是非终结符，则将其first集增加到指定语法产生式的first集中
            {
                foreach (string item in firstDicForUnterminator[factor])
                {
                    if (!firstDicForGrammar[grammar].Contains(item))
                    {
                        firstDicForGrammar[grammar].Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 生成follow集合
        /// </summary>
        /// <param name="grammarList"></param>
        /// <param name="nullableUnterminatorList"></param>
        /// <returns></returns>
        private static Dictionary<string, List<string>> getFollowDic(List<Grammar> grammarList, List<string> unterminatorList, List<string> nullableUnterminatorList, Dictionary<string, List<string>> firstDic)
        {
            Dictionary<string, List<string>> followDic = new Dictionary<string, List<string>>();
            

            return followDic;
        }

        /// <summary>
        /// 输出结果
        /// </summary>
        /// <param name="grammarList">语法</param>
        /// <param name="unterminatorList">非终结符集合</param>
        /// <param name="nullableUnterminatorList">可空非终结符结合</param>
        /// <param name="firstDic">first集合</param>
        /// <param name="followDic">follow集合</param>
        private static void showResult(List<Grammar> grammarList, List<string> unterminatorList, List<string> nullableUnterminatorList, Dictionary<string, List<string>> firstDicForUnterminator, Dictionary<Grammar, List<string>> firstDicForGrammar, Dictionary<string, List<string>> followDic)
        {
            Console.WriteLine();
            Console.WriteLine("扫描到的语法：");
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
            Console.WriteLine("非终结符的First字典：");
            foreach (string item in firstDicForUnterminator.Keys)
            {
                Console.Write(item + ":");
                foreach (string first in firstDicForUnterminator[item])
                {
                    Console.Write(first + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("语法的First字典：");
            foreach (Grammar item in firstDicForGrammar.Keys)
            {
                Console.Write("( " + item.Show() + " ):");
                foreach (string first in firstDicForGrammar[item])
                {
                    Console.Write(first + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Follow字典：");
            foreach (string item in followDic.Keys)
            {
                Console.Write(item + ":");
                foreach (string follow in followDic[item])
                {
                    Console.Write(follow + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 确定字符串是否是终结符，是则返回true，否则返回false
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool isTerminator(string str)
        {
            return SystemWord.TerminatorList.Contains(str);
        }

        /// <summary>
        /// 主处理程序
        /// </summary>
        /// <param name="ast"></param>
        /// <param name="tokenList"></param>
        /// <param name="syntacticTbl"></param>
        private static void process(AST ast, List<Token> tokenList, SyntacticTbl syntacticTbl)
        {
            Stack<string> analysisStack = new Stack<string>();  //分析栈
            analysisStack.Push("$");    //语法结束符号入栈
            tokenList.Add(new SystemWordToken("$", 0));   //向token串的尾部添加语法结束符号
            analysisStack.Push("program");
            int i = 0; //输入缓冲区指向当前符号的指针
            Token curToken = tokenList[i];
            string stackPeak;

            while ((analysisStack.Count != 0) && ( i != tokenList.Count))
            {
                curToken = tokenList[i];
                stackPeak = analysisStack.Pop();
                if (isTerminator(stackPeak))
                {
                    if (stackPeak == curToken.Name)
                    {
                        i++;
                    }
                    else
                    { 
                        throw(new SyntacticAnalysisException(curToken.Name, curToken.LineNo, new Exception()));
                    }
                }
                else
                {
                    if (stackPeak == "$")
                    {
                        if (stackPeak == curToken.Name)
                        {
                            break;
                        }
                        else
                        {
                            throw (new SyntacticAnalysisException(curToken.Name, curToken.LineNo, new Exception()));
                        }
                    }
                    else
                    {
                        Grammar grammar = syntacticTbl.getGrammar(stackPeak, curToken.Name);
                        if (grammar != null)
                        {
                            if (!(grammar.Right.Count == 1 && grammar.Right[0] == "EMPTY"))
                            {
                                for (int j = grammar.Right.Count - 1; j >= 0; j--)
                                {
                                    analysisStack.Push(grammar.Right[j]);
                                }
                            }
                            //i++;
                        }
                        else
                        {
                            throw (new SyntacticAnalysisException(curToken.Name, curToken.LineNo, new Exception()));
                        }
                    }
                }
            }
            if ((analysisStack.Count != 0) || (i != tokenList.Count))
            {
                throw (new SyntacticAnalysisException(curToken.Name, curToken.LineNo, new Exception()));
            }
        }

        /// <summary>
        /// 检测制定list的子list是否都是可为空的，都可为空返回true，否则返回false
        /// </summary>
        /// <param name="list">制定list</param>
        /// <param name="p">起</param>
        /// <param name="p_2">止</param>
        /// <param name="nullableUnterminatorList">可空非终结符list</param>
        /// <returns></returns>
        private static bool isNullable(List<string> list, int start, int stop, List<string> nullableUnterminatorList)
        {
            bool returnValue = true;
            for (int i = start; i <= stop; i++)
            {
                if (!nullableUnterminatorList.Contains(list[i]))
                {
                    returnValue = false;
                    break;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 检测指定的list中的所有元素是不是都可为空
        /// </summary>
        /// <param name="list"></param>
        /// <param name="nullableUnterminatorList"></param>
        /// <returns></returns>
        private static bool isNullable(List<string> list, List<string> nullableUnterminatorList)
        {
            bool returnValue = true;
            foreach (string item in list)
            {
                if (isTerminator(item) || !nullableUnterminatorList.Contains(item) ) //如果是终结符或者不在可空列表里，则返回false
                {
                    returnValue = false;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 检测语法的左部非终结符是否可以为空，可以则返回true，否则返回false
        /// </summary>
        /// <param name="grammar"></param>
        /// <param name="nullableUnterminatorList"></param>
        /// <returns></returns>
        private static bool isNullable(Grammar grammar, List<string> nullableUnterminatorList)
        {
            bool returnValue = false;
            if (grammar.Right.Count == 1 && grammar.Right[0] == "EMPTY")    //右部为空的语法
                returnValue = true;
            else if(isSubList(grammar.Right, nullableUnterminatorList))
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// 检测语法是不是空语法，是则返回true，否则返回false
        /// </summary>
        /// <param name="grammar"></param>
        /// <returns></returns>
        private static bool isNullGrammar(Grammar grammar)
        {
            bool returnValue = false;
            if (grammar.Right.Count == 1 && grammar.Right[0] == "EMPTY")
            {
                returnValue = true;
            }
            return returnValue;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIPS246.Compiler.DataStructure;
using System.IO;
using MIPS246.Compiler.ErrorHandling;

namespace MIPS246.Compiler.LexicalAnalyzer
{
    public class LexicalAnalysis
    {
        #region Public Method
        /// <summary>
        /// 对输入的单词流进行词法分析
        /// </summary>
        /// <param name="filePath">指定的文件路径</param>
        /// <param name="showResult">是否输出阶段结果</param>
        /// <returns></returns>
        public static void Analysis(ref List<Token> tokenList, string filePath, bool showResult)
        {

            List<string> contentList;   //包含源文件每一行的List
            LexTbl lexTbl;              //词法分析表

            try
            {
                contentList = getContentListFromSourceFile(filePath);   //读源文件
                lexTbl = getLexTbl();                     //读词法转换表配置文件
                tokenList = genTokenList(contentList, lexTbl);          //生成Token串
            }
            catch (MccBaseException e)
            {
                throw (e);
            }

            if (showResult) //如果要求显示阶段结果
            {
                showTokenList(tokenList);
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 将指定源文件读到List中
        /// </summary>
        /// <param name="filePath">源文件路径</param>
        /// <returns></returns>
        private static List<string> getContentListFromSourceFile(string filePath)
        {
            List<string> contentList = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("gb2312")))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim() + " "; //在每行的末尾加上一个空格，方便超前搜索
                        contentList.Add(line);
                    }
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                int stage = 1;
                throw (new MIPS246.Compiler.ErrorHandling.FileNotFoundException(filePath, stage, e));
            }
            return contentList;
        }

        /// <summary>
        /// 获取词法分析表
        /// </summary>
        /// <returns></returns>
        private static LexTbl getLexTbl()
        {
            string filePath = "lextbl.inf";//词法分析表配置文件路径

            LexTbl lexTbl = new LexTbl();
            try
            {
                using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("gb2312")))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        string[] statusStr = line.Split('\t');
                        int[] statusInt = new int[statusStr.Length];
                        for (int i = 0; i != statusStr.Length; i++)
                        {
                            statusInt[i] = Convert.ToInt32(statusStr[i]);
                        }
                        lexTbl.AddNewLine(statusInt);
                    }
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                int stage = 1;
                throw (new MIPS246.Compiler.ErrorHandling.FileNotFoundException(filePath, stage, e));
            }

            return lexTbl;
        }

        /// <summary>
        /// 生成token串
        /// </summary>
        /// <param name="contentList">源代码</param>
        /// <param name="lexTbl">词法转换表</param>
        /// <returns></returns>
        private static List<Token> genTokenList(List<string> contentList, LexTbl lexTbl)
        {
            List<Token> tokenList = new List<Token>();

            int lineNo = 1;
            foreach (string line in contentList)
            {
                if ((line == "") || line.StartsWith("#")) //忽略空行和#行
                {
                    continue;
                }
                else
                {
                    char[] lineCharArray = line.ToCharArray();
                    int nextPos = 0;    //指向源代码中下一个待处理的字符
                    int curStatus = 1;  //当前状态
                    int nextStatus = 0; //根据当前状态和下一个待处理字符得出的下一个状态
                    string buffer = ""; //存储识别的单词
                    while (nextPos != lineCharArray.Length)
                    {
                        if (!SystemWord.AcceptableCharList.Contains(lineCharArray[nextPos])) //遇到不能识别的字符
                        {
                            throw (new UnknowCharExcpetion(lineCharArray[nextPos], lineNo, new Exception()));
                        }
                        nextStatus = lexTbl.getNextStatus(curStatus, lineCharArray[nextPos]);
                        if (nextStatus == 1)    //空格
                        {
                            nextPos++;
                        }
                        else if (nextStatus < 0)    //识别了一个单词
                        {
                            processWord(buffer, lineNo, nextStatus, tokenList);
                            curStatus = 1;
                            buffer = "";
                        }
                        else if (nextStatus > 1) //读入下一个字符
                        {
                            buffer += lineCharArray[nextPos].ToString();
                            curStatus = nextStatus;
                            nextPos++;
                        }
                        else        //未能识别的单词
                        {
                            buffer += lineCharArray[nextPos].ToString();
                            throw (new UnknowWordExcpetion(buffer, lineNo, new Exception()));
                        }
                    }
                }
                lineNo++;
            }
            return tokenList;
        }

        /// <summary>
        /// 处理识别的单词
        /// </summary>
        /// <param name="buffer">识别的单词</param>
        /// <param name="lineNo">行号</param>
        /// <param name="tokenList">Token串</param>
        private static void processWord(string buffer, int lineNo, int nextStatus, List<Token> tokenList)
        {
            if (nextStatus == -1)   //标识符或者关键字
            {
                if (SystemWord.KeyWordList.Contains(buffer))
                {
                    tokenList.Add(new SystemWordToken(buffer, lineNo));
                }
                else
                {
                    tokenList.Add(new IDToken(buffer, lineNo));
                }
            }
            else if (nextStatus == -2)  //常量
            {
                tokenList.Add(new ConstantToken(Convert.ToInt32(buffer), lineNo));
            }
            else if (nextStatus == -6)  //注释
            {

            }
            else    //系统单词
            {
                tokenList.Add(new SystemWordToken(buffer, lineNo));
            }
        }

        /// <summary>
        /// 输出单词流
        /// </summary>
        /// <param name="tokenList"></param>
        private static void showTokenList(List<Token> tokenList)
        {
            Console.WriteLine("单词流如下：");
            for (int i = 0; i < tokenList.Count; i++)
            {
                Console.WriteLine(tokenList[i].Show());
            }
        }
        #endregion
    }
}

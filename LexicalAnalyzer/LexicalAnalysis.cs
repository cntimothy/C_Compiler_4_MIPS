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
        public static List<Token> Analysis(string filePath, bool showResult)
        {
            List<Token> tokenList = new List<Token>();  //分析得到的token流
            List<string> contentList;   //包含源文件每一行的List

            //读源文件
            try
            {
                contentList = getContentListFromSourceFile(filePath);
            }
            catch (MccBaseException e)
            {
                throw (e);
            }

            return tokenList;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 将指定源文件去掉空行和#行读到List中
        /// </summary>
        /// <param name="filePath"></param>
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
                        line = line.Trim();
                        if (line.StartsWith("#"))
                        {
                            continue;
                        }
                        if (line == "")
                        {
                            continue;
                        }
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
        /// 获取关键字表
        /// </summary>
        /// <returns></returns>
        private static KeyWordTbl getKeyWordTbl()
        {
            KeyWordTbl keyWordTbl = new KeyWordTbl();
            return keyWordTbl;
        }

        /// <summary>
        /// 获取词法分析表
        /// </summary>
        /// <returns></returns>
        private static LexTbl getLexTbl()
        {
            LexTbl lexTbl = new LexTbl();
            return lexTbl;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MIPS246.Compiler.DataStructure;
using MIPS246.Compiler.ErrorHandling;

namespace MIPS246.Compiler.Mcc
{
    class Program
    {
        static void Main(string[] args)
        {
            //检查参数
            if (!checkParams(args))
            {
                return;
            }

            //获取文件路径
            string filePath = getFilePath(args);

            //获取进行到哪一阶段
            int stage = getStage(args);

            //获取是否输出阶段结果
            bool showStageResult = isShowStageResult(args);

            //进行词法分析
            List<Token> tokenList;
            if (stage >= 1)
            {
                try
                {
                    tokenList = LexicalAnalyzer.LexicalAnalysis.Analysis(filePath, showStageResult);
                }
                catch (MccBaseException e)
                {
                    Console.WriteLine(e.ShowMessage());
                    return;
                }
            }

            Console.ReadKey();
        }

        #region Private Method
        /// <summary>
        /// 根据参数集合生成帮助文件
        /// </summary>
        /// <param name="paramDic"></param>
        /// <returns></returns>
        private static string getHelpStr()
        {
            Dictionary<string, string> paramDic = getParamDic();
            StringBuilder sb = new StringBuilder();
            sb.Append("用法： Mcc 文件名 -阶段 [-sr]" + "\n")
                .Append("其中“-阶段”包括：" + "\n");
            foreach (string key in paramDic.Keys)
            {
                sb.Append("\t" + key + "\t\t" + paramDic[key] + "\n");
            }
            sb.Append("其中“-st”表示是否输出阶段结果");
            return sb.ToString();
        }

        /// <summary>
        /// 生成参数集合
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> getParamDic()
        {
            Dictionary<string, string> paramDic = new Dictionary<string, string>();
            paramDic.Add("-1", "进行到词法分析");
            paramDic.Add("-2", "进行到语法分析");
            paramDic.Add("-3", "进行到语义分析");
            paramDic.Add("-4", "进行到中间代码生成");
            paramDic.Add("-5", "进行到汇编代码生成分析");
            return paramDic;
        }

        /// <summary>
        /// 检查输入的参数是否合法，合法返回true，否则返回false，并向控制台输出相关信息
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool checkParams(string[] args)
        {
            bool returnValue = true;
            string helpStr = getHelpStr();
            int paramCount = args.Length;
            if (paramCount == 0)    //未输入参数
            {
                Console.WriteLine(helpStr);
                Console.WriteLine("未输入文件名！");
                returnValue = false;
            }
            else if (paramCount == 2)
            {
                List<string> paramDicKeys = getParamDic().Keys.ToList();
                if (!paramDicKeys.Contains(args[1]))
                {
                    Console.WriteLine("未识别参数：" + args[1]);
                    Console.WriteLine(helpStr);
                    returnValue = false;
                }
            }
            else if (paramCount == 3)
            {
                List<string> paramDicKeys = getParamDic().Keys.ToList();
                if (!paramDicKeys.Contains(args[1]))
                {
                    Console.WriteLine("未识别参数：" + args[1]);
                    Console.WriteLine(helpStr);
                    returnValue = false;
                }
                if (args[2] != "-sr")
                {
                    Console.WriteLine("未识别参数：" + args[1]);
                    Console.WriteLine(helpStr);
                    returnValue = false;
                }
            }
            else if (paramCount > 3)
            {
                Console.WriteLine("参数错误");
                Console.WriteLine(helpStr);
                returnValue = false;
            }
            return returnValue;
        }

        /// <summary>
        /// 从参数中读取进行到哪一步
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static int getStage(string[] args)
        {
            int stage = 5;
            if (args.Length != 1)
            {
                stage = Convert.ToInt32(args[1].ElementAt(1).ToString());
            }
            return stage;
        }

        /// <summary>
        /// 从参数中获取文件路径
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static string getFilePath(string[] args)
        {
            return args[0];
        }

        /// <summary>
        /// 从参数中获取是否输出阶段结果
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool isShowStageResult(string[] args)
        {
            bool returnValue = false;
            if (args.Length == 3 && args[2] == "-sr")
            {
                returnValue = true;
            }
            return returnValue;
        }
        #endregion
    }
}

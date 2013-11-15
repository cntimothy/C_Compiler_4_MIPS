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
            List<Grammer> grammerList = getGrammer();
            foreach (Grammer item in grammerList)
            {
                Console.WriteLine(item.Show());
            }
        }
        #endregion

        #region Private Method
        private static List<Grammer> getGrammer()
        {
            string filePath = "grammar.inf";
            List<Grammer> grammerList = new List<Grammer>();
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
                        Grammer grammer = new Grammer(line);
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
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MIPS246.Compiler.DataStructure
{
    public class Token
    {
        public string Name { set; get; }    //名称
        public int LineNo { set; get; }     //行号

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="lineNo">行号</param>
        public Token(string name, int lineNo)
        {
            this.Name = name;
            this.LineNo = lineNo;
        }

        public virtual string Show()
        {
            return "( " + this.Name + ", " + this.LineNo + " )";
        }
    }

    /// <summary>
    /// 标识符
    /// </summary>
    public class IDToken:Token
    {
        public string ID{set; get;}

        /// <summary>
        /// 标识符Token构造函数
        /// </summary>
        /// <param name="id">标识符自身</param>
        /// <param name="lineNo">行号</param>
        public IDToken(string id, int lineNo)
            : base("ID", lineNo)
        {
            this.ID = id;
        }

        public override string Show()
        {
            return "标识符：( " + this.ID + ", " + this.Name + ", " + this.LineNo + " )";
        }
    }

    /// <summary>
    /// 常量
    /// </summary>
    public class ConstantToken : Token
    {
        public int Value { set; get; }

        public ConstantToken(int value, int lineNo)
            : base("NUM", lineNo)
        {
            this.Value = value;
        }

        public override string Show()
        {
            return "常量：( " + this.Name + ", " + this.Value + ", " + this.LineNo + " )";
        }
    }

    /// <summary>
    /// 系统单词
    /// </summary>
    public class SystemWordToken : Token
    {
        /// <summary>
        /// 系统单词Token构造函数
        /// </summary>
        /// <param name="name">系统单词自身</param>
        /// <param name="lineNo"></param>
        public SystemWordToken(string name, int lineNo):base(name, lineNo)
        {
        }

        public override string Show()
        {
            return "系统单词：( " + this.Name + ", " + this.LineNo + " )";
        }
    }
}

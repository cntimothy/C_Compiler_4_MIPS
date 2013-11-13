using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MIPS246.Compiler.DataStructure
{
    public class Token
    {
        public int LineNo { set; get; }

        public Token(int lineNo)
        {
            this.LineNo = lineNo;
        }

        public virtual string Show()
        {
            return "( " + LineNo + " )";
        }
    }

    /// <summary>
    /// 标识符
    /// </summary>
    public class IDToken:Token
    {
        public string Name { set; get; }

        public IDToken(string name, int lineNo)
            : base(lineNo)
        {
            this.Name = name;
        }

        public override string Show()
        {
            return "标识符：( " + this.Name + ", " + this.LineNo + " )";
        }
    }

    /// <summary>
    /// 常量
    /// </summary>
    public class ConstantToken : Token
    {
        public int Value { set; get; }

        public ConstantToken(int value, int lineNo)
            : base(lineNo)
        {
            this.Value = value;
        }

        public override string Show()
        {
            return "常量：( " + this.Value + ", " + this.LineNo + " )";
        }
    }

    /// <summary>
    /// 系统单词
    /// </summary>
    public class SystemWordToken : Token
    {
        public SystemWord.SystemWordID ID { set; get; }

        public SystemWordToken(SystemWord.SystemWordID id, int lineNo):base(lineNo)
        {
            this.ID = id;
        }

        public override string Show()
        {
            return "系统单词：( " + this.ID + ", " + this.LineNo + " )";
        }
    }
}

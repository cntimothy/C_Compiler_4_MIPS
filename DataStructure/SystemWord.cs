using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIPS246.Compiler.DataStructure
{
    public static class SystemWord
    {
        /// <summary>
        /// 终结符集合
        /// </summary>
        public static readonly List<char> terminalCharList = new List<char>(){'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D',
            'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y',
            'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '-', '*', '/', '<', '>', '=', '!', ';', ',',
            '(', ')', '[', ']', '{', '}'};

        /// <summary>
        /// 关键字集合
        /// </summary>
        public static readonly List<string> keyWordList = new List<string>() { "else", "if", "int", "return", "void", "while" };

        public enum SystemWordID
        { 
            ELSE, IF, INT, RETURN, VOID, WHILE,
            ADD, SUB, MULT, DIV, BIGGER, NOTLESS, LESS, NOTBIGGER, EUQAL, NOTEQUAL, ASSIGN,
            COMMA, SEMI, LEFTPARENTHESIS, RIGHTPARENTHESIS, LEFTBRACKET, RIGHTBRACKET, LEFTBRACE, RIGHTBRACE
        }
    }
}

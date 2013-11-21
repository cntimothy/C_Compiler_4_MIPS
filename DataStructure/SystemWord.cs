using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIPS246.Compiler.DataStructure
{
    public static class SystemWord
    {
        /// <summary>
        /// 字符集合
        /// </summary>
        public static readonly List<char> AcceptableCharList = new List<char>(){'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D',
            'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y',
            'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '-', '*', '/', '<', '>', '=', '!', ';', ',',
            '(', ')', '[', ']', '{', '}', ' '};

        public static readonly List<string> TerminatorList = new List<string>() { "ID", "NUM", "+", "-", "*", "/",
            "<", "<=", ">", ">=", "=", "==", "!=", ";", ",", "(", ")", "[", "]", "{", "}", 
            "else", "if", "int", "return", "void", "while"};

        /// <summary>
        /// 关键字集合
        /// </summary>
        public static readonly List<string> KeyWordList = new List<string>() { "else", "if", "int", "return", "void", "while" };

        public enum SystemWordID
        { 
            ELSE, IF, INT, RETURN, VOID, WHILE,
            ADD, SUB, MULT, DIV, BIGGER, NOTLESS, LESS, NOTBIGGER, EUQAL, NOTEQUAL, ASSIGN,
            COMMA, SEMI, LEFTPARENTHESIS, RIGHTPARENTHESIS, LEFTBRACKET, RIGHTBRACKET, LEFTBRACE, RIGHTBRACE
        }

        public static readonly Dictionary<string, SystemWordID> SystemWordNameIDDic = new Dictionary<string, SystemWordID>() { 
        {"else", SystemWord.SystemWordID.ELSE}, {"if", SystemWord.SystemWordID.IF}, {"int", SystemWord.SystemWordID.INT},
        {"return", SystemWord.SystemWordID.RETURN}, {"void", SystemWord.SystemWordID.VOID}, {"while", SystemWord.SystemWordID.WHILE},
        {"+", SystemWord.SystemWordID.ADD}, {"-", SystemWord.SystemWordID.SUB}, {"*", SystemWord.SystemWordID.MULT},
        {"/", SystemWord.SystemWordID.DIV}, {">", SystemWord.SystemWordID.BIGGER}, {">=", SystemWord.SystemWordID.NOTLESS},
        {"<", SystemWord.SystemWordID.LESS}, {"<=", SystemWord.SystemWordID.NOTBIGGER}, {"==", SystemWord.SystemWordID.EUQAL},
        {"!=", SystemWord.SystemWordID.NOTEQUAL}, {"=", SystemWord.SystemWordID.ASSIGN}, {",", SystemWord.SystemWordID.COMMA},
        {";", SystemWord.SystemWordID.SEMI}, {"(", SystemWord.SystemWordID.LEFTPARENTHESIS}, {")", SystemWord.SystemWordID.RIGHTPARENTHESIS},
        {"[", SystemWord.SystemWordID.LEFTBRACKET}, {"]", SystemWord.SystemWordID.RIGHTBRACKET}, {"{", SystemWord.SystemWordID.LEFTBRACE},
        {"}", SystemWord.SystemWordID.RIGHTBRACE}};
    }
}

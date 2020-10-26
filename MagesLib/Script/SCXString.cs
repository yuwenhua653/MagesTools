using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using Mages.Script.Tokens;
using System.IO;

namespace Mages.Script
{
    public class SCXString
    {
        public bool IsEmpty => !Tokens.Any(s => s is TextToken);

        public List<Token> Tokens = new List<Token>();

        public SCXString(Reader reader)
        {
            while (!(reader.BaseStream.Position == reader.BaseStream.Length))
            {
                var type = (TokenType)reader.ReadByte();
                switch (type)
                {
                    case TokenType.SetColor:
                    case TokenType.EvaluateExpression:
                        Tokens.Add(new ExpressionToken(type, reader));
                        break;
                    case TokenType.SetFontSize:
                    case TokenType.SetTopMargin:
                    case TokenType.SetLeftMargin:
                    case TokenType.GetHardcodedValue:
                        Tokens.Add(new ShortToken(type, reader.ReadInt16()));
                        break;
                    case TokenType.AltLineBreak:
                    case TokenType.CharacterNameStart:
                    case TokenType.DialogueLineStart:
                    case TokenType.Present:
                    case TokenType.Present_ResetAlignment:
                    case TokenType.RubyBaseStart:
                    case TokenType.RubyTextStart:
                    case TokenType.RubyTextEnd:
                    case TokenType.PrintInParallel:
                    case TokenType.CenterText:
                    case TokenType.AutoForward:
                    case TokenType.AutoForward_1A:
                    case TokenType.Present_0x18:
                    case TokenType.Unk_1E:
                        Tokens.Add(new Token(type));
                        break;
                    case TokenType.Terminator:
                        return;
                    case TokenType.LineBreak:
                        goto CREATE_TEXT;
                    default:
                        if ((type & TokenType.TextMask) == 0)
                        {
                            throw new Exception("Unexpected token"+ reader.BaseStream.Position+","+type);
                        }
                    CREATE_TEXT:
                        reader.BaseStream.Position--;
                        Tokens.Add(new TextToken(reader));
                        break;

                }
            }
        }

        public void Encode(SCXWriter target)
        {
            foreach (var t in Tokens)
            {
                t.Encode(target);
            }
            target.Write((byte)TokenType.Terminator);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var t in Tokens)
            {
                sb.Append(t.ToString()).Append(" ");
            }
            if (sb.Length == 0)
            {
                return "(empty)";
            }
            sb.Length--;
            return sb.ToString();
        }
    }
}

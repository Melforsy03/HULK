using AST;
using Usuario;
using System.Text.RegularExpressions;
namespace Tokenizador
{
    public class input 
    {
    public  static List<token> TokenizeString(string input)
    {
        List<token> tokens = new List<token>();
        string currentToken = "";
        bool literal = false ;

        for (int i = 0; i < input.Length; i++)
        {
            char currentChar = input[i];
            if (currentChar == ' ' )
            {
                continue;
            }
            if (currentChar == '"')
            {
                literal = true ;
                continue;
            }
            if(CaracteresNoDefinidos(currentChar))
            {
                throw new ArgumentException("el caracter" + currentChar + "no esta definido en este lenguaje");
            }
            if (IsOperator(currentChar.ToString()))
            {
                    tokens.Add(new token(currentChar.ToString(), TokenTypes.Operator));
                    currentToken = "";
                    continue;
            }
            else if (IsPunctuation(currentChar.ToString()))
            {
                
                if (currentChar == '=' && input[i + 1] == '>' )
                {
                    tokens.Add(new token("=>" , TokenTypes.Punctuation));
                    currentToken = "";
                    i++;
                    continue;
                }
                else if (currentChar == '<' && input [i + 1] == '=')
                {
                    tokens.Add(new token("<=" , TokenTypes.Punctuation));
                    currentToken = "";
                    i++;
                    continue;
                }
                else if (currentChar == '>' && input [i + 1] == '=')
                {
                    tokens.Add(new token(">=" , TokenTypes.Punctuation));
                    currentToken = "";
                    i++;
                    continue;
                }
                else if (currentChar == '=' && input [i + 1] == '=')
                {
                    tokens.Add(new token("==" , TokenTypes.Punctuation));
                    currentToken = "";
                    i++;
                    continue;
                }
                else
                {
                    tokens.Add(new token(currentChar.ToString(), TokenTypes.Punctuation));
                }
            }
            else
            {
                currentToken += currentChar;
                for (int j = i + 1 ; j < input.Length; j++)
                  {
                    if(literal && input[j] != '"')
                    {
                        currentToken += input[j];
                        continue;
                    }
                     if(!IsPunctuation(input[j].ToString()) && input[j] != ' ' && input[j] != '"' && !IsOperator(input[j].ToString()))
                    {
                       currentToken += input[j];
                       continue;
                    }
                    if(literal)
                    {
                        literal = false ;
                         tokens.Add(new tokenLiteral (currentToken , TokenTypes.Literal));
                        i = j ;
                        currentToken = "";
                        break;
                    }
                    else if(IsKeyWords(currentToken))
                    {
                     tokens.Add(new token (currentToken , TokenTypes.Keyword));
                     currentToken = "";
                     i = j;
                     break;
                    }
                    else if (Isfunction(currentToken))
                    {
                    tokens.Add(new FunctionNode(currentToken , TokenTypes.funcion));
                    i = j;
                    currentToken  = "";
                    break;
                    }
                     else if (double.TryParse(currentToken ,out double value ))
                    {
                    tokens.Add(new tokenNumero(currentToken ,TokenTypes.Number));
                    i = j ;
                    currentToken  = "";
                    break;
                    }
                    else
                    {
                      tokens.Add(new Identificador (currentToken , TokenTypes.Identifier));
                      i = j;
                      currentToken = "";
                      break;
                    }
                  }
                    if (IsOperator(input[i].ToString()))
                    {
                        tokens.Add(new OperatorNode (input[i].ToString() , TokenTypes.Operator));
                        continue;
                    }
                    if (input[i] != ' ' && input[i] != '"')
                    {
                      tokens.Add(new token (input[i].ToString()  , TokenTypes.Punctuation));
                      continue;
                    }
            }
               
        }
    return tokens ;          
}
    
   public static bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "*" || c == "/" || c == "%" || c == "^" ;
    }

  public static bool IsPunctuation(string c)
    {
        return c == "." || c == "," || c == ";" || c == ":" || c == "\"" || c == "=>" || c == "=" || c == ">" || c == "<" || c == "<=" || c == "!=" || c == "==" || c == "(" || c == ")" || c == ">=";
    }
   public static bool IsKeyWords(string c )
    {
        return c == "let" || c == "in" || c =="Print" || c == "if" || c == "else" || c == "function" ;
    }
   public  static bool Isfunction(string c)
   {
        return c == "sin" || c == "cos" || c == "tan" || c == "Sqrt"  ;
   }
   private static bool CaracteresNoDefinidos(char c)
   {
        return c == '@' || c == '#'|| c == '$' || c== '~' ;
   }
 
  }
 public class Errors
{
    public ErrorCode Code { get; private set; }

    public string Argument { get; private set; }

    //public CodeLocation Location { get; private set; }

    public Errors(ErrorCode code, string argument)
    {
        this.Code = code;
        this.Argument = argument;
    }
}
public enum ErrorCode
{
    Lexer,
    Sintaxis,
    Semantic,
    Any

}

}









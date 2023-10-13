using AST;
namespace Tokenizador
{
public class tokenizador
{
    
public  static List<token> Identificador(List<token> expression) 

{
    List<token> variables = new List<token>();
    
    for (int i = 0; i < expression.Count; i++)
    {
                    if (i >= 1)
                    {
                        
                    if (expression[i].Value == "=" && expression[i-1].Value =="let" )
                    {
                        Console.WriteLine("la variable " + expression[i- 1].Value + "no existe en este contexto");
                    }
          
}
    }
    return null;
}
 public  static List<token> TokenizeString(string input)
    {
        List<token> tokens = new List<token>();
        string currentToken = "";

        for (int i = 0; i < input.Length; i++)
        {
            char currentChar = input[i];
            if (currentChar == ' ')
            {
                continue;
            }
            if (IsOperator(currentChar))
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
                else
                {
                    
                    tokens.Add(new token(currentChar.ToString(), TokenTypes.Punctuation));
                    currentToken = "";
                    continue;
                }
            }
            else if (currentChar == '(' || currentChar == ')' || currentChar == '{' || currentChar == '}')
            {
              
                    tokens.Add(new token(currentChar.ToString(), TokenTypes.Punctuation));
                    currentToken = "";
                    continue;
             
                //tokens.Add(new token(currentChar.ToString(), TokenTypes.Punctuation));
            }
            else
            {
                currentToken += currentChar;
            }
        
          //si es una palabra clave
             if(IsKeyWords(currentToken))
             {
                tokens.Add(new token (currentToken , TokenTypes.Keyword));
                currentToken = "";
                continue;
             }
             else
             { 
             //si es una funcion
             if (Isfunction(currentToken))
             {
                tokens.Add(new FunctionNode(currentToken , TokenTypes.funcion));
                currentToken  = "";
                continue;
             }
             else
             {
             //si es un numero 
             if (double.TryParse(currentToken ,out double value ))
             {
                for (int j = i + 1; j < input.Length; j++)
                {
                    if(double.TryParse(input[j].ToString() ,out double Value))
                    {
                        currentToken += input[j];
                    }
                   else
                   {
                    i = j-1 ;
                    break;
                   }
                }
                    tokens.Add(new tokenNumero(currentToken ,TokenTypes.Number));
                    currentToken  = "";
                    continue ;
             }
             else
             {  
                //si es un identicador 
                if (tokens.Count >= 1)
                {
                    
            if (tokens[tokens.Count - 1].Value == "let" || tokens[tokens.Count - 1].Value == "function" || tokens[tokens.Count - 1].Value == "*" || tokens[tokens.Count - 1].Value == "-" ||tokens[tokens.Count - 1].Value == "+" || tokens[tokens.Count - 1].Value == "/"|| tokens[tokens.Count - 1].Value == "%" || tokens[tokens.Count - 1].Value == "(")
            {
                for (int j = i+1; j < input.Length; j++)
                {
                    if (input[j] != ' ' && input[j] != ')')
                    {
                    
                        currentToken += input[j];
                        
                   }
                    if (input[j] == ' ' || input[j] == ')' )
                    {
                     if (tokens[tokens.Count-1].Value == "function")
                   {
            
                    tokens.Add(new Function (currentToken , TokenTypes.funcion));
                     i = j;
                        break;
                  }
                  else
                  {
                    
                        tokens.Add(new  identificador(currentToken , TokenTypes.Identifier));
                        i = j;
                        break;
                  }
                    }
                }
                     if(input[i] == ')' ||IsOperator(input[i]) || input[i] == ';')
                    {
                        if (input[i] == ')' || input[i] == ';')
                        {
                            tokens.Add(new token(input[i].ToString() , TokenTypes.Punctuation));
                        }
                        else if (IsOperator(input[i]))
                        {
                            tokens.Add(new OperatorNode(input[i].ToString(),TokenTypes.Operator));
                        }
                
                    }
                else if  (tokens[tokens.Count-1].Value == "let")
                {
                     tokens.Add(new identificador (currentToken , TokenTypes.Identifier));
                }
                
                    currentToken  = "";
                    
                    continue;
            } 
            else if(tokens.Count > 0 )
            {
            if(tokens[tokens.Count - 1].Value == "\"" || tokens[tokens.Count - 1].Value == "'")
            {
                for (int j = i +1; j < input.Length;j++)
                {
                    
                 if ( input[j] != '\"') currentToken += input[j];
                    if (input[j] == '\"' )
                    {
                        i = j - 1;
                        break;
                    
                    }
                }
                    tokens.Add(new token(currentToken ,TokenTypes.Literal));
                    currentToken  = "";
                    continue;
            }
            else if (encuentro(currentToken , tokens) != -1)
            {
                tokens.Add(tokens[encuentro(currentToken ,tokens)]);
                currentToken = "";
                continue;
            }
        }
    
        }
        }
           // tokens.Add(new token(currentToken, TokenTypes.Identifier));
        }
         }
        }
        return tokens;
    }

   public static bool IsOperator(char c)
    {
        return c == '+' || c == '-' || c == '*' || c == '/' ;
    }

  public  static bool IsPunctuation(string c)
    {
        return c == "." || c == "," || c == ";" || c == ":" || c == "\"" || c == "=>" || c == "=" || c == ">" || c == "<" || c == "<=" || c == "!=" || c == "==";
    }


   public static bool IsKeyWords(string c )
    {
        return c == "let" || c == "in" || c =="Print" || c == "if" || c == "else" || c == "function" ;
    }
   public  static bool Isfunction(string c)
   {
        return c == "sin" || c == "cos" || c == "tan" || c == "sqrt"  || c == "^";
  }
  public static int encuentro(string a ,List<token> b)
  {
    for (int i = 0; i < b.Count; i++)
    {
        if (a == b[i].Value)
        {
            return i;
        }
    }
    return -1;
  }
  
    }
}







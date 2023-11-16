using AST;
using Usuario;
using System.Text.RegularExpressions;
namespace Tokenizador
{
    public class input 
    {
        public List<string> inputs {get ;set;}
        public static List<string> errores {get ; set ;}
        
        public static  string cadenas {get ; set ;}
        public input ()
        {
            inputs = new List<string>();
            errores = new List<string>();
        }
        public static List<token> ERROR ( string cadenas)
        {
            List<token> m = TokenizeString(cadenas);
            Corregir(m);
            if (errores.Count == 0)
            {
                return m;
            }
            for (int i = 0; i < errores.Count; i++)
            {
                Console.WriteLine(errores[i]);
            }
            return null;
        }
    
public static List<string> Corregir(List<token> expression) 
{
   
    int parentisis = 0;
    errores = new List<string>();
    for (int i = 0; i < expression.Count; i++)
    {
        if (expression[expression.Count - 1].Value !=";")
        {
             errores.Add("error sintactico , esperabamos un ;");
             break;
        }
        if (expression[i].Value == "in" )
        {
            if (expression[i + 1].Value == ";")
            {
                errores.Add("error sintactico ,en el let - in ");
            }
            if (expression[i+ 1].Type != TokenTypes.funcion && expression [i + 1].Type != TokenTypes.Keyword && expression [i + 1].Type != TokenTypes.Identifier)
            {
                errores.Add("error semantico , en el in - let ");
            }
        }
        if (i >= 1)
        { 
        if (expression[i].Value == "=" && expression[i-1].Type != TokenTypes.Identifier )
        {
        errores.Add(" error lexico , en el in - let");
        continue ;
        }
        if(expression[i].Value == "=" &&  expression[i+1].Value == "in")
        {
            errores.Add("error sintactico en el in - let");
            continue ;
        }
        if (expression[i].Value == "=")
        {
            if(expression[i + 1].Type != TokenTypes.Identifier && expression[i + 1].Type != TokenTypes.Number && expression[i+1].Type != TokenTypes.Literal && expression[i + 1].Type != TokenTypes.funcion )
            {
                if (expression[i + 1].Value == "\"")
                {
                    if (expression[i + 2].Type != TokenTypes.Literal)
                    {
                         errores.Add("error de sintaxis , se esperaba un string");
                         continue;
                    }
                    if (expression[i + 3].Value != "\"")
                    {
                       errores.Add("error de sintaxis , se esperaba un \"");
                       continue; 
                    }
                }
              
            }
        }
        }
        if (expression[i].Type == TokenTypes.Identifier)
        {
          if (Double.TryParse(expression[i].Value ,out  double result))
          {
            errores.Add(" error lexico , la variable " + expression[i].Value + " no es un nombre valido");
            continue;
          }
          else if (Regex.IsMatch (expression[i].Value , @"\d"))
          {
            errores.Add(" error lexico ,la variable "  + expression[i].Value + " no es un nombre valido");
            continue;
          }
        }
        if (expression[i].Value == ")" )
        {
            parentisis--;
            if (parentisis < 0)
            {
                errores.Add("error lexico , se esperaba un )");
                parentisis++;
                continue;
            }
           
       }
       if (expression[i].Value == "(")
       {
         parentisis++;
         continue;
       }
 }
 if (parentisis != 0)
 {
    errores.Add("error lexico , se esperaba un (");
 }
   return errores ;
}

 public  static List<token> TokenizeString(string input)
    {
        int cont = 0;
        bool c = false ;
        List<token> tokens = new List<token>();
        string currentToken = "";

        for (int i = 0; i < input.Length; i++)
        {
            char currentChar = input[i];
            if (currentChar == ' ')
            {
                continue;
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
                else
                {
                    if (currentChar.ToString() == "\"")
                    {
                        cont++;
                    }
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
            else if (currentChar == ',')
            {
                tokens.Add(new token(currentChar.ToString(),TokenTypes.Keyword));
                currentToken ="";
                continue;
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
            if (tokens[tokens.Count - 1].Value == "let" || tokens[tokens.Count - 1].Value == "function" || tokens[tokens.Count - 1].Value == "*" || tokens[tokens.Count - 1].Value == "-" ||tokens[tokens.Count - 1].Value == "+" || tokens[tokens.Count - 1].Value == "/"|| tokens[tokens.Count - 1].Value == "%" || tokens[tokens.Count - 1].Value == "(" || tokens[tokens.Count - 1].Value == "in" || tokens[tokens.Count - 1].Value == ",")
            {
              
                for (int j = i+1; j < input.Length; j++)
                {
                    if (input[j] != ' ' && input[j] != ')' && input[j] != '(' && !IsOperator(input[j].ToString()) && !Isfunction(input[j].ToString()) && input[j] !=',' && input[j] != ';')
                    {
                        currentToken += input[j];
                    }
                    if (input[j] == ' ' || input[j] == ')' || input[j] == '(' || IsOperator(input[j].ToString()) || Isfunction(input[j].ToString()) || input[j] == ',' || input[j] == ';')
                    {
                     if (tokens[tokens.Count - 1].Value == "function")
                   {
                    tokens.Add(new Function (currentToken , TokenTypes.funcion));
                     i = j;
                     c = true;
                     if (input[j] == ';')
                    {
                         tokens.Add(new token(currentToken ,TokenTypes.Punctuation ));
                    }
                    break;
                  }
                  else if ( tokens[tokens.Count-1 ].Value == "in" && encuentro(currentToken , Usuario.A.root.fuc) != -1)
                  {
                     tokens.Add(new Function (currentToken , TokenTypes.funcion));
                     i = j;
                     c = true ;
                     if (input[j] == ';')
                    {
                         tokens.Add(new token(currentToken ,TokenTypes.Punctuation ));
                    }
                    break;
                  }
                  else if ( tokens[tokens.Count - 1 ].Value == "in" && IsKeyWords(currentToken))
                  {
                    c =true;
                    tokens.Add(new token(currentToken ,TokenTypes.Keyword) );
                    i = j ;
                    if (input[j] == ';')
                    {
                         tokens.Add(new token(currentToken ,TokenTypes.Punctuation ));
                    }
                    break;
                  }
                   else
                  {
                    if (IsKeyWords(currentToken))
                    {
                        tokens.Add(new token(currentToken  , TokenTypes.Keyword));
                        currentToken = "";
                        i = j ; 
                        c = true ;
                        break;
                        
                    }
                    else
                    {
                        tokens.Add(new identificador(currentToken , TokenTypes.Identifier));
                        currentToken = "";
                        i = j;
                        break;
                    }
                    
                  }
                }
            }
            }
        }
            
                if (c == true )
                {
                    c = false ;
                    continue;
                }
                if (input[i] == ',' )
                {
                    tokens.Add(new token(input[i].ToString(), TokenTypes.Punctuation));
                    continue;
                }
            if(tokens.Count > 0 )
            {
            if(cont > 0 && tokens[tokens.Count - 1].Value == "\"" )
            {
              // currentToken = "";
                for (int j = i + 1; j < input.Length;j++)
                {
                 if ( input[j] != '\"') currentToken += input[j];
                    if (input[j] == '\"' )
                    {
                        cont--;
                        i = j ;
                        tokens.Add(new token(currentToken ,TokenTypes.Literal));
                        tokens.Add(new token (input[j].ToString() , TokenTypes.Punctuation));
                        currentToken = "";
                        break;
                    }
                }
                    continue;
            }
            else if (i >= 1)
            {
             if (encuentro(currentToken , tokens) != - 1)
            {
                tokens.Add(new token(currentToken , TokenTypes.Identifier));
                currentToken = "";
                continue;
            }
           
            }
        
                if (i < input.Length)
                {
                 if(input[i] == ')' ||IsOperator(input[i].ToString()) || input[i] == ';' || input[i] == '(' || input[i] == ',')
                    {
                        if (input[i] == ')' || input[i] == ';' ||input[i] == '(' ||input[i] ==',')
                        {
                            tokens.Add(new token(input[i].ToString() , TokenTypes.Punctuation));
                            continue ;
                        }
                        else if (IsOperator(input[i].ToString()))
                        {
                            tokens.Add(new OperatorNode(input[i].ToString(),TokenTypes.Operator));
                            continue;
                        }
                
                    }
                else if  (tokens[tokens.Count-1].Value == "let")
                {
                     tokens.Add(new identificador (currentToken , TokenTypes.Identifier));
                     continue;
                }
                }
                }
                else  if (tokens.Count == 0)
                {
                    for (int j = i + 1; j < input.Length; j++)
                    {
                        if (input[j] != ' ')
                        {
                            currentToken += input[j];
                        }
                        else
                        {
                                i = j ;
                            break ;
                    
                        }
                    }
                    if (currentToken == "function")
                    {
                        tokens.Add(new token (currentToken , TokenTypes.Keyword));
                        currentToken = "";
                        continue;
                    }
                    else
                    {
                        if (IsKeyWords(currentToken))
                    {
                        tokens.Add(new token(currentToken  , TokenTypes.Keyword));
                        currentToken = "";
                        continue ;
                        
                    }
                    else
                    {
                        
                    tokens.Add(new identificador (currentToken , TokenTypes.Identifier));
                    currentToken = "";
                    continue;
                    }
                    }
                  
                }
                    
             
        }
             }
             }
        }
        
          return tokens;
    }

   public static bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "*" || c == "/" || c == "%" ;
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








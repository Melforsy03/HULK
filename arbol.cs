using AST;
namespace Abol
{
public  class Parser :token
{
    public static string value {get; set;}
    public static TokenTypes Type {get ; set;}
    private  int position;
     public Parser Root {get ; set ;}
    public  List<token> expression {get; set;}
    public  List<token> variables {get ; set;}
    public List<token> parameters{get ; set;}
    public  List<token> fuc{get ; set;}
    public static List<token> fuc2 {get ; set ;}
    public static List<token> variables2 {get ; set;}

    public Parser(Parser root) :base(value , Type)
    {
         this.Root = root;
        this.position = 0;
        tokens = new List<token>();
        variables = new List<token>();
        parameters = new List<token>();
        fuc = new List<token>();
    
    }

//parsea el arbol 
public void construir()
{
     Parse();
    
}
public void EvaluateO()
{

    //List<String> A = new List<string>();
      Evaluate(tokens );
}
public void Evaluate (List<token> b )
{
    string evaluar = "";
    foreach (var item in b)
    {
     if(item == null)
     {
        continue;
     }
         if (item.Value == "+")
        {
            evaluar = ((OperatorNode)item).Evaluar().ToString();
            Console.WriteLine(evaluar);
        }
        else if(item.Value== "-")
        {
            evaluar = ((OperatorNode)item).Evaluar().ToString();
            Console.WriteLine(evaluar);
        }
        else if (item.Value == "let")
        {
            
            evaluar = ((LetIn)item).Evaluar().ToString();
            Console.WriteLine(evaluar);
        }
        else if (item.Value == ">")
        {
            evaluar = ((tokenBul)item).Evaluar().ToString();
        }
        else if (item.Value == "Print")
        {
            evaluar = item.Evaluar();
        }
        else if (item.Value == "if")
        {
            evaluar = ((IfElseNode)item).Evaluar().ToString();
            Console.WriteLine(evaluar);
        }
        else if(Isfunction(item.Value))
        {
                evaluar = ((FunctionNode)item).Evaluar().ToString();
                Console.WriteLine(evaluar); 
        }
        else if(item.Type == TokenTypes.funcion && FindFun(item , variables) != -1 )
        {
            int k = FindFun(item , variables);
            Function funcion = (Function)variables[k];
            funcion.globales = variables;
            funcion.parametro = item.tokens[0];
           Console.WriteLine(funcion.Evaluar().ToString()); 
        }
        else if(item.tokens != null)
        {
              Evaluate(item.tokens);
        }
       
    }      
}
    public void  Parse()
    
    {
       expresiones();
      fuc2 = fuc ;
     variables2 = variables;
    }
    public void expresiones ()
    {
          if (expression.Count == 0)
        {
            return;
        }
            if (expression[position].Value == "Print")
            {
               position++;
               tokens.Add(parserPrint(expression[position - 1]));
               expresiones();  
            }
           
             else if (expression[position].Value == "if")
            {
                position++;
                tokens.Add(parserIFelse());
                expresiones();
            }
            else if(Isfunction(expression[position].Value))
            {
                position++;
                tokens.Add(ParseFunction(expression[position - 1] ));
                expresiones();
            }
            else  
            {
                tokens.Add(ParseExpression());
            }
    }
//parsea las expresiones 
    private token ParseExpression()
    {
       
        token leftNode = ParseTerm();
        if (position == expression.Count)
        {
           return leftNode ;
        }
        
        while (position < expression.Count)
        {
            string c = expression[position].Value;
          
          if (leftNode != null && leftNode is Function && c == ")" )
          {
            if (position + 1 < expression.Count)
            {
                 position++;
               c = expression[position].Value ;
            }
          }
          if (leftNode != null && leftNode is OperatorNode && c == ")")
          {
          if (position + 1 < expression.Count)
            {
                 position++;
               c = expression[position].Value ;
            }
          }
            //si encuentra una funcion
             if(Isfunction(c))
            {
                int p = position;
                FunctionNode operatorNode = new FunctionNode(c,TokenTypes.Operator );
                operatorNode.tokens[0] = ParseTerm();
                position++;
                operatorNode.tokens[1] = ParseFunction(operatorNode);
                //operatorNode.Argument = (tokenNumero)operatorNode.tokens[1];
               return operatorNode;
  
            }
            else if (c == "Print")
            {
                position++;
                return parserPrint(expression[position - 1]);
            }
      
            else if (c == "if")
            {
                position++;
                return parserIFelse();
            }
            else if (c == "else")
            {
                position++;
                return ParseFunction(expression[position - 1]);
            }
          
            //si encuentra un operador de estos
            else if (c == "+" || c == "-" || c == "%")
            {
                 OperatorNode operatorNode = new OperatorNode( c,TokenTypes.Operator );
                 operatorNode.tokens.Add(leftNode);
                 position++;
                 operatorNode.tokens.Add(ParseTerm());
                 return operatorNode;
                
            }   
            else if (c == "(")
            {
                return ParseTerm();
            }
            //si encuentra un operador de estos 
            else if (c == ">" || c == "<" || c == "<=" || c == ">=" ||  c == "!=" || c == "==" )
            {
                position++;
                token rigthNode = ParseTerm();
                tokenBul condicion = new tokenBul(c,TokenTypes.Condicional);
                 condicion.tokens.Add(leftNode);
                 condicion.tokens.Add(rigthNode);
                return condicion;
            }
            //si encuentra un operador de estos 
            else if(c == "&&" || c == "||" )
            {
                position++;
                tokenBul condicion = new tokenBul(c,TokenTypes.Condicional);
                token rigthNode = ParseTerm();
                condicion.tokens.Add((tokenBul)leftNode);
                condicion.tokens.Add((tokenBul)rigthNode);
                return condicion;

            }
            else if (c == "let")
            {
                position++;
                return LETIN(c);
            }
            else
            {
                break;
            }
        }
    return leftNode;
    }
    //si encuentra un operador de estos
    private token ParseTerm()
    {
        token leftNode = ParseFactor();


        while (position < expression.Count)
        {
            string c = expression[position].Value;

            if (c == "*" || c == "/")
            {
                OperatorNode operatorNode = new OperatorNode(c , TokenTypes.Operator);
                position++;
                token rightNode = ParseFactor();
                 operatorNode.tokens.Add(leftNode);
                operatorNode.tokens.Add(rightNode);
                leftNode = operatorNode;
            }
            else
            {
                break;
            }
        }

        return leftNode;
    }

//este metodo devuelve un numero o en caso de ser un parentesis de apertura entra a la sub cadena 
    private token ParseFactor()
    
    {
        token node  = null ;
        //si ya no hay mas token por recorrer 
        if (position >= expression.Count)
        {
            Console.WriteLine("Expresión inválida");
            return node;
        }

        //esto es por si se encuentra una variable que almacene su valor 
        token k = FinIndeX(expression[position]);
        if (k != null)
        {
           node = k;
           position++;
        }
        else if( encuentro(expression[position].Value , Root.variables) != -1 || encuentro(expression[position].Value , fuc) != -1 )
        {
          Function p = new Function(expression[position].Value , TokenTypes.funcion );
            position++;
            token com = new token("," , TokenTypes.Keyword);
            if (expression[position].Value == "(")
            {
                position++;
            }
            for (int i = position; i < expression.Count; i++)
            {
                       com.tokens.Add(ParseExpression());
                       if (expression[position].Value == ",")
                       {
                        position++;
                        continue;
                       } 
                       if (expression[position].Value == ")")
                       {
                        position++;
                        break;
                       }
                    }
                     p.tokens.Add(com);
                     
                     
                     if (encuentro(p.Value , Root.variables) != -1)
                     {
                        int j = encuentro(p.Value , Root.variables);
                        if (p.tokens[0].tokens.Count != Root.variables[j].tokens[0].tokens.Count )
                        {
                            Console.WriteLine("error semantico , la funcion " + p.Value  + " no recive " + p.tokens[0].tokens .Count + " paramatros");
                            throw new ArgumentException();
                        }
                       p.tokens.Add(Root.variables[j].tokens[1]);
                       p.globales.Add(Root.variables[j]);
                        return p;
                     }
                    node = p;
        }
        else if(position >= 2 && expression[position].Type == TokenTypes.Identifier && expression[position - 2].Type == TokenTypes.funcion)
        {
            node = expression[position];
            parameters.Add(node);
            position++;
        }
      
        else if(encuentro(expression[position].Value , parameters) != - 1)
        {
            node = parameters[encuentro(expression[position].Value , parameters)];
            position++;

        }
        else
        {
        string c = expression[position].Value;
       
         if (expression[position].Type == TokenTypes.Identifier && expression[position -1 ].Value == "let")
        {
            identificador iden = (identificador)expression[position];
            position++;
            iden.tokens.Add(ParseExpression());
            variables.Add(iden);
            node = ParseExpression();
        }
       
        else if(expression[position].Type == TokenTypes.Identifier) 
        {
            node = expression[position];
            position++;
           
        }
        else if (expression[position].Type == TokenTypes.funcion && expression[position - 1].Value == "function")
        {
            ParseFUC(expression[position]);
            position++;
            if (position < expression.Count)
            {
                 node = ParseExpression();
            }
        }
        else if (double.TryParse(c,out double value))
        {
            double val = value;
            node = new tokenNumero (val.ToString() , TokenTypes.Number );
            position++;
        }
        //si el token es un literal 
        else if (expression[position].Type == TokenTypes.Literal)
        {
            node = new tokenLiteral(expression[position].Value , TokenTypes.Literal);
            position++;
            if (expression[position].Value == "\"")
            {
                return node ;
            }
            else 
            {
                Console.WriteLine("error falto un \" ");
            }
           
            if (expression[position].Value != "}" && position == expression.Count - 1)
            {
                Console.WriteLine("error falta un }");
            }
        } 
        //si es una coma continue o algunas de estas cosas seguir en lo suyo
         else if (c == "(" || c == "{" || c =="\"" || c == "="  || c == "function" || c == "=>"  )
        {
        
            if (position == expression.Count && expression[position - 1].Value != ")")
            {
                Console.WriteLine("Expresión inválida , se esperaba un parentesis");
            }
            else if (position == expression.Count && expression[position - 1].Value != "}")
            {
                Console.WriteLine("Expresion invalida , se esperaba un corchete ");
            }
            else if (position == expression.Count && expression[position - 1].Value != "\"")
            {
                Console.WriteLine("Expresion invalida , se esperaba un corchete ");
            }
               position++;
               node = ParseExpression();
        }
      /* else if (IsOperator(c))
        {
            node =  ParseExpression();
        }*/
        else if (c == "Print")
        {
            position++;
            Print a = new Print(c , TokenTypes.Keyword);
            node = parserPrint(a);
        }
        else if(Isfunction(c))
        {
            position++;
            node = ParseFunction(expression[position - 1]);
        }
        else if (c == "if")
        {
            position++;
            node = parserIFelse();
        }
        else if (c == "else")
        {
            position++;
            node = ParseFunction(expression[position - 1]);
        }
        else if (c == "let")
        {
            position++;
            node = LETIN(c);
        }
        
       
     }
     return node ;
  }
    //Para recorrer las funciones
    public  token ParseFunction(token funcion )
{
    token a = ParseExpression();
    funcion.tokens.Add(a);
    return funcion;
            
} 

    public token parserPrint(token parent)
    {
            token a = ParseExpression();
            parent.tokens.Add(a);
            return parent;
    }

    public token parserIFelse()

    {
        IfElseNode ifi = new IfElseNode("if" , TokenTypes.Condicional);
        token a = ParseExpression();
        ifi.tokens.Add(a);
        position++;
        token b = ParseExpression();
        ifi.tokens.Add(b);
       if(expression[position].Value == "(")
       {
        position++;
       }
        token c = ParseExpression();
        ifi.tokens.Add(c);
        return ifi;
    }
public token LETIN(string c )
{
       LetIn tok = new LetIn(c , TokenTypes.Keyword);

               for (int i = position; i < expression.Count; position++)
               {
                    if (expression[position] is identificador)
                    {
                        identificador nodo = (identificador)expression[position];
                        position++;
                        nodo.tokens.Add(ParseExpression());
                       // variables.Add(iden);
                       tok.variables.Add(nodo);
                       if (expression[position].Value == "\"")
                       {
                        position++;
                       }
                       if (expression[position].Value == ",")
                       {
                        continue;
                       } 
                       if (expression[position].Value == "in")
                       {
                        break;
                       }
                       
                    }
               }
                    if (expression[position].Value == "in")
                    {
                        position++;
                        tok.tokens.Add(ParseTerm());
                        
                    }
                    return tok ;
            }
                

public void ParseFUC(token b)
{
            fuc.Add(expression[position]);
            Function iden = (Function)b;
            position++;
            if (expression[position].Value == "(")
            {
                position++;
            }
            token coma = new token("," , TokenTypes.Keyword);
             for (int i = position; i < expression.Count; i++)
               {
                    if (expression[position] is identificador)
                    {
                        identificador nodo = (identificador)expression[position];
                        position++;
                        nodo.tokens.Add(ParseExpression());
                       // variables.Add(iden);
                       coma.tokens.Add(nodo);
                       if (expression[position].Value == ",")
                       {
                        //coma.tokens.Add(nodo);
                        position++;
                        continue;
                       } 
                       if (expression[position].Value == ")")
                       {
                        break;
                       }
                    }
               }
             if(expression[position].Value == ")")
            {
              position++;
            }
            iden.tokens.Add(coma);
            iden.tokens.Add(ParseExpression());
            Root.variables.Add(iden);
            
}

    public  token FinIndeX (token a)
    {
        for (int i = 0 ; i < variables.Count; i++)
        {
            if (a.Value == variables[i].Value)
            {
                return variables[i];
            }
        }
        return null;
    }
  
   public  static bool Isfunction(string c)
   {
        return c == "sin" || c == "cos" || c == "tan" || c == "sqrt"  || c == "^";
  }

public static int FindFun(token a , List<token> variables )
{
    for (int i = 0; i < variables.Count; i++)
    {
        if (a.Value == variables[i].Value)
        {
            return i;
        }
    }
 
    return -1;
}
 public static  int encuentro(string a , List<token> b)
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
 public static bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "*" || c == "/" ;
    }
public void agregar (Parser a )
{
    tokens.Add(a);
}
}
}

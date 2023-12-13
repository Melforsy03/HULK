using AST;
using Tokenizador;
namespace Abol
{
public  class Parser :token
{
    private  int position;
    public Parser Root {get ; set ;}
    public  List<token> expression {get; set;}
    public List<token> variablesGlobales {get ; set;}
    public List<token> variablesLocales {get ; set;}
    public Stack <string> parentesis {get ; set ;}

    private List<Errors> errors { get ; set ;}
    public Parser(string value , TokenTypes Type , Parser root) :base(value , Type)
    {
       this.Root = root;
       variablesGlobales = new List<token>();
       variablesLocales = new List<token>();
       parentesis = new Stack<string>();
       errors = new List<Errors>();
    }
    private bool CheckSemantic(List<Errors> errors)
    {
        bool chek = true;
        foreach (var item in variablesLocales)
        {
             item.CheckSemantic(errors);
        }
        foreach (var item in tokens)
        {
            item.CheckSemantic(errors);
            if(variablesLocales.Any(valor => valor.Value == item.Value))
            {
                FunctionHulk auxiliar = (FunctionHulk)variablesLocales.Find(valor => valor.Value == item.Value);

                if(auxiliar.variablesLocales.Count != ((FunctionHulk)item).variablesLocales.Count)
                {
                    errors.Add(new Errors(ErrorCode.Semantic , "la funcion " + item.Value + " no cuenta con esa cantidad de parametros"));
                }
                else
                {
                    for (int i = 0; i < auxiliar.variablesLocales.Count; i++)
                    {
                        if(auxiliar.variablesLocales[i].TypeReturn != ((FunctionHulk)item).variablesLocales[i].TypeReturn)
                        {
                            errors.Add(new Errors(ErrorCode.Semantic , "En la funcion " + auxiliar.Value + "la variable" + ((FunctionHulk)item).variablesLocales[i].Value + " recive un tipo incorrecto"));
                            
                        }
                    }
                }
                
            }
            else if(item is FunctionHulk)
            {
                errors.Add(new Errors(ErrorCode.Semantic ,"la funcion " + item.Value + " no existe en este contexto" ));
            }
          
        }
        if(errors.Count == 0)return true;
        return false ;
    }
//parsea el arbol 
   public void construir()
{
   try
   {
     Parse();
   }
   catch (Exception ex)
   {
    Console.WriteLine(ex.Message);
   }
    Evaluate();
   
}

   public void Evaluate ()
{
    bool chek = CheckSemantic(errors);
    if (!chek)
    {
        foreach (var item in errors)
        {
            Console.WriteLine(item.Argument);
        }
    }
    else
    {
    foreach (token item in tokens)
    {
     try
     {
        Console.WriteLine(item.Evaluar());
     }
     catch ( Exception x )
     {
        Console.WriteLine(x.Message);
        
     }
   } 
 }
}
    private void  Parse()
    {
        token auxiliar = ParseExpression();
        if(auxiliar is LetIn || auxiliar  is Print || auxiliar is FunctionHulk && auxiliar.tokens.Count == 0 || auxiliar is IfElseNode)
        {
            tokens.Add(auxiliar);
        }
        else
        {
            variablesLocales.Add(auxiliar);
        }
        if(position < expression.Count -1 )
        {
             position++;
              Parse();
       
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

            if (c == ";" || c == "," || c == "in" )
            {
                return leftNode;
            }
            //si encuentra una funcion
             else if(Isfunction(c))
            {
                int p = position;
                FunctionNode operatorNode = new FunctionNode(c,TokenTypes.Operator );
                operatorNode.tokens.Add(ParseTerm());
                position++;
                operatorNode.tokens.Add(ParseFunction(operatorNode));
                   if(expression[position].Value == ")")
                {
                    position++;
                    if(parentesis.Count > 0)parentesis.Pop();
                    else
                    {
                        throw new ArgumentException("se esperaba un parentesis de apertura");
                    }
                }
                return operatorNode;
  
            }
            //si encuentra un operador de estos
            else if (IsOperator(c))
            {
                 OperatorNode operatorNode = new OperatorNode( c,TokenTypes.Operator );
                 operatorNode.tokens.Add(leftNode);
                 position++;
                 operatorNode.tokens.Add(ParseTerm());
                if(expression[position].Value == ")")
                {
                    position++;
                    if(parentesis.Count > 0)parentesis.Pop();
                    else
                    {
                        throw new ArgumentException("se esperaba un parentesis de apertura");
                    }
                }
                 return operatorNode;
                
            }   
            else if (c == "(")
            {
                parentesis.Push("(");
                return ParseTerm();
            }
            //si encuentra un operador de estos 
            else if (c == ">" || c == "<" || c == "<=" || c == ">=" ||  c == "!=" || c == "==" )
            {
                position++;
                token rigthNode = ParseTerm();
                tokenBul condicion = new tokenBul(c,TokenTypes.boolean);
                condicion.tokens.Add(leftNode);
                condicion.tokens.Add(rigthNode);
                return condicion;
            }
            //si encuentra un operador de estos 
            else if(c == "&&" || c == "||" )
            {
                position++;
                tokenBul condicion = new tokenBul(c,TokenTypes.boolean);
                token rigthNode = ParseTerm();
                condicion.tokens.Add((tokenBul)leftNode);
                condicion.tokens.Add((tokenBul)rigthNode);
                return condicion;
            }
            else if ( c == ")" ||c == "else")
            {
                if (c== ")")
                {
                    if(parentesis.Count > 0)parentesis.Pop();
                    else
                    {
                        throw new ArgumentException("esperabamos un parentesis de apertura");
                    }
                }
                return leftNode;
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

            if (c == "*" || c == "/" || c == "%")
            {
                OperatorNode operatorNode = new OperatorNode(c , TokenTypes.Operator);
                position++;
                token rightNode = ParseFactor();
                 operatorNode.tokens.Add(leftNode);
                operatorNode.tokens.Add(rightNode);
                leftNode = operatorNode;
                if(expression[position].Value == ")")
                {
                    position++;
                    if(parentesis.Count > 0)parentesis.Pop();
                    else
                    {
                        throw new ArgumentException("se esperaba un parentesis de apertura");
                    }
                }
                
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

        ///valor del token 
        string c  = expression[position].Value ; 
        //si es una coma continue o algunas de estas cosas seguir en lo suyo
        if (c == "(" || c == "{" || c =="\"" || c == "="   || c == "=>"  )
        {
              if(c == "(")parentesis.Push("(");
              position++;
             node = ParseExpression();
        }
        else if (c == "function" || expression[position].Type ==  TokenTypes.Identifier && expression[position+1].Value == "(")
        {
           return FuncionesHulk();
        }
        else if (expression[position].Type == TokenTypes.Identifier)
        {
            if (variablesLocales.Any(valor => valor.Value == c))
            {
                position++;
                return (token)variablesLocales.Find(valor => valor.Value == c).Clone();
            }
            else if (expression[position + 1].Value == "=")
            {
                token temporal = new Identificador(expression[position].Value , TokenTypes.Identifier);
                position++;
                temporal.tokens.Add(ParseExpression());
                return temporal;
            }
            else if (variablesGlobales.Any(valor => valor.Value == c))
            {
                position++;
                return (token)variablesGlobales.Find(valor => valor.Value == c).Clone();
            }
            else
            {
                position++;
                return (token)expression[position-1].Clone();
            }
        }
         else if (double.TryParse(c,out double value))
        {
            position++;
            return new tokenNumero (c, TokenTypes.Number );
        }
        else if (c == "Print")
        {
            if (expression[position].Value == "(")
            {
                parentesis.Push("(");
            }
            position++;
            node = parserPrint();
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
        else if (c == "let")
        {
            position++;
            node = Letin(c);
        }
        else if (expression[position].Type == TokenTypes.Literal)
        {
            position++;
            return new tokenLiteral(expression[position - 1].Value , TokenTypes.Literal);
        }
        
     return node ;
  }
    //Para recorrer las funciones
    private  token ParseFunction(token funcion )
  {
    token a = ParseExpression();
    funcion.tokens.Add(a);
    return funcion;
            
  } 
    private token parserPrint()

    {       Print Print = new Print("Print" , TokenTypes.Keyword);
            Print.tokens.Add(ParseExpression());
            if(expression[position].Value == ")" )
            {
               if(parentesis.Count > 0 ) parentesis.Pop();
               else
               {
                throw new ArgumentNullException("esperabamos un (");
               }
            }
            if(parentesis.Count != 0)
            {
                throw new ArgumentException("esperabamos un )");
            }
            return Print;
    }

    private token parserIFelse()
    {
       IfElseNode IfElse = new IfElseNode("if" , TokenTypes.boolean);
      // parsea la condicion del if else 
      token condicion = ParseExpression();
      IfElse.tokens.Add(condicion);
      //parsea el cuerpo then 
      if(expression[position].Value == ")")parentesis.Pop();
      else
      {
        throw new ArgumentException("esperabamos un )");
      }
      position++;
      token then = ParseExpression();
      IfElse.tokens.Add(then);
      position++;
      //parsea el cuerpo Else
      token Else = ParseExpression();
      IfElse.tokens.Add(Else);
      position = position;
      return IfElse;
    }
    private token Letin(string c )
   {
       LetIn let = new LetIn("let" , TokenTypes.Keyword , this);
       let.variablesGlobales.AddRange(let.Root.variablesGlobales.Select (x => x));
       let.variablesGlobales.AddRange(let.Root.variablesLocales.Select (x => x));
       let.variablesGlobales = let.variablesGlobales.Distinct().ToList();
       let.parentesis = parentesis;
       let.expression = expression;
       let.position = position ;
              while(let.position < expression.Count - 1)
              {
                    if (expression[let.position] is Identificador)
                    {
                         Identificador nodo = (Identificador)expression[let.position];
                         let.position++;
                         nodo.tokens.Add(let.ParseExpression());
                         let.variablesLocales.Add(nodo);
                        if(expression[let.position].Value == ",")let.position++;
                    }
                    if (expression[let.position].Value == ")") 
                    {
                        if(let.parentesis.Count != 0)parentesis.Pop();
                        else
                        {
                            throw new ArgumentException("esperabamos un )");
                        }
                        let.position++;
                    }
                    if (expression[let.position].Value == "in")break;
                    if(position > expression.Count - 1 && expression[let.position].Value == ";" )
                    {
                        throw new ArgumentException("error en let - in ,esperabamos la instruccion in");
                    }

               }
                if (expression[let.position].Value == "in")
                {
                  let.position++;
                  let.tokens.Add(let.ParseExpression());
                }
                if(expression[let.position].Value != ";")
                {
                    throw new ArgumentException("esperabamos un ;");
                }
                 position = let.position++;
                
                
         return let ;
   }

    private  static bool Isfunction(string c)
   {
        return c == "sin" || c == "cos" || c == "tan" || c == "Sqrt"  ;
  }

    private static bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "*" || c == "/" || c == "^" ;
    }
    private token FuncionesHulk()
    {
        string NombreFuncion = "";
         if(expression[position].Value == "function")
        {
            position++;
        }
        if (expression[position].Type == TokenTypes.Identifier )
        {
            NombreFuncion = expression[position].Value;
            position++;
        }
        else if(expression[position].Value != "(")
        {
            throw new ArgumentException("esprabamos un parentesis de apertura despues declarada la funcion");
        }
        FunctionHulk Funcion = new FunctionHulk(NombreFuncion, TokenTypes.funcion , this);
        //agrega las variables locales del arbol padre a las variables gloabales (clonadas)
        Funcion.variablesGlobales.AddRange(variablesGlobales.Select(x => x));
        //agrega las variables gloabales del arbol padre a las variables gloabales (clonadas)
        Funcion.variablesGlobales.AddRange(variablesLocales.Select(x => x));
        //eliminar elementos duplicados 
        Funcion.variablesGlobales = Funcion.variablesGlobales.Distinct().ToList();
        Funcion.position = position;
        Funcion.expression = expression;
        while (expression[Funcion.position].Value != ")")
        {
        if(expression[Funcion.position].Value == "," )Funcion.position++;
        if(expression[Funcion.position].Value == "(") 
        {
            Funcion.position++;
            parentesis.Push(expression[Funcion.position].Value);
        }
       
         if (expression[Funcion.position].Value != ")")Funcion.variablesLocales.Add(Funcion.ParseExpression());
        
        if (expression[Funcion.position].Value == ";" || Funcion.position > expression.Count - 1)
        {

            position = Funcion.position++;
            return Funcion ;
        }
        if(expression[Funcion.position].Value == ")")
        {
            parentesis.Push(expression[Funcion.position].Value);
            Funcion.position++;
            break;
        }
        
        }
        position = Funcion.position;
        // si la funcion sera definida 
        if(expression[Funcion.position].Value == "=>") Funcion.position++;
        // si la funcion solo fue llamada 
        else if(expression[Funcion.position].Value != "=>")return Funcion;
        Funcion.tokens.Add(Funcion.ParseExpression());
        position = Funcion.position;
        if(parentesis.Count != 0)
        {
            if (parentesis.Peek() == ")")
            {
                throw new ArgumentException ("esperabamos un parentesis de apertura");
            }
            else
            {
                throw new ArgumentNullException("esperabamos un parentesis de cierre");
            }
        }
        return Funcion;
    }
    

}
}

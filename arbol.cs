using System.Linq.Expressions;
using System.Security.Principal;
using AST;
using Tokenizador;
namespace Abol
{
 public  class Parser :token
 {
    private int position;
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
        }
        
        ErroresVariablesLocales();
        
        InferenciaSobrecarga(tokens);
        if(errors.Count == 0)return true;
        return false ;
    }
  //parsea el arbol 
   public void construir()
  {
    try
    {
     Parse();
     position = 0 ;
    }
    catch (Exception ex)
    {
     Console.WriteLine(ex.Message);
    }
   
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
    if (parentesis.Count > 0)
    throw new ArgumentException("esperabamos " + parentesis.Count + "(");
    if(auxiliar is LetIn || auxiliar  is Print || auxiliar is FunctionHulk && auxiliar.tokens.Count == 0 || auxiliar is IfElseNode)
    {
        tokens.Add(auxiliar);
    }
    else
    {
        variablesLocales.Add(auxiliar);
    }
   
  }
 //parsea las expresiones 
  private token ParseExpression()
  {
        token leftNode = ParseTerm();
        if (position > expression.Count- 1)
        {
           return leftNode ;
        }
        int bucle = 20 ;
        while (position < expression.Count)
        {
            bucle --;
            if(bucle == 0)return leftNode;
            string c = expression[position].Value;

            if (c == ";" || c == "," || c == "in" )
            {
                return leftNode;
            }
            //si encuentra una funcion
             else if(Isfunction(c))
            {
                position++;
                FunctionNode operatorNode = new FunctionNode(c,TokenTypes.Operator );
                operatorNode.tokens.Add(ParseExpression());
                if (position < expression.Count && (expression[position].Value == ")" || expression[position].Value == ";") )
                 return operatorNode ;
                 if (position > expression.Count - 1)
                 return operatorNode;
                 c =  expression[position].Value ;
                 if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" ||  c != "!=" || c != "==" || c != "&&" || c != "||"))
                 position++;
                leftNode = operatorNode;
            }
            //si encuentra un operador de estos
            else if (IsOperator(c))
            {
                 OperatorNode operatorNode = new OperatorNode( c,TokenTypes.Operator );
                 operatorNode.tokens.Add(leftNode);
                 position++;
                 operatorNode.tokens.Add(ParseExpression());
                if ( position < expression.Count  && (expression[position].Value == ")" || expression[position].Value == ";"))
                return operatorNode;
                if (position > expression.Count -1 )
                return operatorNode;
                if (position < expression.Count )
                {
                c = expression[position].Value;
                if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" ||  c != "!=" || c != "==" || c != "&&" || c != "||" ))
                position++;
                }
                leftNode = operatorNode;
              
            }   
            else if (c == "(")
            {
                return ParseTerm();
            }
            //si encuentra un operador de estos 
            else if (c == ">" || c == "<" || c == "<=" || c == ">=" ||  c == "!=" || c == "==" )
            {
                position++;
                token rigthNode = ParseExpression();
                tokenBul condicion = new tokenBul(c,TokenTypes.boolean);
                condicion.tokens.Add(leftNode);
                condicion.tokens.Add(rigthNode);
               if (position < expression.Count && (expression[position].Value == ")" || expression[position].Value == ";") )
                return condicion;
                if (position > expression.Count - 1)
                return condicion;
                c = expression[position].Value ; 
                if(aumentoContador() &&( c != ">" || c != "<" || c != "<=" || c != ">=" ||  c != "!=" || c != "==" || c != "&&" || c != "||"))
                position++;
                leftNode = condicion;
            }
            //si encuentra un operador de estos 
            else if(c == "&&" || c == "||" )
            {
                position++;
                tokenBul condicion = new tokenBul(c,TokenTypes.boolean);
                token rigthNode = ParseExpression();
                condicion.tokens.Add(leftNode);
                condicion.tokens.Add(rigthNode);
             if (position < expression.Count &&( expression[position].Value == ")" || expression[position].Value == ";") )
                return condicion;
                if (position > expression.Count- 1)
                return condicion;
                c = expression[position].Value;
                if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" ||  c != "!=" || c != "==" || c != "&&" || c != "||"))
                position++;
                leftNode = condicion;
            }
            else if ( c == ")" ||c == "else" || c== "=>")
            {
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
                if (position < expression.Count &&( expression[position].Value == ")" || expression[position].Value == ";"))
                return operatorNode;
                if(position > expression.Count - 1)
                return operatorNode;
                c = expression[position].Value;
                if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" ||  c != "!=" || c != "==" || c != "&&" || c != "||"))
                position++;
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
        if(position > expression.Count - 1 ) return node ;
        ///valor del token 
        string c  = expression[position].Value ; 
        //continuar el parseo
        if (c == "(" || c == "{" || c =="\"" || c == "="   || c == "=>"  )
        {
           if(c == "(")
            parentesis.Push("(");
            position++;
            node = ParseExpression();
            if(position < expression.Count - 1 && expression [position].Value == ")" )
            {
                try
                {
                     parentesis.Pop();
                     position++;
                }
                catch (System.Exception)
                {
                    throw new ArgumentException("esperabamos un )");
                }
              
            }
        }
        else if (position + 1 < expression.Count - 1 && (c == "function" || expression[position].Type ==  TokenTypes.Identifier && expression[position+1].Value == "("))
        {
           return FuncionesHulk();
        }
        else if (expression[position].Type == TokenTypes.Identifier)
        {
          return Identificador(c);
        }
         else if (double.TryParse(c,out double value))
        {
            position++;
            return new tokenNumero (c, TokenTypes.Number );
        }
        else if (c == "Print")
        {
            return parserPrint();
        }
       
        else if (c == "if")
        {
            return parserIFelse();
        }
        else if (c == "let")
        {
            return Letin();
        }
        else if (expression[position].Type == TokenTypes.Literal)
        {
            position++;
            return new tokenLiteral(expression[position - 1].Value , TokenTypes.Literal);
        }
        else if (Isfunction(c))
        {
             return ParseFunction(c);
        }
       
     return node ;
  }
    //Para recorrer las funciones
  private  token ParseFunction( string nombre )
  {
    FunctionNode funcion = new FunctionNode(nombre , TokenTypes.funcion);
    position++;
    if (expression[position].Value != "(")
    throw new ArgumentException("esperabamos un parentesis de apertura");
    parentesis.Push("(");
    position++;
    funcion.tokens.Add(ParseExpression());
    if (expression[position].Value != ")")
    throw new ArgumentException("esperabamos un parentesis de cierre");
    parentesis.Pop();
    position++;
    return funcion;     
  } 
  private token Identificador(string c)
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
  private token parserPrint()
  {       
        Print Print = new Print("Print" , TokenTypes.Keyword);
         position++;
         if (expression[position].Value != "(")
         throw new ArgumentException("esperabamos un parentesis de apertura");
         parentesis.Push("(");
         position++;
         Print.tokens.Add(ParseExpression());
         if (expression[position].Value != ")")
         throw new ArgumentException("esperabamos un parentesis de cierre");
        parentesis.Pop();
    
        if (position == expression.Count - 1 && expression[position].Value != ";")
        {
             throw new ArgumentException("esperabamos un ; ");
        }
        else if (position > expression.Count - 1)
        {
            throw new ArgumentException("esperabamos un ; ");
        }
        position++;
        return Print;
  }
  private token parserIFelse()
  {
       IfElseNode IfElse = new IfElseNode("if" , TokenTypes.boolean);
      // parsea la condicion del if else 
      position++;
      if (expression[position].Value != "(")
      throw new ArgumentException("esperabamos un (");
      else
      {
        parentesis.Push("(");
        position++;
      }
      token condicion = ParseExpression();
      if (condicion == null)
      throw new ArgumentException("la condicion del if no puede ser nula");
      IfElse.tokens.Add(condicion);
       if (expression[position].Value != ")")
       {
            throw new ArgumentException("esperabamos un (");
        }
        else
        {
            parentesis.Pop();
            position++;
        }
      //parsea el cuerpo then 
      token then = ParseExpression();
       if (then == null)
      throw new ArgumentException("la expresion then  del if no puede ser nula");
      IfElse.tokens.Add(then);
      //parsea el cuerpo Else
      if (expression[position].Value != "else")
      {
        throw new ArgumentException("esperabamos la palabra clave else");
      }
      position++;
      token Else = ParseExpression();
       if (Else == null)
      throw new ArgumentException("la expresion else del if no puede ser nula");
      if ( position > expression.Count - 1 )
       {
            throw new ArgumentException("esperabamos un ;");
       }
       if (position < expression.Count && expression[position].Value == ")" && parentesis.Count == 0)
       {
            throw new ArgumentException("esperabamos un (");
       }
      IfElse.tokens.Add(Else);
      
      return IfElse;
  }
   private token Letin()
  {
       LetIn let = new LetIn("let" , TokenTypes.Keyword , this);
       position++;
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
                        if ( let.position < expression.Count && expression[let.position].Value == ")")
                         {
                          throw new ArgumentException("esperabamos un (");
                          }
                         let.variablesLocales.Add(nodo);
                        if(expression[let.position].Value == ",")let.position++;
                    }
                    if (expression[let.position].Value == "in")break;
                    if(let.position > expression.Count - 1 && expression[let.position - 1].Value == ";" )
                    {
                        throw new ArgumentException("error en let - in ,esperabamos la instruccion in");
                    }
               }
                if (expression[let.position].Value == "in")
                {
                  let.position++;
                  let.tokens.Add(let.ParseExpression());
                if(let.position > expression.Count - 1)
                {
                throw new ArgumentException("esperabamos un ;");
                }
                if (expression[let.position].Value == ")")
                {
                throw new ArgumentException("esperabamos un (");
                }
                }
                if(expression[let.position].Value == ";")
                 position = let.position++;
                position = let.position;
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
        if(expression[position].Value != "(")
        {
            throw new ArgumentException("esprabamos un parentesis de apertura despues declarada la funcion");
        }
        else
        {
            parentesis.Push("(");
            position++;
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
        Funcion.parentesis = parentesis;
        while (expression[Funcion.position].Value != ")")
        {
        Funcion.variablesLocales.Add(Funcion.ParseExpression());
         if (expression[Funcion.position].Value == ";")return Funcion;
        if( expression[Funcion.position].Value == ")" )
        {
            Funcion.parentesis.Pop();
            Funcion.position++;
            break;
        }
        else if(expression[Funcion.position].Value == "," )
        Funcion.position++;
        else 
        {
            throw new ArgumentException("esperabamos un )");
        } 
      
        }
        if ( Funcion.position > expression.Count - 1 )
        {
            throw new ArgumentException("esperabamos un ;");
        }
        if( expression[Funcion.position].Value == ";")
        {
            position = Funcion.position++;
            return Funcion;
        }
          position = Funcion.position;
        
        // si la funcion sera definida 
        if(expression[Funcion.position].Value == "=>") Funcion.position++;
        // si la funcion solo fue llamada 
        else if(expression[Funcion.position].Value != "=>" )return Funcion;
        Funcion.tokens.Add(Funcion.ParseExpression());
          if (Funcion.position < expression.Count - 1 && expression[Funcion.position].Value == ")")
        {
            throw new ArgumentException("esperabamos un (");
        }
        position = Funcion.position;
        if(position > expression.Count - 1 && expression [Funcion.position - 1].Value != ";")
        throw new ArgumentException("esperabamos un ;");
        parentesis = Funcion.parentesis;
        return Funcion;
  }

   private void InferenciaSobrecarga(List <token> tokens)
   {
    if (tokens.Count == 0)return ;
       foreach (var item in tokens)
       {
            if (item is FunctionHulk)
            {
                bool sinError = true ;
                FunctionHulk auxiliar = (FunctionHulk)Valido((FunctionHulk)item);
                if(auxiliar != null)
                {
                    for (int i = 0; i < auxiliar.variablesLocales.Count; i++)
                    {
                        if(auxiliar.variablesLocales[i].TypeReturn != ((FunctionHulk)item).variablesLocales[i].TypeReturn && auxiliar.variablesLocales[i].TypeReturn != TokenTypes.Identifier && auxiliar.variablesLocales[i].TypeReturn != TokenTypes.comparacion )
                        {
                            sinError = false ;
                            errors.Add(new Errors(ErrorCode.Semantic , "En la funcion " + auxiliar.Value + " el parametro " + ((FunctionHulk)auxiliar).variablesLocales[i].Value + " recibe un tipo " + ((FunctionHulk)auxiliar).variablesLocales[i].TypeReturn));
                        }
                        else if(auxiliar.TypeReturn == TokenTypes.Identifier&&  item.TypeReturn != TokenTypes.Number && item.TypeReturn !=TokenTypes.Literal&& item.TypeReturn != TokenTypes.Identifier)
                        {
                            sinError = false ;
                            errors.Add(new Errors(ErrorCode.Semantic , "En la funcion " + auxiliar.Value + " el parametro " + ((FunctionHulk)auxiliar).variablesLocales[i].Value + " recibe un tipo Number o Literal "));
                        }
                    }
                
                 if (auxiliar.TypeReturn != TokenTypes.Identifier) 
                 item.TypeReturn = auxiliar.TypeReturn;
                 if (sinError)
                 {
                    ((FunctionHulk)item).FuncionGuardada = auxiliar.FuncionGuardada;
                 }
                }
            }
            else
            {
                InferenciaSobrecarga(item.tokens);
            }
        }
           
   }
   private FunctionHulk Valido (FunctionHulk item)
   {
     List <  FunctionHulk > FuncionesGuardadas = new List <FunctionHulk> ();
        for ( int i =  0 ; i < variablesLocales.Count ; i ++)
        {
            if (variablesLocales[i].Value == item.Value && variablesLocales[i] is FunctionHulk && ((FunctionHulk)variablesLocales[i]).variablesLocales.Count == item.variablesLocales.Count )
            {
                ((FunctionHulk)variablesLocales[i]).FuncionGuardada = i ;
                FuncionesGuardadas.Add((FunctionHulk)variablesLocales[i]);
            }
        }
        if (FuncionesGuardadas.Count == 1)
        return FuncionesGuardadas[0];
        else if (FuncionesGuardadas.Count > 1)
        {
            bool correctos = true;
            foreach (FunctionHulk items in FuncionesGuardadas)
            {
                correctos = true;
                for (int i = 0; i < item.variablesLocales.Count; i++)
                {
                    if(items.variablesLocales[i].TypeReturn != TokenTypes.Identifier && items.variablesLocales[i].TypeReturn != item.variablesLocales[i].TypeReturn )
                    {
                        correctos = false;
                        break;
                    }
                    else if(items.variablesLocales[i].TypeReturn == TokenTypes.Identifier && item.variablesLocales[i].TypeReturn != TokenTypes.Literal && item.variablesLocales[i].TypeReturn != TokenTypes.Number && item.variablesLocales[i].TypeReturn != TokenTypes.Identifier)
                    {
                        correctos = false;
                        break ;
                    }
                }
                 if (correctos)
                {
                    if (items.TypeReturn != TokenTypes.Identifier) 
                    item.TypeReturn = items.TypeReturn;
                    return items;
                }
                
            }
            if (!correctos)
            {
                errors.Add(new Errors (ErrorCode.Semantic , "no existe una funcion en este contexto , q reciba parametros con esos tipos"));
                return null;
            }
        }
        else 
        {
             errors.Add(new Errors (ErrorCode.Semantic , "no existe la funcion "+ item.Value + " en este contexto que cumpla con los parametros"));
            
        }
    return null;
   }
   private void ErroresVariablesLocales()
   {
    bool Sobrecarga = true ;
    
    for (int i = 0; i < variablesLocales.Count; i++)
    {   
        
        for (int j = i + 1; j < variablesLocales.Count; j++)
        {
            if (variablesLocales[i] is FunctionHulk && variablesLocales[j] is FunctionHulk && ((FunctionHulk)variablesLocales[i]).variablesLocales.Count == ((FunctionHulk)variablesLocales[j]).variablesLocales.Count)
            {
                int count = ((FunctionHulk)variablesLocales[i]).variablesLocales.Count;
                int count1 = ((FunctionHulk)variablesLocales[j]).variablesLocales.Count;
                for (int k = 0; k < ((FunctionHulk)variablesLocales[i]).variablesLocales.Count ; k++)
                {
                    if (((FunctionHulk)variablesLocales[i]).variablesLocales[k].TypeReturn != ((FunctionHulk)variablesLocales[j]).variablesLocales[k].TypeReturn)
                    {
                        Sobrecarga = false;
                        break;
                    }
                }
                if(Sobrecarga && count == count1)
                {
                    errors.Add(new Errors (ErrorCode.Semantic ,"la funcion " + variablesLocales[i].Value + " fue definida mas de una vez en este contexto"));
                }
            }
        }
    }
   }
   private bool aumentoContador()
   {
    return position < expression.Count - 1 && expression[position].Value != ";" && !IsOperator(expression[position].Value) && !Isfunction(expression[position].Value) && expression[position].Value != "in" && expression[position].Value != "else" && expression[position].Value == "," && expression[position].Value != ";" ;
   }
 }
}


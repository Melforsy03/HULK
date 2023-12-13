using Abol;
using Tokenizador;
namespace AST
{
    //tipos de token 
 public enum TokenTypes
{
    Keyword , Identifier ,Number, Operator, Punctuation ,Literal  , funcion , boolean, letIn , ErrorType
} 
//nodo token con valor string y tipo de token 
public abstract class metodo
{
    public abstract string Evaluar();
}
public class token : metodo , ICloneable
{
    public List<token> tokens {get; set;}
    public string Value { get; set; }
    public TokenTypes Type { get; set; }

    public TokenTypes  TypeReturn {get ; set ;}
    public token(string value, TokenTypes type)
    {
        Value = value;
        Type = type;
        tokens = new List<token>();
    }
public object Clone ()
{
   if(this is LetIn) return ((LetIn)this).Clone();
    else if(this is Print)return ((Print)this).Clone();
    else if (this is OperatorNode)return ((OperatorNode)this).Clone();
    else if (this is Identificador) return ((Identificador)this).Clone();
    else if (this is tokenNumero)return ((tokenNumero)this).Clone();
    else if (this is tokenBul) return ((tokenBul)this).Clone();
    else if(this is IfElseNode) return ((IfElseNode)this).Clone();
    else if (this is tokenLiteral)return ((tokenLiteral)this).Clone();
    else
    {
      return ((FunctionHulk)this).Clone();
    }
}
 public override string Evaluar()
 {
    if(this is LetIn) return ((LetIn)this).Evaluar();
    else if(this is Print)return ((Print)this).Evaluar();
    else if (this is OperatorNode)return ((OperatorNode)this).Evaluar().ToString();
    else if (this is Identificador) return ((Identificador)this).Evaluar();
    else if (this is tokenNumero)return ((tokenNumero)this).Evaluar().ToString();
    else if (this is tokenBul) return ((tokenBul)this).Evaluar().ToString();
    else if(this is IfElseNode) return ((IfElseNode)this).Evaluar();
    else if (this is tokenLiteral)return ((tokenLiteral)this).Evaluar();
    else
    {
      return ((FunctionHulk)this).Evaluar();
    }
    
 }
 public bool CheckSemantic(List<Errors> errors)
 {
    if(this is LetIn) return ((LetIn)this).CheckSemantic(errors);
    else if(this is Print)return ((Print)this).CheckSemantic(errors);
    else if (this is OperatorNode)return ((OperatorNode)this).CheckSemantic(errors);
    else if (this is Identificador) return ((Identificador)this).CheckSemantic(errors);
    else if (this is tokenNumero)return ((tokenNumero)this).CheckSemantic(errors);
    else if (this is tokenBul) return ((tokenBul)this).CheckSemantic(errors);
    else if(this is IfElseNode) return ((IfElseNode)this).CheckSemantic(errors);
    else if (this is tokenLiteral)return ((tokenLiteral)this).CheckSemantic(errors);
    else
    {
      return ((FunctionHulk)this).CheckSemantic(errors);
    }
 }
}
//token print 
public class Print :token 
{
  public Print(string Value , TokenTypes type ) : base (Value,type){}

   public string Evaluar()
  {
    if (tokens.Count == 0)
    {
        return "";
    }
    return tokens[0].Evaluar(); 
  }
    public Print Clone ()
  {
    Print objeto = new Print(this.Value , this.Type);
    foreach (var item in this.tokens)
    {
      objeto.tokens.Add((token)item.Clone());
    }
    return objeto;
  }
  public bool CheckSemantic(List<Errors> errors)
  {
   
    bool error = tokens[0].CheckSemantic(errors);
    if (!error)
    {
    errors.Add(new Errors(ErrorCode.Semantic , "error en la funcion Print"));
       TypeReturn = TokenTypes.ErrorType;
       return false ;
    }
    TypeReturn = tokens[0].TypeReturn;
    return true ;
    
  }
}
//token indentificador de variables 
//retorna el valor del toquen almacenado 
public class Identificador : token  ,ICloneable
{
    
    //public int position{get ;set;}
    public Identificador(string Value , TokenTypes type) :base(Value , type ){}
    public string Evaluar ()
    {
      try
      {
         return tokens[0].Evaluar().ToString();
      }
      catch(Exception x)
      {
        throw new ArgumentException("esta variables no contine ningun valor");
         
      }
      
    }

   public Identificador Clone ()
  {
    Identificador objeto = new Identificador(this.Value , this.Type);
    foreach (var item in this.tokens)
    {
      objeto.tokens.Add((token)item.Clone());
    }
    return objeto;
  }
  public bool CheckSemantic(List<Errors> errors)
  {
      bool check = false ;
   
      check = tokens[0].CheckSemantic(errors);
    
      if (!check)
    {
      errors.Add(new Errors (ErrorCode.Semantic , "error en la variable " + Value ));
      TypeReturn = TokenTypes.ErrorType;
      return false ;
    }
    if (tokens.Count > 0)TypeReturn = tokens[0].TypeReturn;
    else {TypeReturn = TokenTypes.Identifier;}
    return true ;
  }
}
//token numero que tiene tipo numero y  un valor que es un numero 
public class tokenNumero: token
{
    public tokenNumero (string Value , TokenTypes type) : base(Value , type){}

    public double Evaluar()
    {
        return double.Parse(Value);
    }
   public tokenNumero Clone ()
  {
    tokenNumero objeto = new tokenNumero(this.Value , this.Type);
    
    return objeto;
  }
  public bool CheckSemantic(List<Errors> errors)
  {
    TypeReturn = TokenTypes.Number;
    return true ;
  }
}
public class tokenLiteral : token 
{
    public tokenLiteral(string value , TokenTypes types) :base(value ,types ){}

    public string Evaluar()
    {
        return Value;
    }
    public tokenLiteral Clone ()
   {
    tokenLiteral objeto = new tokenLiteral(this.Value , this.Type);
    
    return objeto;
   }
   public bool CheckSemantic(List<Errors> errors)
   {
    TypeReturn = TokenTypes.Literal;
    return true ;
   }
}

//token condicional que tiene dos valores if y else , es tipo condicional 
//tiene un token booleano , un token veradero , un token falso 
public class IfElseNode : token
{
    public IfElseNode(string Value ,  TokenTypes Type ) : base(Value , Type ){}
  
    public string Evaluar()
    {
        if (((tokenBul)tokens[0]).Evaluar())
        {
        return tokens[1].Evaluar().ToString();
        }
        else
        {
         return tokens[2].Evaluar().ToString();
        }

    }
    public IfElseNode Clone ()
  {
    IfElseNode objeto = new IfElseNode(this.Value , this.Type);
    foreach (var item in this.tokens)
    {
      objeto.tokens.Add((token)item.Clone());
    }
    return objeto;
  }
  public bool CheckSemantic(List<Errors> errors)
  {
    bool condicion = false;
    bool then = false;
    bool elsse = false ;

    condicion = tokens[0].CheckSemantic(errors);
    if(!condicion || tokens[0].TypeReturn == TokenTypes.boolean)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la condicion de la expresion if - else"));
         TypeReturn = TokenTypes.ErrorType;
    }
    then = tokens[1].CheckSemantic(errors);
     if(!then)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la instruccion then de la expresion if - else"));
         TypeReturn = TokenTypes.ErrorType;
    }
    elsse = tokens[2].CheckSemantic(errors);
     if(!elsse)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la instruccion else de la expresion if - else"));
         TypeReturn = TokenTypes.ErrorType;
    }
    if(!(condicion && then && elsse))
    {
    return true;
    }
    else
    {
      return false;
    }
   
  }
}
//token operador + ,- ,* , / ,^ metodo evaluar que devuelve un double 
public class OperatorNode : token
{
     public OperatorNode (string Operator , TokenTypes type) : base(Operator , type){}

    public double Evaluar()
    {
      try
      {
       if (Value == "+")
       {
         return double.Parse(tokens[0].Evaluar()) + double.Parse(tokens[1].Evaluar()) ;  
       }
       else if (Value == "-")
       {
        return double.Parse(tokens[0].Evaluar()) - double.Parse(tokens[1].Evaluar()) ;     
       }
       else if (Value == "*")
       {
        return double.Parse(tokens[0].Evaluar()) * double.Parse(tokens[1].Evaluar()) ;  
       }
       else if (Value == "/")
       {
        return double.Parse(tokens[0].Evaluar()) / double.Parse(tokens[1].Evaluar()) ;  
       }
        else if (Value == "%")
       {
       return double.Parse(tokens[0].Evaluar()) % double.Parse(tokens[1].Evaluar()) ;  
       }
       else 
       {
        return Math.Pow(double.Parse(tokens[0].Evaluar()) , double.Parse(tokens[1].Evaluar())) ; 
       }
        }
      catch (System.Exception)
      {
        throw new ArgumentException("el operador " + Value + " no puede operar con estos elementos");
        
      }
    }
    public OperatorNode Clone ()
  {
    OperatorNode objeto = new OperatorNode(this.Value , this.Type);
    foreach (var item in this.tokens)
    {
      objeto.tokens.Add((token)item.Clone());
    }
    return objeto;
  }
  public bool CheckSemantic(List<Errors> errors)
  {
    bool check = false ;
    bool check1 = false;
     if(tokens.Count == 0)
    {
        errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value + " parametros incorrectos" ));
        TypeReturn = TokenTypes.ErrorType;
        return false ;
    }
    check = tokens[0].CheckSemantic(errors);
     if(!check)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value ));
       TypeReturn = TokenTypes.ErrorType;
      
    }
     check1 = tokens[1].CheckSemantic(errors);
     if(!check1)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value ));
       TypeReturn = TokenTypes.ErrorType;
      
    }
    if(tokens[0].Type == TokenTypes.Identifier)
    {
      tokens[0].TypeReturn = tokens[1].TypeReturn;
    }
    else if (tokens[1].Type == TokenTypes.Identifier)
    {
      tokens[1].TypeReturn = tokens[0].TypeReturn;
    }
     if(tokens[0].TypeReturn != tokens[1].TypeReturn)
    {
       errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value + " no se puede efectuar esta operacion sobre tipos diferentes"));
        TypeReturn = TokenTypes.ErrorType;
        return false ;
    }
    if((check && check1))
    {
      TypeReturn = tokens[0].TypeReturn;
      return true ;
    }
    else
    {
         TypeReturn = TokenTypes.ErrorType;
         return false;
    }
  }

}
//un token valor que tiene un valor que en este caso es el operador , es tipo booleano 
//tiene dos token numero 
public class  tokenBul : token
{
   public tokenBul(string Value , TokenTypes type) : base(Value , type){}

   public bool Evaluar()
  {
    if (Value == "&&")
    {
    return ((tokenBul)tokens[0]).Evaluar() && ((tokenBul)tokens[1]).Evaluar();
    }
    else if (Value == "||")
    {
    return ((tokenBul)tokens[0]).Evaluar() || ((tokenBul)tokens[1]).Evaluar();
    }
    else if (Value == "!=")
    {
    return double. Parse(tokens[0].Evaluar()) != double.Parse(tokens[1].Evaluar()); 
    }
    else if (Value == ">")
    {
     return double. Parse(tokens[0].Evaluar()) > double.Parse(tokens[1].Evaluar());
    }
    else if (Value == "<" )
    {
    return double. Parse(tokens[0].Evaluar()) < double.Parse(tokens[1].Evaluar());
    }
    else if (Value == "==" )
    {
    return double. Parse (tokens[0].Evaluar()) == double.Parse(tokens[1].Evaluar());
    }
    else if (Value == ">=" )
    {
     return double. Parse (tokens[0].Evaluar()) >= double.Parse(tokens[1].Evaluar());
    }
    else  
    {
     return double. Parse (tokens[0].Evaluar()) <= double.Parse(tokens[1].Evaluar());
    }
    
  }
  public tokenBul Clone ()
  {
    tokenBul objeto = new tokenBul(this.Value , this.Type);
    foreach (var item in this.tokens)
    {
      objeto.tokens.Add((token)item.Clone());
    }
    return objeto;
  }
  public bool CheckSemantic(List<Errors> errors)
  {
    bool check = false ;
    bool check1 = false;
    check = tokens[0].CheckSemantic(errors);
    if(tokens.Count != 0)
    {
        errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value + " parametros incorrectos" ));
        return false ;
    }
     if(!check)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value ));
    }
     check1 = tokens[1].CheckSemantic(errors);
     if(!check1)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value ));
      
    }
    else if(tokens[0].Type != tokens[1].Type)
    {
       errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value + " no se puede efectuar esta operacion sobre tipos diferentes"));
    }
    
    if(!(check && check1))
    {
      TypeReturn = tokens[0].TypeReturn;
      return true ;
    }
    else
    {
         TypeReturn = TokenTypes.ErrorType;
         return false ;
    }
  }
}
//esta clase es para parsear expresiones aritmeticas dado una lista de tokens 
public class FunctionNode : token
{
    public string FunctionName { get; set; }
    public FunctionNode (string FunctionName , TokenTypes type ):base (FunctionName , type){}
    
    public double Evaluar()
    {
        // Evaluar la función según el nombre
       if (Value == "sin")
       {
        return Math.Sin(Double.Parse(tokens[0].Evaluar()));
       }
       else if (Value == "cos")
       {
        return Math.Cos(Double.Parse(tokens[0].Evaluar()));
       }
         else if (Value == "tan")
       {
        return Math.Tan(Double.Parse(tokens[0].Evaluar()));
       }
        else 
       {
       return Math.Sqrt(Double.Parse(tokens[0].Evaluar()));
       }
    }
    public FunctionNode Clone ()
    {
        FunctionNode objeto = new FunctionNode(this.Value , this.Type);
        foreach (var item in this.tokens)
        {
          objeto.tokens.Add((token)item.Clone());
        }
        return objeto;
    }
    public bool CheckSemantic(List<Errors> errors)
    {
      bool check = tokens[0].CheckSemantic(errors);
      if(!check || tokens[0].TypeReturn != TokenTypes.Number)
      {
        TypeReturn = TokenTypes.ErrorType;
        return false ;
      }
      else
      {
        TypeReturn = TokenTypes.Number;
        return true ;
      }
    }
}
    
public class FunctionHulk :Parser
{
    List<token> parametros {get ; set ;}
    public FunctionHulk(String Value , TokenTypes Type , Parser root) : base(Value , Type , root)
    {
        parametros = new List<token>();
    }
    public FunctionHulk Clone ()
    {
      FunctionHulk objeto = new FunctionHulk(this.Value , this.Type , this.Root);
      objeto.tokens = new List<token>();
      foreach (token item in this.tokens)
      {
        objeto.tokens.Add((token)item.Clone());
      }
      objeto.variablesLocales = new List<token>();
      foreach (var item in this.variablesLocales)
      {
        objeto.variablesLocales.Add((token)item.Clone());
      }
      objeto.variablesGlobales = new List<token>();
      foreach (var item in this.variablesGlobales)
      {
        objeto.variablesGlobales.Add((token)item.Clone());
      }
     
      return objeto;
    }
   public string Evaluar()
    {
      if (Root.variablesLocales.Any(valor => valor .Value ==  Value))
      {
        FunctionHulk actual = (FunctionHulk)Root.variablesLocales.Find(valor => valor.Value == Value).Clone();
        actual.variablesGlobales = new List<token>(Root.variablesLocales);
        actual.tokens = CambioDPadre(actual.tokens , actual);
        for (int i = 0; i < variablesLocales.Count; i++)
        {
          actual.variablesLocales[i].tokens.Add(variablesLocales[i]);
        }
        actual.CambioDevariable2(actual.tokens , actual.variablesLocales);
        return actual.tokens[0].Evaluar();
      }
      else if (Root.variablesGlobales.Any(valor => valor.Value ==  Value ))
      {
        FunctionHulk actual = (FunctionHulk)((FunctionHulk)Root.variablesGlobales.Find(valor => valor.Value == Value)).Clone();
         actual.tokens = CambioDPadre(actual.tokens , actual);
         actual.variablesGlobales.Add((FunctionHulk)Root.variablesGlobales.Find(valor => valor.Value ==  Value).Clone());

        for (int i = 0; i < variablesLocales.Count; i++)
        {
        // actual.CambioDevariable(Root.variablesLocales[i] , variablesLocales[i]);
         actual.variablesLocales[i].tokens.Add(actual.CambioDevariable(Root.variablesLocales[i] , variablesLocales[i]).tokens[0]);
        
        }
         actual.CambioDevariable2(actual.tokens, actual.variablesLocales);
        return actual.tokens[0].Evaluar();
      }
      else
      {
        throw new ArgumentException("esta funcion no fue definida en este lenguaje");
      }
      
    }
    private token CambioDevariable(token token , token varible)
    {
      if (varible.tokens.Count == 0)
      {
        token.tokens = new List<token>();
        token.tokens.Add(varible);
        return token;
      }
      token auxiliar = (token)varible.Clone();
      for (int i = 0; i < varible.tokens.Count; i++)
      {
        if(varible.tokens[i].Value == token.Value)
        {
          auxiliar.tokens[i] = (token)token.Clone();
        }
      }
       token auxiliar2 = (token)token.Clone();
        auxiliar2.tokens = new List<token>();
        auxiliar2.tokens.Add(new tokenNumero (auxiliar.Evaluar().ToString(), TokenTypes.Number));
        return auxiliar2;

    }
    private List<token> CambioDevariable2(List<token> cambio, List<token> parametros)
    {
      if (cambio.Count == 0)
      {
        return cambio ;
      }
      for (int i = 0; i < cambio.Count; i++)
      {
        if (parametros.Any(valor => valor.Value == cambio[i].Value))
        {
          cambio[i].tokens = new List<token>();
          cambio[i].tokens.Add((token)parametros.Find(valor=> valor.Value == cambio[i].Value).tokens[0].Clone());
        }
        else
        {
          CambioDevariable2(cambio[i].tokens, parametros);
        }
      }
      return cambio;
    }
    private List<token>  CambioDPadre (List<token> hijos ,FunctionHulk actual)
    {
      for (int i = 0; i < hijos.Count; i++)
      {
        if (hijos[i].Value == actual.Value)
        {
          ((FunctionHulk)hijos[i]).Root =  actual;
        }
        else
        {
          CambioDPadre(hijos[i].tokens,actual);
        }
      }
      return hijos ;
    }
    public bool CheckSemantic(List<Errors> errors)
    {
      bool cuerpo = false;
      bool variablesL = true;
      bool variablesG = true ;
      cuerpo = tokens[0].CheckSemantic(errors);
      if(!cuerpo)
      {
        errors.Add(new Errors(ErrorCode.Semantic , "error en la funcion " + Value ));
      
      }
     
      foreach (var item in variablesLocales)
      {
        variablesL = variablesL && item.CheckSemantic(errors);
        if (!variablesL)
        {
          errors.Add(new Errors(ErrorCode.Semantic , "error en la funcion " + Value + "error en las variables locales"));
        }
      }
      foreach (var item1 in variablesLocales)
      {
        foreach (var item in tokens)
       {
        if(item1.Value == item.Value) item1.TypeReturn = item.TypeReturn;
        break;
       }
      }
    if(cuerpo && variablesG)
    {
      TypeReturn = tokens[0].TypeReturn;
      return true ;
    }
    else
    {
      TypeReturn = TokenTypes.ErrorType;
      return false ;
    }
  }

}
public class LetIn : Parser
{
    public LetIn(string Value, TokenTypes type, Parser Padre) : base(Value, type, Padre)
    {
      this.Value = "let";
      this.Type = TokenTypes.letIn;
    }
  
    public  bool CheckSemantic(List<Errors> errores)
    {
      bool expresion_let = true;
      bool expresion_in = true;
      for (int i = 0; i < variablesLocales.Count; i++)
      {
        expresion_let = expresion_let && variablesLocales[i].CheckSemantic(errores);
      }

      for (int i = 0; i < tokens.Count; i++)
      {
        expresion_in = expresion_in && tokens[i].CheckSemantic(errores);
      }
      if (!expresion_let)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "hay error en la expresion let - in , error en la expresion let "));
        TypeReturn = TokenTypes.ErrorType;
      }
      if(!expresion_in)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "hay error en la expresion let - in , error en la expresion let "));
        TypeReturn = TokenTypes.ErrorType;
      }
     
      if (!(expresion_in && expresion_let))
      {
        return false ;
      }
      return true;
    }
    public LetIn Clone ()
    {
      LetIn objeto = new LetIn(this.Value , this.Type , this.Root);
      objeto.tokens = new List<token>();
      foreach (token item in this.tokens)
      {
        objeto.tokens.Add((token)item.Clone());
      }
      objeto.variablesLocales = new List<token>();
      foreach (var item in this.variablesLocales)
      {
        objeto.variablesLocales.Add((token)item.Clone());
      }
      objeto.variablesGlobales = new List<token>();
      foreach (var item in this.variablesGlobales)
      {
        objeto.variablesGlobales.Add((token)item.Clone());
      }
      
      return objeto;
    }
    public string Evaluar ()
    {
      return tokens[0].Evaluar();
    } 
}
}










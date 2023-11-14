using Abol;
namespace AST
{
    //tipos de token 
 public enum TokenTypes
{
    Keyword , Identifier ,Number, Operator, Punctuation ,Literal ,Condicional , funcion , boolean, letIn , parameter
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

    public token(string value, TokenTypes type)
    {
        Value = value;
        Type = type;
        tokens = new List<token>();
    }
public object Clone ()
{
    if (this.Type == TokenTypes.Identifier)
    {
    return new identificador (this.Value , TokenTypes.Identifier)
    {
      
    };
    }
    else if (this is tokenNumero)
    {
        return new tokenNumero(this.Value , TokenTypes.Number)
        {
            
        };
    }
    else if (this is tokenLiteral)
    {
        return new tokenLiteral(this.Value , this.Type)
        {
        
        };
    }
    else if (this.Type == TokenTypes.funcion)
    {
        return new Function(this.Value , this.Type)
        {
        
        };
    }
     else if (this is IfElseNode)
    {
        return new IfElseNode(this.Value , this.Type)
        {
            
        };
    }
    else if(this is tokenBul)
    {
       return new tokenBul(this.Value , this.Type)
        {
            
        };
    }
    else if(this is Print)
    {
       return new Print(this.Value , this.Type)
        {
        
        };
    }
    else if(this is OperatorNode)
    {
       return new OperatorNode(this.Value , this.Type)
        {
            
        };
    }
    else if (this.Value == "else")
    {
        return new token(this.Value , this.Type)
        {
            
        };
    }
    return null;
}

 public override string Evaluar()
 {
    if (this is tokenNumero)
    {
        return Value;
    }
     else if(this.Type is TokenTypes.Condicional)
    {
        return ((tokenBul)this).Evaluar().ToString();
    }
    else if (this is Function)
    {
    return ((Function)this).Evaluar().ToString();
    }
    else if(this is Print)
    {
        return ((Print)this).Evaluar().ToString();
    }
     else if(this.Type is TokenTypes.Operator)
    {
        return ((OperatorNode)this).Evaluar().ToString();
    }
    else if (this is identificador)
    {
        return ((identificador)this).Evaluar().ToString();
    }
    else if (this is tokenLiteral)
    {
        return this.Value;
    }
    if (tokens[0].Type == TokenTypes.Number)
    {
        return ((tokenNumero)tokens[0]).Evaluar().ToString();
    }
     else if (tokens[0].Type == TokenTypes.Operator)
    {
         return ((OperatorNode)tokens[0]).Evaluar().ToString() ;
    }
   
   else if (tokens[0].Type == TokenTypes.funcion && tokens[0] is Function)
    {
        return ((Function)tokens[0]).Evaluar().ToString();
    }
    else if (tokens[0].Type == TokenTypes.funcion && tokens[0] is FunctionNode)
    {
        return ((FunctionNode)tokens[0]).Evaluar().ToString();
    }
    else if (tokens[0].Type == TokenTypes.Condicional)
    {
        return ((tokenBul)tokens[0]).Evaluar().ToString();
    }
    else if(tokens[0].Type == TokenTypes.Identifier)
    {
        return ((identificador)tokens[0]).Evaluar().ToString();
    }
    else if (tokens[0].Value == "Print")
    {
        return tokens[0].Evaluar();
    }
    else if(tokens[0] is tokenBul)
    {
        return ((tokenBul)tokens[0]).Evaluar().ToString();
    }
    else if (tokens[0].Type is TokenTypes.Literal)
    {
        return tokens[0].Value ;
    }
    else if(this.Value == "else" )
    {
        return tokens[0].Evaluar().ToString();
    }
    return null;
 }
}
//token print 
public class Print :token 
{
public Print(string Value , TokenTypes type ) : base (Value,type){}

  public string Evaluar()
{
    return tokens[0].Evaluar(); 
}
}
//token indentificador de variables 
//retorna el valor del toquen almacenado 
public class identificador : token  ,ICloneable
{
    
    //public int position{get ;set;}
    public identificador(string Value , TokenTypes type) :base(Value , type ){}
    public string Evaluar ()
    {
     return tokens[0].Evaluar().ToString();
  
    }
    public Object Clone ()
    {
        return new identificador(Value , TokenTypes.Identifier)
        {
           
        };
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
}
public class tokenLiteral : token 
{
    public tokenLiteral(string value , TokenTypes types) :base(value ,types ){}

    public string evaluar()
    {
        return Value;
    }
}

//token condicional que tiene dos valores if y else , es tipo condicional 
//tiene un token booleano , un token veradero , un token falso 
public class IfElseNode : token
{
   
    public IfElseNode(string Value ,  TokenTypes Type ) : base(Value , Type)
    {
    }
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
}
//token operador + ,- ,* , / ,^ metodo evaluar que devuelve un double 
public class OperatorNode : token
{
     public OperatorNode (string Operator , TokenTypes type) : base(Operator , type){}

    public double Evaluar()
    {
        // Evaluar la operación según el operador        
            
       if (Value == "+")
       {
        
            try
            {
                return double.Parse(tokens[0].Evaluar()) + double.Parse(tokens[1].Evaluar()) ;  
            }
            catch (System.Exception)
            {
                
                Console.WriteLine(" error en ejecucuion ,el operador" +  Value + " no puede operar con estos elementos");
            }

       }
       else if (Value == "-")
       {
         try
            {
                return double.Parse(tokens[0].Evaluar()) - double.Parse(tokens[1].Evaluar()) ;  
            }
            catch (System.Exception)
            {
                
                Console.WriteLine(" error en ejecucuion ,el operador" +  Value + " no puede operar con estos elementos");
            }           
            
       }
       else if (Value == "*")
       {
            try
            {
                return double.Parse(tokens[0].Evaluar()) * double.Parse(tokens[1].Evaluar()) ;  
            }
            catch (System.Exception)
            {
                
                Console.WriteLine(" error en ejecucuion ,el operador " + Value + " no puede operar con estos elementos");
            }
       }
       else if (Value == "/")
       {
            try
            {
                return double.Parse(tokens[0].Evaluar()) / double.Parse(tokens[1].Evaluar()) ;  
            }
            catch (System.Exception)
            {
                Console.WriteLine(" error en ejecucuion ,el operador " + Value + " no puede operar con estos elementos");
            }
        
       }
       else if (Value == "^")
       {
      try
      {
        return Math.Pow(double.Parse(tokens[0].Evaluar()) , double.Parse(tokens[1].Evaluar())) ; 
      }
      catch (System.Exception)
      {
        
         Console.WriteLine("error en ejecucuion , el operador " + Value + "no opera con esos elementos");
       }
             
       }
       else if (Value == "Sqrt")
       {
        try
        {
        return Math.Sqrt(double.Parse(tokens[0].Evaluar()));
        }
        catch (System.Exception)
        {
            
            throw;
        }
       }
     throw new ArgumentException (" error en ejecucuion ,no se pudo ejecutar esta operacion");
}
}
//un token valor que tiene un valor que en este caso es el operador , es tipo booleano 
//tiene dos token numero 
public class  tokenBul : token
{
   public tokenBul(string Value , TokenTypes type) : base(Value , type){}

   public bool Evaluar()

{
    bool resultado = false;

    if (Value == "&&")
    {
       try
       {
          return ((tokenBul)tokens[0]).Evaluar() && ((tokenBul)tokens[1]).Evaluar();
       }
       catch (System.Exception)
       {
                Console.WriteLine("error ne ejecucion , el operador " + Value + " no opera con estos elementos");
       }
       
    }
    else if (Value == "||")
    {
         try
       {
          return ((tokenBul)tokens[0]).Evaluar() || ((tokenBul)tokens[1]).Evaluar();
       }
       catch (System.Exception)
       {
                Console.WriteLine("error ne ejecucion , el operador " + Value + " no opera con estos elementos");
       }
        
    }
    else if (Value == "!=")
    {
       try
       {
         return double. Parse(tokens[0].Evaluar()) != double.Parse(tokens[1].Evaluar());
       }
       catch (System.Exception)
       {
        Console.WriteLine("error ne ejecucion , el operador " + Value + " no opera con estos elementos");
       }  
    }
    else if (Value == ">")
    {
       try
       {
         return double. Parse(tokens[0].Evaluar()) > double.Parse(tokens[1].Evaluar());
       }
       catch (System.Exception)
       {
        Console.WriteLine("error ne ejecucion , el operador " + Value + " no opera con estos elementos");
       }  
    }
    else if (Value == "<" )
    {
       try
       {
         return double. Parse(tokens[0].Evaluar()) < double.Parse(tokens[1].Evaluar());
       }
       catch (System.Exception)
       {
        Console.WriteLine("error en ejecucion , el operador " + Value + " no opera con estos elementos");
       }  
    }
    else if (Value == "==" )
    {
       
        try
        {
            return double. Parse (tokens[0].Evaluar()) == double.Parse(tokens[1].Evaluar());
        }
        catch (System.Exception)
        {
         Console.WriteLine("error en ejecucion , el operador " + Value + " no opera con estos elementos");
        }
       

       
    }
    else if (Value == ">=" )
    {
       try
        {
            return double. Parse (tokens[0].Evaluar()) >= double.Parse(tokens[1].Evaluar());
        }
        catch (System.Exception)
        {
         Console.WriteLine("error en ejecucion , el operador " + Value + " no opera con estos elementos");
        }
    }
    else if (Value == "<=" )
    {
     try
        {
            return double. Parse (tokens[0].Evaluar()) <= double.Parse(tokens[1].Evaluar());
        }
        catch (System.Exception)
        {
         Console.WriteLine("error en ejecucion , el operador " + Value + " no opera con estos elementos");
        }
    }
    throw new ArgumentException("error en ejecucuion ,no se ha podido ejecutar el codigo");
}
    
}
//esta clase es para parsear expresiones aritmeticas dado una lista de tokens 
public class FunctionNode : token
{
    public string FunctionName { get; set; }

    public FunctionNode (string FunctionName , TokenTypes type ):base (FunctionName , type){}
    public  tokenNumero Argument { get; set; }

    public double Evaluar()
    {
        // Evaluar la función según el nombre
       if (Value == "sin")
       {
                if (tokens[0].Type == TokenTypes.Operator)
                {
                return Math.Sin(((OperatorNode)tokens[0]).Evaluar());
                }
                else
                {
                    return Math.Sin(Double.Parse(tokens[0].Evaluar()));
                }
       }
       else if (Value == "cos")
       {
         if (tokens[0].Type == TokenTypes.Operator)
                {
                return Math.Cos(((OperatorNode)tokens[0]).Evaluar());
                }
                else
                {
                    return Math.Cos(Double.Parse(tokens[0].Evaluar()));
                }
       }
         else if (Value == "tan")
       {
         if (tokens[0].Type == TokenTypes.Operator)
                {
                return Math.Tan(((OperatorNode)tokens[0]).Evaluar());
                }
                else
                {
                    return Math.Tan(Double.Parse(tokens[0].Evaluar()));
                }
       }
        else if (Value == "sqrt")
       {
         if (tokens[0].Type == TokenTypes.Operator)
                {
                return Math.Sqrt(((OperatorNode)tokens[0]).Evaluar());
                }
                else
                {
                    return Math.Sqrt(Double.Parse(tokens[0].Evaluar()));
                }
       }   
                throw new InvalidOperationException("Función no válida");
    }
}
    
public class Function :token , ICloneable
{
    public List<token> globales {get ; set ;}
    public Function(String Value , TokenTypes Type) : base(Value , Type)
    {
           globales = new List<token>();
    }
    public token parametro {get ; set ;}
    public List<token> CambioV(List<token> c , List<token> parameter , List<token> auxiliar , token alfa)
    {
        if (c.Count == 0 || c == null )
        {
            return c;
        }
        if(c.Count == 1)
        {
        if(  c[0].Type == TokenTypes.Number || c[0].Type == TokenTypes.Literal)
        {
            auxiliar.Add(c[0]);
             alfa.tokens = auxiliar;
             return auxiliar;
        }
        }
        for (int i = 0; i < c.Count; i++)
        {
            if ( c[i]!= null )
            {
                string a = c[i].Value ;
                token  buscar = parameter.FirstOrDefault(p => p.Value == c[i].Value );
                if (buscar != null )
                {
                    token clone = buscar;
                    auxiliar.Add(clone);
                    CambioV(c[i].tokens , parameter , new List<token>() , clone );
               }
            else 
            {
                 token clone = (token)c[i].Clone();
                 auxiliar.Add(clone);
                 CambioV(c[i].tokens , parameter , new List<token>() , clone );
            
                
            }
          }
        }
        alfa.tokens = auxiliar;
        return auxiliar ;
    }
    public List<token> cambioV2(List<token> c , token param , token para)
    {
        if (c == null || c.Count == 0)
        {
            return c ;
        }
        for (int i = 0; i < c.Count; i++)
        {
           
            if ( c != null && param.Value == c[i].Value )
            {
                c[i] = new identificador(c[i].Value , TokenTypes.Identifier);
                c[i].tokens.Add(new tokenNumero (para.Value , TokenTypes.Number));
            }
            else if (c != null && c[i].Value == param.Value)
            {
                 c[i] = new identificador(param.Value, TokenTypes.Number);
                 c[i].tokens.Add(new tokenNumero (para.Value , TokenTypes.Number));

            }
            else if ( c[i] != null )
            {
                    cambioV2(c[i].tokens, param , para);
             }
           }
        
        return c ;
    }
    public string CambioF(List<token> tokens ,token parameter, token b , List<string> cadenas)
    {
        if(b.Type == TokenTypes.funcion)
        {
            if (b.tokens .Count >= 2)
            {
             if (!Convert.ToBoolean(b.tokens[1].tokens[0].Evaluar()))
            {
                return tokens[1].tokens[2].Evaluar ().ToString();
            }
            
            }
        }
        for (int i = 0; i < b.tokens.Count; i++)
        {
            token auxiliar = globales.FirstOrDefault(p => p.Value == b.tokens[i].Value );
            if (auxiliar != null && auxiliar.Value  == this.Value)
            {
                token contador = b.tokens[i].tokens[0];
                b = auxiliar;
                b.tokens[0] = contador;
                for (int j = 0; j < parameter.tokens.Count ; j++)
                {
                    b.tokens[0].tokens = cambioV2(b.tokens[0].tokens ,parameter.tokens[j] ,parameter.tokens[j].tokens[0] );
                }
                //string valor = contador.Evaluar();
                token param = new token (null , TokenTypes.Identifier);
                if (contador.Value == ",")
                {
                    parameter = new token(contador.Value , TokenTypes.Keyword);
                }
                else
                {
                     parameter = new token(contador.Value , TokenTypes.Keyword);
                }
                for (int k = 0; k < contador.tokens.Count; k++)
                {
                if (contador.Value == ",")
                {
                 param = new identificador(cadenas[k] , TokenTypes.Number) ;
                 string valor = contador.tokens[k].Evaluar();
                 param.tokens.Add(new tokenNumero(contador.tokens[k].Evaluar(), TokenTypes.Number));
                 parameter.tokens.Add(param);
                }
                else
                {
                    param = new identificador(parameter.Value , TokenTypes.Identifier) ;
                    param.tokens.Add(new tokenNumero(b.tokens[k].Evaluar(), TokenTypes.Number));
                }
                }
              //  parameter.tokens[0] = new tokenNumero(valor , TokenTypes.Number); 
              for (int z= 0; z < parameter.tokens.Count; z++)
              {
                b.tokens[1].tokens[0].tokens = cambioV2( b.tokens[1].tokens[0].tokens, parameter.tokens[z], parameter.tokens[z].tokens[0]);
                b.tokens[0].tokens = cambioV2(b.tokens[0].tokens , parameter.tokens[z] ,parameter.tokens[z].tokens[0]);
              }
            //  b.tokens[0] = parameter;
                token v = new token (CambioF(b.tokens , parameter , b , cadenas), TokenTypes.Literal);
                List <token> cambio = cambioV2(b.tokens[1].tokens[1].tokens , this , v);
                for (int z = 0; z < parameter.tokens.Count; z++)
                {
                cambio = cambioV2(cambio ,parameter.tokens[z], parameter.tokens[z].tokens[0] );
                }
                token evaluador = asignacion( b.tokens[1].tokens[1] ,b.tokens[1].tokens[1].Value , b.tokens[1].tokens[1].Type);

                evaluador.tokens = cambio;
                
                return evaluador.Evaluar().ToString();
            }
            else
            {
               string k =  CambioF(b.tokens[i].tokens ,parameter ,b.tokens[i] , cadenas);
               if (k != null && b is OperatorNode )
               {
                return k;
               }
               else if (k != null && i == 1 )
               {
                return k ;
               }
            }
        }
        return null;
        
    }
   public string Evaluar()
    {
     // string valor = parametro.tokens[0].Value;
     List<token> lista = new List<token>();
     List<string> cadenas = new List<string>();
     List<token> virg = new List<token>();
     for (int i = 0; i < tokens[0].tokens.Count ; i++)
     {
        virg.Add(tokens[0].tokens[i]);
     }
     // List<token > lista  = CambioV(tokens, parametro.tokens ,new List<token >() , this);
     for (int i = 0; i < tokens[0].tokens.Count; i++)
     {
        lista = cambioV2(tokens , tokens[0].tokens[i] , new tokenNumero(tokens[0].tokens[i].Evaluar() , TokenTypes.Number));
        
        cadenas.Add(tokens[0].tokens[i].Value);
     }
    
     string k = CambioF( lista ,tokens[0],this , cadenas);
    // lista = cambioV2( lista ,parametro ,  new tokenNumero(valor , TokenTypes.Number));
     lista = cambioV2(lista ,this , new tokenNumero(k , TokenTypes.Number));
     for (int i = 0; i < virg.Count; i++)
     {
        lista = cambioV2(lista ,virg[i] , virg[i].tokens[0]);
     }
    this.tokens = lista;
    return tokens [1].tokens[1].Evaluar();
    }
public static token asignacion (token a , string value , TokenTypes type )
{
    if (a.Type == TokenTypes.Operator)
    {
       return  new OperatorNode(value , type);
    }
    if (a is Function)
    {
        return new Function(value , type);
    }
    if (a is FunctionNode)
    {
        return new FunctionNode(value , type);
    }
    if (a is Print)
    {
        return new Print (value , type);
    }
    if (a is identificador)
    {
        return new identificador(value , type);
    }
 return null;
}
}
public class LetIn : token 
{
    public  List <token> variables {get ; set ;}
    public List<token> globales {get ; set ;}
    public LetIn (string Value , TokenTypes type) : base(Value , type)
    {
        this.Value = "let";
        this.Type = TokenTypes.letIn;
        variables = new List<token>();
        
    } 

 public List<token> cambioV2(List<token> c)
    {
        if (c == null || c.Count == 0)
        {
            return c ;
        }
        for (int i = 0; i < c.Count; i++)
        {
            if ( variables.FirstOrDefault(p => p.Value == c[i].Value ) != null)
            {
                c[i] = variables.FirstOrDefault(p => p.Value == c[i].Value );
            }
           
            else if ( c[i] != null )
            {
                    cambioV2(c[i].tokens);
             }
           }
        
        return c ;
    }
    public string Evaluar()
    {
        tokens = cambioV2(tokens);
        tokens[0].tokens = cambioV2(tokens[0].tokens);
       
            try
            {   
                 return tokens[0].Evaluar() ;
            }
            catch (System.Exception)
            {
                Console.WriteLine("error en ejecucuion del let-in ");
            }
         throw new ArgumentException();
        }
       
    }

}









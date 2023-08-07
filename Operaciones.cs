using METODOS;
namespace AST

{
    //tipos de token que hay 
    public enum TokenTypes
    {
     Keyword , Identifier , Number , Operator, Punctuation , Literal
    } 
   public interface  Itoken  
   {
       double Evaluar();
   }
   public interface Iboolean
   {
    bool Evaluar();
    
    }
 
    //token definido 
    public class tokenDefinition 
    {
        List<tokenDefinition> tokens ;
        public TokenTypes Type {get ;  set ;}
        public string Value {get ;  set ;}

    public tokenDefinition (TokenTypes type , string value )
     {
        this.Type = type ;
        this.Value = value ;
     }

    }
   public class NodoOperacion : tokenDefinition  
       {
      
        public static  tokenNumero izquierdo {get ; set;}
        public static tokenNumero derecho{get ; set ;} 

        public NodoOperacion(TokenTypes Type , string  Value , tokenNumero izquierdo , tokenNumero  derecho) : base(Type , Value)
        {
            izquierdo = izquierdo;
            derecho = derecho;
        }
        
      
    }
    public class tokenNumero : tokenDefinition , Itoken   
    {
        public tokenNumero(TokenTypes Type , string Value ): base (Type , Value){}

        public double Evaluar()
        {
            try
            {
               double value =  double.Parse(Value);
                return value ;
            }
            catch (System.Exception)
            {
                throw new ArgumentException("LEXICAL ERROR : " + Value + "id not valid token");
            }
        }
      
    }
    public class tokenIdentidad : tokenDefinition    
    {
        public tokenDefinition valor;
        public tokenIdentidad(TokenTypes Type , string Value ): base (Type , Value)
        {
            this.valor = valor;
        }
        public void  evaluar( List<tokenDefinition> toks)
        {
        
                if(char.IsNumber(Value[0]));
            else
            {
                throw new ArgumentException("LEXICAL ERROR : " + Value + "is not valid token" );
            }
                for (int i = 0; i < toks.Count; i++)
                {
                    if (toks[i].Value == Value)
                    {
                      throw new ArgumentException("LEXICAL ERROR : " + Value + "is not valid token  , this identity already exists in the context");
                    }
                }
               
        }    
      
    }
    public class tokenAsignacion : tokenDefinition
    {
        public tokenIdentidad  identidad ;
        public tokenDefinition  definicion;
        public TokenTypes Types;
        public string Value;

        public tokenAsignacion(TokenTypes Types , string Value , tokenIdentidad identidad , tokenDefinition definicion) : base(Types , Value )
        {
            this.identidad = identidad ;
            this.definicion = definicion;
        }


    }
    public class tokenLiteral : tokenDefinition    
    {
        public tokenLiteral(TokenTypes Type , string Value ): base (Type , Value){}

        public string Evaluar()
        {
            return Value;
        }
      
    }

   public class TokenFuncionCos : tokenDefinition , Itoken
   {
       
        public tokenNumero numero ;
    //voy a coger ese string value y lo voy  a tokenizar 
   
        public TokenFuncionCos (TokenTypes Type ,string Value ,tokenNumero numero) : base(Type , Value){}

       public double Evaluar()
        {
            return Math.Cos(metodos.evaluador(numero.Value ,0));
        }
        
   }
   public class TokenFuncionSin : tokenDefinition , Itoken
   {   
        public tokenNumero numero ;
   //voy a coger ese string value y lo voy  a tokenizar 
    
        public TokenFuncionSin (TokenTypes Type ,string Value ,tokenNumero numero) : base(Type , Value){}

       public double Evaluar()
        {
            return Math.Sin(metodos.evaluador(numero.Value ,0));
        }
        
   }
   public class TokenFuncionTan : tokenDefinition , Itoken
   {

   //voy a coger ese string value y lo voy  a tokenizar 
      public tokenNumero  numero ;
        public TokenFuncionTan (TokenTypes Type ,string Value , tokenNumero numero) : base(Type , Value){}

       public double Evaluar()
        {
            return Math.Tan(metodos.evaluador(numero.Value ,0));
        }
        
   }
   public class TokenFuncionPow: NodoOperacion, Itoken
   {
   //voy a coger ese string value y lo voy  a tokenizar 
        
    
        public TokenFuncionPow (TokenTypes Type ,string Value ,tokenNumero izquierdo ,tokenNumero derecho ) : base(Type , Value ,izquierdo , derecho){}
        
       public double Evaluar()
        {
            return Math.Pow(metodos.evaluador(izquierdo.Value ,0),metodos.evaluador(derecho.Value ,0));
        }
        
   }
   public class TokenFuncionSqrt: tokenDefinition , Itoken
   {
   //voy a coger ese string value y lo voy  a tokenizar 
        public  tokenNumero numero ;
        public TokenFuncionSqrt (TokenTypes Type ,string Value ,tokenNumero numero) : base(Type , Value)
        {
            this.numero = numero;
        }
        

       public double Evaluar()
        {
            return Math.Sqrt(metodos.evaluador(numero.Value,0));
        }
        
   }
   public class TokenFuncionMax: NodoOperacion, Itoken
   {
   //voy a coger ese string value y lo voy  a tokenizar 
        
        public TokenFuncionMax (TokenTypes Type ,string Value ,tokenNumero izquierdo , tokenNumero derecho ) : base(Type , Value , izquierdo , derecho){}
       
       public double Evaluar()
        {
            return Math.Max(metodos.evaluador(izquierdo.Value ,0) ,metodos.evaluador(derecho.Value ,0));
        }
        
   }
   public class TokenFuncionMin: NodoOperacion , Itoken
   {
   //voy a coger ese string value y lo voy  a tokenizar 
        
     
        public TokenFuncionMin (TokenTypes Type ,string Value ,tokenNumero izquierdo , tokenNumero derecho) : base(Type , Value , izquierdo , derecho){}
       

       public double Evaluar()
        {
            return Math.Min(metodos.evaluador(izquierdo.Value ,0) ,metodos.evaluador(derecho.Value ,0));
        }
        
   }
 //NODO DE OPERACIONES BINARIAS 
    
    //NODO QUE REALIZA LA SUMA 
    public class NodoSuma : NodoOperacion , Itoken
    {

        public static  tokenNumero izquierdo {get; private set;}
        public static tokenNumero derecho {get ; private set;}

        public NodoSuma(TokenTypes Types , string  value ,tokenNumero izquierdo , tokenNumero derecho ) :base (Types , value ,izquierdo , derecho){}
        public double Evaluar()
        {
            return izquierdo.Evaluar() + derecho.Evaluar();
        }
    }
    //NODO QUE REALIZA LA RESTA 
    public class NodoResta :NodoOperacion , Itoken
    {
        public static tokenNumero izquierdo {get; private set;}
        public static tokenNumero derecho {get ; private set;}

        public NodoResta(TokenTypes Types , string  value ,tokenNumero izquierdo ,tokenNumero derecho) :base (Types , value , izquierdo ,derecho ){}
        public double  Evaluar()
        {
            return izquierdo.Evaluar() - derecho.Evaluar();
        }
    }
    //NODO QUE REALIZA LA MULTIPLICACION 
    public class NodoMulti :NodoOperacion , Itoken
    {
        public static tokenNumero izquierdo {get; private set;}
        public static tokenNumero derecho {get ; private set;}

        public NodoMulti(TokenTypes Types , string  value ,tokenNumero izquierdo ,tokenNumero derecho) :base (Types , value , izquierdo , derecho ){}
        public double  Evaluar()
        {
            return izquierdo.Evaluar() * derecho.Evaluar();
        }
    }
    //NODO QUE REALIZA LA DIVISION 
    public class NodoDivision :NodoOperacion , Itoken
    {
       public static tokenNumero izquierdo {get; private set;}
        public static tokenNumero derecho {get ; private set;}

        public NodoDivision(TokenTypes Types , string  value ,tokenNumero izquierdo ,tokenNumero derecho) :base (Types , value , izquierdo , derecho ){}
    
        public double Evaluar()
        {
            return izquierdo.Evaluar() / derecho.Evaluar();
        }
    }
   //NODO DEL OPERADOR MENOR 
    public class NodoMenor :NodoOperacion , Iboolean
    {
        public static  tokenNumero izquierdo {get; private set;}
        public static tokenNumero derecho {get ; private set;}

        public NodoMenor(TokenTypes Types , string  value,tokenNumero izquierdo ,tokenNumero derecho ) :base (Types , value ,izquierdo , derecho){}
        public bool Evaluar()
        {
            return izquierdo.Evaluar() < derecho.Evaluar();
        }
    }
    //NODO DEL OPERADOR MAYOR 
     public class NodoMayor :NodoOperacion ,Iboolean
    {
        public static  tokenNumero izquierdo {get; private set;}
        public static tokenNumero derecho {get ; private set;}

        public NodoMayor(TokenTypes Types , string  value ,tokenNumero izquierdo ,tokenNumero derecho ) :base (Types , value, izquierdo , derecho ){}
        public bool Evaluar()
        {
            return izquierdo.Evaluar () > derecho.Evaluar();
        }
    }
    //NODO DEL OPERADOR IGUAL 
     public class NodoIgual :NodoOperacion ,Iboolean
    {
        public static tokenNumero izquierdo {get; private set;}
        public static tokenNumero derecho {get ; private set;}

        public NodoIgual(TokenTypes Types , string  value ,tokenNumero izquierdo ,tokenNumero derecho) :base (Types , value , izquierdo , derecho ){}
        public bool Evaluar()
        {
            return izquierdo.Evaluar () == derecho.Evaluar();
        }
    }
     //NODO DEL OPERADOR MENOR IGUAL 
     public class NodoMenorIgual :NodoOperacion ,Iboolean
    {
        public static  tokenNumero izquierdo {get; private set;}
        public static tokenNumero derecho {get ; private set;}

        public NodoMenorIgual(TokenTypes Types , string  value ,tokenNumero izquierdo ,tokenNumero derecho) :base (Types , value , izquierdo , derecho ){}
        public bool Evaluar()
        {
            return izquierdo.Evaluar () <= derecho.Evaluar();
        }
    }
    //NODO DEL OPERADOR MAYOR IGUAL 
     public class NodoMayorIgual :NodoOperacion  , Iboolean
    {
        public static tokenNumero izquierdo {get; private set;}
        public static  tokenNumero derecho {get ; private set;}

        public NodoMayorIgual(TokenTypes Types , string  value ,tokenNumero izquierdo ,tokenNumero derecho) :base (Types , value , izquierdo , derecho ){}
        public  bool Evaluar()
        {
            return izquierdo.Evaluar () >= derecho.Evaluar();
        }
    }
     //NODO DEL OPERADOR DISTINTO 
     public class NodoDistinto :NodoOperacion ,Iboolean
    {
        public static tokenNumero izquierdo {get; private set;}
        public static tokenNumero derecho {get ; private set;}

        public NodoDistinto(TokenTypes Types , string  value ,tokenNumero izquierdo ,tokenNumero derecho) :base (Types , value ,izquierdo , derecho ){}
        public bool Evaluar()
        {
            return izquierdo.Evaluar () != derecho.Evaluar();
        }
    }
    public class NodoOperacionL 
    {
        public TokenTypes Type;
        public string Value ;
        public tokenLiteral izquierdo;
        public tokenLiteral derecho ;

        public NodoOperacionL(TokenTypes Type  , string  Value ,tokenLiteral izquierodo , tokenLiteral derecho) 
        {
            this.Type = Type;
            this.Value = Value ;
            this.izquierdo = izquierdo;
            this.derecho = derecho ;
        }
      
    }
    public class NodoCompLit :NodoOperacionL ,Iboolean
    {
        public TokenTypes Type;
        public string Value ;
        public tokenLiteral izquierdo;
        public tokenLiteral derecho ;

        public NodoCompLit(TokenTypes Type  , string  Value ,tokenLiteral izquierodo , tokenLiteral derecho): base(Type , Value ,izquierodo , derecho){}

        public bool Evaluar()
        {
            return izquierdo.Evaluar() == derecho.Evaluar();
        }
    }
    public class NodoDistLit :NodoOperacionL ,Iboolean
    {
        public TokenTypes Type;
        public string Value ;
        public tokenLiteral izquierdo;
        public tokenLiteral derecho ;

        public NodoDistLit(TokenTypes Type  , string  Value ,tokenLiteral izquierodo , tokenLiteral derecho): base(Type , Value ,izquierodo , derecho){}

        public bool Evaluar()
        {
            return izquierdo.Evaluar() != derecho.Evaluar();
        }
    }
    //le falta el metodo que me evalua 
public class TernariaNum
{
    public  NodoIgual condicion ;
    public  tokenDefinition valorVerdadero;
    public  tokenDefinition valorFalso;

    public TernariaNum(NodoIgual  condicion , tokenDefinition valorVerdadero , tokenDefinition valorFalso)
    {
            this.condicion = condicion;
            this.valorVerdadero = valorVerdadero;
            this.valorFalso = valorFalso;
    }
   
}
//le falta el metodo que me evalua 
    public class TernariaLiteral 
    {
        public NodoCompLit condicion ;
        public tokenDefinition valorVerdadero;

        public tokenDefinition valorFalso;

        public TernariaLiteral(NodoCompLit condicion,tokenDefinition valorVerdadero,tokenDefinition valorFalso)
        {
            this.condicion = condicion;
            this.valorVerdadero = valorVerdadero ;
            this.valorFalso = valorFalso ;
        }
        
    }
    
}


    


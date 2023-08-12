using METODOS;
namespace AST

{
    //tipos de token que hay 
    public enum TokenTypes
    {
     Keyword , Identifier , Number , Operator, Punctuation , Literal ,Condicional , raiz
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
       public List<tokenDefinition> tokens = new List<tokenDefinition>();
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
        public static  List<tokenDefinition> izquierdo {get ;set;}
        public static List<tokenDefinition> derecho {get ; set;}
        
        public NodoOperacion(TokenTypes Type , string  Value ,List<tokenDefinition> izquierdo , List<tokenDefinition> derecho ) : base(Type , Value)
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
                throw new ArgumentException("LEXICAL ERROR : " + Value + "is not valid token");
            }
        }
      
    }
    public class tokenIdentidad : tokenDefinition    
    {
        public tokenDefinition valor;
        public tokenIdentidad(TokenTypes Type , string Value ,tokenDefinition valor): base (Type , Value)
        {
            this.valor = valor;
        }
        public string Evaluar( List<tokenDefinition> toks)
        {
        
                if(char.IsNumber(Value[0]))
                {

                }
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
               return valor.Value;
        }    
      
    }
    public class tokenAsignacion : tokenDefinition
    {
        
        public TokenTypes Types;
        public string Value;

        public tokenAsignacion(TokenTypes Types , string Value , tokenIdentidad identidad , tokenDefinition definicion) : base(Types , Value )
        {
          
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
    public class tokenFuncionPrint : tokenDefinition
    {
      
        public tokenFuncionPrint (TokenTypes Types ,string Value ) : base (Types , Value)
        {
            
        }

        public void Evaluar ()
        {
            Console.WriteLine(metodos.EvaluadorPrint(tokens).Value);
        }
    }
   public class TokenFuncionCos : tokenDefinition, Itoken
   {
        
    //voy a coger ese string value y lo voy  a tokenizar 
   
        public TokenFuncionCos (TokenTypes Type ,string Value) : base(Type , Value){}

       public double Evaluar()
        {
            if (tokens == null)
            {
                throw new ArgumentException("this function takes a parameter");
            }
            try
            {
               
                 return  Math.Cos(metodos.evaluador(tokens ,0 ,0));
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the cos function can only work with numbers");
            }
            
        }
        
   }
   public class TokenFuncionSin : tokenDefinition , Itoken
   {   
        public tokenNumero numero ;
   //voy a coger ese string value y lo voy  a tokenizar 
    
        public TokenFuncionSin (TokenTypes Type ,string Value ) : base(Type , Value){}

       public double Evaluar()
        {
             if (numero.Value == null)
            {
                throw new ArgumentException("this function takes a parameter");
            }
            try
            {
                return Math.Sin(metodos.evaluador(tokens ,0 ,0));
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the SIN function can only work with numbers");
            }
        }
        
   }
   public class TokenFuncionTan : tokenDefinition , Itoken
   {

   //voy a coger ese string value y lo voy  a tokenizar 
      public List<tokenDefinition> numero ;
        public TokenFuncionTan (TokenTypes Type ,string Value , List<tokenNumero> numero) : base(Type , Value){}

       public double Evaluar()
        {
             if (numero == null)
            {
                throw new ArgumentException("this function takes a parameter");
            }
            try
            {
                 return Math.Tan(metodos.evaluador(tokens ,0 ,0));
            }
            catch (System.Exception)
            {
                throw new ArgumentException("SEMANTIC ERROR : the TAN function can only work with numbers");
            }
           
        }
        
   }
   public class TokenFuncionPow: NodoOperacion, Itoken
   {
   //voy a coger ese string value y lo voy  a tokenizar 
        
        
        public TokenFuncionPow (TokenTypes Type ,string Value ,List<tokenDefinition> izquierdo ,List<tokenDefinition> derecho ) : base(Type , Value ,izquierdo , derecho){}
        
       public double Evaluar()
        {
            if (izquierdo == null || derecho == null)
            {
                throw new ArgumentException("this function receives two parameters");
            }
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                 return Math.Pow(a.Evaluar(),b.Evaluar());
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the POW function can only work with numbers");
             
            }
          
        }
        
   }
   public class TokenFuncionSqrt: tokenDefinition , Itoken
   {
   //voy a coger ese string value y lo voy  a tokenizar 
        public List<tokenDefinition> p;
        public TokenFuncionSqrt (TokenTypes Type ,string Value ,List<tokenDefinition> p) : base(Type , Value )
        {
            this.p = p;
        }
        
       public double Evaluar()
        {
            if (tokens == null)
            {
                throw new ArgumentException("this function takes a parameter");
            }
            try
            {       tokenNumero a = new tokenNumero (TokenTypes.Number ,metodos.evaluador(p, 0 ,0).ToString());
                    tokens.Add(a);
                    return Math.Sqrt(double.Parse(a.Value));
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the Sqrt function can only work with numbers");
            }
           
        }
        
   }
   public class TokenFuncionMax: NodoOperacion, Itoken
   {
   //voy a coger ese string value y lo voy  a tokenizar 
        
        
        public TokenFuncionMax (TokenTypes Type ,string Value , List<tokenDefinition> izquierdo , List<tokenDefinition> derecho ) : base(Type , Value ,izquierdo ,derecho ){}
       
       public double Evaluar()
        {
            if (izquierdo == null || derecho == null)
            {
                throw new ArgumentException("this function takes two pararmeters");
            }
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                return Math.Max(a.Evaluar() ,b.Evaluar());
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the Max function can only work with numbers");
            }
           
        }
        
   }
   public class TokenFuncionMin: NodoOperacion , Itoken
   {
   //voy a coger ese string value y lo voy  a tokenizar 
        
     
        public TokenFuncionMin (TokenTypes Type ,string Value ,List<tokenDefinition> izquierdo , List<tokenDefinition> derecho) : base(Type , Value , izquierdo , derecho){}
       

       public double Evaluar()
        {
             if (izquierdo == null || Value == null)
            {
                throw new ArgumentException("this function takes two pararmeters");
            }
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                return Math.Min(a.Evaluar() ,b.Evaluar());
            
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the Min function can only work with numbers");
            }
            
        }
        
   } 
      //NODO QUE REALIZA LA SUMA 
    public class NodoSuma : NodoOperacion , Itoken
    {

        

        public NodoSuma(TokenTypes Types , string  value ,List<tokenDefinition> izquierdo , List<tokenDefinition> derecho ) :base (Types , value ,izquierdo , derecho)
        {
           
        }
        public double Evaluar()
        {
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                 return a.Evaluar() + b.Evaluar();
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador + can only work with numbers");
            }
           
        }
    }
    //NODO QUE REALIZA LA RESTA 
    public class NodoResta :NodoOperacion , Itoken
    {
      

        public NodoResta(TokenTypes Types , string  value ,List<tokenDefinition> izquierdo ,List<tokenDefinition>derecho) :base (Types , value , izquierdo ,derecho )
        {
           
        }
        public double  Evaluar()
        {
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                 return a.Evaluar() - b.Evaluar();
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador - can only work with numbers");
            }
          
        }
    }
    //NODO QUE REALIZA LA MULTIPLICACION 
    public class NodoMulti :NodoOperacion , Itoken
    {
       

        public NodoMulti(TokenTypes Types , string  value ,List<tokenDefinition> izquierdo ,List<tokenDefinition> derecho) :base (Types , value , izquierdo , derecho )
        {
           
        }
        public double  Evaluar()
        {
            
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                return a.Evaluar() * b.Evaluar();
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador * can only work with numbers");
            }
        }
    }
    //NODO QUE REALIZA LA DIVISION 
    public class NodoDivision :NodoOperacion , Itoken
    {
       
        public NodoDivision(TokenTypes Types , string  value ,List<tokenDefinition> izquierdo ,List<tokenDefinition> derecho) :base (Types , value , izquierdo , derecho )
        {
           
        }
    
        public double Evaluar()
        {
            try
            {
                 tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                return a.Evaluar() / b.Evaluar();
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador / can only work with numbers");
            }
           
        }
    }
   //NODO DEL OPERADOR MENOR 
    public class NodoMenor :NodoOperacion , Iboolean
    {
        
        public NodoMenor(TokenTypes Types , string  value,List<tokenDefinition> izquierdo ,List<tokenDefinition> derecho ) :base (Types , value ,izquierdo , derecho)
        {
        
        }
        public bool Evaluar()
        {
      
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                return a.Evaluar() < b.Evaluar();
                
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador < can only work with numbers");
            }
        }
    }
    //NODO DEL OPERADOR MAYOR 
     public class NodoMayor :NodoOperacion ,Iboolean
    {
    

        public NodoMayor(TokenTypes Types , string  value ,List<tokenDefinition> izquierdo ,List<tokenDefinition> derecho) :base (Types , value, izquierdo , derecho )
        {
           
        }
        public bool Evaluar()
        {
           
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                 return a.Evaluar () > b.Evaluar();
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador > can only work with numbers");
            }
        }
    }
    //NODO DEL OPERADOR IGUAL 
     public class NodoIgual :NodoOperacion ,Iboolean
    {
       

        public NodoIgual(TokenTypes Types , string  value ,List<tokenDefinition> izquierdo ,List<tokenDefinition> derecho) :base (Types , value , izquierdo , derecho )
        {
        }
        public bool Evaluar()
        {
            
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                return a.Evaluar() == b.Evaluar();
        
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador == can only work with numbers  or string ");
            }
        }
    }
     //NODO DEL OPERADOR MENOR IGUAL 
     public class NodoMenorIgual :NodoOperacion ,Iboolean
    {
       

        public NodoMenorIgual(TokenTypes Types , string  value ,List<tokenDefinition> izquierdo ,List<tokenDefinition> derecho) :base (Types , value , izquierdo , derecho )
        {

        }
        public bool Evaluar()
        {
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                return a.Evaluar() <= b.Evaluar();
                
               
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador <= can only work with numbers");
            }
        
        }
    }
    //NODO DEL OPERADOR MAYOR IGUAL 
     public class NodoMayorIgual :NodoOperacion  , Iboolean
    {
        
     
        

        public NodoMayorIgual(TokenTypes Types , string  value ,List<tokenDefinition> izquierdo ,List<tokenDefinition> derecho) :base (Types , value , izquierdo , derecho )
        {
          
        }
        public  bool Evaluar()
        {
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                return a.Evaluar() >= b.Evaluar();
                
                ;
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador >= can only work with numbers");
            }
            
        }
    }
     //NODO DEL OPERADOR DISTINTO 
     public class NodoDistinto :NodoOperacion ,Iboolean
    {
       

        public NodoDistinto(TokenTypes Types , string  value ,List<tokenDefinition> izquierdo ,List<tokenDefinition> derecho) :base (Types , value ,izquierdo , derecho )
        {
          
        }
        public bool Evaluar()
        {
            try
            {
                tokenNumero a = new tokenNumero(TokenTypes.Number,metodos.evaluador(izquierdo ,0 ,0).ToString());
                tokenNumero b = new tokenNumero (TokenTypes.Number ,metodos.evaluador(derecho ,0 ,0).ToString());
                tokens.Add(a);
                tokens.Add(b);
                return a.Evaluar() != b.Evaluar();
             
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador != can only work with numbers or string");
            }
            
        }
    }
    public class NodoOperacionL :tokenDefinition
    {
        public TokenTypes Type;
        public string Value ;
        public tokenLiteral izquierdo;
        public tokenLiteral derecho ;

        public NodoOperacionL(TokenTypes Type  , string  Value ,tokenLiteral izquierodo , tokenLiteral derecho) :base(Type , Value) 
        {
         
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

        public NodoCompLit(TokenTypes Type  , string  Value ,tokenLiteral izquierodo , tokenLiteral derecho): base(Type , Value ,izquierodo , derecho)
        {
            tokens[0] = izquierdo;
            tokens[1] = izquierdo;
        }

        public bool Evaluar()
        {
            
            try
            {
                return izquierdo.Evaluar() == derecho.Evaluar();
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador == can only work with numbers or string");
            }
        }
    }
    public class NodoDistLit :NodoOperacionL ,Iboolean
    {
        public TokenTypes Type;
        public string Value ;
        public tokenLiteral izquierdo;
        public tokenLiteral derecho ;

        public NodoDistLit(TokenTypes Type  , string  Value ,tokenLiteral izquierodo , tokenLiteral derecho): base(Type , Value ,izquierodo , derecho)
        {
            tokens[0] = izquierdo;
            tokens[1] = izquierdo;
        }

        public bool Evaluar()
        {
          
            try
            {
                 return izquierdo.Evaluar() != derecho.Evaluar();
            }
            catch (System.Exception)
            {
                
                throw new ArgumentException("SEMANTIC ERROR : the operador != can only work with numbers or string");
            }
        }
    }
    //le falta el metodo que me evalua 
public class TernariaNum :tokenDefinition
{
    public  NodoIgual condicion ;
    public  tokenDefinition valorVerdadero;
    public  tokenDefinition valorFalso;
    public string Value2;

    public TernariaNum(TokenTypes Types , string Value  , string Value2 ,NodoIgual  condicion , tokenDefinition valorVerdadero , tokenDefinition valorFalso): base(Types,Value)
    {
            this.Value2 = Value2;
            this.condicion = condicion;
            this.valorVerdadero = valorVerdadero;
            this.valorFalso = valorFalso;
             tokens[0] = valorVerdadero;
            tokens[1] = valorFalso;
    }
   
}
//le falta el metodo que me evalua 
    public class TernariaLiteral :tokenDefinition
    {
        public NodoCompLit condicion ;
        public tokenDefinition valorVerdadero;
        public tokenDefinition valorFalso;
         public string Value2;
         public TokenTypes Type ;
        public TernariaLiteral(TokenTypes Types , string Value,string Value2 ,NodoCompLit condicion,tokenDefinition valorVerdadero,tokenDefinition valorFalso) :base(Types , Value)
        {
            this.Value2 = Value2;
            this.condicion = condicion;
            this.valorVerdadero = valorVerdadero ;
            this.valorFalso = valorFalso ;
            tokens[0] = valorVerdadero;
            tokens[1] = valorFalso;
        }
        
    }

 public class funcion 
 {
    public string name ;

    public string value ;




 }   
}


    


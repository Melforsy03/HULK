
namespace AST
{
    //tipos de token 
 public enum TokenTypes
{
    Keyword , Identifier ,Number, Operator, Punctuation ,Literal ,Condicional , funcion , boolean
} 
//nodo token con valor string y tipo de token 
public abstract class metodo
{
    public abstract string Evaluar();
}
public class token : metodo
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

 public override string Evaluar()
 {
    if (Type == TokenTypes.Number)
    {
        return Value;;
    }
    if (tokens[0].Type == TokenTypes.Number)
    {
        return ((tokenNumero)tokens[0]).Evaluar().ToString();
    }
     else if (tokens[0].Type == TokenTypes.Operator)
    {
         return ((OperatorNode)tokens[0]).Evaluar().ToString() ;
    }
   else if (tokens[0].Type == TokenTypes.funcion)
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
public class identificador : token  
{
    
    //public int position{get ;set;}
    public identificador(string Value , TokenTypes type) :base(Value , type ){}
    public string Evaluar ()
    {
        
     return tokens[0].Evaluar().ToString();
  
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
        
            if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number )
            {
                    return ((tokenNumero)tokens[0]).Evaluar() + ((tokenNumero)tokens[1]).Evaluar();;
            }
            else  if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
            {
                return ((OperatorNode)tokens[0]).Evaluar() + ((tokenNumero)tokens[1]).Evaluar();
            }
            else if (tokens[1].Type == TokenTypes.Operator && tokens[0].Type == TokenTypes.Number)
            {
                return ((OperatorNode)tokens[1]).Evaluar() + ((tokenNumero)tokens[0]).Evaluar();
            }
            else if(tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
            {
                return ((OperatorNode)tokens[0]).Evaluar() + ((OperatorNode)tokens[1]).Evaluar();
            }
          
            else  if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
            {
                return ((FunctionNode)tokens[0]).Evaluar() + ((tokenNumero)tokens[1]).Evaluar();
            }
            else if (tokens[1].Type == TokenTypes.funcion && tokens[0].Type == TokenTypes.Number)
            {
                return ((FunctionNode)tokens[1]).Evaluar() +  ((tokenNumero)tokens[0]).Evaluar();
            }
            else if(tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
            {
                return ((FunctionNode)tokens[0]).Evaluar() + ((FunctionNode)tokens[1]).Evaluar();
            }
            else 
            {
                return double.Parse(tokens[0].Evaluar()) + double.Parse(tokens[1].Evaluar()) ;           
            }
       }
       else if (Value == "-")
       {
        
            if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number )
            {
                 return ((tokenNumero)tokens[0]).Evaluar() - ((tokenNumero)tokens[1]).Evaluar();
            }
            else  if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
            {
                return ((OperatorNode)tokens[0]).Evaluar() - ((tokenNumero)tokens[1]).Evaluar();
            }
            else if (tokens[1].Type == TokenTypes.Operator && tokens[0].Type == TokenTypes.Number)
            {
                return ((OperatorNode)tokens[1]).Evaluar() -((tokenNumero)tokens[0]).Evaluar();
            }
            else if(tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
            {
                return ((OperatorNode)tokens[0]).Evaluar() - ((OperatorNode)tokens[1]).Evaluar();
            }
            else  if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
            {
                return ((FunctionNode)tokens[0]).Evaluar() - ((tokenNumero)tokens[1]).Evaluar();
            }
            else if (tokens[1].Type == TokenTypes.funcion && tokens[0].Type == TokenTypes.Number)
            {
                return ((FunctionNode)tokens[1]).Evaluar() - ((tokenNumero)tokens[0]).Evaluar();
            }
            else if(tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
            {
                return ((FunctionNode)tokens[0]).Evaluar() - ((FunctionNode)tokens[1]).Evaluar();
            }
       }
       else if (Value == "*")
       {
        
            if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number )
            {
                 return ((tokenNumero)tokens[0]).Evaluar() * ((tokenNumero)tokens[1]).Evaluar();
            }
            else  if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
            {
                return ((OperatorNode)tokens[0]).Evaluar() * ((tokenNumero)tokens[1]).Evaluar();
            }
            else if (tokens[1].Type == TokenTypes.Operator && tokens[0].Type == TokenTypes.Number)
            {
                return ((OperatorNode)tokens[1]).Evaluar() * ((tokenNumero)tokens[0]).Evaluar();
            }
            else if(tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
            {
                return ((OperatorNode)tokens[0]).Evaluar() * ((OperatorNode)tokens[1]).Evaluar();
            }
            else  if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
            {
                return ((FunctionNode)tokens[0]).Evaluar() * ((tokenNumero)tokens[1]).Evaluar();
            }
            else if (tokens[1].Type == TokenTypes.funcion && tokens[0].Type == TokenTypes.Number)
            {
                return ((FunctionNode)tokens[1]).Evaluar() * ((tokenNumero)tokens[0]).Evaluar();
            }
            else if(tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
            {
                return ((FunctionNode)tokens[0]).Evaluar() * ((FunctionNode)tokens[1]).Evaluar();
            }
       }
       else if (Value == "/")
       {
        
            if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number )
            {
                 return ((tokenNumero)tokens[0]).Evaluar() / ((tokenNumero)tokens[1]).Evaluar();
            }
            else  if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
            {
                 return ((OperatorNode)tokens[0]).Evaluar() /((tokenNumero)tokens[1]).Evaluar();
            }
            else if (tokens[1].Type == TokenTypes.Operator && tokens[0].Type == TokenTypes.Number)
            {
                return   ((tokenNumero)tokens[0]).Evaluar() /((OperatorNode)tokens[1]).Evaluar();
            }
            else if(tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
            {
                return ((OperatorNode)tokens[0]).Evaluar() / ((OperatorNode)tokens[1]).Evaluar();
            }
            else  if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
            {
                return ((FunctionNode)tokens[0]).Evaluar() / ((tokenNumero)tokens[1]).Evaluar();
            }
            else if (tokens[1].Type == TokenTypes.funcion && tokens[0].Type == TokenTypes.Number)
            {
                return  ((tokenNumero)tokens[0]).Evaluar()/ ((FunctionNode)tokens[1]).Evaluar();
            }
            else if(tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
            {
                return ((FunctionNode)tokens[0]).Evaluar() / ((FunctionNode)tokens[1]).Evaluar();
            }
       }
       else if (Value == "^")
       {
        
            if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number )
            {
                 return Math.Pow (((tokenNumero)tokens[0]).Evaluar() , ((tokenNumero)tokens[1]).Evaluar());
            }
            else  if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
            {
                return Math.Pow(((OperatorNode)tokens[0]).Evaluar() ,((tokenNumero)tokens[1]).Evaluar());
            }
            else if (tokens[1].Type == TokenTypes.Operator && tokens[0].Type == TokenTypes.Number)
            {
                return   Math.Pow (((tokenNumero)tokens[0]).Evaluar() ,((OperatorNode)tokens[1]).Evaluar());
            }
            else if(tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
            {
                return Math.Pow(((OperatorNode)tokens[0]).Evaluar(), ((OperatorNode)tokens[1]).Evaluar());
            }
            else  if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
            {
                return Math.Pow(((FunctionNode)tokens[0]).Evaluar() , ((tokenNumero)tokens[1]).Evaluar());
            }
            else if (tokens[1].Type == TokenTypes.funcion && tokens[0].Type == TokenTypes.Number)
            {
                return  Math.Pow( ((tokenNumero)tokens[0]).Evaluar() , ((FunctionNode)tokens[1]).Evaluar());
            }
            else if(tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
            {
                return Math.Pow(((FunctionNode)tokens[0]).Evaluar() , ((FunctionNode)tokens[1]).Evaluar());
            }
       }
       else if (Value == "%")
       {
        
            if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number )
            {
                                  return ((tokenNumero)tokens[0]).Evaluar() % ((tokenNumero)tokens[1]).Evaluar();
            }
            else  if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
            {
                return ((OperatorNode)tokens[0]).Evaluar() % ((tokenNumero)tokens[1]).Evaluar();
            }
            else if (tokens[1].Type == TokenTypes.Operator && tokens[0].Type == TokenTypes.Number)
            {
                return   ((tokenNumero)tokens[0]).Evaluar() %((OperatorNode)tokens[1]).Evaluar();
            }
            else if(tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
            {
                return ((OperatorNode)tokens[0]).Evaluar() % ((OperatorNode)tokens[1]).Evaluar();
            }
          else  if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
            {
                return ((FunctionNode)tokens[0]).Evaluar() % ((tokenNumero)tokens[1]).Evaluar();
            }
            else if (tokens[1].Type == TokenTypes.funcion && tokens[0].Type == TokenTypes.Number)
            {
                return  ((tokenNumero)tokens[0]).Evaluar() % ((FunctionNode)tokens[1]).Evaluar();
            }
            else if(tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
            {
                return ((FunctionNode)tokens[0]).Evaluar() % ((FunctionNode)tokens[1]).Evaluar();
            }
       }
                throw new InvalidOperationException("Operador no válido");
        
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
       
        resultado = ((tokenBul)tokens[0]).Evaluar() && ((tokenBul)tokens[1]).Evaluar();
    }
    else if (Value == "||")
    {
        resultado = ((tokenBul)tokens[0]).Evaluar() || ((tokenBul)tokens[1]).Evaluar();
    }
    else if (Value == "!=")
    {
       if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() != ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() != ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() != ((OperatorNode)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() != ((OperatorNode)tokens[1]).Evaluar();
       }
          if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Operator)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() != ((OperatorNode)tokens[1]).Evaluar();
       }
         if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((OperatorNode)tokens[0]).Evaluar() != ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() != ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((tokenNumero)tokens[0]).Evaluar() != ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() != ((tokenNumero)tokens[1]).Evaluar();
       }
    }
    else if (Value == ">")
    {
         if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() > ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() > ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() > ((OperatorNode)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() > ((OperatorNode)tokens[1]).Evaluar();
       }
          if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Operator)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() > ((OperatorNode)tokens[1]).Evaluar();
       }
         if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((OperatorNode)tokens[0]).Evaluar() > ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() > ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((tokenNumero)tokens[0]).Evaluar() > ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() > ((tokenNumero)tokens[1]).Evaluar();
       }
    }

    else if (Value == "<" )
    {
       if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() < ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() < ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() < ((OperatorNode)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() < ((OperatorNode)tokens[1]).Evaluar();
       }
      if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Operator)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() < ((OperatorNode)tokens[1]).Evaluar();
       }
         if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((OperatorNode)tokens[0]).Evaluar() < ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() < ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((tokenNumero)tokens[0]).Evaluar() < ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() < ((tokenNumero)tokens[1]).Evaluar();
       }
    }
    else if (Value == "==" )
    {
       
         if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() == ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() == ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() == ((OperatorNode)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() == ((OperatorNode)tokens[1]).Evaluar();
       }
          if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Operator)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() == ((OperatorNode)tokens[1]).Evaluar();
       }
         if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((OperatorNode)tokens[0]).Evaluar() == ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() == ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((tokenNumero)tokens[0]).Evaluar() == ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() == ((tokenNumero)tokens[1]).Evaluar();
       }
    }
    else if (Value == ">=" )
    {
                 if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() >= ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() >= ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() >= ((OperatorNode)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() >= ((OperatorNode)tokens[1]).Evaluar();
       }
          if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Operator)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() >= ((OperatorNode)tokens[1]).Evaluar();
       }
         if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((OperatorNode)tokens[0]).Evaluar() >= ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() >= ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((tokenNumero)tokens[0]).Evaluar() >= ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() >= ((tokenNumero)tokens[1]).Evaluar();
       }
    }
    else if (Value == "<=" )
    {
     if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() <= ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Number)
       {

        resultado = ((OperatorNode)tokens[0]).Evaluar() <= ((tokenNumero)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.Operator)
       {

        resultado = ((tokenNumero)tokens[0]).Evaluar() <= ((OperatorNode)tokens[1]).Evaluar();
       }
       if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.Operator)
       {
        resultado = ((OperatorNode)tokens[0]).Evaluar() <= ((OperatorNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Operator)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() <= ((OperatorNode)tokens[1]).Evaluar();
       }
         if (tokens[0].Type == TokenTypes.Operator && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((OperatorNode)tokens[0]).Evaluar() <= ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() <= ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.Number && tokens[1].Type == TokenTypes.funcion)
       {
        resultado = ((tokenNumero)tokens[0]).Evaluar() <= ((FunctionNode)tokens[1]).Evaluar();
       }
        if (tokens[0].Type == TokenTypes.funcion && tokens[1].Type == TokenTypes.Number)
       {
        resultado = ((FunctionNode)tokens[0]).Evaluar() <= ((tokenNumero)tokens[1]).Evaluar();
       }
    }
    return resultado;
}
    
}
//esta clase es para parsear expresiones aritmeticas dado una lista de tokens 



//parsea el arbol 
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
    
public class Function :token 
{
    
    public Function(String Value , TokenTypes Type) : base(Value , Type){}

    public token parametro {get ; set ;}

    public void CambioV()
    {
        if (tokens.Count == 0)
        {
            return;
        }
        for (int i = 1; i < tokens.Count; i++)
        {
            if (tokens[i].Value == tokens[0].Value)
            {
                tokens[i] = parametro;
            }
        }
    }
    public string Evaluar()
    {
        CambioV();
        return tokens[1].Evaluar();
    }
}
}



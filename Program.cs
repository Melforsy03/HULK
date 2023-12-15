using AST;
using Tokenizador;
using Abol;

namespace Usuario
{
     public class A 
    {
       public static void Main (string [] args)
        {
          Parser ganma = new Parser("" , TokenTypes.Identifier,null);
          while(true)
          {
            Console.WriteLine(" ");
            Console.Write(">");
            string a = Console.ReadLine ();
            if(a == "") break;
            ganma.expression = Tokenizador.input.TokenizeString(a);
            ganma.construir();
            
          }
          ganma.Evaluate();
        }
    }
}


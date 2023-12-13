using AST;
using Tokenizador;
using Abol;

namespace Usuario
{
     public class A 
    {
       public static void Main (string [] args)
        {
      
          input c = new input();
          Parser ganma = new Parser("" , TokenTypes.Identifier,null);
          while(true)
          {
           // Console.WriteLine(" ");
           // Console.Write(">");
           // string a = Console.ReadLine();
            
              string a = "let a = 4, b = 5 in a + b;";
             // string a = "function sum ( a , b ) => if ( a > 0) sum ( a - 1 , b) + b else  {0} ; ";
             // string b = "let a = 2 , b = 6 in sum (a , b) ;";
              if(a == "")break;
            
              ganma.expression = Tokenizador.input.TokenizeString(a);
              ganma.construir();
             // root.agregar(ganma);
             // root.fuc = root.variables;
             // ganma.Root = root;
             break;
             
          }
          
           
          // Console.WriteLine( root.EvaluateO());
        }
    }
}


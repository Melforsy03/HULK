using AST;
using Tokenizador;
using Abol;

namespace Usuario
{
     public class A 
    {
       public static  Parser root = new Parser(null);
       public static void Main (string [] args)
        {
      
          input c = new input();
          Parser root = new Parser(null);
          while(true)
          {
               string a = Console.ReadLine();
              if(a == "" )break;
               List<token> alfa = input.ERROR(a);
               Parser ganma = new Parser(root);
            if (alfa != null)
           {
              ganma.expression = alfa;
              ganma.construir();
              root.agregar(ganma);
              root.fuc = root.variables;
              ganma.Root = root;
           }
          }
          
             root.EvaluateO();
          // Console.WriteLine( root.EvaluateO());
        }
    }
}


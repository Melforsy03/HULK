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
             //string a = Console.ReadLine();
             string a = "Sqrt ( 4 ) ;";
             // string a = " Print ( if  ( 6 > 9 )  4 + 5  else { Print ( 10 * 10 ) } ) ;";
             // string a = "function sum ( a , b ) => if ( a > 0) sum ( a - 1 , b) + b else  {0} ; ";
             // string b = "let a = 2 , b = 6 in sum (a , b) ;";
              if(a == "" )
              {
                 root.EvaluateO();
                 break;
              }
               List<token> alfa = input.ERROR(a);

               Parser ganma = new Parser(root);
            if (alfa != null)
           {
              ganma.expression = alfa;
              ganma.construir();
              root.agregar(ganma);
              root.fuc = root.variables;
              ganma.Root = root;
              root.EvaluateO();
                 break;
            

           }
          }
           
          // Console.WriteLine( root.EvaluateO());
        }
    }
}


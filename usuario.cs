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
         // string a = " let a = 90 , x = \"melani\" in Print ( x )";
          input c = new input();
          string a = "function sum (a, b) => if(a > 0) (sum (a - 1 , b) + a) + b else {0};";
          string b = "let a = 5 , b = 6 in sum(a , b);";
           List<token> alfa = input.ERROR(a);
           List<token> papa = input.ERROR(b);
           Parser root = new Parser(null);
           Parser beta = new Parser(root);
           Parser ganma = new Parser(root);
           beta.Root = root;
           ganma.Root  = root;
           if (alfa != null)
           {
              beta.expression = alfa;
              beta.construir();
              root.agregar(beta);
              root.fuc = root.variables;
              beta.Root = root;

           }
         if (papa != null)
         {
           ganma.expression = papa;
           ganma.construir();
             root.agregar(ganma);
             ganma.Root = root;
         }
            root.EvaluateO();
          // Console.WriteLine( root.EvaluateO());
         
        }
    }
}

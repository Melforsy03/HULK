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
         string d = "let a = 90 in a;";
          input c = new input();
          string a = "function sum (a , b) => a + b ;";
          string b = "let a = 5 , b = 6 in sum(a , b);";
           List<token> alfa = input.ERROR(a);
           List<token> papa = input.ERROR(b);
           List<token> deede = input.ERROR(d);
           Parser root = new Parser(null);
           Parser beta = new Parser(root);
           Parser ganma = new Parser(root);
           Parser DE = new Parser(root);
         
           DE.Root = root;
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
             root.fuc = root.variables;
             ganma.Root = root;
         }
         if (deede != null)
         {
              DE.expression = deede;
              DE.construir();
              root.agregar(DE);
              root.fuc = root.variables;
              DE.Root = root;
         }
            root.EvaluateO();
          // Console.WriteLine( root.EvaluateO());
         
        }
    }
}

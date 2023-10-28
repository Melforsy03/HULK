using AST;
using Tokenizador;
using Abol;

namespace Usuario
{
     public class A 
    {
       public static  Parser root = new Parser();
       public static void Main (string [] args)
        {
            string a = "function sum (a) => if(a > 0) sum (a - 1) + a else {0}";
            string b = "let a = 5 in sum(a)";
            List<token> alfa = tokenizador.TokenizeString(a);
            List<token> papa = tokenizador.TokenizeString(b);
            Parser root = new Parser();
            Parser beta = new Parser();
            Parser ganma = new Parser();
            beta.Root = root;
            ganma.Root  = root;
            beta.expression = alfa;
            ganma.expression = papa;
            beta.construir();
            ganma.construir();
            root.agregar(beta);
            root.fuc = root.variables;
            root.agregar(ganma);
            
            List<string> m = new List<string>();
            root.EvaluateO();
         
        }
    }
}

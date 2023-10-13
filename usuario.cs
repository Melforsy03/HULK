using AST;
using Tokenizador;
using Abol;

namespace Usuario
{
     public class A 
    {
       public static void Main (string [] args)
        {
            string a = "function sum (a) => if(a > 0) sum (a - 1) + a else {0}";
            string b = "let a = 5 in sum(a)";
            List<token> alfa = tokenizador.TokenizeString(a);
            List<token> papa = tokenizador.TokenizeString(b);
            Parser root = new Parser();
            Parser beta = new Parser();
            beta.Root = root;
            beta.expression = alfa ;
            Parser ganma = new Parser();
            ganma.expression = papa;
            Console.WriteLine(beta.EvaluateO());
           
        }
    }
}

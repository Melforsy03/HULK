using AST;
namespace METODOS
{

    public class metodos 
    {
     static int I = 0;
    
    //metodo que me evalua las expresiones aritmeticas
    public static double evaluador (string cadena ,double valor)
    {
       
       for (int i = 0; i < cadena.Length; i++)
       {
        if (cadena[i] == '(')
        {
           return valor =  evaluador(cadena ,valor);

        }
        else
        {
             string sub = caden( "",cadena);

            List <tokenDefinition> tok = tokenizar.Tokenizer.Tokenizando(sub);

            List<tokenDefinition> a = primero(tok , 0 , tok);

             return valor = operacion(tok ,0,0);

 
        }
       }
       return valor ;

    }
    public static string caden (string sub , string cadena )
    {

         for (int j = I + 1; j < cadena.Length; j++)
            {
                if (cadena[j] != ')')
                {
                sub += cadena[j];
                }
                else 
                {
                    I = j;
                    break;
                }
            }

            return sub;
    }
    //devuelve un valor numerico 
    public static double operacion(List<tokenDefinition> input  , int index, double num )
    {
        if (index == input.Count - 1)
        {
            num = Double.Parse(input[index].Value);
            return num ;
        }

         List<tokenDefinition> a = primero(input , 0 , input);

         for (int i = 0; i < a.Count; i++)
         { 

            if (a[i].Value == "+")
            {
                 num = double.Parse(a[i-1].Value);
                return num += operacion(a,i + 1 ,0);
                  
            }
            if (a[i].Value == "-")
            {
               num = double.Parse(a[i-1].Value);
                return num -= operacion(a,i + 1 ,0);
            }
         }
        return num ;
    }
    //metodo que me devuleve una lista de tokens con la multiplicacion y division ya realizada
    public static List<tokenDefinition> primero(List<tokenDefinition> input , int index ,List<tokenDefinition> toks )
    {
        if(index == input.Count)return toks;

        for (int i = 0; i < input.Count; i++)
        {
                if (input[i].Value == "*")
                {
                    NodoMulti a  = new NodoMulti(TokenTypes.Operator ,"*",new tokenNumero(TokenTypes.Operator ,input[i - 1].Value),new tokenNumero(TokenTypes.Number ,input[i+1].Value));
                    tokenNumero b = new tokenNumero(TokenTypes.Number,Convert.ToString(a.Evaluar()));
                    toks.RemoveAt(i);
                    toks.RemoveAt(i+1);
                    toks[i-1] = b;
                    return primero(toks, i , toks);

                }
                if (input[i].Value == "/" )
                {
                    NodoDivision a  = new NodoDivision(TokenTypes.Operator ,"/",new tokenNumero(TokenTypes.Operator ,input[i - 1].Value),new tokenNumero(TokenTypes.Number ,input[i+1].Value));
                    tokenNumero b = new tokenNumero(TokenTypes.Number,Convert.ToString(a.Evaluar()));
                    toks.RemoveAt(i);
                    toks.RemoveAt(i+1);
                    toks[i-1] = b ;
                    return primero(toks, i , toks);

                }
        }
        return toks;

    }

}
}
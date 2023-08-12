using AST;
namespace METODOS
{

    public class metodos 
    {
    
    public static tokenDefinition A = new tokenDefinition(TokenTypes.raiz ,"");
    public static tokenDefinition EvaluadorPrint(List<tokenDefinition> tok)
    {
        int comas1 = 0;
        int comas2 = 0;
         
         string value = "";
         for (int i = 0; i < tok.Count; i++)
         {
            A.tokens.Add(tok[i]);
            if (tok[i].Value == "Print")
            {
                if (tok[i+1].Value == "(")
                {
                    tok[i].tokens.Add(tok[i+1]);
                     A.tokens[i].tokens.Add( opcion1(tok ,i + 1 , tok[i]));
                       return A.tokens[0].tokens[0];
                }
                
            }
         }
     return null;
    }
    public static tokenDefinition opcion1(List<tokenDefinition> tok , int J , tokenDefinition beta)
    
    {     for (int j = J; j < tok.Count; j++)
                {    
                if (tok[j].Value == "\"" && tok[j+1].Type == TokenTypes.Literal)
                {
                    return tok[j+1];
                }
                if (tok[j].Type == TokenTypes.Identifier && tok[j+1].Value == ")")
                {
                    return tok[j];
                }
                else
                {
                    tokenNumero a = new tokenNumero(TokenTypes.Number ,evaluador(tok, 0 , J- 1).ToString());
                    beta.tokens.Add(a);
                    return a;
                }
                }
               return null; 
    }

    //metodo que me evalua las expresiones aritmeticas
    public static double evaluador(List<tokenDefinition> cadena , double valor , int iterador)
    {
       List<tokenDefinition> b = new List<tokenDefinition>();
       for (int i = iterador; i < cadena.Count; i++)
       {
        if (cadena[i].Value == "(")
        {
           return valor =  evaluador(cadena ,valor ,i+1);

        }
        else
        {
             List<tokenDefinition> sub = caden(b,cadena ,i );

            //List<tokenDefinition> a = primero(tok , 0 , tok);

             return valor = operacion(sub ,0,0);

 
        }
       }
       return valor ;

    }
    public static List<tokenDefinition> caden (List <tokenDefinition> sub , List <tokenDefinition> cadena , int I)
    {

         for (int j = I ; j < cadena.Count; j++)
            {
                if (cadena[j].Value != ")")
                {
                sub.Add(cadena[j]);
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
        List<tokenDefinition> uno = new List<tokenDefinition>();
        List<tokenDefinition> dos = new List<tokenDefinition>();
        
         List<tokenDefinition> b = segundo(input ,0 ,input ,uno ,0,0,dos);
         List<tokenDefinition> a = primero(b , 0 , b ,uno , 0 , 0 , dos);
        

         for (int i = 0; i < a.Count; i++)
         { 
            if (input.Count > 1)
            {
               return double.Parse(a[0].Value);
            }
            else
            {
                
            if (a[i].Value == "+")
            {
                 num = double.Parse(a[i-1].Value);
                return num += operacion(a,i + 1 ,0);
                  
            }
            if  (a[i].Value == "-")
            {
               num = double.Parse(a[i-1].Value);
                return num -= operacion(a,i + 1 ,0);
            }
            }
         }
        return num ;
    }
    //metodo que me devuleve una lista de tokens con la multiplicacion y division ya realizada
    public static List<tokenDefinition> primero(List<tokenDefinition> input , int index ,List<tokenDefinition> toks,List<tokenDefinition> al , int key , int index1 ,List<tokenDefinition> d )
    {
        if(index == input.Count)return toks;

        for (int i = 0; i < input.Count; i++)
        {
                if (input[i].Value == "*")
                {
                   for (int s = i; s < input.Count; s--)
                    {
                        if (input[s].Value != ")" || input[s].Type == TokenTypes.Number)
                        {
                            key = s;
                            al.Add(input[s]); 
                        }
                        else if (input[s].Value == "(" || input[s].Type == TokenTypes.Operator)
                        {
                            break;
                        }
                       
                    }
                    for (int s = 0; s < input.Count; s++)
                    {
                        if (input[s].Value != "(" || input[s].Type == TokenTypes.Number)
                        {
                            index1 = s;
                            d.Add(input[s]); 
                        }
                        else if (input[s].Value == ")" || input[s].Type == TokenTypes.Operator)
                        {
                            break;
                        }
                    }
                    NodoMulti a  = new NodoMulti(TokenTypes.Operator ,"/",al ,d);
                    for (int h = 0; h < input.Count; h++)
                {
                    if (h== key)
                    {
                        tokenNumero b = new tokenNumero(TokenTypes.Number,Convert.ToString(a.Evaluar()));
                        toks[h] = b;
                    }
                    if (h>= index1 && h<= key)
                    {
                        toks.RemoveAt(h);
                    }
                    List<tokenDefinition> uno = new List<tokenDefinition>();
                    List<tokenDefinition> dos = new List<tokenDefinition>();
                     return primero(toks,key ,toks ,uno , 0 ,0, dos);

                }
                if (input[i].Value == "/")
                {
                    for (int s = i; s < input.Count; s--)
                    {
                        if (input[s].Value != ")" || input[s].Type == TokenTypes.Number)
                        {
                            key = s;
                            al.Add(input[s]); 
                        }
                        else if (input[s].Value == "(" || input[s].Type == TokenTypes.Operator)
                        {
                            break;
                        }
                       
                    }
                    for (int s = 0; s < input.Count; s++)
                    {
                        if (input[s].Value != "(" || input[s].Type == TokenTypes.Number)
                        {
                            index1 = s;
                            d.Add(input[s]); 
                        }
                        else if (input[s].Value == ")" || input[s].Type == TokenTypes.Operator)
                        {
                            break;
                        }
                    }
                    NodoDivision p  = new NodoDivision(TokenTypes.Operator ,"/",al ,d);
                    for (int h = 0; h < input.Count; h++)
                {
                    if (h== key)
                    {
                        tokenNumero b = new tokenNumero(TokenTypes.Number,Convert.ToString(p.Evaluar()));
                        toks[h] = b;
                    }
                    if (h>= index1 && h<= key)
                    {
                        toks.RemoveAt(h);
                    }
                }
                List<tokenDefinition> uno = new List<tokenDefinition>();
                List<tokenDefinition> dos = new List<tokenDefinition>();
                return primero(toks,key ,toks ,uno , 0 ,0, dos);
                }
        }
    }
    return toks ;
    }
     public static List<tokenDefinition> segundo (List<tokenDefinition> input , int index ,List<tokenDefinition> toks , List<tokenDefinition> a , double k , double t, List<tokenDefinition> d )
     {
        if(index == input.Count)return toks ;
        int key = 0;
        int index1 = 0;
        int index2 = 0;
        for (int i = 0; i < input.Count; i++)
        {
            if (input[i].Value == "Sqrt")
            {
                key = i;
                if (input[i+1].Value == "(")
                {
                    index1 = i+1;
                    for (int j = i + 2; j < input.Count; j++)
                    {
                        if (input[j].Value != ")")
                        {
                         a.Add(input[j]);
                        
                        }
                        else
                        {
                             index2 = j;
                             break;
                        }
                    }
                TokenFuncionSqrt sqrt = new TokenFuncionSqrt(TokenTypes.Number ,"Sqrt" ,a);
                for (int h = 0; h < input.Count; h++)
                {
                    if (h== key)
                    {
                        tokenNumero b = new tokenNumero(TokenTypes.Number,Convert.ToString(sqrt.Evaluar()));
                        toks[h] = b;
                    }
                    if (h>= index1 && h<= index2)
                    {
                        toks.RemoveAt(h);
                    }
                }
                
                 List<tokenDefinition> uno = new List<tokenDefinition>();
                List<tokenDefinition> dos = new List<tokenDefinition>();
                return segundo(toks,i + 1,toks , uno ,0 ,0,dos);
            }
            if (input[i].Value == "Pow")
            {
                key = i;
                if (input[i+1].Value == "(")
                {
                    index1 = i+1;
                    for (int j = i + 2; j < input.Count; j++)
                    {
                        if (input[j].Value != ",")
                        {
                         a.Add(input[j]);
                        }
                        else
                        { 
                             

                             for (int l = j+1; l < input.Count; l++)
                             {
                                if (input[l].Value != ")")
                                {
                                  d.Add(input[l]);
                                }
                                else
                                {
                                     index2 = l; 
                                    
                                     break;

                                }
                             }
                             
                        }
                        
                    }
                }
                TokenFuncionPow pow = new TokenFuncionPow(TokenTypes.Number ,"Pow" ,a,d);
                for (int h = 0; h < input.Count; h++)
                {
                    if (h== key)
                    {
                        tokenNumero b = new tokenNumero(TokenTypes.Number,Convert.ToString(pow.Evaluar()));
                        toks[h] = b;
                    }
                    if (h>= index1 && h<= index2)
                    {
                        toks.RemoveAt(h);
                    }
                }
                 List<tokenDefinition> uno = new List<tokenDefinition>();
                List<tokenDefinition> dos = new List<tokenDefinition>();
                return segundo(toks,i + 1,toks , uno,0,0 ,dos);
            }
     }
    }
     return toks;
    
}
}
}

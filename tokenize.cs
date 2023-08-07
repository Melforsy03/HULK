using AST;
namespace tokenizar
{
    public class probar
    {
    
    public static void Main(string [] args)
    {
        
    }
    }
     
    
    public class Tokenizer 
    {
        private static List<tokenDefinition> TokenDefi = new List<tokenDefinition>(){
            new tokenDefinition(TokenTypes.Keyword ,"if"),
            new tokenDefinition(TokenTypes.Keyword ,"let") ,
            new tokenDefinition(TokenTypes.Keyword ,"print"),
            new tokenDefinition(TokenTypes.Keyword ,"in"),
            new tokenDefinition(TokenTypes.Keyword ,"function"),
            new tokenDefinition(TokenTypes.Keyword , "else"),
            new tokenDefinition(TokenTypes.Keyword , "for"),
            new tokenDefinition(TokenTypes.Keyword , "while"),
            new tokenDefinition(TokenTypes.Keyword, "int"),
            new tokenDefinition(TokenTypes.Keyword, "double"),
            new tokenDefinition(TokenTypes.Keyword, "float"),
            new tokenDefinition(TokenTypes.Keyword, "static"),
            new tokenDefinition(TokenTypes.Keyword, "public"),
            new tokenDefinition(TokenTypes.Keyword, "void"),
            new tokenDefinition(TokenTypes.Keyword, "class"),
            new tokenDefinition(TokenTypes.Keyword, "abstract"),
            new tokenDefinition(TokenTypes.Keyword, "string"),
            new tokenDefinition(TokenTypes.Keyword, "char"),
            new tokenDefinition(TokenTypes.Keyword, "new"),
            new tokenDefinition(TokenTypes.Keyword, "continue"),
            new tokenDefinition(TokenTypes.Keyword, "bool"),
            new tokenDefinition(TokenTypes.Keyword, "break"),
            new tokenDefinition(TokenTypes.Keyword, "delegate"),
            new TokenFuncionPow(TokenTypes.Keyword ,"Pow", null ,null),
            new TokenFuncionSqrt(TokenTypes.Keyword ,"Sqrt" , null),
            new TokenFuncionMax(TokenTypes.Keyword ,"Max" ,null , null),
            new TokenFuncionMin(TokenTypes.Keyword ,"Min" , null ,null),
            new tokenDefinition(TokenTypes.Keyword ,"Length"),
            new tokenDefinition(TokenTypes.Keyword ,"Count"),
            new tokenDefinition(TokenTypes.Keyword ,"Sin"),
            new tokenDefinition(TokenTypes.Keyword ,"Cos"),
            new tokenDefinition(TokenTypes.Keyword ,"Tan"),
            new NodoSuma(TokenTypes.Operator ,"+" , null , null),
            new NodoResta(TokenTypes.Operator ,"-",null , null),
            new NodoDivision(TokenTypes.Operator,"/",null ,null),
            new NodoMulti(TokenTypes.Operator ,"*" , null,null),
            new NodoMayor(TokenTypes.Operator,">",null,null),
            new NodoMenor(TokenTypes.Operator,"<",null,null),
            new tokenAsignacion(TokenTypes.Operator,"=",null,null),
            new NodoIgual(TokenTypes.Operator,"==" ,null ,null),
            new NodoDistinto(TokenTypes.Operator,"!=",null ,null),
            new NodoMayorIgual(TokenTypes.Operator,">=",null , null),
            new NodoMenorIgual(TokenTypes.Operator,"<=",null , null),
            new tokenDefinition(TokenTypes.Punctuation,"."),
            new tokenDefinition(TokenTypes.Punctuation,":"),
            new tokenDefinition(TokenTypes.Punctuation,","),
            new tokenDefinition(TokenTypes.Punctuation,";"),
            new tokenDefinition(TokenTypes.Punctuation,"%"),
            new tokenDefinition(TokenTypes.Punctuation,"||"),
            new tokenDefinition(TokenTypes.Punctuation,"'"), 
        };
       public static string [] numeros = new string [10] {"0","1","2","3","4","5","6","7","8","9"};
        public static List <tokenDefinition> tokens = new List<tokenDefinition>();
        //metodo que me guarda los tokens en una lista 
        public static List <tokenDefinition> Tokenizando(string input)
        {
          
                string a = "";
                bool tok = false ;
           //recorro el codigo fuente 
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != ' ')
                {
                   a += input[i];
                //miro si un token es un caracter 
                
                    for (int j = 0; j < TokenDefi.Count; j++)
                    {
                        //miro si es un token compuesto de simbolos
                        if (a == ">" || a == "<" || a == "=")
                        {
                            
                        bool M = simbolos(tokens ,i ,input ,a);
                        if (M)
                        {
                            i = i + 1;
                            a ="";
                            tok = true;
                        }
                        }
                        else if (!tok && a == TokenDefi[j].Value)
                        {
                            tokens.Add(TokenDefi[j]);
                            tok = true;
                            a = "";
                        }
                    }
                  
                
                //miro a ver si es un token de literales 
                if (!tok)
                {
                    
                    if (a == "\""  )
                    {
                       i =  cadena(i , input , tokens);
                       a = "";
                       continue ;
                    }
                    //miro si es un token de numeros 
                    else if (numeros.Contains(a) )
                    {
                       i = numeral(numeros , a , input , i, tokens);
                       continue;
                    }
                    else
                    {
                        i = Identidad(input ,i, tokens, a);
                    }
                }
                }

            }
            return tokens;
            }

        
        private static int cadena (int I , string input , List<tokenDefinition> tokens )
        {
            string b = "";
            int B = 0 ;
            for (int i = I + 1; i < input.Length; i++)
            {
                
                if (input[i] != '"')
                {
                    b += input[i];
                }
                else
                {
                    tokens.Add(new tokenLiteral(TokenTypes.Literal , b));
                    B = i ;
                }
            }
            return B;
        }
        private static bool simbolos(List<tokenDefinition> tokens , int I ,string input , string a)
        {
            bool si = false;
            for (int i = I; i < input.Length; i++)
            {
                 for (int j = 0; j < tokens.Count; j++)
                    {
                        
                            a += input[i + 1];
                            if (a == ">=" || a == "<=" || a == "==")
                            {
                            if ( a == "<=")
                            {
                                tokens.Add(new  NodoMenorIgual(TokenTypes.Operator ,"<=" , null,null));
                                si = true ;
                              
                            }
                            if(a == ">=")
                            {
                                tokens.Add(new NodoMayorIgual(TokenTypes.Operator ,">=",null,null));
                                si = true ;
                              
                            }
                            if (a == "==")
                            {
                                tokens.Add(new NodoIgual(TokenTypes.Operator , "==" ,null ,null));
                                si = true;
                              
                            }
                            }
                        
                       
                    }
            }
            return si ;
        }
        private static int numeral (string [] numeros , string a ,string input , int I , List<tokenDefinition> tokens)
        {
            int b = 0;
            for (int i = I + 1; i < input.Length; i++)
            {
                if (numeros.Contains(input[i].ToString()))
                {
                    a += input[i];
                }
                else
                {
                    tokens.Add(new tokenNumero(TokenTypes.Number ,a));
                    b = i;
                    
                }
                
            }
            return b ;
        }
        private static int Identidad(string input,int I ,List<tokenDefinition> tokens, string a )
        {
            int b = 0;
            for (int i = I + 1; i < input.Length; i++)
            {
                if (input[i] != ' ')
                {
                    a+=input[i];
                }
                else
                {
                        tokens.Add(new tokenDefinition(TokenTypes.Identifier,a));
                        b = i;
                        
                }
            }
          return b;
        }
        

}

}


using tokenizar;
using AST;
namespace produccion
{
    public class reglas 

    {
        public void Errores(string input)
        {
            List <tokenDefinition> tokens = tokenizar.Tokenizer.Tokenizando(input);
            List<tokenIdentidad> identidad = new List<tokenIdentidad>();
             if (tokens[tokens.Count - 1].Value != ";")
            {
                throw new ArgumentException("SINTAXIS ERROR : SE ESPERABA UN ;");
            }
            for (int i = 0; i < tokens.Count; i++)
            {
                //exepcion si tenemos varias variables que se llaman igual 
                if (tokens[i].Type == TokenTypes.Identifier)
                {
                   tokenIdentidad  a =  (tokenIdentidad)tokens[i];
                   a.Evaluar(tokens);
                }
              
              if (tokens[i].Value == "let" && tokens[i + 1 ].Type != TokenTypes.Identifier)
              {
                throw new ArgumentException ("Invalid token " + tokens[i+ 1 ] + "in - let");
              }
              if (tokens[i].Value == "=" && tokens[i + 1].Type != TokenTypes.Number || tokens[i +1].Type != TokenTypes.Literal)
              {
                throw new ArgumentException("SYNTAXIS ERROR : Missing expresion in - let " );
              }
         }
        }
       
    }
}
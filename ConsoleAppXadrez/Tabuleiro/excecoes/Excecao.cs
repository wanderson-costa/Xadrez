using System;

namespace tabuleiro.excecoes
{
    class Excecao : Exception
    {
        public Excecao(string msg) : base(msg)
        { 
        }
    }
}

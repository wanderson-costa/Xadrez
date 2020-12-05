using System;
using tabuleiro;
using tabuleiro.enums;
using tabuleiro.excecoes;
using xadrez;

namespace ConsoleAppXadrez
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.colocarPeca(new Torre(tab, Cor.Preta), new Posicao(0, 0));
                tab.colocarPeca(new Torre(tab, Cor.Preta), new Posicao(1, 2));
                tab.colocarPeca(new Rei(tab, Cor.Preta), new Posicao(0, 1));
                tab.colocarPeca(new Torre(tab, Cor.Branca), new Posicao(3, 5));

                Tela.imprimirTabuleiro(tab);
            }
            catch (Excecao e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}

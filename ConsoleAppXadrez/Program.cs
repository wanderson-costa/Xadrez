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
                PartidaXadrez partida = new PartidaXadrez();

                while (!partida.finalizada)
                {
                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tabuleiro);

                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Posicao origem = Tela.lerPosicaoXadrez().toPosicao();

                    Console.Write("Destino: ");
                    Posicao destino = Tela.lerPosicaoXadrez().toPosicao();

                    partida.executarMovimento(origem, destino);

                }
                
            }
            catch (Excecao e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}

using System;
using tabuleiro;
using tabuleiro.enums;

namespace xadrez
{
    class PartidaXadrez
    {
        public Tabuleiro tabuleiro { get; private set; }
        public bool finalizada { get; private set; }
        
        private int turno;
        private Cor jogadorAtual;

        public PartidaXadrez()
        {
            this.tabuleiro = new Tabuleiro(8, 8);
            this.finalizada = false;
            turno = 1;
            jogadorAtual = Cor.Branca;
            colocarPecas();
        }

        public void executarMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaOrigem = this.tabuleiro.retirarPeca(origem);
            pecaOrigem.incrementarMovimento();
            Peca pecaCapturada = this.tabuleiro.retirarPeca(destino);
            this.tabuleiro.colocarPeca(pecaOrigem, destino);
        }

        private void colocarPecas()
        {
            this.tabuleiro.colocarPeca(new Torre(this.tabuleiro, Cor.Branca), new PosicaoXadrez('c', 1).toPosicao());
            this.tabuleiro.colocarPeca(new Torre(this.tabuleiro, Cor.Branca), new PosicaoXadrez('c', 2).toPosicao());
            this.tabuleiro.colocarPeca(new Torre(this.tabuleiro, Cor.Branca), new PosicaoXadrez('d', 2).toPosicao());
            this.tabuleiro.colocarPeca(new Torre(this.tabuleiro, Cor.Branca), new PosicaoXadrez('e', 2).toPosicao());
            this.tabuleiro.colocarPeca(new Torre(this.tabuleiro, Cor.Branca), new PosicaoXadrez('e', 1).toPosicao());
            this.tabuleiro.colocarPeca(new Rei(this.tabuleiro, Cor.Branca), new PosicaoXadrez('d', 1).toPosicao());

            this.tabuleiro.colocarPeca(new Torre(this.tabuleiro, Cor.Preta), new PosicaoXadrez('c', 7).toPosicao());
            this.tabuleiro.colocarPeca(new Torre(this.tabuleiro, Cor.Preta), new PosicaoXadrez('c', 8).toPosicao());
            this.tabuleiro.colocarPeca(new Torre(this.tabuleiro, Cor.Preta), new PosicaoXadrez('d', 7).toPosicao());
            this.tabuleiro.colocarPeca(new Torre(this.tabuleiro, Cor.Preta), new PosicaoXadrez('e', 7).toPosicao());
            this.tabuleiro.colocarPeca(new Torre(this.tabuleiro, Cor.Preta), new PosicaoXadrez('e', 8).toPosicao());
            this.tabuleiro.colocarPeca(new Rei(this.tabuleiro, Cor.Preta), new PosicaoXadrez('d', 8).toPosicao());
        }
    }
}

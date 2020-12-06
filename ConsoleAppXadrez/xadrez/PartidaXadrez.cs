using System;
using tabuleiro;
using tabuleiro.enums;
using tabuleiro.excecoes;

namespace xadrez
{
    class PartidaXadrez
    {
        public Tabuleiro tabuleiro { get; private set; }
        public bool finalizada { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }

        public PartidaXadrez()
        {
            this.tabuleiro = new Tabuleiro(8, 8);
            this.finalizada = false;
            this.turno = 1;
            this.jogadorAtual = Cor.Branca;
            colocarPecas();
        }

        private void executarMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaOrigem = this.tabuleiro.retirarPeca(origem);
            pecaOrigem.incrementarMovimentos();
            Peca pecaCapturada = this.tabuleiro.retirarPeca(destino);
            this.tabuleiro.colocarPeca(pecaOrigem, destino);
        }

        private void alternarJogador()
        {
            this.jogadorAtual = this.jogadorAtual == Cor.Branca ? Cor.Preta : Cor.Branca;
        }

        public void realizarJogada(Posicao origem, Posicao destino)
        {
            executarMovimento(origem, destino);
            alternarJogador();
            this.turno++;
        }

        public void validarPosicaoOrigem(Posicao posicao)
        {
            if (this.tabuleiro.peca(posicao) == null) {
                throw new Excecao("Não existe peça na posição escolhida!");
            }

            if (this.jogadorAtual != this.tabuleiro.peca(posicao).cor)
            {
                throw new Excecao("A peça de origem escolhida não é sua!");
            }

            if (!this.tabuleiro.peca(posicao).existeMovimentosPossiveis())
            {
                throw new Excecao("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!this.tabuleiro.peca(origem).podeMoverPara(destino))
            {
                throw new Excecao("Posição de destino inválida!");
            }
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

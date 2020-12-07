using System.Collections.Generic;
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

        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public PartidaXadrez()
        {
            this.tabuleiro = new Tabuleiro(8, 8);
            this.finalizada = false;
            this.turno = 1;
            this.jogadorAtual = Cor.Branca;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        private void executarMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaOrigem = this.tabuleiro.retirarPeca(origem);
            pecaOrigem.incrementarMovimentos();
            Peca pecaCapturada = this.tabuleiro.retirarPeca(destino);
            this.tabuleiro.colocarPeca(pecaOrigem, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
        }

        private void alternarJogador()
        {
            this.jogadorAtual = this.jogadorAtual == Cor.Branca ? Cor.Preta : Cor.Branca;
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }
        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        public void realizarJogada(Posicao origem, Posicao destino)
        {
            executarMovimento(origem, destino);
            alternarJogador();
            this.turno++;
        }

        public void validarPosicaoOrigem(Posicao posicao)
        {
            if (this.tabuleiro.peca(posicao) == null)
            {
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

        private void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            this.tabuleiro.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            this.pecas.Add(peca);
        }
        private void colocarPecas()
        {
            colocarNovaPeca('c', 1, new Torre(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('c', 2, new Torre(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('d', 2, new Torre(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('e', 2, new Torre(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('e', 1, new Torre(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('d', 1, new Rei(this.tabuleiro, Cor.Branca));

            colocarNovaPeca('c', 7, new Torre(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('c', 8, new Torre(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('d', 7, new Torre(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('e', 7, new Torre(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('e', 8, new Torre(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('d', 8, new Rei(this.tabuleiro, Cor.Preta)) ;
        }
    }
}

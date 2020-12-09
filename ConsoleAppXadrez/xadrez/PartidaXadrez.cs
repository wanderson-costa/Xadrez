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
        public bool xeque { get; private set; }

        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public PartidaXadrez()
        {
            this.tabuleiro = new Tabuleiro(8, 8);
            this.finalizada = false;
            this.turno = 1;
            this.jogadorAtual = Cor.Branca;
            this.xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        private Peca executarMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaOrigem = this.tabuleiro.retirarPeca(origem);
            pecaOrigem.incrementarMovimentos();
            Peca pecaCapturada = this.tabuleiro.retirarPeca(destino);
            this.tabuleiro.colocarPeca(pecaOrigem, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        private void desfazerMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca pecaDestino = this.tabuleiro.retirarPeca(destino);
            pecaDestino.decrementarMovimentos();

            if (pecaCapturada != null)
            {
                this.tabuleiro.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            this.tabuleiro.colocarPeca(pecaDestino, origem);
        }

        private void alternarJogador()
        {
            this.jogadorAtual = this.jogadorAtual == Cor.Branca ? Cor.Preta : Cor.Branca;
        }

        private Cor pecaAdversaria(Cor cor)
        {
            return cor == Cor.Branca ? Cor.Preta : Cor.Branca;
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca peca in pecasEmJogo(cor))
            {
                if (peca is Rei)
                {
                    return peca;
                }
            }
            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca peca = rei(cor);
            if (peca == null)
            {
                throw new Excecao("Não há rei da cor " + cor + "tabuleiro");
            }
            foreach (Peca objPeca in pecasEmJogo(pecaAdversaria(cor)))
            {
                bool[,] mat = objPeca.movimentosPossiveis();
                if (mat[peca.posicao.linha, peca.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool estaEmXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca objPeca in pecasEmJogo(cor))
            {
                bool[,] mat = objPeca.movimentosPossiveis();
                for (int i = 0; i < this.tabuleiro.linhas; i++)
                {
                    for (int j = 0; j < this.tabuleiro.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = objPeca.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executarMovimento(origem, destino);
                            bool xeque = estaEmXeque(cor);
                            desfazerMovimento(origem, destino, pecaCapturada);
                            if (!xeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> objPeca = new HashSet<Peca>();
            foreach (Peca peca in capturadas)
            {
                if (peca.cor == cor)
                {
                    objPeca.Add(peca);
                }
            }
            return objPeca;
        }
        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> objPeca = new HashSet<Peca>();
            foreach (Peca peca in pecas)
            {
                if (peca.cor == cor)
                {
                    objPeca.Add(peca);
                }
            }
            objPeca.ExceptWith(pecasCapturadas(cor));
            return objPeca;
        }

        public void realizarJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executarMovimento(origem, destino);
            if (estaEmXeque(jogadorAtual))
            {
                desfazerMovimento(origem, destino, pecaCapturada);
                throw new Excecao("Você não pode se colocar em xeque!");
            }

            this.xeque = estaEmXeque(pecaAdversaria(jogadorAtual)) ? true : false;
            if (estaEmXequemate(pecaAdversaria(jogadorAtual)))
            {
                this.finalizada = true;
            }
            else
            {
                alternarJogador();
                this.turno++;
            }
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
            if (!this.tabuleiro.peca(origem).movimentoPossivel(destino))
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
            /*
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
            colocarNovaPeca('d', 8, new Rei(this.tabuleiro, Cor.Preta));
            */
            colocarNovaPeca('c', 1, new Torre(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('d', 1, new Rei(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('h', 7, new Torre(this.tabuleiro, Cor.Branca));

            colocarNovaPeca('a', 8, new Rei(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('b', 8, new Torre(this.tabuleiro, Cor.Preta));
        }
    }
}

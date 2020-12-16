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
        public Peca capturaEspecial { get; private set; }

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
            capturaEspecial = null;
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

            // #jogadas especiais - roque pequeno
            if (pecaOrigem is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca pecaT = this.tabuleiro.retirarPeca(origemT);
                pecaT.incrementarMovimentos();
                this.tabuleiro.colocarPeca(pecaT, destinoT);
            }

            // #jogadas especiais - roque grande
            if (pecaOrigem is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca pecaT = this.tabuleiro.retirarPeca(origemT);
                pecaT.incrementarMovimentos();
                this.tabuleiro.colocarPeca(pecaT, destinoT);
            }

            // #jogadas especiais - en passant
            if (pecaOrigem is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == null) 
                {
                    Posicao origemP;
                    if (pecaOrigem.cor == Cor.Branca)
                    {
                        origemP = new Posicao(destino.linha + 1, destino.coluna);
                    } 
                    else
                    {
                        origemP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    pecaCapturada = this.tabuleiro.retirarPeca(origemP);
                    capturadas.Add(pecaCapturada);
                }
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

            // #jogadas especiais - roque pequeno
            if (pecaDestino is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca pecaT = this.tabuleiro.retirarPeca(destinoT);
                pecaT.decrementarMovimentos();
                this.tabuleiro.colocarPeca(pecaT, origemT);
            }

            // #jogadas especiais - roque grande
            if (pecaDestino is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca pecaT = this.tabuleiro.retirarPeca(destinoT);
                pecaT.decrementarMovimentos();
                this.tabuleiro.colocarPeca(pecaT, origemT);
            }

            // #jogadas especiais - en passant
            if (pecaDestino is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == capturaEspecial)
                {
                    Peca peao = tabuleiro.retirarPeca(destino);
                    Posicao destinoP;
                    if (pecaDestino.cor == Cor.Branca)
                    {
                        destinoP = new Posicao(3, destino.coluna);
                    }
                    else
                    {
                        destinoP = new Posicao(4, destino.coluna);
                    }
                    this.tabuleiro.colocarPeca(peao, destinoP);
                }
            }
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

            Peca peca = this.tabuleiro.peca(destino);

            // #jogadas especiais - promoção
            if (peca is Peao)
            {
                if ((peca.cor == Cor.Branca && destino.linha == 0) || (peca.cor == Cor.Preta && destino.linha == 7))
                {
                    peca = this.tabuleiro.retirarPeca(destino);
                    pecas.Remove(peca);
                    Peca dama = new Dama(this.tabuleiro, peca.cor);
                    this.tabuleiro.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
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

            // #jogadas especiais - en passant
            if (peca is Peao && (destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2))
            {
                capturaEspecial = peca;
            }
            else
            {
                capturaEspecial = null;
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
            colocarNovaPeca('a', 1, new Torre(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('b', 1, new Cavalo(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('c', 1, new Bispo(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('d', 1, new Dama(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('e', 1, new Rei(this.tabuleiro, Cor.Branca, this));
            colocarNovaPeca('f', 1, new Bispo(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('g', 1, new Cavalo(this.tabuleiro, Cor.Branca));
            colocarNovaPeca('h', 1, new Torre(this.tabuleiro, Cor.Branca));

            colocarNovaPeca('a', 2, new Peao(this.tabuleiro, Cor.Branca, this));
            colocarNovaPeca('b', 2, new Peao(this.tabuleiro, Cor.Branca, this));
            colocarNovaPeca('c', 2, new Peao(this.tabuleiro, Cor.Branca, this));
            colocarNovaPeca('d', 2, new Peao(this.tabuleiro, Cor.Branca, this));
            colocarNovaPeca('e', 2, new Peao(this.tabuleiro, Cor.Branca, this));
            colocarNovaPeca('f', 2, new Peao(this.tabuleiro, Cor.Branca, this));
            colocarNovaPeca('g', 2, new Peao(this.tabuleiro, Cor.Branca, this));
            colocarNovaPeca('h', 2, new Peao(this.tabuleiro, Cor.Branca, this));

            colocarNovaPeca('a', 8, new Torre(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('b', 8, new Cavalo(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('c', 8, new Bispo(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('d', 8, new Dama(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('e', 8, new Rei(this.tabuleiro, Cor.Preta, this));
            colocarNovaPeca('f', 8, new Bispo(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('g', 8, new Cavalo(this.tabuleiro, Cor.Preta));
            colocarNovaPeca('h', 8, new Torre(this.tabuleiro, Cor.Preta));

            colocarNovaPeca('a', 7, new Peao(this.tabuleiro, Cor.Preta, this));
            colocarNovaPeca('b', 7, new Peao(this.tabuleiro, Cor.Preta, this));
            colocarNovaPeca('c', 7, new Peao(this.tabuleiro, Cor.Preta, this));
            colocarNovaPeca('d', 7, new Peao(this.tabuleiro, Cor.Preta, this));
            colocarNovaPeca('e', 7, new Peao(this.tabuleiro, Cor.Preta, this));
            colocarNovaPeca('f', 7, new Peao(this.tabuleiro, Cor.Preta, this));
            colocarNovaPeca('g', 7, new Peao(this.tabuleiro, Cor.Preta, this));
            colocarNovaPeca('h', 7, new Peao(this.tabuleiro, Cor.Preta, this));
        }
    }
}

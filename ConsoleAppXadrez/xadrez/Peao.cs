using tabuleiro;
using tabuleiro.enums;

namespace xadrez
{
    class Peao : Peca
    {
        private PartidaXadrez partida;

        public Peao(Tabuleiro tabuleiro, Cor cor, PartidaXadrez partida) : base(tabuleiro, cor)
        {
            this.partida = partida;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool podeMover(Posicao posicao)
        {
            Peca peca = base.tabuleiro.peca(posicao);
            return peca == null || peca.cor != base.cor;
        }

        private bool existeInimigo(Posicao posicao)
        {
            Peca peca = base.tabuleiro.peca(posicao);
            return peca != null && peca.cor != base.cor;
        }

        private bool livre(Posicao posicao)
        {
            return base.tabuleiro.peca(posicao) == null;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[base.tabuleiro.linhas, base.tabuleiro.colunas];
            Posicao pos = new Posicao(0, 0);

            if (base.cor == Cor.Branca)
            {
                pos.definirValores(base.posicao.linha - 1, base.posicao.coluna);
                if (base.tabuleiro.posicaoValida(pos) && livre(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.definirValores(base.posicao.linha - 2, base.posicao.coluna);
                Posicao pos2 = new Posicao(base.posicao.linha - 1, base.posicao.coluna);
                if (base.tabuleiro.posicaoValida(pos2) && livre(pos2) && base.tabuleiro.posicaoValida(pos) && livre(pos) && base.qtdMovimento == 0)
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.definirValores(base.posicao.linha - 1, base.posicao.coluna - 1);
                if (base.tabuleiro.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.definirValores(base.posicao.linha - 1, base.posicao.coluna + 1);
                if (base.tabuleiro.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                // #jogadas especiais - en oassant
                if (base.posicao.linha == 3)
                {
                    Posicao posEsquerda = new Posicao(base.posicao.linha, base.posicao.coluna - 1);
                    if (base.tabuleiro.posicaoValida(posEsquerda) && existeInimigo(posEsquerda) && base.tabuleiro.peca(posEsquerda) == this.partida.capturaEspecial)
                    {
                        mat[posEsquerda.linha - 1, posEsquerda.coluna] = true;
                    }

                    Posicao posDireita = new Posicao(base.posicao.linha, base.posicao.coluna + 1);
                    if (base.tabuleiro.posicaoValida(posDireita) && existeInimigo(posDireita) && base.tabuleiro.peca(posDireita) == this.partida.capturaEspecial)
                    {
                        mat[posDireita.linha - 1, posDireita.coluna] = true;
                    }
                }
            }
            else
            {
                pos.definirValores(base.posicao.linha + 1, base.posicao.coluna);
                if (base.tabuleiro.posicaoValida(pos) && livre(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.definirValores(base.posicao.linha + 2, base.posicao.coluna);
                Posicao pos2 = new Posicao(posicao.linha + 1, posicao.coluna);
                if (base.tabuleiro.posicaoValida(pos2) && livre(pos2) && base.tabuleiro.posicaoValida(pos) && livre(pos) && base.qtdMovimento == 0)
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.definirValores(base.posicao.linha + 1, base.posicao.coluna - 1);
                if (base.tabuleiro.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                pos.definirValores(base.posicao.linha + 1, base.posicao.coluna + 1);
                if (base.tabuleiro.posicaoValida(pos) && existeInimigo(pos))
                {
                    mat[pos.linha, pos.coluna] = true;
                }
                // #jogadas especiais - en oassant
                if (base.posicao.linha == 4)
                {
                    Posicao posEsquerda = new Posicao(base.posicao.linha, base.posicao.coluna - 1);
                    if (base.tabuleiro.posicaoValida(posEsquerda) && existeInimigo(posEsquerda) && base.tabuleiro.peca(posEsquerda) == this.partida.capturaEspecial)
                    {
                        mat[posEsquerda.linha + 1, posEsquerda.coluna] = true;
                    }

                    Posicao posDireita = new Posicao(base.posicao.linha, base.posicao.coluna + 1);
                    if (base.tabuleiro.posicaoValida(posDireita) && existeInimigo(posDireita) && base.tabuleiro.peca(posDireita) == this.partida.capturaEspecial)
                    {
                        mat[posDireita.linha + 1 , posDireita.coluna] = true;
                    }
                }
            }
            return mat;
        }
    }
}

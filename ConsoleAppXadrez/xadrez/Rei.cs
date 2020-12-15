using tabuleiro;
using tabuleiro.enums;

namespace xadrez
{
    class Rei : Peca
    {
        private PartidaXadrez partida;

        public Rei(Tabuleiro tabuleiro, Cor cor, PartidaXadrez partida) : base(tabuleiro, cor)
        {
            this.partida = partida;
        }

        public override string ToString()
        {
            return "R";
        }

        private bool podeMover(Posicao posicao)
        {
            Peca peca = base.tabuleiro.peca(posicao);
            return peca == null || peca.cor != base.cor;
        }

        private bool testaTorreElegivelRoque(Posicao posicao)
        {
            Peca peca = base.tabuleiro.peca(posicao);
            return peca != null && peca is Torre && peca.cor == base.cor && peca.qtdMovimento == 0;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[base.tabuleiro.linhas, base.tabuleiro.colunas];
            Posicao pos = new Posicao(0, 0);

            //acima
            pos.definirValores(base.posicao.linha - 1, base.posicao.coluna);
            if (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //ne
            pos.definirValores(base.posicao.linha - 1, base.posicao.coluna + 1);
            if (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //direita
            pos.definirValores(base.posicao.linha, base.posicao.coluna + 1);
            if (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //se
            pos.definirValores(base.posicao.linha + 1, base.posicao.coluna + 1);
            if (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //abaixo
            pos.definirValores(base.posicao.linha + 1, base.posicao.coluna);
            if (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //so
            pos.definirValores(base.posicao.linha + 1, base.posicao.coluna - 1);
            if (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //esquerda
            pos.definirValores(base.posicao.linha, base.posicao.coluna - 1);
            if (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            //no
            pos.definirValores(base.posicao.linha - 1, base.posicao.coluna - 1);
            if (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
            }

            // #jogadas especiais - roque
            if (base.qtdMovimento == 0 && !this.partida.xeque)
            {
                //roque pequeno
                Posicao posT1 = new Posicao(base.posicao.linha, base.posicao.coluna + 3);

                if (testaTorreElegivelRoque(posT1))
                {
                    Posicao p1 = new Posicao(base.posicao.linha, base.posicao.coluna + 1);
                    Posicao p2 = new Posicao(base.posicao.linha, base.posicao.coluna + 2);

                    if (base.tabuleiro.peca(p1) == null && base.tabuleiro.peca(p2) == null)
                    {
                        mat[base.posicao.linha, base.posicao.coluna + 2] = true;
                    }
                }

                //roque grande
                Posicao posT2 = new Posicao(base.posicao.linha, base.posicao.coluna - 4);

                if (testaTorreElegivelRoque(posT2))
                {
                    Posicao p1 = new Posicao(base.posicao.linha, base.posicao.coluna - 1);
                    Posicao p2 = new Posicao(base.posicao.linha, base.posicao.coluna - 2);
                    Posicao p3 = new Posicao(base.posicao.linha, base.posicao.coluna - 3);

                    if (base.tabuleiro.peca(p1) == null && base.tabuleiro.peca(p2) == null && base.tabuleiro.peca(p3) == null)
                    {
                        mat[base.posicao.linha, base.posicao.coluna - 2] = true;
                    }
                }
            }
            return mat;
        }
    }
}

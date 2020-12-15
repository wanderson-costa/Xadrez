using tabuleiro;
using tabuleiro.enums;

namespace xadrez
{
    class Torre : Peca
    {
        public Torre(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor)
        {

        }

        public override string ToString()
        {
            return "T";
        }

        private bool podeMover(Posicao posicao)
        {
            Peca peca = base.tabuleiro.peca(posicao);
            return peca == null || peca.cor != base.cor;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[base.tabuleiro.linhas, base.tabuleiro.colunas];
            Posicao pos = new Posicao(0, 0);

            //acima
            pos.definirValores(base.posicao.linha - 1, base.posicao.coluna);
            while (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (base.tabuleiro.peca(pos) != null && base.tabuleiro.peca(pos).cor != base.cor)
                {
                    break;
                }
                pos.linha = pos.linha - 1;
            }

            //abaixo
            pos.definirValores(base.posicao.linha + 1, base.posicao.coluna);
            while (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (base.tabuleiro.peca(pos) != null && base.tabuleiro.peca(pos).cor != base.cor)
                {
                    break;
                }
                pos.linha = pos.linha + 1;
            }

            //direita
            pos.definirValores(base.posicao.linha, base.posicao.coluna + 1);
            while (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (base.tabuleiro.peca(pos) != null && base.tabuleiro.peca(pos).cor != base.cor)
                {
                    break;
                }
                pos.coluna = pos.coluna + 1;
            }

            //esquerda
            pos.definirValores(base.posicao.linha, base.posicao.coluna - 1);
            while (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (base.tabuleiro.peca(pos) != null && base.tabuleiro.peca(pos).cor != base.cor)
                {
                    break;
                }
                pos.coluna = pos.coluna - 1;
            }
            return mat;
        }
    }
}

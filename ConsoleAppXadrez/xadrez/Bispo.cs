﻿using tabuleiro;
using tabuleiro.enums;

namespace xadrez
{
    class Bispo : Peca
    {
        public Bispo(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor)
        {

        }
        
        public override string ToString()
        {
            return "B";
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

            //no
            pos.definirValores(base.posicao.linha - 1, base.posicao.coluna - 1);
            while (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (base.tabuleiro.peca(pos) != null && base.tabuleiro.peca(pos).cor != base.cor)
                {
                    break;
                }
                pos.definirValores(pos.linha - 1, pos.coluna - 1);
            }

            //ne
            pos.definirValores(base.posicao.linha - 1, base.posicao.coluna + 1);
            while (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (base.tabuleiro.peca(pos) != null && base.tabuleiro.peca(pos).cor != base.cor)
                {
                    break;
                }
                pos.definirValores(pos.linha - 1, pos.coluna + 1);
            }

            //se
            pos.definirValores(base.posicao.linha + 1, base.posicao.coluna + 1);
            while (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (base.tabuleiro.peca(pos) != null && base.tabuleiro.peca(pos).cor != base.cor)
                {
                    break;
                }
                pos.definirValores(pos.linha + 1, pos.coluna + 1);
            }

            //so
            pos.definirValores(base.posicao.linha + 1, base.posicao.coluna - 1);
            while (base.tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (base.tabuleiro.peca(pos) != null && base.tabuleiro.peca(pos).cor != base.cor)
                {
                    break;
                }
                pos.definirValores(pos.linha + 1, pos.coluna - 1);
            }
            return mat;
        }
    }
}

﻿using Tabuleiro.Enums;

namespace Tabuleiro
{
    class Peca
    {
        public Posicao posicao { get; set; }
        public Tabuleiro tabuleiro { get; set; }
        public Cor cor { get; protected set; }
        public int qtdMovimento { get; protected set; }

        public Peca()
        {
        }

        public Peca(Posicao posicao, Tabuleiro tabuleiro, Cor cor)
        {
            this.posicao = posicao;
            this.tabuleiro = tabuleiro;
            this.cor = cor;
            this.qtdMovimento = 0;
        }
    }
}

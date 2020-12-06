using tabuleiro.enums;

namespace tabuleiro
{
    abstract class Peca
    {
        public Posicao posicao { get; set; }
        public Tabuleiro tabuleiro { get; set; }
        public Cor cor { get; protected set; }
        public int qtdMovimento { get; protected set; }

        public Peca(Tabuleiro tabuleiro, Cor cor)
        {
            this.posicao = null;
            this.tabuleiro = tabuleiro;
            this.cor = cor;
            this.qtdMovimento = 0;
        }

        public void incrementarMovimentos() 
        {
            this.qtdMovimento++;
        }

        public abstract bool[,] movimentosPossiveis();
    }
}

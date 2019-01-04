using EnglynionBedd.Endidau;
using System.Threading.Tasks;

namespace EnglynionBedd.Gwasanaethau
{
    public interface IGwasanaethauGwybodol
    {
        Task<GwybodaethDelwedd> DadansoddiTestun(byte[] delwedd, bool argraffedig);
    }
}
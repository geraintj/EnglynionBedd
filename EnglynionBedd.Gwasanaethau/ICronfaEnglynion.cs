using System.Collections.Generic;
using System.Threading.Tasks;
using EnglynionBedd.Endidau;

namespace EnglynionBedd.Gwasanaethau
{
    public interface ICronfaEnglynion
    {
        Task<Englyn> ArbedEnglyn(Englyn englyn);
        Task<Englyn> AdalwEnglyn(string id);
        Task<List<Englyn>> AdalwEnglynion();
        Task<string> ArbedDelwedd(byte[] delwedd);
        Task<Englyn> GolyguEnglyn(Englyn englyn);
    }
}
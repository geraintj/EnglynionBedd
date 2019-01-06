using System.Collections.Generic;
using System.Threading.Tasks;
using EnglynionBedd.Endidau;

namespace EnglynionBedd.Gwasanaethau
{
    public interface ICronfaBeddargraff
    {
        Task<string> ArbedBeddargraff(Beddargraff beddargraffiad);
        Task<Beddargraff> AdalwBeddargraff(string id);
        Task<List<Beddargraff>> AdalwBeddargraffiadau();
        Task<string> ArbedDelwedd(byte[] delwedd);
    }
}
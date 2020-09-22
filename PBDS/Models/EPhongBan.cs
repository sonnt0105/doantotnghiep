using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class EPhongBan
    {
        int idphongban;
        string tenphongban;
        string mota;

        public EPhongBan (int idphongban, string tenphongban, string mota)
        {
            this.idphongban = idphongban;
            this.tenphongban = tenphongban;
            this.mota = mota;
        }
    }
}
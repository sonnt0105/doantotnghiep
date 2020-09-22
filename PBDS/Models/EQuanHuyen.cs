using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class EQuanHuyen
    {
        int idquanhuyen;
        string ten;
        int idtinhthanhpho;

        public EQuanHuyen(int idquanhuyen, string ten, int idtinhthanhpho)
        {
            this.idquanhuyen = idquanhuyen;
            this.ten = ten;
            this.idtinhthanhpho = idtinhthanhpho;
        }
    }
}
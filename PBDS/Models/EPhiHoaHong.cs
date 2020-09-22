using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class EPhiHoaHong
    {
        int idhoahong;
        int sales;
        decimal tongtien;
        int nguoitao;
        DateTime ngaytao;

        public EPhiHoaHong(int idhoahong,int sales, decimal tongtien,int nguoitao,DateTime ngaytao)
        {
            this.idhoahong = idhoahong;
            this.sales = sales;
            this.tongtien = tongtien;
            this.nguoitao = nguoitao;
            this.ngaytao = ngaytao;
        }
    }
}
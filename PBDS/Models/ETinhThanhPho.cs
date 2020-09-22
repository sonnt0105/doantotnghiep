using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBDS.Models
{
    public class ETinhThanhPho
    {
        int idtinhthanhpho { get; set; }
        string tentinhthanhpho { get; set; }
        


        public ETinhThanhPho (int idtinhthanhpho, string tentinhthanhpho)
        {
            this.idtinhthanhpho = idtinhthanhpho;
            this.tentinhthanhpho = tentinhthanhpho;
        }

        public ETinhThanhPho()
        {

        }
    }
}
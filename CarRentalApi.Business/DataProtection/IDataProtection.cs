using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.DataProtection
{
    public interface IDataProtection
    {
        public string Protect(string text);  // gönderilen bir metni şifreleyip geri dönecek method
        public string UnProtect(string protectedText); // gelen şifreli metni normal metne çevircek buda
    }
}

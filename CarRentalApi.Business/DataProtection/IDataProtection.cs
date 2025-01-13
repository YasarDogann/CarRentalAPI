using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.DataProtection
{
    public interface IDataProtection
    {
        public string Protect(string text);  // gönderilen metni şifreli hale getircek
        public string UnProtect(string protectedText);
    }
}

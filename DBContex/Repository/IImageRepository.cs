using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBContex.Models;

namespace DBContex.Repository
{
    public interface IImageRepository
    {
        void AddImages(IEnumerable<Image> images);
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBContex.Models;

namespace DBContex.Repository
{
    public class ImageRepository : IImageRepository
    {
        private Context db;

        public ImageRepository(Context context)
        {
            db = context;
        }

        public void AddImages(IEnumerable<Image> images)
        {
            if (images != null)
            {
                foreach (var image in images)
                {
                    db.Images.Add(image);
                }
                db.SaveChanges();
            }
        }
    }
}

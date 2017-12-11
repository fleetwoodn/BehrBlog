using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


namespace BehrBlog.Models
{
    public class Picts
    {
        public int ID { get; set; }
        public int PostFK { get; set; }
        public string PicTitle { get; set; }
        public string EditDate { get; set; }
        public string PictPict { get; set; }
    }
}
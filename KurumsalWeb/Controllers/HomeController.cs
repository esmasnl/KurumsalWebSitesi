using KurumsalWeb.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace KurumsalWeb.Controllers
{
    public class HomeController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Home
        public ActionResult Index()
        {
           

            Degerler();
            return View();
        }

        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(x => x.SliderId));
        }
        public ActionResult HizmetPartial()
        {
            return View(db.Hizmet.ToList());
        }

        public ActionResult Hakkimizda()
        {
            Degerler();
            return View(db.Hakkimizda.SingleOrDefault());
        }

        public ActionResult Hizmetlerimiz()
        {
            Degerler();
            return View(db.Hizmet.ToList().OrderByDescending(x => x.HizmetId));
        }
        public ActionResult Iletisim()
        {
            Degerler();
            return View(db.Iletisim.SingleOrDefault());
        }

        [HttpPost]
        public ActionResult Iletisim(string adsoyad = null, string email = null, string konu = null, string mesaj = null)
        {
            if (adsoyad != null && email != null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "esmaasnli1@gmail.com";
                WebMail.Password = "dzodglbsrbypsfrx";
                WebMail.SmtpPort = 587;
                WebMail.Send("esmaasnli1@gmail.com", konu, email + "</br>" + mesaj);
                ViewBag.Uyari = "Mesajınız Gönderilmiştir.";
            }
            else
            {
                ViewBag.Uyari = "Hata Oluştu. Tekrar Deneyiniz";
            }

            Degerler();
            return View();
        }

        public ActionResult Blog(int Sayfa = 1)
        {
            Degerler();
            return View(db.Blog.Include("Kategori").OrderByDescending(x=> x.BlogId).ToPagedList(Sayfa,5));
        }
        public ActionResult KategoriBlog(int id,int Sayfa=1) 
        
        {
            Degerler();
            var b= db.Blog.Include("Kategori").OrderByDescending(x=>x.BlogId).Where(x=> x.Kategori.KategoriId == id).ToPagedList(Sayfa,5);
            return View(b); 
        }   

        public ActionResult BlogDetay(int id)
        {
            var b = db.Blog.Include("Kategori").Where(x => x.BlogId == id).SingleOrDefault();

            Degerler();
            return View(b);
        }

        public ActionResult BlogKategoriPartial()
        {
            Degerler();
            return PartialView(db.Kategori.Include("Blogs").ToList().OrderBy(x => x.KategoriAd));
        }
        public ActionResult BlogKayıtPartial()
        {
            Degerler();
            return PartialView(db.Blog.ToList().OrderByDescending(x => x.BlogId));
        }

        public void Degerler()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);
            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();
            ViewBag.Blog = db.Blog.ToList().OrderByDescending(x => x.BlogId);
        }
    }
}
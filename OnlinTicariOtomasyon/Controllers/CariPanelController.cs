using OnlinTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlinTicariOtomasyon.Controllers
{
    public class CariPanelController : Controller
    {
        // GET: CariPanel
        Context c = new Context();
        [Authorize]

        public ActionResult Index()
        {
            var mail = (string)Session["CariMail"];
            var degerler = c.mesajlars.Where(x => x.Alıcı == mail).ToList();
            ViewBag.m = mail;
            var mailid = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.Cariid).FirstOrDefault();
            ViewBag.mid = mailid;
            var toplamsatis = c.SatisHarakets.Where(x => x.Cariid == mailid).Count();
            ViewBag.toplamsatis = toplamsatis;
            var toplamtuar = c.SatisHarakets.Where(x => x.Cariid == mailid).Sum(y => y.ToplamTutar);
            ViewBag.toplamtutar = toplamtuar;
            var toplamurunsayisi = c.SatisHarakets.Where(x => x.Cariid == mailid).Sum(y => y.Adet);
            ViewBag.toplamurunsayisi = toplamurunsayisi;
            var adsoyad = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.CariAd + " " + y.CariSoyad).FirstOrDefault();
            ViewBag.adsoyad = adsoyad;
            return View(degerler); 
        }

        public ActionResult Siparislerim()
        {
            var mail = (string)Session["CariMail"];
            var id = c.Carilers.Where(x => x.CariMail == mail.ToString()).Select(y => y.Cariid).FirstOrDefault();
            var degerler = c.SatisHarakets.Where(x => x.Cariid == id).ToList();
            return View(degerler);
        }

        public ActionResult GelenMesajlar()
        {
            var mail = (string)Session["CariMail"];
            var mesajlar = c.mesajlars.Where(x=>x.Alıcı==mail).OrderByDescending(x=>x.MesajID).ToList();
            var gelensayisi = c.mesajlars.Count(x => x.Alıcı == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.mesajlars.Count(x => x.Göndetici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View(mesajlar);
        }

        public ActionResult GidenMesajlar()
        {
            var mail = (string)Session["CariMail"];
            var mesajlar = c.mesajlars.Where(x => x.Göndetici == mail).OrderByDescending(x => x.MesajID).ToList();
            var gelensayisi = c.mesajlars.Count(x => x.Alıcı == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.mesajlars.Count(x => x.Göndetici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View(mesajlar);
        }

        public ActionResult MesajDetay(int id)
        {
            var degerler = c.mesajlars.Where(x => x.MesajID == id).ToList();
            var mail = (string)Session["CariMail"];
            var gelensayisi = c.mesajlars.Count(x => x.Alıcı == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.mesajlars.Count(x => x.Göndetici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View(degerler);
        }


        [HttpGet]
        public ActionResult YeniMesaj()
        {
            var mail = (string)Session["CariMail"];
            var gelensayisi = c.mesajlars.Count(x => x.Alıcı == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.mesajlars.Count(x => x.Göndetici == mail).ToString();
            ViewBag.d2 = gidensayisi;

            return View();
        }

        [HttpPost]
        public ActionResult YeniMesaj(mesajlar m)
        {
            var mail = (string)Session["CariMail"];
            m.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            m.Göndetici = mail;
            c.mesajlars.Add(m);
            c.SaveChanges();
            return View();
        }

        public ActionResult KargoTakip(string p)
        {
            var k = from x in c.KargoDetays select x;
           
            k = k.Where(y => y.TakipKodu.Contains(p));
           
            return View(k.ToList());
        }

        public ActionResult CariKargoTakip(string id)
        {
            var degerler = c.KargoTakips.Where(x => x.TakipKodu == id).ToList();

            return View(degerler);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        public PartialViewResult Partial1()
        {
            var mail = (string)Session["CariMail"];
            var id = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.Cariid).FirstOrDefault();
            var caribul = c.Carilers.Find(id);
            return PartialView("Partial1", caribul);//KategoriGetir sayfasına döndür, kategori değişkenimle beraber yap
         
        }

        public PartialViewResult Partial2()
        {
            var veriler = c.mesajlars.Where(x => x.Göndetici == "admin").ToList();
            return PartialView(veriler);
        }

        public ActionResult CariBilgiGuncelle(Cariler cr)
        {
            var cari = c.Carilers.Find(cr.Cariid);
            cari.CariAd = cr.CariAd;// sağ taraftaki onun yeni değeri
            cari.CariSoyad = cr.CariSoyad;// sağ taraftaki onun yeni değeri
            cari.CariSifre = cr.CariSifre;// sağ taraftaki onun yeni değeri
            c.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
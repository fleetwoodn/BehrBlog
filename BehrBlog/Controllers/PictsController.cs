using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BehrBlog.Models;
using System.IO;


namespace BehrBlog.Controllers
{
    public class PictsController : Controller
    {
        private PostDbContext db = new PostDbContext();

        // GET: Picts
        public ActionResult Index(string searchString)
        {
            var picts = from q in db.Picts
                        select q;

            if (!String.IsNullOrEmpty(searchString))
            {
                picts = picts.Where(r => r.PostFK.Equals(searchString));
            }

            return View(db.Picts.ToList());
        }

        //// GET: Picts/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Picts picts = db.Picts.Find(id);
        //    if (picts == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(picts);
        //}

        // GET: Picts/Create
        public ActionResult Create(int? fkid)
        {
            if (fkid == null)
                {
                ViewBag.fkid = "no post attached, go back!!!";
                }

            ViewBag.fkid = fkid;

            return View();
        }

        // POST: Picts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PostFK,PicTitle,EditDate,PictPict")] Picts picts)
        {

            var file = Request.Files[0];
            var sessUnq = Session.SessionID.Substring(Session.SessionID.Length - 4);
            if (file != null && file.ContentLength > 0)
            {
                var fileName = sessUnq + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/UploadPictures/"), fileName);
                file.SaveAs(path);
                picts.PictPict = fileName;
            }

            if (ModelState.IsValid)
            {
                //picts.PostFK = ViewBag.aId;

                String aDate = DateTime.UtcNow.ToString("yyMMddHHmmss");
                picts.EditDate = aDate;

                db.Picts.Add(picts);
                db.SaveChanges();

                int fkid = picts.PostFK;

                return RedirectToAction("Edit","Main", new { id = fkid });
                //return RedirectToAction("Index");
            }

            return View(picts);
        }

        // GET: Picts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picts picts = db.Picts.Find(id);
            if (picts == null)
            {
                return HttpNotFound();
            }
            return View(picts);
        }

        // POST: Picts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PostFK,PicTitle,EditDate,PictPict")] Picts picts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(picts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(picts);
        }

        // GET: Picts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picts picts = db.Picts.Find(id);
            if (picts == null)
            {
                return HttpNotFound();
            }
            return View(picts);
        }

        // POST: Picts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picts picts = db.Picts.Find(id);

            string fullPath = Request.MapPath("~/Images/Cakes/" + picts.PictPict);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            db.Picts.Remove(picts);
            db.SaveChanges();



            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.Entity;

using System.Net;

using BehrBlog.Models;
using BehrBlog.ViewModels;

namespace BehrBlog.Controllers
{
    public class MainAuthController : Controller
    {
        private PostDbContext db = new PostDbContext();

        // GET: Posts
        public ActionResult Index(string searchString)
        {
            var posts = from q in db.Posts
                        select q;

            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(r => r.PostTitle.Contains(searchString)
                    || r.PostTags.Contains(searchString));

            }

            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Posts posts = db.Posts.Find(id);
            //if (posts == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(posts);

            Posts posts = db.Posts.Find(id);

            var qPict = db.Picts.Where(q => q.PostFK == id).ToList();

            var viewModel = new MainDetailViewModel
            {
                ID = posts.ID,
                PostTitle = posts.PostTitle,
                PostAuthor = posts.PostAuthor,
                PostTags = posts.PostTags,
                PostText = posts.PostText,
                TitlePic = posts.TitlePic,
                EditDate = posts.EditDate,

                Picts = qPict,

            };

            return View(viewModel);

        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PostTitle,PostAuthor,PostTags,PostText,TitlePic,EditDate")] Posts posts)
        {

            if (ModelState.IsValid)
            {
                String aDate = DateTime.UtcNow.ToString("yyMMddHHmmss");
                posts.EditDate = aDate;

                db.Posts.Add(posts);
                db.SaveChanges();

                //grab id for new record

                //Posts posts = db.Posts.

                //int aId = int.Parse(db.Posts
                //    .OrderByDescending(b => b.ID)
                //    .Select(c => c.ID)
                //    .First()
                //    .ToString());

                int aId = posts.ID;

                //return RedirectToAction("Index");
                return RedirectToAction("Edit", new { id = aId });
            }

            //nn to drive to edit action...fuck fuck fuck --- done done done motherfucker

            return View(posts);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }

            ViewBag.aId = id;

            return View(posts);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PostTitle,PostAuthor,PostTags,PostText,TitlePic,EditDate")] Posts posts)
        {
            if (ModelState.IsValid)
            {
                String aDate = DateTime.UtcNow.ToString("yyMMddHHmmss");
                posts.EditDate = aDate;

                db.Entry(posts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(posts);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posts posts = db.Posts.Find(id);
            if (posts == null)
            {
                return HttpNotFound();
            }
            return View(posts);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posts posts = db.Posts.Find(id);
            db.Posts.Remove(posts);

            //delete the pictures too (how??? fk == id

            //Picts picts = db.Picts.Where(j => j.PostFK == id);
            //db.Picts.Remove(picts);

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
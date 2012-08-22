using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using NerdDinner.Models;
using NerdDinner.Helpers;

namespace <%= model.NameSpace %>
{
    public partial class <%= model.Name %Controller : Controller
    {
     
        /// <summary>
        /// Get index of entities ( based on page ).
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>   
        public ActionResult Index(int? page)
        {
            var entities = <%= model.Name %>s.GetAll();
            return View(entities);
        }


        /// <summary>
        /// Create a new entity.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            <%= model.Name %> job = new <%= model.Name %>();
            return View(job);
        } 


        /// <summary>
        /// Create the entity and persist it.
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult Create(<%= model.Name %> job) 
        {
             JobRepository.Provider.Create(job);
             return RedirectToAction("Details", new { id = job.Id });
        }

        //
        // POST: /<%= model.Name %>/Create

        /*
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/

       
        /// <summary>
        /// Edit the entity referenced by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            JobRepository repo = JobRepository.Provider;
            <%= model.Name %> job = repo.Get(id);
            return View(job);
        }


        /// <summary>
        /// Edit the entity referenced by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            <%= model.Name %> job = null;
            
            try
            {
                job = JobRepository.Provider.Get(id);
                UpdateModel(job);
                collection["JobName"] = job.Title; 
                JobRepository.Provider.Update(job);
                return RedirectToAction("Details", new { id = job.Id });
            }
            catch
            {
                ModelState.AddModelError("Title", "Title is too short");
                return View(job);
            }
        }


        /// <summary>
        /// Show the details of the entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            JobRepository repo = JobRepository.Provider;
            <%= model.Name %> job = repo.Get(id);
            return View(job);
        }


        /// <summary>
        /// Delete the entity with the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(int id) {

            <%= model.Name %> job = JobRepository.Provider.Get(id);

            if (job == null)
                return View("NotFound");

            //if (!job.IsHostedBy(User.Identity.Name))
            //    return View("InvalidOwner");

            return View(job);
        }

        

        /// <summary>
        /// Delete the entity with confirmation.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="confirmButton"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult Delete(int id, string confirmButton) 
        {

            <%= model.Name %> job = JobRepository.Provider.Get(id);
            
            if (job == null)
                return View("NotFound");

            //if (!job.IsHostedBy(User.Identity.Name))
            //    return View("InvalidOwner");

            JobRepository.Provider.DeleteByEntity(job);           

            return View("Deleted");
        }    
    }
}

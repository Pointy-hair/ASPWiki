﻿using Microsoft.AspNetCore.Mvc;
using ASPWiki.Services;
using ASPWiki.Model;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ASPWiki.Controllers
{
    //DRY - @PARAMETIRIZE
    //MINIMIZE CONTROLLER LOGIC - UNIT TESTING - :)
    public class WikiController : Controller
    {
        private readonly IRouteGenerator routeGenerator;

        //TODO THIS WILL BE A REAL REPOSITORY_____ !!
        private readonly IWikiRepository wikiRepository;

        private readonly IWikiService wikiService;

        public WikiController(IRouteGenerator routeGenerator, IWikiRepository wikiRepository, IWikiService wikiService)
        {
            this.routeGenerator = routeGenerator;
            this.wikiRepository = wikiRepository;
            this.wikiService = wikiService;
        }

        [HttpGet("Wiki/New")]
        public IActionResult New()
        {
            string route = routeGenerator.GenerateRoute();
            return RedirectToAction("Add", new { title = route });
        }

        [HttpGet("Wiki/Add/{title?}")]
        public IActionResult Add(string title)
        {
            var wikiPage = new WikiPage(title);
            return View("Edit", wikiPage);
        }

        [HttpGet("Wiki/Edit/{*path}")]
        public IActionResult Edit(string path)
        {
            var paths = path?.Split('/');
            var wikiPage = wikiRepository.GetByPath(paths);

            if (wikiPage == null)
                return RedirectToAction("Add", new { title = paths?[paths.Length - 1] }); 

            return View("Edit", wikiPage);
        }

        [HttpGet("Wiki/View/{*path}")]
        public IActionResult ViewPage(string path, string version)
        {
            var paths = path?.Split('/');
            var wikiPage = wikiRepository.GetByPath(paths);

            int versionNum;
            if (version != null && int.TryParse(version, out versionNum))
                wikiPage.Content = wikiPage.ContentHistory[Convert.ToInt32(versionNum)];

            if (wikiPage == null)
                return RedirectToAction("NotFound", "Wiki", new { title = paths?[paths.Length - 1] });

            return View("View", wikiPage);
        }

        [HttpPost("Wiki/Save")]
        [ValidateAntiForgeryToken]
        public IActionResult Save(WikiPage wikiPage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    wikiService.Save(wikiPage);
                    this.FlashMessageSuccess("Wikipage: " + wikiPage.Title + " succesfully saved");
                    return Redirect("/Wiki/View/" + wikiPage.GetPathString());
                }
                catch (Exception e)
                {
                    this.FlashMessageError(e.Message);
                    return View("Edit", wikiPage);
                }
            }
            else
            {
                this.FlashMessageError("Model invalid: " + this.GetModelStateErrors());
                return View("Edit", wikiPage);
            }
        }

        [HttpGet("Wiki/Delete/{*path}")]
        public IActionResult Delete(string path)
        {
            var paths = path?.Split('/');

            wikiRepository.Delete(paths);

            this.FlashMessageError("Wikipage: " + paths?[paths.Length - 1] + 
                " deleted: <a style='float:right;' href='/Wiki/Revert/" + path + 
                "'><b><i class='fa fa-reply' aria-hidden='true'></i>UNDO </b></a>");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Wiki/Revert/{*path}")]
        public IActionResult Revert(string path)
        {
            var paths = path?.Split('/');

            if (wikiRepository.Recover(paths))
                this.FlashMessageSuccess("Wikipage: " + paths?[paths.Length - 1] + " recovered");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Wiki/NotFound/{title?}")]
        public IActionResult NotFound(string title)
        {
            return View("NotFound", title);
        }

        [HttpPost("Wiki/IsValidPath")]
        public IActionResult IsValidPath([FromBody]object ajaxData)
        {
            JObject data = ajaxData as JObject;

            var path = data.Value<string>("Path");
            var Id = data.Value<string>("Id");

            string[] pathValue = path.Split('/');

            try
            {
                wikiService.IsValidPath(pathValue, new Guid(Id));
                return new OkObjectResult(JsonConvert.SerializeObject(pathValue));
            }
            catch (Exception e)
            {
                return new OkObjectResult(JsonConvert.SerializeObject(e.Message));
            }   
        }
    }
}

using AutoMapper;
using Jba.UI.Models;
using Jba.UI.ViewModels;
using JbaCodeAssessment.Data;
using JbaCodeAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Jba.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileManager _fileManager;
        private readonly JbaContext _jbaContext;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IFileManager fileManager, JbaContext jbaContext, IMapper mapper)
        {
            _logger = logger;
            _fileManager = fileManager;
            _jbaContext = jbaContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(FileImportViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var stream = model.PercipitationFile.OpenReadStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        await _fileManager.ProcessAsync(sr, JbaCodeAssessment.Models.FileTypes.Pre);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ExceptionError", ex.Message);
            }

            return RedirectToAction("results", new { skip = 0, take = 1000 });

        }

        public IActionResult Results(int skip, int take)
        {
            var reportData = _jbaContext.ReportData.ToList().Skip(skip).Take(take);
            var results = _mapper.Map<List<PercipitationDataViewModel>>(reportData);
            return View(results);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

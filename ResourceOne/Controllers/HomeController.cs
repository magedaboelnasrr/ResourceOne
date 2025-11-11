using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ResourceOne.Helper;
using ResourceOne.Models;
using ResourceOne.Services;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Security.Claims;

namespace ResourceOne.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        private readonly EmailServiceee _emailServicee;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this._context = context;
        }

        public IActionResult Index()
        {
            var query = _context.Blogs.Where(c => c.Category == "News")
                .Include(b => b.Comments)
                .OrderByDescending(b => b.CreatedDate).Take(4);

            int totalItems = query.Count();
            var blogs = query

                .Select(b => new BlogViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Body = b.Body,
                    Category = b.Category,
                    ImageName = b.ImageName,
                    CreatedDate = b.CreatedDate,
                    Publisher = b.Publisher,
                    Comments = b.Comments.Select(c => new CommentViewModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt
                    }).ToList()
                }).ToList();
            return View(blogs);

        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Service()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult TermsOfUse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewsLetter(NewsLetter newsLetter)
        {
            var data = await _context.NewsLetters.AddAsync(newsLetter);
            await _context.SaveChangesAsync();
            TempData["NewsLetter"] = "Thank you for subscription";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult CreateBlogAndSendEmailToSubscribers()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBlogAndSendEmailToSubscribers(BlogViewModel post, [FromServices] IEmailServiceee emailServicee)
        {
            if (ModelState.IsValid)
            {
                var blog = new Blog()
                {
                    Body = post.Body,
                    Category = post.Category,

                    CreatedDate = DateTime.Now,
                    Id = post.Id,
                    Title = post.Title,
                    Publisher = post.Publisher,
                    ImageName = DocumentSettings.UploadFile(post.Image, "Images")
                };

                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync();

                // ✨ هنا الشرط
                if (blog.Category == "News")
                {
                    var subscribers = await _context.NewsLetters
                                                    .Select(n => n.Email)
                                                    .ToListAsync();

                    foreach (var email in subscribers)
                    {
                        await emailServicee.SendEmailAsync(
                            email,
                            "📢 New Post Published",
                            $"<h2>{blog.Title}</h2><p>{blog.Body}</p><a href='https://site.resourcesone.com/{blog.Id}'></a>"
                        );
                    }
                }

                return RedirectToAction(nameof(InsightsHub));
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload == null || upload.Length == 0)
                return BadRequest(new { error = new { message = "No file uploaded" } });

            // اسم الصورة الفريد
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(upload.FileName);

            // مسار الحفظ
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blogImages");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await upload.CopyToAsync(stream);
            }

            // CKEditor عايز JSON بشكل محدد
            return Json(new
            {
                uploaded = true,
                url = "/uploads/blogImages/" + fileName
            });
        }



        [HttpPost]
        public async Task<IActionResult> Send(ContactFormModel model, [FromServices] IEmailService emailService, string Services)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "البيانات غير صحيحة" });
            }

            try
            {
                string emailBody = $@"
                     <h2>رسالة جديدة من الموقع</h2>
                     <p><b>الاسم:</b> {model.Name}</p>
                     <p><b>البريد:</b> {model.Email}</p>
                     <p><b>الموبايل:</b> {model.Mobile}</p>
                     <p><b>الخدمه:</b><br>{Services}</p>
                     <p><b>الموضوع:</b> {model.Subject}</p>
                     <p><b>الرسالة:</b><br>{model.Message}</p>";


                await emailService.SendEmailAsync("test@site.resourcesone.com", model.Subject, emailBody);

                return Json(new { success = true, message = "Message Sent Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Invalid server connection: " + ex.Message });
            }
        }
        [HttpGet]
        public IActionResult SuccessStories()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            Blog blog = _context.Blogs.Include(b => b.Comments).FirstOrDefault(b => b.Id == id);
            var blogViewModel = new BlogViewModel()
            {
                Title = blog.Title,
                ImageName = blog.ImageName,
                Body = blog.Body,
                Category = blog.Category,
                CreatedDate = blog.CreatedDate,
                Id = blog.Id,
                Publisher = blog.Publisher,
                Comments = blog.Comments.Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.UserName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).ToList()
            };

            if (blog == null)
            {
                return NotFound();
            }
            return View(blogViewModel);
        }

        [HttpGet]
        public IActionResult InsightsHub(int newsPageNumber = 1, int blogsPageNumber = 1, int pageSize = 8)
        {
            // News Query
            var newsQuery = _context.Blogs
                .Where(c => c.Category == "News")
                .Include(b => b.Comments)
                .OrderByDescending(b => b.CreatedDate);

            int totalNewsItems = newsQuery.Count();

            var news = newsQuery
                .Skip((newsPageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BlogViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Body = b.Body,
                    Category = b.Category,
                    ImageName = b.ImageName,
                    CreatedDate = b.CreatedDate,
                    Publisher = b.Publisher,
                    Comments = b.Comments.Select(c => new CommentViewModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        UserName = c.UserName
                    }).ToList()
                }).ToList();

            // Blogs Query
            var blogsQuery = _context.Blogs
                .Where(c => c.Category == "Blogs")
                .Include(b => b.Comments)
                .OrderByDescending(b => b.CreatedDate);

            int totalBlogsItems = blogsQuery.Count();

            var blogs = blogsQuery
                .Skip((blogsPageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BlogViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Body = b.Body,
                    Category = b.Category,
                    ImageName = b.ImageName,
                    CreatedDate = b.CreatedDate,
                    Publisher = b.Publisher,
                    Comments = b.Comments.Select(c => new CommentViewModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        UserName = c.UserName
                    }).ToList()
                }).ToList();

            // ViewModel
            var model = new InsightsHubPageViewModel
            {
                News = news,
                Blogs = blogs,
                NewsPageNumber = newsPageNumber,
                NewsTotalPages = (int)Math.Ceiling(totalNewsItems / (double)pageSize),
                BlogsPageNumber = blogsPageNumber,
                BlogsTotalPages = (int)Math.Ceiling(totalBlogsItems / (double)pageSize)
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult AddComment(int blogId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return RedirectToAction("Details", new { id = blogId });

            var comment = new Comment
            {
                BlogId = blogId,
                Content = content,
                CreatedAt = DateTime.Now,
                UserName = User.Identity.Name ?? "Guest",
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = blogId });
        }

        [HttpGet]
        public IActionResult UpdateInsightsHub(BlogViewModel blogViewModel)
        {
            var data = _context.Blogs.Find(blogViewModel.Id);
            blogViewModel.Body = data.Body;
            blogViewModel.Category = data.Category;
            blogViewModel.CreatedDate = data.CreatedDate;

            blogViewModel.Title = data.Title;
            blogViewModel.Id = data.Id;
            blogViewModel.ImageName = data.ImageName;
            blogViewModel.Publisher = data.Publisher;
            return View(blogViewModel);
        }
        [HttpPost]
        public IActionResult UpdateInsightsHub(int id, BlogViewModel blogViewModel)
        {
            if (ModelState.IsValid)
            {
                var blog = _context.Blogs.Find(id);
                if (blog == null) return NotFound();


                blog.Body = blogViewModel.Body;
                blog.Category = blogViewModel.Category;

                blog.CreatedDate = blogViewModel.CreatedDate;
                blog.Title = blogViewModel.Title;
                blog.Publisher = blogViewModel.Publisher;

                if (blogViewModel.Image != null)
                {
                    blog.ImageName = DocumentSettings.UploadFile(blogViewModel.Image, "Images");
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(InsightsHub));
            }
            return View(blogViewModel);
        }
        [HttpGet]
        public IActionResult DeleteInsightsHub(BlogViewModel blogViewModel)
        {
            var data = _context.Blogs.Find(blogViewModel.Id);
            blogViewModel.Body = data.Body;
            blogViewModel.Category = data.Category;
            blogViewModel.CreatedDate = data.CreatedDate;
            blogViewModel.Publisher = data.Publisher;
            blogViewModel.Title = data.Title;
            blogViewModel.Id = data.Id;
            return View(blogViewModel);
        }
        [HttpPost]
        public IActionResult DeleteInsightsHub(int id, BlogViewModel blogViewModel)
        {

            var blog = _context.Blogs.Find(id); 
            if (blog == null) return NotFound();

            _context.Blogs.Remove(blog);

            _context.SaveChanges();
            return RedirectToAction(nameof(InsightsHub));
        }
        [HttpGet]
        public IActionResult Careers()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Careers(ApplicationFormViewModel applicationFormViewModel)
        {
            if (ModelState.IsValid == true)
            {

                var applicationForm = new ApplicationForm()
                {
                    Id = applicationFormViewModel.Id,
                    Address = applicationFormViewModel.Address,
                    Age = applicationFormViewModel.Age,
                    CurrentPosition = applicationFormViewModel.CurrentPosition,
                    Email = applicationFormViewModel.Email,
                    FullName = applicationFormViewModel.FullName,
                    PhoneNumber = applicationFormViewModel.PhoneNumber,
                    YearsOfExperience = applicationFormViewModel.YearsOfExperience,
                    CVName = DocumentSettings.UploadFile(applicationFormViewModel.CV, "Files")
                };

                await _context.ApplicationForms.AddAsync(applicationForm);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Form has been submitted successfully";
                return RedirectToAction(nameof(Careers));
            }
            return View(applicationFormViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

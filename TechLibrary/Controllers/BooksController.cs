using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TechLibrary.Contracts.Responses;
using TechLibrary.Domain;
using TechLibrary.Services;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        private readonly IConfiguration _config;
        private int _itemsPerPage => _config.GetValue<int>("ItemsPerPage");

        public BooksController(ILogger<BooksController> logger, IBookService bookService, IMapper mapper, IConfiguration config)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet("{page:int?}")]
        public async Task<IActionResult> GetAll(int? page)
        {
            _logger.LogInformation($"Get all books, page {page}");

            int p = page ?? 1;

            var books = await _bookService.GetBooksAsync(p, _itemsPerPage);

            //var bookResponse = _mapper.Map<List<BookResponse>>(books);

            var bookResponse = _mapper.Map<PageList<Book>, PagedBookResponse>(books);

            return Ok(bookResponse);
        }

        [HttpGet("book/{id:int:min(1)}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Get book by id {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }

        [HttpGet("search/{query:minlength(2)}/{page:int?}")]
        public async Task<IActionResult> SearchBooks(string query, int? page)
        {
            _logger.LogInformation($"Book search query {query} page {page}");

            int p = page ?? 1;

            var books = await _bookService.SearchBooks(query, p, _itemsPerPage);

            var bookResponse = _mapper.Map<PageList<Book>, PagedBookResponse>(books);

            return Ok(bookResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditBook(int? id, BookResponse req)
        {
            if (id == null)
                return BadRequest();

            if (id != req.BookId)
                return BadRequest();
            
            try
            {
                var book = _mapper.Map<Book>(req);
                await _bookService.UpdateBook(book);
                return Accepted();

            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e.Message, e);
                return ValidationProblem();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return BadRequest();
            }


        }
    }
}

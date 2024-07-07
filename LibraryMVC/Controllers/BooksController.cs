using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryMVC.Data;
using LibraryMVC.Models;
using Microsoft.Data.SqlClient;
using System.Xml;

namespace LibraryMVC.Controllers;

public class BooksController(LibraryContext context) : Controller
{
    // POST: Books/Create
    [HttpPost]
    public IActionResult EditTableContents(int id, string tableContents)
    {
        context.Database.ExecuteSqlRaw("UpdateTableContents @id, @table_contents",
                new SqlParameter("@id", id),
                new SqlParameter("@table_contents", tableContents));

        return RedirectToAction(nameof(Index));
    }

    // GET: Books
    public async Task<IActionResult> Index()
    {
        var listBookView = new List<BookView>();
        foreach (var item in await context.Books.FromSqlRaw("SelectedBooks").ToListAsync())
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(item.TableContents);
            XmlElement? xRoot = xmlDocument.DocumentElement;
            var tableContents = new List<string>();
            //Выборка данных из xml
            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    tableContents.Add(xnode.InnerXml);
                }
            }

            listBookView.Add(new BookView
            { 
                Id = item.Id,
                Author = item.Author,
                Title = item.Title,
                Description = item.Description,
                TableContents = tableContents,
                Year = item.Year,
                TableContentsXML = xmlDocument,
            });
        }

        return View(listBookView);
    }

    // GET: Books/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var books = await context.Books.FromSqlRaw("SelectedBookById @id",
                 new SqlParameter("@id", id)).ToListAsync();

        if (books.Count != 0)
        {
            return View(books.First());
        }

        return NotFound();
    }

    // GET: Books/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Books/Create

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Id, Author, Year, Title, Description, TableContents")] Book book)
    {
        if (ModelState.IsValid)
        {
            context.Database.ExecuteSqlRaw("AddBook @author, @year, @title, @description, @table_contents",
                new SqlParameter("@author", book.Author),
                new SqlParameter("@year", book.Year),
                new SqlParameter("@title", book.Title),
                new SqlParameter("@description", book.Description),
                new SqlParameter("@table_contents", book.TableContents));

            return RedirectToAction(nameof(Index));
        }

        return View(book);
    }


    // GET: Books/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var books = await context.Books.FromSqlRaw("SelectedBookById @id",
                  new SqlParameter("@id", id)).ToListAsync();

        if (books.Count != 0)
        {
            return View(books.First());
        }

        return NotFound();
    }


    // POST: Books/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Author,Title,Year,Description,TableContents")] Book book)
    {
        if (id != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Database.ExecuteSqlRaw("UpdateBook @id, @author, @year, @title, @description, @table_contents",
                    new SqlParameter("@id", book.Id),
                    new SqlParameter("@author", book.Author),
                    new SqlParameter("@year", book.Year),
                    new SqlParameter("@title", book.Title),
                    new SqlParameter("@description", book.Description),
                    new SqlParameter("@table_contents", book.TableContents));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        return View(book);
    }

    // GET: Books/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var books = await context.Books.FromSqlRaw("SelectedBookById @id",
                  new SqlParameter("@id", id)).ToListAsync();

        if (books.Count != 0)
        {
            return View(books.First());
        }

        return NotFound();
    }

    // POST: Books/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        context.Database.ExecuteSqlRaw("DeleteBook @id",
                new SqlParameter("@id", id));

        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return context.Books.Any(e => e.Id == id);
    }
}
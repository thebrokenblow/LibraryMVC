using LibraryMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryMVC.Data;

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
}